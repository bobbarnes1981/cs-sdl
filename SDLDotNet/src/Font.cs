/*
 * $RCSfile$
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
 *	the Text (Latin1) and UTF8 functions convert the string to Unicode then use the unicode version.
 *	DotNet seems to do this conversion when marshalling so I have decided to only have methods for the unicode versions.
 *
 *	In the future, I might merge Solid/Shaded/Blended into 1 function with the type as a parameter.  
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
	public class Font
	{
		private IntPtr fontPtr; // Pointer to Ttf_Font struct
		private Ttf ttf;
		private Video video;

/* TODO: This one will be tricky.  
		Possibly split into 3 properties - GlyphMinSize, GlyphMaxSize and GlyphAdvance.
		Or make a struct of these 5 values: GlyphMetrics

		[DllImport(Ttf_DLL)]
		private static extern int Ttf_GlyphMetrics(IntPtr font, UInt16 ch,
				     out int minx, out int maxx, out int miny, out int maxy, out int advance);
*/


/*
		// Going by the Sdl_ttf source, all the Size functions always return 0.
		[DllImport(Ttf_DLL)]
		private static extern int Ttf_SizeText(IntPtr font, string text, out int w, out int h);

		[DllImport(Ttf_DLL)]
		private static extern int Ttf_SizeUTF8(IntPtr font, string text, out int w, out int h);
*/
		
		// Rendering functions
