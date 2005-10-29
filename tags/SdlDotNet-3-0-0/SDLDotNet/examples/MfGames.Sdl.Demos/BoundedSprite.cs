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

namespace MfGames.Sdl.Demos
{
  public class BoundedSprite : AnimatedSprite
  {
    private Rectangle2 rect = new Rectangle2();

    public BoundedSprite(IDrawable d, Rectangle2 bounds, Vector3 coords)
      : base(d, coords)
    {
      this.rect = new Rectangle2(bounds);
      this.rect.Size.Width -= (int) d.Size.Width;
      this.rect.Size.Height -= (int) d.Size.Height;
    }

    public Rectangle2 SpriteBounds
    {
      get { return rect; }
    }

    public override void OnTick(TickArgs args)
    {
      // Animate
      base.OnTick(args);

      // Bounce off the left
      if (Coords.X < rect.Left)
	Coords.X = rect.Left;

      // Bounce off the top
      if (Coords.Y < rect.Top)
	Coords.Y = rect.Top;

      // Bounce off the bottom
      if (Coords.Y > rect.Bottom)
	Coords.Y = rect.Bottom;

      // Bounce off the right
      if (Coords.X > rect.Right)
	Coords.X = rect.Right;
    }
  }
}