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
	/// Summary description for UserEventArgs.
	/// </summary>
	public class UserEventArgs : SdlEventArgs 
	{
		/// <summary>
		/// 
		/// </summary>
		public UserEventArgs()
		{
			this.eventStruct = new Sdl.SDL_Event();
			this.eventStruct.type = (byte)EventTypes.UserEvent;
			this.eventStruct.user.type =  (byte)EventTypes.UserEvent;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="userEvent">The user event object</param>
		public UserEventArgs(object userEvent)
		{
			this.eventStruct = new Sdl.SDL_Event();
			this.userEvent = userEvent;
			this.eventStruct.type = (byte)EventTypes.UserEvent;
			this.eventStruct.user.type =  (byte)EventTypes.UserEvent;
		}
		
		private object userEvent;
		/// <summary>
		/// 
		/// </summary>
		public object UserEvent
		{
			get
			{
				return this.userEvent;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public int UserCode
		{
			get
			{
				return this.eventStruct.user.code;
			}
			set
			{
				this.eventStruct.user.code = value;
			}
		}
	}
}
