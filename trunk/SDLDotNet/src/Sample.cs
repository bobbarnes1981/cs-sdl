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
	/// Represents a sound sample.
	/// Create with Mixer.LoadWav().
	/// </summary>
	public class Sample : BaseSdlResource 
	{
		private IntPtr handle;
		private bool disposed = false;

		internal Sample(IntPtr handle) 
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
					CloseHandle(handle);
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
			SdlMixer.Mix_FreeChunk(handleToClose);
			GC.KeepAlive(this);
			handleToClose = IntPtr.Zero;
		}

		/// <summary>
		/// Gets/Set the volume of the sample. Should be between 0 and 128 inclusive.
		/// </summary>
		public int Volume
		{
			get
			{
				int result = SdlMixer.Mix_VolumeChunk(handle, -1);
				GC.KeepAlive(this);
				return result;
			}
			set
			{
				if (value >= 0 && value <= SdlMixer.MIX_MAX_VOLUME)
				{
					int result = SdlMixer.Mix_VolumeChunk(handle, value);
				}
				GC.KeepAlive(this);
			}

		}
	}
}
