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
	/// Summary description for MouseMotionEventArgs.
	/// </summary>
	public class MouseMotionEventArgs : SdlEventArgs 
	{
		/// <summary>
		/// MouseMotion
		/// </summary>
		/// <param name="buttonPressed">The current mouse button state</param>
		/// <param name="x">The current X coordinate</param>
		/// <param name="y">The current Y coordinate</param>
		/// <param name="relativeX">
		/// The difference between the last X coordinate and current</param>
		/// <param name="relativeY">
		/// The difference between the last Y coordinate and current</param>
		public MouseMotionEventArgs(bool buttonPressed, short x, short y, 
			short relativeX, short relativeY)
		{
			this.eventStruct = new Sdl.SDL_Event();
			this.eventStruct.motion.xrel = relativeX;
			this.eventStruct.motion.yrel = relativeY;
			this.eventStruct.motion.which = 0;
			this.eventStruct.motion.x = x;
			this.eventStruct.motion.y = y;
			this.eventStruct.type = (byte)EventTypes.MouseMotion;
			if (buttonPressed)
			{
				this.eventStruct.motion.state = (byte)ButtonKeyState.Pressed;
			}
			else
			{
				this.eventStruct.motion.state = (byte)ButtonKeyState.NotPressed;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ev"></param>
		internal MouseMotionEventArgs(Sdl.SDL_Event ev)
		{
			this.eventStruct = ev;
		}

		/// <summary>
		/// True if button pressed
		/// </summary>
		public bool ButtonPressed
		{
			get
			{
				return (this.eventStruct.motion.state == (byte)ButtonKeyState.Pressed);
			}
		}

		/// <summary>
		/// X position of mouse
		/// </summary>
		public int X
		{
			get
			{
				return this.eventStruct.motion.x;
			}
		}

		/// <summary>
		/// Y position of mouse
		/// </summary>
		public int Y
		{
			get
			{ 
				return this.eventStruct.motion.y;
			}
		}

		/// <summary>
		/// change in X position of mouse
		/// </summary>
		public int RelativeX
		{
			get
			{
				return this.eventStruct.motion.xrel;
			}
		}

		/// <summary>
		/// Change in Y position of mouse
		/// </summary>
		public int RelativeY
		{
			get
			{
				return this.eventStruct.motion.yrel;
			}
		}
	}
}
