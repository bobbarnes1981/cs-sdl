using System;
using SDLDotNet;

/*
	REVISION HISTORY

	Tue 25 Mar 2003 18:19:03 EST LM
	Basically removed all code due to Natives.SDL_GetError being inaccessible.
*/
namespace SDLTTFDotNet 
{
	public class SDLTTFException : SDLException 
	{
		public SDLTTFException(string msg) : base(msg) {}
	}
}
