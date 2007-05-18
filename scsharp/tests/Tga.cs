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
    public class Tga
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="image"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public static void WriteTga(string filename, byte[] image, uint width, uint height)
        {
            using (FileStream fs = File.OpenWrite(filename))
            {
                fs.WriteByte(0);
                fs.WriteByte(0); // no colormap
                fs.WriteByte(2); // rgb

                Utilities.WriteWord(fs, 0); // first color map entry
                Utilities.WriteWord(fs, 0); // number of colors in palette
                fs.WriteByte(0); // number of bites per palette entry

                Console.WriteLine("width = {0}, height = {1}", (ushort)width, (ushort)height);

                Utilities.WriteWord(fs, 0); // image x origin
                Utilities.WriteWord(fs, 0); // image y origin
                Utilities.WriteWord(fs, (ushort)width); // width
                Utilities.WriteWord(fs, (ushort)height); // height

                fs.WriteByte(24); // bits per pixel

                fs.WriteByte(32);

                fs.Write(image, 0, image.Length);

                fs.Close();
            }
        }
    }
}
