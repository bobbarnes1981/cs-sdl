/*
 * $RCSfile$
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

using MfGames.Sdl.Drawable;
using MfGames.Sdl.Sprites;
using MfGames.Utility;
using SdlDotNet;
using System.Drawing;

namespace MfGames.Sdl.Demos
{
  public class FontMode : DemoMode
  {
    private BoundedTextSprite moving = null;

    /// <summary>
    /// Constructs the internal sprites needed for our demo.
    /// </summary>
    public FontMode()
    {
      // Create our fonts
      SdlDotNet.Font f1 = new SdlDotNet.Font("../../Data/comicbd.ttf", 24);
      SdlDotNet.Font f2 = new SdlDotNet.Font("../../Data/comicbd.ttf", 48);
      SdlDotNet.Font f3 = new SdlDotNet.Font("../../Data/comicbd.ttf", 72);
      SdlDotNet.Font f4 = new SdlDotNet.Font("../../Data/comicbd.ttf", 15);

      // Create our text sprites
      Color c2 = Color.FromArgb(255, 0, 123);

      sm.Add(new TextSprite("Testing...", f1, new Vector2(5, 5)));
      sm.Add(new TextSprite("...one", f2, c2, new Vector2(5, 35)));
      sm.Add(new TextSprite("...two", f3, c2, new Vector2(5, 90)));

      sm.Add(new TextSprite("A quick brown fox", f2, new Vector2(5, 200)));
      sm.Add(new TextSprite("jumps over the lazy", f2, new Vector2(5, 280)));
      sm.Add(new TextSprite("dog. 1234567890", f2, new Vector2(5, 360)));

      int w = SdlDemo.SpriteContainer.Size.Width - 10;
      sm.Add(new BoundedTextSprite("one", f4, new Dimension2(w, 30),
				   0.0,  0.5, new Vector2(5, 450)));
      sm.Add(new BoundedTextSprite("one", f4, new Dimension2(w, 30),
				   0.25, 0.0, new Vector2(5, 465)));
      sm.Add(new BoundedTextSprite("one", f4, new Dimension2(w, 30),
				   0.5,  1.0, new Vector2(5, 480)));
      sm.Add(new BoundedTextSprite("one", f4, new Dimension2(w, 30),
				   1.0,  0.5, new Vector2(5, 495)));

      // Add the moving one
      moving = new BoundedTextSprite("one", f4, new Dimension2(w, 30),
				     0.0, 0.5, new Vector2(5, 510));
      sm.Add(moving);
    }

    #region Events
    private double delta = 0.01;

    public override void OnTick(TickArgs args)
    {
      double dx = args.RatePerSecond(delta);
      moving.HorizontalWeight += dx;

      if (moving.HorizontalWeight > 1.0)
      {
	moving.HorizontalWeight = 1.0;
	delta *= -1;
      }

      if (moving.HorizontalWeight < 0.0)
      {
	moving.HorizontalWeight = 0.0;
	delta *= -1;
      }
      
      // Change the text
      moving.Text = moving.HorizontalWeight.ToString("#0.0000000");
    }
    #endregion

    #region Operators
    public override string ToString() { return "Font"; }
    #endregion
  }
}
