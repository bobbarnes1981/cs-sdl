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

namespace SdlDotNet {
	/// <summary>
	/// Provides methods for querying the number and make-up of the joysticks on a system.
	/// You can obtain an instance of this class by accessing the Joysticks property of the main Sdl object.
	/// Note that actual joystick input is handled by the Events class
	/// </summary>
	public sealed class Joysticks 
	{
		static readonly Joysticks instance = new Joysticks();

		Joysticks()
		{
		}

		public static Joysticks Instance 
		{
			get
			{
				if (Sdl.SDL_Init(Sdl.SDL_INIT_JOYSTICK) != 0)
				{
					throw SdlException.Generate();
				}
				return instance;
			}
		}

		/// <summary>
		/// Returns the number of joysticks on this system
		/// </summary>
		/// <returns>The number of joysticks</returns>
		public int NumJoysticks() 
		{
			return Sdl.SDL_NumJoysticks();
		}

		/// <summary>
		/// Creates a joystick object to read information about a joystick
		/// </summary>
		/// <param name="index">The 0-based index of the joystick to read</param>
		/// <returns>A Joystick object</returns>
		public Joystick OpenJoystick(int index) 
		{
			IntPtr joy = Sdl.SDL_JoystickOpen(index);
			if (joy == IntPtr.Zero)
			{
				throw SdlException.Generate();
			}
			return new Joystick(joy);
		}
	}
}
