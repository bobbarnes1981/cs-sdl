/*
 * $RCSfile$
 * Copyright (C) 2004, 2005 David Hudson (jendave@yahoo.com)
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
using SdlDotNet.Sprites;

namespace SdlDotNet 
{
	/// <summary>
	/// Represents an Sdl drawing surface.
	/// You can create instances of this class with the methods in the Video 
	/// object.
	/// </summary>
	public class Surface : BaseSdlResource, ICloneable
	{
		/// <summary>
		/// Private field. Used by the Transparent property 
		/// </summary>
		private bool transparent;

		/// <summary>
		/// Private field. Used by the TransparentColor property 
		/// </summary>
		private Color transparentcolor;

		/// <summary>
		/// Private field. Used by the AlphaFlags property 
		/// int SDL_RLEACCEL = 0X00004000
		/// int SDL_SRCALPHA = 0x00010000
		/// </summary>
		private Alphas alphaFlags;

		/// <summary>
		/// Private field. Used by the AlphaValue property 
		/// </summary>
		private byte alphaValue;
		private int colorKey;
		private bool disposed;

		#region Constructors and Destructors
		/// <summary>
		/// 
		/// </summary>
		/// <param name="handle"></param>
		internal Surface(IntPtr handle) 
		{
			this.Handle = handle;
		}

		/// <summary>
		/// <para>Represents a Image that can be drawn on a Sdl.Surface.</para>
		/// <para>The image can be loaded from from a file, 
		/// a System.Drawing.Bitmap, or a byte array.</para>
		/// <para>Supported image formats follows the development 
		/// cycle of SDL_Image. Currently, supported and planned 
		/// supported image formats are:</para>
		/// 
		/// <para>
		/// <list type="bullet">
		///		<item><term>.BMP</term>
		///		<description>Windows Bitmap</description>
		///		</item>
		///		<item><term>.PNM</term>
		///		<description>Portable Anymap File Format</description>
		///		</item>
		///		<item><term>.XPM</term>
		///		<description>X PixMap</description></item>
		///		<item><term>.LBM</term>
		///		<description>Tagged Image File Format</description>
		///		</item>
		///		<item><term>.PCX</term>
		///		<description>Z-Soft’s PC Paintbrush file format</description>
		///		</item>
		///		<item><term>.GIF</term>
		///		<description>Graphics Interchange Format</description>
		///		</item>
		///		<item><term>.JPG</term>
		///		<description>Joint Photographic Experts Group (JPEG)
		///		</description>
		///		</item>
		///		<item><term>.TIF</term>
		///		<description>Tagged Image File Format</description>
		///		</item>
		///		<item><term>.PNG</term>
		///		<description>Portable Network Graphics</description>
		///		</item>
		///		<item><term>.TGA</term>
		///		<description>Truevision (Targa) File Format</description>
		///		</item>
		///	</list>
		///	</para>
		/// 
		/// <para>Usage:</para>
		/// <code>
		/// SdlImage image = SdlImage("mybitmap.jpg")
		/// image.Draw(screen, new Rectangle(new Point(0,0),image.Size))
		/// </code>
		/// </summary> 
		public Surface(string file)
		{
			this.Handle = SdlImage.IMG_Load(file);
			if (this.Handle == IntPtr.Zero)
			{
				throw SdlException.Generate();
			}
		}

		/// <summary>
		/// Create surface of a given width and height
		/// </summary>
		/// <param name="width"></param>
		/// <param name="height"></param>
		public Surface(int width, int height)
		{
			this.Handle = 
				Sdl.SDL_CreateRGBSurface((int)VideoModes.None, width, height, VideoInfo.BitsPerPixel,VideoInfo.RedMask, VideoInfo.GreenMask, VideoInfo.BlueMask, VideoInfo.AlphaMask);
			if (this.Handle == IntPtr.Zero)
			{
				throw SdlException.Generate();
			}
		}

		/// <summary>
		/// Create surface that has the same size as the screen
		/// </summary>
		public Surface() : this(Video.Screen.Width, Video.Screen.Height)
		{
		}

		/// <summary>
		/// Create a Surface from a byte array in memory.
		/// </summary>
		/// <param name="array">
		/// A array of byte that should the image data
		/// </param>
		public Surface(byte[] array)
		{
			this.Handle = 
				SdlImage.IMG_Load_RW(Sdl.SDL_RWFromMem(array, array.Length), 1);
			if (this.Handle == IntPtr.Zero) 
			{
				throw SdlException.Generate();
			}
		}

		/// <summary>
		/// Create a SdlImage instance from a System.Drawing.Bitmap object. 
		/// Loads a bitmap from a System.Drawing.Bitmap object, 
		/// usually obtained from a resource.
		/// </summary>
		/// <param name="bitmap">A System.Drawing.Bitmap object</param>
		public Surface(System.Drawing.Bitmap bitmap)
		{
			MemoryStream stream = new MemoryStream();
			bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
			byte[] arr = stream.ToArray();
			this.Handle = 
				SdlImage.IMG_Load_RW(Sdl.SDL_RWFromMem(arr, arr.Length), 1);
			if (this.Handle == IntPtr.Zero) 
			{
				throw SdlException.Generate();
			}
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
					CloseHandle();
					GC.SuppressFinalize(this);
					this.disposed = true;
				}
				finally
				{
					base.Dispose(disposing);
					this.disposed = true;
				}
			}
		}

		/// <summary>
		/// Closes Surface handle
		/// </summary>
		protected override void CloseHandle() 
		{
			try
			{
				if (this.Handle != IntPtr.Zero)
				{
					Sdl.SDL_FreeSurface(this.Handle);
					GC.KeepAlive(this);
					this.Handle = IntPtr.Zero;
				}
			}
			catch (NullReferenceException)
			{
				this.Handle = IntPtr.Zero;
			}
		}

		internal static Surface FromScreenPtr(IntPtr surfacePtr) 
		{
			return new Surface(surfacePtr);
		}

		internal Sdl.SDL_Surface SurfaceStruct
		{
			get
			{
				if (this.disposed)
				{
					throw (new ObjectDisposedException(this.ToString(), "Object has been disposed"));
				}
				GC.KeepAlive(this);
				return (Sdl.SDL_Surface)Marshal.PtrToStructure(this.Handle, 
					typeof(Sdl.SDL_Surface));
			}
		}

		internal Sdl.SDL_PixelFormat PixelFormat
		{
			get
			{
				if (this.disposed)
				{
					throw (new ObjectDisposedException(this.ToString(), "Object has been disposed"));
		}
				GC.KeepAlive(this);
				return (Sdl.SDL_PixelFormat)Marshal.PtrToStructure(this.SurfaceStruct.format, 
					typeof(Sdl.SDL_PixelFormat));
			}
		}

		/// <summary>
		/// If the surface is double-buffered, 
		/// this method will flip the back buffer onto the screen
		/// </summary>
		public void Flip() 
		{
			if (this.disposed)
			{
				throw (new ObjectDisposedException(this.ToString(), "Object has been disposed"));
			}
			int result = Sdl.SDL_Flip(this.Handle);
			GC.KeepAlive(this);
			if (result != 0)
			{
				throw SdlException.Generate();
			}
		}

		private static Sdl.SDL_Rect ConvertRecttoSDLRect(
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
		/// <param name="rectangle">The rectangle coordinates</param>
		/// <param name="color">The color to draw</param>
		public Rectangle Fill(System.Drawing.Rectangle rectangle,
			System.Drawing.Color color) 
		{
			if (this.disposed)
			{
				throw (new ObjectDisposedException(this.ToString(), "Object has been disposed"));
			}

			Sdl.SDL_Rect sdlrect = ConvertRecttoSDLRect(rectangle);

			int result = Sdl.SDL_FillRect(this.Handle, ref sdlrect, GetColorValue(color));
			GC.KeepAlive(this);
			if (result != (int) SdlFlag.Success)
			{
				throw SdlException.Generate();
			}
			return new Rectangle(sdlrect.x, sdlrect.y, sdlrect.w, sdlrect.h);
		}

		/// <summary>
		/// Fill entire surface with color
		/// </summary>
		/// <param name="color">Color to fill surface</param>
		public Rectangle Fill(System.Drawing.Color color) 
		{
			return this.Fill(new System.Drawing.Rectangle(0, 0, this.SurfaceStruct.w, this.SurfaceStruct.h), color);
		}

		/// <summary>
		/// Draw filled circle onto surface
		/// </summary>
		/// <param name="circle">Circle</param>
		/// <param name="color">Color to fill circle</param>
		public void DrawFilledCircle(Circle circle, System.Drawing.Color color)
		{
			if (this.disposed)
			{
				throw (new ObjectDisposedException(this.ToString(), "Object has been disposed"));
			}
			int result = SdlGfx.filledCircleRGBA(this.Handle, circle.XPosition, circle.YPosition, circle.Radius, color.R, color.G, color.B,
				color.A);
			GC.KeepAlive(this);
			if (result != (int) SdlFlag.Success)
			{
				throw SdlException.Generate();
			}
		}

		/// <summary>
		/// Draw circle onto surface
		/// </summary>
		/// <param name="circle"></param>
		/// <param name="color"></param>
		/// <param name="antiAlias"></param>
		public void DrawCircle(Circle circle, System.Drawing.Color color, bool antiAlias)
		{
			if (this.disposed)
			{
				throw (new ObjectDisposedException(this.ToString(), "Object has been disposed"));
			}
			int result = 0;
			if (antiAlias)
			{
				result = SdlGfx.aacircleRGBA(this.Handle, circle.XPosition, circle.YPosition, circle.Radius, color.R, color.G, color.B,
					color.A);
				GC.KeepAlive(this);
			}
			else
			{
				result = SdlGfx.circleRGBA(this.Handle, circle.XPosition, circle.YPosition, circle.Radius, color.R, color.G, color.B,
					color.A);
				GC.KeepAlive(this);
			}
			if (result != (int) SdlFlag.Success)
			{
				throw SdlException.Generate();
			}
		}

		/// <summary>
		/// Drawe circle onto surface
		/// </summary>
		/// <param name="circle"></param>
		/// <param name="color"></param>
		public void DrawCircle(Circle circle, System.Drawing.Color color)
		{
			DrawCircle(circle, color, true);
		}

		/// <summary>
		/// Draw ellipse onto surface
		/// </summary>
		/// <param name="ellipse"></param>
		/// <param name="color"></param>
		/// <param name="antiAlias"></param>
		public void DrawEllipse(Ellipse ellipse, System.Drawing.Color color, bool antiAlias)
		{
			if (this.disposed)
			{
				throw (new ObjectDisposedException(this.ToString(), "Object has been disposed"));
			}
			int result = 0;

			if (antiAlias)
			{
				result = SdlGfx.aaellipseRGBA(
					this.Handle, ellipse.XPosition, ellipse.YPosition, 
					ellipse.RadiusX, ellipse.RadiusY, 
					color.R, color.G, color.B,
					color.A);
				GC.KeepAlive(this);
			}
			else
			{
				result = SdlGfx.ellipseRGBA(
					this.Handle, ellipse.XPosition, ellipse.YPosition, 
					ellipse.RadiusX, ellipse.RadiusY, 
					color.R, color.G, color.B,
					color.A);
				GC.KeepAlive(this);
			}
			if (result != (int) SdlFlag.Success)
			{
				throw SdlException.Generate();
			}
		}

		/// <summary>
		/// Draw ellipse onto surface
		/// </summary>
		/// <param name="ellipse"></param>
		/// <param name="color"></param>
		public void DrawEllipse(Ellipse ellipse, System.Drawing.Color color)
		{
			DrawEllipse(ellipse, color, true);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ellipse"></param>
		/// <param name="color"></param>
		public void DrawFilledEllipse(Ellipse ellipse, System.Drawing.Color color)
		{
			if (this.disposed)
			{
				throw (new ObjectDisposedException(this.ToString(), "Object has been disposed"));
			}
			int result = SdlGfx.filledEllipseRGBA(this.Handle, ellipse.XPosition, ellipse.YPosition, ellipse.RadiusX, ellipse.RadiusY,color.R, color.G, color.B,
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
		public void DrawLine(Line line, System.Drawing.Color color, bool antiAlias)
		{
			if (this.disposed)
			{
				throw (new ObjectDisposedException(this.ToString(), "Object has been disposed"));
			}
			int result = 0;

			if (antiAlias)
			{
				result = SdlGfx.aalineRGBA(
					this.Handle, line.XPosition1, line.YPosition1, 
					line.XPosition2, line.YPosition2, 
					color.R, color.G, color.B,
					color.A);
				GC.KeepAlive(this);
			}
			else
			{
				result = SdlGfx.lineRGBA(
					this.Handle, line.XPosition1, line.YPosition1, 
					line.XPosition2, line.YPosition2, 
					color.R, color.G, color.B,
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
		public void DrawLine(Line line, System.Drawing.Color color)
		{
			DrawLine(line, color, true);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="triangle"></param>
		/// <param name="color"></param>
		/// <param name="antiAlias"></param>
		public void DrawTriangle(Triangle triangle, System.Drawing.Color color, bool antiAlias)
		{
			if (this.disposed)
			{
				throw (new ObjectDisposedException(this.ToString(), "Object has been disposed"));
			}
			int result = 0;

			if (antiAlias)
			{
				result = SdlGfx.aatrigonRGBA(
					this.Handle, triangle.XPosition1, triangle.YPosition1, 
					triangle.XPosition2, triangle.YPosition2, 
					triangle.XPosition3, triangle.YPosition3, 
					color.R, color.G, color.B,
					color.A);
				GC.KeepAlive(this);
			}
			else
			{
				result = SdlGfx.trigonRGBA(
					this.Handle, triangle.XPosition1, triangle.YPosition1, 
					triangle.XPosition2, triangle.YPosition2, 
					triangle.XPosition3, triangle.YPosition3, 
					color.R, color.G, color.B,
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
		public void DrawTriangle(Triangle triangle, System.Drawing.Color color)
		{
			DrawTriangle(triangle, color, true);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="triangle"></param>
		/// <param name="color"></param>
		public void DrawFilledTriangle(Triangle triangle, System.Drawing.Color color)
		{
			if (this.disposed)
			{
				throw (new ObjectDisposedException(this.ToString(), "Object has been disposed"));
			}
			int result = 0;
			result = SdlGfx.filledTrigonRGBA(
				this.Handle, triangle.XPosition1, triangle.YPosition1, 
				triangle.XPosition2, triangle.YPosition2, 
				triangle.XPosition3, triangle.YPosition3, 
				color.R, color.G, color.B,
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
		public void DrawFilledPolygon(Polygon polygon, System.Drawing.Color color)
		{
			if (this.disposed)
			{
				throw (new ObjectDisposedException(this.ToString(), "Object has been disposed"));
			}
			int result = SdlGfx.filledPolygonRGBA(this.Handle, polygon.XPositions(), polygon.YPositions(), polygon.NumberOfSides, color.R, color.G, color.G,
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
		public void DrawPolygon(Polygon polygon, System.Drawing.Color color, bool antiAlias)
		{
			if (this.disposed)
			{
				throw (new ObjectDisposedException(this.ToString(), "Object has been disposed"));
			}
			int result = 0;
			if (antiAlias)
			{
				result = SdlGfx.aapolygonRGBA(this.Handle, polygon.XPositions(), polygon.YPositions(), polygon.NumberOfSides, color.R, color.G, color.B,
					color.A);
				GC.KeepAlive(this);
			}
			else
			{
				result = SdlGfx.polygonRGBA(this.Handle, polygon.XPositions(), polygon.YPositions(), polygon.NumberOfSides, color.R, color.G, color.B,
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
		public void DrawPolygon(Polygon polygon, System.Drawing.Color color)
		{
			DrawPolygon(polygon, color, true);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="pie"></param>
		/// <param name="color"></param>
		public void DrawPie(Pie pie, System.Drawing.Color color)
		{
			if (this.disposed)
			{
				throw (new ObjectDisposedException(this.ToString(), "Object has been disposed"));
			}
			int result = 0;

			result = SdlGfx.pieRGBA(
				this.Handle, pie.XPosition, pie.YPosition, 
				pie.Radius,
				pie.StartingPoint, pie.EndingPoint, 
				color.R, color.G, color.B,
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
		public void DrawFilledPie(Pie pie, System.Drawing.Color color)
		{
			if (this.disposed)
			{
				throw (new ObjectDisposedException(this.ToString(), "Object has been disposed"));
			}
			int result = SdlGfx.filledPieRGBA(this.Handle, pie.XPosition, pie.YPosition, pie.Radius, pie.StartingPoint, pie.EndingPoint,color.R, color.G, color.B,
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
		public void DrawBezier(Bezier bezier, System.Drawing.Color color)
		{
			if (this.disposed)
			{
				throw (new ObjectDisposedException(this.ToString(), "Object has been disposed"));
			}
			int result = 0;
			result = SdlGfx.bezierRGBA(
				this.Handle, bezier.XPositions(), bezier.YPositions(), 
				bezier.NumberOfPoints, bezier.Steps, 
				color.R, color.G, color.B,
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
		/// <param name="rectangle"></param>
		/// <param name="color"></param>
		public void DrawBox(Rectangle rectangle, System.Drawing.Color color)
		{
			this.DrawBox(new Box((short)rectangle.X, (short)rectangle.Y, (short)rectangle.Right, (short)rectangle.Bottom), color);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="box"></param>
		/// <param name="color"></param>
		public void DrawBox(Box box, System.Drawing.Color color)
		{
			if (this.disposed)
			{
				throw (new ObjectDisposedException(this.ToString(), "Object has been disposed"));
			}
			int result = 0;

			result = SdlGfx.rectangleRGBA(
				this.Handle, box.XPosition1, box.YPosition1, 
				box.XPosition2, box.YPosition2, 
				color.R, color.G, color.B,
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
		/// <param name="rectangle"></param>
		/// <param name="color"></param>
		public void DrawFilledBox(Rectangle rectangle, System.Drawing.Color color)
		{
			this.DrawFilledBox(new Box((short)rectangle.X, (short)rectangle.Y, (short)rectangle.Right, (short)rectangle.Bottom), color);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="box"></param>
		/// <param name="color"></param>
		public void DrawFilledBox(Box box, System.Drawing.Color color)
		{
			if (this.disposed)
			{
				throw (new ObjectDisposedException(this.ToString(), "Object has been disposed"));
			}
			int result = 0;

			result = SdlGfx.boxRGBA(
				this.Handle, box.XPosition1, box.YPosition1, 
				box.XPosition2, box.YPosition2, 
				color.R, color.G, color.B,
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
		public int GetColorValue(System.Drawing.Color color) 
		{
			if (this.disposed)
			{
				throw (new ObjectDisposedException(this.ToString(), "Object has been disposed"));
			}
			if (this.SurfaceStruct.format != IntPtr.Zero)
			{
				GC.KeepAlive(this);
				return Sdl.SDL_MapRGBA(
					this.SurfaceStruct.format, 
					color.R, 
					color.G, 
					color.B,
					color.A);
			}
			else
			{
				throw (new ObjectDisposedException(this.ToString(), "Object has been disposed"));
			}
		}
		
		/// <summary>
		/// Maps an Sdl 32-bit pixel value to a color using the surface's 
		/// pixel format
		/// </summary>
		/// <param name="colorValue">The pixel value to map</param>
		/// <returns>
		/// A Color value for a pixel value in the surface's format
		/// </returns>
		public System.Drawing.Color GetColor(int colorValue) 
		{
			if (this.disposed)
			{
				throw (new ObjectDisposedException(this.ToString(), "Object has been disposed"));
			}

			byte r, g, b, a;
			Sdl.SDL_GetRGBA(colorValue, this.SurfaceStruct.format, out r, out g, out b, out a);
			GC.KeepAlive(this);
			return System.Drawing.Color.FromArgb(a, r, g, b);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public Surface CreateCompatibleSurface() 
		{
			return this.CreateCompatibleSurface(this.Size.Width, this.Size.Height, true);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="size"></param>
		/// <returns></returns>
		public Surface CreateCompatibleSurface(Size size) 
		{
			return this.CreateCompatibleSurface(size.Width, size.Height, true);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="hardware"></param>
		/// <param name="size"></param>
		/// <returns></returns>
		public Surface CreateCompatibleSurface(Size size, bool hardware) 
		{
			return this.CreateCompatibleSurface(size.Width, size.Height, hardware);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <returns></returns>
		public Surface CreateCompatibleSurface(
			int width, int height) 
		{
			return this.CreateCompatibleSurface(width, height, true);
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
			if (this.disposed)
			{
				throw (new ObjectDisposedException(this.ToString(), "Object has been disposed"));
			}

			int flag = 0;
			if (hardware)
			{
				flag = (int)VideoModes.HardwareSurface;
			}

			return new Surface(Sdl.SDL_CreateRGBSurface(
				flag,
				width, 
				height, 
				this.PixelFormat.BitsPerPixel,
				this.PixelFormat.Rmask, 
				this.PixelFormat.Gmask, 
				this.PixelFormat.Bmask, 
				this.PixelFormat.Amask));
		}

		/// <summary>
		/// Converts the surface to the same pixel format as the source
		/// </summary>
		/// <param name="source">The source surface</param>
		/// <param name="hardware">
		/// A flag indicating whether or not to 
		/// attempt to place the new surface in video memory
		/// </param>
		/// <param name="alpha"></param>
		/// <returns>The new surface</returns>
		public Surface Convert(Surface source, bool hardware, bool alpha) 
		{
			if (this.disposed)
			{
				throw (new ObjectDisposedException(this.ToString(), "Object has been disposed"));
			}
			int flags = 0;

			if (hardware)
			{
				flags = (int)VideoModes.HardwareSurface;
			}
			if (alpha)
			{
				flags |= (int)Alphas.RleEncoded;

			}
			
			return new Surface(Sdl.SDL_ConvertSurface(this.Handle, source.SurfaceStruct.format, flags));
		}

		/// <summary>
		/// Converts the surface to the same pixel format as the source
		/// </summary>
		/// <param name="source">The source surface</param>
		/// <returns>The new surface</returns>
		public Surface Convert(Surface source) 
		{
			return this.Convert(source, true, false);
		}

		/// <summary>
		/// Converts this surface to the format of 
		/// the display window
		/// </summary>
		/// <returns>The new surface</returns>
		public Surface Convert() 
		{
			if (this.disposed)
			{
				throw (new ObjectDisposedException(this.ToString(), "Object has been disposed"));
			}
			return new Surface(Sdl.SDL_DisplayFormat(this.Handle));
		}

		/// <summary>
		/// Gets the size of the surface
		/// </summary>
		public System.Drawing.Size Size 
		{
			get 
			{ 
				return new System.Drawing.Size(this.SurfaceStruct.w, this.SurfaceStruct.h); 
			}
		}

		/// <summary>
		/// Gets a rectangle that covers the entire of the surface
		/// </summary>
		public System.Drawing.Rectangle Rectangle
		{
			get 
			{ 
				return new System.Drawing.Rectangle(0, 0, this.SurfaceStruct.w, this.SurfaceStruct.h); 
			}
		}

		/// <summary>
		/// Gets the width of the surface
		/// </summary>
		public int Width 
		{ 
			get
			{ 
				return (int)this.SurfaceStruct.w; 
			} 
		}

		/// <summary>
		/// Gets the height of the surface
		/// </summary>
		public int Height 
		{ 
			get
			{ 
				return (int)this.SurfaceStruct.h; 
			} 
		}

		/// <summary>
		/// Copies another surface to this surface
		/// </summary>
		/// <param name="sourceSurface">
		/// The surface to copy from
		/// </param>
		/// <param name="destinationPosition">
		/// The rectangle coordinates on this surface to copy to
		/// </param>
		public Rectangle Blit(Surface sourceSurface, System.Drawing.Point destinationPosition) 
		{
			return this.Blit(
				sourceSurface, 
				destinationPosition,
				new Rectangle(new System.Drawing.Point(0, 0), sourceSurface.Size));
		}

		/// <summary>
		/// Copies another surface to this surface
		/// </summary>
		/// <param name="sourceSurface">
		/// The surface to copy from
		/// </param>
		/// <param name="destinationRectangle">
		/// The rectangle coordinates on this surface to copy to
		/// </param>
		public Rectangle Blit(Surface sourceSurface, System.Drawing.Rectangle destinationRectangle) 
		{
			return this.Blit(
				sourceSurface, 
				destinationRectangle.Location,
				new Rectangle(new System.Drawing.Point(0, 0), sourceSurface.Size));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sourceSurface"></param>
		/// <returns></returns>
		public Rectangle Blit(Surface sourceSurface) 
		{
			return this.Blit(
				sourceSurface, 
				new Point(0, 0));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sprite"></param>
		public void Blit(Sprite sprite)
		{
			if (sprite.Visible)
			{
				this.Blit(sprite.Render());
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="spriteCollection"></param>
		public void Blit(SpriteCollection spriteCollection)
		{
			spriteCollection.Draw(this);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sourceSurface"></param>
		/// <param name="destinationRectangle"></param>
		/// <param name="sourceRectangle"></param>
		/// <returns></returns>
		public Rectangle Blit( 
			Surface sourceSurface, 
			System.Drawing.Rectangle destinationRectangle,
			System.Drawing.Rectangle sourceRectangle) 
		{
			if (this.disposed)
			{
				throw (new ObjectDisposedException(this.ToString(), "Object has been disposed"));
			}
			Sdl.SDL_Rect s = Surface.ConvertRecttoSDLRect(sourceRectangle); 
			Sdl.SDL_Rect d = Surface.ConvertRecttoSDLRect(destinationRectangle);
			int result = Sdl.SDL_BlitSurface(sourceSurface.Handle, ref s, this.Handle, ref d);
			GC.KeepAlive(this);
			if (result!= (int) SdlFlag.Success)
			{
				throw SdlException.Generate();
			}
			return new Rectangle(d.x, d.y, d.w, d.h);
		}

		/// <summary>
		/// Copies a portion of a source surface to this surface
		/// </summary>
		/// <param name="sourceRectangle">
		/// The rectangle coordinates of the source surface to copy from
		/// </param>
		/// <param name="sourceSurface">The surface to copy from</param>
		/// <param name="destinationPosition">
		/// The coordinates on this surface to copy to
		/// </param>
		/// <returns>Destination Rectangle after any necessary clipping</returns>
		public Rectangle Blit( 
			Surface sourceSurface, 
			System.Drawing.Point destinationPosition,
			System.Drawing.Rectangle sourceRectangle) 
		{
			return this.Blit(sourceSurface, 
				new Rectangle(
				destinationPosition.X, 
				destinationPosition.Y, 0, 0), 
				sourceRectangle);
		}

		/// <summary>
		/// Locks a surface to allow direct pixel manipulation
		/// </summary>
		public void Lock() 
		{
			if (this.disposed)
			{
				throw (new ObjectDisposedException(this.ToString(), "Object has been disposed"));
			}
			if (MustLock) 
			{
				int result = Sdl.SDL_LockSurface(this.Handle);
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
				if (this.disposed)
				{
					throw (new ObjectDisposedException(this.ToString(), "Object has been disposed"));
				}
				GC.KeepAlive(this);
				return this.SurfaceStruct.pixels; 
			}
		}

		/// <summary>
		/// Unlocks a surface which has been locked.
		/// </summary>
		public void Unlock() 
		{
			if (MustLock) 
			{
				if (this.disposed)
				{
					throw (new ObjectDisposedException(this.ToString(), "Object has been disposed"));
				}
				int result = Sdl.SDL_UnlockSurface(this.Handle);
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
				if (this.disposed)
				{
					throw (new ObjectDisposedException(this.ToString(), "Object has been disposed"));
				}
				
				int result = Sdl.SDL_MUSTLOCK(this.Handle);
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
			if (this.disposed)
			{
				throw (new ObjectDisposedException(this.ToString(), "Object has been disposed"));
			}
			int result = Sdl.SDL_SaveBMP(this.Handle, file);
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
			SetColorKey(this.GetColorValue(transparent), accelerationRle);
		}

		/// <summary>
		/// Sets a colorkey for the surface
		/// </summary>
		/// <param name="key">The transparent pixel value</param>
		/// <param name="accelerationRle">
		/// A flag indicating whether or not to use hardware acceleration for RLE
		/// </param>
		public void SetColorKey(int key, bool accelerationRle) 
		{
			if (this.disposed)
			{
				throw (new ObjectDisposedException(this.ToString(), "Object has been disposed"));
			}
			int flag = Sdl.SDL_SRCCOLORKEY;
			if (accelerationRle)
			{
				flag |= Sdl.SDL_RLEACCELOK;
			}
			int result = Sdl.SDL_SetColorKey(this.Handle, (int)flag, key);
			GC.KeepAlive(this);
			if (result != (int) SdlFlag.Success)
			{
				throw SdlException.Generate();
			}
			this.colorKey = key;
		}

		/// <summary>
		/// Clears the colorkey for the surface
		/// </summary>
		public void ClearColorKey() 
		{
			if (this.disposed)
			{
				throw (new ObjectDisposedException(this.ToString(), "Object has been disposed"));
			}
			int result = Sdl.SDL_SetColorKey(this.Handle, 0, 0);
			GC.KeepAlive(this);
			if (result != (int) SdlFlag.Success)
			{
				throw SdlException.Generate();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public Color ColorKey
		{
			get
			{
				return this.GetColor(this.colorKey);
			}
		}

		/// <summary>
		/// Sets/Gets the clipping rectangle for the surface
		/// </summary>
		public System.Drawing.Rectangle ClipRectangle
		{
			get
			{
				if (this.disposed)
				{
					throw (new ObjectDisposedException(this.ToString(), "Object has been disposed"));
				}
				Sdl.SDL_Rect sdlrect = 
					Surface.ConvertRecttoSDLRect(new System.Drawing.Rectangle());
				Sdl.SDL_GetClipRect(this.Handle, ref sdlrect);
				GC.KeepAlive(this);
				return new System.Drawing.Rectangle(sdlrect.x, sdlrect.y, sdlrect.w, sdlrect.h);
			}
			set
			{
				Sdl.SDL_Rect sdlrect = Surface.ConvertRecttoSDLRect(value);
				Sdl.SDL_SetClipRect(this.Handle, ref sdlrect);
				GC.KeepAlive(this);
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
			if (this.disposed)
			{
				throw (new ObjectDisposedException(this.ToString(), "Object has been disposed"));
			}

			switch (this.PixelFormat.BytesPerPixel) 
			{
				case 1: // Assuming 8-bpp
				{
					byte pixelColorValue = (byte) this.GetColorValue(color);
					//IntPtr pixelColorValuePtr = 
					//	new IntPtr(surface.pixels.ToInt32() + y*surface.pitch + 2*x);
					Marshal.WriteByte(new IntPtr(this.SurfaceStruct.pixels.ToInt32() + y*this.SurfaceStruct.pitch + 2*x), pixelColorValue);
				}
					break;
				case 2: // Probably 15-bpp or 16-bpp
				{
					short pixelColorValue = (short) this.GetColorValue(color);
					//IntPtr pixelColorValuePtr = 
					//	new IntPtr(surface.pixels.ToInt32() + y*surface.pitch + 2*x);
					Marshal.WriteInt16(new IntPtr(this.SurfaceStruct.pixels.ToInt32() + y*this.SurfaceStruct.pitch + 2*x), pixelColorValue);
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
					int pixelColorValue = this.GetColorValue(color);
					//IntPtr pixelColorValuePtr = 
					//	new IntPtr(surface.pixels.ToInt32() + (y*surface.pitch + 4*x));
					Marshal.WriteInt32(new IntPtr(this.SurfaceStruct.pixels.ToInt32() + (y*this.SurfaceStruct.pitch + 4*x)), pixelColorValue);
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

			Lock();
			while (first < second) 
			{
				//Take first scanline
				//Copy pointer data from scanline to tempByte array
				Marshal.Copy(new IntPtr(this.SurfaceStruct.pixels.ToInt32() + first * pitch), tempByte, 0, pitch);
				//Take last scanline
				//Copy pointer data from scanline to firstByte array
				Marshal.Copy(new IntPtr(this.SurfaceStruct.pixels.ToInt32() + second * pitch), firstByte, 0, pitch);
				//Take tempByte array
				//Copy pointer data from tempByte to last scanline
				Marshal.Copy(tempByte, 0, new IntPtr(this.SurfaceStruct.pixels.ToInt32() + second * pitch), pitch);
				//Take firstByte array
				//Copy pointer data from firstByte array to first scanline
				Marshal.Copy(firstByte, 0, new IntPtr(this.SurfaceStruct.pixels.ToInt32() + first * pitch), pitch);
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
			this.Rotate(270);
			this.FlipVertical();
			this.Rotate(90);
		}

		/// <summary>
		/// Gets the pitch of the surface. Pitch is the number of bytes in a scanline of the surface.
		/// </summary>
		public short Pitch 
		{ 
			get 
			{ 
				return this.SurfaceStruct.pitch; 
			} 
		}

		/// <summary>
		/// Attempting to code GetPixel. The getter equivalent of DrawPixel.
		/// </summary>
		/// <param name="x">The x coordinate of the surface</param>
		/// <param name="y">The y coordinate of the surface</param>
		/// <returns>ColorValue of pixel</returns>
		public Color GetPixel(int x, int y) 
		{
			if (this.disposed)
			{
				throw (new ObjectDisposedException(this.ToString(), "Object has been disposed"));
			}

			int bytesPerPixel = this.PixelFormat.BytesPerPixel;

			switch (bytesPerPixel) 
			{
				case 1: //Assuming 8-bpp
				{
					return this.GetColor(Marshal.ReadInt32(new IntPtr(this.SurfaceStruct.pixels.ToInt32() + y*this.SurfaceStruct.pitch + 2*x)));
				}
				case 2:
				{
					return this.GetColor(Marshal.ReadInt32(new IntPtr(this.SurfaceStruct.pixels.ToInt32() + y*this.SurfaceStruct.pitch + 2*x)));
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
					return this.GetColor(0);
				}
				case 4:
				{
					return this.GetColor(Marshal.ReadInt32(new IntPtr(this.SurfaceStruct.pixels.ToInt32() + (y*this.SurfaceStruct.pitch + 4*x))));
				}
				default: //Should never come here
				{
					return this.GetColor(0);
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
			if (this.disposed)
			{
				throw (new ObjectDisposedException(this.ToString(), "Object has been disposed"));
			}
			int result = Sdl.SDL_SetAlpha(this.Handle, (int)flag, alpha);
			GC.KeepAlive(this);
			if (result != (int) SdlFlag.Success) 
			{
				throw SdlException.Generate();
			}
		}

		/// <summary>
		/// Rotate surface.
		/// </summary>
		/// <remarks>Smoothing is turned on.</remarks>
		/// <param name="degreesOfRotation">degrees of rotation</param>
		public void Rotate(int degreesOfRotation)
		{
			if (this.disposed)
			{
				throw (new ObjectDisposedException(this.ToString(), "Object has been disposed"));
			}
			this.Handle = 
				SdlGfx.rotozoomSurface(this.Handle, degreesOfRotation, 1, SdlGfx.SMOOTHING_ON);
		}

		/// <summary>
		/// Rotate surface
		/// </summary>
		/// <param name="degreesOfRotation">degrees of rotation</param>
		/// <param name="antiAlias">If true, smoothing will be turned on.</param>
		/// <returns></returns>
		public void Rotate(int degreesOfRotation, bool antiAlias)
		{
			if (this.disposed)
			{
				throw (new ObjectDisposedException(this.ToString(), "Object has been disposed"));
			}
			int antiAliasParameter = SdlGfx.SMOOTHING_OFF;
			if (antiAlias == true)
			{
				antiAliasParameter = SdlGfx.SMOOTHING_ON;
			}
			
			this.Handle = 
				SdlGfx.rotozoomSurface(
				this.Handle, 
				degreesOfRotation, 
				1, 
				antiAliasParameter);
		}

		/// <summary>
		/// Rotate and Zoom surface
		/// </summary>
		/// <param name="degreesOfRotation">Degrees of rotation</param>
		/// <param name="zoom">Scale of zoom</param>
		/// <remarks>Smoothing is turned on.</remarks>
		public void RotationZoom(int degreesOfRotation, double zoom)
		{
			if (this.disposed)
			{
				throw (new ObjectDisposedException(this.ToString(), "Object has been disposed"));
			}
			this.Handle = 
				SdlGfx.rotozoomSurface(this.Handle, degreesOfRotation, zoom, SdlGfx.SMOOTHING_ON);
		}

		/// <summary>
		/// Rotate and zoom surface
		/// </summary>
		/// <param name="degreesOfRotation">degrees of rotation</param>
		/// <param name="zoom">scale of zoom</param>
		/// <param name="antiAlias">If true, moothing is turned on.</param>
		/// <returns></returns>
		public void RotationZoom(int degreesOfRotation, 
			double zoom, bool antiAlias)
		{
			if (this.disposed)
			{
				throw (new ObjectDisposedException(this.ToString(), "Object has been disposed"));
			}
			int antiAliasParameter = SdlGfx.SMOOTHING_OFF;
			if (antiAlias == true)
			{
				antiAliasParameter = SdlGfx.SMOOTHING_ON;
			}
			
			this.Handle = 
				SdlGfx.rotozoomSurface(
				this.Handle, 
				degreesOfRotation, 
				zoom, 
				antiAliasParameter);
		}

		/// <summary>
		/// Rescale surface
		/// </summary>
		/// <param name="zoomX">Scale on X-axis</param>
		/// <param name="zoomY">Scale on Y-axis</param>
		/// <returns></returns>
		public void Scale(double zoomX, double zoomY)
		{
			if (this.disposed)
			{
				throw (new ObjectDisposedException(this.ToString(), "Object has been disposed"));
			}
			this.Handle = 
				SdlGfx.zoomSurface(this.Handle, zoomX, zoomY, SdlGfx.SMOOTHING_ON);
		}

		/// <summary>
		/// Rescale surface
		/// </summary>
		/// <param name="zoomX">Scale on X-axis</param>
		/// <param name="zoomY">Scale on Y-axis</param>
		/// <param name="antiAlias">If true, smoothing is turned on.</param>
		/// <returns></returns>
		public void Scale(double zoomX, double zoomY, bool antiAlias)
		{
			if (this.disposed)
			{
				throw (new ObjectDisposedException(this.ToString(), "Object has been disposed"));
			}
			int antiAliasParameter = SdlGfx.SMOOTHING_OFF;
			if (antiAlias == true)
			{
				antiAliasParameter = SdlGfx.SMOOTHING_ON;
			}
			this.Handle = 
				SdlGfx.zoomSurface(this.Handle, zoomX, zoomY, antiAliasParameter);
		}

		/// <summary>
		/// Scale surface on both axes
		/// </summary>
		/// <param name="zoom">Scale amount</param>
		public void Scale(double zoom)
		{
			if (this.disposed)
			{
				throw (new ObjectDisposedException(this.ToString(), "Object has been disposed"));
			}
			this.Handle = 
				SdlGfx.zoomSurface(this.Handle, zoom, zoom, SdlGfx.SMOOTHING_ON);
		}

		/// <summary>
		/// Scale surface on both axes
		/// </summary>
		/// <param name="zoom">Scale amount</param>
		/// <param name="antiAlias">If true, smoothing is turned on</param>
		/// <returns></returns>
		public void Scale(double zoom, bool antiAlias)
		{
			if (this.disposed)
			{
				throw (new ObjectDisposedException(this.ToString(), "Object has been disposed"));
			}
			int antiAliasParameter = SdlGfx.SMOOTHING_OFF;
			if (antiAlias == true)
			{
				antiAliasParameter = SdlGfx.SMOOTHING_ON;
			}
			this.Handle = 
				SdlGfx.zoomSurface(this.Handle, zoom, zoom, antiAliasParameter);
		}

		/// <summary>
		/// Doubles the size of the surface
		/// </summary>
		/// <returns></returns>
		public void ScaleDouble()
		{
			if (this.disposed)
			{
				throw (new ObjectDisposedException(this.ToString(), "Object has been disposed"));
			}
			this.Handle = 
				SdlGfx.zoomSurface(this.Handle, 2, 2, SdlGfx.SMOOTHING_ON);
		}

		/// <summary>
		/// Doubles the size of the surface
		/// </summary>
		/// <param name="antiAlias">If true</param>
		/// <returns></returns>
		public void ScaleDouble(bool antiAlias)
		{
			if (this.disposed)
			{
				throw (new ObjectDisposedException(this.ToString(), "Object has been disposed"));
			}
			int antiAliasParameter = SdlGfx.SMOOTHING_OFF;
			if (antiAlias == true)
			{
				antiAliasParameter = SdlGfx.SMOOTHING_ON;
			}
			this.Handle = 
				SdlGfx.zoomSurface(this.Handle, 2, 2, antiAliasParameter);
		}

		/// <summary>
		/// Uses a Transformation object to perform rotation, zooming and scaling
		/// </summary>
		/// <param name="transformation"></param>
		public void Transform(Transformation transformation)
		{
			if (Math.Round(transformation.Zoom, 1) != 1.0 && Math.Round(transformation.Zoom, 1) != 0.0)
			{
				this.RotationZoom(transformation.DegreesOfRotation, transformation.Zoom, transformation.AntiAlias);
			}
			if (Math.Round(transformation.ScaleX, 1) != 1.0 && 
				Math.Round(transformation.ScaleY, 1) != 1.0 && 
				Math.Round(transformation.ScaleX, 1) != 0.0 && 
				Math.Round(transformation.ScaleY, 1) != 0.0)
			{
				this.Scale(transformation.ScaleX, transformation.ScaleY, transformation.AntiAlias);
			}
		}

		/// <summary>
		/// Get/set the transparency of the image.  
		/// </summary>
		public bool Transparent
		{
			get 
			{
				return transparent;
			}
			set	
			{
				transparent = value;
				if (value)
				{
					this.SetColorKey(transparentcolor,true);
				}
				else 
				{
					this.ClearColorKey();
				}
			}
		}

		/// <summary>
		/// Get/set the transparent color of the image.
		/// </summary>
		public Color TransparentColor
		{
			get 
			{
				return transparentcolor;
			}
			set	
			{
				transparentcolor = value;
				if (Transparent) 
				{
					this.SetColorKey(transparentcolor,true);
				}
			}
		}

		/// <summary>
		/// Get/set the Alpha flags of the image.
		/// </summary>
		public Alphas AlphaFlags
		{
			get 
			{
				return alphaFlags;
			}
			set	
			{
				this.alphaFlags = value;
				this.SetAlpha(this.alphaFlags, this.alphaValue);
			}
		}

		/// <summary>
		/// Get/set the Alpha value of the image. 
		/// 0 indicates that the image fully transparent. 
		/// 255 indicates that the image is not tranparent.
		/// </summary>
		public byte AlphaValue
		{
			get 
			{
				return alphaValue;
			}
			set	
			{
				this.alphaValue = value;
				this.SetAlpha(this.alphaFlags, this.alphaValue);
			}
		}


		/// <summary>
		/// Updates rectangle
		/// </summary>
		/// <param name="rectangle"></param>
		public void Update(System.Drawing.Rectangle rectangle)
		{
			if (this.disposed)
			{
				throw (new ObjectDisposedException(this.ToString(), "Object has been disposed"));
			}
			Sdl.SDL_UpdateRect(
				this.Handle, 
				rectangle.X, 
				rectangle.Y, 
				rectangle.Width, 
				rectangle.Height);
				GC.KeepAlive(this);
		}

		/// <summary>
		/// Updates entire surface
		/// </summary>
		public void Update()
		{
			if (this.disposed)
			{
				throw (new ObjectDisposedException(this.ToString(), "Object has been disposed"));
			}
			Sdl.SDL_UpdateRect(
				this.Handle, 
				0, 
				0, 
				this.Size.Width, 
				this.Size.Height 
				);
			GC.KeepAlive(this);
		}

		/// <summary>
		/// Update an array of rectangles
		/// </summary>
		/// <param name="rectangles"></param>
		public void Update(System.Drawing.Rectangle[] rectangles)
		{
			if (this.disposed)
			{
				throw (new ObjectDisposedException(this.ToString(), "Object has been disposed"));
			}
			Sdl.SDL_Rect[] rects = new Sdl.SDL_Rect[rectangles.Length];
			for (int i=0;i < rectangles.Length;i++)
			{
				rects[i] = Surface.ConvertRecttoSDLRect(rectangles[i]);
			}
			Sdl.SDL_UpdateRects(this.Handle, rects.Length, rects);
			GC.KeepAlive(this);
		}

		/// <summary>
		/// Gets the number of Bits per pixel.
		/// </summary>
		/// <remarks>Typically 8, 16, 24 or 32</remarks>
		public byte BitsPerPixel
		{
			get
			{
				return this.PixelFormat.BitsPerPixel;
			}
		}

		/// <summary>
		/// Gets the number of bytes per pixel for this surface
		/// </summary>
		/// <remarks>Tyically 1, 2, 3 or 4</remarks>
		public byte BytesPerPixel
		{
			get
			{
				return this.PixelFormat.BytesPerPixel;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public int AlphaMask
		{
			get
			{
				return this.PixelFormat.Amask;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public int RedMask
		{
			get
			{
				return this.PixelFormat.Rmask;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public int GreenMask
		{
			get
			{
				return this.PixelFormat.Gmask;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public int BlueMask
		{
			get
			{
				return this.PixelFormat.Bmask;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public int BlueShift
		{
			get
			{
				return this.PixelFormat.Bshift;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public int RedShift
		{
			get
			{
				return this.PixelFormat.Rshift;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public int GreenShift
		{
			get
			{
				return this.PixelFormat.Gshift;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public int AlphaShift
		{
			get
			{
				return this.PixelFormat.Ashift;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public int AlphaLoss
		{
			get
			{
				return this.PixelFormat.Aloss;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public int RedLoss
		{
			get
			{
				return this.PixelFormat.Rloss;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public int GreenLoss
		{
			get
			{
				return this.PixelFormat.Gloss;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public int BlueLoss
		{
			get
			{
				return this.PixelFormat.Bloss;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public int Flags
		{
			get
			{
				return this.SurfaceStruct.flags;
			}
		}	
		#region ICloneable Members
		/// <summary>
		/// Creates a shallow copy of the Surface.
		/// </summary>
		/// <returns>
		/// Returns an object that needs to be cast to a Surface
		/// </returns>
		public object Clone()
		{
			return Clone(false);
		}

		/// <summary>
		/// Creates a copy of the Surface
		/// </summary>
		/// <param name="doDeepCopy">If true, a deep copy is created</param>
		/// <returns></returns>
		public object Clone(bool doDeepCopy)
		{
			if (doDeepCopy)
			{
				return (new Surface(SdlGfx.zoomSurface(this.Handle, 1, 1, SdlGfx.SMOOTHING_OFF)));
			}
			else
			{
				return (this.MemberwiseClone());
			}
		}
		#endregion
	}
}
