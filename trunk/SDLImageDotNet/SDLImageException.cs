using System;
using SDLDotNet;

namespace SDLDotNet.Image
{
	/// <summary>
	/// Exception class for SDLDotNet.Images
	/// </summary>
	public class SDLImageException : SDLException 
	{
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="msg">Exception message</param>
		public SDLImageException(string msg) : base(msg) {}
	}
}
