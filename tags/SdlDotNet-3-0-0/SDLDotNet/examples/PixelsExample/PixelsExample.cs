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
using System.Drawing;
using SdlDotNet;

// Simple SDL.NET Example
// Just draws a bunch of rectangles to the screen, to quit hit 'Q' or Esc.

namespace SdlDotNet.Examples {
	/// <summary>
	/// 
	/// </summary>
	public class PixelsExample {
		private bool quitFlag;
		
		/// <summary>
		/// 
		/// </summary>
		public PixelsExample() 
		{
			quitFlag = false;
		}

		/// <summary>
		/// 
		/// </summary>
		public void Run() {
			int width = 640;
			int height = 480;
			int bitsPerPixel = 32;
			Random rand = new Random();

			Events.KeyboardDown += 
				new KeyboardEventHandler(this.KeyboardDown); 
			Events.Quit += new QuitEventHandler(this.Quit);

			int x;
			int y;
			int r;
			int g;
			int b;
			int colorValue;

			try {
				// set the video mode
				Surface screen = Video.SetVideoModeWindow(width, height, bitsPerPixel, true); 
				Video.WindowCaption = "Pixels Example";
				Video.Mouse.ShowCursor(false);

				while (!quitFlag) 
				{
					while (Events.Poll()) 
					{
						// handle events till the queue is empty
					} 
					
					try 
					{
						screen.Lock();
						x = rand.Next(10,width);
						y = rand.Next(10,height);
						r = rand.Next(255);
						g = rand.Next(255);
						b = rand.Next(255);

						colorValue = screen.GetColorValue(Color.FromArgb(r, g, b));
						//colorValue = screen.MapColor(Color.FromArgb(254, 0, 0));
						//screen.DrawPixel(x, y, Color.Red);
						Console.WriteLine("colorValue: " + colorValue.ToString());
						screen.DrawPixel(x, y, Color.FromArgb(r, g, b));
						//screen.DrawPixel(x, y, Color.Red);
						Console.WriteLine("GetPixel: " + screen.GetPixel(x, y).ToString());
						Console.WriteLine("GetPixel: " + screen.GetColorValue(screen.GetPixel(x, y)).ToString());
					

						screen.Unlock();
						screen.Flip();
					} 
					catch (SurfaceLostException) 
					{
						// if we are fullscreen and the user hits alt-tab we can get this, for this simple app we can ignore it
					}
					catch (NullReferenceException e) 
					{
						Console.WriteLine(e.StackTrace);
					}
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
			PixelsExample pixelsExample = new PixelsExample();
			pixelsExample.Run();
		}
	}
}
