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

namespace SdlDotNet {
	/// <summary>
	/// Summary description for ActiveEventArgs.
	/// </summary>
	public class ActiveEventArgs : EventArgs 
	{
		/// <summary>
		/// 
		/// </summary>
		public ActiveEventArgs()
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="gained">
		/// True if the focus was gained, False if it was lost
		/// </param>
		public ActiveEventArgs(bool gained)
		{
			this.gained = gained;
		}
		
		private bool gained;
		/// <summary>
		/// 
		/// </summary>
		public bool Gained
		{
			get
			{
				return this.gained;
			}
			set
			{
				this.gained = value;
			}
		}

	}
}
