/*
 * $RCSfile$
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
using Tao.Sdl;

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
			//	Console.WriteLine("Font\t{0}\nStyle\t{1}\nSize\t{2}\nText\t{3}\n", FontName, Style, Size, Text);

			//			sdl.Events.Quit += new QuitEventHandler(SDL_Quit);
			//			sdl.Events.MouseButtonDown += new MouseButtonEventHandler(SDL_MouseButtonEvent);
			//			sdl.Events.KeyboardDown += new KeyboardEventHandler(SDL_KeyboardDown);

			
			Video video = Video.Instance;
			WindowManager wm = WindowManager.Instance;
			Ttf ttf = Ttf.Instance;
			font = new Font(FontName, Size);
			Surface screen = video.SetVideoModeWindow(width, height, true); 
			wm.Caption = "Font Example";
			Surface surf = screen.CreateCompatibleSurface(width, height, true);
			//fill the surface with black
			surf.FillRect(new Rectangle(new Point(0, 0), surf.Size), Color.Black); 

			while (!quitFlag) 
			{
				try
				{
					font.Style = (Style)styleArray[rand.Next(styleArray.Length)];
					text = font.RenderTextSolid(
						textArray[rand.Next(textArray.Length)], 
						new Sdl.SDL_Color((byte)rand.Next(255), 
						(byte)rand.Next(255),(byte)rand.Next(255)));

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

		private void SDL_Quit() 
		{
			quitFlag  = true;
			//mDone = true;
		}

		//		private void SDL_MouseButtonEvent(MouseButton button, bool down, int x, int y)
		//		{
		//			System.Drawing.Rectangle DestRect;
		//
		//			DestRect = new System.Drawing.Rectangle(new System.Drawing.Point(x, y), text.Size);
		//			text.Blit( screen, DestRect );
		//			screen.Flip();
		//		}

		//		private void SDL_KeyboardDown(int device, bool down, int scancode, Key key, Mod mod)
		//		{
		//			mDone = true;
		//		}

		[STAThread]
		static void Main() 
		{
			FontExample fontExample = new FontExample();
			fontExample.Run();
		}
	}
}
