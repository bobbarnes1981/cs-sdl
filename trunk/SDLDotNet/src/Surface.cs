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
using System.IO;
using System.Drawing;
using System.Runtime.InteropServices;
using Tao.Sdl;

namespace SdlDotNet 
{
	/// <summary>
	/// 
	/// </summary>
	[FlagsAttribute]
	public enum Alphas
	{
		/// <summary>
		/// Equivalent to SDL_RLEACC
		/// </summary>
		RleEncoded = Sdl.SDL_RLEACCEL,

		/// <summary>
		/// Equivalent to SDL_SRCALPHA
		/// </summary>
		SourceAlphaBlending  = Sdl.SDL_SRCALPHA
	}
	
	/// <summary>
	/// An opaque structure representing an Sdl pixel value.
	/// </summary>
	public struct PixelValue 
	{
		private int val;
		/// <summary>
		/// Creates a pixel value.
		/// </summary>
		/// <param name="pixelValue">
		/// The raw pixel value to use.
		/// </param>
		public PixelValue(int pixelValue) 
		{
			val = pixelValue;
		}

		/// <summary>
		/// Gets or sets the pixel value
		/// </summary>
		public int Value 
		{
			get 
			{ 
				return val; 
			}
			set 
			{ 
				val = value;
			}
		}
	}

	/// <summary>
	/// Represents an Sdl drawing surface.
	/// You can create instances of this class with the methods in the Video 
	/// object.
	/// </summary>
	public class Surface : IDisposable 
	{
		private bool _freeondispose;
		private bool _disposed;
		private IntPtr _surfacePtr;

