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
	/// Summary description for KeyboardEventArgs.
	/// </summary>
	public class KeyboardEventArgs : EventArgs 
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="device">The device index of the keyboard</param>
		/// <param name="down">
		/// True if the key is pressed, False if it was released
		/// </param>
		/// <param name="scanCode">The scancode of the key</param>
		/// <param name="key">The Sdl virtual keycode</param>
		/// <param name="modifierKeys">Current modifier flags</param>
		public KeyboardEventArgs(
			int device, bool down, 
			int scanCode, Key key, 
			ModifierKeys modifierKeys)
		{
			this.device = device;
			this.down = down;
			this.scanCode = scanCode;
			this.key = key;
			this.mod = mod;
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

		private int scanCode;
		/// <summary>
		/// 
		/// </summary>
		public int ScanCode
		{
			get
			{
				return this.scanCode;
			}
			set
			{
				this.scanCode = value;
			}
		}

		private Key key;
		/// <summary>
		/// 
		/// </summary>
		public Key Key
		{
			get
			{
				return this.key;
			}
			set
			{
				this.key = value;
			}
		}
        
		private ModifierKeys mod;
		/// <summary>
		/// 
		/// </summary>
		public ModifierKeys Mod
		{
			get
			{
				return this.mod;
			}
			set
			{
				this.mod = value;
			}
		}
	}
}
