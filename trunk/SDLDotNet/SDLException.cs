using System;

namespace SDLDotNet {
	/// <summary>
	/// Represents a run-time error from the SDL library
	/// </summary>
	public class SDLException : Exception {
		/// <summary>
		/// Initializes an SDLException instance
		/// </summary>
		/// <param name="msg">The string representing the error message</param>
		public SDLException(string msg) : base(msg) {}

		/// <summary>
		/// Generates an SDLException based on the last SDL Error code
		/// </summary>
		/// <returns>A new SDLException object</returns>
		public static SDLException Generate() {
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
	public class SurfaceLostException : SDLException {
		internal SurfaceLostException(string msg) : base(msg) {}
	}
}
