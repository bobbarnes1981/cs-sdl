/*
 * $RCSfile$
 * Copyright (C) 2005 David Hudson (jendave@yahoo.com)
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
	/// Summary description for soundEventArgs.
	/// </summary>
	public class SoundEventArgs : EventArgs 
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="action"></param>
		/// <param name="fadeoutTime"></param>
		public SoundEventArgs(SoundAction action, int fadeoutTime)
		{
			this.action = action;
			this.fadeoutTime = fadeoutTime;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="action"></param>
		public SoundEventArgs(SoundAction action)
		{
			this.action = action;
		}

		private int fadeoutTime;
		private SoundAction action;
		/// <summary>
		/// 
		/// </summary>
		public int FadeoutTime
		{
			get
			{
				return this.fadeoutTime;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public SoundAction Action
		{
			get
			{
				return this.action;
			}
		}
	}
}
