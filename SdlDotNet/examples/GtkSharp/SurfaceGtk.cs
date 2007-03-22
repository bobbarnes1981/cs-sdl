#region LICENSE
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
#endregion LICENSE

using System;
using SdlDotNet.Input;
using SdlDotNet.Graphics;
using Gtk;
using System.IO;
using System.Drawing;
using System.Diagnostics.CodeAnalysis;

namespace SdlDotNet.GtkSharp
{

    /// <summary>
    /// 
    /// </summary>
    [CLSCompliant(false)]
    [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Justification = "Correct Spelling")]
    public class SurfaceGtk : DrawingArea
    {
        /// <summary>
        /// Save absolute mouse position X-Position
        /// </summary>
        private int mouseX;

        /// <summary>
        /// 
        /// </summary>
        protected int MouseX
        {
            get { return mouseX; }
            set { mouseX = value; }
        }

        /// <summary>
        /// Save absolute mouse position Y-Position
        /// </summary>
        private int mouseY;

        /// <summary>
        /// 
        /// </summary>
        protected int MouseY
        {
            get { return mouseY; }
            set { mouseY = value; }
        }

        /// <summary>
        /// Save relative mouse position X-Position
        /// </summary>
        private int relativeX;

        /// <summary>
        /// 
        /// </summary>
        protected int RelativeX
        {
            get { return relativeX; }
            set { relativeX = value; }
        }

        /// <summary>
        /// Save relative mouse position Y-Position
        /// </summary>
        private int relativeY;

        /// <summary>
        /// 
        /// </summary>
        protected int RelativeY
        {
            get { return relativeY; }
            set { relativeY = value; }
        }

        /// <summary>
        /// Save mouse button status
        /// </summary>
        private MouseButton mouseButton;

        /// <summary>
        /// 
        /// </summary>
        protected MouseButton MouseButton
        {
            get { return mouseButton; }
            set { mouseButton = value; }
        }

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
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                this.surface.Dispose();
                this.surface = value;
                this.SetSizeRequest(surface.Width, surface.Height);
            }
        }

        /// <summary>
        /// Basic constructor
        /// </summary>
        public SurfaceGtk()
        {
            // init mouse parameters
            //mouseX = 0;
            //mouseY = 0;
            //relativeX = 0;
            //relativeY = 0;
            mouseButton = MouseButton.None;

            // Enable Events 
            this.AddEvents((int)Gdk.EventMask.AllEventsMask);

            // Connect button events for mapping
            this.ButtonPressEvent += new ButtonPressEventHandler(this.OnButtonPress);
            this.ButtonReleaseEvent += new ButtonReleaseEventHandler(this.OnButtonRelease);

            // Connect motion events for mapping
            this.MotionNotifyEvent += new MotionNotifyEventHandler(this.OnMouseMotion);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="evnt"></param>
        /// <returns></returns>
        [CLSCompliant(false)]
        protected override bool OnExposeEvent(Gdk.EventExpose evnt)
        {
            this.GdkWindow.DrawPixbuf(null, ImageToPixbuf(surface.Bitmap), 0, 0, 0, 0, surface.Width, surface.Height,
                        Gdk.RgbDither.Normal, 0, 0);
            return true;
        }

        /// <summary>
        /// Process mouse motion event. Save coordinates and map the event.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="args">Event arguments</param>
        private void OnMouseMotion(object sender, Gtk.MotionNotifyEventArgs args)
        {
            bool pressed;
            MouseMotionEventArgs eventArgs;

            // Save mouse position (relative and absolute)
            relativeX = (int)(args.Event.X - mouseX);
            relativeY = (int)(args.Event.Y - mouseY);
            mouseX = (int)args.Event.X;
            mouseY = (int)args.Event.Y;

            // Check mouse button
            pressed = mouseButton != MouseButton.None;

            // Prepare mouse event
            eventArgs = new MouseMotionEventArgs(pressed, mouseButton, (short)mouseX, (short)mouseY, (short)relativeX, (short)relativeY);
            // queue mouse event
            SdlDotNet.Core.Events.AddEvent(eventArgs);
            // execute event queue
            SdlDotNet.Core.Events.Poll();

        }

        /// <summary>
        /// Receives Gtk ButtonPress Signal an maps it into an SDL Event
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="args">Event arguments</param>
        private void OnButtonPress(object sender, Gtk.ButtonPressEventArgs args)
        {
            MouseButtonEventArgs mouseEvent;

            // Map Mouse Button
            switch (args.Event.Button)
            {
                case 1:
                    mouseButton = MouseButton.PrimaryButton;
                    break;
                case 2:
                    mouseButton = MouseButton.MiddleButton;
                    break;
                case 3:
                    mouseButton = MouseButton.SecondaryButton;
                    break;
                default:
                    mouseButton = MouseButton.None;
                    break;
            }

            // Create SDL Mouse Button Event
            mouseEvent = new MouseButtonEventArgs(mouseButton, true, (short)mouseX, (short)mouseY);
            // Queue Event
            SdlDotNet.Core.Events.AddEvent(mouseEvent);
            // Execute Event Queue
            SdlDotNet.Core.Events.Poll();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnButtonRelease(object sender, Gtk.ButtonReleaseEventArgs args)
        {
            // Reset mouse button
            mouseButton = MouseButton.None;
            // Prepare release event
            MouseButtonEventArgs mouseEvent = new MouseButtonEventArgs(mouseButton, false, (short)relativeX, (short)relativeY);
            // Queue release event
            SdlDotNet.Core.Events.AddEvent(mouseEvent);
            // Execute event queue
            SdlDotNet.Core.Events.Poll();
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
