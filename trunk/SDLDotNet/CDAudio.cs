using System;
using System.Runtime.InteropServices;

namespace SDLDotNet {

	/// <summary>
	/// Contains methods for playing audio CDs.
	/// Obtain an instance of this class by accessing the CDAudio property of the main SDL object
	/// </summary>
	public class CDAudio {
		internal CDAudio() {
			if (Natives.SDL_InitSubSystem((int)Natives.Init.Cdrom) != 0)
				throw SDLException.Generate();
		}

		/// <summary>
		/// Gets the number of CD-ROM drives available on the system
		/// </summary>
		public int NumDrives {
			get {
				int ret = Natives.SDL_CDNumDrives();
				if (ret == -1)
					throw SDLException.Generate();
				return ret;
			}
		}

		/// <summary>
		/// Opens a CD-ROM drive for manipulation
		/// </summary>
		/// <param name="index">The number of the drive to open, from 0 - CDAudio.NumDrives</param>
		/// <returns>The CDDrive object representing the CD-ROM drive</returns>
		public CDDrive OpenDrive(int index) {
			IntPtr cd = Natives.SDL_CDOpen(index);
			if (cd == IntPtr.Zero)
				throw SDLException.Generate();
			return new CDDrive(cd, index);
		}

		/// <summary>
		/// Returns a platform-specific name for a CD-ROM drive
		/// </summary>
		/// <param name="index">The number of the drive</param>
		/// <returns>A platform-specific name, i.e. "D:\"</returns>
		public string DriveName(int index) {
			if (index < 0 || index >= NumDrives)
				throw new SDLException("Device index out of range");
			return Natives.SDL_CDName(index);
		}

		/// <summary>
		/// Converts frames (75th of a second) to minutes, seconds and leftover frames
		/// </summary>
		/// <param name="f">The number of frames to convert</param>
		/// <param name="M">A reference to a variable to receive the number of minutes</param>
		/// <param name="S">A reference to a variable to receive the number of seconds</param>
		/// <param name="F">A reference to a variable to receive the number of leftover frames</param>
		public static void FramesToMinSecFrames(int f, out int M, out int S, out int F) {
			F = f % FramesPerSecond;
			f /= FramesPerSecond;
			S = f % 60;
			f /= 60;
			M = f;
		}
		/// <summary>
		/// Converts minutes, seconds and frames into a frames value
		/// </summary>
		/// <param name="M">The number of minutes to convert</param>
		/// <param name="S">The number of seconds to convert</param>
		/// <param name="F">The number of frames to convert</param>
		/// <returns>The total number of frames</returns>
		public static int MinSecFramesToFrames(int M, int S, int F) {
			return (M * 60 * FramesPerSecond + S * FramesPerSecond + F);
		}
		/// <summary>
		/// Gets a static value indicating the number of frames in a second (75)
		/// </summary>
		public static int FramesPerSecond { get { return 75; } }
	}
}
