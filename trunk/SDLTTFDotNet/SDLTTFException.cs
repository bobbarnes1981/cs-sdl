using System;
using SDLDotNet;

/*
	REVISION HISTORY

	Mon 31 Mar 2003 23:28:02 EST LM
	Changed namespace from SDLTTFDotNet

	Tue 25 Mar 2003 18:19:03 EST LM
	Basically removed all code due to Natives.SDL_GetError being inaccessible.
*/
namespace SDLDotNet.TTF {
	/// <summary>
	/// 
	/// </summary>
	public class SDLTTFException : SDLException {
		/// <summary>
		/// 
		/// </summary>
		/// <param name="msg"></param>
		public SDLTTFException(string msg) : base(msg) {}
	}
}
