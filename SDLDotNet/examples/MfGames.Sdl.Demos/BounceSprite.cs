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

namespace MfGames.Sdl.Demos
{
	public class BounceSprite : BoundedSprite
	{
		private int dx = Entropy.Next(-10, 10);
		private int dy = Entropy.Next(-10, 10);

		public BounceSprite(IDrawable d, Rectangle rect)
			: base(d, rect,
			new Vector(Entropy.Next(rect.Left, rect.Right
			- (int) d.Size.Width),
			Entropy.Next(rect.Top, rect.Bottom
			- (int) d.Size.Height),
			0))
		{
		}

		public BounceSprite(IDrawable d, Rectangle rect, int z)
			: base(d, rect,
			new Vector(Entropy.Next(rect.Left, rect.Right
			- (int) d.Size.Width),
			Entropy.Next(rect.Top, rect.Bottom
			- (int) d.Size.Height),
			z))
		{
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
			dx += Entropy.Next(-5, 5);
			dy += Entropy.Next(-5, 5);

			// Call the base which also normalizes the bounds
			base.OnTick(args);

			// Normalize the directions
			if (Coords.X == SpriteBounds.Left)
				dx = Entropy.Next(1, 10);

			if (Coords.X == SpriteBounds.Right)
				dx = Entropy.Next(-1, -10);

			if (Coords.Y == SpriteBounds.Top)
				dy = Entropy.Next(1, 10);

			if (Coords.Y == SpriteBounds.Bottom)
				dy = Entropy.Next(-1, -10);
		}
	}
}
