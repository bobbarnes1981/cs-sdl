/*
 * $RCSfile$
 * Copyright (C) 2003 Will Weisser (ogl@9mm.com)
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
	/// Represents a run-time error from the Sdl library.
	/// </summary>
	public class SdlException : Exception 
	{
		/// <summary>
		/// Initializes an SdlException instance
		/// </summary>
		/// <param name="msg">
		/// The string representing the error message
		/// </param>
		public SdlException(string msg) : base(msg) 
		{
		}

		/// <summary>
		/// Generates an SdlException based on the last Sdl Error code.
		/// </summary>
		/// <returns>
		/// A new SdlException object
		/// </returns>
		public static SdlException Generate() 
		{
			string msg = Sdl.SDL_GetError();

			if (msg.IndexOf("Surface was lost") == -1)
			{
				return new SdlException(msg);
			}
			else
			{
				return new SurfaceLostException(msg);
			}
		}
	}
}
