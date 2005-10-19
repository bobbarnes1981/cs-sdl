/*
 * $RCSfile$
 * Copyright (C) 2004, 2005 David Hudson (jendave@yahoo.com)
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

using System;
using System.Globalization;

using Tao.Sdl;

namespace SdlDotNet 
{
	/// <summary>
	/// Struct for trackball motion
	/// </summary>
	public struct BallMotion
	{
		int x;
		int y;

		/// <summary>
		/// Ball motion
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		public BallMotion(int x, int y)
		{
			this.x = x;
			this.y = y;
		}

		/// <summary>
		/// motion in X-axis
		/// </summary>
		public int MotionX
		{
			get
			{
				return this.x;
			}
			set
			{
				this.x = value;
			}
		}

		/// <summary>
		/// Motion in Y-axis
		/// </summary>
		public int MotionY
		{
			get
			{
				return this.y;
			}
			set
			{
				this.y = value;
			}
		}

		/// <summary>
		/// String output
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return String.Format(CultureInfo.CurrentCulture, "({0},{1}, {2})", x, y);
		}
		/// <summary>
		/// Equals
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			if (obj.GetType() != typeof(BallMotion))
				return false;
                
			BallMotion c = (BallMotion)obj;   
			return ((this.x == c.x) && (this.y == c.y));
		}

		/// <summary>
		/// Equals operator
		/// </summary>
		/// <param name="c1"></param>
		/// <param name="c2"></param>
		/// <returns></returns>
		public static bool operator== (BallMotion c1, BallMotion c2)
		{
			return ((c1.x == c2.x) && (c1.y == c2.y));
		}
		
		/// <summary>
		/// no equals operator
		/// </summary>
		/// <param name="c1"></param>
		/// <param name="c2"></param>
		/// <returns></returns>
		public static bool operator!= (BallMotion c1, BallMotion c2)
		{
			return !(c1 == c2);
		}

		/// <summary>
		/// GetHashCode openrator
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode()
		{
			return x ^ y;

		}
	}
	/// <summary>
	/// Represents a joystick on the system
	/// </summary>
	public class Joystick : BaseSdlResource 
	{
		private IntPtr handle;
		private int index;
		private bool disposed = false;

		/// <summary>
		/// open joystick at index number
		/// </summary>
		/// <param name="index"></param>
		public Joystick(int index)
		{
			if (!Joysticks.IsInitialized)
			{
				Joysticks.Initialize();
			}
			if (Joysticks.IsValidJoystickNumber(index))
			{
				this.handle = Sdl.SDL_JoystickOpen(index);
			}
			if (this.handle == IntPtr.Zero)
			{
				throw SdlException.Generate();
			}
			else
			{
				this.index = index;
			}
		}
		internal Joystick(IntPtr handle) 
		{
			this.handle = handle;
			this.index = Sdl.SDL_JoystickIndex(handle); 
		}

		/// <summary>
		/// Destroys the surface object and frees its memory
		/// </summary>
		/// <param name="disposing"></param>
		protected override void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				try
				{
					if (disposing)
					{
						CloseHandle(handle);
						GC.KeepAlive(this);
					}
					this.disposed = true;
				}
				finally
				{
					base.Dispose(disposing);
				}
			}
		}

		/// <summary>
		/// Closes Joystick handle
		/// </summary>
		protected override void CloseHandle(IntPtr handleToClose) 
		{
			Sdl.SDL_JoystickClose(handleToClose);
			GC.KeepAlive(this);
			handleToClose = IntPtr.Zero;
		}
		

		/// <summary>
		/// Returns true if the joystick has been initialized
		/// </summary>
		public bool IsInitialized
		{
			get
			{
				if (Sdl.SDL_JoystickOpened(this.index) == (int) SdlFlag.TrueValue)
				{
					return true;
				}
				else
				{
					return false;
				}
			}
		}
		/// <summary>
		/// Gets the 0-based numeric index of this joystick
		/// </summary>
		public int Index 
		{
			get 
			{ 
				return this.index;
			}
		}

		/// <summary>
		/// Gets the number of axes on this joystick (usually 2 for each stick handle)
		/// </summary>
		public int NumberOfAxes 
		{
			get 
			{ 
				int result = Sdl.SDL_JoystickNumAxes(handle); 
				GC.KeepAlive(this);
				return result;
			}
		}

		/// <summary>
		/// Gets the number of trackballs on this joystick
		/// </summary>
		public int NumberOfBalls 
		{
			get 
			{ 
				int result = Sdl.SDL_JoystickNumBalls(handle); 
				GC.KeepAlive(this);
				return result;
			}
		}

		/// <summary>
		/// Gets the number of hats on this joystick
		/// </summary>
		public int NumberOfHats 
		{
			get 
			{ 
				int result = Sdl.SDL_JoystickNumHats(handle); 
				GC.KeepAlive(this);
				return result;
			}
		}

		/// <summary>
		/// Gets the number of buttons on this joystick
		/// </summary>
		public int NumberOfButtons 
		{
			get 
			{ 
				int result = Sdl.SDL_JoystickNumButtons(handle); 
				GC.KeepAlive(this);
				return result;
			}
		}

		/// <summary>
		/// Gets the name of this joystick
		/// </summary>
		public string Name 
		{
			get 
			{ 
				string result = Sdl.SDL_JoystickName(this.Index); 
				GC.KeepAlive(this);
				return result;
			}
		}

		/// <summary>
		/// Gets the current axis position
		/// </summary>
		/// <param name="axis"></param>
		/// <returns></returns>
		public short GetAxisPosition(JoystickAxes axis)
		{
			return Sdl.SDL_JoystickGetAxis(this.handle, (int) axis);
		}

		/// <summary>
		/// Gets the ball motion
		/// </summary>
		/// <param name="ball"></param>
		/// <returns></returns>
		public BallMotion GetBallMotion(int ball)
		{
			int motionX;
			int motionY;

			if (Sdl.SDL_JoystickGetBall(
				this.handle, ball, out motionX, out motionY) == 
				(int) SdlFlag.Success)
			{
				return new BallMotion(motionX, motionY);
			}
			else
			{
				throw new SdlException();
			}
		}

		/// <summary>
		/// Gets the current button state
		/// </summary>
		/// <param name="button"></param>
		/// <returns></returns>
		public ButtonKeyState GetButtonState(int button)
		{
			return (ButtonKeyState) Sdl.SDL_JoystickGetButton(this.handle, button);
		}

		/// <summary>
		/// Gets the current Hat state
		/// </summary>
		/// <param name="hat"></param>
		/// <returns></returns>
		public JoystickHatStates GetHatState(int hat)
		{
			return (JoystickHatStates) Sdl.SDL_JoystickGetHat(this.handle, (int) hat);
		}
	}
}