#region LICENSE
/*
 * $RCSfile: FontMode.cs,v $
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

using System;
using System.IO;
using System.Drawing;

using SdlDotNet.Graphics.Sprites;

namespace SdlDotNetExamples.SpriteDemos
{
    /// <summary>
    /// 
    /// </summary>
    public class FontMode : DemoMode
    {
        private BoundedTextSprite moving;
        string dataDirectory = "Data";
        string filePath = Path.Combine("..", "..");

        /// <summary>
        /// Constructs the internal sprites needed for our demo.
        /// </summary>
        public FontMode()
        {
            if (File.Exists(Path.Combine(dataDirectory, "comic.ttf")))
            {
                filePath = "";
            }
            // Create our fonts
            SdlDotNet.Graphics.Font f1 =
                new SdlDotNet.Graphics.Font(Path.Combine(filePath, Path.Combine(dataDirectory, "comicbd.ttf")), 24);
            SdlDotNet.Graphics.Font f2 =
                new SdlDotNet.Graphics.Font(Path.Combine(filePath, Path.Combine(dataDirectory, "comicbd.ttf")), 48);
            SdlDotNet.Graphics.Font f3 =
                new SdlDotNet.Graphics.Font(Path.Combine(filePath, Path.Combine(dataDirectory, "comicbd.ttf")), 72);
            SdlDotNet.Graphics.Font f4 =
                new SdlDotNet.Graphics.Font(Path.Combine(filePath, Path.Combine(dataDirectory, "comicbd.ttf")), 15);

            // Create our text sprites
            Color c2 = Color.FromArgb(255, 0, 123);
            Sprites.Add(new TextSprite("Testing...", f1, new Point(5, 5)));
            Sprites.Add(new TextSprite("...one", f2, c2, new Point(5, 35)));
            Sprites.Add(new TextSprite("...two", f3, c2, new Point(5, 90)));

            Sprites.Add(new TextSprite("A quick brown fox", f2, new Point(5, 200)));
            Sprites.Add(new TextSprite("jumps over the lazy", f2, new Point(5, 280)));
            Sprites.Add(new TextSprite("dog. 1234567890", f2, new Point(5, 360)));

            Sprites.Add(new BoundedTextSprite("one", f4,
                0.1, 0.5, new Point(5, 450)));
            Sprites.Add(new BoundedTextSprite("one", f4,
                0.25, 0.0, new Point(50, 465)));
            Sprites.Add(new BoundedTextSprite("one", f4,
                0.5, 1.0, new Point(100, 480)));
            Sprites.Add(new BoundedTextSprite("one", f4,
                1.0, 0.5, new Point(150, 495)));

            // Add the moving one
            moving = new BoundedTextSprite("two", f4,
                0.0, 0.5, new Point(10, 510));
            Sprites.Add(moving);
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

        #region Operators
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "Font";
        }
        #endregion
    }
}
