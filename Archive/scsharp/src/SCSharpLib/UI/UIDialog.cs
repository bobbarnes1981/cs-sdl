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
using System.Threading;
using System.Collections.Generic;

using SdlDotNet.Graphics;
using SCSharp;
using SCSharp.MpqLib;

namespace SCSharp.UI
{
    /* this should probably subclass from UIElement instead... look into that */
    /// <summary>
    ///
    /// </summary>
    public abstract class UIDialog : UIScreen, IDisposable 
    {
        //protected UIScreen parent;
        bool dimScreen;
        Surface dimScreenSurface;

        /// <summary>
        ///
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="mpq"></param>
        /// <param name="prefix"></param>
        /// <param name="binFile"></param>
        protected UIDialog(UIScreen parent, Mpq mpq, string prefix, string binFile)
            : base(mpq, prefix, binFile)
        {
            this.Parent = parent;
            BackgroundTranslucent = 254;
            BackgroundTransparent = 0;

            dimScreen = true;

            dimScreenSurface = new Surface(Painter.ScreenResX, Painter.ScreenResY);
            dimScreenSurface.Alpha = 100;
            dimScreenSurface.AlphaBlending = true;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="painter"></param>
        public override void AddToPainter(Painter painter)
        {
            if (painter == null)
            {
                throw new ArgumentNullException("painter");
            }

            this.Painter = painter;

            if (Background != null)
            {
                painter.Add(Layer.DialogBackground, BackgroundPainter);
            }

            if (UIPainter != null)
            {
                painter.Add(Layer.DialogUI, UIPainter.Paint);
            }

            if (dimScreen)
            {
                painter.Add(Layer.DialogDimScreenHack, DimScreenPainter);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="painter"></param>
        public override void RemoveFromPainter(Painter painter)
        {
            if (painter == null)
            {
                throw new ArgumentNullException("painter");
            }
            if (Background != null)
            {
                painter.Remove(Layer.DialogBackground, BackgroundPainter);
            }

            if (UIPainter != null)
            {
                painter.Remove(Layer.DialogUI, UIPainter.Paint);
            }

            if (dimScreen)
            {
                painter.Remove(Layer.DialogDimScreenHack, DimScreenPainter);
            }

            this.Painter = null;
        }

        void DimScreenPainter(Surface surf, DateTime dt)
        {
            surf.Blit(dimScreenSurface);
        }

        /// <summary>
        ///
        /// </summary>
        protected override void ResourceLoader()
        {
            base.ResourceLoader();

            /* figure out where we're going to be located on the screen */
            int baseX, baseY;
            int si;

            if (Background != null)
            {
                baseX = (Painter.ScreenResX - Background.Width) / 2;
                baseY = (Painter.ScreenResY - Background.Height) / 2;
                si = 0;
            }
            else
            {
                baseX = Elements[0].X1;
                baseY = Elements[0].Y1;
                si = 1;
            }

            /* and add that offset to all our elements */
            for (int i = si; i < Elements.Count; i++)
            {
                Elements[i].X1 += (ushort)baseX;
                Elements[i].Y1 += (ushort)baseY;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public override bool UseTiles
        {
            get { return true; }
        }

        //public override Painter Painter
        //{
        // get { return painter; }
        //}

        //public UIScreen Parent
        //{
        // get { return parent; }
        //}

        /// <summary>
        ///
        /// </summary>
        public bool DimScreen
        {
            get { return dimScreen; }
            set { dimScreen = value; }
        }

        Painter rememberedPainter;

        /// <summary>
        ///
        /// </summary>
        /// <param name="dialog"></param>
        public override void ShowDialog(UIDialog dialog)
        {
            if (dialog == null)
            {
                throw new ArgumentNullException("dialog");
            }
            Console.WriteLine("showing {0}", dialog);

            if (this.Dialog != null)
            {
                throw new SCException("only one active dialog is allowed");
            }
            this.Dialog = dialog;

            dialog.Load();
            dialog.Ready += delegate(object sender, SCEventArgs e)
            {
                dialog.AddToPainter(this.Painter);
                rememberedPainter = this.Painter;
                RemoveFromPainter(this.Painter);
            };
        }

        /// <summary>
        ///
        /// </summary>
        public override void DismissDialog()
        {
            if (Dialog == null)
            {
                return;
            }

            Dialog.RemoveFromPainter(rememberedPainter);
            Dialog = null;
            AddToPainter(rememberedPainter);
        }

        #region IDisposable Members

        bool disposed;
        /// <summary>
        /// Destroy sprite
        /// </summary>
        /// <param name="disposing">If true, remove all unamanged resources</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    if (this.dimScreenSurface != null)
                    {
                        this.dimScreenSurface.Dispose();
                        this.dimScreenSurface = null;
                    }
                }
                this.disposed = true;
            }
        }

                /// <summary>
        /// Destroy object
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Destroy object
        /// </summary>
        public void Close()
        {
            Dispose();
        }

        /// <summary>
        /// Destroy object
        /// </summary>
        ~UIDialog()
        {
            Dispose(false);
        }


        #endregion
    }

    ///// <summary>
    /////
    ///// </summary>
    ////public delegate void DialogEvent(object sender, SCEventArgs args);
    //public delegate void DialogEventHandler(object sender, EventArgs e);
}
