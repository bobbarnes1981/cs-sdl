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


using SdlDotNet.Sprites;
using SdlDotNet;
using System.Drawing;

namespace MfGames.Sdl.Demos
{
	public class StaticMode : DemoMode
	{
		/// <summary>
		/// Constructs the internal sprites needed for our demo.
		/// </summary>
		public StaticMode()
		{
			// Create our image and add it to our sprite manager
			ImageDrawable id = new ImageDrawable("../../Data/marble1.png");
			DrawableSprite s = new DrawableSprite(id, new Point(5, 5));
			sm.Add(s);

			// Create the fragment image
			TiledDrawable td = new TiledDrawable(id, new Size(64, 64), 6, 6);
			AnimatedSprite an = new AnimatedSprite(td, new Vector(200, 32, 100));
			an.Coordinates.X = 250;
			sm.Add(an);

			// Create the full marble, but test order
			IDrawable m1 = LoadMarble("marble1");

			for (int i = 0; i < 10; i++)
			{
				AnimatedSprite as1 = new AnimatedSprite(m1,
					new Vector(50 + i * 32,
					436, i));
				AnimatedSprite as2 = new AnimatedSprite(m1,
					new Vector(50 + i * 32,
					468, 10 - i));
				sm.Add(as1);
				sm.Add(as2);
			}
		}

		public override string ToString() 
		{ 
			return "Static"; 
		}
	}
}
