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

using SdlDotNet.Utility;
using SdlDotNet.Drawable;
using System.Drawing;
using System;

namespace MfGames.Sdl.Demos
{
	public class BounceSprite : BoundedSprite
	{
		Random rand = new Random();
		private int dx;
		private int dy;

		public BounceSprite(IDrawable d, Rectangle rect, Vector coords)
			: base(d, rect, coords)
		{
			this.dx = rand.Next(-10, 11);
			this.dy = rand.Next(-10, 11);
		}

		public override bool IsTickable { get { return true; } }

		public override void OnTick(TickArgs args)
		{
			// Move our direction a little
			int x = Coords.X;
			int y = Coords.Y;

			Coords.X += args.RatePerSecond(dx);
			Coords.Y += args.RatePerSecond(dy);

			// Adjust our entropy
			dx += rand.Next(-5, 6);
			dy += rand.Next(-5, 6);

			// Call the base which also normalizes the bounds
			base.OnTick(args);

			// Normalize the directions
			if (Coords.X == SpriteBounds.Left)
			{
				dx = rand.Next(1, 10);
			}

			if (Coords.X == SpriteBounds.Right)
			{
				dx = ((-1) * rand.Next(1, 10));
			}

			if (Coords.Y == SpriteBounds.Top)
			{
				dy = rand.Next(1, 10);
			}

			if (Coords.Y == SpriteBounds.Bottom)
			{
				dy = ((-1) * rand.Next(1, 10));
			}
		}
	}
}
