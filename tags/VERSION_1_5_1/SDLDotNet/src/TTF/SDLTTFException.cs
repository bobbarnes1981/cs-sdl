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
 *	Changed namespace from SDLTTFDotNet
 *
 *	Tue 25 Mar 2003 18:19:03 EST LM
 *	Basically removed all code due to Natives.SDL_GetError being inaccessible.
 */

using System;
using SDLDotNet;

namespace SDLDotNet.TTF {
	/// <summary>
	/// SDL TTF Exception Class
	/// </summary>
	public class SDLTTFException : SDLException {
		/// <summary>
		/// Constructor for SDL TTF Exception.
		/// </summary>
		/// <param name="msg">Exception message</param>
		public SDLTTFException(string msg) : base(msg) {}
	}
}
