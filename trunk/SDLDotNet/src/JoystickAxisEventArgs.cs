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
	/// Summary description for JoystickAxisEventArgs.
	/// </summary>
	public class JoystickAxisEventArgs : EventArgs 
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="device">The joystick index</param>
		/// <param name="axisIndex">The axis index</param>
		/// <param name="axisValue">The new axis value</param>
		public JoystickAxisEventArgs(int device, int axisIndex, float axisValue)
		{
			this.device = device;
			this.axisIndex = axisIndex;
			this.axisValue = axisValue;
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

		private int axisIndex;
		/// <summary>
		/// 
		/// </summary>
		public int AxisIndex
		{
			get
			{
				return this.axisIndex;
			}
			set
			{
				this.axisIndex = value;
			}
		}

		private float axisValue;

		/// <summary>
		/// 
		/// </summary>
		public float AxisValue
		{
			get
			{ 
				return this.axisValue;
			}
			set
			{
				this.axisValue = value;
			}
		}
	}
}
