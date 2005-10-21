/*
 * $RCSfile$
 * Copyright (C) 2004 David Hudson (jendave@yahoo.com)
 * Copyright (C) 2003 Lucas Maloney
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
using System.Threading;
using SdlDotNet;

namespace SdlDotNet.Examples
{
	public class FontExample
	{

		Surface text;
		bool quitFlag = false;
		string FontName = "Vera.ttf";
		int Size = 12;
		int width = 640;
		int height = 480;
			

		string[] textArray = {"Hello World!","This is a test", "FontExample"};
		int[] styleArray = {0, 1, 2, 4};


		public void Run()
		{

			Font font;
			Random rand = new Random();

			Video video = Video.Instance;
			WindowManager wm = WindowManager.Instance;
			Ttf ttf = Ttf.Instance;
			Events events = Events.Instance;

			events.KeyboardDown += new KeyboardEventHandler(this.KeyboardDown);
			events.Quit += new QuitEventHandler(this.Quit);

			font = new Font(FontName, Size);
			Surface screen = video.SetVideoModeWindow(width, height, true); 
			wm.Caption = "Font Example";
			video.HideMouseCursor();

			Surface surf = screen.CreateCompatibleSurface(width, height, true);
			//fill the surface with black
			surf.FillRectangle(new Rectangle(new Point(0, 0), surf.Size), Color.Black); 

			while (!quitFlag) 
			{
				while (events.PollAndDelegate()) 
				{
					// handle events till the queue is empty
				}
				try
				{
					font.Style = (Styles)styleArray[rand.Next(styleArray.Length)];
					text = font.RenderTextSolid(
						textArray[rand.Next(textArray.Length)], 
						Color.FromArgb(0, (byte)rand.Next(255), 
						(byte)rand.Next(255),(byte)rand.Next(255)));

					if (rand.Next(2) == 1)
					{
						text.FlipVertical();
					}

					text.Blit(
						screen, 
						new Rectangle(new Point(rand.Next(width - 100),rand.Next(height - 100)), 
						text.Size));
					screen.Flip();
					Thread.Sleep(100);
					//				while (!mDone)
					//				{
					//					sdl.Events.WaitAndDelegate();
					//				}
				} 
				catch 
				{
					//sdl.Dispose();
					throw;
				}
			}
		}

		private void KeyboardDown(object sender, KeyboardEventArgs e) 
		{
			if (e.Key == Key.Escape ||
				e.Key == Key.Q)
			{
				quitFlag = true;
			}
		}

		private void Quit(object sender, QuitEventArgs e) 
		{
			quitFlag  = true;
		}

		[STAThread]
		static void Main() 
		{
			FontExample fontExample = new FontExample();
			fontExample.Run();
		}
	}
}