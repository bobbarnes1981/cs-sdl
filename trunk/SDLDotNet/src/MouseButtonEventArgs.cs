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
	public class MouseButtonEventArgs : EventArgs 
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="button">The mouse button</param>
		/// <param name="down">True if the button is pressed, 
		/// False if it is released</param>
		/// <param name="x">The current X coordinate</param>
		/// <param name="y">The current Y coordinate</param>
		public MouseButtonEventArgs(int button, bool down, int x, int y)
		{
			this.button = button;
			this.x = x;
			this.y = y;
			this.down = down;
		}

		private int button;
		/// <summary>
		/// 
		/// </summary>
		public int Button
		{
			get
			{
				return this.button;
			}
			set
			{
				this.button = value;
			}
		}

		private bool down;
		/// <summary>
		/// 
		/// </summary>
		public bool Down
		{
			get
			{
				return this.down;
			}
			set
			{
				this.down = value;
			}
		}

		private int x;
		/// <summary>
		/// 
		/// </summary>
		public int X
		{
			get
			{
				return this.x;
			}
			set
			{
				this.x = value;
			}
		}

		private int y;

		/// <summary>
		/// 
		/// </summary>
		public int Y
		{
			get
			{ 
				return this.y;
			}
			set
			{
				this.y = value;
			}
		}
	}
}
