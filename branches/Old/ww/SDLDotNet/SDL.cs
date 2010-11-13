using System;

namespace SDLDotNet {
	/// <summary>
	/// The main SDL object.
	/// Only one of these should be created for any SDL application.
	/// If you wish to shut down the SDL library before the end of the program, call the Dispose()
	/// method.
	/// All the functionality of the SDL library is available through this class and its properties.
	/// </summary>
	/// <type>class</type>
	/// <implements>System.IDisposable</implements>
	public class SDL : IDisposable {
		private Video _video;
		private Events _events;
		private WindowManager _wm;
		private Joysticks _joy;
		private CDAudio _cd;
		private Mixer _mix;
		private bool _disposed;

		/// <summary>
		/// Creates a new SDL object
		/// </summary>
		/// <param name="initaudio">if True, sound will be initialized</param>
		public SDL(bool initaudio) {
			Natives.Init flags = Natives.Init.Video;
			if (initaudio)
				flags |= Natives.Init.Audio;
			if (Natives.SDL_Init((int)flags) == -1)
				throw SDLException.Generate();
			_video = new Video();
			_events = new Events();
			_wm = new WindowManager();
			_joy = null;
			_cd = null;
			_disposed = false;
			if (initaudio)
				_mix = new Mixer(_events);
			else
				_mix = null;
		}

		/// <protected/>
		~SDL() {
			Natives.SDL_Quit();
		}

		/// <summary>
		/// Destroys this object and shuts down the SDL library
		/// </summary>
		public void Dispose() {
			if (!_disposed) {
				_disposed = true;
				Natives.SDL_Quit();
				GC.SuppressFinalize(this);
			}
		}

		/// <summary>
		/// Gets an object from which the video libraries can be accessed
		/// </summary>
		/// <proptype>SDLDotNet.Video</proptype>
		/// <readonly/>
		public Video Video {
			get { return _video; }
		}

		/// <summary>
		/// Gets an object from which the event libraries can be accessed
		/// </summary>
		/// <proptype>SDLDotNet.Events</proptype>
		/// <readonly/>
		public Events Events {
			get { return _events; }
		}

		/// <summary>
		/// Gets an object from which the window manager libraries can be accessed
		/// </summary>
		/// <proptype>SDLDotNet.WindowManager</proptype>
		/// <readonly/>
		public WindowManager WindowManager {
			get { return _wm; }
		}

		/// <summary>
		/// Gets an object from which the sound libraries can be accessed
		/// </summary>
		/// <proptype>SDLDotNet.Mixer</proptype>
		/// <readonly/>
		public Mixer Mixer {
			get {
				if (_mix == null)
					throw new SDLException("Audio not initialized");
				return _mix;
			}
		}

		/// <summary>
		/// Gets an object from which the CD audio can be accessed
		/// </summary>
		/// <proptype>SDLDotNet.CDAudio</proptype>
		/// <readonly/>
		public CDAudio CDAudio {
			get {
				if (_cd == null)
					_cd = new CDAudio();
				return _cd;
			}
		}

		/// <summary>
		/// Gets an object from which the joystick libraries can be accessed
		/// </summary>
		/// <proptype>SDLDotNet.Joysticks</proptype>
		/// <readonly/>
		public Joysticks Joysticks {
			get {
				if (_joy == null)
					_joy = new Joysticks();
				return _joy;
			}
		}

		/// <summary>
		/// Gets the number of ticks since SDL was initialized
		/// This is not a high-resolution timer
		/// </summary>
		/// <returntype>System.UInt32</returntype>
		/// <returns>The number of ticks</returns>
		public uint GetTicks() {
			return Natives.SDL_GetTicks();
		}
	}
}
