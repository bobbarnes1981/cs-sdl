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
using System.Drawing;

namespace SdlDotNet.Sprites
{
	/// <summary>
	/// The centered viewport centers the viewport on the given sprite.
	/// </summary>
	public class CenteredViewport : IViewport
	{
		// The sprite the follow
		private Sprite sprite = null;

		/// <summary>
		/// Constructs a viewport to center on a specific sprite.
		/// </summary>
		public CenteredViewport(Sprite centerOnSprite)
		{
			sprite = centerOnSprite;
		}

		/// <summary>
		/// 
		/// </summary>
		public Sprite Sprite
		{
			get { return sprite; }
			set
			{
				if (value == null)
					throw new SpriteException("Cannot assign null sprite");

				sprite = value;
			}
		}

		/// <summary>
		/// This gets the upper-left corner of the viewport, based on the
		/// given coordinates of the actual screen. This enables a
		/// viewport to only show in a specific part of the screen. The
		/// point returned is relative to the sprite manager.
		/// </summary>
		public virtual void AdjustViewport(ref RenderArgs args)
		{
			// Get the midpoint of the surface
			int mx = GetMidX(args);
			int my = GetMidY(args);

			// Return the offset point
			args.TranslateX += mx - sprite.Coords.X;
			args.TranslateY += my - sprite.Coords.Y;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="args"></param>
		/// <returns></returns>
		public int GetMidX(RenderArgs args)
		{
			return args.Size.Width / 2 - sprite.Size.Width / 2;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="args"></param>
		/// <returns></returns>
		public int GetMidY(RenderArgs args)
		{
			return args.Size.Height / 2 - sprite.Size.Height / 2;
		}
	}
}
