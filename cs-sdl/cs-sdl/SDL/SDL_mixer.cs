/*
 * BSD Licence:
 * Copyright (c) 2001, Lloyd Dupont (lloyd@galador.net)
 * <ORGANIZATION> 
 * All rights reserved.
 * 
 *
 * Redistribution and use in source and binary forms, with or without 
 * modification, are permitted provided that the following conditions are met:
 *
 * 1. Redistributions of source code must retain the above copyright notice, 
 * this list of conditions and the following disclaimer.
 * 2. Redistributions in binary form must reproduce the above copyright 
 * notice, this list of conditions and the following disclaimer in the 
 * documentation and/or other materials provided with the distribution.
 * 3. Neither the name of the <ORGANIZATION> nor the names of its contributors
 * may be used to endorse or promote products derived from this software
 * without specific prior written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
 * AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
 * IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
 * ARE DISCLAIMED. IN NO EVENT SHALL THE REGENTS OR CONTRIBUTORS BE LIABLE FOR
 * ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
 * DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
 * SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER
 * CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT 
 * LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY
 * OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH
 * DAMAGE.
 */
/*
    SDL - Simple DirectMedia Layer
    Copyright (C) 1997, 1998, 1999, 2000, 2001  Sam Lantinga

    This library is free software; you can redistribute it and/or
    modify it under the terms of the GNU Library General Public
    License as published by the Free Software Foundation; either
    version 2 of the License, or (at your option) any later version.

    This library is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
    Library General Public License for more details.

    You should have received a copy of the GNU Library General Public
    License along with this library; if not, write to the Free
    Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA

    Sam Lantinga
    slouken@devolution.com
*/
using System;
using System.Collections;
using System.IO;
using System.Runtime.InteropServices;

namespace CsGL.SDL
{
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct Mix_Chunk
	{
		public int allocated;
		public byte * abuf;
		public uint alen;
		public byte volume;
	}
	
	public enum Mix_Fading : int {
		MIX_NO_FADING,
		MIX_FADING_OUT,
		MIX_FADING_IN
	}
	
	[StructLayout(LayoutKind.Sequential)]
	public struct Mix_Music { // stand for (Mix_Music *)
		public IntPtr opaqueHandle;
	}
	
	public delegate void HookMusicFinished();
	public delegate void HookMusic(IntPtr udata, 
	                               [Out][MarshalAs(UnmanagedType.LPArray, SizeParamIndex=2)] byte[] stream, 
	                               int len);
	public delegate void PostMix(IntPtr udata, 
	                             [Out][MarshalAs(UnmanagedType.LPArray, SizeParamIndex=2)] byte[] stream, 
	                             int len);
	
	public unsafe class SDL_mixer : SDL
	{
		/// <summary>
		/// tell wether or not SDL_mixer is installed on this system
		/// </summary>
		public static readonly bool SoundEnabled;
		
		static SDL_mixer() {
			SoundEnabled = Library.Check("SDL_mixer");
			if(SoundEnabled && SDL_Init(SDL_INIT_AUDIO) < 0) {
					SoundEnabled = false;
					throw new SDLException();
			}
			
			if(SDL_byteorder.SysLittleEndian)
			MIX_DEFAULT_FORMAT = (ushort) (SDL_byteorder.SysLittleEndian ?
				SDL_audio.AUDIO_S16LSB : SDL_audio.AUDIO_S16LSB);
		}
	
		public const ushort MIX_CHANNELS = 8;
		public const int MIX_DEFAULT_FREQUENCY = 22050;
		public const ushort MIX_DEFAULT_CHANNELS = 2;
		public const ushort MIX_MAX_VOLUME = 128;
		public static readonly ushort MIX_DEFAULT_FORMAT;
	
	/* Open the mixer with a certain audio format */
		[DllImport("SDL_mixer.dll")]
		public static extern int Mix_OpenAudio(int frequency, 
		                                       ushort format, 
		                                       int channels,
		                                       int chunksize);
		
		/* Dynamically change the number of channels managed by the mixer.
		   If decreasing the number of channels, the upper channels are
		   stopped.
		   This function returns the new number of allocated channels.
		 */
		[DllImport("SDL_mixer.dll")]
		public static extern int Mix_AllocateChannels(int numchans);
		
		/* Find out what the actual audio device parameters are.
		   This function returns 1 if the audio has been opened, 0 otherwise.
		 */
		[DllImport("SDL_mixer.dll")]
		public static extern int Mix_QuerySpec(out int frequency,
		                                       out ushort format,
		                                       out int channels);
		
