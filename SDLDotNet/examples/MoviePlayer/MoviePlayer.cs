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
using System.Runtime.InteropServices;
using SdlDotNet;

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
	/// </remarks>
	#endregion Class Documentation
	class MoviePlayer 
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

			Events.KeyboardDown += 
				new KeyboardEventHandler(this.KeyboardDown); 
			Events.Quit += new QuitEventHandler(this.Quit);

			Mixer.Initialize();
			Surface screen = Video.SetVideoModeWindow(width, height, true); 
			Video.WindowCaption = "SDL.NET - Movie Player";
			Mixer.Close();
			Movie movie = new Movie(filepath + "test.mpg");
			Console.WriteLine("Time: " + movie.Length);
			Console.WriteLine("Width: " + movie.Size.Width);
			Console.WriteLine("Height: " + movie.Size.Height);
			Console.WriteLine("HasAudio: " + movie.HasAudio);
			movie.Display(screen);
			
			movie.Play();

			try
			{
				while (movie.IsPlaying && (quitFlag == false))
				{
					while (Events.Poll()){}
				}
			} catch (MovieStatusException)
			{
				throw;
			}
			movie.Stop();
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
	}
}