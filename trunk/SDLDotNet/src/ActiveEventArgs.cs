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
	/// Summary description for ActiveEventArgs.
	/// </summary>
	public class ActiveEventArgs : SdlEventArgs
	{
		internal ActiveEventArgs(Sdl.SDL_Event ev)
		{
			this.eventStruct = ev;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="gainedFocus">
		/// True if the focus was gained, False if it was lost
		/// </param>
		/// <param name="state"></param>
		public ActiveEventArgs(bool gainedFocus, Focus state)
		{
			this.eventStruct = new Sdl.SDL_Event();
			if (gainedFocus)
			{
				this.eventStruct.active.gain = 1;
			}
			else
			{
				this.eventStruct.active.gain = 0;
			}
			this.eventStruct.type = (byte)EventTypes.ActiveEvent;
			this.eventStruct.active.state = (byte)state;
		}
		
		/// <summary>
		/// 
		/// </summary>
		public bool GainedFocus
		{
			get
			{
				if (this.eventStruct.active.gain != 0)
				{
					return true;
				}
				else
				{
					return false;
				}
			}
		}	
		/// <summary>
		/// 
		/// </summary>
		public Focus State
		{
			get
			{
				return (Focus)this.eventStruct.active.state;
			}
		}
	}
}
