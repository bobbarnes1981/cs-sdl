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
using System.Drawing;

namespace SdlDotNet.Sprites
{
	/// <summary>
	/// 
	/// </summary>
	public class SpriteProxy : Sprite
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sprite"></param>
		public SpriteProxy(Sprite sprite)
		{
			Sprite = sprite;
		}

		#region Display
		/// <summary>
		/// 
		/// </summary>
		/// <param name="args"></param>
		public override void Render(RenderArgs args)
		{
			sprite.Render(args);
		}
		#endregion

		#region Events
		/// <summary>
		/// 
		/// </summary>
		public override bool IsMouseSensitive
		{
			get { return sprite.IsMouseSensitive; }
		}

		/// <summary>
		/// 
		/// </summary>
		public override bool IsKeyboardSensitive
		{
			get { return sprite.IsKeyboardSensitive; }
		}

		/// <summary>
		/// 
		/// </summary>
		public override bool IsTickable
		{
			get { return sprite.IsTickable; }
			set { sprite.IsTickable = value; }
		}

		/// <summary>
		/// Processes the keyboard. If this function returns true, then
		/// the system no longer processes the keyboard event. If it is
		/// not true, then the next sprite that is keyboard sensitive is
		/// processed.
		/// </summary>
		public override bool OnKeyboard(object sender, KeyboardEventArgs e)
		{
			return sprite.OnKeyboard(this, e);
		}

		/// <summary>
		/// Processes a mouse button. This event is trigger by the SDL
		/// system. If this function returns true, then processing is
		/// stopped. If it returns false, then the next sprite is
		/// processed.
		/// </summary>
		public override bool OnMouseButton(object sender, MouseArgs args)
		{
			return sprite.OnMouseButton(this, args);
		}

		/// <summary>
		/// Processes a mouse motion event. This event is triggered by
		/// SDL. If this returns true, then processing of the motion event
		/// is stopped, otherwise the next sprite is processed. Only
		/// sprites that are MouseSensitive are processed.
		/// </summary>
		public override bool OnMouseMotion(object sender, MouseArgs args)
		{
			return sprite.OnMouseMotion(this, args);
		}

		/// <summary>
		/// All sprites are tickable, regardless if they actual do
		/// anything. This ensures that the functionality is there, to be
		/// overriden as needed.
		/// </summary>
		public override void OnTick(TickArgs args)
		{
			sprite.OnTick(args);
		}
		#endregion

		#region Geometry
		/// <summary>
		/// 
		/// </summary>
		public override Vector Coords
		{
			get { return sprite.Coords; }
			set { sprite.Coords = value; }
		}

		/// <summary>
		/// 
		/// </summary>
		public override Size Size
		{
			get { return sprite.Size; }
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="point"></param>
		/// <returns></returns>
		public override bool IntersectsWith(Point point)
		{
			return Bounds.IntersectsWith(new Rectangle(point, new Size(0, 0)));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="rect"></param>
		/// <returns></returns>
		public override bool IntersectsWith(Rectangle rect)
		{
			return Bounds.IntersectsWith(rect);
		}
		#endregion

		#region Properties
		private Sprite sprite = null;

		/// <summary>
		/// 
		/// </summary>
		public Sprite Sprite
		{
			get { return sprite; }
			set
			{
				if (value == null)
					throw new SpriteException("Cannot assign a null sprite");

				sprite = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public override bool IsHidden
		{
			get { return sprite.IsHidden; }
			set { sprite.IsHidden = value; }
		}

		/// <summary>
		/// 
		/// </summary>
		public override bool IsTraced
		{
			get { return sprite.IsTraced; }
			set { sprite.IsTraced = value; }
		}
		#endregion
	}
}
