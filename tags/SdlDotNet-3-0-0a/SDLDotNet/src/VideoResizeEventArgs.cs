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
	/// Event args for resizing the application window.
	/// </summary>
	public class VideoResizeEventArgs : SdlEventArgs 
	{
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="w">The new window width</param>
		/// <param name="h">The new window height</param>
		public VideoResizeEventArgs(int w, int h)
		{
			this.eventStruct.resize.w = w;
			this.eventStruct.resize.h = h;
			this.eventStruct.type = (byte)EventTypes.VideoResize;
		}

		internal VideoResizeEventArgs(Sdl.SDL_Event ev)
		{
			this.eventStruct = ev;
		}
		
		/// <summary>
		/// Width of window
		/// </summary>
		public int W
		{
			get
			{
				return this.eventStruct.resize.w;
			}
		}

		/// <summary>
		/// Height of window
		/// </summary>
		public int H
		{
			get
			{
				return this.eventStruct.resize.h;
			}
		}

	}
}
