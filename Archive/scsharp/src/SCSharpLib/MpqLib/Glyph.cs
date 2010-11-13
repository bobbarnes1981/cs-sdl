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
using System.Text;



namespace SCSharp.MpqLib
{
    /// <summary>
    /// 
    /// </summary>
    public class Glyph
    {
        internal Glyph(int width, int height, int xOffset, int yOffset,
                byte[,] bitmap)
        {
            this.bitmap = bitmap;
            this.width = width;
            this.height = height;
            this.xoffset = xOffset;
            this.yoffset = yOffset;
        }

        int width;
        int height;
        int xoffset;
        int yoffset;
        byte[,] bitmap;

        /// <summary>
        /// 
        /// </summary>
        public byte[,] Bitmap
        {
            get { return bitmap; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int Width
        {
            get { return width; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int Height
        {
            get { return height; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int XOffset
        {
            get { return xoffset; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int YOffset
        {
            get { return yoffset; }
        }
    }
}
