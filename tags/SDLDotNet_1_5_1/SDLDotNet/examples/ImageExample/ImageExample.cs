/*
 * $RCSfile$
 * Copyright (C) 2003 Klavs Martens
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
using SDLDotNet.Image;


namespace SDLDotNet.Image.Example
{
	public class ImageExample
	{

		/// <summary>
		/// Private field. Indicated if mainloop is to be continued.
		/// </summary>
		private bool Continue;
		Point MousePos = new Point(100,100);


		public ImageExample() 
		{
			Continue = true;
		}

		public void Go() 
		{
			int width = 640;
			int height = 480;
			Random rand = new Random();
			string imagepath;

			SDL sdl = SDL.Instance; // get SDL object

			sdl.Events.KeyboardDown += new KeyboardEventHandler(this.SDL_KeyboardDown); // register event handlers
			sdl.Events.MouseMotion += new MouseMotionEventHandler(this.SDL_MouseMotion);
			
			sdl.Events.Quit += new QuitEventHandler(this.SDL_Quit);

			try 
			{

				//Surface screen = sdl.Video.SetVideoMode(width, height); // set the video mode
				Surface screen = sdl.Video.SetVideoModeWindow(width, height, true);
				sdl.Video.HideMouseCursor(); // hide the cursor

				// create a surface to draw to...we cant draw rectangles directly to the hardware back buffer because
				// different video cards have different numbers of back buffers that flip in sequence :(
				Surface surf = screen.CreateCompatibleSurface(width, height, true);
				surf.FillRect(new Rectangle(new Point(0, 0), surf.Size), System.Drawing.Color.Black); // fill the surface with black

				imagepath = @"images/";

				SDLImage Background = new SDLImage(imagepath + "background.tga");

				SDLImage sdlimg = new SDLImage(imagepath +  "sdlimage.png");
				sdlimg.AlphaFlags = Alpha.RLEAccel | Alpha.Source;
				sdlimg.AlphaValue = 100;

				SDLImage Cursor = new SDLImage(imagepath +  "cursor.png");
				Cursor.Transparent = true;


				SDLImageList Jeep = new SDLImageList();

				for (int j = 1; j <= 16;j++) 
				{
					Jeep.Images.Add(imagepath + @"jeep/jeep" + j.ToString() +".gif");
					Jeep.Images[Jeep.Images.Count-1].Transparent = true;
				}


				SDLImageList ImageList = new SDLImageList();


				SDLImage Tree = new SDLImage(imagepath + "Tree.bmp");

				Tree.TransparentColor = System.Drawing.Color.Magenta;
				Tree.Transparent = true;

				Tree.AlphaFlags = Alpha.RLEAccel | Alpha.Source;
				Tree.AlphaValue = 0;


				int JeepFrame = 0;

				while (Continue) 
				{
					while (sdl.Events.PollAndDelegate()) {} // handle events till the queue is empty

					try 
					{


						// Draw Background
						Background.Draw(surf,new Rectangle(new Point(0,0),Background.Size));
						
						// Draw tree
						if (Tree.AlphaValue == 255) Tree.AlphaValue = 0;
						Tree.AlphaValue++;

						Tree.Draw(surf,new Rectangle(0,0,20,20));

						// Draw Jeep;
						if (JeepFrame == 15) JeepFrame = 0;
						JeepFrame++;

						Jeep.Images[JeepFrame].Draw(surf, new Rectangle(new Point(100,100),Jeep.Images[JeepFrame].Size));


						// Draw Textbox
						sdlimg.Draw(surf,new Rectangle(new Point(230,440),Background.Size));

						Cursor.Draw(surf,new Rectangle(MousePos, screen.Size));

						// Draw frame to screen
						surf.Blit(screen, new Rectangle(new Point(0, 0), screen.Size));
						screen.Flip();
					} 
					catch (SurfaceLostException) 
					{
						// if we are fullscreen and the user hits alt-tab we can get this, for this simple app we can ignore it
					}
				}
			} 
			catch 
			{
				sdl.Dispose(); // quit sdl so the window goes away, then handle the error...
				throw; // for this example we'll just throw it to the debugger
			}
		}


		/// <summary>
		/// Response to keybord messages
		/// </summary>
		private void SDL_KeyboardDown(int device, bool down, int scancode, Key key, Mod mod) 
		{
			if (key == Key.K_ESCAPE || key == Key.K_q)	Continue = false;
		}

		public void SDL_MouseMotion(MouseButtonState state, int x, int y, int relx, int rely)
		{
			MousePos.X = x;
			MousePos.Y = y;
		}


		/// <summary>
		/// Quits the application
		/// </summary>
		private void SDL_Quit() 
		{
			Continue = false;
		}

		/// <summary>
		/// Application EntryPoint.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			new ImageExample().Go();
		}

	}
}
