/*****************************************************************************
 * (c) The CsGL Project, 2003
 *     http://sourceforge.net/projects/cs-sdl
 * 
 *     Author(s) : Jakub Florczyk <kuba@osw.pl>
 ****************************************************************************/

using System;
using System.Runtime.InteropServices;

namespace CsGL.SDL
{
	public unsafe class SDL_ttf : SDL
	{	
		/*
		SDL_ttf:  A companion library to SDL for working with TrueType (tm) fonts
		Copyright (C) 1997, 1998, 1999, 2000, 2001  Sam Lantinga

		This library is free software; you can redistribute it and/or
		modify it under the terms of the GNU Library General Public
		License as published by the Free Software Foundation; either
		version 2 of the License, or (at your option) any later version.

		This library is distributed in the hope that it will be useful,
		but WITHOUT ANY WARRANTY; without even the implied warranty of
		MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
		Library General Public License for more details.

		You should have received a copy of the GNU Library General Public
		License along with this library; if not, write to the Free
		Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA

		Sam Lantinga
		slouken@libsdl.org
		*/

		/* $Id$ */

		/* This library is a wrapper around the excellent FreeType 2.0 library,
			 available at:
			http://www.freetype.org/
		*/

		//#ifndef _SDLttf_h
		//#define _SDLttf_h

		//#include "SDL.h"
		//#include "begin_code.h"

		/* Set up for C function definitions, even when using C++ */
		//#ifdef __cplusplus
		//		extern "C" 
		//	{
		//#endif

		/* Printable format: "%d.%d.%d", MAJOR, MINOR, PATCHLEVEL
		*/
		//#define TTF_MAJOR_VERSION	2
		//#define TTF_MINOR_VERSION	0
		//#define TTF_PATCHLEVEL		6

		/* This macro can be used to fill a version structure with the compile-time
		 * version of the SDL_ttf library.
		 */
		//#define TTF_VERSION(X)							\
		//{
		//		\
		//	(X)->major = TTF_MAJOR_VERSION;					\
		//	(X)->minor = TTF_MINOR_VERSION;					\
		//	(X)->patch = TTF_PATCHLEVEL;					\
		//}

		/* This function gets the version of the dynamically linked SDL_ttf library.
			it should NOT be used to fill a version structure, instead you should
			use the TTF_VERSION() macro.
		*/
		[DllImport("SDL_ttf.dll")]
		public static extern SDL_version * TTF_Linked_Version();

		/* ZERO WIDTH NO-BREAKSPACE (Unicode byte order mark) */
		//#define UNICODE_BOM_NATIVE	0xFEFF
		//#define UNICODE_BOM_SWAPPED	0xFFFE

		/* This function tells the library whether UNICODE text is generally
			byteswapped.  A UNICODE BOM character in a string will override
			this setting for the remainder of that string.
		*/
		[DllImport("SDL_ttf.dll")]
		public static extern void TTF_ByteSwappedUNICODE(int swapped);

		/* The internal structure containing font information */
		//typedef struct _TTF_Font TTF_Font;

		/* Initialize the TTF engine - returns 0 if successful, -1 on error */
		[DllImport("SDL_ttf.dll")]
		public static extern int TTF_Init();

		/* Open a font file and create a font of the specified point size.
			* Some .fon fonts will have several sizes embedded in the file, so the
			* point size becomes the index of choosing which size.  If the value
			* is too high, the last indexed size will be the default. */
		[DllImport("SDL_ttf.dll")]
		public static extern TTF_Font * TTF_OpenFont(char *file, int ptsize);

		[DllImport("SDL_ttf.dll")]
		public static extern TTF_Font * TTF_OpenFontIndex(char *file, int ptsize, long index);

		[DllImport("SDL_ttf.dll")]
		public static extern TTF_Font * TTF_OpenFontRW(SDL_RWops *src, int freesrc, int ptsize);

		[DllImport("SDL_ttf.dll")]
		public static extern TTF_Font * TTF_OpenFontIndexRW(SDL_RWops *src, int freesrc, int ptsize, long index);

