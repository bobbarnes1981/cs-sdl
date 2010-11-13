using System;
using System.Drawing;
using SDLDotNet;

// Simple SDL.NET Example
// Just draws a bunch of rectangles to the screen, to quit hit 'Q' or Esc.

namespace Example {
	public class SDLNetExample {
		private bool _quitflag;

		public SDLNetExample() {
			_quitflag = false;
		}

		public void Go() {
			int width = 800;
			int height = 600;
			Random rand = new Random();
			
#if __MONO__
			string musicfile = "BZO-prym-guitar.ogg";
#else
			string musicfile = "..\\..\\BZO-prym-guitar.ogg";
#endif

			SDL sdl = new SDL(true); // create SDL object

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
			SDLNetExample example = new SDLNetExample();
			example.Go();
		}
	}
}
