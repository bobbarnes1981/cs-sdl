using System;
using SDLDotNet;

namespace SDLDotNet.Images 
{
	/// <summary>
	/// Exception class for SDLDotNet.Images
	/// </summary>
	public class SDLImageException : SDLException 
	{
		public SDLImageException(string msg) : base(msg) {}
	}
}
