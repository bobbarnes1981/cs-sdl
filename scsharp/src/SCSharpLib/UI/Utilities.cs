#region LICENSE
//
// Authors:
// Chris Toshok (toshok@hungry.com)
//
// (C) 2006 The Hungry Programmers (http://www.hungry.com/)
//

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
using System.Collections.Generic;
using System.Text;

namespace SCSharp.UI
{
    /// <summary>
    ///
    /// </summary>
    public class Utilities
    {
        /// <summary>
        /// read in a LE word
        /// </summary>
        /// <param name="fs"></param>
        /// <returns></returns>
        [CLSCompliant(false)]
        public static ushort ReadWord(Stream fs)
        {
            return ((ushort)(fs.ReadByte() | (fs.ReadByte() << 8)));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="buf"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        [CLSCompliant(false)]
        public static ushort ReadWord(byte[] buf, int position)
        {
            return ((ushort)((int)buf[position] | (int)buf[position + 1] << 8));
        }

        /// <summary>
        /// read in a LE doubleword
        /// </summary>
        /// <param name="fs"></param>
        /// <returns></returns>
        [CLSCompliant(false)]
        public static uint ReadDWord(Stream fs)
        {
            return (uint)(fs.ReadByte() | (fs.ReadByte() << 8) | (fs.ReadByte() << 16) | (fs.ReadByte() << 24));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="buf"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        [CLSCompliant(false)]
        public static uint ReadDWord(byte[] buf, int position)
        {
            return ((uint)((uint)buf[position] | (uint)buf[position + 1] << 8 | (uint)buf[position + 2] << 16 | (uint)buf[position + 3] << 24));
        }

        /// <summary>
        /// read in a byte
        /// </summary>
        /// <param name="fs"></param>
        /// <returns></returns>
        public static byte ReadByte(Stream fs)
        {
            return (byte)fs.ReadByte();
        }

        /// <summary>
        /// write a LE word
        /// </summary>
        /// <param name="fs"></param>
        /// <param name="word"></param>
        [CLSCompliant(false)]
        public static void WriteWord(Stream fs, ushort word)
        {
            fs.WriteByte((byte)(word & 0xff));
            fs.WriteByte((byte)((word >> 8) & 0xff));
        }

        /// <summary>
        /// write a LE doubleword
        /// </summary>
        /// <param name="fs"></param>
        /// <param name="dword"></param>
        [CLSCompliant(false)]
        public static void WriteDWord(Stream fs, uint dword)
        {
            fs.WriteByte((byte)(dword & 0xff));
            fs.WriteByte((byte)((dword >> 8) & 0xff));
            fs.WriteByte((byte)((dword >> 16) & 0xff));
            fs.WriteByte((byte)((dword >> 24) & 0xff));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        public static string ReadUntilNull(StreamReader r)
        {
            StringBuilder sb = new StringBuilder();

            char c;
            do
            {
                c = (char)r.Read();
                if (c != 0)
                    sb.Append(c);
            } while (c != 0);

            return sb.ToString();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="buf"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public static string ReadUntilNull(byte[] buf, int position)
        {
            StringBuilder sb = new StringBuilder();

            int i = position;

            while (buf[i] != 0)
            {
                i++;
            }

            byte[] bs = new byte[i - position];
            Array.Copy(buf, position, bs, 0, i - position);

            return Encoding.UTF8.GetString(bs);
        }

        /// <summary>
        ///
        /// </summary>
        public static char[] RaceChar = { 'P', 'T', 'Z' };
        /// <summary>
        ///
        /// </summary>
        public static char[] RaceCharLower = { 'p', 't', 'z' };

        /// <summary>
        ///
        /// </summary>
        public static string[] TilesetNames = {
"badlands",
"platform",
"install",
"ashworld",
"jungle",
"desert",
"ice",
"twilight"
};
    }
}

