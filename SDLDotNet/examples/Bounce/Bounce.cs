/*
 * $RCSfile$
 * Copyright (C) Sam Hart 
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

namespace SdlDotNet.Examples 
{
	/// <summary>
	/// Simple SDL.NET Example of bouncing balls
	/// By Sam Hart 
	/// Updated to SdlDotNet 3.0.0 by David Hudson
	/// </summary>
	public class Bounce 
	{
		private bool _quitflag;
		int WIDTH = 640;
		int HEIGHT = 480;
		static int MAX_BALLS = 20;
		int MIN_MOTION = -5;
		int MAX_MOTION = 5;
		Random rand = new Random();
		string[] balls = { "../../red.bmp", "../../blue.bmp", "../../green.bmp" };
		int[] x = new int[MAX_BALLS];
		int[] y = new int[MAX_BALLS];
		int[] mx = new int[MAX_BALLS];
		int[] my = new int[MAX_BALLS];
		Surface[] ballSurf = new Surface[MAX_BALLS];

		private void RandomizeMotion() 
		{
			for(int i = 0; i < MAX_BALLS; i++) 
			{
				mx[i] = rand.Next(MIN_MOTION, MAX_MOTION);
				my[i] = rand.Next(MIN_MOTION, MAX_MOTION);
			}
		}

		private void SDL_KeyboardDown(object sender, KeyboardEventArgs e) 
		{
			if (e.Key == Key.Escape || e.Key == Key.Q)
				_quitflag = true;
			if (e.Key == Key.Space)
				this.RandomizeMotion();
		}
		private void SDL_Quit(object sender, QuitEventArgs e) 
		{
			_quitflag = true;
		}

		public void Go() 
		{
			for(int i = 0; i < MAX_BALLS; i++) 
			{
				ballSurf[i] = new Surface(balls[rand.Next(0, balls.Length)]);
				ballSurf[i].SetColorKey(Color.FromArgb(255, 255, 255), true);
				x[i] = 0; y[i] = 0;
			}

			this.RandomizeMotion();

			Events.KeyboardDown += new KeyboardEventHandler(this.SDL_KeyboardDown); 
			// register event handlers
			Events.Quit += new QuitEventHandler(this.SDL_Quit);
			try 
			{

				Surface screen = Video.SetVideoModeWindow(WIDTH, HEIGHT, true);
				Video.WindowCaption = "SDL# Bouncer"; //, "SDL# Bouncer");
				Video.Mouse.ShowCursor(false); // hide the cursor

				// create a surface to draw to...
				// we cant draw rectangles directly to the hardware back buffer because
				// different video cards have different 
				// numbers of back buffers that flip in sequence :(
				Surface surf = screen.CreateCompatibleSurface(WIDTH, HEIGHT, true);

				surf.Fill(new Rectangle(new Point(0, 0), screen.Size), Color.FromArgb(255, 255, 255));

				while (!_quitflag) 
				{
					while (Events.Poll()) {} // handle events till the queue is empty
					for(int i = 0; i < MAX_BALLS; i++) 
					{
						surf.Blit(ballSurf[i], new Rectangle(new Point(x[i], y[i]), ballSurf[i].Size));
						x[i] += mx[i];
						y[i] += my[i];
						if(x[i] > WIDTH - ballSurf[i].Width) 
						{
							x[i] = WIDTH - ballSurf[i].Width;
							mx[i] *= -1;
						} 
						else if(x[i] < 0) 
						{
							x[i] = 0;
							mx[i] *= -1;
						}
						if(y[i] > HEIGHT - ballSurf[i].Height) 
						{
							y[i] = HEIGHT - ballSurf[i].Height;
							my[i] *= -1;
						} 
						else if(y[i] < 0) 
						{
							y[i] = 0;
							my[i] *= -1;
						}
					}

					screen.Blit(surf, new Rectangle(new Point(0, 0), screen.Size));
					screen.Flip();

					surf.Fill(new Rectangle(new Point(0, 0), surf.Size), Color.FromArgb(255, 255, 255));

					try 
					{

					} 
					catch (SurfaceLostException) 
					{
						// if we are fullscreen and the user hits 
						// alt-tab we can get this, for this simple app we can ignore it
					}
				}
			} 
			catch 
			{
				//sdl.Dispose(); // quit sdl so the window goes away, then handle the error...
				throw; // for this example we'll just throw it to the debugger
			}

		}

		public static void Main() 
		{
			Bounce bounce = new Bounce();
			bounce.Go();
		}
	}
}
