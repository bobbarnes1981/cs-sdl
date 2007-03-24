#region LICENSE
/*
 * $RCSfile: BounceMode.cs,v $
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
    public class BounceMode : DemoMode
    {
        /// <summary>
        /// Constructs the internal sprites needed for our demo.
        /// </summary>
        public BounceMode()
        {
            // Create the fragment marbles
            Rectangle rect = new Rectangle(new Point(0, 0), SpriteDemosMain.Size);
            for (int i = 0; i < 10; i++)
            {
                Thread.Sleep(10);
                SurfaceCollection d = LoadRandomMarble();
                BounceSprite bounceSprite =
                    new BounceSprite(d,
                    new Rectangle(new Point(0, 0), SpriteDemosMain.Size),
                    new Vector(Randomizer.Next(rect.Left, rect.Right),
                    Randomizer.Next(rect.Top, rect.Bottom), 1));
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
            return "Bounce"; 
        }
    }
}