		/* Load a wave file or a music (.mod .s3m .it .xm) file */
		[DllImport("SDL_mixer.dll")]
		public static extern Mix_Chunk * Mix_LoadWAV_RW(SDL_RWops src, int freesrc);
		
		public static Mix_Chunk * Mix_LoadWAV(string file)
		{
			Stream input = new FileStream(file, FileMode.Open);
			SDL_RWops io = new SDL_RWops(input);
			return Mix_LoadWAV_RW(io, 1);
		}

		[DllImport("SDL_mixer.dll")]
		public static extern Mix_Music Mix_LoadMUS(string file);
		
		/* Load a wave file of the mixer format from a memory buffer */
		[DllImport("SDL_mixer.dll")]
		public static extern Mix_Chunk * Mix_QuickLoad_WAV(byte[] mem);
		[DllImport("SDL_mixer.dll")]
		public static extern Mix_Chunk * Mix_QuickLoad_WAV(byte * mem);
		
		/* Free an audio chunk previously loaded */
		[DllImport("SDL_mixer.dll")]
		public static extern void Mix_FreeChunk(Mix_Chunk * chunk);
		[DllImport("SDL_mixer.dll")]
		public static extern void Mix_FreeMusic(Mix_Music music);
		
#if MIX_HOOK_ENABLED // not now as i am not sur of how it works...		
		/* Set a function that is called after all mixing is performed.
		   This can be used to provide real-time visual display of the audio stream
		   or add a custom mixer filter for the stream data.
		*/
		[DllImport("SDL_mixer", EntryPoint="DM_Mix_SetPostMix")]
		public static extern void Mix_SetPostMix(PostMix mix_func, IntPtr arg);
		
		/* Add your own music player or additional mixer function.
		   If 'mix_func' is NULL, the default music player is re-enabled.
		 */
		[DllImport("SDL_mixer", EntryPoint="DM_Mix_HookMusic")]
		public static extern void Mix_HookMusic(HookMusic mix_func, IntPtr arg);
		
		/* Add your own callback when the music has finished playing.
		 */
#endif
		 
		[DllImport("SDL_mixer", EntryPoint="DM_Mix_HookMusicFinished")]
		public static extern void Mix_HookMusicFinished(HookMusicFinished music_finished);
		
		/* Get a pointer to the user data for the current music hook */
		[DllImport("SDL_mixer.dll")]
		public static extern IntPtr Mix_GetMusicHookData();
		
		/* Reserve the first channels (0 -> n-1) for the application, i.e. don't allocate
		   them dynamically to the next sample if requested with a -1 value below.
		   Returns the number of reserved channels.
		 */
		[DllImport("SDL_mixer.dll")]
		public static extern int Mix_ReserveChannels(int num);
		
		/* Channel grouping functions */
		
		/* Attach a tag to a channel. A tag can be assigned to several mixer
		   channels, to form groups of channels.
		   If 'tag' is -1, the tag is removed (actually -1 is the tag used to
		   represent the group of all the channels).
		   Returns true if everything was OK.
		 */
		[DllImport("SDL_mixer.dll")]
		public static extern int Mix_GroupChannel(int which, int tag);
		/* Assign several consecutive channels to a group */
		[DllImport("SDL_mixer.dll")]
		public static extern int Mix_GroupChannels(int from, int to, int tag);
		/* Finds the first available channel in a group of channels */
		[DllImport("SDL_mixer.dll")]
		public static extern int Mix_GroupAvailable(int tag);
		/* Returns the number of channels in a group. This is also a subtle
		   way to get the total number of channels when 'tag' is -1
		 */
		[DllImport("SDL_mixer.dll")]
		public static extern int Mix_GroupCount(int tag);
		/* Finds the "oldest" sample playing in a group of channels */
		[DllImport("SDL_mixer.dll")]
		public static extern int Mix_GroupOldest(int tag);
		/* Finds the "most recent" (i.e. last) sample playing in a group of channels */
		[DllImport("SDL_mixer.dll")]
		public static extern int Mix_GroupNewer(int tag);
		
