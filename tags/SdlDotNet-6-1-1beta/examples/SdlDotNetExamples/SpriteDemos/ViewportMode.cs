#region LICENSE
/*
 * $RCSfile: ViewportMode.cs,v $
 * Copyright (C) 2004 D. R. E. Moonfire (d.moonfire@mfgames.com)
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
 */
#endregion LICENSE

using System.Drawing;
using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Threading;

using SdlDotNet.Graphics;
using SdlDotNet.Core;
using SdlDotNet.Graphics.Sprites;

namespace SdlDotNetExamples.SpriteDemos
{
    /// <summary>
    /// 
    /// </summary>
    public class ViewportMode : DemoMode
    {
        private Sprite sprite;
        Collection<Sprite> spriteSingle = new Collection<Sprite>();
        private Size size;
        Rectangle rect;

        /// <summary>
        /// 
        /// </summary>
        public Collection<Sprite> CenterSprite
        {
            get
            {
                return spriteSingle;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Rectangle ViewRect
        {
            get
            {
                return rect;
            }
        }

        /// <summary>
        /// Constructs the internal sprites needed for our demo.
        /// </summary>
        public ViewportMode()
        {
            // Create the fragment marbles
            SurfaceCollection td = LoadMarble("marble1");
            SurfaceCollection td2 = LoadMarble("marble2");

            // Load the floor
            SurfaceCollection floorTiles = LoadFloor();

            // Place the floors
            int rows = 8;
            int cols = 15;
            size = new Size(floorTiles[0].Size.Width * cols,
                floorTiles[0].Size.Height * rows);
            rect = new Rectangle(new Point(0, 0), size);

            for (int i = 0; i < cols; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    // Create the floor tile sprites
                    AnimatedSprite dw =
                        new AnimatedSprite(floorTiles,
                        new Point(i * floorTiles[0].Size.Width,
                        j * floorTiles[0].Size.Height));
                    Sprites.Add(dw);
                }
            }

            // Load the trigger sprite
            sprite = new BounceSprite(td2, rect, new Vector(Randomizer.Next(rect.Left, rect.Right -
                (int)td2.Size.Width),
                Randomizer.Next(rect.Top, rect.Bottom -
                (int)td2.Size.Height), 1));
            Sprites.Add(sprite);
            CenterSprite.Add(sprite);

            // Load the bouncing sprites
            for (int i = 0; i < 10; i++)
            {
                Thread.Sleep(10);
                BounceSprite bounceSprite =
                    new BounceSprite(td,
                    rect,
                    new Vector(Randomizer.Next(rect.Left, rect.Right -
                    (int)td.Size.Width),
                    Randomizer.Next(rect.Top, rect.Bottom -
                    (int)td.Size.Height), 1));
                Sprites.Add(bounceSprite);
            }
        }
        
        /// <summary>
        /// Adds the internal sprite manager to the outer one.
        /// </summary>
        public override void Start()
        {
            Sprites.EnableTickEvent();
        }

        /// <summary>
        /// Removes the internal manager from the controlling manager.
        /// </summary>
        public override void Stop()
        {
            Sprites.DisableTickEvent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString() 
        { 
            return "Viewport"; 
        }

        #region Events
        /// <summary>
        /// 
        /// </summary>
        public override Surface RenderSurface()
        {
            this.Surface.Fill(Color.Black);
            foreach (Sprite s in Sprites)
            {
                Rectangle offsetRect = s.Rectangle;
                offsetRect.Offset(AdjustBoundedViewport());
                this.Surface.Blit(s, offsetRect);
            }
            return this.Surface;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Point AdjustViewport()
        {
            return new Point(
             this.Surface.Size.Width / 2 -
             this.CenterSprite[0].Size.Width / 2 -
             this.CenterSprite[0].X,
             this.Surface.Size.Height / 2 -
             this.CenterSprite[0].Size.Height / 2 -
             this.CenterSprite[0].Y);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Point AdjustBoundedViewport()
        {
            Point offset = this.AdjustViewport();

            // Check to see if the window is too small
            bool doWidth = true;
            bool doHeight = true;

            if (this.ViewRect.Width < this.Surface.Size.Width)
            {
                doWidth = false;
            }

            if (this.ViewRect.Height < this.Surface.Size.Height)
            {
                doHeight = false;
            }

            if (!doWidth && !doHeight)
            {
                return offset;
            }

            // Find out the "half" point for the sprite in the view
            int mx = this.CenterSprite[0].X + this.CenterSprite[0].Size.Width / 2;
            int my = this.CenterSprite[0].Y + this.CenterSprite[0].Size.Height / 2;

            // Figure out the coordinates
            int x1 = mx - this.Surface.Size.Width / 2;
            int x2 = mx + this.Surface.Size.Width / 2;
            int y1 = my - this.Surface.Size.Height / 2;
            int y2 = my + this.Surface.Size.Height / 2;

            // Make sure we don't exceed the bounds
            if (doWidth && x1 < this.ViewRect.Left)
            {
                offset.X -= this.ViewRect.Left - x1;
            }

            if (doHeight && y1 < this.ViewRect.Top)
            {
                offset.Y -= this.ViewRect.Top - y1;
            }

            if (doWidth && x2 > this.ViewRect.Right)
            {
                offset.X += x2 - this.ViewRect.Right;
            }

            if (doHeight && y2 > this.ViewRect.Bottom)
            {
                offset.Y += y2 - this.ViewRect.Bottom;
            }
            return new Point(offset.X, offset.Y);
        }
        #endregion
    }
}
