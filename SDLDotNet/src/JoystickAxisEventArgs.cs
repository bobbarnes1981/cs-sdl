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
	/// Summary description for JoystickAxisEventArgs.
	/// </summary>
	public class JoystickAxisEventArgs : SdlEventArgs 
	{		
		private const float JOYSTICK_ADJUSTMENT = 32768;
		private const float JOYSTICK_SCALE = 65535;
		/// <summary>
		/// 
		/// </summary>
		private const short JOYSTICK_THRESHHOLD = 3277;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="device">The joystick index</param>
		/// <param name="axisIndex">The axis index</param>
		/// <param name="axisValue">The new axis value</param>
		public JoystickAxisEventArgs(byte device, byte axisIndex, float axisValue)
		{
			this.eventStruct = new Sdl.SDL_Event();
			this.eventStruct.jaxis.which = device;
			this.eventStruct.jaxis.axis = axisIndex;
			this.eventStruct.jaxis.val = 
				(short)((axisValue * JOYSTICK_SCALE) - JOYSTICK_ADJUSTMENT);
			this.eventStruct.type = (byte)EventTypes.JoystickAxisMotion;
		}

		internal JoystickAxisEventArgs(Sdl.SDL_Event ev)
		{
			this.eventStruct = ev;
		}

		/// <summary>
		/// 
		/// </summary>
		public byte Device
		{
			get
			{
				return this.eventStruct.jaxis.which;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public byte AxisIndex
		{
			get
			{
				return this.eventStruct.jaxis.axis;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public float AxisValue
		{
			get
			{ 
				float jaxisValue = 
					((float)this.eventStruct.jaxis.val + JOYSTICK_ADJUSTMENT) / JOYSTICK_SCALE;
				if (jaxisValue < 0)
				{
					jaxisValue = 0;
				}
				return jaxisValue;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public static short JoystickThreshold
		{
			get
			{
				return JOYSTICK_THRESHHOLD;
			}
		}
	}
}