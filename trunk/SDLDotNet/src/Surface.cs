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
using System.Globalization;

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
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return String.Format(CultureInfo.CurrentCulture, "({0})", val);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			if (obj.GetType() != typeof(PixelValue))
				return false;
                
			PixelValue pixelValue = (PixelValue)obj;   
			return (this.val == pixelValue.val);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="pixelValue1"></param>
		/// <param name="pixelValue2"></param>
		/// <returns></returns>
		public static bool operator== (
			PixelValue pixelValue1, 
			PixelValue pixelValue2)
		{
			return (pixelValue1.val == pixelValue2.val);
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="pixelValue1"></param>
		/// <param name="pixelValue2"></param>
		/// <returns></returns>
		public static bool operator!= (
			PixelValue pixelValue1, 
			PixelValue pixelValue2)
		{
			return !(pixelValue1 == pixelValue2);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode()
		{
			return val;

		}
	}

	/// <summary>
	/// Represents an Sdl drawing surface.
	/// You can create instances of this class with the methods in the Video 
	/// object.
	/// </summary>
	public class Surface : BaseSdlResource
	{
		private bool disposed = false;
		private IntPtr handle;

		#region Constructors and Destructors
		/// <summary>
		/// 
		/// </summary>
		/// <param name="handle"></param>
		internal Surface(IntPtr handle) 
		{
			this.handle = handle;
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
			handle = surfPtr;
		}
	
		/// <summary>
		/// Allows an Object to attempt to free resources 
		/// and perform other cleanup operations before the Object 
		/// is reclaimed by garbage collection.
		/// </summary>
		~Surface() 
		{
			Dispose(false);
		}
		#endregion Constructors and Destructors

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
		/// Closes Surface handle
		/// </summary>
		protected override void CloseHandle(IntPtr handleToClose) 
		{
			Sdl.SDL_FreeSurface(handleToClose);
			GC.KeepAlive(this);
			handleToClose = IntPtr.Zero;
		}


		internal static Surface FromScreenPtr(IntPtr surfacePtr) 
		{
			return new Surface(surfacePtr);
		}

		//		internal static Surface FromPtr(IntPtr surfacePtr) {
		//			return new Surface(surfacePtr, true);
		//		}

		private Sdl.SDL_Surface GetSurfaceStructFromPtr(IntPtr ptr)
		{
			return (Sdl.SDL_Surface)Marshal.PtrToStructure(ptr, 
				typeof(Sdl.SDL_Surface));
		}

		private Sdl.SDL_PixelFormat GetFormatStructFromPtr(IntPtr ptr)
		{
			return (Sdl.SDL_PixelFormat)Marshal.PtrToStructure(ptr, 
				typeof(Sdl.SDL_PixelFormat));
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
				return handle;
			}
			set
			{
				GC.KeepAlive(this);
				handle = value;
			}
		}

		internal static Surface FromImageFile(string file) 
		{
			IntPtr surfPtr = SdlImage.IMG_Load(file);
			if (surfPtr == IntPtr.Zero)
			{
				throw SdlException.Generate();
			}
			return new Surface(surfPtr);
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
			return new Surface(surf);
		}

		internal static Surface FromBitmap(byte[] arr) 
		{
			IntPtr surf = Sdl.SDL_LoadBMP_RW(Sdl.SDL_RWFromMem(arr, arr.Length), 1);
			//GC.KeepAlive(this);
			if (surf == IntPtr.Zero)
			{
				throw SdlException.Generate();
			}
			return new Surface(surf);
		}

		/// <summary>
		/// If the surface is double-buffered, 
		/// this method will flip the back buffer onto the screen
		/// </summary>
		public void Flip() 
		{
			int result = Sdl.SDL_Flip(handle);
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

			int result = Sdl.SDL_FillRect(handle, ref sdlrect, MapColor(color).Value);
			GC.KeepAlive(this);
			if (result != (int) SdlFlag.Success)
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
				this.GetSurfaceStructFromPtr(handle);
			Sdl.SDL_Rect sdlrect = 
				new Sdl.SDL_Rect(0, 0, (short)surf.w, (short)surf.h);
			int result = Sdl.SDL_FillRect(handle, ref sdlrect, MapColor(color).Value);
			GC.KeepAlive(this);
			if (result != 0)
			{
				throw SdlException.Generate();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="circle"></param>
		/// <param name="color"></param>
		public void CreateFilledCircle(Circle circle, System.Drawing.Color color)
		{
			int result = SdlGfx.filledCircleRGBA(handle, circle.XPosition, circle.YPosition, circle.Radius, color.R, color.B, color.G,
				color.A);
			GC.KeepAlive(this);
			if (result != (int) SdlFlag.Success)
			{
				throw SdlException.Generate();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="circle"></param>
		/// <param name="color"></param>
		/// <param name="antiAlias"></param>
		public void CreateCircle(Circle circle, System.Drawing.Color color, bool antiAlias)
		{
			int result = 0;
			if (antiAlias)
			{
				result = SdlGfx.aacircleRGBA(handle, circle.XPosition, circle.YPosition, circle.Radius, color.R, color.B, color.G,
					color.A);
				GC.KeepAlive(this);
			}
			else
			{
				result = SdlGfx.circleRGBA(handle, circle.XPosition, circle.YPosition, circle.Radius, color.R, color.B, color.G,
					color.A);
				GC.KeepAlive(this);
			}
			if (result != (int) SdlFlag.Success)
			{
				throw SdlException.Generate();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="circle"></param>
		/// <param name="color"></param>
		public void CreateCircle(Circle circle, System.Drawing.Color color)
		{
			CreateCircle(circle, color, false);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ellipse"></param>
		/// <param name="color"></param>
		/// <param name="antiAlias"></param>
		public void CreateEllipse(Ellipse ellipse, System.Drawing.Color color, bool antiAlias)
		{
			int result = 0;

			if (antiAlias)
			{
				result = SdlGfx.aaellipseRGBA(
					handle, ellipse.XPosition, ellipse.YPosition, 
					ellipse.RadiusX, ellipse.RadiusY, 
					color.R, color.B, color.G,
					color.A);
				GC.KeepAlive(this);
			}
			else
			{
				result = SdlGfx.ellipseRGBA(
					handle, ellipse.XPosition, ellipse.YPosition, 
					ellipse.RadiusX, ellipse.RadiusY, 
					color.R, color.B, color.G,
					color.A);
				GC.KeepAlive(this);
			}
			if (result != (int) SdlFlag.Success)
			{
				throw SdlException.Generate();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ellipse"></param>
		/// <param name="color"></param>
		public void CreateEllipse(Ellipse ellipse, System.Drawing.Color color)
		{
			CreateEllipse(ellipse, color, false);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ellipse"></param>
		/// <param name="color"></param>
		public void CreateFilledEllipse(Ellipse ellipse, System.Drawing.Color color)
		{
			int result = SdlGfx.filledEllipseRGBA(handle, ellipse.XPosition, ellipse.YPosition, ellipse.RadiusX, ellipse.RadiusY,color.R, color.B, color.G,
				color.A);
			GC.KeepAlive(this);
			if (result != (int) SdlFlag.Success)
			{
				throw SdlException.Generate();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="line"></param>
		/// <param name="color"></param>
		/// <param name="antiAlias"></param>
		public void CreateLine(Line line, System.Drawing.Color color, bool antiAlias)
		{
			int result = 0;

			if (antiAlias)
			{
				result = SdlGfx.aalineRGBA(
					handle, line.XPosition1, line.YPosition1, 
					line.XPosition2, line.YPosition2, 
					color.R, color.B, color.G,
					color.A);
				GC.KeepAlive(this);
			}
			else
			{
				result = SdlGfx.lineRGBA(
					handle, line.XPosition1, line.YPosition1, 
					line.XPosition2, line.YPosition2, 
					color.R, color.B, color.G,
					color.A);
				GC.KeepAlive(this);
			}
			if (result != (int) SdlFlag.Success)
			{
				throw SdlException.Generate();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="line"></param>
		/// <param name="color"></param>
		public void CreateLine(Line line, System.Drawing.Color color)
		{
			CreateLine(line, color, false);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="triangle"></param>
		/// <param name="color"></param>
		/// <param name="antiAlias"></param>
		public void CreateTriangle(Triangle triangle, System.Drawing.Color color, bool antiAlias)
		{
			int result = 0;

			if (antiAlias)
			{
				result = SdlGfx.aatrigonRGBA(
					handle, triangle.XPosition1, triangle.YPosition1, 
					triangle.XPosition2, triangle.YPosition2, 
					triangle.XPosition3, triangle.YPosition3, 
					color.R, color.B, color.G,
					color.A);
				GC.KeepAlive(this);
			}
			else
			{
				result = SdlGfx.trigonRGBA(
					handle, triangle.XPosition1, triangle.YPosition1, 
					triangle.XPosition2, triangle.YPosition2, 
					triangle.XPosition3, triangle.YPosition3, 
					color.R, color.B, color.G,
					color.A);
				GC.KeepAlive(this);
			}
			if (result != (int) SdlFlag.Success)
			{
				throw SdlException.Generate();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="triangle"></param>
		/// <param name="color"></param>
		public void CreateTriangle(Triangle triangle, System.Drawing.Color color)
		{
			CreateTriangle(triangle, color, false);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="triangle"></param>
		/// <param name="color"></param>
		public void CreateFilledTriangle(Triangle triangle, System.Drawing.Color color)
		{
			int result = 0;
			result = SdlGfx.filledTrigonRGBA(
				handle, triangle.XPosition1, triangle.YPosition1, 
				triangle.XPosition2, triangle.YPosition2, 
				triangle.XPosition3, triangle.YPosition3, 
				color.R, color.B, color.G,
				color.A);
			GC.KeepAlive(this);

			if (result != (int) SdlFlag.Success)
			{
				throw SdlException.Generate();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="polygon"></param>
		/// <param name="color"></param>
		public void CreateFilledPolygon(Polygon polygon, System.Drawing.Color color)
		{
			int result = SdlGfx.filledPolygonRGBA(handle, polygon.XPositions(), polygon.YPositions(), polygon.NumberOfSides, color.R, color.B, color.G,
				color.A);
			GC.KeepAlive(this);
			if (result != 0)
			{
				throw SdlException.Generate();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="polygon"></param>
		/// <param name="color"></param>
		/// <param name="antiAlias"></param>
		public void CreatePolygon(Polygon polygon, System.Drawing.Color color, bool antiAlias)
		{
			int result = 0;
			if (antiAlias)
			{
				result = SdlGfx.aapolygonRGBA(handle, polygon.XPositions(), polygon.YPositions(), polygon.NumberOfSides, color.R, color.B, color.G,
					color.A);
				GC.KeepAlive(this);
			}
			else
			{
				result = SdlGfx.polygonRGBA(handle, polygon.XPositions(), polygon.YPositions(), polygon.NumberOfSides, color.R, color.B, color.G,
					color.A);
				GC.KeepAlive(this);
			}
			if (result != (int) SdlFlag.Success)
			{
				throw SdlException.Generate();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="polygon"></param>
		/// <param name="color"></param>
		public void CreatePolygon(Polygon polygon, System.Drawing.Color color)
		{
			CreatePolygon(polygon, color, false);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="pie"></param>
		/// <param name="color"></param>
		public void CreatePie(Pie pie, System.Drawing.Color color)
		{
			int result = 0;

			result = SdlGfx.pieRGBA(
				handle, pie.XPosition, pie.YPosition, 
				pie.Radius,
				pie.StartingPoint, pie.EndingPoint, 
				color.R, color.B, color.G,
				color.A);
			GC.KeepAlive(this);

			if (result != (int) SdlFlag.Success)
			{
				throw SdlException.Generate();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="pie"></param>
		/// <param name="color"></param>
		public void CreateFilledPie(Pie pie, System.Drawing.Color color)
		{
			int result = SdlGfx.filledPieRGBA(handle, pie.XPosition, pie.YPosition, pie.Radius, pie.StartingPoint, pie.EndingPoint,color.R, color.B, color.G,
				color.A);
			GC.KeepAlive(this);
			if (result != (int) SdlFlag.Success)
			{
				throw SdlException.Generate();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="bezier"></param>
		/// <param name="color"></param>
		public void CreateBezier(Bezier bezier, System.Drawing.Color color)
		{
			int result = 0;
			result = SdlGfx.bezierRGBA(
				handle, bezier.XPositions(), bezier.YPositions(), 
				bezier.NumberOfPoints, bezier.Steps, 
				color.R, color.B, color.G,
				color.A);
			GC.KeepAlive(this);
			if (result != (int) SdlFlag.Success)
			{
				throw SdlException.Generate();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="box"></param>
		/// <param name="color"></param>
		public void CreateBox(Box box, System.Drawing.Color color)
		{
			int result = 0;

			
			result = SdlGfx.rectangleRGBA(
				handle, box.XPosition1, box.YPosition1, 
				box.XPosition2, box.YPosition2, 
				color.R, color.B, color.G,
				color.A);
			GC.KeepAlive(this);
			
			if (result != (int) SdlFlag.Success)
			{
				throw SdlException.Generate();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="box"></param>
		/// <param name="color"></param>
		public void CreateFilledBox(Box box, System.Drawing.Color color)
		{
			int result = 0;

			
			result = SdlGfx.boxRGBA(
				handle, box.XPosition1, box.YPosition1, 
				box.XPosition2, box.YPosition2, 
				color.R, color.B, color.G,
				color.A);
			GC.KeepAlive(this);
			
			if (result != (int) SdlFlag.Success)
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
			Sdl.SDL_Surface surf = GetSurfaceStructFromPtr(handle);
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
		public System.Drawing.Color GetColor(PixelValue pixelValue) 
		{
			byte r, g, b, a;
			Sdl.SDL_Surface surf = this.GetSurfaceStructFromPtr(handle);
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
			
			Sdl.SDL_Surface surf = GetSurfaceStructFromPtr(handle);
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
				handle, surf.format, flag);
			GC.KeepAlive(this);
			//Sdl.SDL_FreeSurface(intPtr);
			return new Surface(intPtr);
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
		public Surface ConvertSurface(Surface toConvert, bool hardware) 
		{
			Sdl.SDL_Surface surf = this.GetSurfaceStructFromPtr(handle);
			IntPtr ret = Sdl.SDL_ConvertSurface(toConvert.handle, surf.format, hardware?(int)Sdl.SDL_HWSURFACE:(int)Sdl.SDL_SWSURFACE);
			GC.KeepAlive(this);
			if (ret == IntPtr.Zero)
			{
				throw SdlException.Generate();
			}
			return new Surface(ret);
		}

		/// <summary>
		/// Copies this surface to a new surface with the format of 
		/// the display window
		/// </summary>
		/// <returns>A copy of this surface</returns>
		public Surface DisplayFormat() 
		{
			IntPtr surfPtr = Sdl.SDL_DisplayFormat(handle);
			GC.KeepAlive(this);
			if (surfPtr == IntPtr.Zero)
			{
				throw SdlException.Generate();
			}
			return new Surface(surfPtr);
		}

		/// <summary>
		/// Gets the size of the surface
		/// </summary>
		public System.Drawing.Size Size 
		{
			get 
			{ 
				Sdl.SDL_Surface surf = GetSurfaceStructFromPtr(handle);
				GC.KeepAlive(this);
				return new System.Drawing.Size(surf.w, surf.h); 
			}
		}

		/// <summary>
		/// Gets the width of the surface
		/// </summary>
		public int Width 
		{ 
			get
			{ 
				Sdl.SDL_Surface surf = GetSurfaceStructFromPtr(handle);
				GC.KeepAlive(this);
				return (int)surf.w; 
			} 
		}

		/// <summary>
		/// Gets the height of the surface
		/// </summary>
		public int Height 
		{ 
			get
			{ 
				Sdl.SDL_Surface surf = GetSurfaceStructFromPtr(handle);
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
		/// The rectangle coordinates on the destination surface to copy to
		/// </param>
		public void Blit(Surface destinationSurface, System.Drawing.Rectangle destinationRectangle) 
		{
			Sdl.SDL_Rect s = this.ConvertRecttoSDLRect(new System.Drawing.Rectangle(
				new System.Drawing.Point(0, 0), this.Size)),
				d = this.ConvertRecttoSDLRect(destinationRectangle);
			int result = Sdl.SDL_BlitSurface(handle, ref s, destinationSurface.handle, ref d);
			GC.KeepAlive(this);
			if (result != (int) SdlFlag.Success)
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
			int result = Sdl.SDL_BlitSurface(handle, ref s, destinationSurface.handle, ref d);
			GC.KeepAlive(this);
			if (result!= (int) SdlFlag.Success)
			{
				throw SdlException.Generate();
			}
		}

		/// <summary>
		/// Locks a surface to allow direct pixel manipulation
		/// </summary>
		public void Lock() 
		{
			if (MustLock) 
			{
				int result = Sdl.SDL_LockSurface(handle);
				GC.KeepAlive(this);
				if (result != (int) SdlFlag.Success)
				{
					throw SdlException.Generate();
				}
			}
		}
		/// <summary>
		/// Gets a pointer to the raw pixel data of the surface
		/// </summary>
		public IntPtr Pixels 
		{
			get 
			{ 
				Sdl.SDL_Surface surf = 
					this.GetSurfaceStructFromPtr(handle);
				GC.KeepAlive(this);
				return surf.pixels; 
			}
		}

		/// <summary>
		/// Unlocks a surface which has been locked.
		/// </summary>
		public void Unlock() 
		{
			if (MustLock) 
			{
				int result = Sdl.SDL_UnlockSurface(handle);
				GC.KeepAlive(this);
				if (result != (int) SdlFlag.Success)
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
				int result = Sdl.SDL_MUSTLOCK(handle);
				GC.KeepAlive(this);
				if (result == 1)
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
			int result = Sdl.SDL_SaveBMP(handle, file);
			GC.KeepAlive(this);
			if (result != (int) SdlFlag.Success)
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
			int result = Sdl.SDL_SetColorKey(handle, (int)flag, key.Value);
			GC.KeepAlive(this);
			if (result != (int) SdlFlag.Success)
			{
				throw SdlException.Generate();
			}
		}

		/// <summary>
		/// Clears the colorkey for the surface
		/// </summary>
		public void ClearColorKey() 
		{
			int result = Sdl.SDL_SetColorKey(handle, 0, 0);
			GC.KeepAlive(this);
			if (result != (int) SdlFlag.Success)
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
				Sdl.SDL_GetClipRect(handle, ref sdlrect);
				GC.KeepAlive(this);
				return new System.Drawing.Rectangle(sdlrect.x, sdlrect.y, sdlrect.w, sdlrect.h);
			}
			set
			{
				Sdl.SDL_Rect sdlrect = this.ConvertRecttoSDLRect(value);
				Sdl.SDL_SetClipRect(handle, ref sdlrect);
				GC.KeepAlive(this);
			}
		}

		/// <summary>
		/// Gets the number of bytes per pixel for this surface
		/// </summary>
		public int BytesPerPixel 
		{
			get
			{ 
				Sdl.SDL_Surface surf = 
					this.GetSurfaceStructFromPtr(handle);
				GC.KeepAlive(this);

				Sdl.SDL_PixelFormat pixelFormat = 
					(Sdl.SDL_PixelFormat)Marshal.PtrToStructure(
					surf.format, typeof(Sdl.SDL_PixelFormat));

				return pixelFormat.BytesPerPixel; 
			}
		}

		/// <summary>
		/// Draws a pixel to this surface - uses 1,2 or 4 BytesPerPixel modes.
		/// Call Lock() before calling this method.
		/// </summary>
		/// <remarks>
		/// copied from http://cone3d.gamedev.net/cgi-bin/index.pl?page=tutorials/gfxsdl/tut1
		/// </remarks>
		/// <param name="x">The x coordinate of where to plot the pixel</param>
		/// <param name="y">The y coordinate of where to plot the pixel</param>
		/// <param name="color">The color of the pixel</param>
		public void DrawPixel(int x, int y, System.Drawing.Color color) 
		{
			PixelValue pixelColor = this.MapColor(color);
			Sdl.SDL_Surface surface = GetSurfaceStructFromPtr(handle);
			GC.KeepAlive(this);

			Sdl.SDL_PixelFormat format = 
				this.GetFormatStructFromPtr(surface.format);
			//Console.WriteLine("PixelFormat: " + format.BytesPerPixel.ToString());
			//Console.WriteLine("Pitch: " + surface.pitch.ToString());
			switch (format.BytesPerPixel) 
			{
				case 1: // Assuming 8-bpp
				{
					byte pixelColorValue = (byte) pixelColor.Value;
					//Console.WriteLine("bufp: " + pixelColor.Value.ToString());
					IntPtr pixelColorValuePtr = 
						new IntPtr(surface.pixels.ToInt32() + y*surface.pitch + 2*x);
					Marshal.WriteByte(pixelColorValuePtr, pixelColorValue);
				}
					break;
				case 2: // Probably 15-bpp or 16-bpp
				{
					short pixelColorValue = (short) pixelColor.Value;
					//Console.WriteLine("bufp: " + pixelColor.Value.ToString());
					IntPtr pixelColorValuePtr = 
						new IntPtr(surface.pixels.ToInt32() + y*surface.pitch + 2*x);
					Marshal.WriteInt16(pixelColorValuePtr, pixelColorValue);
				}
					break;
				case 3: // Slow 24-bpp mode, usually not used
				{
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
				}
					break;
				case 4: // Probably 32-bpp
				{
					int pixelColorValue = pixelColor.Value;
					//Console.WriteLine("bufp: " + pixelColor.Value.ToString());
					IntPtr pixelColorValuePtr = 
						new IntPtr(surface.pixels.ToInt32() + (y*surface.pitch + 4*x));
					Marshal.WriteInt32(pixelColorValuePtr, pixelColorValue);
				}
					break;
			}
		}

		/// <summary>
		/// Flips the rows of a surface, for use in an OpenGL texture for example
		/// </summary>
		public void FlipVertical() 
		{

			int first = 0;
			int second = this.Height-1;
			int pitch = this.Pitch;
			byte[] tempByte = new byte[pitch];
			byte[] firstByte = new byte[pitch];
			Sdl.SDL_Surface surface = this.GetSurfaceStructFromPtr(handle);
			GC.KeepAlive(this);

			IntPtr pixelsPtr = surface.pixels;

			Lock();
			while (first < second) 
			{
				//Take first scanline
				//Copy pointer data from scanline to tempByte array
				Marshal.Copy(new IntPtr(surface.pixels.ToInt32() + first * pitch), tempByte, 0, pitch);
				//Take last scanline
				//Copy pointer data from scanline to firstByte array
				Marshal.Copy(new IntPtr(surface.pixels.ToInt32() + second * pitch), firstByte, 0, pitch);
				//Take tempByte array
				//Copy pointer data from tempByte to last scanline
				Marshal.Copy(tempByte, 0, new IntPtr(surface.pixels.ToInt32() + second * pitch), pitch);
				//Take firstByte array
				//Copy pointer data from firstByte array to first scanline
				Marshal.Copy(firstByte, 0, new IntPtr(surface.pixels.ToInt32() + first * pitch), pitch);
				first++;
				second--;
			}
			Unlock();
		}

		/// <summary>
		/// Flips the columns of a surface, for use in an OpenGL texture for example
		/// </summary>
		public void FlipHorizontal() 
		{
			this.RotateSurface(270);
			this.FlipVertical();
			this.RotateSurface(90);
		}

		/// <summary>
		/// returns the length of a scanline in bytes
		/// </summary>
		public short Pitch 
		{ 
			get 
			{ 
				Sdl.SDL_Surface surf = this.GetSurfaceStructFromPtr(handle);
				GC.KeepAlive(this);
				return surf.pitch; 
			} 
		}

		/// <summary>
		/// Attempting to code GetPixel. The getter equivalent of DrawPixel.
		/// </summary>
		/// <param name="x">The x coordinate of the surface</param>
		/// <param name="y">The y coordinate of the surface</param>
		public int GetPixel(int x, int y) 
		{
			Sdl.SDL_Surface surface = GetSurfaceStructFromPtr(handle);
			GC.KeepAlive(this);

			Sdl.SDL_PixelFormat format = 
				this.GetFormatStructFromPtr(surface.format);

			int bytesPerPixel = format.BytesPerPixel;

			switch (bytesPerPixel) 
			{
				case 1: //Assuming 8-bpp
				{
					IntPtr pixelColorValuePtr = 
						new IntPtr(surface.pixels.ToInt32() + y*surface.pitch + 2*x);
					return Marshal.ReadInt32(pixelColorValuePtr);
				}
				case 2:
				{
					IntPtr pixelColorValuePtr = 
						new IntPtr(surface.pixels.ToInt32() + y*surface.pitch + 2*x);
					return Marshal.ReadInt32(pixelColorValuePtr);
				}
				case 3: //Assuming this is not going to be used much... 
				{
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
					return 0;
				}
				case 4:
				{
					IntPtr pixelColorValuePtr = 
						new IntPtr(surface.pixels.ToInt32() + (y*surface.pitch + 4*x));
					return Marshal.ReadInt32(pixelColorValuePtr);
				}
				default: //Should never come here
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
		public void SetAlpha(Alphas flag, byte alpha) 
		{
			int result = Sdl.SDL_SetAlpha(handle, (int)flag, alpha);
			GC.KeepAlive(this);
			if (result != (int) SdlFlag.Success) 
			{
				throw SdlException.Generate();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="degreesOfRotation"></param>
		/// <returns></returns>
		public void RotateSurface(int degreesOfRotation)
		{
			this.SurfacePointer = 
				SdlGfx.rotozoomSurface(this.SurfacePointer, degreesOfRotation, 1, SdlGfx.SMOOTHING_OFF);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="degreesOfRotation"></param>
		/// <param name="antiAlias"></param>
		/// <returns></returns>
		public void RotateSurface(int degreesOfRotation, bool antiAlias)
		{
			int antiAliasParameter = SdlGfx.SMOOTHING_OFF;
			if (antiAlias == true)
			{
				antiAliasParameter = SdlGfx.SMOOTHING_ON;
			}
			
			this.SurfacePointer = 
				SdlGfx.rotozoomSurface(
				this.SurfacePointer, 
				degreesOfRotation, 
				1, 
				antiAliasParameter);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="degreesOfRotation"></param>
		/// <param name="zoom"></param>
		/// <returns></returns>
		public void RotateAndZoomSurface(int degreesOfRotation, double zoom)
		{
			this.SurfacePointer = 
				SdlGfx.rotozoomSurface(this.SurfacePointer, degreesOfRotation, zoom, SdlGfx.SMOOTHING_OFF);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="degreesOfRotation"></param>
		/// <param name="zoom"></param>
		/// <param name="antiAlias"></param>
		/// <returns></returns>
		public void RotateAndZoomSurface(int degreesOfRotation, 
			double zoom, bool antiAlias)
		{
			int antiAliasParameter = SdlGfx.SMOOTHING_OFF;
			if (antiAlias == true)
			{
				antiAliasParameter = SdlGfx.SMOOTHING_ON;
			}
			
			this.SurfacePointer = 
				SdlGfx.rotozoomSurface(
				this.SurfacePointer, 
				degreesOfRotation, 
				zoom, 
				antiAliasParameter);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="zoomX"></param>
		/// <param name="zoomY"></param>
		/// <returns></returns>
		public void ZoomSurface(double zoomX, double zoomY)
		{
			this.SurfacePointer = 
				SdlGfx.zoomSurface(this.SurfacePointer, zoomX, zoomY, SdlGfx.SMOOTHING_OFF);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="zoomX"></param>
		/// <param name="zoomY"></param>
		/// <param name="antiAlias"></param>
		/// <returns></returns>
		public void ZoomSurface(double zoomX, double zoomY, bool antiAlias)
		{
			int antiAliasParameter = SdlGfx.SMOOTHING_OFF;
			if (antiAlias == true)
			{
				antiAliasParameter = SdlGfx.SMOOTHING_ON;
			}
			this.SurfacePointer = 
				SdlGfx.zoomSurface(this.SurfacePointer, zoomX, zoomY, antiAliasParameter);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="zoom"></param>
		/// <returns></returns>
		public void ZoomSurface(double zoom)
		{
			this.SurfacePointer = 
				SdlGfx.zoomSurface(this.SurfacePointer, zoom, zoom, SdlGfx.SMOOTHING_OFF);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="zoom"></param>
		/// <param name="antiAlias"></param>
		/// <returns></returns>
		public void ZoomSurface(double zoom, bool antiAlias)
		{
			int antiAliasParameter = SdlGfx.SMOOTHING_OFF;
			if (antiAlias == true)
			{
				antiAliasParameter = SdlGfx.SMOOTHING_ON;
			}
			this.SurfacePointer = 
				SdlGfx.zoomSurface(this.SurfacePointer, zoom, zoom, antiAliasParameter);
		}
	}
}
