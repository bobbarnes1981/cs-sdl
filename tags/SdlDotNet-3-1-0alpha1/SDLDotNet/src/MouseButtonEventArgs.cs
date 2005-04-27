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
using System.Drawing;

using Tao.Sdl;

namespace SdlDotNet
{
	/// <summary>
	/// Summary description for MouseMotionEventArgs.
	/// </summary>
	public class MouseButtonEventArgs : SdlEventArgs 
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="button">The mouse button</param>
		/// <param name="buttonPressed">True if the button is pressed, 
		/// False if it is released</param>
		/// <param name="x">The current X coordinate</param>
		/// <param name="y">The current Y coordinate</param>
		public MouseButtonEventArgs(MouseButton button, bool buttonPressed, short x, short y)
		{
			this.eventStruct = new Sdl.SDL_Event();
			this.eventStruct.button.button = (byte)button;
			this.eventStruct.button.which = 0;
			this.eventStruct.button.x = x;
			this.eventStruct.button.y = y;
			if (buttonPressed)
			{
				this.eventStruct.button.state = (byte)ButtonKeyState.Pressed;
				this.eventStruct.type = (byte)EventTypes.MouseButtonDown;
			}
			else
			{
				this.eventStruct.button.state = (byte)ButtonKeyState.NotPressed;
				this.eventStruct.type = (byte)EventTypes.MouseButtonUp;
			}
		}

		internal MouseButtonEventArgs(Sdl.SDL_Event ev)
		{
			this.eventStruct = ev;
		}

		/// <summary>
		/// Which mouse button created the event
		/// </summary>
		public MouseButton Button
		{
			get
			{
				return (MouseButton)this.eventStruct.button.button;
			}
		}

		/// <summary>
		/// True if button is pressed
		/// </summary>
		public bool ButtonPressed
		{
			get
			{
				return (this.eventStruct.button.state == (byte)ButtonKeyState.Pressed);
			}
		}

		/// <summary>
		/// X position of mouse at time of event
		/// </summary>
		public short X
		{
			get
			{
				return this.eventStruct.button.x;
			}
		}

		/// <summary>
		/// Y position of mouse at time of event 
		/// </summary>
		public short Y
		{
			get
			{ 
				return this.eventStruct.button.y;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public Point Position
		{
			get
			{
				return new Point(this.eventStruct.button.x, this.eventStruct.button.y);
			}
		}
	}
}
