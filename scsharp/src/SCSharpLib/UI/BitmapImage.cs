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
using System.Globalization;

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

        /// <summary>
        /// String representation of circle
        /// </summary>
        /// <returns>string representation of circle</returns>
        public override string ToString()
        {
            return String.Format(CultureInfo.CurrentCulture, "({0},{1}, {2})", this.image, this.pixelHeight, this.PixelWidth);
        }

        /// <summary>
        /// Equals operator
        /// </summary>
        /// <param name="obj">Circle to compare</param>
        /// <returns>true if circles are equal</returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }
            if (obj.GetType() != typeof(BitmapImage))
                return false;

            BitmapImage c = (BitmapImage)obj;
            return ((this.image == c.image) && (this.pixelHeight == c.pixelHeight) && (this.pixelWidth == c.pixelWidth));
        }

        /// <summary>
        /// Equals operator
        /// </summary>
        /// <param name="c1">Circle to compare</param>
        /// <param name="c2">Circle to compare</param>
        /// <returns>True if circles are equal</returns>
        public static bool operator ==(BitmapImage c1, BitmapImage c2)
        {
            return ((c1.image == c2.image) && (c1.pixelHeight == c2.pixelHeight) && (c1.pixelWidth == c2.pixelWidth));
        }

        /// <summary>
        /// Not equals operator
        /// </summary>
        /// <param name="c1">Circle to compare</param>
        /// <param name="c2">Circle to compare</param>
        /// <returns>True if circles are not equal</returns>
        public static bool operator !=(BitmapImage c1, BitmapImage c2)
        {
            return !(c1 == c2);
        }

        /// <summary>
        /// Hash Code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            return image.GetHashCode() ^ pixelWidth ^ pixelHeight;
        }
    }
}