		/* Play an audio chunk on a specific channel.
		   If the specified channel is -1, play on the first free channel.
		   If 'loops' is greater than zero, loop the sound that many times.
		   If 'loops' is -1, loop inifinitely (~65000 times).
		   Returns which channel was used to play the sound.
		*/
		public static int Mix_PlayChannel(int channel, Mix_Chunk * chunk, int loops) {
			return Mix_PlayChannelTimed(channel,chunk,loops,-1);
		}
		/* The same as above, but the sound is played at most 'ticks' milliseconds */
		[DllImport("SDL_mixer.dll")]
		public static extern int Mix_PlayChannelTimed(int channel, Mix_Chunk *chunk, int loops, int ticks);
		[DllImport("SDL_mixer.dll")]
		public static extern int Mix_PlayMusic(Mix_Music music, int loops);
		
		/* Fade in music or a channel over "ms" milliseconds, same semantics as the "Play" functions */
		[DllImport("SDL_mixer.dll")]
		public static extern int Mix_FadeInMusic(Mix_Music music, int loops, int ms);
		public static int Mix_FadeInChannel(int channel, Mix_Chunk * chunck, int loops, int ms) {
			return Mix_FadeInChannelTimed(channel,chunck,loops,ms,-1);
		}
		[DllImport("SDL_mixer.dll")]
		public static extern int Mix_FadeInChannelTimed(int channel, Mix_Chunk * chunk, int loops, int ms, int ticks);
		
		/* Set the volume in the range of 0-128 of a specific channel or chunk.
		   If the specified channel is -1, set volume for all channels.
		   Returns the original volume.
		   If the specified volume is -1, just return the current volume.
		*/
		[DllImport("SDL_mixer.dll")]
		public static extern int Mix_Volume(int channel, int volume);
		[DllImport("SDL_mixer.dll")]
		public static extern int Mix_VolumeChunk(Mix_Chunk * chunk, int volume);
		[DllImport("SDL_mixer.dll")]
		public static extern int Mix_VolumeMusic(int volume);
		
		/* Halt playing of a particular channel */
		[DllImport("SDL_mixer.dll")]
		public static extern int Mix_HaltChannel(int channel);
		[DllImport("SDL_mixer.dll")]
		public static extern int Mix_HaltGroup(int tag);
		[DllImport("SDL_mixer.dll")]
		public static extern int Mix_HaltMusic();
		
		/* Change the expiration delay for a particular channel.
		   The sample will stop playing after the 'ticks' milliseconds have elapsed,
		   or remove the expiration if 'ticks' is -1
		*/
		[DllImport("SDL_mixer.dll")]
		public static extern int Mix_ExpireChannel(int channel, int ticks);
		
		/* Halt a channel, fading it out progressively till it's silent
		   The ms parameter indicates the number of milliseconds the fading
		   will take.
		 */
		[DllImport("SDL_mixer.dll")]
		public static extern int Mix_FadeOutChannel(int which, int ms);
		[DllImport("SDL_mixer.dll")]
		public static extern int Mix_FadeOutGroup(int tag, int ms);
		[DllImport("SDL_mixer.dll")]
		public static extern int Mix_FadeOutMusic(int ms);
		
		/* Query the fading status of a channel */
		[DllImport("SDL_mixer.dll")]
		public static extern Mix_Fading Mix_FadingMusic();
		[DllImport("SDL_mixer.dll")]
		public static extern Mix_Fading Mix_FadingChannel(int which);
		
		/* Pause/Resume a particular channel */
		[DllImport("SDL_mixer.dll")]
		public static extern void Mix_Pause(int channel);
		[DllImport("SDL_mixer.dll")]
		public static extern void Mix_Resume(int channel);
		[DllImport("SDL_mixer.dll")]
		public static extern int  Mix_Paused(int channel);
		
		/* Pause/Resume the music stream */
		[DllImport("SDL_mixer.dll")]
		public static extern void Mix_PauseMusic();
		[DllImport("SDL_mixer.dll")]
		public static extern void Mix_ResumeMusic();
		[DllImport("SDL_mixer.dll")]
		public static extern void Mix_RewindMusic();
		[DllImport("SDL_mixer.dll")]
		public static extern int  Mix_PausedMusic();
		
		/* Check the status of a specific channel.
		   If the specified channel is -1, check all channels.
		*/
		[DllImport("SDL_mixer.dll")]
		public static extern int Mix_Playing(int channel);
		[DllImport("SDL_mixer.dll")]
		public static extern int Mix_PlayingMusic();
		
		/* Stop music and set external music playback command */
		[DllImport("SDL_mixer.dll")]
		public static extern int Mix_SetMusicCMD(string command);
		
		/* Close the mixer, halting all playing audio */
		[DllImport("SDL_mixer.dll")]
		public static extern void Mix_CloseAudio();
	}
}
