/*
 * $RCSfile$
 * Copyright (C) 2004 David Hudson (jendave@yahoo.com)
 * Copyright (C) 2003 Lucas Maloney
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

/*
 * $Revision$
 * $Date$
 *
 *	Notes
 *
 *	For all functions where there are 3 versions, 
 *	the Text (Latin1) and UTF8 functions convert the string to 
 *  Unicode then use the unicode version.
 *	DotNet seems to do this conversion when marshalling so 
 *  I have decided to only have methods for the unicode versions.
 *
 *	In the future, I might merge Solid/Shaded/Blended into 
 *  1 function with the type as a parameter.  
 *	At the moment I can't see any reason to do this.
 *
 *	REVISION HISTORY
 *
 *	Mon 31 Mar 2003 23:28:02 EST LM
 *	Changed namespace from SdlTtfDotNet
 *	Now using singleton architecture
 *
 *	Tue 25 Mar 2003 18:18:27 EST LM
 *	Changed all exception throws to use the Generate method.
 *
 *	Mon 24 Mar 2003 20:45:40 EST LM
 *	There is currently a bug in mono which meant this class did not need an instance of Sdl.
 *	I have fixed this so it does not depend on that bug.
 */

using System;
using System.Drawing;
using System.Runtime.InteropServices;
using SdlDotNet;
using Tao.Sdl;

namespace SdlDotNet
{
	/// <summary>
	/// Font Class
	/// </summary>
	public class Font : BaseSdlResource
	{
		private IntPtr handle; // Pointer to Ttf_Font struct
		private bool disposed = false;
	
		/// <summary>
		/// Font Constructor
		/// </summary>
		/// <param name="filename"></param>
		/// <param name="pointSize"></param>
		public Font(string filename, int pointSize) 
		{
			if (!FontSystem.IsInitialized)
			{
				FontSystem.Initialize();
			}

			handle = SdlTtf.TTF_OpenFont(filename, pointSize);
			if (handle == IntPtr.Zero) 
			{
				throw FontException.Generate();
			}
		}

