/*
 * Copyright (C) 2006 by Drazen Soronda, Croatia
 *
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later version.
 * 
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 * 
 * You should have received a copy of the GNU Lesser General Public
 * License along with this library; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
 *
 * Exclusive rights to for use to SDL.NET & GTK# projects !!!
 * Others may contact me on SDL.NET forums under nickname Shoky
 */

using System;
using SdlDotNet.Graphics;
using Gtk;
using System.IO;
using System.Drawing;

namespace SdlDotNet.GtkSharp
{
    /// <summary>
    /// 
    /// </summary>
    [CLSCompliant(false)]
    public class SdlSurface : DrawingArea
    {
        private Surface surface = new Surface(new Size(0, 0));	// empty surface

        /// <summary>
        /// SDL.NET.Surface
        /// </summary>
        public Surface Surface
        {
            get
            {
                return this.surface;
            }
            set
            {
                this.surface.Dispose();
                this.surface = value;
                this.SetSizeRequest(surface.Width, surface.Height);
            }
        }

        /// <summary>
        /// Basic constructor
        /// </summary>
        public SdlSurface()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        [CLSCompliant(false)]
        protected override bool OnExposeEvent(Gdk.EventExpose args)
        {
            this.GdkWindow.DrawPixbuf(null, ImageToPixbuf(surface.Bitmap), 0, 0, 0, 0, surface.Width, surface.Height,
                        Gdk.RgbDither.Normal, 0, 0);
            return true;
        }

        /// <summary>
        /// Converts System.Drawing.Bitmap to GTK.PixBuf
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        private static Gdk.Pixbuf ImageToPixbuf(System.Drawing.Bitmap image)
        {
            if (image != null)
            {
                using (System.IO.MemoryStream stream = new MemoryStream())
                {
                    image.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
                    stream.Position = 0;
                    Gdk.Pixbuf pixbuf = new Gdk.Pixbuf(stream);
                    return pixbuf;
                }
            }
            else
            {
                return null;
            }
        }

    }
}
