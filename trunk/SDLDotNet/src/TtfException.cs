/*
 * $RCSfile$
 * Copyright (C) 2003 Lucas Maloney
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

/*
 * $Revision$
 * $Date$
 *
 *	REVISION HISTORY
 *
 *	Mon 31 Mar 2003 23:28:02 EST LM
 *	Changed namespace from SdlTtfDotNet
 *
 *	Tue 25 Mar 2003 18:19:03 EST LM
 *	Basically removed all code due to Natives.Sdl_GetError being inaccessible.
 */

using System;
using SdlDotNet;

namespace SdlDotNet {
	/// <summary>
	/// Sdl Ttf Exception Class
	/// </summary>
	public class TtfException : SdlException {
		/// <summary>
		/// Constructor for Sdl Ttf Exception.
		/// </summary>
		/// <param name="msg">Exception message</param>
		public TtfException(string msg) : base(msg) {}
	}
}
