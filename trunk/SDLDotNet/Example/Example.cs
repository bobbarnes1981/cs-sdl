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
using System.Drawing;
using SDLDotNet;
using SDLDotNet.Mixer;

// Simple SDL.NET Example
// Just draws a bunch of rectangles to the screen, to quit hit 'Q' or Esc.

namespace SDLDotNet.Example {
	public class SDLExample {
		private bool _quitflag;

		public SDLExample() {
			_quitflag = false;
		}

		public void Go() {
			int width = 800;
			int height = 600;
			Random rand = new Random();
			
			string musicfile = "../../fard-two.ogg";


			SDL sdl = SDL.Instance; // get SDL object

			sdl.Events.KeyboardDown += new KeyboardEventHandler(this.SDL_KeyboardDown); // register event handlers
			sdl.Events.Quit += new QuitEventHandler(this.SDL_Quit);

			try {
				Music music = sdl.Mixer.LoadMusic(musicfile);
				sdl.Mixer.PlayMusic(music, -1);

				//Surface screen = sdl.Video.SetVideoMode(width, height); // set the video mode
				Surface screen = sdl.Video.SetVideoModeWindow(width, height, true);
				sdl.Video.HideMouseCursor(); // hide the cursor

				// create a surface to draw to...we cant draw rectangles directly to the hardware back buffer because
				// different video cards have different numbers of back buffers that flip in sequence :(
				Surface surf = screen.CreateCompatibleSurface(width, height, true);
				surf.FillRect(new Rectangle(new Point(0, 0), surf.Size), Color.Black); // fill the surface with black

				while (!_quitflag) {
					while (sdl.Events.PollAndDelegate()) {} // handle events till the queue is empty

					try {
						surf.FillRect(new Rectangle(rand.Next(-300, width), rand.Next(-300, height), rand.Next(20, 300), rand.Next(20, 300)),
							Color.FromArgb(rand.Next(255), rand.Next(255), rand.Next(255)));
						surf.Blit(screen, new Rectangle(new Point(0, 0), screen.Size));
						screen.Flip();
					} catch (SurfaceLostException) {
						// if we are fullscreen and the user hits alt-tab we can get this, for this simple app we can ignore it
					}
				}
			} catch {
				sdl.Dispose(); // quit sdl so the window goes away, then handle the error...
				throw; // for this example we'll just throw it to the debugger
			}
		}

		private void SDL_KeyboardDown(int device, bool down, int scancode, Key key, Mod mod) {
			if (key == Key.K_ESCAPE || key == Key.K_q)
				_quitflag = true;
		}
		private void SDL_Quit() {
			_quitflag = true;
		}

		[STAThread]
		static void Main() {
			SDLExample example = new SDLExample();
			example.Go();
		}
	}
}
