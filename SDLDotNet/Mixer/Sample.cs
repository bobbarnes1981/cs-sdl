using System;

namespace SDLDotNet.Mixer {
	
	/// <summary>
	/// Represents a sound sample.
	/// Create with Mixer.LoadWav().
	/// </summary>
	/// <implements>System.IDisposable</implements>
	public class Sample : IDisposable {
		private IntPtr _handle;
		private bool _disposed;

		internal Sample(IntPtr handle) {
			_handle = handle;
			_disposed = false;
		}

		internal IntPtr GetHandle() { return _handle; }

		/// <protected/>
		~Sample() {
			Natives.Mix_FreeChunk(_handle);
		}

		/// <summary>
		/// Destroys this Sample and frees the memory associated with it
		/// </summary>
		public void Dispose() {
			if (!_disposed) {
				_disposed = true;
				Natives.Mix_FreeChunk(_handle);
				GC.SuppressFinalize(this);
			}
		}

		/// <summary>
		/// Sets the volume of the sample
		/// </summary>
		/// <param name="volume">New volume. Should be between 0 and 128 inclusive.</param>
		public void SetVolume(int volume) {
			if (Natives.Mix_VolumeChunk(_handle, volume) != 0)
				throw SDLException.Generate();
		}
	}
}
