#region License
/*
 * Copyright (C) 2001-2005 Wouter van Oortmerssen.
 * 
 * This software is provided 'as-is', without any express or implied
 * warranty.  In no event will the authors be held liable for any damages
 * arising from the use of this software.
 * 
 * Permission is granted to anyone to use this software for any purpose,
 * including commercial applications, and to alter it and redistribute it
 * freely, subject to the following restrictions:
 * 
 * 1. The origin of this software must not be misrepresented; you must not
 * claim that you wrote the original software. If you use this software
 * in a product, an acknowledgment in the product documentation would be
 * appreciated but is not required.
 * 2. Altered source versions must be plainly marked as such, and must not be
 * misrepresented as being the original software.
 * 3. This notice may not be removed or altered from any source distribution.
 * 
 * additional clause specific to Cube:
 * 
 * 4. Source versions may not be "relicensed" under a different license
 * without my explicitly written permission.
 *
 */

/* 
 * All C# code Copyright (C) 2006 David Y. Hudson
 * Mezzanine is a .NET port of Cube (version released on 2005-Aug-29).
 * Cube was written by Wouter van Oortmerssen (http://cubeengine.com)
 */
#endregion License

using System;
using System.Collections;
using System.IO;
using Tao.Sdl;
using Tao.OpenGl;
using SdlDotNet;
using System.Runtime.InteropServices;

namespace MezzanineLib.Support
{
	/// <summary>
	/// 
	/// </summary>
	public struct SoundLocation 
	{ 
		/// <summary>
		/// 
		/// </summary>
		public Vector Location; 
		/// <summary>
		/// 
		/// </summary>
		public bool InUse; 
	} 
	
	public sealed class Sound
	{
		public const int MAXCHAN = 32;
		public const int SOUNDFREQ = 22050;
		public static int soundsatonce = 0;
		public static int lastsoundmillis = 0;

		public static bool noSound = false;
		static int soundVolume = 255;
		static int musicVolume = 128;
		static SdlDotNet.Music music;
		static SoundLocation[] soundLocations = new SoundLocation[MAXCHAN];
		static ArrayList samples = new ArrayList();
		static ArrayList snames = new ArrayList();

		/// <summary>
		/// TODO
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public static int RegisterSound(string name)
		{
			if (snames.Contains(name))
			{
				return snames.IndexOf(name);
			}
			snames.Add(name);
			samples.Add(null);
			return samples.Count-1;
		}

		//#define MAXVOL MIX_MAX_VOLUME
		//Mix_Music *mod = NULL;
		//void *stream = NULL;

		//VAR(soundbufferlen, 128, 1024, 4096);

		public static void Music(string filename)
		{
			if(noSound) 
			{
				return;
			}
			SdlDotNet.Music.Stop();
			if(soundVolume!= 0 && musicVolume!=0)
			{
				string soundName = @"packages/" + filename;
				music = new Music(soundName);
				music.Play(true);
				SdlDotNet.Music.Volume = musicVolume;
			}
		}

		//	COMMAND(music, ARG_1STR);

		//	vector<Mix_Chunk *> samples;

		//	cvector snames;
		//
		//		int registersound(char *name)
		//		{
		//			loopv(snames) if(strcmp(snames[i], name)==0) return i;
		//			snames.add(newstring(name));
		//			samples.add(NULL);
		//			return samples.length()-1;
		//		};

		//	COMMAND(registersound, ARG_1EST);

		public static void CleanSound()
		{
			SdlDotNet.Music.Stop();
			Mixer.Close();
		}

		//VAR(stereo, 0, 1, 1);

		public static void UpdateChannelVolume(int channel, Vector location)
		{
			int volume = soundVolume;
			//int pan = 255/2;
			//if(location = )
			//{
			//vdist(dist, v, location, GameInit.Player1.o);
			//volume -= (int)(dist*3*soundVolume/255); // simple mono distance attenuation
			//if(stereo && (v.x != 0 || v.y != 0))
			//{
			//	float yaw = -Math.Atan2(v.x, v.y) - player1->yaw*(PI / 180.0f); // relative angle of sound along X-Y axis
				//pan = int(255.9f*(0.5*sin(yaw)+0.5f)); // range is from 0 (left) to 255 (right)
			//};
			//};
			//volume = (volume*MAXVOL)/255;
			// Mix_Volume(chan, vol);
			// Mix_SetPanning(chan, 255-pan, pan);
		}  
		
		//				void newsoundloc(int chan, vec *loc)
		//				{
		//					assert(chan>=0 && chan<MAXCHAN);
		//					soundlocs[chan].loc = *loc;
		//					soundlocs[chan].inuse = true;
		//				};
		//
		//		void updatevol()
		//		{
		//			if(nosound) return;
		//			loopi(MAXCHAN) if(soundlocs[i].inuse)
		//						   {
		//							   /* if(Mix_Playing(i))
		//									updatechanvol(i, &soundlocs[i].loc);
		//								else soundlocs[i].inuse = false;*/
		//						   };
		//		};
		//
		//		void playsoundc(int n) { addmsg(0, 2, SV_SOUND, n); playsound(n); };

		public static int soundsAtOnce = 0;
		public static int lastSoundMillis = 0;

		//		void playsound(int n, vec *loc)
		//		{
		//			if(nosound) return;
		//			if(!soundvol) return;
		//			if(lastmillis==lastsoundmillis) soundsatonce++; else soundsatonce = 1;
		//			lastsoundmillis = lastmillis;
		//			if(soundsatonce>5) return;  // avoid bursts of sounds with heavy packetloss and in sp
		//			if(n<0 || n>=samples.length()) { conoutf("unregistered sound: %d", n); return; };
		//
		//			if(!samples[n])
		//			{
		//				sprintf_sd(buf)("packages/sounds/%s.wav", snames[n]);
		//
		//				//samples[n] = Mix_LoadWAV(path(buf));
		//
		//				// if(!samples[n]) { conoutf("failed to load sample: %s", buf); return; };
		//			};
		//
		//			int chan;// = Mix_PlayChannel(-1, samples[n], 0);
		//			if(chan<0) return;
		//			if(loc) newsoundloc(chan, loc);
		//			updatechanvol(chan, loc);
		//		};

		//		void sound(int n) { playsound(n, NULL); };
		//		COMMAND(sound, ARG_1INT);

	}
}
