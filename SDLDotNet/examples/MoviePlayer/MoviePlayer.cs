/*
 * $RCSfile$
 * Copyright (C) 2004 David Hudson (jendave@yahoo.com)
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
using System.IO;
using Tao.Sdl;
using System.Runtime.InteropServices;

namespace SdlDotNet.Examples 
{
	#region Class Documentation
	/// <summary>
	/// Simple Tao.Sdl Example
	/// </summary>
	/// <remarks>
	/// Just plays a short movie.
	/// To quit, you can close the window, 
	/// press the Escape key or press the 'q' key
	/// <p>Written by David Hudson (jendave@yahoo.com)</p>
	/// <p>This is a reimplementation of an example 
	/// written by Will Weisser (ogl@9mm.com)</p>
	/// </remarks>
	#endregion Class Documentation
	public class MoviePlayer 
	{		
		bool quitFlag;

		#region Run()
		/// <summary>
		/// 
		/// </summary>
		public void Run() 
		{
			quitFlag = false;
			string filepath = @"../../";
			if (File.Exists("test.mpg"))
			{
				filepath = "";
			}
			
			int width = 352;
			int height = 240;
			
			Video video = Video.Instance;
			WindowManager wm = WindowManager.Instance;
			Mixer mixer = Mixer.Instance;
			Events events = Events.Instance;

			events.KeyboardDown += 
				new KeyboardEventHandler(this.KeyboardDown); 
			events.Quit += new QuitEventHandler(this.Quit);

			//SdlMixer.MixFunctionDelegate audioMixer = new SdlMixer.MixFunctionDelegate(this.player);
			//(Smpeg.SMPEG_playAudioSDL);
			Surface screen = video.SetVideoModeWindow(width, height, true); 
			Mixer.Close();
			Movie movie = new Movie(filepath + "test.mpg");
			Console.WriteLine("Time: " + movie.TotalTime);
			Console.WriteLine("Width: " + movie.Width);
			Console.WriteLine("Height: " + movie.Height);
			Console.WriteLine("HasAudio: " + movie.HasAudio);
//			movie.DisableAudio();
//			int freq;
//			short format;
//			int channels;
//			SdlMixer.Mix_QuerySpec(out freq, out unchecked(format), out channels);
//			Sdl.SDL_AudioSpec audiofmt = new Tao.Sdl.Sdl.SDL_AudioSpec();
//			audiofmt.freq = freq;
//			audiofmt.format = unchecked(format);
//			audiofmt.channels = (byte) channels;
//			Console.WriteLine("Freq: " + audiofmt.freq);
//			Console.WriteLine("Format: " + audiofmt.format);
//			Console.WriteLine("Channels: " + audiofmt.channels);
//			Smpeg.SMPEG_actualSpec(movie.GetHandle, ref audiofmt); 
//			SdlMixer.Mix_HookMusic(audioMixer, movie.GetHandle);
//			movie.EnableAudio();
			movie.Display(screen);
			
			movie.Play();

			try
			{
				while (movie.IsPlaying && (quitFlag == false))
				{
					while (events.PollAndDelegate()){}
				}
			} catch (MovieStatusException)
			{
				throw;
			}
			movie.Stop();
			//mixer.Open();
			//SdlMixer.Mix_HookMusic(null, IntPtr.Zero);
			movie.Close();

		} 
		#endregion Run()

		#region Main()
		[STAThread]
		static void Main() 
		{
			MoviePlayer player = new MoviePlayer();
			player.Run();
		}
		#endregion Main()

		private void KeyboardDown(
			object sender,
			KeyboardEventArgs e) 
		{
			if (e.Key == Key.Escape ||
				e.Key == Key.Q)
			{
				quitFlag = true;
			}
		}

		private void Quit(object sender, QuitEventArgs e) 
		{
			quitFlag = true;
		}

//		private void player(IntPtr one, IntPtr two, int len)
//		{
//
//			byte[] testbyte = new byte[len];
//			Marshal.Copy(two, testbyte, 0, len);
//			Smpeg.SMPEG_playAudioSDL(one, testbyte, len);
//
//		}
	}
}
