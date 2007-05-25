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
using System.Drawing;
using System.IO;
using System.Text;
using System.Collections.Generic;

using SdlDotNet.Graphics;
using SdlDotNet.Input;
using SCSharp;
using SCSharp.MpqLib;

namespace SCSharp.UI
{
    /// <summary>
    ///
    /// </summary>
    public class UIElement
    {
        BinElement el;
        Surface surface;
        UIScreen screen;
        byte[] palette;
        bool sensitive;
        bool visible;
        SCFont fnt;
        string background;
        Surface backgroundSurface;

        /// <summary>
        ///
        /// </summary>
        /// <param name="screen"></param>
        /// <param name="el"></param>
        /// <param name="palette"></param>
        public UIElement(UIScreen screen, BinElement el, byte[] palette)
        {
            if (el == null)
            {
                throw new ArgumentNullException("el");
            }
            this.screen = screen;
            this.el = el;
            this.x1 = el.X1;
            this.y1 = el.Y1;
            this.palette = palette;
            this.sensitive = true;
            this.visible = (el.Flags & SCElements.Visible) != 0;
        }

        /// <summary>
        ///
        /// </summary>
        public UIScreen ParentScreen
        {
            get { return screen; }
        }

        /// <summary>
        ///
        /// </summary>
        public Mpq Mpq
        {
            get { return screen.Mpq; }
        }

        /// <summary>
        ///
        /// </summary>
        public string Background
        {
            get { return background; }
            set
            {
                background = value;
                backgroundSurface = null;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public string Text
        {
            get { return el.Text1; }
            set
            {
                el.Text1 = value;
                ClearSurface();
            }
        }

        /// <summary>
        ///
        /// </summary>
        public bool Sensitive
        {
            get { return sensitive; }
            set
            {
                sensitive = value;
                ClearSurface();
            }
        }

        /// <summary>
        ///
        /// </summary>
        public bool Visible
        {
            get { return visible; }
            set
            {
                visible = value;
                ClearSurface();
            }
        }

        /// <summary>
        ///
        /// </summary>
        public byte[] Palette
        {
            get { return palette; }
            set
            {
                palette = value;
                ClearSurface();
            }
        }

        /// <summary>
        ///
        /// </summary>
        public Surface Surface
        {
            get
            {
                if (surface == null)
                {
                    surface = CreateSurface();
                }

                if (background != null && background.Length != 0
                && backgroundSurface == null)
                {
                    backgroundSurface = CreateBackgroundSurface();
                }

                return surface;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public SCFont Font
        {
            get
            {
                if (fnt == null)
                {
                    int idx = 2;

                    if ((Flags & SCElements.FontSmallest) != 0)
                    {
                        idx = 0;
                    }
                    else if ((Flags & SCElements.FontSmaller) != 0)
                    {
                        idx = 3;
                    }
                    else if ((Flags & SCElements.FontLarger) != 0)
                    {
                        idx = 3;
                    }
                    else if ((Flags & SCElements.FontLargest) != 0)
                    {
                        idx = 4;
                    }

                    fnt = GuiUtility.GetFonts(Mpq)[idx];

                    if (fnt == null)
                    {
                        throw new SCException(String.Format("null font at index {0}.. bad things are afoot", idx));
                    }
                }
                return fnt;
            }
            set
            {
                fnt = value;
                ClearSurface();
            }
        }

        /// <summary>
        ///
        /// </summary>
        public SCElements Flags { get { return el.Flags; } }

        /// <summary>
        ///
        /// </summary>
        public ElementType Type { get { return el.Type; } }

        ushort x1;
        /// <summary>
        ///
        /// </summary>
        [CLSCompliant(false)]
        public ushort X1
        {
            get { return x1; }
            set { x1 = value; }
        }

        ushort y1;
        /// <summary>
        ///
        /// </summary>
        [CLSCompliant(false)]
        public ushort Y1
        {
            get { return y1; }
            set { y1 = value; }
        }

        /// <summary>
        ///
        /// </summary>
        [CLSCompliant(false)]
        public ushort Width { get { return el.Width; } }

        /// <summary>
        ///
        /// </summary>
        [CLSCompliant(false)]
        public ushort Height { get { return el.Height; } }

        /// <summary>
        ///
        /// </summary>
        public Key Hotkey { get { return (Key)el.Hotkey; } }

        /// <summary>
        ///
        /// </summary>
        public event EventHandler<SCEventArgs> Activate;

        /// <summary>
        ///
        /// </summary>
        public void OnActivate()
        {
            if (Activate != null)
            {
                Activate(this, new SCEventArgs());
            }
        }

        /// <summary>
        ///
        /// </summary>
        public event EventHandler<SCEventArgs> MouseEnterEvent;

        /// <summary>
        ///
        /// </summary>
        public void OnMouseEnter()
        {
            if (MouseEnterEvent != null)
            {
                MouseEnterEvent(this, new SCEventArgs());
            }
        }

        /// <summary>
        ///
        /// </summary>
        public event EventHandler<SCEventArgs> MouseLeaveEvent;

        /// <summary>
        ///
        /// </summary>
        public void OnMouseLeave()
        {
            if (MouseLeaveEvent != null)
            {
                MouseLeaveEvent(this, new SCEventArgs());
            }
        }

        /// <summary>
        ///
        /// </summary>
        protected void ClearSurface()
        {
            surface = null;
        }

        Surface CreateBackgroundSurface()
        {
            return GuiUtility.SurfaceFromStream((Stream)Mpq.GetResource(background),
            254, 0);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual Surface CreateSurface()
        {
            switch (Type)
            {
                case ElementType.DefaultButton:
                case ElementType.Button:
                case ElementType.ButtonWithoutBorder:
                    return GuiUtility.ComposeText(Text, Font, palette, Width, Height,
                    sensitive ? 4 : 24);
                default:
                    return null;
            }
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
            if (!visible)
            {
                return;
            }

            if (Surface == null)
            {
                return;
            }

            if (backgroundSurface != null)
            {
                surf.Blit(backgroundSurface, new Point(X1, Y1));
            }
            surf.Blit(surface, new Point(X1, Y1));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="positionX"></param>
        /// <param name="positionY"></param>
        /// <returns></returns>
        public virtual bool PointInside(int positionX, int positionY)
        {
            if (positionX >= X1 && positionX < X1 + Width &&
            positionY >= Y1 && positionY < Y1 + Height)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="args"></param>
        public virtual void MouseWheel(MouseButtonEventArgs args)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="args"></param>
        public virtual void MouseButtonDown(MouseButtonEventArgs args)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="args"></param>
        public virtual void MouseButtonUp(MouseButtonEventArgs args)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="args"></param>
        public virtual void PointerMotion(MouseMotionEventArgs args)
        {
        }

        /// <summary>
        ///
        /// </summary>
        public virtual void MouseEnter()
        {
            OnMouseEnter();
        }

        /// <summary>
        ///
        /// </summary>
        public virtual void MouseLeave()
        {
            OnMouseLeave();
        }
    }

    ///// <summary>
    /////
    ///// </summary>
    //public delegate void ElementEventHandler(object sender, EventArgs e);
}
