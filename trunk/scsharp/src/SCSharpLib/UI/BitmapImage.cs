#region LICENSE
//
// Authors:
// Chris Toshok (toshok@hungry.com)
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

using SdlDotNet.Graphics;
using SCSharp;
using SCSharp.MpqLib;


namespace SCSharp.UI
{
    /// <summary>
    /// 
    /// </summary>
    public struct BitmapImage
    {
        byte[] image;

        /// <summary>
        /// 
        /// </summary>
        public byte[] Image
        {
            get { return image; }
            set { image = value; }
        }
        ushort pixelWidth;

        /// <summary>
        /// 
        /// </summary>
        [CLSCompliant(false)]
        public ushort PixelWidth
        {
            get { return pixelWidth; }
            set { pixelWidth = value; }
        }
        ushort pixelHeight;

        /// <summary>
        /// 
        /// </summary>
        [CLSCompliant(false)]
        public ushort PixelHeight
        {
            get { return pixelHeight; }
            set { pixelHeight = value; }
        }
    }
}
