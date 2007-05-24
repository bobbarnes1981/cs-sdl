#region LICENSE
//
// MpqHuffman.cs
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
using System.IO;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using ICSharpCode.SharpZipLib.BZip2;

using SCSharp.UI;

namespace SCSharp.MpqLib
{
    /// <summary>
    /// A Stream based class for reading a file from an MPQ file
    /// </summary>
    public class MpqStream : Stream
    {
        private Stream mStream;
        private int mBlockSize;

        private MpqBlock mBlock;
        private uint[] mBlockPositions;
        private uint mSeed1;

        private long mPosition;
        private byte[] mCurrentData;
        private int mCurrentBlockIndex = -1;

        private MpqStream()
        { }

        internal MpqStream(MpqArchive File, MpqBlock Block)
        {
            mBlock = Block;

            mStream = File.BaseStream;
            mBlockSize = File.BlockSize;

            if (mBlock.IsCompressed)
            {
                LoadBlockPositions();
            }
        }

        // Compressed files start with an array of offsets to make seeking possible
        private void LoadBlockPositions()
        {
            int blockposcount = (int)((mBlock.FileSize + mBlockSize - 1) / mBlockSize) + 1;

            mBlockPositions = new uint[blockposcount];

            lock (mStream)
            {
                mStream.Seek(mBlock.FilePos, SeekOrigin.Begin);
                BinaryReader br = new BinaryReader(mStream);
                for (int i = 0; i < blockposcount; i++)
                {
                    mBlockPositions[i] = br.ReadUInt32();
                }
            }

            uint blockpossize = (uint)blockposcount * 4;

            // StormLib takes this to mean the data is encrypted
            if (mBlockPositions[0] != blockpossize)
            {
                if (mSeed1 == 0)
                {
                    mSeed1 = MpqArchive.DetectFileSeed(mBlockPositions, blockpossize);
                    if (mSeed1 == 0)
                    {
                        throw new SCException("Unable to determine encryption seed");
                    }
                }
                MpqArchive.DecryptBlock(mBlockPositions, mSeed1);
                mSeed1++; // Add 1 because the first block is the offset list
            }
        }

        private byte[] LoadBlock(int blockIndex, int expectedLength)
        {
            uint offset;
            int toread;

            if (mBlock.IsCompressed)
            {
                offset = mBlockPositions[blockIndex];
                toread = (int)(mBlockPositions[blockIndex + 1] - offset);
            }
            else
            {
                offset = (uint)(blockIndex * mBlockSize);
                toread = expectedLength;
            }
            offset += mBlock.FilePos;

            byte[] data = new byte[toread];
            lock (mStream)
            {
                mStream.Seek(offset, SeekOrigin.Begin);
                mStream.Read(data, 0, toread);
            }

            if (mBlock.IsEncrypted && mBlock.FileSize > 3)
            {
                if (mSeed1 == 0)
                {
                    throw new SCException("Unable to determine encryption key");
                }
                MpqArchive.DecryptBlock(data, (uint)(mSeed1 + blockIndex));
            }

            if (mBlock.IsCompressed && data.Length != expectedLength)
            {
                if ((mBlock.Flags & MpqFileFlags.CompressedMulti) != 0)
                {
                    data = DecompressMulti(data, expectedLength);
                }
                else
                {
                    data = PKDecompress(new MemoryStream(data), expectedLength);
                }
            }

            return data;
        }

        #region Stream overrides
        /// <summary>
        /// 
        /// </summary>
        public override bool CanRead
        { get { return true; } }

        /// <summary>
        /// 
        /// </summary>
        public override bool CanSeek
        { get { return true; } }

        /// <summary>
        /// 
        /// </summary>
        public override bool CanWrite
        { get { return false; } }

        /// <summary>
        /// 
        /// </summary>
        public override long Length
        { get { return mBlock.FileSize; } }

