#region LICENSE
/*
 * $RCSfile: DragMode.cs,v $
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

using System.Collections;
using System.Drawing;
using System.Threading;

using SdlDotNet.Graphics;
using SdlDotNet.Core;
using SdlDotNet.Graphics.Sprites;
using SdlDotNetExamples.LargeDemos;

namespace SdlDotNetExamples.SpriteDemos
{
    /// <summary>
    /// 
    /// </summary>
    public class DragMode : DemoMode
    {
        /// <summary>
        /// Constructs the internal sprites needed for our demo.
        /// </summary>
        public DragMode()
        {
            // Create the fragment marbles
            int rows = 3;
            int cols = 3;
            int sx = (SpriteDemosMain.Size.Width - cols * 50) / 2;
            int sy = (SpriteDemosMain.Size.Height - rows * 50) / 2;
            SurfaceCollection m1 = LoadMarble("marble1");
            SurfaceCollection m2 = LoadMarble("marble2");
            AnimationCollection anim1 = new AnimationCollection();
            anim1.Add(m1);
            AnimationCollection anim2 = new AnimationCollection();
            anim2.Add(m2);
            AnimationDictionary frames = new AnimationDictionary();
            frames.Add("marble1", anim1);
            frames.Add("marble2", anim2);

            DragSprite dragSprite;
            for (int i = 0; i < cols; i++)
            {
                Thread.Sleep(10);
                for (int j = 0; j < rows; j++)
                {
                    dragSprite = new DragSprite(frames["marble1"],
                        new Point(sx + i * 50, sy + j * 50)
                        );
                    dragSprite.Animations.Add("marble1", anim1);
                    dragSprite.Animations.Add("marble2", anim2);
                    dragSprite.Animate = true;
                    if (Randomizer.Next(2) == 1)
                    {
                        dragSprite.AnimateForward = false;
                    }
                    Thread.Sleep(10);
                    Sprites.Add(dragSprite);
                }
            }
        }

        /// <summary>
        /// Adds the internal sprite manager to the outer one.
        /// </summary>
        public override void Start()
        {
            Sprites.EnableTickEvent();
            Sprites.EnableMouseButtonEvent();
            Sprites.EnableMouseMotionEvent();
        }

        /// <summary>
        /// Removes the internal manager from the controlling manager.
        /// </summary>
        public override void Stop()
        {
            Sprites.DisableTickEvent();
            Sprites.DisableMouseButtonEvent();
            Sprites.DisableMouseMotionEvent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString() 
        { 
            return "Drag"; 
        }
    }
}
