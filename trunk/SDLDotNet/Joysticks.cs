using System;

namespace SDLDotNet {
	/// <summary>
	/// Provides methods for querying the number and make-up of the joysticks on a system.
	/// You can obtain an instance of this class by accessing the Joysticks property of the main SDL object.
	/// Note that actual joystick input is handled by the Events class
	/// </summary>
	public class Joysticks {
		internal Joysticks() {
			if (Natives.SDL_InitSubSystem((int)Natives.Init.Joystick) != 0)
				throw SDLException.Generate();
		}

		/// <summary>
		/// Returns the number of joysticks on this system
		/// </summary>
		/// <returns>The number of joysticks</returns>
		public int NumJoysticks() {
			return Natives.SDL_NumJoysticks();
		}
		/// <summary>
		/// Creates a joystick object to read information about a joystick
		/// </summary>
		/// <param name="index">The 0-based index of the joystick to read</param>
		/// <returns>A Joystick object</returns>
		public Joystick OpenJoystick(int index) {
			IntPtr joy = Natives.SDL_JoystickOpen(index);
			if (joy == IntPtr.Zero)
				throw SDLException.Generate();
			return new Joystick(joy);
		}
	}
}