		internal Font(IntPtr handle) 
		{
			this.handle = handle;
		}

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
						CloseHandle(handle);
						GC.KeepAlive(this);
					}
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
			if (handleToClose != IntPtr.Zero)
			{
				SdlTtf.TTF_CloseFont(handleToClose);
				GC.KeepAlive(this);
				handleToClose = IntPtr.Zero;
			}
		}

		// Possibly add Bold/Italic/Underline properties

		/// <summary>
		/// Style Property
		/// </summary>
		public Styles Style 
		{
			set 
			{ 
				SdlTtf.TTF_SetFontStyle(handle, (int) value); 
				GC.KeepAlive(this);
			}
			get 
			{ 
				Styles style = (Styles)SdlTtf.TTF_GetFontStyle(handle);
				GC.KeepAlive(this);
				return style; 
			}
		}

		/// <summary>
		/// Bold Property
		/// </summary>
		public bool Bold 
		{
			set 
			{ 
				if (value == true)
				{
					Styles style = (Styles)SdlTtf.TTF_GetFontStyle(handle);
					SdlTtf.TTF_SetFontStyle(handle, (int) style | (int) Styles.Bold); 
					GC.KeepAlive(this);
				}
				else
				{
					Styles style = (Styles)SdlTtf.TTF_GetFontStyle(handle);
					SdlTtf.TTF_SetFontStyle(handle, (int) style ^ (int) Styles.Bold); 
					GC.KeepAlive(this);
				}
			}
			get 
			{ 
				Styles style = (Styles)SdlTtf.TTF_GetFontStyle(handle);
				GC.KeepAlive(this);
				if ((int)(style & Styles.Bold) == (int) SdlFlag.FalseValue)
				{
					return false;
				}
				else
				{
					return true;
				}
			}
		}

		/// <summary>
		/// Italic Property
		/// </summary>
		public bool Italic
		{
			set 
			{ 
				if (value == true)
				{
					Styles style = (Styles)SdlTtf.TTF_GetFontStyle(handle);
					SdlTtf.TTF_SetFontStyle(handle, (int) style | (int) Styles.Italic); 
					GC.KeepAlive(this);
				}
				else
				{
					Styles style = (Styles)SdlTtf.TTF_GetFontStyle(handle);
					SdlTtf.TTF_SetFontStyle(handle, (int) style ^ (int) Styles.Italic); 
					GC.KeepAlive(this);
				}
			}
			get 
			{ 
				Styles style = (Styles)SdlTtf.TTF_GetFontStyle(handle);
				GC.KeepAlive(this);
				if ((int)(style & Styles.Italic) == (int) SdlFlag.FalseValue)
				{
					return false;
				}
				else
				{
					return true;
				}
			}
		}

		/// <summary>
		/// Underline Property
		/// </summary>
		public bool Underline
		{
			set 
			{ 
				if (value == true)
				{
					Styles style = (Styles)SdlTtf.TTF_GetFontStyle(handle);
					SdlTtf.TTF_SetFontStyle(handle, (int) style | (int) Styles.Underline); 
					GC.KeepAlive(this);
				}
				else
				{
					Styles style = (Styles)SdlTtf.TTF_GetFontStyle(handle);
					SdlTtf.TTF_SetFontStyle(handle, (int) style ^ (int) Styles.Underline); 
					GC.KeepAlive(this);
				}
			}
			get 
			{ 
				Styles style = (Styles)SdlTtf.TTF_GetFontStyle(handle);
				GC.KeepAlive(this);
				if ((int)(style & Styles.Underline) == (int) SdlFlag.FalseValue)
				{
					return false;
				}
				else
				{
					return true;
				}
			}
		}

		/// <summary>
		/// Normal Property
		/// </summary>
		public bool Normal
		{
			set 
			{ 
				if (value == true)
				{
					SdlTtf.TTF_SetFontStyle(handle, (int) Styles.Normal); 
					GC.KeepAlive(this);
				}
			}
			get 
			{ 
				Styles style = (Styles)SdlTtf.TTF_GetFontStyle(handle);
				GC.KeepAlive(this);
				if ((int)(style | Styles.Underline | Styles.Bold | Styles.Italic) == (int) SdlFlag.TrueValue)
				{
					return false;
				}
				else
				{
					return true;
				}
			}
		}

		/// <summary>
		/// Height Property
		/// </summary>
		public int Height 
		{
			get 
			{ 
				int result = SdlTtf.TTF_FontHeight(handle);
				GC.KeepAlive(this);
				return result; 
			}
		}

		/// <summary>
		/// Ascent Property
		/// </summary>
		public int Ascent 
		{
			get 
			{ 
				int result = SdlTtf.TTF_FontAscent(handle);
				GC.KeepAlive(this);
				return result;
			}
		}

		/// <summary>
		/// Descent Property
		/// </summary>
		public int Descent
		{
			get 
			{ 
				int result = SdlTtf.TTF_FontDescent(handle);
				GC.KeepAlive(this);
				return result;
			}
		}

		/// <summary>
		/// Line Size property
		/// </summary>
		public int LineSize 
		{
			get 
			{ 
				int result = SdlTtf.TTF_FontLineSkip(handle); 
				GC.KeepAlive(this);
				return result;
			}
		}

		/// <summary>
		/// Size
		/// </summary>
		/// <param name="textItem"></param>
		/// <returns></returns>
		public Size SizeText(string textItem) 
		{
			int width;
			int height;

			SdlTtf.TTF_SizeUNICODE(handle, textItem, out width, out height);
			GC.KeepAlive(this);
			return new Size(width, height);
		}

		/// <summary>
		/// Render Text to Solid
		/// </summary>
		/// <param name="textItem"></param>
		/// <param name="color"></param>
		/// <returns></returns>
		private Surface RenderTextSolid(string textItem, Color color) 
		{
			IntPtr pSurface;
			Sdl.SDL_Color colorSdl = SdlColor.ConvertColor(color);
			pSurface = SdlTtf.TTF_RenderUNICODE_Solid(handle, textItem, colorSdl);
			GC.KeepAlive(this);
			if (pSurface == IntPtr.Zero) 
			{
				throw FontException.Generate();
			}
			return new Surface(pSurface);
		}

