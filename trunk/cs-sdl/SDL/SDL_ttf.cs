/*****************************************************************************
 * (c) The CsGL Project, 2003
 *     http://www.sourceforge.net/projects/csgl
 * 
 *     Author(s) : Jakub Florczyk <kuba@osw.pl>
 ****************************************************************************/

/*	Read me: _TTF_Font not developed
 * 
 */

using System;
using System.Runtime.InteropServices;

namespace CsGL.SDL
{
	public unsafe class SDL_ttf : SDL
	{	
		/* The internal structure containing font information */
		//typedef struct _TTF_Font TTF_Font;

		/* Initialize the TTF engine - returns 0 if successful, -1 on error */
		[DllImport("SDL_ttf.dll")]
		public static extern int TTF_Init();
		
		/* Open a font file and create a font of the specified point size */
		[DllImport("SDL_ttf.dll")]
		public static extern TTF_Font * TTF_OpenFont(char *file, int ptsize);

		[DllImport("SDL_ttf.dll")]
		public static extern TTF_Font * TTF_OpenFontIndex(char *file, int ptsize, long index);

		/* Set and retrieve the font style
				This font style is implemented by modifying the font glyphs, and
				doesn't reflect any inherent properties of the truetype font file.
							*/
		public const uint TTF_STYLE_NORMAL = 0x00;
		public const uint TTF_STYLE_BOLD = 0x01;
		public const uint TTF_STYLE_ITALIC = 0x02;
		public const uint TTF_STYLE_UNDERLINE = 0x04;

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
		public static extern int TTF_GlyphMetrics(TTF_Font *font, ushort ch, int *minx, int *maxx, int *miny, int *maxy, int *advance);

		/* Get the dimensions of a rendered string of text */
		[DllImport("SDL_ttf.dll")]
		public static extern int TTF_SizeText(TTF_Font *font,  char *text, int *w, int *h);

		[DllImport("SDL_ttf.dll")]
		public static extern int TTF_SizeUTF8(TTF_Font *font, char *text, int *w, int *h);

		[DllImport("SDL_ttf.dll")]
		public static extern int TTF_SizeUNICODE(TTF_Font *font, ushort *text, int *w, int *h);

		/* Create an 8-bit palettized surface and render the given text at
				fast quality with the given font and color.  The 0 pixel is the
				colorkey, giving a transparent background, and the 1 pixel is set
				to the text color.
				This function returns the new surface, or NULL if there was an error.
		*/
		[DllImport("SDL_ttf.dll")]
		public static extern SDL_Surface * TTF_RenderText_Solid(TTF_Font *font, char *text, SDL_Color fg);

		[DllImport("SDL_ttf.dll")]
		public static extern SDL_Surface * TTF_RenderUTF8_Solid(TTF_Font *font, char *text, SDL_Color fg);

		[DllImport("SDL_ttf.dll")]
		public static extern SDL_Surface * TTF_RenderUNICODE_Solid(TTF_Font *font, ushort *text, SDL_Color fg);

		/* Create an 8-bit palettized surface and render the given glyph at
				fast quality with the given font and color.  The 0 pixel is the
				colorkey, giving a transparent background, and the 1 pixel is set
				to the text color.  The glyph is rendered without any padding or
				centering in the X direction, and aligned normally in the Y direction.
				This function returns the new surface, or NULL if there was an error.
		*/
		[DllImport("SDL_ttf.dll")]
		public static extern SDL_Surface * TTF_RenderGlyph_Solid(TTF_Font *font, ushort ch, SDL_Color fg);

		/* Create an 8-bit palettized surface and render the given text at
				high quality with the given font and colors.  The 0 pixel is background,
				while other pixels have varying degrees of the foreground color.
				This function returns the new surface, or NULL if there was an error.
		*/
		[DllImport("SDL_ttf.dll")]
		public static extern SDL_Surface * TTF_RenderText_Shaded(TTF_Font *font, char *text, SDL_Color fg, SDL_Color bg);

		[DllImport("SDL_ttf.dll")]
		public static extern SDL_Surface * TTF_RenderUTF8_Shaded(TTF_Font *font, char *text, SDL_Color fg, SDL_Color bg);

		[DllImport("SDL_ttf.dll")]
		public static extern SDL_Surface * TTF_RenderUNICODE_Shaded(TTF_Font *font, ushort *text, SDL_Color fg, SDL_Color bg);

		/* Create an 8-bit palettized surface and render the given glyph at
				high quality with the given font and colors.  The 0 pixel is background,
				while other pixels have varying degrees of the foreground color.
				The glyph is rendered without any padding or centering in the X
				direction, and aligned normally in the Y direction.
				This function returns the new surface, or NULL if there was an error.
		*/
		[DllImport("SDL_ttf.dll")]
		public static extern SDL_Surface * TTF_RenderGlyph_Shaded(TTF_Font *font, ushort ch, SDL_Color fg, SDL_Color bg);

		/* Create a 32-bit ARGB surface and render the given text at high quality,
				using alpha blending to dither the font with the given color.
				This function returns the new surface, or NULL if there was an error.
		*/

		[DllImport("SDL_ttf.dll")]
		public static extern SDL_Surface * TTF_RenderText_Blended(TTF_Font *font, char *text, SDL_Color fg);

		[DllImport("SDL_ttf.dll")]
		public static extern SDL_Surface * TTF_RenderUTF8_Blended(TTF_Font *font, char *text, SDL_Color fg);

		[DllImport("SDL_ttf.dll")]
		public static extern SDL_Surface * TTF_RenderUNICODE_Blended(TTF_Font *font, ushort *text, SDL_Color fg);

		/* Create a 32-bit ARGB surface and render the given glyph at high quality,
				using alpha blending to dither the font with the given color.
				The glyph is rendered without any padding or centering in the X
				direction, and aligned normally in the Y direction.
				This function returns the new surface, or NULL if there was an error.
		*/
		[DllImport("SDL_ttf.dll")]
		public static extern SDL_Surface * TTF_RenderGlyph_Blended(TTF_Font *font, ushort ch, SDL_Color fg);

		/* For compatibility with previous versions, here are the old functions */
		public static SDL_Surface * TTF_RenderText(TTF_Font *font, char *text, SDL_Color fg, SDL_Color bg)
		{
			return TTF_RenderText_Shaded(font, text, fg, bg);
		}

		public static SDL_Surface * TTF_RenderUTF8(TTF_Font *font, char *text, SDL_Color fg, SDL_Color bg)
		{
			return TTF_RenderUTF8_Shaded(font, text, fg, bg);
		}

		public static SDL_Surface * TTF_RenderUNICODE(TTF_Font *font, ushort *text, SDL_Color fg, SDL_Color bg)
		{
			return TTF_RenderUNICODE_Shaded(font, text, fg, bg);
		}

		/* Close an opened font file */
		[DllImport("SDL_ttf.dll")]
		public static extern void TTF_CloseFont(TTF_Font *font);

		/* De-initialize the TTF engine */
		[DllImport("SDL_ttf.dll")]
		public static extern void TTF_Quit();		
	}
}
