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
	public class Joystick : IDisposable 
	{
		private IntPtr _handle;
		private bool _disposed;

		internal Joystick(IntPtr j) 
		{
			_handle = j;
			_disposed = false;
		}

		/// <summary>
		/// Allows an Object to attempt to free resources 
		/// and perform other cleanup operations before the Object 
		/// is reclaimed by garbage collection.
		/// </summary>
		~Joystick() 
		{
			Sdl.SDL_JoystickClose(_handle);
		}

		/// <summary>
		/// Destroys this joystick object
		/// </summary>
		public void Dispose() 
		{
			if (!_disposed) 
			{
				_disposed = true;
				Sdl.SDL_JoystickClose(_handle);
				GC.SuppressFinalize(this);
			}
		}

		/// <summary>
		/// Gets the 0-based numeric index of this joystick
		/// </summary>
		public int Index 
		{
			get 
			{ 
				return Sdl.SDL_JoystickIndex(_handle); 
			}
		}

		/// <summary>
		/// Gets the number of axes on this joystick (usually 2 for each stick handle)
		/// </summary>
		public int NumAxes 
		{
			get 
			{ 
				return Sdl.SDL_JoystickNumAxes(_handle); 
			}
		}

		/// <summary>
		/// Gets the number of trackballs on this joystick
		/// </summary>
		public int NumBalls 
		{
			get 
			{ 
				return Sdl.SDL_JoystickNumBalls(_handle); 
			}
		}

		/// <summary>
		/// Gets the number of hats on this joystick
		/// </summary>
		public int NumHats 
		{
			get 
			{ 
				return Sdl.SDL_JoystickNumHats(_handle); 
			}
		}

		/// <summary>
		/// Gets the number of buttons on this joystick
		/// </summary>
		public int NumButtons 
		{
			get 
			{ 
				return Sdl.SDL_JoystickNumButtons(_handle); 
			}
		}

		/// <summary>
		/// Destroys this joystick object
		/// </summary>
		public void Close() 
		{
			Dispose();
		}
	}
}
