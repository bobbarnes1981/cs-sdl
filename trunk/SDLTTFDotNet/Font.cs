using System;
using System.Drawing;
using System.Runtime.InteropServices;
using SDLDotNet;

/*
	Notes

	For all functions where there are 3 versions, the Text (Latin1) and UTF8 functions convert the string to Unicode then use the unicode version.  DotNet seems to do this conversion when marshalling so I have decided to only have methods for the unicode versions.

	In the future, I might merge Solid/Shaded/Blended into 1 function with the type as a parameter.  At the moment I can't see any reason to do this.

	REVISION HISTORY

	Mon 31 Mar 2003 23:28:02 EST LM
	Changed namespace from SDLTTFDotNet
	Now using singleton architecture

	Tue 25 Mar 2003 18:18:27 EST LM
	Changed all exception throws to use the Generate method.

	Mon 24 Mar 2003 20:45:40 EST LM
	There is currently a bug in mono which meant this class did not need an instance of SDL.
	I have fixed this so it does not depend on that bug.
*/
namespace SDLDotNet.TTF
{
	/// <summary>
	/// Font Class
	/// </summary>
	public class Font
	{
		private IntPtr mFont; // Pointer to TTF_Font struct
		private SDL mSDL;
		private SDLTTF mSDLTTF;

		const string TTF_DLL = "SDL_ttf";

		[DllImport(TTF_DLL)]
		private static extern IntPtr TTF_OpenFont(string file, int ptsize);

		[DllImport(TTF_DLL)]
		private static extern void TTF_SetFontStyle(IntPtr font, int style);

		[DllImport(TTF_DLL)]
		private static extern int TTF_GetFontStyle(IntPtr font);

		[DllImport(TTF_DLL)]
		private static extern int TTF_FontHeight(IntPtr font);

		[DllImport(TTF_DLL)]
		private static extern int TTF_FontAscent(IntPtr font);

		[DllImport(TTF_DLL)]
		private static extern int TTF_FontDescent(IntPtr font);

		[DllImport(TTF_DLL)]
		private static extern int TTF_FontLineSkip(IntPtr font);

/* TODO: This one will be tricky.  
		Possibly split into 3 properties - GlyphMinSize, GlyphMaxSize and GlyphAdvance.
		Or make a struct of these 5 values: GlyphMetrics

		[DllImport(TTF_DLL)]
		private static extern int TTF_GlyphMetrics(IntPtr font, UInt16 ch,
				     out int minx, out int maxx, out int miny, out int maxy, out int advance);
*/


/*
		// Going by the SDL_ttf source, all the Size functions always return 0.
		[DllImport(TTF_DLL)]
		private static extern int TTF_SizeText(IntPtr font, string text, out int w, out int h);

		[DllImport(TTF_DLL)]
		private static extern int TTF_SizeUTF8(IntPtr font, string text, out int w, out int h);
*/
		[DllImport(TTF_DLL)]
		private static extern int TTF_SizeUNICODE(IntPtr font, [MarshalAs(UnmanagedType.LPWStr)] string text, out int w, out int h);

		// Rendering functions
/*
		[DllImport(TTF_DLL)]
		private static extern IntPtr TTF_RenderText_Solid(IntPtr font, string text, SDLColor fg);

		[DllImport(TTF_DLL)]
		private static extern IntPtr TTF_RenderUTF8_Solid(IntPtr font, string text, SDLColor fg);
*/
		[DllImport(TTF_DLL)]
		private static extern IntPtr TTF_RenderUNICODE_Solid(IntPtr font, [MarshalAs(UnmanagedType.LPWStr)] string text, SDLColor fg);

		[DllImport(TTF_DLL)]
		private static extern IntPtr TTF_RenderGlyph_Solid(IntPtr font, UInt16 ch, SDLColor fg);
/*
		[DllImport(TTF_DLL)]
		private static extern IntPtr TTF_RenderText_Shaded(IntPtr font, string text, SDLColor fg, SDLColor bg);

		[DllImport(TTF_DLL)]
		private static extern IntPtr TTF_RenderUTF8_Shaded(IntPtr font, string text, SDLColor fg, SDLColor bg);
*/
		[DllImport(TTF_DLL)]
		private static extern IntPtr TTF_RenderUNICODE_Shaded(IntPtr font, [MarshalAs(UnmanagedType.LPWStr)] string text, SDLColor fg, SDLColor bg);

		[DllImport(TTF_DLL)]
		private static extern IntPtr TTF_RenderGlyph_Shaded(IntPtr font, UInt16 ch, SDLColor fg, SDLColor bg);
/*
		[DllImport(TTF_DLL)]
		private static extern IntPtr TTF_RenderText_Blended(IntPtr font, string text, SDLColor fg);

		[DllImport(TTF_DLL)]
		private static extern IntPtr TTF_RenderUTF8_Blended(IntPtr font, string text, SDLColor fg);
*/
		[DllImport(TTF_DLL)]
		private static extern IntPtr TTF_RenderUNICODE_Blended(IntPtr font, [MarshalAs(UnmanagedType.LPWStr)] string text, SDLColor fg);

		[DllImport(TTF_DLL)]
		private static extern IntPtr TTF_RenderGlyph_Blended(IntPtr font, UInt16 ch, SDLColor fg);

