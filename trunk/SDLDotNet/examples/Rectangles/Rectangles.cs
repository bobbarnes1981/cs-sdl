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
using System.Drawing;
using System.IO;
using SdlDotNet;

// Simple SDL.NET Example
// Just draws a bunch of rectangles to the screen, to quit hit 'Q' or Esc.

namespace SdlDotNet.Examples 
{
	/// <summary>
	/// 
	/// </summary>
	public class Rectangles 
	{
		private bool quitFlag;
		
		/// <summary>
		/// 
		/// </summary>
		public Rectangles() 
		{
			quitFlag = false;
		}

		/// <summary>
		/// 
		/// </summary>
		public void Run() 
		{
			string filepath = @"../../";
			if (File.Exists("fard-two.ogg"))
			{
				filepath = "";
			}
			int width = 640;
			int height = 480;
			Random rand = new Random();
				
			Events.KeyboardDown += 
				new KeyboardEventHandler(this.KeyboardDown); 
			Events.Quit += new QuitEventHandler(this.Quit);

			try {
				Music music = Mixer.LoadMusic(filepath + "fard-two.ogg");
				Mixer.PlayMusic(music, 1);
				// set the video mode
				Surface screen = Video.SetVideoModeWindow(width, height, true); 
				WindowManager.Caption = "Rectangles Example";
				Video.HideMouseCursor();
				Mixer.EnableMusicFinishedCallback();

				Surface surf = 
					screen.CreateCompatibleSurface(width, height, true);
				//fill the surface with black
				surf.FillRectangle(new Rectangle(new Point(0, 0), surf.Size), Color.Black); 

				while (!quitFlag) 
				{
					while (Events.PollAndDelegate()) 
					{
						// handle events till the queue is empty
					} 
					
					try 
					{
						surf.FillRectangle(
							new Rectangle(rand.Next(-300, width), 
							rand.Next(-300, height), rand.Next(20, 300), 
							rand.Next(20, 300)),
						Color.FromArgb(rand.Next(255), rand.Next(255), rand.Next(255)));
						surf.Blit(screen, new Rectangle(new Point(0, 0), screen.Size));
						screen.Flip();
					} 
					catch (SurfaceLostException) 
					{
						// if we are fullscreen and the user hits alt-tab 
						// we can get this, for this simple app we can ignore it
					}
					//}
				}
			} 
			catch 
			{
				//sdl.Dispose(); 
				// quit sdl so the window goes away, then handle the error...
				throw; // for this example we'll just throw it to the debugger
			}
		}

		private void KeyboardDown(
			object sender,
			KeyboardEventArgs e) {
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

		[STAThread]
		static void Main() {
			Rectangles rectangles = new Rectangles();
			rectangles.Run();
		}
	}
}
