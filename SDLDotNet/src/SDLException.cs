/*
 * $RCSfile$
 * Copyright (C) 2003 Will Weisser (ogl@9mm.com)
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

namespace SDLDotNet {
	/// <summary>
	/// Represents a run-time error from the SDL library
	/// </summary>
	public class SDLException : Exception {
		/// <summary>
		/// Initializes an SDLException instance
		/// </summary>
		/// <param name="msg">The string representing the error message</param>
		public SDLException(string msg) : base(msg) {}

		/// <summary>
		/// Generates an SDLException based on the last SDL Error code
		/// </summary>
		/// <returns>A new SDLException object</returns>
		public static SDLException Generate() {
			string msg = Natives.SDL_GetError();

			if (msg.IndexOf("Surface was lost") == -1)
				return new SDLException(msg);
			else
				return new SurfaceLostException(msg);
		}
	}

	/// <summary>
	/// Represents an error resulting from a surface being lost, usually as a result of
	/// the user changing the input focus away from a full-screen application
	/// </summary>
	public class SurfaceLostException : SDLException {
		internal SurfaceLostException(string msg) : base(msg) {}
	}
}
