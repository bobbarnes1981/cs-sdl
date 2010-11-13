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
using SdlDotNet.Graphics;
using SCSharp.MpqLib;

using System.Drawing;
using System.Drawing.Imaging;

namespace SCSharp.UI
{
    /// <summary>
    /// 
    /// </summary>
    public class CursorAnimator
    {
        Grp grp;

        DateTime last;
        TimeSpan deltaToChange = TimeSpan.FromMilliseconds(100);
        int currentFrame;

        Surface[] surfaces;

        uint positionX;
        uint positionY;

        uint hotX;
        uint hotY;

        byte[] palette;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="grp"></param>
        /// <param name="palette"></param>
        public CursorAnimator(Grp grp, byte[] palette)
        {
            if (grp == null)
            {
                throw new ArgumentNullException("grp");
            }

            this.grp = grp;
            this.positionX = 100;
            this.positionY = 100;
            this.palette = palette;
            surfaces = new Surface[grp.FrameCount];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hotX"></param>
        /// <param name="hotY"></param>
        [CLSCompliant(false)]
        public void SetHotspot(uint hotX, uint hotY)
        {
            this.hotX = hotX;
            this.hotY = hotY;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="positionX"></param>
        /// <param name="positionY"></param>
        [CLSCompliant(false)]
        public void SetPosition(uint positionX, uint positionY)
        {
            this.positionX = positionX;
            this.positionY = positionY;
        }

        /// <summary>
        /// 
        /// </summary>
        [CLSCompliant(false)]
        public uint PositionX
        {
            get { return positionX; }
        }

        /// <summary>
        /// 
        /// </summary>
        [CLSCompliant(false)]
        public uint PositionY
        {
            get { return positionY; }
        }

        /// <summary>
        /// 
        /// </summary>
        [CLSCompliant(false)]
        public uint HotX
        {
            get { return hotX; }
        }

        /// <summary>
        /// 
        /// </summary>
        [CLSCompliant(false)]
        public uint HotY
        {
            get { return hotY; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="surf"></param>
        /// <param name="now"></param>
        public void Paint(Surface surf, DateTime now)
        {
            if (surf == null)
            {
                throw new ArgumentNullException("surf");
            }

            deltaToChange -= now - last;
            if (deltaToChange < TimeSpan.Zero)
            {
                currentFrame++;
                deltaToChange = TimeSpan.FromMilliseconds(200);
            }
            last = now;

            int drawX = (int)(positionX - hotX);
            int drawY = (int)(positionY - hotY);

            if (currentFrame == grp.FrameCount)
            {
                currentFrame = 0;
            }

            if (surfaces[currentFrame] == null)
            {
                surfaces[currentFrame] = GuiUtility.CreateSurfaceFromBitmap(grp.GetFrame(currentFrame),
                                               grp.Width, grp.Height,
                                               palette,
                                               true);
            }

            surf.Blit(surfaces[currentFrame], new Point(drawX, drawY));
        }
    }
}
