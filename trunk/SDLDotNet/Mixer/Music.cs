using System;

namespace SDLDotNet.Mixer {
	/// <summary>
	/// Represents a music sample.  Music is generally longer than a sound effect sample,
	/// however it can also be compressed e.g. by Ogg Vorbis
	/// </summary>
	public class Music : IDisposable {
		private IntPtr _handle;
		private bool _disposed;

		internal Music(IntPtr handle) {
			_handle = handle;
			_disposed = false;
		}

		internal IntPtr GetHandle() { return _handle; }

		/// <protected/>
		~Music() {
			Natives.Mix_FreeMusic(_handle);
		}

		/// <summary>
		/// Destroys this sample and frees the memory associated with it
		/// </summary>
		public void Dispose() {
			if (!_disposed) {
				_disposed = true;
				Natives.Mix_FreeMusic(_handle);
				GC.SuppressFinalize(this);
			}
		}
	}
}
