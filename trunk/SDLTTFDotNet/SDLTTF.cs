using System;
using System.Runtime.InteropServices;
using SDLDotNet;
/*
	REVISION HISTORY

	Mon 31 Mar 2003 23:28:02 EST LM
	Changed namespace from SDLTTFDotNet
	Now using singleton architecture

	Tue 25 Mar 2003 17:50:09 EST LM
	Added error check to TTF_Init call.  It will return -1 if there was a problem.

	Mon 24 Mar 2003 20:45:40 EST LM
	There is currently a bug in mono which meant this class did not need an instance of SDL.
	I have fixed this so it does not depend on that bug.
*/
namespace SDLDotNet.TTF
{
	public enum Style {
		Normal = 0x00,
		Bold = 0x01,
		Italic = 0x02,
		Underline = 0x04
	}

	public class SDLTTF
	{
		const string TTF_DLL = "SDL_ttf";
		static private SDLTTF mInstance;

		[DllImport(TTF_DLL)]
		private static extern int TTF_Init();

		[DllImport(TTF_DLL)]
		private static extern void TTF_Quit();

		public static SDLTTF Instance {
			get {
				if (mInstance == null) mInstance = new SDLTTF();
				return mInstance;
			}
		}

		private SDLTTF() {
			if (TTF_Init() != 0)
				SDLTTFException.Generate();
		}

		~SDLTTF() {
			TTF_Quit();
		}
	}
}
