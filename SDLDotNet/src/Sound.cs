/*
 * $RCSfile$
 * Copyright (C) 2004, 2005 David Hudson (jendave@yahoo.com)
 *
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later version.
 * 
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 * 
 * You should have received a copy of the GNU Lesser General Public
 * License along with this library; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
 */

using System;
using System.Runtime.InteropServices;
using Tao.Sdl;

namespace SdlDotNet 
{
	/// <summary>
	/// 
	/// </summary>
	public delegate void SoundEventHandler(object sender, SoundEventArgs e);
	/// <summary>
	/// Represents a sound sample.
	/// Create with Mixer.LoadWav().
	/// </summary>
	public class Sound : BaseSdlResource 
	{
		private int channels;
		private bool disposed;
		private long size;
		/// <summary>
		/// 
		/// </summary>
		public event SoundEventHandler SoundEvent;
		
		internal Sound(IntPtr handle) 
		{
			this.Handle = handle;
		}

		internal Sound(IntPtr handle, long size) 
		{
			this.Handle = handle;
			this.size = size;
		}

		/// <summary>
		/// Internal constructor which makes reference to the given object.
		/// </summary>
		/// <param name="sound">The sound object to make reference to.</param>
		internal Sound(Sound sound) : this(sound.Handle, sound.size)
		{
			channels = sound.channels;
			this.Volume = sound.Volume;
		}

		/// <summary>
		/// Loads a .wav file into memory.
		/// </summary>
		/// <param name="file">The file to load into memory.</param>
		public Sound(string file) : this(Mixer.Sound(file))
		{
		}

		/// <summary>
		/// Destroys the surface object and frees its memory
		/// </summary>
		/// <param name="disposing"></param>
		protected override void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				try
				{
					if (disposing)
					{
					}
					CloseHandle();
					//GC.KeepAlive(this);
					GC.SuppressFinalize(this);
					this.disposed = true;
				}
				finally
				{
					base.Dispose(disposing);
					this.disposed = true;
				}
			}
		}

