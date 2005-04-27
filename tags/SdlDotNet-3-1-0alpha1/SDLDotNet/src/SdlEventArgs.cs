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
	/// Summary description for ActiveEventArgs.
	/// </summary>
	public class SdlEventArgs : EventArgs
	{
		/// <summary>
		/// 
		/// </summary>
		internal Sdl.SDL_Event eventStruct;

		/// <summary>
		/// 
		/// </summary>
		public SdlEventArgs()
		{
		}

		/// <summary>
		/// 
		/// </summary>
		internal SdlEventArgs(Sdl.SDL_Event ev)
		{
			this.eventStruct = ev;
		}

		internal Sdl.SDL_Event EventStruct
		{
			get
			{
				return this.eventStruct;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ev"></param>
		/// <returns></returns>
		internal static SdlEventArgs CreateEventArgs( Sdl.SDL_Event ev )
		{
			switch ((EventTypes)ev.type) 
			{
				case EventTypes.KeyDown:
					return new KeyboardEventArgs(ev);
				case EventTypes.KeyUp:
					return new KeyboardEventArgs(ev);
				case EventTypes.ActiveEvent:
					return new ActiveEventArgs(ev);
				case EventTypes.Quit:
					return new QuitEventArgs(ev);
				case EventTypes.MouseButtonUp:
					return new MouseButtonEventArgs(ev);
				case EventTypes.MouseButtonDown:
					return new MouseButtonEventArgs(ev);
				case EventTypes.MouseMotion:
					return new MouseMotionEventArgs(ev);
				case EventTypes.VideoExpose:
					return new VideoExposeEventArgs(ev);
				case EventTypes.VideoResize:
					return new VideoResizeEventArgs(ev);
				case EventTypes.UserEvent:
					return new UserEventArgs(ev);
				default:
					return new SdlEventArgs(ev);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public EventTypes Type
		{
			get
			{
				return (EventTypes) this.EventStruct.type;
			}
		}
	}
}
