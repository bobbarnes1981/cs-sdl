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
		/// <param name="coordinates"></param>
		public Sprite(Point coordinates)
		{
			this.coordinates = new Vector(coordinates, 0);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="coordinates"></param>
		/// <param name="z"></param>
		public Sprite(Point coordinates, int z)
		{
			this.coordinates = new Vector(coordinates, z);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="coordinates"></param>
		public Sprite(Vector coordinates)
		{
			this.coordinates = coordinates;
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
		public virtual bool IsMouseSensitive 
		{ 
			get 
			{ 
				return false; 
			} 
		}

		/// <summary>
		/// 
		/// </summary>
		public virtual bool IsKeyboardSensitive 
		{ 
			get 
			{ 
				return false; 
			} 
		}

		/// <summary>
		/// 
		/// </summary>
		public virtual bool IsJoystickSensitive 
		{ 
			get 
			{ 
				return false; 
			} 
		}

		/// <summary>
		/// 
		/// </summary>
		public virtual bool IsTickable
		{
			get 
			{ 
				return tickable; 
			}
			set 
			{ 
				tickable = value; 
			}
		}

		/// <summary>
		/// Processes the keyboard.
		/// </summary>
		public virtual void OnKeyboard(object sender, KeyboardEventArgs e)
		{
		}

		/// <summary>
		/// Processes the keyboard.
		/// </summary>
		public virtual void OnKeyboardDown(object sender, KeyboardEventArgs e)
		{
		}

		/// <summary>
		/// Processes the keyboard.
		/// </summary>
		public virtual void OnKeyboardUp(object sender, KeyboardEventArgs e)
		{
		}

		/// <summary>
		/// Processes a mouse button. This event is trigger by the SDL
		/// system. 
		/// </summary>
		public virtual void OnMouseButtonDown(object sender, MouseButtonEventArgs args)
		{
		}

		/// <summary>
		/// Processes a mouse button. This event is trigger by the SDL
		/// system.
		/// </summary>
		public virtual void OnMouseButtonUp(object sender, MouseButtonEventArgs args)
		{
		}

		/// <summary>
		/// Processes a mouse button. This event is trigger by the SDL
		/// system.
		/// </summary>
		public virtual void OnMouseButton(object sender, MouseButtonEventArgs args)
		{
		}

		/// <summary>
		/// Processes a mouse motion event. This event is triggered by
		/// SDL. Only
		/// sprites that are MouseSensitive are processed.
		/// </summary>
		public virtual void OnMouseMotion(object sender, MouseMotionEventArgs args)
		{
		}

		/// <summary>
		/// Processes a joystick motion event. This event is triggered by
		/// SDL. Only
		/// sprites that are JoystickSensitive are processed.
		/// </summary>
		public virtual void OnJoystickAxisMotion(object sender, JoystickAxisEventArgs args)
		{
		}

		/// <summary>
		/// Processes a joystick motion event. This event is triggered by
		/// SDL. Only
		/// sprites that are JoystickSensitive are processed.
		/// </summary>
		public virtual void OnJoystickVerticalAxisMotion(object sender, JoystickAxisEventArgs args)
		{
		}

		/// <summary>
		/// Processes a joystick motion event. This event is triggered by
		/// SDL. Only
		/// sprites that are JoystickSensitive are processed.
		/// </summary>
		public virtual void OnJoystickHorizontalAxisMotion(object sender, JoystickAxisEventArgs args)
		{
		}

		/// <summary>
		/// Processes a joystick button event. This event is triggered by
		/// SDL. Only
		/// sprites that are JoystickSensitive are processed.
		/// </summary>
		public virtual void OnJoystickButton(object sender, JoystickButtonEventArgs args)
		{
		}

		/// <summary>
		/// Processes a joystick button event. This event is triggered by
		/// SDL. Only
		/// sprites that are JoystickSensitive are processed.
		/// </summary>
		public virtual void OnJoystickButtonDown(object sender, JoystickButtonEventArgs args)
		{
		}

		/// <summary>
		/// Processes a joystick button event. This event is triggered by
		/// SDL. Only
		/// sprites that are JoystickSensitive are processed.
		/// </summary>
		public virtual void OnJoystickButtonUp(object sender, JoystickButtonEventArgs args)
		{
		}

		/// <summary>
		/// Processes a joystick hat motion event. This event is triggered by
		/// SDL. Only
		/// sprites that are JoystickSensitive are processed.
		/// </summary>
		public virtual void OnJoystickHatMotion(object sender, JoystickHatEventArgs args)
		{
		}

		/// <summary>
		/// Processes a joystick hat motion event. This event is triggered by
		/// SDL. Only
		/// sprites that are JoystickSensitive are processed.
		/// </summary>
		public virtual void OnJoystickBallMotion(object sender, JoystickBallEventArgs args)
		{
		}

		/// <summary>
		/// All sprites are tickable, regardless if they actual do
		/// anything. This ensures that the functionality is there, to be
		/// overridden as needed.
		/// </summary>
		public virtual void OnTick(object sender, TickEventArgs args)
		{
		}
		#endregion

		#region Geometry
		private Vector coordinates = new Vector();

		private Size size;

		/// <summary>
		/// 
		/// </summary>
		public Rectangle Bounds
		{
			get { return new Rectangle(Coordinates.X, Coordinates.Y, Size.Width, Size.Height); }
		}

		/// <summary>
		/// 
		/// </summary>
		public virtual Vector Coordinates
		{
			get 
			{ 
				return coordinates; 
			}
			set
			{
				coordinates = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public virtual Size Size
		{
			get 
			{ 
				return size; 
			}
			set 
			{ 
				size = value; 
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="point"></param>
		/// <returns></returns>
		public virtual bool IntersectsWith(Point point)
		{
			return Bounds.IntersectsWith(new Rectangle(point, new Size(0, 0)));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="rectangle"></param>
		/// <returns></returns>
		public virtual bool IntersectsWith(Rectangle rectangle)
		{
			return Bounds.IntersectsWith(rectangle);
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
			int res = this.coordinates.Z.CompareTo(s.Coordinates.Z);

			if (res != 0)
			{
				return res;
			}

			// Compare the hashes
			return GetHashCode().CompareTo(s.GetHashCode());
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals (Object obj)
		{
			if (!(obj is Sprite))
			{
				return false;
			}
			return (this.CompareTo(obj) == 0);
		}  
//
//		/// <summary>
//		/// 
//		/// </summary>
//		/// <returns></returns>
//		public override int GetHashCode()
//		{
//			return this.coordinates.X ^ this.coordinates.Y ^ this.coordinates.Z;
//		}  
//
//		/// <summary>
//		/// 
//		/// </summary>
//		/// <param name="sprite1"></param>
//		/// <param name="sprite2"></param>
//		/// <returns></returns>
//		public static bool operator == (Sprite sprite1, Sprite sprite2)
//		{
//			return sprite1.Equals(sprite2);
//		}  
//
//		/// <summary>
//		/// 
//		/// </summary>
//		/// <param name="sprite1"></param>
//		/// <param name="sprite2"></param>
//		/// <returns></returns>
//		public static bool operator != (Sprite sprite1, Sprite sprite2)
//		{
//			return !(sprite1==sprite2);
//		}  
//
//		/// <summary>
//		/// 
//		/// </summary>
//		/// <param name="sprite1"></param>
//		/// <param name="sprite2"></param>
//		/// <returns></returns>
//		public static bool operator < (Sprite sprite1, Sprite sprite2)
//		{
//			return (sprite1.CompareTo(sprite2) < 0);
//		}  
//
//		/// <summary>
//		/// 
//		/// </summary>
//		/// <param name="sprite1"></param>
//		/// <param name="sprite2"></param>
//		/// <returns></returns>
//		public static bool operator > (Sprite sprite1, Sprite sprite2)
//		{
//			return (sprite1.CompareTo(sprite2) > 0);
//		} 



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
		private bool mouseMotionLocked = false;
		private bool mouseButtonLocked = false;

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

		/// <summary>
		/// 
		/// </summary>
		public virtual bool IsMouseButtonLocked
		{
			get
			{
				return mouseButtonLocked;
			}
			set
			{
				mouseButtonLocked = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public virtual bool IsMouseMotionLocked
		{
			get
			{
				return mouseMotionLocked;
			}
			set
			{
				mouseMotionLocked = value;
			}
		}

		#endregion
	}
}