//		/// <summary>
//		/// This is a utility function for rendering and blitting text
//		/// It's only really useful for one-off text
//		/// </summary>
//		/// <param name="textItem"></param>
//		/// <param name="color"></param>
//		/// <param name="destinationSurface"></param>
//		/// <param name="x"></param>
//		/// <param name="y"></param>
//		public void Render(
//			string textItem, Color color, 
//			Surface destinationSurface, int x, int y) 
//		{
//			Surface fontSurface;
//			System.Drawing.Rectangle destinationRectangle;
//
//			fontSurface = RenderTextSolid(textItem, color);
//			destinationRectangle = 
//				new System.Drawing.Rectangle(new System.Drawing.Point(x, y), fontSurface.Size);
//			destinationSurface.Blit(fontSurface, destinationRectangle);
//		}

		/// <summary>
		/// Shade text
		/// </summary>
		/// <param name="textItem"></param>
		/// <param name="backgroundColor"></param>
		/// <param name="foregroundColor"></param>
		/// <returns></returns>
		private Surface RenderTextShaded(
			string textItem, Color foregroundColor, Color backgroundColor) 
		{
			IntPtr pSurface;

			Sdl.SDL_Color foregroundColorSdl = 
				SdlColor.ConvertColor(foregroundColor);
			Sdl.SDL_Color backgroundColorSdl = 
				SdlColor.ConvertColor(backgroundColor);
			pSurface = SdlTtf.TTF_RenderUNICODE_Shaded(
				handle, textItem, foregroundColorSdl, backgroundColorSdl);
			GC.KeepAlive(this);
			if (pSurface == IntPtr.Zero) 
			{
				throw FontException.Generate();
			}
			return new Surface(pSurface);
		}

		/// <summary>
		/// Blended Text
		/// </summary>
		/// <param name="color"></param>
		/// <param name="textItem"></param>
		/// <returns></returns>
		private Surface RenderTextBlended(
			string textItem, Color color) 
		{
			IntPtr pSurface;

			Sdl.SDL_Color colorSdl = SdlColor.ConvertColor(color);
			pSurface = SdlTtf.TTF_RenderUNICODE_Blended(
				handle, textItem, colorSdl);
			GC.KeepAlive(this);
			if (pSurface == IntPtr.Zero) 
			{
				throw FontException.Generate();
			}
			return new Surface(pSurface);
		}


		/// <summary>
		/// Render text to a surface.
		/// </summary>
		/// <param name="textItem"></param>
		/// <param name="antiAlias"></param>
		/// <param name="foregroundColor">Color of text</param>
		/// <returns></returns>
		public Surface Render(string textItem, bool antiAlias, Color foregroundColor)
		{
			if (antiAlias)
			{
				return Render(textItem, foregroundColor);
			}
			else
			{
				return RenderTextSolid(textItem, foregroundColor);
			}
		}

		/// <summary>
		/// Render text to a surface with a background color
		/// </summary>
		/// <param name="textItem"></param>
		/// <param name="foregroundColor"></param>
		/// <param name="backgroundColor"></param>
		/// <returns></returns>
		public Surface Render(string textItem, Color foregroundColor, Color backgroundColor)
		{
			return RenderTextShaded(textItem, foregroundColor, backgroundColor);
		}

		/// <summary>
		/// Render Text to a surface
		/// </summary>
		/// <param name="textItem">Text string</param>
		/// <param name="foregroundColor">Color of text</param>
		/// <returns></returns>
		public Surface Render(string textItem, Color foregroundColor)
		{
			return RenderTextBlended(textItem, foregroundColor);
		}


//		/// <summary>
//		/// Render Glyphs as Solid
//		/// </summary>
//		/// <param name="character"></param>
//		/// <param name="foregroundColor"></param>
//		/// <returns></returns>
//		public Surface RenderGlyphSolid(
//			short character, Sdl.SDL_Color foregroundColor) 
//		{
//			IntPtr pSurface;
//
//			pSurface = SdlTtf.TTF_RenderGlyph_Solid(
//				handle, character, foregroundColor);
//			GC.KeepAlive(this);
//			if (pSurface == IntPtr.Zero) 
//			{
//				throw FontException.Generate();
//			}
//			return Video.GenerateSurfaceFromPointer(pSurface);
//		}
//
//		/// <summary>
//		/// Shade Glyphs
//		/// </summary>
//		/// <param name="character"></param>
//		/// <param name="foregroundColor"></param>
//		/// <param name="backgroundColor"></param>
//		/// <returns></returns>
//		public Surface RenderGlyphShaded(short character, 
//			Sdl.SDL_Color foregroundColor, Sdl.SDL_Color backgroundColor) 
//		{
//			IntPtr pSurface;
//
//			pSurface = SdlTtf.TTF_RenderGlyph_Shaded(
//				handle, character, foregroundColor, backgroundColor);
//			GC.KeepAlive(this);
//			if (pSurface == IntPtr.Zero) 
//			{
//				throw FontException.Generate();
//			}
//			return Video.GenerateSurfaceFromPointer(pSurface);
//		}
//
//		/// <summary>
//		/// Blend glyphs
//		/// </summary>
//		/// <param name="character"></param>
//		/// <param name="foregroundColor"></param>
//		/// <returns></returns>
//		public Surface RenderGlyphBlended(
//			short character, Sdl.SDL_Color foregroundColor) 
//		{
//			IntPtr pSurface;
//
//			pSurface = SdlTtf.TTF_RenderGlyph_Blended(
//				handle, character, foregroundColor);
//			GC.KeepAlive(this);
//			if (pSurface == IntPtr.Zero) 
//			{
//				throw FontException.Generate();
//			}
//			return Video.GenerateSurfaceFromPointer(pSurface);
//		}
	}
}
