#region LICENSE
//
// MpqArchive.cs
//
// Authors:
//		Foole (fooleau@gmail.com)
//
// (C) 2006 Foole (fooleau@gmail.com)
// Based on code from StormLib by Ladislav Zezula
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
#endregion LICENSE

using System;
using System.Collections;
using System.IO;
using System.Globalization;

using SCSharp.UI;

namespace SCSharp.MpqLib
{
    /// <summary>
    /// 
    /// </summary>
    public class MpqArchive : Mpq
    {
        private Stream mStream;
        //private string mFilename;
        private MpqHeader mHeader;
        private long mHeaderOffset;
        private int mBlockSize;
        private MpqHash[] mHashes;
        private MpqBlock[] mBlocks;

        private static uint[] sStormBuffer = BuildStormBuffer();

        //static MpqArchive()
        //{
        //    sStormBuffer = BuildStormBuffer();
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        public MpqArchive(string fileName)
        {
            //mFilename = filename;
            mStream = File.Open(fileName, FileMode.Open, FileAccess.Read);
            Init();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceStream"></param>
        public MpqArchive(Stream sourceStream)
        {
            mStream = sourceStream;
            Init();
        }

        private void Init()
        {
            if (LocateMpqHeader() == false)
            {
                throw new SCException("Unable to find MPQ header");
            }

            BinaryReader br = new BinaryReader(mStream);

            mBlockSize = 0x200 << mHeader.BlockSize;

            // Load hash table
            mStream.Seek(mHeader.HashTablePos, SeekOrigin.Begin);
            byte[] hashdata = br.ReadBytes((int)(mHeader.HashTableSize * MpqHash.Size));
            DecryptTable(hashdata, "(hash table)");

            BinaryReader br2 = new BinaryReader(new MemoryStream(hashdata));
            mHashes = new MpqHash[mHeader.HashTableSize];

            for (int i = 0; i < mHeader.HashTableSize; i++)
            {
                mHashes[i] = new MpqHash(br2);
            }

            // Load block table
            mStream.Seek(mHeader.BlockTablePos, SeekOrigin.Begin);
            byte[] blockdata = br.ReadBytes((int)(mHeader.BlockTableSize * MpqBlock.Size));
            DecryptTable(blockdata, "(block table)");

            br2 = new BinaryReader(new MemoryStream(blockdata));
            mBlocks = new MpqBlock[mHeader.BlockTableSize];

            for (int i = 0; i < mHeader.BlockTableSize; i++)
            {
                mBlocks[i] = new MpqBlock(br2, (uint)mHeaderOffset);
            }
        }

        private bool LocateMpqHeader()
        {
            BinaryReader br = new BinaryReader(mStream);

            // In .mpq files the header will be at the start of the file
            // In .exe files, it will be at a multiple of 0x200
            for (long i = 0; i < mStream.Length - MpqHeader.Size; i += 0x200)
            {
                mStream.Seek(i, SeekOrigin.Begin);
                mHeader = new MpqHeader(br);

                if (mHeader.ID == MpqHeader.MpqId)
                {
                    mHeaderOffset = i;
                    mHeader.HashTablePos += (uint)mHeaderOffset;
                    mHeader.BlockTablePos += (uint)mHeaderOffset;
                    if (mHeader.DataOffset == 0x6d9e4b86)
                    {
                        // then this is a protected archive
                        mHeader.DataOffset = (uint)(MpqHeader.Size + i);
                    }
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public MpqStream OpenFile(string fileName)
        {
            MpqHash hash;
            MpqBlock block;

            hash = GetHashEntry(fileName);
            uint blockindex = hash.BlockIndex;

            if (blockindex == uint.MaxValue)
            {
                throw new FileNotFoundException("File not found: " + fileName);
            }

            block = mBlocks[blockindex];

            return new MpqStream(this, block);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public bool FileExists(string fileName)
        {
            MpqHash hash = GetHashEntry(fileName);
            return (hash.BlockIndex != uint.MaxValue);
        }

        internal Stream BaseStream
        { get { return mStream; } }

        internal int BlockSize
        { get { return mBlockSize; } }

        private MpqHash GetHashEntry(string filename)
        {
            uint index = HashString(filename, 0);
            index &= mHeader.HashTableSize - 1;
            uint name1 = HashString(filename, 0x100);
            uint name2 = HashString(filename, 0x200);

            for (uint i = index; i < mHashes.Length; ++i)
            {
                MpqHash hash = mHashes[i];
                if (hash.Name1 == name1 && hash.Name2 == name2)
                {
                    return hash;
                }
            }

            MpqHash nullhash = new MpqHash();
            nullhash.BlockIndex = uint.MaxValue;
            return nullhash;
        }

        internal static uint HashString(string input, int offset)
        {
            uint seed1 = 0x7fed7fed;
            uint seed2 = 0xeeeeeeee;

            foreach (char c in input)
            {
                int val = (int)char.ToUpper(c, CultureInfo.CurrentCulture);
                seed1 = sStormBuffer[offset + val] ^ (seed1 + seed2);
                seed2 = (uint)val + seed1 + seed2 + (seed2 << 5) + 3;
            }
            return seed1;
        }

        // Used for Hash Tables and Block Tables
        internal static void DecryptTable(byte[] data, string key)
        {
            DecryptBlock(data, HashString(key, 0x300));
        }

        internal static void DecryptBlock(byte[] data, uint seed1)
        {
            uint seed2 = 0xeeeeeeee;

            // NB: If the block is not an even multiple of 4,
            // the remainder is not encrypted
            for (int i = 0; i < data.Length - 3; i += 4)
            {
                seed2 += sStormBuffer[0x400 + (seed1 & 0xff)];

                uint result = BitConverter.ToUInt32(data, i);

                result ^= (seed1 + seed2);

                seed1 = ((~seed1 << 21) + 0x11111111) | (seed1 >> 11);
                seed2 = result + seed2 + (seed2 << 5) + 3;
                byte[] bytes = BitConverter.GetBytes(result);
                Array.Copy(bytes, 0, data, i, 4);
            }
        }

        internal static void DecryptBlock(uint[] data, uint seed1)
        {
            uint seed2 = 0xeeeeeeee;

            for (int i = 0; i < data.Length; i++)
            {
                seed2 += sStormBuffer[0x400 + (seed1 & 0xff)];
                uint result = data[i];
                result ^= seed1 + seed2;

                seed1 = ((~seed1 << 21) + 0x11111111) | (seed1 >> 11);
                seed2 = result + seed2 + (seed2 << 5) + 3;
                data[i] = result;
            }
        }

        // This function calculates the encryption key based on
        // some assumptions we can make about the headers for encrypted files
        internal static uint DetectFileSeed(uint[] data, uint decrypted)
        {
            uint value0 = data[0];
            uint value1 = data[1];
            uint temp = (value0 ^ decrypted) - 0xeeeeeeee;

            for (int i = 0; i < 0x100; i++)
            {
                uint seed1 = temp - sStormBuffer[0x400 + i];
                uint seed2 = 0xeeeeeeee + sStormBuffer[0x400 + (seed1 & 0xff)];
                uint result = value0 ^ (seed1 + seed2);

                if (result != decrypted)
                {
                    continue;
                }

                uint saveseed1 = seed1;

                // Test this result against the 2nd value
                seed1 = ((~seed1 << 21) + 0x11111111) | (seed1 >> 11);
                seed2 = result + seed2 + (seed2 << 5) + 3;

                seed2 += sStormBuffer[0x400 + (seed1 & 0xff)];
                result = value1 ^ (seed1 + seed2);

                if ((result & 0xffff0000) == 0)
                {
                    return saveseed1;
                }
            }
            return 0;
        }

        private static uint[] BuildStormBuffer()
        {
            uint seed = 0x100001;

            uint[] result = new uint[0x500];

            for (uint index1 = 0; index1 < 0x100; index1++)
            {
                uint index2 = index1;
                for (int i = 0; i < 5; i++, index2 += 0x100)
                {
                    seed = (seed * 125 + 3) % 0x2aaaab;
                    uint temp = (seed & 0xffff) << 16;
                    seed = (seed * 125 + 3) % 0x2aaaab;

                    result[index2] = temp | (seed & 0xffff);
                }
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public override Stream GetStreamForResource(string path)
        {
            throw new SCException("The method or operation is not implemented.");
        }
    }
}
