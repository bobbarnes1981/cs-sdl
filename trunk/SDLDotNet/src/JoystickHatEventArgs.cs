/*
 * $RCSfile$
 * Copyright (C) 2004 David Hudson (jendave@yahoo.com)
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
	/// Summary description for JoystickHatEventArgs.
	/// </summary>
	public class JoystickHatEventArgs : SdlEventArgs 
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="device">The joystick index</param>
		/// <param name="hatIndex">The hat index</param>
		/// <param name="hatValue">The new hat position</param>
		public JoystickHatEventArgs(byte device, byte hatIndex, byte hatValue)
		{
			this.eventStruct = new Sdl.SDL_Event();
			this.eventStruct.jhat.which = device;
			this.eventStruct.jhat.hat = hatIndex;
			this.eventStruct.jhat.val = hatValue;
			this.eventStruct.type = (byte)EventTypes.JoystickHatMotion;
		}

		internal JoystickHatEventArgs(Sdl.SDL_Event ev)
		{
			this.eventStruct = ev;
		}

		/// <summary>
		/// 
		/// </summary>
		public int Device
		{
			get
			{
				return this.eventStruct.jhat.which;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public int HatIndex
		{
			get
			{
				return this.eventStruct.jhat.hat;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public int HatValue
		{
			get
			{ 
				return this.eventStruct.jhat.val;
			}
		}
	}
}
