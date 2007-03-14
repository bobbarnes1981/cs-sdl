#region LICENSE
/*
 * Copyright (C) 2007 by Thomas Krieger
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
using System.Drawing;
using System.Collections;

using SdlDotNet;
using SdlDotNet.Graphics;
using SdlDotNet.Graphics.Sprites;
using SdlDotNet.Input;
using SdlDotNet.Core;

namespace SdlDotNetExamples.SmallDemos
{
    public class GtkSprite : Sprite
    {
        /// <summary>
        /// Defaultkonstruktor
        /// </summary>
        /// <param name="path">Image path (png,jpg etc.)</param>
        public GtkSprite(string path)
            : base(path)
        {
            this.AllowDrag = true;
        }

        /// <summary>
        /// Update Method for Mouse Motion Events
        /// </summary>
        /// <param name="args"></param>
        public override void Update(MouseMotionEventArgs args)
        {
            if (args == null)
            {
                throw new ArgumentNullException("args");
            }
            if (!AllowDrag)
            {
                return;
            }

            // Move the window as appropriate
            if (this.BeingDragged)
            {
                this.X += args.RelativeX;
                this.Y += args.RelativeY;
            }
        }

        /// <summary>
        /// Update Method for the Mouse Button Events
        /// </summary>
        /// <param name="args"></param>
        public override void Update(MouseButtonEventArgs args)
        {
            if (args == null)
            {
                throw new ArgumentNullException("args");
            }
            if (this.IntersectsWith(new Point(args.X, args.Y)))
            {
                // If we are being held down, pick up the marble
                if (args.ButtonPressed)
                {
                    if (args.Button == MouseButton.PrimaryButton)
                    {
                        this.BeingDragged = true;
                    }
                }
            }
            else
            {
                this.BeingDragged = false;
            }
        }
    }
}