//		/// <summary>
//		/// 
//		/// </summary>
//		public override bool HasBeenDisposed
//		{
//			get
//			{
//				return this.disposed;
//			}
//		}

		/// <summary>
		/// Closes Music handle
		/// </summary>
		protected override void CloseHandle() 
		{
			if (this.Handle != IntPtr.Zero)
			{
				SdlMixer.Mix_FreeChunk(this.Handle);
				GC.KeepAlive(this);
				this.Handle = IntPtr.Zero;
			}
		}

		/// <summary>
		/// Gets/Set the volume of the sound. Should be between 0 and 128 inclusive.
		/// </summary>
		public int Volume
		{
			get
			{
				int result = SdlMixer.Mix_VolumeChunk(this.Handle, -1);
				GC.KeepAlive(this);
				return result;
			}
			set
			{
				if (value >= 0 && value <= SdlMixer.MIX_MAX_VOLUME)
				{
					SdlMixer.Mix_VolumeChunk(this.Handle, value);
				}
				GC.KeepAlive(this);
			}
		}

		/// <summary>
		/// Returns sound as an array of bytes.
		/// </summary>
		public byte[] Array()
		{
				byte[] array = new byte[this.size];
				Marshal.Copy(this.Handle, array, 0, (int)this.size);
				return array;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public Channel Play()
		{
			return this.Play(0);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="loops"></param>
		/// <returns></returns>
		public Channel Play(int loops) 
		{
			return this.Play(loops, (int) SdlFlag.InfiniteLoop);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="loopIndefinitely"></param>
		/// <returns></returns>
		public Channel Play(bool loopIndefinitely) 
		{
			if (loopIndefinitely == true)
			{
				return this.Play(-1, (int) SdlFlag.InfiniteLoop);
			}
			else
			{
				return this.Play(0);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="loops"></param>
		/// <param name="milliseconds"></param>
		/// <returns></returns>
		public Channel Play(int loops, int milliseconds) 
		{
			int index = 
				SdlMixer.Mix_PlayChannelTimed
				(
				Mixer.FindAvailableChannel(), 
				this.Handle, 
				loops, 
				milliseconds
				);

			if (index == (int) SdlFlag.Error)
			{
				throw SdlException.Generate();
			}
			this.channels++;
			return new Channel(index);
		}

		/// <summary>
		/// Fades in a sample once using the first available channel
		/// </summary>
		/// <param name="milliseconds">The number of milliseconds to fade in for</param>
		/// <returns>The channel used to play the sample</returns>
		public Channel FadeIn(int milliseconds) 
		{
			int index = SdlMixer.Mix_FadeInChannelTimed(Mixer.FindAvailableChannel(), this.Handle, 0, milliseconds, -1);
			if (index == (int) SdlFlag.Error)
			{
				throw SdlException.Generate();
			}
			this.channels++;
			return new Channel(index);
		}

		/// <summary>
		/// Fades in a sample the specified number of times using 
		/// the first available channel
		/// </summary>
		/// <param name="milliseconds">
		/// The number of milliseconds to fade in for
		/// </param>
		/// <param name="loops">The number of loops.  
		/// Specify 1 to have the sample play twice</param>
		/// <returns>The channel used to play the sample</returns>
		public Channel FadeIn(int milliseconds, int loops) 
		{
			int index = SdlMixer.Mix_FadeInChannelTimed(Mixer.FindAvailableChannel(), this.Handle, loops, milliseconds, -1);
			if (index == (int) SdlFlag.Error)
			{
				throw SdlException.Generate();
			}
			this.channels++;
			return new Channel(index);
		}

		/// <summary>
		/// Fades in a sample once using the first available channel, 
		/// stopping after the specified number of ms
		/// </summary>
		/// <param name="milliseconds">The number of milliseconds to fade in for</param>
		/// <param name="ticks">The time limit in milliseconds</param>
		/// <returns>The channel used to play the sample</returns>
		public Channel FadeInTimed(int milliseconds, int ticks) 
		{
			int index = SdlMixer.Mix_FadeInChannelTimed(Mixer.FindAvailableChannel(), this.Handle, 0, milliseconds, ticks);
			if (index == (int) SdlFlag.Error)
			{
				throw SdlException.Generate();
			}
			this.channels++;
			return new Channel(index);
		}

		/// <summary>
		/// Fades in a sample the specified number of times using 
		/// the first available channel, stopping after the 
		/// specified number of ms
		/// </summary>
		/// <param name="milliseconds">The number of milliseconds to fade in for</param>
		/// <param name="loops">The number of loops.  
		/// Specify 1 to have the sample play twice</param>
		/// <param name="ticks">The time limit in milliseconds</param>
		/// <returns>The channel used to play the sample</returns>
		public Channel FadeInTimed(int milliseconds, int loops, int ticks) 
		{
			int index = SdlMixer.Mix_FadeInChannelTimed(Mixer.FindAvailableChannel(), this.Handle, loops, milliseconds, ticks);
			if (index == (int) SdlFlag.Error)
			{
				throw SdlException.Generate();
			}
			this.channels++;
			return new Channel(index);
		}

		/// <summary>
		/// 
		/// </summary>
		public int NumberOfChannels
		{
			get
			{
				return this.channels;
			}
			set
			{
				this.channels = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public void Stop()
		{
			SoundEventArgs args = new SoundEventArgs(SoundAction.Stop);
			OnSoundEvent(args);
		}

		/// <summary>
		/// 
		/// </summary>
		public void Fadeout(int fadeoutTime)
		{
			SoundEventArgs args = new SoundEventArgs(SoundAction.Stop, fadeoutTime);
			OnSoundEvent(args);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		protected void OnSoundEvent(SoundEventArgs e)
		{
			if (SoundEvent != null)
			{
				SoundEvent(this, e);
			}
		}
	}
}
