/*
 * $RCSfile$
 * Copyright (C) 2004 David Hudson (jendave@yahoo.com)
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
using Tao.Sdl;

namespace SdlDotNet 
{
	/// <summary>
	/// Represents a music sample.  Music is generally longer than a sound effect sample,
	/// however it can also be compressed e.g. by Ogg Vorbis
	/// </summary>
	public class Music : BaseSdlResource 
	{
		private IntPtr handle;
		private bool disposed = false;

		internal Music(IntPtr handle) 
		{
			this.handle = handle;
		}

		internal IntPtr GetHandle() 
		{ 
			GC.KeepAlive(this);
			return handle; 
		}

		/// <summary>
		/// Destroys the surface object and frees its memory
		/// </summary>
		/// <param name="disposing"></param>
		protected override void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				try
				{
					if (disposing)
					{
					}
					// TODO: fix this issue correctly.
					// In Release mode, GC disposes the Music class too quickly unless
					// The CloseHandle is commented out.
					//CloseHandle(handle);
					GC.KeepAlive(this);
					this.disposed = true;
				}
				finally
				{
					base.Dispose(disposing);
				}
			}
		}

		/// <summary>
		/// Closes Music handle
		/// </summary>
		protected override void CloseHandle(IntPtr handleToClose) 
		{
			SdlMixer.Mix_FreeMusic(handleToClose);
			GC.KeepAlive(this);
			handleToClose = IntPtr.Zero;
		}
	}
}
