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

using System.Drawing;

namespace SdlDotNet.Sprites
{
	/// <summary>
	/// The translated viewport just translates all sprites by a given amount.
	/// </summary>
	public class TranslatedViewport : IViewport
	{
		private int translateX = 0;
		private int translateY = 0;

		/// <summary>
		/// Constructs a viewport to center on a specific sprite.
		/// </summary>
		public TranslatedViewport(int translateX, int translateY)
		{
			this.translateX = translateX;
			this.translateY = translateY;
		}

		/// <summary>
		/// This gets the upper-left corner of the viewport, based on the
		/// given coordinates of the actual screen. This enables a
		/// viewport to only show in a specific part of the screen. The
		/// point returned is relative to the sprite manager.
		/// </summary>
		public void AdjustViewport(RenderArgs args)
		{
			args.TranslateX += this.translateX;
			args.TranslateY += this.translateY;
		}
	}
}
