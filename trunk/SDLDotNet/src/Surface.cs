/*
 * $RCSfile$
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
using System.IO;
using System.Drawing;
using System.Runtime.InteropServices;

namespace SDLDotNet {

	/// <summary>
	/// Enum for setting surface alpha
	/// </summary>
	public enum Alpha {
		/// <summary>
		/// Equivalent to SDL_RLEACC
		/// </summary>
		RLEAccel = 0x00004000,

		/// <summary>
		/// Equivalent to SDL_SRCALPHA
		/// </summary>
		Source = 0x00010000
	}
	
	/// <summary>
	/// An opaque structure representing an SDL pixel value
	/// </summary>
	public struct PixelValue {

		private uint _val;
		/// <summary>
		/// Creates a pixel value
		/// </summary>
		/// <param name="val">The raw pixel value to use</param>
		public PixelValue(uint val) {
			_val = val;
		}

		/// <summary>
		/// Gets or sets the pixel value
		/// </summary>
		public uint Value {
			get { return _val; }
			set { _val = value; }
		}
	}

	/// <summary>
	/// Represents an SDL drawing surface.
	/// You can create instances of this class with the methods in the Video object
	/// </summary>
	unsafe public class Surface : IDisposable {
		private bool _freeondispose;
		private bool _disposed;
		private Natives.SDL_Surface *_surface;

		private Surface(Natives.SDL_Surface *surface, bool freeondispose) {
			_freeondispose = freeondispose;
			_surface = surface;
			_disposed = false;
		}

		internal static Surface FromScreenPtr(Natives.SDL_Surface *surface) {
			return new Surface(surface, false);
		}

		internal static Surface FromPtr(Natives.SDL_Surface *surface) {
			return new Surface(surface, true);
		}

		/// <summary>
		/// Returns the native SDL Surface pointer
		/// </summary>
		/// <returns>An IntPtr pointing at the SDL surface reference</returns>
		public IntPtr GetPtr() {
			return new IntPtr((void *)_surface);
		}

		internal static Surface FromBMPFile(string file) {
			Natives.SDL_Surface *surf = Natives.SDL_LoadBMP_RW(Natives.SDL_RWFromFile(file, "rb"), 1);
			if (surf == null)
				throw SDLException.Generate();
			return new Surface(surf, true);
		}

		internal static Surface FromBitmap(System.Drawing.Bitmap bitmap) {
			if(Environment.Version.ToString() == "0.0")
				throw new NotSupportedException("Method not supported on Mono");
#if !__MONO__
			MemoryStream stream = new MemoryStream();
			bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
			byte[] arr = stream.ToArray();
			Natives.SDL_Surface *surf = Natives.SDL_LoadBMP_RW(Natives.SDL_RWFromMem(arr, arr.Length), 1);
			if (surf == null)
				throw SDLException.Generate();
			return new Surface(surf, true);
#else
			throw new NotSupportedException("Mono compiled, method not supported");
#endif
		}

		internal static Surface FromBitmap(byte[] arr) {
			Natives.SDL_Surface *surf = Natives.SDL_LoadBMP_RW(Natives.SDL_RWFromMem(arr, arr.Length), 1);
			if (surf == null)
				throw SDLException.Generate();
			return new Surface(surf, true);
		}

		/// <summary>
		/// Allows an Object to attempt to free resources 
		/// and perform other cleanup operations before the Object 
		/// is reclaimed by garbage collection.
		/// </summary>
		~Surface() 
		{
			if (_freeondispose)
				Natives.SDL_FreeSurface(_surface);
		}

		/// <summary>
		/// Destroys the surface object and frees its memory
		/// </summary>
		public void Dispose() {
			if (_freeondispose && !_disposed) {
				_disposed = true;
				//Natives.SDL_FreeSurface(_surface); //Scott Hilleard: fixes some crashes
				GC.SuppressFinalize(this);
			}
		}

		/// <summary>
		/// If the surface is double-buffered, this method will flip the back buffer onto the screen
		/// </summary>
		public void Flip() {
			if (Natives.SDL_Flip(_surface) != 0)
				throw SDLException.Generate();
		}

		/// <summary>
		/// Draws a rectangle onto the surface
		/// </summary>
		/// <param name="rect">The rectangle coordinites</param>
		/// <param name="color">The color to draw</param>
		public void FillRect(System.Drawing.Rectangle rect, System.Drawing.Color color) {
			Natives.SDL_Rect sdlrect = new Natives.SDL_Rect(rect);
			if (Natives.SDL_FillRect(_surface, &sdlrect, MapColor(color).Value) != 0)
				throw SDLException.Generate();
		}


		/// <summary>
		/// Maps a logical color to a pixel value in the surface's pixel format
		/// </summary>
		/// <param name="color">The color to map</param>
		/// <returns>A pixel value in the surface's format</returns>
		public PixelValue MapColor(System.Drawing.Color color) 
		{
			return new PixelValue(Natives.SDL_MapRGBA(_surface->format, color.R, color.G, color.B, color.A));
		}
		
		/// <summary>
		/// Maps an SDL 32-bit pixel value to a color using the surface's pixel format
		/// </summary>
		/// <param name="val">The pixel value to map</param>
		/// <returns>A Color value for a pixel value in the surface's format</returns>
		public System.Drawing.Color GetColor(PixelValue val) {
			byte r, g, b, a;
			Natives.SDL_GetRGBA(val.Value, _surface->format, out r, out g, out b, out a);
			return System.Drawing.Color.FromArgb(a, r, g, b);
		}

		/// <summary>
		/// Create a surface with the same pixel format as this one
		/// </summary>
		/// <param name="width">The width of the new surface</param>
		/// <param name="height">The height of the new surface</param>
		/// <param name="hardware">Flag indicating whether to attempt to place the surface in video memory</param>
		/// <returns>A new surface</returns>
		public Surface CreateCompatibleSurface(int width, int height, bool hardware) {
			Natives.SDL_Surface *surf = Natives.SDL_CreateRGBSurface(hardware?(int)Natives.Video.HWSurface:(int)Natives.Video.SWSurface,
				width, height, _surface->format->BitsPerPixel,
				_surface->format->Rmask, _surface->format->Gmask, _surface->format->Bmask, _surface->format->Amask);
			if (surf == null)
				throw SDLException.Generate();
			Natives.SDL_Surface *ret = Natives.SDL_ConvertSurface(surf, _surface->format, hardware?(int)Natives.Video.HWSurface:(int)Natives.Video.SWSurface);
			Natives.SDL_FreeSurface(surf);
			if (ret == null)
				throw SDLException.Generate();
			return new Surface(ret, true);
		}

		/// <summary>
		/// Converts an existing surface to the same pixel format as this one
		/// </summary>
		/// <param name="toconvert">The surface to convert</param>
		/// <param name="hardware">A flag indicating whether or not to attempt to place the new surface in video memory</param>
		/// <returns>The new surface</returns>
		public Surface ConvertSurface(Surface toconvert, bool hardware) {
			Natives.SDL_Surface *ret = Natives.SDL_ConvertSurface(toconvert._surface, _surface->format, hardware?(int)Natives.Video.HWSurface:(int)Natives.Video.SWSurface);
			if (ret == null)
				throw SDLException.Generate();
			return new Surface(ret, true);
		}

		/// <summary>
		/// Copies this surface to a new surface with the format of the display window
		/// </summary>
		/// <returns>A copy of this surface</returns>
		public Surface DisplayFormat() {
			Natives.SDL_Surface *surf = Natives.SDL_DisplayFormat(_surface);
			if (surf == null)
				throw SDLException.Generate();
			return new Surface(surf, true);
		}

		/// <summary>
		/// Gets the size of the surface
		/// </summary>
		public System.Drawing.Size Size {
			get { return new System.Drawing.Size(_surface->w, _surface->h); }
		}

		/// <summary>
		/// Gets the width of the surface
		/// </summary>
		public int Width { get{ return (int)_surface->w; } }

		/// <summary>
		/// Gets the height of the surface
		/// </summary>
		public int Height { get{ return (int)_surface->h; } }

		/// <summary>
		/// Copies this surface to another surface
		/// </summary>
		/// <param name="dest">The surface to copy to</param>
		/// <param name="destrect">The rectangle coordinites on the destination surface to copy to</param>
		public void Blit(Surface dest, System.Drawing.Rectangle destrect) {
			Natives.SDL_Rect s = new Natives.SDL_Rect(new System.Drawing.Rectangle(new System.Drawing.Point(0, 0), this.Size)), d = new Natives.SDL_Rect(destrect);
			if (Natives.SDL_BlitSurface(_surface, &s, dest._surface, &d) != 0)
				throw SDLException.Generate();
		}
		/// <summary>
		/// Copies a portion of this surface to another surface
		/// </summary>
		/// <param name="srcrect">The rectangle coordinites of this surface to copy from</param>
		/// <param name="dest">The surface to copy to</param>
		/// <param name="destrect">The rectangle coordinites on the destination surface to copy to</param>
		public void Blit(System.Drawing.Rectangle srcrect, Surface dest, System.Drawing.Rectangle destrect) {
			Natives.SDL_Rect s = new Natives.SDL_Rect(srcrect), d = new Natives.SDL_Rect(destrect);
			if (Natives.SDL_BlitSurface(_surface, &s, dest._surface, &d) != 0)
				throw SDLException.Generate();
		}

		/// <summary>
		/// Locks a surface to allow direct pixel manipulation
		/// </summary>
		public void Lock() {
			if (MustLock) {
				if (Natives.SDL_LockSurface(_surface) != 0)
					throw SDLException.Generate();
			}
		}
		/// <summary>
		/// Gets a pointer to the raw pixel data of the surface
		/// </summary>
		public IntPtr Pixels {
			get { return _surface->pixels; }
		}
		/// <summary>
		/// Unlocks a surface which has been locked.
		/// </summary>
		public void Unlock() {
			if (MustLock) {
				if (Natives.SDL_UnlockSurface(_surface) != 0)
					throw SDLException.Generate();
			}
		}

		/// <summary>
		/// Gets a flag indicating if it is neccessary to lock the surface before accessing its pixel data directly
		/// </summary>
		public bool MustLock {
			get { return (_surface->offset != 0 || ((_surface->flags & (int)(Natives.Video.HWSurface|Natives.Video.AsyncBlit|Natives.Video.RLEAccel)) != 0)); }
		}

		/// <summary>
		/// Saves the surface to disk as a .bmp file
		/// </summary>
		/// <param name="file">The filename to save to</param>
		public void SaveBMP(string file) {
			if (Natives.SDL_SaveBMP_RW(_surface, Natives.SDL_RWFromFile(file, "wb"), 1) != 0)
				throw SDLException.Generate();
		}

		/// <summary>
		/// Sets a colorkey for the surface
		/// </summary>
		/// <param name="transparent">The transparent color</param>
		/// <param name="accelrle">A flag indicating whether or not to use hardware acceleration for RLE</param>
		public void SetColorKey(Color transparent, bool accelrle) {
			SetColorKey(this.MapColor(transparent), accelrle);
		}

		/// <summary>
		/// Sets a colorkey for the surface
		/// </summary>
		/// <param name="key">The transparent pixel value</param>
		/// <param name="accelrle">A flag indicating whether or not to use hardware acceleration for RLE</param>
		public void SetColorKey(PixelValue key, bool accelrle) {
			Natives.ColorKey flag = Natives.ColorKey.SrcColorKey;
			if (accelrle)
				flag |= Natives.ColorKey.RLEAccelOK;
			if (Natives.SDL_SetColorKey(_surface, (int)flag, key.Value) != 0)
				throw SDLException.Generate();
		}
		/// <summary>
		/// Clears the colorkey for the surface
		/// </summary>
		public void ClearColorKey() {
			if (Natives.SDL_SetColorKey(_surface, 0, 0) != 0)
				throw SDLException.Generate();
		}

		/// <summary>
		/// Sets the clipping rectangle for the surface
		/// </summary>
		/// <param name="rect">The rectangle coordinites to clip to</param>
		public void SetClipRect(System.Drawing.Rectangle rect) {
			Natives.SDL_Rect sdlrect = new Natives.SDL_Rect(rect);
			Natives.SDL_SetClipRect(_surface, &sdlrect);
		}
		/// <summary>
		/// Gets the clipping rectangle for the surface
		/// </summary>
		/// <returns>The current clipping coordinites</returns>
		public System.Drawing.Rectangle GetClipRect() {
			Natives.SDL_Rect sdlrect = new Natives.SDL_Rect(new System.Drawing.Rectangle());
			Natives.SDL_GetClipRect(_surface, &sdlrect);
			return new System.Drawing.Rectangle(sdlrect.x, sdlrect.y, sdlrect.w, sdlrect.h);
		}

		/// <summary>
		/// Gets the number of bytes per pixel for this surface
		/// </summary>
		public int BytesPerPixel {
			get{ return _surface->format->BytesPerPixel; }
		}

		//copied from http://cone3d.gamedev.net/cgi-bin/index.pl?page=tutorials/gfxsdl/tut1
		/// <summary>
		/// Draws a pixel to this surface - uses 1,2 or 4 BytesPerPixel modes.
		/// Call Lock() before calling this method.
		/// </summary>
		/// <param name="x">The x coordinate of where to plot the pixel</param>
		/// <param name="y">The y coordinate of where to plot the pixel</param>
		/// <param name="c">The color of the pixel</param>
		public void DrawPixel(int x, int y, System.Drawing.Color c) {
			PixelValue color = this.MapColor(c);
			switch (_surface->format->BytesPerPixel) {
				case 1: // Assuming 8-bpp
				{
					byte *bufp;
					bufp = (byte *)_surface->pixels.ToPointer() + y*_surface->pitch + x;
					*bufp = (byte)color.Value;
				}
				break;
				case 2: // Probably 15-bpp or 16-bpp
				{
					UInt16 *bufp;
					bufp = (UInt16 *)_surface->pixels.ToPointer() + y*_surface->pitch/2 + x;
					*bufp = (UInt16)color.Value;
				}
				break;
				case 3: // Slow 24-bpp mode, usually not used
				{/*
					byte *bufp;
					bufp = (byte *)_surface->pixels + y*_surface->pitch + x * 3;
					if(SDL_BYTEORDER == SDL_LIL_ENDIAN)
					{
						bufp[0] = color;
						bufp[1] = color >> 8;
						bufp[2] = color >> 16;
					} 
					else 
					{
						bufp[2] = color;
						bufp[1] = color >> 8;
						bufp[0] = color >> 16;
					}
				*/}
				break;
				case 4: // Probably 32-bpp
				{
					UInt32 *bufp;
					bufp = (UInt32 *)_surface->pixels.ToPointer() + y*_surface->pitch/4 + x;
					*bufp = color.Value;
				}
				break;
			}
		}

		/// <summary>
		/// Flips the rows of a surface, for use in as an OpenGL texture for example
		/// </summary>
		public void FlipVertical() {

			int first = 0, second = Height-1;
			int pitch = Pitch;
			IntPtr temp = Marshal.AllocHGlobal(pitch);
			byte *tempp = (byte *)temp.ToPointer();
			byte *pixels = (byte *)_surface->pixels.ToPointer();

			Lock();
			while (first < second) {
				UnmanagedCopy(pixels + first * pitch, tempp, pitch);
				UnmanagedCopy(pixels + second * pitch, pixels + first * pitch, pitch);
				UnmanagedCopy(tempp, pixels + second * pitch, pitch);
				first++;
				second--;
			}
			Unlock();

			Marshal.FreeHGlobal(temp);

		}

		private void UnmanagedCopy(byte *src, byte *dest, int len) {
			while (len > 0) {
				*(dest + (len - 1)) = *(src + (len - 1));
				len--;
			}
		}

		/// <summary>
		/// Returns the length of a scanline in bytes
		/// </summary>
		public ushort Pitch { get { return _surface->pitch; } }

		/// <summary>
		/// Attempting to code GetPixel. The getter equivalent of PutPixel.
		/// Mridul - Added for my code...
		/// </summary>
		/// <param name="x">The x co-ordinate of the surface</param>
		/// <param name="y">The y co-ordinate of the surface</param>
		public UInt32 GetPixel(int x, int y) {
			int bpp = _surface->format->BytesPerPixel;

			byte* p = (byte*)Pixels + y * Pitch + x * BytesPerPixel;

			switch(bpp) {
				case 1: //Assuming 8-bpp
				{
					byte *bufp;
					bufp = (byte *)_surface->pixels.ToPointer() + y*_surface->pitch + x;
					return *bufp;
				}
				case 2:
				{
					UInt16 *bufp;
					bufp = (UInt16 *)_surface->pixels.ToPointer() + y*_surface->pitch/2 + x;
					return *bufp;
				}
				case 3: //Assuming this is not going to be used much... 
				{/*
					byte *bufp;
					bufp = (byte *)screen->pixels + y*screen->pitch + x * 3;
					if(SDL_BYTEORDER == SDL_LIL_ENDIAN)
					{
						return p[0] << 16 | p[1] << 8 | p[2];
					}
					else
					{
						return p[0] | p[1] << 8 | p[2] << 16;
					}
				*/return 0;
				}
				case 4:
				{
					UInt32 *bufp;
					bufp = (UInt32 *)_surface->pixels.ToPointer() + y*_surface->pitch/4 + x;
					return *bufp;
				}
				default: //Should never come here... but kya kare...
				{
					return 0;
				}
			}
		}

		/// <summary>
		/// Sets the alpha value of a surface
		/// </summary>
		/// <param name="flag">The alpha flags</param>
		/// <param name="alpha">The alpha value</param>
		public void SetAlpha(Alpha flag, byte alpha) {
			if ((Natives.SDL_SetAlpha(_surface, (UInt32)flag, alpha)) != 0) {
				throw SDLException.Generate();
			}
		}
	}
}
