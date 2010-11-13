#region LICENSE
//
// MpqStructs.cs
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

using System.IO;

namespace SCSharp.MpqLib
{
    enum MpqFileFlags : uint
    {
        Changed = 1,
        Protected = 2,
        CompressedPK = 0x100,
        CompressedMulti = 0x200,
        Compressed = 0xff00,
        Encrypted = 0x10000,
        FixSeed = 0x20000,
        SingleUnit = 0x1000000,
        Exists = 0x80000000
    }

    struct MpqBlock
    {
        public uint FilePos;
        public uint CompressedSize;
        public uint FileSize;
        public MpqFileFlags Flags;

        public const uint Size = 16;

        public MpqBlock(BinaryReader br, uint HeaderOffset)
        {
            FilePos = br.ReadUInt32() + HeaderOffset;
            CompressedSize = br.ReadUInt32();
            FileSize = br.ReadUInt32();
            Flags = (MpqFileFlags)br.ReadUInt32();
        }

        public bool IsEncrypted
        {
            get
            {
                return (Flags & MpqFileFlags.Encrypted) != 0;
            }
        }

        public bool IsCompressed
        {
            get
            {
                return (Flags & MpqFileFlags.Compressed) != 0;
            }
        }
    }
}
