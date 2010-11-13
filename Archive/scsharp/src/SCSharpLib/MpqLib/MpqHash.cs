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
    struct MpqHash
    {
        public uint Name1;
        public uint Name2;
        public uint Locale;
        public uint BlockIndex;

        public const uint Size = 16;

        public MpqHash(BinaryReader br)
        {
            Name1 = br.ReadUInt32();
            Name2 = br.ReadUInt32();
            Locale = br.ReadUInt32();
            BlockIndex = br.ReadUInt32();
        }

        //public bool IsValid
        //{
        //    get
        //    {
        //        return Name1 != uint.MaxValue && Name2 != uint.MaxValue;
        //    }
        //}
    }
}