		[DllImport(TTF_DLL)]
		private static extern void TTF_CloseFont(IntPtr font);

		/// <summary>
		/// Font Constructor
		/// </summary>
		/// <param name="Filename"></param>
		/// <param name="PointSize"></param>
		public Font(string Filename, int PointSize) {
			mSDLTTF = SDLTTF.Instance;
			mSDL = SDL.Instance;
			mFont = TTF_OpenFont(Filename, PointSize);
			if (mFont == IntPtr.Zero) throw SDLTTFException.Generate();
		}

		internal Font(IntPtr pFont) {
			mSDL = SDL.Instance;
			mFont = pFont;
		}

		// Possibly add Bold/Italic/Underline properties

		/// <summary>
		/// Style Property
		/// </summary>
		public Style Style {
			set { TTF_SetFontStyle(mFont, (int) value); }
			get { return (Style) TTF_GetFontStyle(mFont); }
		}

		/// <summary>
		/// Height Property
		/// </summary>
		public int Height {
			get { return TTF_FontHeight(mFont); }
		}

		/// <summary>
		/// Ascent Property
		/// </summary>
		public int Ascent {
			get { return TTF_FontAscent(mFont); }
		}

		/// <summary>
		/// Line Skip property
		/// </summary>
		public int LineSkip {
			get { return TTF_FontLineSkip(mFont); }
		}

		/// <summary>
		/// Size
		/// </summary>
		/// <param name="Text"></param>
		/// <returns></returns>
		public Size SizeText(string Text) {
			int Width, Height;

			TTF_SizeUNICODE(mFont, Text, out Width, out Height);
			return new Size(Width, Height);
		}

		/// <summary>
		/// Render Text to Solid
		/// </summary>
		/// <param name="Text"></param>
		/// <param name="Color"></param>
		/// <returns></returns>
		public Surface RenderTextSolid(string Text, SDLColor Color) {
			IntPtr pSurface;

			pSurface = TTF_RenderUNICODE_Solid(mFont, Text, Color);
			if (pSurface == IntPtr.Zero) throw SDLTTFException.Generate();
			return mSDL.Video.GenerateSurfaceFromPointer(pSurface);
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
		public void RenderTextSolid(string Text, SDLColor Color, Surface DestSurface, int X, int Y) 
		{
			Surface FontSurface;
			System.Drawing.Rectangle DestRect;

			FontSurface = RenderTextSolid(Text, Color);
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
		public Surface RenderTextShaded(string Text, SDLColor FG, SDLColor BG) {
			IntPtr pSurface;

			pSurface = TTF_RenderUNICODE_Shaded(mFont, Text, FG, BG);
			if (pSurface == IntPtr.Zero) throw SDLTTFException.Generate();
			return mSDL.Video.GenerateSurfaceFromPointer(pSurface);
		}

		/// <summary>
		/// Blended Text
		/// </summary>
		/// <param name="Text"></param>
		/// <param name="FG"></param>
		/// <returns></returns>
		public Surface RenderTextBlended(string Text, SDLColor FG) {
			IntPtr pSurface;

			pSurface = TTF_RenderUNICODE_Blended(mFont, Text, FG);
			if (pSurface == IntPtr.Zero) throw SDLTTFException.Generate();
			return mSDL.Video.GenerateSurfaceFromPointer(pSurface);
		}

		/// <summary>
		/// Render Glyphs as Solid
		/// </summary>
		/// <param name="Character"></param>
		/// <param name="FG"></param>
		/// <returns></returns>
		public Surface RenderGlyphSolid(UInt16 Character, SDLColor FG) {
			IntPtr pSurface;

			pSurface = TTF_RenderGlyph_Solid(mFont, Character, FG);
			if (pSurface == IntPtr.Zero) throw SDLTTFException.Generate();
			return mSDL.Video.GenerateSurfaceFromPointer(pSurface);
		}

		/// <summary>
		/// Shade Glyphs
		/// </summary>
		/// <param name="Character"></param>
		/// <param name="FG"></param>
		/// <param name="BG"></param>
		/// <returns></returns>
		public Surface RenderGlyphShaded(UInt16 Character, SDLColor FG, SDLColor BG) {
			IntPtr pSurface;

			pSurface = TTF_RenderGlyph_Shaded(mFont, Character, FG, BG);
			if (pSurface == IntPtr.Zero) throw SDLTTFException.Generate();
			return mSDL.Video.GenerateSurfaceFromPointer(pSurface);
		}

		/// <summary>
		/// Blend glyphs
		/// </summary>
		/// <param name="Character"></param>
		/// <param name="FG"></param>
		/// <returns></returns>
		public Surface RenderGlyphBlended(UInt16 Character, SDLColor FG) {
			IntPtr pSurface;

			pSurface = TTF_RenderGlyph_Blended(mFont, Character, FG);
			if (pSurface == IntPtr.Zero) throw SDLTTFException.Generate();
			return mSDL.Video.GenerateSurfaceFromPointer(pSurface);
		}

		/// <summary>
		/// Destructor
		/// </summary>
		~Font() {
			if (mFont != IntPtr.Zero) {
				TTF_CloseFont(mFont);
				mFont = IntPtr.Zero;
			}
		}
	}
}
