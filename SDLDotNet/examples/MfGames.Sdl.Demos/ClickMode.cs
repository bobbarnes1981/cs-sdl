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

using MfGames.Utility;
using MfGames.Sdl.Drawable;
using MfGames.Sdl.Sprites;
using System.Drawing;

namespace MfGames.Sdl.Demos
{
  public class ClickMode : DemoMode
  {
    /// <summary>
    /// Constructs the internal sprites needed for our demo.
    /// </summary>
    public ClickMode()
    {
      // Create the fragment marbles
      int rows = 6;
      int cols = 6;
      int sx = (800 - cols * 64) / 2;
      int sy = (600 - rows * 64) / 2;
      IDrawable m1 = LoadMarble("marble1");
      IDrawable m2 = LoadMarble("marble2");

      for (int i = 0; i < cols; i++)
      {
	for (int j = 0; j < rows; j++)
	{
	  sm.Add(new ClickSprite(m1, m2,
				 new Vector2(sx + i * 64, sy + j * 64)));
	}
      }
    }

    /// <summary>
    /// Adds the internal sprite manager to the outer one.
    /// </summary>
    public override void Start(SpriteContainer manager)
    {
      manager.Add(sm);
    }

    /// <summary>
    /// Removes the internal manager from the controlling manager.
    /// </summary>
    public override void Stop(SpriteContainer manager)
    {
      manager.Remove(sm);
    }

    public override string ToString() { return "Click"; }
  }
}
