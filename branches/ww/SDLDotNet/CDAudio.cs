using System;
using System.Runtime.InteropServices;

namespace SDLDotNet {
	/// <summary>
	/// Indicates the current CD Status
	/// </summary>
	/// <type>enum</type>
	public enum CDStatus {
		/// <summary>
		/// The CD tray is empty
		/// </summary>
		TrayEmpty,
		/// <summary>
		/// Playing is stopped
		/// </summary>
		Stopped,
		/// <summary>
		/// CD is playing 
		/// </summary>
		Playing,
		/// <summary>
		/// CD is paused
		/// </summary>
		Paused,
		/// <summary>
		/// An error occured while fetching the status
		/// </summary>
		Error = -1
	}

	/// <summary>
	/// Represents a CD-ROM drive on the system
	/// </summary>
	/// <type>class</type>
	/// <implements>System.IDisposable</implements>
	public class CDDrive : IDisposable {
		private bool _disposed;
		private IntPtr _handle;
		private int _index;

		internal CDDrive(IntPtr handle, int index) {
			_disposed = false;
			_handle = handle;
			_index = index;
		}

		/// <protected/>
		~CDDrive() {
			Natives.SDL_CDClose(_handle);
		}

		/// <summary>
		/// Closes and destroys this CDDrive object
		/// </summary>
		public void Dispose() {
			if (!_disposed) {
				_disposed = true;
				Natives.SDL_CDClose(_handle);
				GC.SuppressFinalize(this);
			}
		}

		/// <summary>
		/// The drive number
		/// </summary>
		/// <proptype>System.Int32</proptype>
		/// <readonly/>
		public int Index { get { return _index; } }

		/// <summary>
		/// Closes and destroys this CDDrive object
		/// </summary>
		public void Close() {
			Dispose();
		}

		/// <summary>
		/// Gets the current drive status
		/// </summary>
		/// <proptype>SDLDotNet.CDStatus</proptype>
		/// <readonly/>
		public CDStatus Status {
			get { return (CDStatus)Natives.SDL_CDStatus(_handle); }
		}

		/// <summary>
		/// Plays the CD in the drive
		/// </summary>
		/// <param name="startmins">The number of minutes on the CD to start playing from</param>
		/// <param name="startsecs">The number of seconds on the CD to start playing from</param>
		/// <param name="lengthmins">The number of minutes on the CD to play</param>
		/// <param name="lengthsecs">The number of seconds on the CD to play</param>
		public void Play(int startmins, int startsecs, int lengthmins, int lengthsecs) {
			if (Natives.SDL_CDPlay(_handle, CDAudio.MinSecFramesToFrames(startmins, startsecs, 0),
				CDAudio.MinSecFramesToFrames(lengthmins, lengthsecs, 0)) == -1)
				throw SDLException.Generate();
		}
		/// <summary>
		/// Plays the CD in the drive
		/// </summary>
		/// <param name="startframes">The number of frames (75th of a second increments) on the CD to start playing from</param>
		/// <param name="lengthframes">The number of frames (75th of a second increments) to play</param>
		public void Play(int startframes, int lengthframes) {
			if (Natives.SDL_CDPlay(_handle, startframes, lengthframes) == -1)
				throw SDLException.Generate();
		}
		/// <summary>
		/// Plays the tracks on a CD in the drive
		/// </summary>
		/// <param name="starttrack">The starting track to play (numbered 0-99)</param>
		/// <param name="numtracks">The number of tracks to play</param>
		public void PlayTracks(int starttrack, int numtracks) {
			if (Natives.SDL_CDPlayTracks(_handle, starttrack, 0, numtracks, 0) == -1)
				throw SDLException.Generate();
		}
		/// <summary>
		/// Plays the tracks on a CD in the drive
		/// </summary>
		/// <param name="starttrack">The starting track to play (numbered 0-99)</param>
		/// <param name="startframe">The frame (75th of a second increment) offset from the starting track to play from</param>
		/// <param name="numtracks">The number of tracks to play</param>
		/// <param name="numframes">The frame (75th of a second increment) offset after the last track to stop playing after</param>
		public void PlayTracks(int starttrack, int startframe, int numtracks, int numframes) {
			if (Natives.SDL_CDPlayTracks(_handle, starttrack, startframe, numtracks, numframes) == -1)
				throw SDLException.Generate();
		}
		/// <summary>
		/// Pauses the CD in this drive
		/// </summary>
		public void Pause() {
			if (Natives.SDL_CDPause(_handle) == -1)
				throw SDLException.Generate();
		}
		/// <summary>
		/// Resumes a previously paused CD in this drive
		/// </summary>
		public void Resume() {
			if (Natives.SDL_CDResume(_handle) == -1)
				throw SDLException.Generate();
		}
		/// <summary>
		/// Stops playing the CD in this drive
		/// </summary>
		public void Stop() {
			if (Natives.SDL_CDStop(_handle) == -1)
				throw SDLException.Generate();
		}
		/// <summary>
		/// Ejects this drive
		/// </summary>
		public void Eject() {
			if (Natives.SDL_CDEject(_handle) == -1)
				throw SDLException.Generate();
		}

		/// <summary>
		/// Gets the number of tracks in the currently inserted CD
		/// </summary>
		/// <proptype>System.Int32</proptype>
		/// <readonly/>
		public int NumTracks {
			get {
				Natives.SDL_CDStatus(_handle);
				Natives.SDL_CD cd = (Natives.SDL_CD)Marshal.PtrToStructure(_handle, typeof(Natives.SDL_CD));
				return cd.numtracks;
			}
		}
		/// <summary>
		/// Gets the currently playing track
		/// </summary>
		/// <proptype>System.Int32</proptype>
		/// <readonly/>
		public int CurrentTrack {
			get {
				Natives.SDL_CDStatus(_handle);
				Natives.SDL_CD cd = (Natives.SDL_CD)Marshal.PtrToStructure(_handle, typeof(Natives.SDL_CD));
				return cd.cur_track;
			}
		}
		/// <summary>
		/// Gets the currently playing frame (75th of a second increment)
		/// </summary>
		/// <proptype>System.Int32</proptype>
		/// <readonly/>
		public int CurrentFrame {
			get {
				Natives.SDL_CDStatus(_handle);
				Natives.SDL_CD cd = (Natives.SDL_CD)Marshal.PtrToStructure(_handle, typeof(Natives.SDL_CD));
				return cd.cur_frame;
			}
		}
	}

	/// <summary>
	/// Contains methods for playing audio CDs.
	/// Obtain an instance of this class by accessing the CDAudio property of the main SDL object
	/// </summary>
	/// <type>class</type>
	public class CDAudio {
		internal CDAudio() {
			if (Natives.SDL_InitSubSystem((int)Natives.Init.Cdrom) != 0)
				throw SDLException.Generate();
		}

		/// <summary>
		/// Gets the number of CD-ROM drives available on the system
		/// </summary>
		/// <proptype>System.Int32</proptype>
		/// <readonly/>
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
		/// <returntype>SDLDotNet.CDDrive</returntype>
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
		/// <returntype>System.String</returntype>
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
		/// <static/>
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
		/// <returntype>System.Int32</returntype>
		/// <returns>The total number of frames</returns>
		/// <static/>
		public static int MinSecFramesToFrames(int M, int S, int F) {
			return (M * 60 * FramesPerSecond + S * FramesPerSecond + F);
		}
		/// <summary>
		/// Gets a static value indicating the number of frames in a second (75)
		/// </summary>
		/// <proptype>System.Int32</proptype>
		/// <static/>
		/// <readonly/>
		public static int FramesPerSecond { get { return 75; } }
	}
}