		/* Set and retrieve the font style
				This font style is implemented by modifying the font glyphs, and
				doesn't reflect any inherent properties of the truetype font file.
		*/
		//#define TTF_STYLE_NORMAL	0x00
		//#define TTF_STYLE_BOLD		0x01
		//#define TTF_STYLE_ITALIC	0x02
		//#define TTF_STYLE_UNDERLINE	0x04
		[DllImport("SDL_ttf.dll")]
		public static extern int TTF_GetFontStyle(TTF_Font *font);

		[DllImport("SDL_ttf.dll")]
		public static extern void TTF_SetFontStyle(TTF_Font *font, int style);

		/* Get the total height of the font - usually equal to point size */
		[DllImport("SDL_ttf.dll")]
		public static extern int TTF_FontHeight(TTF_Font *font);

		/* Get the offset from the baseline to the top of the font
				This is a positive value, relative to the baseline.
			*/
		[DllImport("SDL_ttf.dll")]
		public static extern int TTF_FontAscent(TTF_Font *font);

		/* Get the offset from the baseline to the bottom of the font
				This is a negative value, relative to the baseline.
			*/
		[DllImport("SDL_ttf.dll")]
		public static extern int TTF_FontDescent(TTF_Font *font);

		/* Get the recommended spacing between lines of text for this font */
		[DllImport("SDL_ttf.dll")]
		public static extern int TTF_FontLineSkip(TTF_Font *font);

		/* Get the number of faces of the font */
		[DllImport("SDL_ttf.dll")]
		public static extern long TTF_FontFaces(TTF_Font *font);

		/* Get the font face attributes, if any */
		[DllImport("SDL_ttf.dll")]
		public static extern int TTF_FontFaceIsFixedWidth(TTF_Font *font);

		[DllImport("SDL_ttf.dll")]
		public static extern char * TTF_FontFaceFamilyName(TTF_Font *font);

		[DllImport("SDL_ttf.dll")]
		public static extern char * TTF_FontFaceStyleName(TTF_Font *font);

		/* Get the metrics (dimensions) of a glyph */
		[DllImport("SDL_ttf.dll")]
		public static extern int TTF_GlyphMetrics(TTF_Font *font, Uint16 ch,
																	int *minx, int *maxx,
																	int *miny, int *maxy, int *advance);

		/* Get the dimensions of a rendered string of text */
		[DllImport("SDL_ttf.dll")]
		public static extern int TTF_SizeText(TTF_Font *font, char *text, int *w, int *h);

		[DllImport("SDL_ttf.dll")]
		public static extern int TTF_SizeUTF8(TTF_Font *font, char *text, int *w, int *h);

		[DllImport("SDL_ttf.dll")]
		public static extern int TTF_SizeUNICODE(TTF_Font *font, Uint16 *text, int *w, int *h);

		/* Create an 8-bit palettized surface and render the given text at
				fast quality with the given font and color.  The 0 pixel is the
				colorkey, giving a transparent background, and the 1 pixel is set
				to the text color.
				This function returns the new surface, or NULL if there was an error.
		*/
		[DllImport("SDL_ttf.dll")]
		public static extern SDL_Surface * TTF_RenderText_Solid(TTF_Font *font,
																		char *text, SDL_Color fg);

		[DllImport("SDL_ttf.dll")]
		public static extern SDL_Surface * TTF_RenderUTF8_Solid(TTF_Font *font,
																		char *text, SDL_Color fg);

		[DllImport("SDL_ttf.dll")]
		public static extern SDL_Surface * TTF_RenderUNICODE_Solid(TTF_Font *font,
																		Uint16 *text, SDL_Color fg);

		/* Create an 8-bit palettized surface and render the given glyph at
				fast quality with the given font and color.  The 0 pixel is the
				colorkey, giving a transparent background, and the 1 pixel is set
				to the text color.  The glyph is rendered without any padding or
				centering in the X direction, and aligned normally in the Y direction.
				This function returns the new surface, or NULL if there was an error.
		*/
		[DllImport("SDL_ttf.dll")]
		public static extern SDL_Surface * TTF_RenderGlyph_Solid(TTF_Font *font,
																		Uint16 ch, SDL_Color fg);

