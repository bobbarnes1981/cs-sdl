/*
 * $RCSfile$
 * Copyright (C) 2004 David Hudson (jendave@yahoo.com)
 * Copyright (C) 2003 Will Weisser (ogl@9mm.com)
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
using Tao.Sdl;

namespace SdlDotNet 
{
	/// <summary>
	/// Represents a joystick on the system
	/// </summary>
	public class Joystick : BaseSdlResource 
	{
		private IntPtr handle;
		private bool disposed = false;

		internal Joystick(IntPtr handle) 
		{
			this.handle = handle;
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
					}
					CloseHandle(handle);
					GC.KeepAlive(this);
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
		/// Gets the 0-based numeric index of this joystick
		/// </summary>
		public int Index 
		{
			get 
			{ 
				int result = Sdl.SDL_JoystickIndex(handle); 
				GC.KeepAlive(this);
				return result;
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
	}
}
