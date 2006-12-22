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
using System.Drawing;
using System.IO;

using Gtk;
using SdlDotNet.GtkSharp;
using SdlDotNet.Graphics;

namespace SdlDotNetExamples.SmallDemos
{
    public class GtkWindow : IDisposable
    {
        // widgets
        Gtk.Window win;
        Gtk.Button btn;
        SdlDotNet.Graphics.Surface sdlScreen;
        Gtk.VBox verBox;
        Gtk.Image myImg;

        public GtkWindow()
        {	// constructor
            // window
            win = new Gtk.Window("SDL.NET Gtk# Example");
            win.BorderWidth = 0;
            win.DeleteEvent += new DeleteEventHandler(Window_Delete);
            //win.Decorated = false;
            win.DoubleBuffered = true;

            //vertical box
            verBox = new VBox();
            verBox.BorderWidth = 3;
            win.Add(verBox);


            sdlScreen = new Surface(new System.Drawing.Size(250, 100));
            sdlScreen.DrawPrimitive(new SdlDotNet.Graphics.Primitives.Box(10, 10, 240, 90), System.Drawing.Color.Blue);

            // RENDERING THRU Gtk.Image
            myImg = new Gtk.Image(ImageToPixbuf(sdlScreen.Bitmap));
            myImg.DoubleBuffered = true;
            verBox.Add(myImg);

            // Rendering using GTK Widget !! :)
            SurfaceGtk mySurface = new SurfaceGtk();
            sdlScreen.DrawPrimitive(new SdlDotNet.Graphics.Primitives.Circle(125, 50, 25), System.Drawing.Color.Red, false, true);		// ADD RED Circle 
            mySurface.Surface = sdlScreen;
            verBox.Add(mySurface);

            // button
            btn = new Button("Close");
            btn.Clicked += new EventHandler(btn_click);
            verBox.Add(btn);

            win.ShowAll();	// show window

        }
        static void Window_Delete(object obj, DeleteEventArgs args)
        {
            Application.Quit();
        }

        void btn_click(object obj, EventArgs args)
        {
            win.Destroy();
            Application.Quit();
        }

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
        #region IDisposable Members

        private bool disposed;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    if (this.sdlScreen != null)
                    {
                        this.sdlScreen.Dispose();
                        this.sdlScreen = null;
                    }
                    if (this.win != null)
                    {
                        this.win.Dispose();
                        this.win = null;
                    }
                    if (this.verBox != null)
                    {
                        this.verBox.Dispose();
                        this.verBox = null;
                    }
                    if (this.btn != null)
                    {
                        this.btn.Dispose();
                        this.btn = null;
                    }
                    if (this.myImg != null)
                    {
                        this.myImg.Dispose();
                        this.myImg = null;
                    }
                }
                this.disposed = true;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Close()
        {
            Dispose();
        }

        /// <summary>
        /// 
        /// </summary>
        ~GtkWindow()
        {
            Dispose(false);
        }

        #endregion
    }
}
