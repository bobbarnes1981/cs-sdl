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
	public class DragSprite : BoundedSprite
	{
		private IDrawable d1 = null;
		private IDrawable d2 = null;

		public DragSprite(IDrawable d1, IDrawable d2, Point coordinates,
			Rectangle bounds)
			: base(d1, bounds, new Vector(coordinates))
		{
			this.d1 = d1;
			this.d2 = d2;
		}

		public override string ToString()
		{
			return String.Format("(drag {0} {1})", beingDragged, base.ToString());
		}

		#region Events
		private bool beingDragged = false;

		public override bool IsMouseSensitive { get { return true; } }

		public override void OnMouseButtonDown(object sender, MouseButtonEventArgs args)
		{
			// If we are being held down, pick up the marble
				// Change the Z-order
				Coordinates.Z += 100;
				beingDragged = true;
				Drawable = d2;
				SdlDemo.MasterSpriteContainer.EventLock = this;		
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		public override void OnMouseButtonUp(object sender, MouseButtonEventArgs args)
		{
				Coordinates.Z -= 100;
				beingDragged = false;
				Drawable = d1;
				SdlDemo.MasterSpriteContainer.EventLock = null;		
		}

		/// <summary>
		/// If the sprite is picked up, this moved the sprite to follow
		/// the mouse.
		/// </summary>
		public override void OnMouseMotion(object sender, MouseMotionEventArgs args)
		{
			if (beingDragged)
			{
				Coordinates.X += args.RelativeX;
				Coordinates.Y += args.RelativeY; 
			}
			else
			{
				//return false;
			}
		}
		#endregion

		/// <summary>
		/// 
		/// </summary>
		public override bool IsMouseMotionLocked
		{
			get
			{
				return beingDragged;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public override bool IsMouseButtonLocked
		{
			get
			{
				return true;
			}
		}
	}

}