        /// <summary>
        /// 
        /// </summary>
        public override long Position
        {
            get
            {
                return mPosition;
            }
            set
            {
                Seek(value, SeekOrigin.Begin);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Flush()
        {
            // NOP
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="origin"></param>
        /// <returns></returns>
        public override long Seek(long offset, SeekOrigin origin)
        {
            long target;

            switch (origin)
            {
                case SeekOrigin.Begin:
                    target = offset;
                    break;
                case SeekOrigin.Current:
                    target = Position + offset;
                    break;
                case SeekOrigin.End:
                    target = Length + offset;
                    break;
                default:
                    throw new ArgumentNullException("origin", "Invalid SeekOrigin");
            }

            if (target < 0)
            {
                throw new ArgumentOutOfRangeException("offset", "Attmpted to Seek before the beginning of the stream");
            }
            if (target >= Length)
            {
                throw new ArgumentOutOfRangeException("offset", "Attmpted to Seek beyond the end of the stream");
            }

            mPosition = target;

            return mPosition;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public override void SetLength(long value)
        {
            throw new NotSupportedException("SetLength is not supported");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public override int Read(byte[] buffer, int offset, int count)
        {
            int toread = count;
            int readtotal = 0;

            while (toread > 0)
            {
                int read = ReadInternal(buffer, offset, toread);
                if (read == 0) break;
                readtotal += read;
                offset += read;
                toread -= read;
            }
            return readtotal;
        }

        private int ReadInternal(byte[] buffer, int offset, int count)
        {
            BufferData();

            int localposition = (int)(mPosition % mBlockSize);
            int bytestocopy = Math.Min(mCurrentData.Length - localposition, count);
            if (bytestocopy <= 0) return 0;

            Array.Copy(mCurrentData, localposition, buffer, offset, bytestocopy);

            mPosition += bytestocopy;
            return bytestocopy;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int ReadByte()
        {
            if (mPosition >= Length) return -1;

            BufferData();

            int localposition = (int)(mPosition % mBlockSize);
            mPosition++;
            return mCurrentData[localposition];
        }

        private void BufferData()
        {
            int requiredblock = (int)(mPosition / mBlockSize);
            if (requiredblock != mCurrentBlockIndex)
            {
                int expectedlength = (int)Math.Min(Length - (requiredblock * mBlockSize), mBlockSize);
                mCurrentData = LoadBlock(requiredblock, expectedlength);
                mCurrentBlockIndex = requiredblock;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException("Writing is not supported");
        }
        #endregion Strem overrides

        /* Compression types in order:
		 *  10 = BZip2
		 *   8 = PKLib
		 *   2 = ZLib
		 *   1 = Huffman
		 *  80 = IMA ADPCM Stereo
		 *  40 = IMA ADPCM Mono
		 */
        private static byte[] DecompressMulti(byte[] input, int outputLength)
        {
            Stream sinput = new MemoryStream(input);

            byte comptype = (byte)sinput.ReadByte();

            // BZip2
            if ((comptype & 0x10) != 0)
            {
                byte[] result = BZip2Decompress(sinput);
                comptype &= 0xEF;
                if (comptype == 0)
                {
                    return result;
                }
                sinput = new MemoryStream(result);
            }

            // PKLib
            if ((comptype & 8) != 0)
            {
                byte[] result = PKDecompress(sinput, outputLength);
                comptype &= 0xF7;
                if (comptype == 0)
                {
                    return result;
                }
                sinput = new MemoryStream(result);
            }

            // ZLib
            if ((comptype & 2) != 0)
            {
                byte[] result = ZlibDecompress(sinput, outputLength);
                comptype &= 0xFD;
                if (comptype == 0)
                {
                    return result;
                }
                sinput = new MemoryStream(result);
            }

            if ((comptype & 1) != 0)
            {
                byte[] result = MpqHuffman.Decompress(sinput);
                comptype &= 0xfe;
                if (comptype == 0)
                {
                    return result;
                }
                sinput = new MemoryStream(result);
            }

            if ((comptype & 0x80) != 0)
            {
                byte[] result = MpqWavCompression.Decompress(sinput, 2);
                comptype &= 0x7f;
                if (comptype == 0)
                {
                    return result;
                }
                sinput = new MemoryStream(result);
            }

            if ((comptype & 0x40) != 0)
            {
                byte[] result = MpqWavCompression.Decompress(sinput, 1);
                comptype &= 0xbf;
                if (comptype == 0)
                {
                    return result;
                }
                sinput = new MemoryStream(result);
            }
            throw new SCException(String.Format("Unhandled compression flags: 0x{0:X}", comptype));
        }

        //private static byte[] BZip2Decompress(Stream data, int expectedLength)
        
        private static byte[] BZip2Decompress(Stream data)
        {
            MemoryStream output = new MemoryStream();
            BZip2.Decompress(data, output);
            return output.ToArray();
        }

        private static byte[] PKDecompress(Stream data, int expectedLength)
        {
            PKLibDecompress pk = new PKLibDecompress(data);
            return pk.Explode(expectedLength);
        }

        private static byte[] ZlibDecompress(Stream data, int expectedLength)
        {
            // This assumes that Zlib won't be used in combination with another compression type
            byte[] Output = new byte[expectedLength];
            Stream s = new InflaterInputStream(data);
            int Offset = 0;
            while (true)
            {
                int size = s.Read(Output, Offset, expectedLength);
                if (size == 0)
                {
                    break;
                }
                Offset += size;
            }
            return Output;
        }
    }
}
