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
	/// Summary description for JoystickButtonEventArgs.
	/// </summary>
	public class JoystickButtonEventArgs : EventArgs 
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="device">The joystick index</param>
		/// <param name="button">The button index</param>
		/// <param name="down">
		/// True if the button was pressed, 
		/// False if it was released
		/// </param>
		public JoystickButtonEventArgs(int device, int button, bool down)
		{
			this.device = device;
			this.button = button;
			this.down = down;
		}

		private int device;
		/// <summary>
		/// 
		/// </summary>
		public int Device
		{
			get
			{
				return this.device;
			}
			set
			{
				this.device = value;
			}
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
	}
}