		#region Constructors and Destructors
		/// <summary>
		/// 
		/// </summary>
		/// <param name="surfacePtr"></param>
		/// <param name="freeondispose"></param>
		internal Surface(IntPtr surfacePtr, bool freeondispose) 
		{
			_freeondispose = freeondispose;
			_surfacePtr = surfacePtr;
			_disposed = false;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="surfacePtr"></param>
		internal Surface(IntPtr surfacePtr) : this(surfacePtr, true)
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="file"></param>
		public Surface(string file)
		{
			IntPtr surfPtr = SdlImage.IMG_Load(file);
			if (surfPtr == IntPtr.Zero)
			{
				throw SdlException.Generate();
			}
			_surfacePtr = surfPtr;
			_freeondispose = true;
			_disposed = false;
		}
	
		/// <summary>
		/// Allows an Object to attempt to free resources 
		/// and perform other cleanup operations before the Object 
		/// is reclaimed by garbage collection.
		/// </summary>
		~Surface() 
		{
			if (_freeondispose)
			{
				Sdl.SDL_FreeSurface(ref _surfacePtr);
			}
		}
		#endregion Constructors and Destructors

		internal static Surface FromScreenPtr(IntPtr surfacePtr) 
		{
			return new Surface(surfacePtr, false);
		}

//		internal static Surface FromPtr(IntPtr surfacePtr) {
//			return new Surface(surfacePtr, true);
//		}

		private Sdl.SDL_Surface GetSurfaceStructFromPtr(IntPtr ptr)
		{
			return (Sdl.SDL_Surface)Marshal.PtrToStructure(ptr, 
				typeof(Sdl.SDL_Surface));
		}

		/// <summary>
		/// Returns the native Sdl Surface pointer
		/// </summary>
		/// <returns>
		/// An IntPtr pointing at the Sdl surface reference
		/// </returns>
		public IntPtr SurfacePointer
		{
			get
			{
				GC.KeepAlive(this);
				return _surfacePtr;
			}
		}

		internal static Surface FromImageFile(string file) 
		{
			IntPtr surfPtr = SdlImage.IMG_Load(file);
			//GC.KeepAlive(this);
			if (surfPtr == IntPtr.Zero)
			{
				throw SdlException.Generate();
			}
			return new Surface(surfPtr, true);
		}

		internal static Surface FromBitmap(System.Drawing.Bitmap bitmap) 
		{
			MemoryStream stream = new MemoryStream();
			bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
			byte[] arr = stream.ToArray();
			IntPtr surf = Sdl.SDL_LoadBMP_RW(Sdl.SDL_RWFromMem(arr, arr.Length), 1);
			//GC.KeepAlive(this);
			if (surf == IntPtr.Zero)
			{
				throw SdlException.Generate();
			}
			return new Surface(surf, true);
		}

		internal static Surface FromBitmap(byte[] arr) 
		{
			IntPtr surf = Sdl.SDL_LoadBMP_RW(Sdl.SDL_RWFromMem(arr, arr.Length), 1);
			//GC.KeepAlive(this);
			if (surf == IntPtr.Zero)
			{
				throw SdlException.Generate();
			}
			return new Surface(surf, true);
		}


		/// <summary>
		/// Destroys the surface object and frees its memory
		/// </summary>
		public void Dispose() 
		{
			if (_freeondispose && !_disposed) 
			{
				_disposed = true;
				//Sdl.SDL_FreeSurface(ref _surfacePtr);
				GC.SuppressFinalize(this);
			}
		}

		/// <summary>
		/// If the surface is double-buffered, 
		/// this method will flip the back buffer onto the screen
		/// </summary>
		public void Flip() 
		{
			int result = Sdl.SDL_Flip(_surfacePtr);
			GC.KeepAlive(this);
			if (result != 0)
			{
				throw SdlException.Generate();
			}
		}

		private Sdl.SDL_Rect ConvertRecttoSDLRect(
			System.Drawing.Rectangle rect)
		{
			return new Sdl.SDL_Rect(
				(short)rect.X, 
				(short)rect.Y,
				(short)rect.Width, 
				(short)rect.Height);
		}

		/// <summary>
		/// Draws a rectangle onto the surface
		/// </summary>
		/// <param name="rectangle">The rectangle coordinites</param>
		/// <param name="color">The color to draw</param>
		public void FillRectangle(System.Drawing.Rectangle rectangle,
			System.Drawing.Color color) 
		{
			Sdl.SDL_Rect sdlrect = ConvertRecttoSDLRect(rectangle);

			int result = Sdl.SDL_FillRect(_surfacePtr, ref sdlrect, MapColor(color).Value);
			GC.KeepAlive(this);
			if (result != 0)
			{
				throw SdlException.Generate();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="color"></param>
		public void Fill(System.Drawing.Color color) 
		{
			Sdl.SDL_Surface surf = 
				this.GetSurfaceStructFromPtr(_surfacePtr);
			Sdl.SDL_Rect sdlrect = 
				new Sdl.SDL_Rect(0, 0, (short)surf.w, (short)surf.h);
			int result = Sdl.SDL_FillRect(_surfacePtr, ref sdlrect, MapColor(color).Value);
			GC.KeepAlive(this);
			if (result != 0)
			{
				throw SdlException.Generate();
			}
		}

		/// <summary>
		/// Maps a logical color to a pixel value in the surface's pixel format
		/// </summary>
		/// <param name="color">The color to map</param>
		/// <returns>A pixel value in the surface's format</returns>
		public PixelValue MapColor(System.Drawing.Color color) 
		{
			Sdl.SDL_Surface surf = GetSurfaceStructFromPtr(_surfacePtr);
			GC.KeepAlive(this);
			return new PixelValue(Sdl.SDL_MapRGBA(
				surf.format, 
				color.R, 
				color.G, 
				color.B,
				color.A));
		}
		
		/// <summary>
		/// Maps an Sdl 32-bit pixel value to a color using the surface's 
		/// pixel format
		/// </summary>
		/// <param name="pixelValue">The pixel value to map</param>
		/// <returns>
		/// A Color value for a pixel value in the surface's format
		/// </returns>
		public System.Drawing.Color GetColor(PixelValue pixelValue) {
			byte r, g, b, a;
			Sdl.SDL_Surface surf = this.GetSurfaceStructFromPtr(_surfacePtr);
			GC.KeepAlive(this);
			Sdl.SDL_GetRGBA(pixelValue.Value, surf.format, out r, out g, out b, out a);
			GC.KeepAlive(this);
			return System.Drawing.Color.FromArgb(a, r, g, b);
		}

		/// <summary>
		/// Create a surface with the same pixel format as this one
		/// </summary>
		/// <param name="width">The width of the new surface</param>
		/// <param name="height">The height of the new surface</param>
		/// <param name="hardware">
		/// Flag indicating whether to attempt to place the surface in 
		/// video memory
		/// </param>
		/// <returns>A new surface</returns>
		public Surface CreateCompatibleSurface(
			int width, int height, bool hardware) 
		{
			int flag;
			if (hardware)
			{
				flag = Sdl.SDL_HWSURFACE;
			}
			else
			{
				flag = Sdl.SDL_SWSURFACE;
			}
			
			Sdl.SDL_Surface surf = GetSurfaceStructFromPtr(_surfacePtr);
			Sdl.SDL_PixelFormat pixelFormat = 
				(Sdl.SDL_PixelFormat)Marshal.PtrToStructure(
				surf.format, typeof(Sdl.SDL_PixelFormat));
			IntPtr intPtr = Sdl.SDL_CreateRGBSurface(
				flag,
				width, 
				height, 
				pixelFormat.BitsPerPixel,
				pixelFormat.Rmask, 
				pixelFormat.Gmask, 
				pixelFormat.Bmask, 
				pixelFormat.Amask);
			GC.KeepAlive(this);

			IntPtr intPtrRet = Sdl.SDL_ConvertSurface(
				_surfacePtr, surf.format, flag);
			GC.KeepAlive(this);
			Sdl.SDL_FreeSurface(ref intPtr);
			return new Surface(intPtrRet, true);
		}

		/// <summary>
		/// Converts an existing surface to the same pixel format as this one
		/// </summary>
		/// <param name="toConvert">The surface to convert</param>
		/// <param name="hardware">
		/// A flag indicating whether or not to 
		/// attempt to place the new surface in video memory
		/// </param>
		/// <returns>The new surface</returns>
		public Surface ConvertSurface(Surface toConvert, bool hardware) {
			Sdl.SDL_Surface surf = this.GetSurfaceStructFromPtr(_surfacePtr);
			IntPtr ret = Sdl.SDL_ConvertSurface(toConvert._surfacePtr, surf.format, hardware?(int)Sdl.SDL_HWSURFACE:(int)Sdl.SDL_SWSURFACE);
			GC.KeepAlive(this);
			if (ret == IntPtr.Zero)
			{
				throw SdlException.Generate();
			}
			return new Surface(ret, true);
		}

		/// <summary>
		/// Copies this surface to a new surface with the format of 
		/// the display window
		/// </summary>
		/// <returns>A copy of this surface</returns>
		public Surface DisplayFormat() {
			IntPtr surfPtr = Sdl.SDL_DisplayFormat(_surfacePtr);
			GC.KeepAlive(this);
			if (surfPtr == IntPtr.Zero)
			{
				throw SdlException.Generate();
			}
			return new Surface(surfPtr, true);
		}

		/// <summary>
		/// Gets the size of the surface
		/// </summary>
		public System.Drawing.Size Size {
			get { 
				Sdl.SDL_Surface surf = GetSurfaceStructFromPtr(_surfacePtr);
				GC.KeepAlive(this);
				return new System.Drawing.Size(surf.w, surf.h); 
			}
		}

		/// <summary>
		/// Gets the width of the surface
		/// </summary>
		public int Width { 
			get
			{ 
				Sdl.SDL_Surface surf = GetSurfaceStructFromPtr(_surfacePtr);
				GC.KeepAlive(this);
				return (int)surf.w; 
			} 
		}

		/// <summary>
		/// Gets the height of the surface
		/// </summary>
		public int Height { 
			get
			{ 
				Sdl.SDL_Surface surf = GetSurfaceStructFromPtr(_surfacePtr);
				GC.KeepAlive(this);
				return (int)surf.h; 
			} 
		}

		/// <summary>
		/// Copies this surface to another surface
		/// </summary>
		/// <param name="destinationSurface">
		/// The surface to copy to
		/// </param>
		/// <param name="destinationRectangle">
		/// The rectangle coordinites on the destination surface to copy to
		/// </param>
		public void Blit(Surface destinationSurface, System.Drawing.Rectangle destinationRectangle) {
			Sdl.SDL_Rect s = this.ConvertRecttoSDLRect(new System.Drawing.Rectangle(
				new System.Drawing.Point(0, 0), this.Size)),
				d = this.ConvertRecttoSDLRect(destinationRectangle);
			int result = Sdl.SDL_BlitSurface(_surfacePtr, ref s, destinationSurface._surfacePtr, ref d);
			GC.KeepAlive(this);
			if (result != 0)
			{
				throw SdlException.Generate();
			}
		}

		/// <summary>
		/// Copies a portion of this surface to another surface
		/// </summary>
		/// <param name="sourceRectangle">
		/// The rectangle coordinites of this surface to copy from
		/// </param>
		/// <param name="destinationSurface">The surface to copy to</param>
		/// <param name="destinationRectangle">
		/// The rectangle coordinates on the destination surface to copy to
		/// </param>
		public void Blit(
			System.Drawing.Rectangle sourceRectangle, 
			Surface destinationSurface, 
			System.Drawing.Rectangle destinationRectangle) 
		{
			Sdl.SDL_Rect s = this.ConvertRecttoSDLRect(sourceRectangle); 
			Sdl.SDL_Rect d = this.ConvertRecttoSDLRect(destinationRectangle);
			int result = Sdl.SDL_BlitSurface(_surfacePtr, ref s, destinationSurface._surfacePtr, ref d);
			GC.KeepAlive(this);
			if (result!= 0)
			{
				throw SdlException.Generate();
			}
		}

		/// <summary>
		/// Locks a surface to allow direct pixel manipulation
		/// </summary>
		public void Lock() {
			if (MustLock) {
				int result = Sdl.SDL_LockSurface(_surfacePtr);
				GC.KeepAlive(this);
				if (result != 0)
				{
					throw SdlException.Generate();
				}
			}
		}
		/// <summary>
		/// Gets a pointer to the raw pixel data of the surface
		/// </summary>
		public IntPtr Pixels {
			get 
			{ 
				Sdl.SDL_Surface surf = 
					this.GetSurfaceStructFromPtr(_surfacePtr);
				GC.KeepAlive(this);
				return surf.pixels; 
			}
		}

		/// <summary>
		/// Unlocks a surface which has been locked.
		/// </summary>
		public void Unlock() {
			if (MustLock) {
				int result = Sdl.SDL_UnlockSurface(_surfacePtr);
				GC.KeepAlive(this);
				if (result != 0)
				{
					throw SdlException.Generate();
				}
			}
		}

		/// <summary>
		/// Gets a flag indicating if it is neccessary to lock 
		/// the surface before accessing its pixel data directly
		/// </summary>
		public bool MustLock 
		{
			get 
			{ 
				int result = Sdl.SDL_MUSTLOCK(_surfacePtr);
				GC.KeepAlive(this);
				if (result== 1)
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
		/// Saves the surface to disk as a .bmp file
		/// </summary>
		/// <param name="file">The filename to save to</param>
		public void SaveBmp(string file) 
		{
			int result = Sdl.SDL_SaveBMP(_surfacePtr, file);
			GC.KeepAlive(this);
			if (result!=0)
			{
				throw SdlException.Generate();
			}
		}

		/// <summary>
		/// Sets a colorkey for the surface
		/// </summary>
		/// <param name="transparent">The transparent color</param>
		/// <param name="accelerationRle">
		/// A flag indicating whether or not to use hardware acceleration for RLE
		/// </param>
		public void SetColorKey(Color transparent, bool accelerationRle) 
		{
			SetColorKey(this.MapColor(transparent), accelerationRle);
		}

		/// <summary>
		/// Sets a colorkey for the surface
		/// </summary>
		/// <param name="key">The transparent pixel value</param>
		/// <param name="accelerationRle">
		/// A flag indicating whether or not to use hardware acceleration for RLE
		/// </param>
		public void SetColorKey(PixelValue key, bool accelerationRle) 
		{
			int flag = Sdl.SDL_SRCCOLORKEY;
			if (accelerationRle)
			{
				flag |= Sdl.SDL_RLEACCELOK;
			}
			int result = Sdl.SDL_SetColorKey(_surfacePtr, (int)flag, key.Value);
			GC.KeepAlive(this);
			if (result != 0)
			{
				throw SdlException.Generate();
			}
		}

		/// <summary>
		/// Clears the colorkey for the surface
		/// </summary>
		public void ClearColorKey() 
		{
			int result = Sdl.SDL_SetColorKey(_surfacePtr, 0, 0);
			GC.KeepAlive(this);
			if (result != 0)
			{
				throw SdlException.Generate();
			}
		}

		/// <summary>
		/// Sets/Gets the clipping rectangle for the surface
		/// </summary>
		public System.Drawing.Rectangle ClipRectangle
		{
			get
			{
				Sdl.SDL_Rect sdlrect = 
					this.ConvertRecttoSDLRect(new System.Drawing.Rectangle());
				Sdl.SDL_GetClipRect(_surfacePtr, ref sdlrect);
				GC.KeepAlive(this);
				return new System.Drawing.Rectangle(sdlrect.x, sdlrect.y, sdlrect.w, sdlrect.h);
			}
			set
			{
				Sdl.SDL_Rect sdlrect = this.ConvertRecttoSDLRect(value);
				Sdl.SDL_SetClipRect(_surfacePtr, ref sdlrect);
				GC.KeepAlive(this);
			}
		}

		/// <summary>
		/// Gets the number of bytes per pixel for this surface
		/// </summary>
		public int BytesPerPixel {
			get
			{ 
				Sdl.SDL_Surface surf = 
					this.GetSurfaceStructFromPtr(_surfacePtr);
				GC.KeepAlive(this);

				Sdl.SDL_PixelFormat pixelFormat = 
					(Sdl.SDL_PixelFormat)Marshal.PtrToStructure(
					surf.format, typeof(Sdl.SDL_PixelFormat));

				return pixelFormat.BytesPerPixel; 
			}
		}

//		//copied from http://cone3d.gamedev.net/cgi-bin/index.pl?page=tutorials/gfxsdl/tut1
//		/// <summary>
//		/// Draws a pixel to this surface - uses 1,2 or 4 BytesPerPixel modes.
//		/// Call Lock() before calling this method.
//		/// </summary>
//		/// <param name="x">The x coordinate of where to plot the pixel</param>
//		/// <param name="y">The y coordinate of where to plot the pixel</param>
//		/// <param name="c">The color of the pixel</param>
//		public void DrawPixel(int x, int y, System.Drawing.Color c) {
//			PixelValue color = this.MapColor(c);
//			Sdl.SDL_Surface surface = GetSurfaceStructFromPtr(_surface);
//
//			switch (surface.format->BytesPerPixel) {
//				case 1: // Assuming 8-bpp
//				{
//					byte *bufp;
//					bufp = (byte *)_surface->pixels.ToPointer() + y*_surface->pitch + x;
//					*bufp = (byte)color.Value;
//				}
//				break;
//				case 2: // Probably 15-bpp or 16-bpp
//				{
//					UInt16 *bufp;
//					bufp = (short *)_surface->pixels.ToPointer() + y*_surface->pitch/2 + x;
//					*bufp = (short)color.Value;
//				}
//				break;
//				case 3: // Slow 24-bpp mode, usually not used
//				{/*
//					byte *bufp;
//					bufp = (byte *)_surface->pixels + y*_surface->pitch + x * 3;
//					if(SDL_BYTEORDER == SDL_LIL_ENDIAN)
//					{
//						bufp[0] = color;
//						bufp[1] = color >> 8;
//						bufp[2] = color >> 16;
//					} 
//					else 
//					{
//						bufp[2] = color;
//						bufp[1] = color >> 8;
//						bufp[0] = color >> 16;
//					}
//				*/}
//				break;
//				case 4: // Probably 32-bpp
//				{
//					int *bufp;
//					bufp = (int *)_surface->pixels.ToPointer() + y*_surface->pitch/4 + x;
//					*bufp = color.Value;
//				}
//				break;
//			}
//		}

//		/// <summary>
//		/// Flips the rows of a surface, for use in as an OpenGL texture for example
//		/// </summary>
//		public void FlipVertical() {
//
//			int first = 0, second = Height-1;
//			int pitch = Pitch;
//			IntPtr temp = Marshal.AllocHGlobal(pitch);
//			byte *tempp = (byte *)temp.ToPointer();
//			byte *pixels = (byte *)_surface->pixels.ToPointer();
//
//			Lock();
//			while (first < second) {
//				UnmanagedCopy(pixels + first * pitch, tempp, pitch);
//				UnmanagedCopy(pixels + second * pitch, pixels + first * pitch, pitch);
//				UnmanagedCopy(tempp, pixels + second * pitch, pitch);
//				first++;
//				second--;
//			}
//			Unlock();
//
//			Marshal.FreeHGlobal(temp);
//
//		}

//		private void UnmanagedCopy(byte *src, byte *dest, int len) {
//			while (len > 0) {
//				*(dest + (len - 1)) = *(src + (len - 1));
//				len--;
//			}
//		}

		/// <summary>
		/// returns the length of a scanline in bytes
		/// </summary>
		public short Pitch 
		{ 
			get 
			{ 
				Sdl.SDL_Surface surf = this.GetSurfaceStructFromPtr(_surfacePtr);
				GC.KeepAlive(this);
				return surf.pitch; 
			} 
		}

//		/// <summary>
//		/// Attempting to code GetPixel. The getter equivalent of PutPixel.
//		/// Mridul - Added for my code...
//		/// </summary>
//		/// <param name="x">The x co-ordinate of the surface</param>
//		/// <param name="y">The y co-ordinate of the surface</param>
//		public int GetPixel(int x, int y) {
//			int bpp = _surface->format->BytesPerPixel;
//
//			byte* p = (byte*)Pixels + y * Pitch + x * BytesPerPixel;
//
//			switch(bpp) {
//				case 1: //Assuming 8-bpp
//				{
//					byte *bufp;
//					bufp = (byte *)_surface->pixels.ToPointer() + y*_surface->pitch + x;
//					return *bufp;
//				}
//				case 2:
//				{
//					short *bufp;
//					bufp = (short *)_surface->pixels.ToPointer() + y*_surface->pitch/2 + x;
//					return *bufp;
//				}
//				case 3: //Assuming this is not going to be used much... 
//				{/*
//					byte *bufp;
//					bufp = (byte *)screen->pixels + y*screen->pitch + x * 3;
//					if(SDL_BYTEORDER == SDL_LIL_ENDIAN)
//					{
//						return p[0] << 16 | p[1] << 8 | p[2];
//					}
//					else
//					{
//						return p[0] | p[1] << 8 | p[2] << 16;
//					}
//				*/return 0;
//				}
//				case 4:
//				{
//					int *bufp;
//					bufp = (int *)_surface->pixels.ToPointer() + y*_surface->pitch/4 + x;
//					return *bufp;
//				}
//				default: //Should never come here... but kya kare...
//				{
//					return 0;
//				}
//			}
//		}
//
		/// <summary>
		/// Sets the alpha value of a surface
		/// </summary>
		/// <param name="flag">The alpha flags</param>
		/// <param name="alpha">The alpha value</param>
		public void SetAlpha(Alphas flag, byte alpha) 
		{
			int result = Sdl.SDL_SetAlpha(_surfacePtr, (int)flag, alpha);
			GC.KeepAlive(this);
			if (result != 0) 
			{
				throw SdlException.Generate();
			}
		}
	}
}
