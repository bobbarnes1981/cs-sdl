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
	/// The main Sdl object.
	/// Only one of these should be created for any Sdl application.
	/// If you wish to shut down the Sdl library before the end of the 
	/// program, call the Dispose() method.
	/// All the functionality of the Sdl library is available through this 
	/// class and its properties.
	/// </summary>
	internal sealed class SdlCore : IDisposable 
	{
		private bool disposed;

		/// <summary>
		/// Returns the global instance of this class.
		/// </summary>
		static readonly SdlCore instance = new SdlCore();

		// Explicit static constructor to tell C# compiler
		// not to mark type as beforefieldinit
		static SdlCore() 
		{
		}

		SdlCore() 
		{
			if (Sdl.SDL_Init(0) == -1)
			{
				throw SdlException.Generate();
			}
			disposed = false;
		}

		public static SdlCore Instance
		{
			get
			{
				return instance;
			}
		}

		/// <summary>
		/// Allows an Object to attempt to free resources 
		/// and perform other cleanup operations before the Object 
		/// is reclaimed by garbage collection.
		/// </summary>
		~SdlCore() 
		{
			Sdl.SDL_Quit();
		}

		/// <summary>
		/// Destroys this object and shuts down the Sdl library.
		/// </summary>
		public void Dispose() 
		{
			if (!disposed) 
			{
				disposed = true;
				Sdl.SDL_Quit();
				GC.SuppressFinalize(this);
			}
		}
	}
}
