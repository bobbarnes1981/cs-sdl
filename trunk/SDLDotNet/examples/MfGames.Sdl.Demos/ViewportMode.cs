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

using MfGames.Sdl.Gui;
using SdlDotNet.Sprites;
using SdlDotNet;
using System.Drawing;
using System;

namespace MfGames.Sdl.Demos
{
	public class ViewportMode : DemoMode
	{
		private Sprite sprite = null;
		private Size size;
		private bool created = false;

		/// <summary>
		/// Constructs the internal sprites needed for our demo.
		/// </summary>
		public ViewportMode()
		{
			Random rand = new Random();
			// Create the fragment marbles
			IDrawable td = LoadMarble("marble1");
			IDrawable td2 = LoadMarble("marble2");

			// Load the floor
			int numberOfFloors = 4;
			IDrawable [] floorTiles = new IDrawable [4];

			for (int i = 0; i < numberOfFloors; i++)
			{
				floorTiles[i] = LoadFloor(i + 1);
			}

			// Place the floors
			int rows = 15;
			int cols = 25;
			size = new Size(floorTiles[0].Size.Width * cols,
				floorTiles[0].Size.Height * rows);
			Rectangle rect = new Rectangle(new Point(0, 0),size);

			for (int i = 0; i < cols; i++)
			{
				for (int j = 0; j < rows; j++)
				{
					// Create the sprite
					DrawableSprite dw =
						new DrawableSprite(floorTiles[rand.Next(0, numberOfFloors)],
						new Vector(i * floorTiles[0].Size.Width,
						j * floorTiles[0].Size.Height,
						-1000));
					sm.Add(dw);
				}
			}

			// Load the trigger sprite
			sprite = new BounceSprite(td2, rect, new Vector(rand.Next(rect.Left, rect.Right - 
				(int) td2.Size.Width),
				rand.Next(rect.Top, rect.Bottom - 
				(int) td2.Size.Height),
				100));
			sm.Add(sprite);
			OnMenuBounded(0);

			// Load the bouncing sprites
			for (int i = 0; i < 53; i++)
			{
				BounceSprite bounceSprite = 
					new BounceSprite(td,
					rect, 
					new Vector(rand.Next(rect.Left, rect.Right - 
					(int) td.Size.Width),
					rand.Next(rect.Top, rect.Bottom - 
					(int) td.Size.Height),
					0));
				sm.Add(bounceSprite);
			}

			created = true;
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

		public override string ToString() { return "Viewport"; }

		#region Events
		private void OnMenuNone(int index)
		{
			sm.Viewport = null;
		}

		private void OnMenuBounded(int index)
		{
			//Rectangle2 rect = new Rectangle2(new Vector2(0, 0), size);
			Rectangle rect = new Rectangle(new Point(0, 0), size);
			sm.Viewport = new BoundedCenterViewport(sprite, rect);
			if (created) 
			{
				SdlDemo.Report("center(" + rect + ")");
			}
		}

		private void OnMenuCentered(int index)
		{
			sm.Viewport = new CenteredViewport(sprite);
			if (created) 
			{
				SdlDemo.Report("center()");
			}
		}

		private void OnMenuTranslated(int index)
		{
			sm.Viewport = new TranslatedViewport(25, 25);
			if (created) 
			{
				SdlDemo.Report("translate(25, 25)");
			}
		}

		private void OnMenuTranslated0(int index)
		{
			sm.Viewport = new TranslatedViewport(0, 0);
			if (created) 
			{
				SdlDemo.Report("translate(0, 0)");
			}
		}
		#endregion
	}
}
