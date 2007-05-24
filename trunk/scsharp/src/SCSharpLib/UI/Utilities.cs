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
    public static class Utilities
    {
        /// <summary>
        /// read in a LE word
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        [CLSCompliant(false)]
        public static ushort ReadWord(Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }
            return ((ushort)(stream.ReadByte() | (stream.ReadByte() << 8)));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        [CLSCompliant(false)]
        public static ushort ReadWord(byte[] buffer, int position)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException("buffer");
            }
            return ((ushort)((int)buffer[position] | (int)buffer[position + 1] << 8));
        }

        /// <summary>
        /// read in a LE doubleword
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        [CLSCompliant(false)]
        public static uint ReadDWord(Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }
            return (uint)(stream.ReadByte() | (stream.ReadByte() << 8) | (stream.ReadByte() << 16) | (stream.ReadByte() << 24));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        [CLSCompliant(false)]
        public static uint ReadDWord(byte[] buffer, int position)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException("buffer");
            }
            return ((uint)((uint)buffer[position] | (uint)buffer[position + 1] << 8 | (uint)buffer[position + 2] << 16 | (uint)buffer[position + 3] << 24));
        }

        /// <summary>
        /// read in a byte
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static byte ReadByte(Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }
            return (byte)stream.ReadByte();
        }

        /// <summary>
        /// write a LE word
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="word"></param>
        [CLSCompliant(false)]
        public static void WriteWord(Stream stream, ushort word)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }
            stream.WriteByte((byte)(word & 0xff));
            stream.WriteByte((byte)((word >> 8) & 0xff));
        }

        /// <summary>
        /// write a LE doubleword
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="word"></param>
        [CLSCompliant(false)]
        public static void WriteDWord(Stream stream, uint word)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }
            stream.WriteByte((byte)(word & 0xff));
            stream.WriteByte((byte)((word >> 8) & 0xff));
            stream.WriteByte((byte)((word >> 16) & 0xff));
            stream.WriteByte((byte)((word >> 24) & 0xff));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="textReader"></param>
        /// <returns></returns>
        public static string ReadUntilNull(TextReader textReader)
        {
            if (textReader == null)
            {
                throw new ArgumentNullException("textReader");
            }
            StringBuilder sb = new StringBuilder();

            char c;
            do
            {
                c = (char)textReader.Read();
                if (c != 0)
                {
                    sb.Append(c);
                }
            } while (c != 0);

            return sb.ToString();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public static string ReadUntilNull(byte[] buffer, int position)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException("buffer");
            }
            //StringBuilder sb = new StringBuilder();

            int i = position;

            while (buffer[i] != 0)
            {
                i++;
            }

            byte[] bs = new byte[i - position];
            Array.Copy(buffer, position, bs, 0, i - position);

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
        public static string[] TileSetNames = {
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