/*
		[DllImport(Ttf_DLL)]
		private static extern IntPtr Ttf_RenderText_Solid(IntPtr font, string text, Color fg);

		[DllImport(Ttf_DLL)]
		private static extern IntPtr Ttf_RenderUTF8_Solid(IntPtr font, string text, Color fg);
*/
	

		/// <summary>
		/// Font Constructor
		/// </summary>
		/// <param name="Filename"></param>
		/// <param name="PointSize"></param>
		public Font(string Filename, int PointSize) 
		{
			ttf = Ttf.Instance;
			video = Video.Instance;
			fontPtr = SdlTtf.TTF_OpenFont(Filename, PointSize);
			if (fontPtr == IntPtr.Zero) 
			{
				throw TtfException.Generate();
			}
		}

		internal Font(IntPtr pFont) 
		{
			fontPtr = pFont;
		}

		// Possibly add Bold/Italic/Underline properties

		/// <summary>
		/// Style Property
		/// </summary>
		public Style Style 
		{
			set 
			{ 
				SdlTtf.TTF_SetFontStyle(fontPtr, (int) value); 
			}
			get 
			{ 
				return (Style) SdlTtf.TTF_GetFontStyle(fontPtr); 
			}
		}

		/// <summary>
		/// Height Property
		/// </summary>
		public int Height 
		{
			get 
			{ 
				return SdlTtf.TTF_FontHeight(fontPtr); 
			}
		}

		/// <summary>
		/// Ascent Property
		/// </summary>
		public int Ascent 
		{
			get 
			{ 
				return SdlTtf.TTF_FontAscent(fontPtr); 
			}
		}

		/// <summary>
		/// Line Skip property
		/// </summary>
		public int LineSkip 
		{
			get 
			{ 
				return SdlTtf.TTF_FontLineSkip(fontPtr); 
			}
		}

		/// <summary>
		/// Size
		/// </summary>
		/// <param name="Text"></param>
		/// <returns></returns>
		public Size SizeText(string Text) 
		{
			int Width, Height;

			SdlTtf.TTF_SizeUNICODE(fontPtr, Text, out Width, out Height);
			return new Size(Width, Height);
		}

		/// <summary>
		/// Render Text to Solid
		/// </summary>
		/// <param name="Text"></param>
		/// <param name="Color"></param>
		/// <returns></returns>
		public Surface RenderTextSolid(string Text, Sdl.SDL_Color color) 
		{
			IntPtr pSurface;
			pSurface = SdlTtf.TTF_RenderUNICODE_Solid(fontPtr, Text, color);
			if (pSurface == IntPtr.Zero) 
			{
				throw TtfException.Generate();
			}
			return video.GenerateSurfaceFromPointer(pSurface);
		}

		/// <summary>
		/// This is a utility function for rendering and blitting text
		/// It's only really useful for one-off text
		/// </summary>
		/// <param name="Text"></param>
		/// <param name="Color"></param>
		/// <param name="DestSurface"></param>
		/// <param name="X"></param>
		/// <param name="Y"></param>
		public void RenderTextSolid(string Text, Sdl.SDL_Color color, Surface DestSurface, int X, int Y) 
		{
			Surface FontSurface;
			System.Drawing.Rectangle DestRect;

			FontSurface = RenderTextSolid(Text, color);
			DestRect = new System.Drawing.Rectangle(new System.Drawing.Point(X, Y), FontSurface.Size);
			FontSurface.Blit(DestSurface, DestRect);
		}

		/// <summary>
		/// Shade text
		/// </summary>
		/// <param name="Text"></param>
		/// <param name="FG"></param>
		/// <param name="BG"></param>
		/// <returns></returns>
		public Surface RenderTextShaded(string Text, Sdl.SDL_Color FG, Sdl.SDL_Color BG) 
		{
			IntPtr pSurface;

			pSurface = SdlTtf.TTF_RenderUNICODE_Shaded(fontPtr, Text, FG, BG);
			if (pSurface == IntPtr.Zero) 
			{
				throw TtfException.Generate();
			}
			return video.GenerateSurfaceFromPointer(pSurface);
		}

		/// <summary>
		/// Blended Text
		/// </summary>
		/// <param name="Text"></param>
		/// <param name="FG"></param>
		/// <returns></returns>
		public Surface RenderTextBlended(string Text, Sdl.SDL_Color FG) 
		{
			IntPtr pSurface;

			pSurface = SdlTtf.TTF_RenderUNICODE_Blended(fontPtr, Text, FG);
			if (pSurface == IntPtr.Zero) 
			{
				throw TtfException.Generate();
			}
			return video.GenerateSurfaceFromPointer(pSurface);
		}

		/// <summary>
		/// Render Glyphs as Solid
		/// </summary>
		/// <param name="Character"></param>
		/// <param name="FG"></param>
		/// <returns></returns>
		public Surface RenderGlyphSolid(short Character, Sdl.SDL_Color FG) 
		{
			IntPtr pSurface;

			pSurface = SdlTtf.TTF_RenderGlyph_Solid(fontPtr, Character, FG);
			if (pSurface == IntPtr.Zero) 
			{
				throw TtfException.Generate();
			}
			return video.GenerateSurfaceFromPointer(pSurface);
		}

		/// <summary>
		/// Shade Glyphs
		/// </summary>
		/// <param name="Character"></param>
		/// <param name="FG"></param>
		/// <param name="BG"></param>
		/// <returns></returns>
		public Surface RenderGlyphShaded(short Character, Sdl.SDL_Color FG, Sdl.SDL_Color BG) 
		{
			IntPtr pSurface;

			pSurface = SdlTtf.TTF_RenderGlyph_Shaded(fontPtr, Character, FG, BG);
			if (pSurface == IntPtr.Zero) 
			{
				throw TtfException.Generate();
			}
			return video.GenerateSurfaceFromPointer(pSurface);
		}

		/// <summary>
		/// Blend glyphs
		/// </summary>
		/// <param name="Character"></param>
		/// <param name="FG"></param>
		/// <returns></returns>
		public Surface RenderGlyphBlended(short Character, Sdl.SDL_Color FG) 
		{
			IntPtr pSurface;

			pSurface = SdlTtf.TTF_RenderGlyph_Blended(fontPtr, Character, FG);
			if (pSurface == IntPtr.Zero) 
			{
				throw TtfException.Generate();
			}
			return video.GenerateSurfaceFromPointer(pSurface);
		}

		/// <summary>
		/// Destructor
		/// </summary>
		~Font() {
			if (fontPtr != IntPtr.Zero) 
			{
				SdlTtf.TTF_CloseFont(fontPtr);
				fontPtr = IntPtr.Zero;
			}
		}
	}
}
