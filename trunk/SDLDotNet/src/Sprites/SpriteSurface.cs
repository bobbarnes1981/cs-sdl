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
using SdlDotNet;
using System;
using System.Collections;
using System.Drawing;

namespace SdlDotNet.Sprites
{
	/// <summary>
	/// 
	/// </summary>
	public class SpriteSurface
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="screen"></param>
		/// <param name="manager"></param>
		public SpriteSurface(Surface screen, SpriteContainer manager)
		{
			// Sanity checking
			if (screen == null)
				throw new Exception("Cannot create a sprite surface with a null "
					+ "screen");

			if (manager == null)
				throw new Exception("Cannot create a sprite surface with a null "
					+ "sprite manager");

			// Save our fields
			this.screen = screen;
			this.manager = manager;

			// Create a compatiable surface
			surface = screen.CreateCompatibleSurface(screen.Width,
				screen.Height,
				true);
			surface.Fill(new Rectangle(new Point(0, 0), surface.Size),
				Color.Black);
		}

		#region SDL
		/// <summary>
		/// This is the primary function of the sprite surface, to get
		/// all sprites in a specific viewable region and display them to
		/// a screen. This draws the sprites in the Z-order, then flips
		/// the screen to prevent any flicker.
		/// </summary>
		public void Blit()
		{
			// Clear the screen
			surface.Fill(new Rectangle(new Point(0, 0), surface.Size),
				Color.Black);

			// Create the rendering args
			RenderArgs args = new RenderArgs(surface, surface.Size);
			manager.Render(args);

			// Draw ourselves on the screen
			screen.Blit(surface, new Rectangle(new Point(0, 0), screen.Size));
			screen.Flip();
		}
		#endregion

		#region Properties
		private SpriteContainer manager = null;

		private Surface screen = null;

		private Surface surface = null;
		#endregion
	}
}
