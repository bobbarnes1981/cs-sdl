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
	public class Sprite : IComparable, ITickable
	{
		/// <summary>
		/// 
		/// </summary>
		public Sprite()
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="coords"></param>
		//public Sprite(Vector2 coords)
		public Sprite(Point coords)
		{
			Coords = new Vector(coords, 0);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="coords"></param>
		/// <param name="z"></param>
//		public Sprite(Vector2 coords, int z)
		public Sprite(Point coords, int z)
		{
			Coords = new Vector(coords, z);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="coords"></param>
		public Sprite(Vector coords)
		{
			Coords = coords;
		}

		#region Display
		/// <summary>
		/// 
		/// </summary>
		/// <param name="args"></param>
		public virtual void Render(RenderArgs args)
		{
			// Do nothing
		}
		#endregion

		#region Events
		private bool tickable = false;

		/// <summary>
		/// 
		/// </summary>
		public virtual bool IsMouseSensitive { get { return false; } }

		/// <summary>
		/// 
		/// </summary>
		public virtual bool IsKeyboardSensitive { get { return false; } }

		/// <summary>
		/// 
		/// </summary>
		public virtual bool IsTickable
		{
			get { return tickable; }
			set { tickable = value; }
		}

		/// <summary>
		/// Processes the keyboard. If this function returns true, then
		/// the system no longer processes the keyboard event. If it is
		/// not true, then the next sprite that is keyboard sensitive is
		/// processed.
		/// </summary>
		public virtual bool OnKeyboard(object sender, KeyboardEventArgs e)
		{
			return false;
		}

		/// <summary>
		/// Processes a mouse button. This event is trigger by the SDL
		/// system. If this function returns true, then processing is
		/// stopped. If it returns false, then the next sprite is
		/// processed.
		/// </summary>
		public virtual bool OnMouseButton(object sender, MouseArgs args)
		{
			return false;
		}

		/// <summary>
		/// Processes a mouse motion event. This event is triggered by
		/// SDL. If this returns true, then processing of the motion event
		/// is stopped, otherwise the next sprite is processed. Only
		/// sprites that are MouseSensitive are processed.
		/// </summary>
		public virtual bool OnMouseMotion(object sender, MouseArgs args)
		{
			return false;
		}

		/// <summary>
		/// All sprites are tickable, regardless if they actual do
		/// anything. This ensures that the functionality is there, to be
		/// overriden as needed.
		/// </summary>
		public virtual void OnTick(TickArgs args)
		{
			// Do nothing
		}
		#endregion

		#region Geometry
		private Vector coords = new Vector();

		private Size size;

		/// <summary>
		/// 
		/// </summary>
		public Rectangle Bounds
		{
			get { return new Rectangle(Coords.X, Coords.Y, Size.Width, Size.Height); }
		}

		/// <summary>
		/// 
		/// </summary>
		public virtual Vector Coords
		{
			get { return coords; }
			set
			{
//				if (value == null)
//					throw new SpriteException("Cannot assign a null coordinates");

				coords = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public virtual Size Size
		{
			get { return size; }
			set { size = value; }
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="point"></param>
		/// <returns></returns>
//		public virtual bool IntersectsWith(Vector2 point)
		public virtual bool IntersectsWith(Point point)
		{
			return Bounds.IntersectsWith(new Rectangle(point, new Size(0, 0)));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="rect"></param>
		/// <returns></returns>
		public virtual bool IntersectsWith(Rectangle rect)
		{
			return Bounds.IntersectsWith(rect);
		}
		#endregion

		#region Operators
		/// <summary>
		/// 
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public int CompareTo(object obj)
		{
			// Cast the resulting object into a sprite, since that is the
			// only thing comparable to this sprite.
			Sprite s = (Sprite) obj;

			// Compare the Z-Order first
			int res = Coords.Z.CompareTo(s.Coords.Z);

			if (res != 0)
				return res;

			// Compare the hashes
			return GetHashCode().CompareTo(s.GetHashCode());
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return Bounds.ToString();
		}
		#endregion

		#region Properties
		private bool hidden = false;
		private bool debug = false;

		/// <summary>
		/// 
		/// </summary>
		public virtual bool IsHidden
		{
			get { return hidden; }
			set { hidden = value; }
		}

		/// <summary>
		/// 
		/// </summary>
		public virtual bool IsTraced
		{
			get { return debug; }
			set { debug = value; }
		}
		#endregion
	}
}
