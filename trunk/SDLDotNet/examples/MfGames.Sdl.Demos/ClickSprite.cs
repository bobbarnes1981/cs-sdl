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
using System;
using System.Drawing;

namespace MfGames.Sdl.Demos
{
	public class ClickSprite : AnimatedSprite
	{
		private IDrawable d1 = null;
		private IDrawable d2 = null;

		public ClickSprite(IDrawable d1, IDrawable d2, Point coordinates)
			: base(d1, coordinates)
		{
			this.d1 = d1;
			this.d2 = d2;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return String.Format("(click {0})", base.ToString());
		}

		#region Events
		/// <summary>
		/// 
		/// </summary>
		public override bool IsMouseSensitive { get { return true; } }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		public override void OnMouseButtonDown(object sender, MouseButtonEventArgs args)
		{
			// Switch the image
			if (Drawable == d1)
			{
				Drawable = d2;
			}
			else
			{
				Drawable = d1;
			}
		}
		#endregion
	}
}
