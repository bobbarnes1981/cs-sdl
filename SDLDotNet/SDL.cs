using System;
using SDLDotNet.Mixer;
// Assembly marked as compliant.
//[assembly: CLSCompliantAttribute(true)]
namespace SDLDotNet {
	/// <summary>
	/// The main SDL object.
	/// Only one of these should be created for any SDL application.
	/// If you wish to shut down the SDL library before the end of the program, call the Dispose()
	/// method.
	/// All the functionality of the SDL library is available through this class and its properties.
	/// </summary>
	public class SDL : IDisposable 
	{
		private Video _video;
		private Events _events;
		private WindowManager _wm;
		private Joysticks _joy;
		private CDAudio _cd;
		private SDLMixer _mix;
		private bool _disposed;
		private static SDL _instance = null;

		/// <summary>
		/// Returns the global instance of this class.
		/// </summary>
		public static SDL Instance {
			get {
				if (_instance == null)
					_instance = new SDL();
				return _instance;
			}
		}

		private SDL() {
			if (Natives.SDL_Init(0) == -1)
				throw SDLException.Generate();
			_video = null;
			_events = null;
			_wm = null;
			_joy = null;
			_cd = null;
			_mix = null;
			_disposed = false;
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
		public Video Video {
			get {
				if (_video == null)
					_video = new Video();
				return _video;
			}
		}

		/// <summary>
		/// Gets an object from which the event libraries can be accessed
		/// </summary>
		public Events Events {
			get {
				if (_events == null)
					_events = new Events(Video);
				return _events;
			}
		}

		/// <summary>
		/// Gets an object from which the window manager libraries can be accessed
		/// </summary>
		public WindowManager WindowManager {
			get {
				if (_wm == null)
					_wm = new WindowManager(Video);
				return _wm;
			}
		}

		/// <summary>
		/// Gets an object from which the sound libraries can be accessed
		/// </summary>
		public SDLMixer Mixer {
			get {
				if (_mix == null)
					_mix = new SDLMixer(Events);
				return _mix;
			}
		}

		/// <summary>
		/// Gets an object from which the CD audio can be accessed
		/// </summary>
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
		public Joysticks Joysticks {
			get {
				if (_joy == null)
					_joy = new Joysticks();
				return _joy;
			}
		}

		/// <summary>
		/// Gets the number of ticks since SDL was initialized.  
		/// This is not a high-resolution timer
		/// </summary>
		public uint GetTicks() {
			return Natives.SDL_GetTicks();
		}

		/// <summary>
		/// Enable keyboard autorepeat
		/// </summary>
		/// <param name="delay">Delay in system ticks before repeat starts. Set to 0 to disable key repeat</param>
		/// <param name="rate">Rate in system ticks at which key repeats</param>
		public void EnableKeyRepeat(int delay, int rate) {
			if (Natives.SDL_EnableKeyRepeat(delay, rate) == -1)
				throw SDLException.Generate();
		}
	}
}
