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
	public class MouseMotionEventArgs : EventArgs 
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="state">The current mouse button state</param>
		/// <param name="x">The current X coordinate</param>
		/// <param name="y">The current Y coordinate</param>
		/// <param name="relativeX">
		/// The difference between the last X coordinite and current</param>
		/// <param name="relativeY">
		/// The difference between the last Y coordinite and current</param>
		public MouseMotionEventArgs(
			MouseButtonState state, int x, int y, 
			int relativeX, int relativeY)
		{
			this.state = state;
			this.x = x;
			this.y = y;
			this.relativeX = relativeX;
			this.relativeY = relativeY;
		}

		private MouseButtonState state;
		/// <summary>
		/// 
		/// </summary>
		public MouseButtonState State
		{
			get
			{
				return this.state;
			}
			set
			{
				this.state = value;
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

		private int relativeX;
		/// <summary>
		/// 
		/// </summary>
		public int RelativeX
		{
			get
			{
				return this.relativeX;
			}
			set
			{
				this.relativeX = value;
			}
		}

		private int relativeY;
		/// <summary>
		/// 
		/// </summary>
		public int RelativeY
		{
			get
			{
				return this.relativeY;
			}
			set
			{
				this.relativeY = value;
			}
		}
	}
}
