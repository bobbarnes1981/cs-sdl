/*
 * $RCSfile$
 * Copyright (C) 2005 David Hudson (jendave@yahoo.com)
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
	/// Represents a sound sample.
	/// Create with Mixer.LoadWav().
	/// </summary>
	public class Channel
	{
		private int index;
		private Sound sound;
		private Sound queuedSound = null;
		private Sound lastSound = null;
		private SdlMixer.ChannelFinishedDelegate channelFinishedDelegate;

//		/// <summary>
//		/// 
//		/// </summary>
//		public Channel()
//		{
//			this.index = Mixer.FindAvailableChannel();
//		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="index"></param>
		public Channel(int index)
		{
			this.index = index;
		}

		/// <summary>
		/// 
		/// </summary>
		public int Index
		{
			get
			{
				return this.index;
			}
		}


		/// <summary>
		/// Plays a sound the specified number of times on a specific channel
		/// </summary>
		/// <param name="sound">The sound to play</param>
		/// <returns>The channel used to play the sound</returns>
		public int Play(Sound sound) 
		{
			return this.Play(sound, 0);
		}
		/// <summary>
		/// Plays a sound the specified number of times on a specific channel
		/// </summary>
		/// <param name="sound">The sound to play</param>
		/// <param name="loops">
		/// The number of loops.  Specify 1 to have the sound play twice
		/// </param>
		/// <returns>The channel used to play the sound</returns>
		public int Play(Sound sound, int loops) 
		{
			return this.Play(sound, loops, (int) SdlFlag.PlayForever);
		}

		/// <summary>
		/// Plays a sound the specified number of times on a 
		/// specific channel, stopping after the specified number of ms
		/// </summary>
		/// <param name="sound">The sound to play</param>
		/// <param name="loops">
		/// The number of loops.  Specify 1 to have the sound play twice
		/// </param>
		/// <param name="milliseconds">The time limit in milliseconds</param>
		/// <returns>The channel used to play the sound</returns>
		public int Play(Sound sound, int loops, int milliseconds) 
		{
			
			int ret = SdlMixer.Mix_PlayChannelTimed(this.index, sound.GetHandle(), loops, milliseconds);
			if (ret == (int) SdlFlag.Error)
			{
				throw SdlException.Generate();
			}
			this.Sound = sound;
			return ret;
		}

		/// <summary>
		/// Plays a sound the specified number of times on a specific channel
		/// </summary>
		/// <param name="sound">The sound to play</param>
		/// <returns>The channel used to play the sound</returns>
		public int PlayContinously(Sound sound) 
		{
			return this.Play(sound, -1, (int) SdlFlag.PlayForever);
		}

		/// <summary>
		/// Plays a sound the specified number of times on a 
		/// specific channel, stopping after the specified number of ms
		/// </summary>
		/// <param name="sound">The sound to play</param>
		/// <param name="milliseconds">The time limit in milliseconds</param>
		/// <returns>The channel used to play the sound</returns>
		public int PlayContinuosly(Sound sound, int milliseconds) 
		{
			
			int ret = SdlMixer.Mix_PlayChannelTimed(this.index, sound.GetHandle(), -1, milliseconds);
			if (ret == (int) SdlFlag.Error)
			{
				throw SdlException.Generate();
			}
			this.Sound = sound;
			return ret;
		}

		/// <summary>
		/// Sets the volume for a channel
		/// </summary>
		/// <returns>Channel volume</returns>
		public int Volume
		{
			get
			{
				return SdlMixer.Mix_Volume(this.index, -1);
			}
			set
			{
				int dummy = SdlMixer.Mix_Volume(this.index, value);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public Sound Sound
		{
			get
			{
				return this.sound;
			}
			set
			{
				if (this.sound != null)
				{
					this.lastSound = this.sound;
					this.lastSound.NumberOfChannels--;
				}
				this.sound = value;
				this.sound.SoundEvent += new SoundEventHandler(ProcessSoundEvent);
				this.sound.NumberOfChannels++;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public Sound LastSound
		{
			get
			{
				return this.lastSound;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public Sound QueuedSound
		{
			get
			{
				return this.queuedSound;
			}
			set
			{
				this.queuedSound = value;
			}
		}

		/// <summary>
		/// Fades in a sound the specified number of times on a 
		/// specific channel
		/// </summary>
		/// <param name="sound">The sound to play</param>
		/// <param name="ms">The number of milliseconds to fade in for</param>
		/// <param name="loops">
		/// The number of loops.  
		/// Specify 1 to have the sound play twice
		/// </param>
		/// <returns>The channel used to play the sound</returns>
		public int FadeIn(Sound sound, int ms, int loops) 
		{
			int ret = SdlMixer.Mix_FadeInChannelTimed(this.index, sound.GetHandle(), loops, ms, -1);
			if (ret == (int) SdlFlag.Error)
			{
				throw SdlException.Generate();
			}
			return ret;
		}

		/// <summary>
		/// Fades in a sound the specified number of times on 
		/// a specific channel, stopping after the specified number of ms
		/// </summary>
		/// <param name="sound">The sound to play</param>
		/// <param name="ms">The number of milliseconds to fade in for
		/// </param>
		/// <param name="loops">The number of loops.  
		/// Specify 1 to have the sound play twice</param>
		/// <param name="ticks">The time limit in milliseconds</param>
		/// <returns>The channel used to play the sound</returns>
		public int FadeInTimed(Sound sound, int ms, int loops, int ticks) 
		{
			int ret = SdlMixer.Mix_FadeInChannelTimed(this.index, sound.GetHandle(), loops, ms, ticks);
			if (ret == (int) SdlFlag.Error)
			{
				throw SdlException.Generate();
			}
			return ret;
		}

		/// <summary>
		/// Pauses playing on a specific channel
		/// </summary>
		public void Pause() 
		{
			SdlMixer.Mix_Pause(this.index);
		}

		/// <summary>
		/// Resumes playing on a paused channel
		/// </summary>
		public void Resume() 
		{
			SdlMixer.Mix_Resume(this.index);
		}

		/// <summary>
		/// Stop playing on a specific channel
		/// </summary>
		public void Stop() 
		{
			SdlMixer.Mix_HaltChannel(this.index);
			this.sound = null;
		}
		/// <summary>
		/// Stop playing a channel after a specified time interval
		/// </summary>
		/// <param name="ms">
		/// The number of milliseconds to stop playing after
		/// </param>
		public void Expire(int ms) 
		{
			SdlMixer.Mix_ExpireChannel(this.index, ms);
		}
		/// <summary>
		/// Fades out a channel.
		/// </summary>
		/// <param name="ms">
		/// The number of milliseconds to fade out for
		/// </param>
		/// <returns>The number of channels fading out</returns>
		public int FadeOut(int ms) 
		{
			return SdlMixer.Mix_FadeOutChannel(this.index, ms);
		}

		/// <summary>
		/// Returns a flag indicating whether or not a channel is playing
		/// </summary>
		/// <returns>True if the channel is playing, otherwise False</returns>
		public bool IsPlaying() 
		{
			return (SdlMixer.Mix_Playing(this.index) != 0);
		}
		/// <summary>
		/// Returns a flag indicating whether or not a channel is paused
		/// </summary>
		/// <returns>True if the channel is paused, otherwise False</returns>
		public bool IsPaused() 
		{
			return (SdlMixer.Mix_Paused(this.index) != 0);
		}
		/// <summary>
		/// Returns the current fading status of a channel
		/// </summary>
		/// <returns>The current fading status of the channel</returns>
		public  FadingStatus FadingStatus() 
		{
			return (FadingStatus)SdlMixer.Mix_FadingChannel(this.index);
		}
		/// <summary>
		/// Sets the panning (stereo attenuation) for a specific channel
		/// </summary>
		/// <param name="left">A left speaker value from 0-255 inclusive</param>
		/// <param name="right">A right speaker value from 0-255 inclusive</param>
		public  void SetPanning(int left, int right) 
		{
			if (SdlMixer.Mix_SetPanning(this.index, (byte)left, (byte)right) == 0)
			{
				throw SdlException.Generate();
			}
		}

		/// <summary>
		/// Sets the distance (attenuate sounds based on distance 
		/// from listener) for a specific channel
		/// </summary>
		/// <param name="distance">Distance value from 0-255 inclusive</param>
		public  void SetDistance(int distance) 
		{
			if (SdlMixer.Mix_SetDistance(this.index, (byte)distance) == 0)
			{
				throw SdlException.Generate();
			}
		}

		/// <summary>
		/// Sets the "position" of a sound (approximate '3D' audio)
		///  for a specific channel
		/// </summary>
		/// <param name="angle">The angle of the sound, between 0 and 359,
		///  0 = directly in front</param>
		/// <param name="distance">The distance of the sound from 0-255
		///  inclusive</param>
		public void SetPosition(int angle, int distance) 
		{
			if (SdlMixer.Mix_SetPosition(this.index, (short)angle, (byte)distance) == 0)
			{
				throw SdlException.Generate();
			}
		}

		/// <summary>
		/// Flips the left and right stereo for the channel
		/// </summary>
		/// <param name="flip">True to flip, False to reset to normal</param>
		public void ReverseStereo(bool flip) 
		{
			if (SdlMixer.Mix_SetReverseStereo(this.index, flip?1:0) == 0)
			{
				throw SdlException.Generate();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public void EnableChannelFinishedCallback() 
		{
			channelFinishedDelegate = new SdlMixer.ChannelFinishedDelegate(ChannelFinished);
			SdlMixer.Mix_ChannelFinished(channelFinishedDelegate);
			Events.ChannelFinished +=new ChannelFinishedEventHandler(Events_ChannelFinished);
		}
		private void ChannelFinished(int channel) 
		{
			Events.NotifyChannelFinished(this.index);
		}

		private void Events_ChannelFinished(object sender, ChannelFinishedEventArgs e)
		{
			//Console.WriteLine("channel finished event handler");
			if (this.queuedSound != null)
			{
				//Console.WriteLine("playing queued sound");
				this.Play(this.queuedSound);
				this.queuedSound = null;
			}
		}

		private void ProcessSoundEvent(object sender, SoundEventArgs e)
		{
			if (e.Action == SoundAction.Stop)
			{
				//Console.WriteLine("channel Stopped: " + this.index);
				this.Stop();
			}
			else if (e.Action == SoundAction.Fadeout)
			{
				this.FadeOut(e.FadeoutTime);
			}
			else
			{
				throw new SdlException();
			}
		}
	}
}
