/*
 * $RCSfile$
 * Copyright (C) 2004 David Hudson (jendave@yahoo.com)
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
using System.IO;
using System.Drawing;
using SdlDotNet;
using Tao.Sdl;

namespace SdlDotNet
{

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
	///		<item><term>.BMP</term><description>Windows Bitmap</description></item>
	///		<item><term>.PNM</term><description>Portable Anymap File Format</description></item>
	///		<item><term>.XPM</term><description>X PixMap</description></item>
	///		<item><term>.LBM</term><description>Tagged Image File Format</description></item>
	///		<item><term>.PCX</term><description>Z-Soft’s PC Paintbrush file format</description></item>
	///		<item><term>.GIF</term><description>Graphics Interchange Format</description></item>
	///		<item><term>.JPG</term><description>Joint Photographic Experts Group (JPEG)</description></item>
	///		<item><term>.TIF</term><description>Tagged Image File Format</description></item>
	///		<item><term>.PNG</term><description>Portable Network Graphics</description></item>
	///		<item><term>.TGA</term><description>Truevision (Targa) File Format</description></item>
	///	</list>
	///	</para>
	/// 
	/// <para>Usage:</para>
	/// <code>
	/// SdlImage image = SdlImage("mybitmap.jpg")
	/// image.Draw(screen, new Rectangle(new Point(0,0),image.Size))
	/// </code>
	/// </summary> 
	public class Image
	{
		/// <summary>
		/// Private field that holds the surface of the image.
		/// </summary>
		private Surface surface;

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

		/// <summary>
		/// Create a SdlImage instance from a diskfile
		/// </summary>
		/// <param name="filename">The filename of the image to load</param>
		public Image(string filename)
		{
			IntPtr pSurface = SdlImage.IMG_Load(filename);
			if (pSurface == IntPtr.Zero) 
			{
				throw ImageException.Generate();
			}
			surface = Video.GenerateSurfaceFromPointer(pSurface);
		}

		/// <summary>
		/// Create a SdlImage instance from a byte array in memory.
		/// </summary>
		/// <param name="array">A array of byte that shold the image data</param>
		public Image(byte[] array)
		{
			IntPtr pSurface = 
				SdlImage.IMG_Load_RW(Sdl.SDL_RWFromMem(array, array.Length), 1);
			if (pSurface == IntPtr.Zero) 
			{
				throw ImageException.Generate();
			}
			surface = Video.GenerateSurfaceFromPointer(pSurface);
		}

		
		/// <summary>
		/// Create a SdlImage instance from a System.Drawing.Bitmap object. 
		/// Loads a bitmap from a System.Drawing.Bitmap object, 
		/// usually obtained from a resource.
		/// </summary>
		/// <param name="bitmap">A System.Drawing.Bitmap object</param>
		public Image(System.Drawing.Bitmap bitmap)
		{
			MemoryStream stream = new MemoryStream();
			bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
			byte[] arr = stream.ToArray();
			IntPtr pSurface = 
				SdlImage.IMG_Load_RW(Sdl.SDL_RWFromMem(arr, arr.Length), 1);
			if (pSurface == IntPtr.Zero) 
			{
				throw ImageException.Generate();
			}
			surface = Video.GenerateSurfaceFromPointer(pSurface);
		}
		
		/// <summary>
		/// Destructor
		/// </summary>
		~Image() 
		{
			surface.Dispose();
		}

		/// <summary>
		/// The Sdl.Surface that represents the Surface of the SdlImage. 
		/// </summary>
		public Surface Surface 
		{ 
			get 
			{ 
				return surface; 
			} 
		}

		/// <summary>
		/// The width of the image
		/// </summary>
		public int Width 
		{ 
			get 
			{ 
				return surface.Width; 
			} 
		}

		/// <summary>
		/// The height of the image
		/// </summary>
		public int Height
		{ 
			get 
			{ 
				return surface.Height;
			} 		
		}

		/// <summary>
		/// The size of the image
		/// </summary>
		public Size Size
		{ 
			get 
			{ 
				return surface.Size;
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
					surface.SetColorKey(transparentcolor,true);
				}
				else 
				{
					surface.ClearColorKey();
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
					surface.SetColorKey(transparentcolor,true);
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
				alphaFlags = value;
				surface.SetAlpha(alphaFlags,alphaValue);
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
				alphaValue = value;
				surface.SetAlpha(alphaFlags,alphaValue);
			}
		}

		/// <summary>
		/// Draws the image on a Sdl.Surface.
		/// </summary>
		/// <param name="destinationSurface">
		/// The Sdl.Surface to draw the image upon</param>
		/// <param name="destinationRectangle">
		/// The position of the image on the destination surface
		/// </param>
		public void Draw(
			Surface destinationSurface, Rectangle destinationRectangle) 
		{
			surface.Blit(destinationSurface,destinationRectangle);
		}

		/// <summary>
		/// Draws the image on a Sdl.Surface.
		/// </summary>
		/// <param name="sourceRectangle">
		/// The area of the image that is to be drawn on the destination surface
		/// </param>
		/// <param name="destinationSurface">
		/// The Sdl.Surface to draw the image upon</param>
		/// <param name="destinationRectangle">
		/// The position of the image on the destination surface
		/// </param>
		public void Draw(
			Rectangle sourceRectangle, 
			Surface destinationSurface, 
			Rectangle destinationRectangle) 
		{
			surface.Blit(sourceRectangle,destinationSurface,destinationRectangle);
		}
	}
}
