using System;

namespace SDLDotNet {
	/// <summary>
	/// Represents a run-time error from the SDL library
	/// </summary>
	/// <type>class</type>
	/// <base>System.Exception</base>
	public class SDLException : Exception {
		internal SDLException(string msg) : base(msg) {}

		internal static SDLException Generate() {
			string msg = Natives.SDL_GetError();
			if (msg.IndexOf("Surface was lost") == -1)
				return new SDLException(msg);
			else
				return new SurfaceLostException(msg);
		}
	}
	/// <summary>
	/// Represents an error resulting from a surface being lost, usually as a result of
	/// the user changing the input focus away from a full-screen application
	/// </summary>
	/// <type>class</type>
	/// <base>SDLDotNet.SDLException</base>
	public class SurfaceLostException : SDLException {
		internal SurfaceLostException(string msg) : base(msg) {}
	}
}
