/*
 * $RCSfile$
 * Copyright (C) 2003 Klavs Martens
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
using System.Security;
using System.Runtime.InteropServices;

namespace SDLDotNet.Image
{
	/// <summary>
	/// SDL_Image bindings for .NET. 
	/// Contains functions to create surfaces from a images in various formats.
	/// 
	/// If the image format supports a transparent pixel, SDL will set the
	/// colorkey for the surface. 
	/// </summary>
	unsafe sealed class Natives 
	{

		/// <summary>
		/// Filename of the SDL library. 
		/// </summary>
		const string SDL_DLL = "SDL";


		/// <summary>
		/// Filename of the SDL_Image library. 
		/// </summary>
		const string SDL_IMAGE_DLL = "SDL_image";


		/* SDL.dll/libSDL.so */

		/// <summary>
		/// Get RWops from a array of bytes.
		/// </summary>
		[DllImport(SDL_DLL, CallingConvention=CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
		public static extern IntPtr SDL_RWFromMem(byte[] mem, int size);


		/* SDL_Image.dll/libSDLImage.so */


		/// <summary>
		/// Load an image with a specific format from memory and return a pointer to its surface.
		/// </summary>
		[DllImport(SDL_IMAGE_DLL, CallingConvention=CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
		public static extern IntPtr IMG_LoadTyped_RW(IntPtr src, int freesrc, string type);


		/// <summary>
		/// Load a image from disk and return a pointer to its surface.
		/// </summary>
		[DllImport(SDL_IMAGE_DLL, CallingConvention=CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
		public static extern IntPtr IMG_Load(string _file);


		/// <summary>
		/// Load an image with an unspecified format from memory and return a pointer to its surface.
		/// </summary>
		[DllImport(SDL_IMAGE_DLL, CallingConvention=CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
		public static extern IntPtr IMG_Load_RW(IntPtr src, int freesrc);



		/* 
		 * Following functions are not used but included to make the bindings complete. 
		 */

		/// <summary>
		/// Load a .BMP image from memory and return a pointer to its surface.
		/// </summary>
		[DllImport(SDL_IMAGE_DLL, CallingConvention=CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
		public static extern IntPtr IMG_LoadBMP_RW(IntPtr src);
		

		/// <summary>
		/// Load a .PNM image from memory and return a pointer to its surface.
		/// </summary>
		[DllImport(SDL_IMAGE_DLL, CallingConvention=CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
		public static extern IntPtr IMG_LoadPNM_RW(IntPtr src);
		

		/// <summary>
		/// Load a .XPM image from memory and return a pointer to its surface.
		/// </summary>
		[DllImport(SDL_IMAGE_DLL, CallingConvention=CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
		public static extern IntPtr IMG_LoadXPM_RW(IntPtr src);
		

		/// <summary>
		/// Load a .XCF image from memory and return a pointer to its surface.
		/// </summary>
		[DllImport(SDL_IMAGE_DLL, CallingConvention=CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
		public static extern IntPtr IMG_LoadXCF_RW(IntPtr src);
		

		/// <summary>
		/// Load a .PCX image from memory and return a pointer to its surface.
		/// </summary>
		[DllImport(SDL_IMAGE_DLL, CallingConvention=CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
		public static extern IntPtr IMG_LoadPCX_RW(IntPtr src);
		

		/// <summary>
		/// Load a .GIF image from memory and return a pointer to its surface.
		/// </summary>
		[DllImport(SDL_IMAGE_DLL, CallingConvention=CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
		public static extern IntPtr IMG_LoadGIF_RW(IntPtr src);
		

		/// <summary>
		/// Load a .JPG image from memory and return a pointer to its surface.
		/// </summary>
		[DllImport(SDL_IMAGE_DLL, CallingConvention=CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
		public static extern IntPtr IMG_LoadJPG_RW(IntPtr src);
		

		/// <summary>
		/// Load a .TIF image from memory and return a pointer to its surface.
		/// </summary>
		[DllImport(SDL_IMAGE_DLL, CallingConvention=CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
		public static extern IntPtr IMG_LoadTIF_RW(IntPtr src);
		

		/// <summary>
		/// Load a .PNG image from memory and return a pointer to its surface.
		/// </summary>
		[DllImport(SDL_IMAGE_DLL, CallingConvention=CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
		public static extern IntPtr IMG_LoadPNG_RW(IntPtr src);


		/// <summary>
		/// Load a .TGA image from memory and return a pointer to its surface.
		/// </summary>
		[DllImport(SDL_IMAGE_DLL, CallingConvention=CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
		public static extern IntPtr IMG_LoadTGA_RW(IntPtr src);

	}

}
