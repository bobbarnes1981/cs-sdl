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
	/// <implements>System.IDisposable</implements>
	public class Sample : IDisposable {
		private IntPtr _handle;
		private bool _disposed;

		internal Sample(IntPtr handle) {
			_handle = handle;
			_disposed = false;
		}

		internal IntPtr GetHandle() { return _handle; }

		/// <summary>
		/// Allows an Object to attempt to free resources 
		/// and perform other cleanup operations before the Object 
		/// is reclaimed by garbage collection.
		/// </summary>
		~Sample() 
		{
			SdlMixer.Mix_FreeChunk(_handle);
		}

		/// <summary>
		/// Destroys this Sample and frees the memory associated with it
		/// </summary>
		public void Dispose() {
			if (!_disposed) {
				_disposed = true;
				SdlMixer.Mix_FreeChunk(_handle);
				GC.SuppressFinalize(this);
			}
		}

		/// <summary>
		/// Sets the volume of the sample
		/// </summary>
		/// <param name="volume">New volume. Should be between 0 and 128 inclusive.</param>
		public void SetVolume(int volume) {
			if (SdlMixer.Mix_VolumeChunk(_handle, volume) != 0)
				throw SdlException.Generate();
		}
	}
}
