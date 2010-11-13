#region LICENSE
//
// Authors:
//	Chris Toshok (toshok@hungry.com)
//
// (C) 2006 The Hungry Programmers (http://www.hungry.com/)
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

using SCSharp.UI;

namespace SCSharp.Tests
{
    /// <summary>
    /// 
    /// </summary>
    public class Bmp
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="grid"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="palette"></param>
        public static void WriteBmp(string filename, byte[,] grid, uint width, uint height, byte[] palette)
        {
            using (FileStream fs = File.OpenWrite(filename))
            {
                fs.WriteByte((byte)'B');
                fs.WriteByte((byte)'M');

                Utilities.WriteDWord(fs, 0); // size of the file (we seek back and patch it at the end)

                Utilities.WriteWord(fs, 0);
                Utilities.WriteWord(fs, 0);

                Utilities.WriteDWord(fs, 1078);

                // info header
                Utilities.WriteDWord(fs, 40);
                Utilities.WriteDWord(fs, width);
                Utilities.WriteDWord(fs, height);
                Utilities.WriteWord(fs, 0);
                Utilities.WriteWord(fs, 8);
                Utilities.WriteDWord(fs, 0);
                Utilities.WriteDWord(fs, 0);
                Utilities.WriteDWord(fs, 0);
                Utilities.WriteDWord(fs, 0);
                Utilities.WriteDWord(fs, 0);
                Utilities.WriteDWord(fs, 0);

                int i;
                // grayscale colormap
                for (i = 0; i < 256 * 3; i += 3)
                {
                    fs.WriteByte(palette[i]);
                    fs.WriteByte(palette[i + 1]);
                    fs.WriteByte(palette[i + 2]);
                    fs.WriteByte(0);
                }

                // pixel data
                uint padding = width % 4;
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        fs.WriteByte(grid[y, x]);
                    }
                    for (i = 0; i < padding; i++)
                    {
                        fs.WriteByte(0);
                    }
                }

                uint size = (uint)fs.Position;
                fs.Seek(2, SeekOrigin.Begin);
                Utilities.WriteDWord(fs, size);
                fs.Close();
            }
        }
    }
}