/*
 * $RCSfile$
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

namespace SDLDotNet.Mixer {
	/// <summary>
	/// Specifies an audio format to mix audio in
	/// </summary>
	public enum AudioFormat {
		/// <summary>
		/// Unsigned 8-bit
		/// </summary>
		U8 = 0x0008,
		/// <summary>
		/// Signed 8-bit
		/// </summary>
		S8 = 0x8008,
		/// <summary>
		/// Unsigned 16-bit, little-endian
		/// </summary>
		U16LSB = 0x0010,
		/// <summary>
		/// Signed 16-bit, little-endian
		/// </summary>
		S16LSB = 0x8010,
		/// <summary>
		/// Unsigned 16-bit, big-endian
		/// </summary>
		U16MSB = 0x1010,
		/// <summary>
		/// Signed 16-bit, big-endian
		/// </summary>
		S16MSB = 0x9010,
		/// <summary>
		/// Default, equal to S16LSB
		/// </summary>
		Default = 0x8010
	}

	/// <summary>
	/// Indicates the current fading status of a sample
	/// </summary>
	public enum FadingStatus {
		/// <summary>
		/// Sample is not fading
		/// </summary>
		NoFading,
		/// <summary>
		/// Sample is fading out
		/// </summary>
		FadingOut,
		/// <summary>
		/// Sample is fading in
		/// </summary>
		FadingIn
	}

	/// <summary>
	/// Provides methods to access the sound system.
	/// You can obtain an instance of this class by accessing the Mixer property of the main SDL object.
	/// </summary>
	public class SDLMixer {
		private Natives.ChannelFinishedDelegate _channelfin;
		private Natives.MusicFinishedDelegate _musicfin;
		private Events _events;

		internal SDLMixer(Events evs) {
			if (Natives.SDL_InitSubSystem((int)Natives.Init.Audio) != 0)
				throw SDLException.Generate();
			_events = evs;
			_channelfin = new Natives.ChannelFinishedDelegate(this.ChannelFinished);
			_musicfin = new Natives.MusicFinishedDelegate(this.MusicFinished);
			PrivateOpen();
		}

		/// <summary>
		/// Allows an Object to attempt to free resources 
		/// and perform other cleanup operations before the Object 
		/// is reclaimed by garbage collection.
		/// </summary>
		~SDLMixer() 
		{
			PrivateClose();
		}

		/// <summary>
		/// Re-opens the sound system with default values.  You do not have to call this method
		/// in order to start using the Mixer object.
		/// </summary>
		public void Open() {
			PrivateClose();
			PrivateOpen();
		}
		/// <summary>
		/// Re-opens the sound-system. You do not have to call this method
		/// in order to start using the Mixer object.
		/// </summary>
		/// <param name="frequency">The frequency to mix at</param>
		/// <param name="format">The audio format to use</param>
		/// <param name="channels">The number of channels to allocate.  You will not be able to mix more than this number of samples.</param>
		/// <param name="chunksize">The chunk size for samples</param>
		public void Open(int frequency, AudioFormat format, int channels, int chunksize) {
			PrivateClose();
			PrivateOpen(frequency, format, channels, chunksize);
		}

		private void PrivateOpen() {
			Natives.Mix_OpenAudio(22050, (ushort)AudioFormat.Default, 2, 1024);
		}
		private void PrivateOpen(int frequency, AudioFormat format, int channels, int chunksize) {
			Natives.Mix_OpenAudio(frequency, (ushort)format, channels, chunksize);
		}
		private void PrivateClose() {
			Natives.Mix_CloseAudio();
		}

		/// <summary>
		/// Loads a .wav file into memory
		/// </summary>
		/// <param name="file">The filename to load</param>
		/// <returns>A new Sample object</returns>
		public Sample LoadWAV(string file) {
			IntPtr p = Natives.Mix_LoadWAV_RW(Natives.SDL_RWFromFile(file, "rb"), 1);
			if (p == IntPtr.Zero)
				throw SDLException.Generate();
			return new Sample(p);
		}
		/// <summary>
		/// Loads a .wav file from a byte array
		/// </summary>
		/// <param name="data">The data to load</param>
		/// <returns>A new Sample object</returns>
		public Sample LoadWAV(byte[] data) {
			IntPtr p = Natives.Mix_LoadWAV_RW(Natives.SDL_RWFromMem(data, data.Length), 1);
			if (p == IntPtr.Zero)
				throw SDLException.Generate();
			return new Sample(p);
		}

		/// <summary>
		/// Changes the number of channels allocated for mixing
		/// </summary>
		/// <param name="num">The number of channels to allocate</param>
		/// <returns>The number of channels allocated</returns>
		public int AllocateChannels(int num) {
			return Natives.Mix_AllocateChannels(num);
		}
		/// <summary>
		/// Sets the volume for all channels
		/// </summary>
		/// <param name="volume">A new volume value, between 0 and 128 inclusive</param>
		/// <returns>New average channel volume</returns>
		public int SetAllChannelsVolume(int volume) {
			return Natives.Mix_Volume(-1, volume);
		}
		/// <summary>
		/// Sets the volume for a channel
		/// </summary>
		/// <param name="channel">Channel number</param>
		/// <param name="volume">A new volume value, between 0 and 128 inclusive</param>
		/// <returns>New channel volume</returns>
		public int SetChannelVolume(int channel, int volume) {
			return Natives.Mix_Volume(channel, volume);
		}

		/// <summary>
		/// Plays a sample once using the first available channel
		/// </summary>
		/// <param name="sample">The sample to play</param>
		/// <returns>The channel used to play the sample</returns>
		public int PlaySample(Sample sample) {
			int ret = Natives.Mix_PlayChannelTimed(-1, sample.GetHandle(), 0, -1);
			if (ret == -1)
				throw SDLException.Generate();
			return ret;
		}
		/// <summary>
		/// Plays a sample the specified number of times using the first available channel
		/// </summary>
		/// <param name="sample">The sample to play</param>
		/// <param name="loops">The number of loops.  Specify 1 to have the sample play twice</param>
		/// <returns>The channel used to play the sample</returns>
		public int PlaySample(Sample sample, int loops) {
			int ret = Natives.Mix_PlayChannelTimed(-1, sample.GetHandle(), loops, -1);
			if (ret == -1)
				throw SDLException.Generate();
			return ret;
		}
		/// <summary>
		/// Plays a sample the specified number of times on a specific channel
		/// </summary>
		/// <param name="channel">The channel to play the sample on</param>
		/// <param name="sample">The sample to play</param>
		/// <param name="loops">The number of loops.  Specify 1 to have the sample play twice</param>
		/// <returns>The channel used to play the sample</returns>
		public int PlaySample(int channel, Sample sample, int loops) {
			int ret = Natives.Mix_PlayChannelTimed(channel, sample.GetHandle(), loops, -1);
			if (ret == -1)
				throw SDLException.Generate();
			return ret;
		}
		/// <summary>
		/// Plays a sample once using the first available channel, stopping after the specified number of ms
		/// </summary>
		/// <param name="sample">The sample to play</param>
		/// <param name="ticks">The time limit in milliseconds</param>
		/// <returns>The channel used to play the sample</returns>
		public int PlaySampleTimed(Sample sample, int ticks) {
			int ret = Natives.Mix_PlayChannelTimed(-1, sample.GetHandle(), 0, ticks);
			if (ret == -1)
				throw SDLException.Generate();
			return ret;
		}
		/// <summary>
		/// Plays a sample the specified number of times using the first available channel, stopping after the specified number of ms
		/// </summary>
		/// <param name="sample">The sample to play</param>
		/// <param name="loops">The number of loops.  Specify 1 to have the sample play twice</param>
		/// <param name="ticks">The time limit in milliseconds</param>
		/// <returns>The channel used to play the sample</returns>
		public int PlaySampleTimed(Sample sample, int loops, int ticks) {
			int ret = Natives.Mix_PlayChannelTimed(-1, sample.GetHandle(), loops, ticks);
			if (ret == -1)
				throw SDLException.Generate();
			return ret;
		}
		/// <summary>
		/// Plays a sample the specified number of times on a specific channel, stopping after the specified number of ms
		/// </summary>
		/// <param name="channel">The channel to play the sample on</param>
		/// <param name="sample">The sample to play</param>
		/// <param name="loops">The number of loops.  Specify 1 to have the sample play twice</param>
		/// <param name="ticks">The time limit in milliseconds</param>
		/// <returns>The channel used to play the sample</returns>
		public int PlaySampleTimed(int channel, Sample sample, int loops, int ticks) {
			int ret = Natives.Mix_PlayChannelTimed(channel, sample.GetHandle(), loops, ticks);
			if (ret == -1)
				throw SDLException.Generate();
			return ret;
		}

		/// <summary>
		/// Fades in a sample once using the first available channel
		/// </summary>
		/// <param name="sample">The sample to play</param>
		/// <param name="ms">The number of milliseconds to fade in for</param>
		/// <returns>The channel used to play the sample</returns>
		public int FadeInSample(Sample sample, int ms) {
			int ret = Natives.Mix_FadeInChannelTimed(-1, sample.GetHandle(), 0, ms, -1);
			if (ret == -1)
				throw SDLException.Generate();
			return ret;
		}
		/// <summary>
		/// Fades in a sample the specified number of times using the first available channel
		/// </summary>
		/// <param name="sample">The sample to play</param>
		/// <param name="ms">The number of milliseconds to fade in for</param>
		/// <param name="loops">The number of loops.  Specify 1 to have the sample play twice</param>
		/// <returns>The channel used to play the sample</returns>
		public int FadeInSample(Sample sample, int ms, int loops) {
			int ret = Natives.Mix_FadeInChannelTimed(-1, sample.GetHandle(), loops, ms, -1);
			if (ret == -1)
				throw SDLException.Generate();
			return ret;
		}
		/// <summary>
		/// Fades in a sample the specified number of times on a specific channel
		/// </summary>
		/// <param name="channel">The channel to play the sample on</param>
		/// <param name="sample">The sample to play</param>
		/// <param name="ms">The number of milliseconds to fade in for</param>
		/// <param name="loops">The number of loops.  Specify 1 to have the sample play twice</param>
		/// <returns>The channel used to play the sample</returns>
		public int FadeInSample(int channel, Sample sample, int ms, int loops) {
			int ret = Natives.Mix_FadeInChannelTimed(channel, sample.GetHandle(), loops, ms, -1);
			if (ret == -1)
				throw SDLException.Generate();
			return ret;
		}
		/// <summary>
		/// Fades in a sample once using the first available channel, stopping after the specified number of ms
		/// </summary>
		/// <param name="sample">The sample to play</param>
		/// <param name="ms">The number of milliseconds to fade in for</param>
		/// <param name="ticks">The time limit in milliseconds</param>
		/// <returns>The channel used to play the sample</returns>
		public int FadeInSampleTimed(Sample sample, int ms, int ticks) {
			int ret = Natives.Mix_FadeInChannelTimed(-1, sample.GetHandle(), 0, ms, ticks);
			if (ret == -1)
				throw SDLException.Generate();
			return ret;
		}
		/// <summary>
		/// Fades in a sample the specified number of times using the first available channel, stopping after the specified number of ms
		/// </summary>
		/// <param name="sample">The sample to play</param>
		/// <param name="ms">The number of milliseconds to fade in for</param>
		/// <param name="loops">The number of loops.  Specify 1 to have the sample play twice</param>
		/// <param name="ticks">The time limit in milliseconds</param>
		/// <returns>The channel used to play the sample</returns>
		public int FadeInSampleTimed(Sample sample, int ms, int loops, int ticks) {
			int ret = Natives.Mix_FadeInChannelTimed(-1, sample.GetHandle(), loops, ms, ticks);
			if (ret == -1)
				throw SDLException.Generate();
			return ret;
		}
		/// <summary>
		/// Fades in a sample the specified number of times on a specific channel, stopping after the specified number of ms
		/// </summary>
		/// <param name="channel">The channel to play the sample on</param>
		/// <param name="sample">The sample to play</param>
		/// <param name="ms">The number of milliseconds to fade in for</param>
		/// <param name="loops">The number of loops.  Specify 1 to have the sample play twice</param>
		/// <param name="ticks">The time limit in milliseconds</param>
		/// <returns>The channel used to play the sample</returns>
		public int FadeInSampleTimed(int channel, Sample sample, int ms, int loops, int ticks) {
			int ret = Natives.Mix_FadeInChannelTimed(channel, sample.GetHandle(), loops, ms, ticks);
			if (ret == -1)
				throw SDLException.Generate();
			return ret;
		}

		/// <summary>
		/// Pauses playing on all channels
		/// </summary>
		public void Pause() {
			Natives.Mix_Pause(-1);
		}
		/// <summary>
		/// Pauses playing on a specific channel
		/// </summary>
		/// <param name="channel">The channel to pause</param>
		public void PauseChannel(int channel) {
			Natives.Mix_Pause(channel);
		}
		/// <summary>
		/// Resumes playing on all paused channels
		/// </summary>
		public void Resume() {
			Natives.Mix_Resume(-1);
		}
		/// <summary>
		/// Resumes playing on a paused channel
		/// </summary>
		/// <param name="channel">The channel to resume</param>
		public void ResumeChannel(int channel) {
			Natives.Mix_Resume(channel);
		}
		
		/// <summary>
		/// Stop playing on all channels
		/// </summary>
		public void Halt() {
			Natives.Mix_HaltChannel(-1);
		}
		/// <summary>
		/// Stop playing on a specific channel
		/// </summary>
		/// <param name="channel">The channel to stop</param>
		public void HaltChannel(int channel) {
			Natives.Mix_HaltChannel(channel);
		}

		/// <summary>
		/// Stop playing on all channels after a specified time interval
		/// </summary>
		/// <param name="ms">The number of milliseconds to stop playing after</param>
		public void Expire(int ms) {
			Natives.Mix_ExpireChannel(-1, ms);
		}
		/// <summary>
		/// Stop playing a channel after a specified time interval
		/// </summary>
		/// <param name="channel">The channel to stop</param>
		/// <param name="ms">The number of milliseconds to stop playing after</param>
		public void ExpireChannel(int channel, int ms) {
			Natives.Mix_ExpireChannel(channel, ms);
		}

		/// <summary>
		/// Fades out all channels
		/// </summary>
		/// <param name="ms">The number of milliseconds to fade out for</param>
		/// <returns>The number of channels fading out</returns>
		public int FadeOut(int ms) {
			return Natives.Mix_FadeOutChannel(-1, ms);
		}
		/// <summary>
		/// Fades out a channel.
		/// </summary>
		/// <param name="channel">The channel to fade out</param>
		/// <param name="ms">The number of milliseconds to fade out for</param>
		/// <returns>The number of channels fading out</returns>
		public int FadeOutChannel(int channel, int ms) {
			return Natives.Mix_FadeOutChannel(channel, ms);
		}

		/// <summary>
		/// Returns the number of currently playing channels
		/// </summary>
		/// <returns>The number of channels playing</returns>
		public int NumChannelsPlaying() {
			return Natives.Mix_Playing(-1);
		}
		/// <summary>
		/// Returns a flag indicating whether or not a channel is playing
		/// </summary>
		/// <param name="channel">The channel to query</param>
		/// <returns>True if the channel is playing, otherwise False</returns>
		public bool IsChannelPlaying(int channel) {
			return (Natives.Mix_Playing(channel) != 0);
		}
		/// <summary>
		/// Returns the number of paused channels
		/// </summary>
		/// <remarks>
		/// Number of channels paused.
		/// </remarks>
		/// <returns>The number of channels paused</returns>
		public int NumChannelsPaused() {
			return Natives.Mix_Paused(-1);
		}
		/// <summary>
		/// Returns a flag indicating whether or not a channel is paused
		/// </summary>
		/// <param name="channel">The channel to query</param>
		/// <returns>True if the channel is paused, otherwise False</returns>
		public bool IsChannelPaused(int channel) {
			return (Natives.Mix_Paused(channel) != 0);
		}
		/// <summary>
		/// Returns the current fading status of a channel
		/// </summary>
		/// <param name="channel">The channel to query</param>
		/// <returns>The current fading status of the channel</returns>
		public FadingStatus ChannelFadingStatus(int channel) {
			return (FadingStatus)Natives.Mix_FadingChannel(channel);
		}

		/// <summary>
		/// Sets the panning (stereo attenuation) for all channels
		/// </summary>
		/// <param name="left">A left speaker value from 0-255 inclusive</param>
		/// <param name="right">A right speaker value from 0-255 inclusive</param>
		public void SetPanning(int left, int right) {
			if (Natives.Mix_SetPanning(-1, (byte)left, (byte)right) == 0)
				throw SDLException.Generate();
		}
		/// <summary>
		/// Sets the panning (stereo attenuation) for a specific channel
		/// </summary>
		/// <param name="channel">The channel to set panning for</param>
		/// <param name="left">A left speaker value from 0-255 inclusive</param>
		/// <param name="right">A right speaker value from 0-255 inclusive</param>
		public void SetPanningChannel(int channel, int left, int right) {
			if (Natives.Mix_SetPanning(channel, (byte)left, (byte)right) == 0)
				throw SDLException.Generate();
		}
		/// <summary>
		/// Sets the distance (attenuate sounds based on distance from listener) for all channels
		/// </summary>
		/// <param name="distance">Distance value from 0-255 inclusive</param>
		public void SetDistance(int distance) {
			if (Natives.Mix_SetDistance(-1, (byte)distance) == 0)
				throw SDLException.Generate();
		}
		/// <summary>
		/// Sets the distance (attenuate sounds based on distance from listener) for a specific channel
		/// </summary>
		/// <param name="channel">Channel to set distance for</param>
		/// <param name="distance">Distance value from 0-255 inclusive</param>
		public void SetDistanceChannel(int channel, int distance) {
			if (Natives.Mix_SetDistance(channel, (byte)distance) == 0)
				throw SDLException.Generate();
		}
		/// <summary>
		/// Sets the "position" of a sound (approximate '3D' audio) for all channels
		/// </summary>
		/// <param name="angle">The angle of the sound, between 0 and 359, 0 = directly in front</param>
		/// <param name="distance">The distance of the sound from 0-255 inclusive</param>
		public void SetPosition(int angle, int distance) {
			if (Natives.Mix_SetPosition(-1, (short)angle, (byte)distance) == 0)
				throw SDLException.Generate();
		}
		/// <summary>
		/// Sets the "position" of a sound (approximate '3D' audio) for a specific channel
		/// </summary>
		/// <param name="channel">The channel to set position for</param>
		/// <param name="angle">The angle of the sound, between 0 and 359, 0 = directly in front</param>
		/// <param name="distance">The distance of the sound from 0-255 inclusive</param>
		public void SetPositionChannel(int channel, int angle, int distance) {
			if (Natives.Mix_SetPosition(channel, (short)angle, (byte)distance) == 0)
				throw SDLException.Generate();
		}
		/// <summary>
		/// Flips the left and right stereo for all channels
		/// </summary>
		/// <param name="flip">True to flip, False to reset to normal</param>
		public void SetReverseStereo(bool flip) {
			if (Natives.Mix_SetReverseStereo(-1, flip?1:0) == 0)
				throw SDLException.Generate();
		}
		/// <summary>
		/// Flips the left and right stereo for a specific channel
		/// </summary>
		/// <param name="channel">The channel to flip</param>
		/// <param name="flip">True to flip, False to reset to normal</param>
		public void SetReverseStereoChannel(int channel, bool flip) {
			if (Natives.Mix_SetReverseStereo(channel, flip?1:0) == 0)
				throw SDLException.Generate();
		}

		/// <summary>
		/// Loads a music sample from disk.
		/// By default, SDL_mixer only supports the Ogg Vorbis format, see http://www.vorbis.com/.
		/// It may be possible to compile in support for other formats such as MOD and and MP3.
		/// </summary>
		/// <param name="file">The filename to load</param>
		/// <returns>A new Music object</returns>
		public Music LoadMusic(string file) {
			IntPtr mus = Natives.Mix_LoadMUS(file);
			if (mus == IntPtr.Zero)
				throw SDLException.Generate();
			return new Music(mus);
		}
		/// <summary>
		/// Plays a music sample
		/// </summary>
		/// <param name="mus">The sample to play</param>
		/// <param name="numtimes">The number of times to play. Specify 1 to play a single time, -1 to loop forever.</param>
		public void PlayMusic(Music mus, int numtimes) {
			if (Natives.Mix_PlayMusic(mus.GetHandle(), numtimes) != 0)
				throw SDLException.Generate();
		}
		/// <summary>
		/// Plays a music sample and fades it in
		/// </summary>
		/// <param name="mus">The sample to play</param>
		/// <param name="numtimes">The number of times to play. Specify 1 to play a single time, -1 to loop forever.</param>
		/// <param name="ms">The number of milliseconds to fade in for</param>
		public void FadeInMusic(Music mus, int numtimes, int ms) {
			if (Natives.Mix_FadeInMusic(mus.GetHandle(), numtimes, ms) != 0)
				throw SDLException.Generate();
		}
		/// <summary>
		/// Plays a music sample, starting from a specific position and fades it in
		/// </summary>
		/// <param name="mus">The sample to play</param>
		/// <param name="numtimes">The number of times to play. Specify 1 to play a single time, -1 to loop forever.</param>
		/// <param name="ms">The number of milliseconds to fade in for</param>
		/// <param name="position">A format-defined position value. For Ogg Vorbis, this is the number of seconds from the beginning of the song</param>
		public void FadeInMusicPosition(Music mus, int numtimes, int ms, double position) {
			if (Natives.Mix_FadeInMusicPos(mus.GetHandle(), numtimes, ms, position) != 0)
				throw SDLException.Generate();
		}
		/// <summary>
		/// Sets the music volume
		/// </summary>
		/// <param name="volume">The new volume value, between 0 and 128 inclusive</param>
		/// <returns>The previous volume setting</returns>
		public int SetMusicVolume(int volume) {
			return Natives.Mix_VolumeMusic(volume);
		}
		/// <summary>
		/// Pauses the music playing
		/// </summary>
		public void PauseMusic() {
			Natives.Mix_PauseMusic();
		}
		/// <summary>
		/// Resumes paused music
		/// </summary>
		public void ResumeMusic() {
			Natives.Mix_ResumeMusic();
		}
		/// <summary>
		/// Resets the music position to the beginning of the sample
		/// </summary>
		public void RewindMusic() {
			Natives.Mix_RewindMusic();
		}
		/// <summary>
		/// Sets the music position to a format-defined value.
		/// For Ogg Vorbis, this is the number of seconds from the beginning of the song
		/// </summary>
		/// <param name="position"></param>
		public void SetMusicPosition(double position) {
			if (Natives.Mix_SetMusicPosition(position) != 0)
				throw SDLException.Generate();
		}
		/// <summary>
		/// Stops playing music
		/// </summary>
		public void HaltMusic() {
			Natives.Mix_HaltMusic();
		}
		/// <summary>
		/// Fades out music
		/// </summary>
		/// <param name="ms">The number of milliseconds to fade out for</param>
		public void FadeOutMusic(int ms) {
			if (Natives.Mix_FadeOutMusic(ms) != 1)
				throw SDLException.Generate();
		}
		/// <summary>
		/// Returns a flag indicating whether or not music is playing
		/// </summary>
		/// <returns>True if music is playing, otherwise False</returns>
		public bool PlayingMusic() {
			return (Natives.Mix_PlayingMusic() != 0);
		}
		/// <summary>
		/// Returns a flag indicating whether or not music is paused
		/// </summary>
		/// <returns>True if music is paused, otherwise False</returns>
		public bool PausedMusic() {
			return (Natives.Mix_PausedMusic() != 0);
		}
		/// <summary>
		/// Returns a flag indicating whether or not music is fading
		/// </summary>
		/// <returns>True if music is fading in or out, otherwise False</returns>
		public bool FadingMusic() {
			return (Natives.Mix_FadingMusic() != 0);
		}

		/// <summary>
		/// For performance reasons, you must call this method to enable the Events.ChannelFinished and Events.MusicFinished events
		/// </summary>
		public void EnableMusicCallbacks() {
			Natives.Mix_ChannelFinished(_channelfin);
			Natives.Mix_HookMusicFinished(_musicfin);
		}

		private void ChannelFinished(int channel) {
			_events.NotifyChannelFinished(channel);
		}
		private void MusicFinished() {
			_events.NotifyMusicFinished();
		}
	}
}