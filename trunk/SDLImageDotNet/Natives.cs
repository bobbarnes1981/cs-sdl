using System;
using System.Security;
using System.Runtime.InteropServices;

namespace SDLDotNet.Images
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
		const string SDL_IMAGE_DLL = "SDL_Image";


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
