/*
 * $RCSfile$
 * Copyright (C) 2004 David Hudson (jendave@yahoo.com)
 * Copyright (C) 2003 Will Weisser (ogl@9mm.com)
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
using System.Threading;
using SdlDotNet;
using Tao.Sdl;

namespace SdlDotNet
{
	/// <summary>
	/// Provides methods to access the sound system.
	/// You can obtain an instance of this class by accessing the 
	/// Mixer property of the main Sdl object.
	/// </summary>
	/// <remarks>
	/// Before instantiating an instance of Movie,
	/// you must call Mxier.Close() to turn off the default mixer.
	/// If you do not do this, any movie will play very slowly. 
	/// Smpeg uses a custom mixer for audio playback. 
	/// </remarks>
	public sealed class Mixer
	{
		static private SdlMixer.ChannelFinishedDelegate ChannelFinishedDelegate;
		static private SdlMixer.MusicFinishedDelegate MusicFinishedDelegate;
		static private bool disposed = false;
		static private int DEFAULT_CHUNK_SIZE = 1024;
		
		static private byte distance;

		static Mixer instance = new Mixer();

		static Mixer()
		{
		}

		Mixer()
		{
			Initialize();	
		}

		/// <summary>
		/// Allows an Object to attempt to free resources 
		/// and perform other cleanup operations before the Object 
		/// is reclaimed by garbage collection.
		/// </summary>
		~Mixer() 
		{
			Dispose(false);
		}

		/// <summary>
		/// Closes and destroys this object
		/// </summary>
		public static void Dispose() 
		{
			Dispose(true);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="disposing"></param>
		public static void Dispose(bool disposing)
		{
			if (!disposed)
			{
				if (disposing)
				{
				}
				SdlMixer.Mix_CloseAudio();
				disposed = true;
			}
		}

		/// <summary>
		/// Closes and destroys this object
		/// </summary>
		public static void Close() 
		{
			Dispose();
		}

		/// <summary>
		/// 
		/// </summary>
		public static void Initialize()
		{
			if ((Sdl.SDL_WasInit(Sdl.SDL_INIT_AUDIO) & Sdl.SDL_INIT_AUDIO) 
				!= (int) SdlFlag.TrueValue)
			{
				if (Sdl.SDL_Init(Sdl.SDL_INIT_AUDIO) != (int) SdlFlag.Success)
				{
					throw SdlException.Generate();
				}	
			}
			Mixer.PrivateOpen();
		}

		/// <summary>
		/// Re-opens the sound system with default values.  
		/// You do not have to call this method
		/// in order to start using the Mixer object.
		/// </summary>
		public static void Open() 
		{
			Close();
			PrivateOpen();
		}
		/// <summary>
		/// Re-opens the sound-system. You do not have to call this method
		/// in order to start using the Mixer object.
		/// </summary>
		/// <param name="frequency">The frequency to mix at</param>
		/// <param name="format">The audio format to use</param>
		/// <param name="channels">
		/// The number of channels to allocate.  
		/// You will not be able to mix more than this number of samples.
		/// </param>
		/// <param name="chunkSize">The chunk size for samples</param>
		public static void Open(int frequency, AudioFormat format, int channels, int chunkSize) 
		{
			Close();
			PrivateOpen(frequency, format, channels, chunkSize);
		}

		private static void PrivateOpen() 
		{
			SdlMixer.Mix_OpenAudio(SdlMixer.MIX_DEFAULT_FREQUENCY, 
				unchecked((short)AudioFormat.Default), 
				(int) SoundChannel.Stereo, 
				DEFAULT_CHUNK_SIZE);
		}
		private static void PrivateOpen(
			int frequency, AudioFormat format, int channels, int chunksize) 
		{
			SdlMixer.Mix_OpenAudio(frequency, (short)format, channels, chunksize);
		}

		

		/// <summary>
		/// Loads a .wav file into memory
		/// </summary>
		/// <param name="file">The filename to load</param>
		/// <returns>A new Sample object</returns>
		public static Sample LoadWav(string file) 
		{
			IntPtr p = SdlMixer.Mix_LoadWAV_RW(Sdl.SDL_RWFromFile(file, "rb"), 1);
			if (p == IntPtr.Zero)
			{
				throw SdlException.Generate();
			}
			return new Sample(p);
		}
		/// <summary>
		/// Loads a .wav file from a byte array
		/// </summary>
		/// <param name="data">The data to load</param>
		/// <returns>A new Sample object</returns>
		public static Sample LoadWav(byte[] data) 
		{
			IntPtr p = SdlMixer.Mix_LoadWAV_RW(Sdl.SDL_RWFromMem(data, data.Length), 1);
			if (p == IntPtr.Zero)
			{
				throw SdlException.Generate();
			}
			return new Sample(p);
		}

		/// <summary>
		/// Changes the number of channels allocated for mixing
		/// </summary>
		/// <param name="num">The number of channels to allocate</param>
		/// <returns>The number of channels allocated</returns>
		public static int AllocateChannels(int num) 
		{
			return SdlMixer.Mix_AllocateChannels(num);
		}
		/// <summary>
		/// Sets the volume for all channels
		/// </summary>
		/// <param name="volume">A new volume value, between 0 and 128 inclusive</param>
		/// <returns>New average channel volume</returns>
		public static int SetAllChannelsVolume(int volume) 
		{
			return SdlMixer.Mix_Volume(-1, volume);
		}
		/// <summary>
		/// Sets the volume for a channel
		/// </summary>
		/// <param name="channel">Channel number</param>
		/// <param name="volume">A new volume value, between 0 and 128 inclusive</param>
		/// <returns>New channel volume</returns>
		public static int SetChannelVolume(int channel, int volume) 
		{
			return SdlMixer.Mix_Volume(channel, volume);
		}

		/// <summary>
		/// Plays a sample once using the first available channel
		/// </summary>
		/// <param name="sample">The sample to play</param>
		/// <returns>The channel used to play the sample</returns>
		public static int PlaySample(Sample sample) 
		{
			int ret = SdlMixer.Mix_PlayChannelTimed(-1, sample.GetHandle(), 0, -1);
			if (ret == -1)
			{
				throw SdlException.Generate();
			}
			return ret;
		}
		/// <summary>
		/// Plays a sample the specified number of times using the first available channel
		/// </summary>
		/// <param name="sample">The sample to play</param>
		/// <param name="loops">The number of loops.  
		/// Specify 1 to have the sample play twice</param>
		/// <returns>The channel used to play the sample</returns>
		public static int PlaySample(Sample sample, int loops) 
		{
			int ret = SdlMixer.Mix_PlayChannelTimed(-1, sample.GetHandle(), loops, (int) SdlFlag.PlayForever);
			if (ret == (int) SdlFlag.Error)
			{
				throw SdlException.Generate();
			}
			return ret;
		}
		/// <summary>
		/// Plays a sample the specified number of times on a specific channel
		/// </summary>
		/// <param name="channel">The channel to play the sample on</param>
		/// <param name="sample">The sample to play</param>
		/// <param name="loops">
		/// The number of loops.  Specify 1 to have the sample play twice
		/// </param>
		/// <returns>The channel used to play the sample</returns>
		public static int PlaySample(int channel, Sample sample, int loops) 
		{
			int ret = SdlMixer.Mix_PlayChannelTimed(channel, sample.GetHandle(), loops, (int) SdlFlag.PlayForever);
			if (ret == (int) SdlFlag.Error)
			{
				throw SdlException.Generate();
			}
			return ret;
		}
		/// <summary>
		/// Plays a sample once using the first available channel, 
		/// stopping after the specified number of ms
		/// </summary>
		/// <param name="sample">The sample to play</param>
		/// <param name="ticks">The time limit in milliseconds</param>
		/// <returns>The channel used to play the sample</returns>
		public static int PlaySampleTimed(Sample sample, int ticks) 
		{
			int ret = SdlMixer.Mix_PlayChannelTimed(-1, sample.GetHandle(), 0, ticks);
			if (ret == (int) SdlFlag.Error)
			{
				throw SdlException.Generate();
			}
			return ret;
		}
		/// <summary>
		/// Plays a sample the specified number of times using 
		/// the first available channel, stopping after the specified number of ms
		/// </summary>
		/// <param name="sample">The sample to play</param>
		/// <param name="loops">The number of loops.  
		/// Specify 1 to have the sample play twice</param>
		/// <param name="ticks">The time limit in milliseconds</param>
		/// <returns>The channel used to play the sample</returns>
		public static int PlaySampleTimed(Sample sample, int loops, int ticks) 
		{
			int ret = SdlMixer.Mix_PlayChannelTimed(-1, sample.GetHandle(), loops, ticks);
			if (ret == (int) SdlFlag.Error)
			{
				throw SdlException.Generate();
			}
			return ret;
		}
		/// <summary>
		/// Plays a sample the specified number of times on a 
		/// specific channel, stopping after the specified number of ms
		/// </summary>
		/// <param name="channel">The channel to play the sample on</param>
		/// <param name="sample">The sample to play</param>
		/// <param name="loops">
		/// The number of loops.  Specify 1 to have the sample play twice
		/// </param>
		/// <param name="ticks">The time limit in milliseconds</param>
		/// <returns>The channel used to play the sample</returns>
		public static int PlaySampleTimed(int channel, Sample sample, int loops, int ticks) 
		{
			int ret = SdlMixer.Mix_PlayChannelTimed(channel, sample.GetHandle(), loops, ticks);
			if (ret == (int) SdlFlag.Error)
			{
				throw SdlException.Generate();
			}
			return ret;
		}

		/// <summary>
		/// Fades in a sample once using the first available channel
		/// </summary>
		/// <param name="sample">The sample to play</param>
		/// <param name="ms">The number of milliseconds to fade in for</param>
		/// <returns>The channel used to play the sample</returns>
		public static int FadeInSample(Sample sample, int ms) 
		{
			int ret = SdlMixer.Mix_FadeInChannelTimed(-1, sample.GetHandle(), 0, ms, -1);
			if (ret == (int) SdlFlag.Error)
			{
				throw SdlException.Generate();
			}
			return ret;
		}
		/// <summary>
		/// Fades in a sample the specified number of times using 
		/// the first available channel
		/// </summary>
		/// <param name="sample">The sample to play</param>
		/// <param name="ms">
		/// The number of milliseconds to fade in for
		/// </param>
		/// <param name="loops">The number of loops.  
		/// Specify 1 to have the sample play twice</param>
		/// <returns>The channel used to play the sample</returns>
		public static int FadeInSample(Sample sample, int ms, int loops) 
		{
			int ret = SdlMixer.Mix_FadeInChannelTimed(-1, sample.GetHandle(), loops, ms, -1);
			if (ret == (int) SdlFlag.Error)
			{
				throw SdlException.Generate();
			}
			return ret;
		}
		/// <summary>
		/// Fades in a sample the specified number of times on a 
		/// specific channel
		/// </summary>
		/// <param name="channel">The channel to play the sample on</param>
		/// <param name="sample">The sample to play</param>
		/// <param name="ms">The number of milliseconds to fade in for</param>
		/// <param name="loops">
		/// The number of loops.  
		/// Specify 1 to have the sample play twice
		/// </param>
		/// <returns>The channel used to play the sample</returns>
		public static int FadeInSample(int channel, Sample sample, int ms, int loops) 
		{
			int ret = SdlMixer.Mix_FadeInChannelTimed(channel, sample.GetHandle(), loops, ms, -1);
			if (ret == (int) SdlFlag.Error)
			{
				throw SdlException.Generate();
			}
			return ret;
		}
		/// <summary>
		/// Fades in a sample once using the first available channel, 
		/// stopping after the specified number of ms
		/// </summary>
		/// <param name="sample">The sample to play</param>
		/// <param name="ms">The number of milliseconds to fade in for</param>
		/// <param name="ticks">The time limit in milliseconds</param>
		/// <returns>The channel used to play the sample</returns>
		public static int FadeInSampleTimed(Sample sample, int ms, int ticks) 
		{
			int ret = SdlMixer.Mix_FadeInChannelTimed(-1, sample.GetHandle(), 0, ms, ticks);
			if (ret == (int) SdlFlag.Error)
			{
				throw SdlException.Generate();
			}
			return ret;
		}
		/// <summary>
		/// Fades in a sample the specified number of times using 
		/// the first available channel, stopping after the 
		/// specified number of ms
		/// </summary>
		/// <param name="sample">The sample to play</param>
		/// <param name="ms">The number of milliseconds to fade in for</param>
		/// <param name="loops">The number of loops.  
		/// Specify 1 to have the sample play twice</param>
		/// <param name="ticks">The time limit in milliseconds</param>
		/// <returns>The channel used to play the sample</returns>
		public static int FadeInSampleTimed(Sample sample, int ms, int loops, int ticks) 
		{
			int ret = SdlMixer.Mix_FadeInChannelTimed(-1, sample.GetHandle(), loops, ms, ticks);
			if (ret == (int) SdlFlag.Error)
			{
				throw SdlException.Generate();
			}
			return ret;
		}
		/// <summary>
		/// Fades in a sample the specified number of times on 
		/// a specific channel, stopping after the specified number of ms
		/// </summary>
		/// <param name="channel">The channel to play the sample on
		/// </param>
		/// <param name="sample">The sample to play</param>
		/// <param name="ms">The number of milliseconds to fade in for
		/// </param>
		/// <param name="loops">The number of loops.  
		/// Specify 1 to have the sample play twice</param>
		/// <param name="ticks">The time limit in milliseconds</param>
		/// <returns>The channel used to play the sample</returns>
		public static int FadeInSampleTimed(int channel, Sample sample, int ms, int loops, int ticks) 
		{
			int ret = SdlMixer.Mix_FadeInChannelTimed(channel, sample.GetHandle(), loops, ms, ticks);
			if (ret == (int) SdlFlag.Error)
			{
				throw SdlException.Generate();
			}
			return ret;
		}

		/// <summary>
		/// Pauses playing on all channels
		/// </summary>
		public static void Pause() 
		{
			SdlMixer.Mix_Pause(-1);
		}
		/// <summary>
		/// Pauses playing on a specific channel
		/// </summary>
		/// <param name="channel">The channel to pause</param>
		public static void PauseChannel(int channel) 
		{
			SdlMixer.Mix_Pause(channel);
		}
		/// <summary>
		/// Resumes playing on all paused channels
		/// </summary>
		public static void Resume() 
		{
			SdlMixer.Mix_Resume(-1);
		}
		/// <summary>
		/// Resumes playing on a paused channel
		/// </summary>
		/// <param name="channel">The channel to resume</param>
		public static void ResumeChannel(int channel) 
		{
			SdlMixer.Mix_Resume(channel);
		}
		
		/// <summary>
		/// Stop playing on all channels
		/// </summary>
		public static void Halt() 
		{
			SdlMixer.Mix_HaltChannel(-1);
		}
		/// <summary>
		/// Stop playing on a specific channel
		/// </summary>
		/// <param name="channel">The channel to stop</param>
		public static void HaltChannel(int channel) 
		{
			SdlMixer.Mix_HaltChannel(channel);
		}

		/// <summary>
		/// Stop playing on all channels after a specified time interval
		/// </summary>
		/// <param name="ms">
		/// The number of milliseconds to stop playing after
		/// </param>
		public static void Expire(int ms) 
		{
			SdlMixer.Mix_ExpireChannel(-1, ms);
		}
		/// <summary>
		/// Stop playing a channel after a specified time interval
		/// </summary>
		/// <param name="channel">The channel to stop</param>
		/// <param name="ms">
		/// The number of milliseconds to stop playing after
		/// </param>
		public static void ExpireChannel(int channel, int ms) 
		{
			SdlMixer.Mix_ExpireChannel(channel, ms);
		}

		/// <summary>
		/// Fades out all channels
		/// </summary>
		/// <param name="ms">
		/// The number of milliseconds to fade out for
		/// </param>
		/// <returns>The number of channels fading out</returns>
		public static int FadeOut(int ms) 
		{
			return SdlMixer.Mix_FadeOutChannel(-1, ms);
		}
		/// <summary>
		/// Fades out a channel.
		/// </summary>
		/// <param name="channel">The channel to fade out</param>
		/// <param name="ms">
		/// The number of milliseconds to fade out for
		/// </param>
		/// <returns>The number of channels fading out</returns>
		public static int FadeOutChannel(int channel, int ms) 
		{
			return SdlMixer.Mix_FadeOutChannel(channel, ms);
		}

		/// <summary>
		/// Returns the number of currently playing channels
		/// </summary>
		/// <returns>The number of channels playing</returns>
		public static int NumChannelsPlaying() 
		{
			return SdlMixer.Mix_Playing(-1);
		}
		/// <summary>
		/// Returns a flag indicating whether or not a channel is playing
		/// </summary>
		/// <param name="channel">The channel to query</param>
		/// <returns>True if the channel is playing, otherwise False</returns>
		public static bool IsChannelPlaying(int channel) 
		{
			return (SdlMixer.Mix_Playing(channel) != 0);
		}
		/// <summary>
		/// Returns the number of paused channels
		/// </summary>
		/// <remarks>
		/// Number of channels paused.
		/// </remarks>
		/// <returns>The number of channels paused</returns>
		public static int NumChannelsPaused() 
		{
			return SdlMixer.Mix_Paused(-1);
		}
		/// <summary>
		/// Returns a flag indicating whether or not a channel is paused
		/// </summary>
		/// <param name="channel">The channel to query</param>
		/// <returns>True if the channel is paused, otherwise False</returns>
		public static bool IsChannelPaused(int channel) 
		{
			return (SdlMixer.Mix_Paused(channel) != 0);
		}
		/// <summary>
		/// Returns the current fading status of a channel
		/// </summary>
		/// <param name="channel">The channel to query</param>
		/// <returns>The current fading status of the channel</returns>
		public static FadingStatus ChannelFadingStatus(int channel) 
		{
			return (FadingStatus)SdlMixer.Mix_FadingChannel(channel);
		}

		/// <summary>
		/// Sets the panning (stereo attenuation) for all channels
		/// </summary>
		/// <param name="left">
		/// A left speaker value from 0-255 inclusive
		/// </param>
		/// <param name="right">
		/// A right speaker value from 0-255 inclusive
		/// </param>
		public static void SetPanning(int left, int right) 
		{
			if (SdlMixer.Mix_SetPanning(-1, (byte)left, (byte)right) == 0)
			{
				throw SdlException.Generate();
			}
		}
		/// <summary>
		/// Sets the panning (stereo attenuation) for a specific channel
		/// </summary>
		/// <param name="channel">The channel to set panning for</param>
		/// <param name="left">A left speaker value from 0-255 inclusive</param>
		/// <param name="right">A right speaker value from 0-255 inclusive</param>
		public static void SetPanningChannel(int channel, int left, int right) 
		{
			if (SdlMixer.Mix_SetPanning(channel, (byte)left, (byte)right) == 0)
			{
				throw SdlException.Generate();
			}
		}
		/// <summary>
		/// Sets the distance (attenuate sounds based on distance 
		/// from listener) for all channels
		/// </summary>
		public static byte Distance 
		{
			set
			{
				if (SdlMixer.Mix_SetDistance(-1, value) == 0)
				{
					throw SdlException.Generate();
				}
				distance = value;
			}
			get
			{
				return distance;
			}
		}
		/// <summary>
		/// Sets the distance (attenuate sounds based on distance 
		/// from listener) for a specific channel
		/// </summary>
		/// <param name="channel">Channel to set distance for</param>
		/// <param name="distance">Distance value from 0-255 inclusive</param>
		public static void SetDistanceChannel(int channel, int distance) 
		{
			if (SdlMixer.Mix_SetDistance(channel, (byte)distance) == 0)
			{
				throw SdlException.Generate();
			}
		}
		/// <summary>
		/// Sets the "position" of a sound (approximate '3D' audio) 
		/// for all channels
		/// </summary>
		/// <param name="angle">The angle of the sound, between 0 and 359,
		///  0 = directly in front</param>
		/// <param name="distance">
		/// The distance of the sound from 0-255 inclusive
		/// </param>
		public static void SetPosition(int angle, int distance) 
		{
			if (SdlMixer.Mix_SetPosition(-1, (short)angle, (byte)distance) == 0)
			{
				throw SdlException.Generate();
			}
		}
		/// <summary>
		/// Sets the "position" of a sound (approximate '3D' audio)
		///  for a specific channel
		/// </summary>
		/// <param name="channel">The channel to set position for</param>
		/// <param name="angle">The angle of the sound, between 0 and 359,
		///  0 = directly in front</param>
		/// <param name="distance">The distance of the sound from 0-255
		///  inclusive</param>
		public static void SetPositionChannel(int channel, int angle, int distance) 
		{
			if (SdlMixer.Mix_SetPosition(channel, (short)angle, (byte)distance) == 0)
			{
				throw SdlException.Generate();
			}
		}
		/// <summary>
		/// Flips the left and right stereo for all channels
		/// </summary>
		/// <param name="flip">True to flip, False to reset to normal</param>
		public static void ReverseStereo(bool flip) 
		{
			if (SdlMixer.Mix_SetReverseStereo(-1, flip?1:0) == 0)
			{
				throw SdlException.Generate();
			}
		}
		/// <summary>
		/// Flips the left and right stereo for a specific channel
		/// </summary>
		/// <param name="channel">The channel to flip</param>
		/// <param name="flip">True to flip, False to reset to normal</param>
		public static void ReverseStereoChannel(int channel, bool flip) 
		{
			if (SdlMixer.Mix_SetReverseStereo(channel, flip?1:0) == 0)
			{
				throw SdlException.Generate();
			}
		}

		/// <summary>
		/// Loads a music sample from disk.
		/// By default, Sdl_mixer only supports the Ogg Vorbis format,
		///  see http://www.vorbis.com/.
		/// It may be possible to compile in support for other formats
		///  such as MOD and and MP3.
		/// </summary>
		/// <param name="file">The filename to load</param>
		/// <returns>A new Music object</returns>
		public static Music LoadMusic(string file) 
		{
			IntPtr musicPtr = SdlMixer.Mix_LoadMUS(file);
			if (musicPtr == IntPtr.Zero)
			{
				throw SdlException.Generate();
			}
			return new Music(musicPtr);
		}
		/// <summary>
		/// Plays a music sample
		/// </summary>
		/// <param name="music">The sample to play</param>
		/// <param name="numberOfTimes">The number of times to play. 
		/// Specify 1 to play a single time, -1 to loop forever.</param>
		public static void PlayMusic(Music music, int numberOfTimes) 
		{
			if (SdlMixer.Mix_PlayMusic(music.GetHandle(), numberOfTimes) != 0)
			{
				throw SdlException.Generate();
			}
		}
		/// <summary>
		/// Plays a music sample and fades it in
		/// </summary>
		/// <param name="music">The sample to play</param>
		/// <param name="numberOfTimes">The number of times to play. 
		/// Specify 1 to play a single time, -1 to loop forever.</param>
		/// <param name="milliseconds">The number of milliseconds to fade in for</param>
		public static void FadeInMusic(Music music, int numberOfTimes, int milliseconds) 
		{
			if (SdlMixer.Mix_FadeInMusic(music.GetHandle(), numberOfTimes, milliseconds) != 0)
			{
				throw SdlException.Generate();
			}
		}
		/// <summary>
		/// Plays a music sample, starting from a specific 
		/// position and fades it in
		/// </summary>
		/// <param name="music">The sample to play</param>
		/// <param name="numberOfTimes">The number of times to play.
		///  Specify 1 to play a single time, -1 to loop forever.</param>
		/// <param name="milliseconds">The number of milliseconds to fade in for
		/// </param>
		/// <param name="position">A format-defined position value. 
		/// For Ogg Vorbis, this is the number of seconds from the
		///  beginning of the song</param>
		public static void FadeInMusicPosition(Music music, int numberOfTimes, 
			int milliseconds, double position) 
		{
			if (SdlMixer.Mix_FadeInMusicPos(music.GetHandle(), 
				numberOfTimes, milliseconds, position) != 0)
			{
				throw SdlException.Generate();
			}
		}
		/// <summary>
		/// Sets the music volume
		/// </summary>
		/// <param name="volume">The new volume value, 
		/// between 0 and 128 inclusive</param>
		/// <returns>The previous volume setting</returns>
		public static int SetMusicVolume(int volume) 
		{
			return SdlMixer.Mix_VolumeMusic(volume);
		}
		/// <summary>
		/// Pauses the music playing
		/// </summary>
		public static void PauseMusic() 
		{
			SdlMixer.Mix_PauseMusic();
		}
		/// <summary>
		/// Resumes paused music
		/// </summary>
		public static void ResumeMusic() 
		{
			SdlMixer.Mix_ResumeMusic();
		}
		/// <summary>
		/// Resets the music position to the beginning of the sample
		/// </summary>
		public static void RewindMusic() 
		{
			SdlMixer.Mix_RewindMusic();
		}
		/// <summary>
		/// Sets the music position to a format-defined value.
		/// For Ogg Vorbis, this is the number of seconds 
		/// from the beginning of the song
		/// </summary>
		/// <param name="position"></param>
		public static void MusicPosition(double position) 
		{
			if (SdlMixer.Mix_SetMusicPosition(position) != 0)
			{
				throw SdlException.Generate();
			}
		}
		/// <summary>
		/// Stops playing music
		/// </summary>
		public static void HaltMusic() 
		{
			SdlMixer.Mix_HaltMusic();
		}
		/// <summary>
		/// Fades out music
		/// </summary>
		/// <param name="ms">
		/// The number of milliseconds to fade out for
		/// </param>
		public static void FadeOutMusic(int ms) 
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
		public static bool PlayingMusic() 
		{
			return (SdlMixer.Mix_PlayingMusic() != 0);
		}
		/// <summary>
		/// Returns a flag indicating whether or not music is paused
		/// </summary>
		/// <returns>True if music is paused, otherwise False</returns>
		public static bool PausedMusic() 
		{
			return (SdlMixer.Mix_PausedMusic() != 0);
		}
		/// <summary>
		/// Returns a flag indicating whether or not music is fading
		/// </summary>
		/// <returns>True if music is fading in or out, 
		/// otherwise False
		/// </returns>
		public static bool FadingMusic() 
		{
			return (SdlMixer.Mix_FadingMusic() != 0);
		}

		/// <summary>
		/// For performance reasons, you must call this method
		///  to enable the Events.ChannelFinished and 
		///  Events.MusicFinished events
		/// </summary>
		public static void EnableMusicCallbacks() 
		{
			Mixer.ChannelFinishedDelegate = new SdlMixer.ChannelFinishedDelegate(Mixer.ChannelFinished);
			Mixer.MusicFinishedDelegate = new SdlMixer.MusicFinishedDelegate(Mixer.MusicFinished);
			SdlMixer.Mix_ChannelFinished(ChannelFinishedDelegate);
			SdlMixer.Mix_HookMusicFinished(MusicFinishedDelegate);
		}

		private static void ChannelFinished(int channel) 
		{
			Events.NotifyChannelFinished(channel);
		}
		private static void MusicFinished() 
		{
			Events.NotifyMusicFinished();
		}
	}

}
