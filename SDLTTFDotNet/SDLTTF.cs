using System;
using System.Runtime.InteropServices;
using SDLDotNet;
/*
	REVISION HISTORY
	Mon 24 Mar 2003 20:45:40 EST LM
	There is currently a bug in mono which meant this class did not need an instance of SDL.
	I have fixed this so it does not depend on that bug.
*/
namespace SDLTTFDotNet
{
	public enum Style {
		Normal = 0x00,
		Bold = 0x01,
		Italic = 0x02,
		Underline = 0x04
	}

	public class SDLTTF
	{
		const string TTF_DLL = "SDL_ttf.dll";

		[DllImport(TTF_DLL)]
		private static extern int TTF_Init();

		[DllImport(TTF_DLL)]
		private static extern void TTF_Quit();

		private SDL mSDL;

		public SDLTTF(SDL SDL) {
			mSDL = SDL;
			TTF_Init();
		}

		~SDLTTF() {
			TTF_Quit();
		}

		public Font OpenFont(string Filename, int PointSize) {
			return new Font(mSDL, Filename, PointSize);
		}
	}
}
