/*
 * $RCSfile$
 * Copyright (C) 2004 David Hudson (jendave@yahoo.com)
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
using SdlDotNet;

namespace SdlDotNet.Examples
{
	public class ImageExample
	{

		/// <summary>
		/// Private field. Indicated if mainloop is to be continued.
		/// </summary>
		private bool quitFlag;
		Point MousePos = new Point(100,100);


		public ImageExample() 
		{
			quitFlag = false;
		}

		public void Run() 
		{
			int width = 640;
			int height = 480;
			Random rand = new Random();
			string imagepath;

			

			Video video = Video.Instance;
			WindowManager wm = WindowManager.Instance;
			Events events = Events.Instance;
			
			events.KeyboardDown += 
				new KeyboardEventHandler(this.KeyboardDown); 
			events.Quit += new QuitEventHandler(this.Quit);
			events.MouseMotion += 
				new MouseMotionEventHandler(this.MouseMotion);
			try 
			{
				Surface screen = video.SetVideoModeWindow(width, height, true);
				wm.Caption = "Image Example";
				video.HideMouseCursor(); // hide the cursor

				Surface surf = 
					screen.CreateCompatibleSurface(width, height, true);
				surf.FillRectangle(new Rectangle(new Point(0, 0), 
					surf.Size), System.Drawing.Color.Black); 

				imagepath = @"images/";

				Image Background = new Image(imagepath + "background.tga");

				Image sdlimg = new Image(imagepath +  "sdlimage.png");
				sdlimg.AlphaFlags = Alphas.RleEncoded| Alphas.SourceAlphaBlending;
				sdlimg.AlphaValue = 100;

				Image Cursor = new Image(imagepath +  "cursor.png");
				Cursor.Transparent = true;

				ImageList Jeep = new ImageList();

				for (int j = 1; j <= 16;j++) 
				{
					Jeep.Images.Add(imagepath + @"jeep/jeep" + j.ToString() +".gif");
					Jeep.Images[Jeep.Images.Count-1].Transparent = true;
				}


				ImageList ImageList = new ImageList();

				Image Tree = new Image(imagepath + "Tree.bmp");

				Tree.TransparentColor = System.Drawing.Color.Magenta;
				Tree.Transparent = true;

				Tree.AlphaFlags = Alphas.RleEncoded | Alphas.SourceAlphaBlending;
				Tree.AlphaValue = 0;

				int JeepFrame = 0;

				while (!quitFlag) 
				{
					while (events.PollAndDelegate()) 
					{
						// handle events till the queue is empty
					}

					try 
					{
						// Draw Background
						Background.Draw(surf,new Rectangle(new Point(0,0),Background.Size));
						
						// Draw tree
						if (Tree.AlphaValue == 255) 
						{
							Tree.AlphaValue = 0;
						}
						Tree.AlphaValue++;

						Tree.Draw(surf,new Rectangle(0,0,20,20));

						// Draw Jeep;
						if (JeepFrame == 15) 
						{
							JeepFrame = 0;
						}
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
				//SdlCore.Instance.Dispose(); 
				// quit sdl so the window goes away, then handle the error...
				throw; // for this example we'll just throw it to the debugger
			}
		}

		public void MouseMotion(
			object sender, 
			MouseMotionEventArgs e)
		{
			MousePos.X = e.X;
			MousePos.Y = e.Y;
		}

		private void KeyboardDown(object sender, KeyboardEventArgs e) 
		{
			if (e.Key == Keys.Escape ||
				e.Key == Keys.q)
			{
				quitFlag = true;
			}
		}

		/// <summary>
		/// Quits the application
		/// </summary>
		private void Quit(object sender, QuitEventArgs e) 
		{
			quitFlag = true;
		}

		/// <summary>
		/// Application EntryPoint.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			ImageExample imageExample = new ImageExample();
			imageExample.Run();
		}

	}
}
