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
	/// Summary description for JoystickBallEventArgs.
	/// </summary>
	public class JoystickBallEventArgs : SdlEventArgs 
	{
		/// <summary>
		/// 
		/// </summary>
		///<param name="device">The joystick index</param>
		/// <param name="ball">The trackball index</param>
		/// <param name="relativeX">The relative X position</param>
		/// <param name="relativeY">The relative Y position</param>
		public JoystickBallEventArgs(byte device, byte ball, short relativeX, short relativeY)
		{
			this.eventStruct = new Sdl.SDL_Event();
			this.eventStruct.jball.which = device;
			this.eventStruct.jball.ball = ball;
			this.eventStruct.jball.xrel = relativeX;
			this.eventStruct.jball.yrel = relativeY;
			this.eventStruct.type = (byte)EventTypes.JoystickBallMotion;
		}

		internal JoystickBallEventArgs(Sdl.SDL_Event ev)
		{
			this.eventStruct = ev;
		}

		/// <summary>
		/// Device
		/// </summary>
		public int Device
		{
			get
			{
				return this.eventStruct.jball.which;
			}
		}

		/// <summary>
		/// Ball
		/// </summary>
		public int Ball
		{
			get
			{
				return this.eventStruct.jball.ball;
			}
		}

		/// <summary>
		/// change in distance on X-axis
		/// </summary>
		public int RelativeX
		{
			get
			{
				return this.eventStruct.jball.xrel;
			}
		}

		/// <summary>
		/// Change in distance on Y axis
		/// </summary>
		public int RelativeY
		{
			get
			{
				return this.eventStruct.jball.yrel;
			}
		}
	}
}
