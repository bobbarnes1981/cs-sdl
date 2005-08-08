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
using System;
using System.Threading;

namespace SdlDotNet.Examples
{
	/// <summary>
	/// 
	/// </summary>
	public class StaticMode : DemoMode
	{
		/// <summary>
		/// Constructs the internal sprites needed for our demo.
		/// </summary>
		public StaticMode()
		{
			// Create our image and add it to our sprite manager
			SurfaceCollection id = new SurfaceCollection("../../Data/marble1.png");
			AnimatedSprite s = new AnimatedSprite(id, new Point(5, 5));
			this.Sprites.Add(s);

			// Create the fragment image
			SurfaceCollection td = new SurfaceCollection(new Surface("../../Data/marble1.png"), new Size(64, 64));
			AnimatedDemoSprite an = new AnimatedDemoSprite(td, new Vector(200, 32, 100));
			an.X = 250;
			Sprites.Add(an);

			// Create the full marble, but test order
			SurfaceCollection m1 = LoadMarble("marble1");

			for (int i = 0; i < 10; i++)
			{
				Thread.Sleep(10);
				AnimatedDemoSprite as1 = new AnimatedDemoSprite(m1,
					new Vector(50 + i * 32,
					436, i));
				Thread.Sleep(10);
				AnimatedDemoSprite as2 = new AnimatedDemoSprite(m1,
					new Vector(50 + i * 32,
					468, 10 - i));
				Sprites.Add(as1);
				Sprites.Add(as2);
			}
			Sprites.EnableTickEvent();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override string ToString() 
		{ 
			return "Static"; 
		}
	}
}
