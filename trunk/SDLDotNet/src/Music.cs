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
using Tao.Sdl;

namespace SdlDotNet 
{
	

	/// <summary>
	/// Represents a music sample.  Music is generally longer than a sound effect sample,
	/// however it can also be compressed e.g. by Ogg Vorbis
	/// </summary>
	public sealed class Music : BaseSdlResource
	{
		private  SdlMixer.MusicFinishedDelegate MusicFinishedDelegate;

		private IntPtr handle;
		private string musicFilename;
		private bool disposed = false;
		private string queuedMusicFilename = null;

		static readonly Music instance = new Music();

		Music()
		{
			Mixer.Initialize();
		}

		internal static Music Instance
		{
			get
			{
				return instance;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		~Music()
		{
			Dispose(false);
		}

		internal IntPtr Handle
		{ 
			get
			{
				GC.KeepAlive(this);
				return handle; 
			}
			set
			{
				handle = value;
				GC.KeepAlive(this);
			}
		}

		/// <summary>
		/// Destroys the surface object and frees its memory
		/// </summary>
		/// <param name="disposing"></param>
		protected override void Dispose(bool disposing)
		{
			if (!disposed)
			{
				try
				{
					if (disposing)
					{
						CloseHandle(handle);
						GC.KeepAlive(this);
					}
					disposed = true;
				}

				finally
				{
					Dispose(disposing);
				}
			}
		}

		/// <summary>
		/// Closes Music handle
		/// </summary>
		protected override void CloseHandle(IntPtr handleToClose) 
		{
			SdlMixer.Mix_FreeMusic(handleToClose);
			handleToClose = IntPtr.Zero;
		}

		/// <summary>
		/// 
		/// </summary>
		public string MusicFilename
		{
			get
			{
				return this.musicFilename;
			}
			set
			{
				this.musicFilename = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string QueuedMusicFilename
		{
			get
			{
				return this.queuedMusicFilename;
			}
			set
			{
				this.queuedMusicFilename = value;
			}
		}

		/// <summary>
		/// Loads a music sound from disk.
		/// By default, Sdl_mixer only supports the Ogg Vorbis format,
		///  see http://www.vorbis.com/.
		/// It may be possible to compile in support for other formats
		///  such as MOD and and MP3.
		/// </summary>
		/// <param name="file">The filename to load</param>
		/// <returns>A new Music object</returns>
		public void Load(string file) 
		{
			IntPtr musicPtr = SdlMixer.Mix_LoadMUS(file);
			if (musicPtr == IntPtr.Zero)
			{
				throw SdlException.Generate();
			}
			this.musicFilename = file;
			this.Handle = musicPtr;
		}

		/// <summary>
		/// Plays a music sample
		/// </summary>
		public void Play() 
		{
			this.Play(1);
		}

		/// <summary>
		/// Plays a music sample
		/// </summary>
		public void Play(bool continuous) 
		{
			if (continuous == true)
			{
				this.Play(-1);
			}
			else
			{
				this.Play(1);
			}
		}

		/// <summary>
		/// Plays a music sample
		/// </summary>
		/// <param name="numberOfTimes">The number of times to play. 
		/// Specify 1 to play a single time, -1 to loop forever.</param>
		public void Play(int numberOfTimes) 
		{
			if (SdlMixer.Mix_PlayMusic(this.Handle, numberOfTimes) != 0)
			{
				throw SdlException.Generate();
			}
		}
		/// <summary>
		/// Plays a music sample and fades it in
		/// </summary>
		/// <param name="numberOfTimes">The number of times to play. 
		/// Specify 1 to play a single time, -1 to loop forever.</param>
		/// <param name="milliseconds">The number of milliseconds to fade in for</param>
		public void FadeIn(int numberOfTimes, int milliseconds) 
		{
			if (SdlMixer.Mix_FadeInMusic(this.Handle, numberOfTimes, milliseconds) != 0)
			{
				throw SdlException.Generate();
			}
		}
		/// <summary>
		/// Plays a music sample, starting from a specific 
		/// position and fades it in
		/// </summary>
		/// <param name="numberOfTimes">The number of times to play.
		///  Specify 1 to play a single time, -1 to loop forever.</param>
		/// <param name="milliseconds">The number of milliseconds to fade in for
		/// </param>
		/// <param name="position">A format-defined position value. 
		/// For Ogg Vorbis, this is the number of seconds from the
		///  beginning of the song</param>
		public void FadeInPosition(int numberOfTimes, 
			int milliseconds, double position) 
		{
			if (SdlMixer.Mix_FadeInMusicPos(this.Handle, 
				numberOfTimes, milliseconds, position) != 0)
			{
				throw SdlException.Generate();
			}
		}
		/// <summary>
		/// Sets the music volume between 0 and 128.
		/// </summary>
		public int Volume
		{
			get
			{
				return SdlMixer.Mix_VolumeMusic(-1);
			}
			set
			{
				int dummy = SdlMixer.Mix_VolumeMusic(value);
			}
		}

		/// <summary>
		/// Pauses the music playing
		/// </summary>
		public void Pause() 
		{
			SdlMixer.Mix_PauseMusic();
		}
		/// <summary>
		/// Resumes paused music
		/// </summary>
		public void Resume() 
		{
			SdlMixer.Mix_ResumeMusic();
		}
		/// <summary>
		/// Resets the music position to the beginning of the sample
		/// </summary>
		public void Rewind() 
		{
			SdlMixer.Mix_RewindMusic();
		}

		/// <summary>
		/// 
		/// </summary>
		public MusicType MusicType
		{
			get
			{
				return (MusicType) SdlMixer.Mix_GetMusicType(this.Handle);
			}
		}
		/// <summary>
		/// Sets the music position to a format-defined value.
		/// For Ogg Vorbis and Mp3, this is the number of seconds 
		/// from the beginning of the song
		/// </summary>
		/// <param name="musicPosition"></param>
		public void Position(double musicPosition) 
		{
			if (this.MusicType == MusicType.Mp3)
			{
				this.Rewind();
			}
			if (SdlMixer.Mix_SetMusicPosition(musicPosition) != 0)
			{
				throw SdlException.Generate();
			}
		}
		/// <summary>
		/// Stops playing music
		/// </summary>
		public void Stop() 
		{
			SdlMixer.Mix_HaltMusic();
		}
		/// <summary>
		/// Fades out music
		/// </summary>
		/// <param name="ms">
		/// The number of milliseconds to fade out for
		/// </param>
		public void FadeOut(int ms) 
		{
			if (SdlMixer.Mix_FadeOutMusic(ms) != 1)
			{
				throw SdlException.Generate();
			}
		}
		/// <summary>
		/// Returns a flag indicating whether or not music is playing
		/// </summary>
		/// <returns>True if music is playing, otherwise False</returns>
		public bool IsPlaying() 
		{
			return (SdlMixer.Mix_PlayingMusic() != 0);
		}
		/// <summary>
		/// Returns a flag indicating whether or not music is paused
		/// </summary>
		/// <returns>True if music is paused, otherwise False</returns>
		public bool IsPaused() 
		{
			return (SdlMixer.Mix_PausedMusic() != 0);
		}
		/// <summary>
		/// Returns a flag indicating whether or not music is fading
		/// </summary>
		/// <returns>True if music is fading in or out, 
		/// otherwise False
		/// </returns>
		public bool IsFading() 
		{
			return (SdlMixer.Mix_FadingMusic() != 0);
		}

		/// <summary>
		/// For performance reasons, you must call this method
		///  to enable the Events.ChannelFinished and 
		///  Events.MusicFinished events
		/// </summary>
		public void EnableMusicFinishedCallback() 
		{
			this.MusicFinishedDelegate = new SdlMixer.MusicFinishedDelegate(this.MusicFinished);
			SdlMixer.Mix_HookMusicFinished(MusicFinishedDelegate);
			Events.MusicFinished +=new MusicFinishedEventHandler(Events_MusicFinished);
			}

		private void MusicFinished() 
		{
			Events.NotifyMusicFinished();
		}

		private void Events_MusicFinished(object sender, MusicFinishedEventArgs e)
		{
			if (this.QueuedMusicFilename != null)
			{
				this.MusicFilename = this.QueuedMusicFilename;
				this.Play();
				this.QueuedMusicFilename = null;
			}
		}
	}
}
