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
using System.Runtime.InteropServices;

using Tao.Sdl;

namespace SdlDotNet 
{
	/// <summary>
	/// Base class for SdlResources
	/// </summary>
	public abstract class BaseSdlResource : IDisposable 
	{
		private bool disposed = false;
		private IntPtr handle;

		/// <summary>
		/// 
		/// </summary>
		protected BaseSdlResource(IntPtr handle) 
		{
			this.handle = handle;
		}

		/// <summary>
		/// 
		/// </summary>
		protected BaseSdlResource() 
		{
		}

		/// <summary>
		/// Allows an Object to attempt to free resources 
		/// and perform other cleanup operations before the Object 
		/// is reclaimed by garbage collection.
		/// </summary>
		~BaseSdlResource() 
		{
			Dispose(false);
		}

		/// <summary>
		/// Closes and destroys this object
		/// </summary>
		public void Dispose() 
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="disposing"></param>
		protected virtual void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				if (disposing)
				{
				}
				CloseHandle(handle);
				GC.KeepAlive(this);
			}
			disposed = true;
		}

		/// <summary>
		/// 
		/// </summary>
		protected abstract void CloseHandle(IntPtr handle);

		/// <summary>
		/// Closes and destroys this object
		/// </summary>
		public void Close() 
		{
			Dispose();
		}
	}
}
