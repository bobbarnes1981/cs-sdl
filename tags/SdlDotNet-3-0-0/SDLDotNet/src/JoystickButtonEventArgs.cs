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

using Tao.Sdl;

namespace SdlDotNet
{
	/// <summary>
	/// Summary description for JoystickButtonEventArgs.
	/// </summary>
	public class JoystickButtonEventArgs : SdlEventArgs 
	{
		/// <summary>
		/// joystick button args
		/// </summary>
		/// <param name="device">The joystick index</param>
		/// <param name="button">The button index</param>
		/// <param name="buttonPressed">
		/// True if the button was pressed, 
		/// False if it was released
		/// </param>
		public JoystickButtonEventArgs(byte device, byte button, bool buttonPressed)
		{
			this.eventStruct = new Sdl.SDL_Event();
			this.eventStruct.jbutton.which = device;
			this.eventStruct.jbutton.button = button;
			if (buttonPressed)
			{
				this.eventStruct.jbutton.state = (byte)ButtonKeyState.Pressed;
				this.eventStruct.type = (byte)EventTypes.JoystickButtonDown;
			}
			else
			{
				this.eventStruct.jbutton.state = (byte)ButtonKeyState.NotPressed;
				this.eventStruct.type = (byte)EventTypes.JoystickButtonUp;
			}
		}

		internal JoystickButtonEventArgs(Sdl.SDL_Event ev)
		{
			this.eventStruct = ev;
		}

		/// <summary>
		/// joystick device
		/// </summary>
		public int Device
		{
			get
			{
				return this.eventStruct.jbutton.which;
			}
		}

		/// <summary>
		/// Button
		/// </summary>
		public int Button
		{
			get
			{
				return this.eventStruct.jbutton.button;
			}
		}

		/// <summary>
		/// Is button pressed
		/// </summary>
		public bool ButtonPressed
		{
			get
			{
				return (this.eventStruct.jbutton.state == (byte)ButtonKeyState.Pressed);
			}
		}
	}
}
