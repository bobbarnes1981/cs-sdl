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
using MfGames.Sdl.Gui;
using SdlDotNet.Sprites;
using System.Drawing;

namespace MfGames.Sdl.Demos
{
	public class MultipleMode : DemoMode
	{
		private Sprite sprite1 = null;
		private Sprite sprite2 = null;
		private Sprite sprite3 = null;
		private Sprite sprite4 = null;

		private Size size;
		//private bool created = false;

		private SpriteContainer sm1 = new SpriteContainer();
		private SpriteContainer sm2 = new SpriteContainer();
		private SpriteContainer sm3 = new SpriteContainer();
		private SpriteContainer sm4 = new SpriteContainer();
		private SpriteContainer all = new SpriteContainer();

		/// <summary>
		/// Constructs the internal sprites needed for our demo.
		/// </summary>
		public MultipleMode()
		{
			// Create the fragment marbles
			IDrawable td = LoadMarble("marble1");
			IDrawable td2 = LoadMarble("marble2");
			IDrawable td3 = LoadMarble("marble3");
			IDrawable td4 = LoadMarble("marble4");
			IDrawable td5 = LoadMarble("marble5");

			// Load the floor
			int numberOfFloors = 4;
			IDrawable [] floorTiles = new IDrawable [4];

			for (int i = 0; i < numberOfFloors; i++)
				floorTiles[i] = LoadFloor(i + 1);

			// Place the floors
			int rows = 10;
			int cols = 10;
			size = new Size(floorTiles[0].Size.Width * cols,
				floorTiles[0].Size.Height * rows);
			Rectangle rect = new Rectangle(new Point(0, 0), size);

			for (int i = 0; i < cols; i++)
			{
				for (int j = 0; j < rows; j++)
				{
					// Create the sprite
					DrawableSprite dw =
						new DrawableSprite(floorTiles[Entropy.Next(0, numberOfFloors -1)],
						new Vector(i * floorTiles[0].Size.Width,
						j * floorTiles[0].Size.Height,
						-1000));
					all.Add(dw);
				}
			}

			// Load the bouncing sprites
			for (int i = 0; i < 15; i++)
			{
				all.Add(new BounceSprite(td, rect));
			}

			// Only one container may be tickable when they all talk to the
			// same inner tick manager.

			// Set up container #1
			sprite1 = new BounceSprite(td2, rect, 100);
			sm1.Coords = new Vector(10, 10);
			sm1.IsTickable = true;
			sm1.Size = new Size(380, 250);
			sm1.Viewport = new BoundedCenterViewport(sprite1, rect);
			all.Add(sprite1);

			// Set up container #2
			sprite2 = new BounceSprite(td3, rect, 100);
			sm2.Coords = new Vector(410, 10);
			sm2.IsTickable = false;
			sm2.Size = new Size(380, 250);
			sm2.Viewport = new BoundedCenterViewport(sprite2, rect);
			all.Add(sprite2);

			// Set up container #3
			sprite3 = new BounceSprite(td4, rect, 100);
			sm3.Coords = new Vector(10, 280);
			sm3.IsTickable = false;
			sm3.Size = new Size(380, 250);
			sm3.Viewport = new BoundedCenterViewport(sprite3, rect);
			all.Add(sprite3);
      
			// Set up container #4
			sprite4 = new BounceSprite(td5, rect, 100);
			sm4.Coords = new Vector(410, 280);
			sm4.IsTickable = false;
			sm4.Size = new Size(380, 250);
			sm4.Viewport = new BoundedCenterViewport(sprite4, rect);
			all.Add(sprite4);

			// Add the managers
			sm1.Add(all);
			sm2.Add(all);
			sm3.Add(all);
			sm4.Add(all);
			sm.Add(sm1);
			sm.Add(sm2);
			sm.Add(sm3);
			sm.Add(sm4);
		}

		/// <summary>
		/// Adds the internal sprite manager to the outer one.
		/// </summary>
		public override void Start(SpriteContainer manager)
		{
			base.Start(manager);
		}

		/// <summary>
		/// Removes the internal manager from the controlling manager.
		/// </summary>
		public override void Stop(SpriteContainer manager)
		{
			base.Stop(manager);
		}

		public override string ToString() { return "Multiple"; }
	}
}