		/* Create an 8-bit palettized surface and render the given text at
				high quality with the given font and colors.  The 0 pixel is background,
				while other pixels have varying degrees of the foreground color.
				This function returns the new surface, or NULL if there was an error.
		*/
		[DllImport("SDL_ttf.dll")]
		public static extern SDL_Surface * TTF_RenderText_Shaded(TTF_Font *font,
																		char *text, SDL_Color fg, SDL_Color bg);

		[DllImport("SDL_ttf.dll")]
		public static extern SDL_Surface * TTF_RenderUTF8_Shaded(TTF_Font *font,
																		char *text, SDL_Color fg, SDL_Color bg);

		[DllImport("SDL_ttf.dll")]
		public static extern SDL_Surface * TTF_RenderUNICODE_Shaded(TTF_Font *font,
																		Uint16 *text, SDL_Color fg, SDL_Color bg);

		/* Create an 8-bit palettized surface and render the given glyph at
				high quality with the given font and colors.  The 0 pixel is background,
				while other pixels have varying degrees of the foreground color.
				The glyph is rendered without any padding or centering in the X
				direction, and aligned normally in the Y direction.
				This function returns the new surface, or NULL if there was an error.
		*/
		[DllImport("SDL_ttf.dll")]
		public static extern SDL_Surface * TTF_RenderGlyph_Shaded(TTF_Font *font,
																		Uint16 ch, SDL_Color fg, SDL_Color bg);

		/* Create a 32-bit ARGB surface and render the given text at high quality,
				using alpha blending to dither the font with the given color.
				This function returns the new surface, or NULL if there was an error.
		*/
		[DllImport("SDL_ttf.dll")]
		public static extern SDL_Surface * TTF_RenderText_Blended(TTF_Font *font,
																		char *text, SDL_Color fg);

		[DllImport("SDL_ttf.dll")]
		public static extern SDL_Surface * TTF_RenderUTF8_Blended(TTF_Font *font,
																		char *text, SDL_Color fg);

		[DllImport("SDL_ttf.dll")]
		public static extern SDL_Surface * TTF_RenderUNICODE_Blended(TTF_Font *font,
																		Uint16 *text, SDL_Color fg);

		/* Create a 32-bit ARGB surface and render the given glyph at high quality,
				using alpha blending to dither the font with the given color.
				The glyph is rendered without any padding or centering in the X
				direction, and aligned normally in the Y direction.
				This function returns the new surface, or NULL if there was an error.
		*/
		[DllImport("SDL_ttf.dll")]
		public static extern SDL_Surface * TTF_RenderGlyph_Blended(TTF_Font *font,
																		Uint16 ch, SDL_Color fg);

		/* For compatibility with previous versions, here are the old functions */
		public static extern SDL_Surface * TTF_RenderText(TTF_Font *font, char *text, SDL_Color fg, SDL_Color bg)
		{
			return TTF_RenderText_Shaded(font, text, fg, bg);
		}

		public static extern SDL_Surface * TTF_RenderUTF8(TTF_Font *font, char *text, SDL_Color fg, SDL_Color bg)
		{
			return TTF_RenderUTF8_Shaded(font, text, fg, bg);
		}

		public static extern SDL_Surface * TTF_RenderUNICODE(TTF_Font *font, Uint16 *text, SDL_Color fg, SDL_Color bg)
		{
			return TTF_RenderUNICODE_Shaded(font, text, fg, bg);
		}

		/* Close an opened font file */
		[DllImport("SDL_ttf.dll")]
		public static extern void TTF_CloseFont(TTF_Font *font);

		/* De-initialize the TTF engine */
		[DllImport("SDL_ttf.dll")]
		public static extern void TTF_Quit();

		/* Check if the TTF engine is initialized */
		[DllImport("SDL_ttf.dll")]
		public static extern int TTF_WasInit();

		/* We'll use SDL for reporting errors */
		//#define TTF_SetError	SDL_SetError
		//#define TTF_GetError	SDL_GetError

		/* Ends C function definitions when using C++ */
		//#ifdef __cplusplus
		//					}
		//#endif
		//#include "close_code.h"
		//
		//#endif /* _SDLttf_h */

	}
}
