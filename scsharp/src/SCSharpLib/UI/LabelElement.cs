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
using System.Text;
using System.Threading;

using SdlDotNet.Graphics;
using SCSharp.MpqLib;
using System.Drawing;

namespace SCSharp.UI
{
    /// <summary>
    ///
    /// </summary>
    public class LabelElement : UIElement
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="screen"></param>
        /// <param name="el"></param>
        /// <param name="palette"></param>
        public LabelElement(UIScreen screen, BinElement el, byte[] palette)
            : base(screen, el, palette)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected override Surface CreateSurface()
        {
            /* this is wrong */
            Surface surf = new Surface(Width, Height);

            Surface textSurf = GuiUtility.ComposeText(Text, Font, Palette, Width, Height,
            Sensitive ? 4 : 24); ;

            int x = 0;
            if (Type == ElementType.LabelRightAlign)
            {
                x += Width - textSurf.Width;
            }
            else if (Type == ElementType.LabelCenterAlign)
            {
                x += (Width - textSurf.Width) / 2;
            }

            surf.Blit(textSurf, new Point(x, 0));

            surf.TransparentColor = Color.Black /* XXX */;
            surf.Transparent = true;
            return surf;
        }
    }
}
