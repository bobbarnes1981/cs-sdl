/*
 * $RCSfile$
 * Copyright (C) 2005 David Hudson (jendave@yahoo.com)
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
using SdlDotNet;
using Tao.Sdl;

namespace SdlDotNet
{
	/// <summary>
	/// Constructor.
	/// </summary>
	public sealed class FontSystem
	{
		static private bool disposed;
		static readonly FontSystem instance = new FontSystem();

		FontSystem()
		{
			Initialize();
		}

		/// <summary>
		/// Initialize Font subsystem.
		/// </summary>
		public static void Initialize()
		{
			if (SdlTtf.TTF_Init() != (int) SdlFlag.Success)
			{
				FontException.Generate();
			}
		}

		/// <summary>
		/// Queries if the Font subsystem has been intialized.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// <returns>True if Font subsystem has been initialized, false if it has not.</returns>
		public static bool IsInitialized
		{
			get
			{

				if (SdlTtf.TTF_WasInit() == (int) SdlFlag.TrueValue)
				{
					return true;
				}
				else 
				{
					return false;
				}
			}
		}

		/// <summary>
		/// Destructor
		/// </summary>
		~FontSystem() 
		{
			Dispose(false);
		}

		/// <summary>
		/// Closes and destroys this object
		/// </summary>
		public static void Dispose() 
		{
			Dispose(true);
		}

		/// <summary>
		/// Closes and destroys this object
		/// </summary>
		/// <param name="disposing"></param>
		public static void Dispose(bool disposing)
		{
			if (!disposed)
			{
				if (disposing)
				{
					SdlTtf.TTF_Quit();
				}
				disposed = true;
			}
		}

		/// <summary>
		/// Closes and destroys this object
		/// </summary>
		public static void Close() 
		{
			Dispose();
		}

		/// <summary>
		/// Create new font object from Truetype font file.
		/// </summary>
		/// <param name="filename">Filename of Truetype font</param>
		/// <param name="pointSize">Point size</param>
		/// <returns></returns>
		public static Font OpenFont(string filename, int pointSize) 
		{
			IntPtr handle = SdlTtf.TTF_OpenFont(filename, pointSize);
			if (handle == IntPtr.Zero) 
			{
				throw FontException.Generate();
			}
			return new Font(handle);
		}

		/// <summary>
		/// Get System Font Names
		/// </summary>
		/// <returns></returns>
		public static System.Drawing.Text.FontCollection SystemFontNames
		{
			get
			{
				return new System.Drawing.Text.InstalledFontCollection();
			}
		}
	}
}
