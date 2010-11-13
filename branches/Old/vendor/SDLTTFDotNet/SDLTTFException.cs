using System;
using SDLDotNet;

namespace SDLTTFDotNet 
{
	public class SDLTTFException : Exception 
	{
		public SDLTTFException() : base(Natives.SDL_GetError())
		{
		}
	}
}
