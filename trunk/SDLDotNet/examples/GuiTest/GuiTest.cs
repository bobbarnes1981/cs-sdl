/*
 * $RCSfile$
 * Copyright (C) 2005 David Hudson (jendave@yahoo.com)
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

namespace SdlDotNet.Examples 
{
	/// <summary>
	/// 
	/// </summary>
	public class GuiTest
	{
		private bool quitFlag;
		private int width = 640;
		private int height = 480;
		Point MousePos = new Point(100,100);
		
		/// <summary>
		/// 
		/// </summary>
		public GuiTest() 
		{
			quitFlag = false;
		}

		/// <summary>
		/// 
		/// </summary>
		public void Run() 
		{
			string filepath = @"../../";
			if (File.Exists("cursor.png"))
			{
				filepath = @"./";
			}

			Random rand = new Random();
			
			Events.KeyboardDown += 
				new KeyboardEventHandler(this.KeyboardDown); 
			Events.Quit += new QuitEventHandler(this.Quit);	
			Events.MouseMotion += 
				new MouseMotionEventHandler(this.MouseMotion);



			try 
			{
				//Console.WriteLine(Joysticks.NumberOfJoysticks);
				//Joystick joystick = Joysticks.OpenJoystick(0);
				//Console.WriteLine("NumberOfAxes: " + joystick.NumberOfAxes);
				//Console.WriteLine("NumberOfBalls: " + joystick.NumberOfBalls);
				//Console.WriteLine("NumberOfButtons: " + joystick.NumberOfButtons);
				//Console.WriteLine("NumberOfHats: " + joystick.NumberOfHats);
				Surface cursor = new Surface(filepath + "cursor.png");
				Surface Background = new Surface(filepath + "background.jpg");
				cursor.Transparent = true;
				// set the video mode
				Surface screen = Video.SetVideoModeWindow(width, height, true); 
				Video.WindowCaption = "Gui Test";
				//video.HideMouseCursor();

				Surface surf = 
					screen.CreateCompatibleSurface(width, height, true);
				//fill the surface with black
				surf.Fill(new Rectangle(new Point(0, 0), surf.Size), Color.Black); 

				SdlButton button = new SdlButton(100, 100, 200, 100, Color.Green, "Hello");
				SdlTextBox textBox = new SdlTextBox(300, 300, 300);
				

				button.Click +=new SdlButtonEventHandler(button_Click);
				
				while (!quitFlag) 
				{
					while (Events.Poll()) 
					{
						// handle events till the queue is empty
					} 
					
					try 
					{
						surf.Blit(Background,new Point(0,0));
						button.Draw(surf);
						
						textBox.Draw(surf);
						screen.Blit(surf, new Point(0, 0));
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
		private void MouseMotion(
			object sender, 
			MouseMotionEventArgs e)
		{
			MousePos.X = e.X;
			MousePos.Y = e.Y;
		}


		[STAThread]
		static void Main() 
		{
			GuiTest guiTest = new GuiTest();
			guiTest.Run();
		}

		private void button_Click(object source, SdlButtonEventArgs e)
		{
			Console.WriteLine("Button was clicked");
		}
	}
}
