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
using System.IO;
using SdlDotNet;
using Tao.Sdl;

// Simple SDL.NET Example
// Just draws a bunch of rectangles to the screen, to quit hit 'Q' or Esc.

namespace SdlDotNet.Examples 
{
	/// <summary>
	/// 
	/// </summary>
	public class JoystickExample 
	{
		private Point joystickPosition = new Point(100,100);
		private bool quitFlag;
		private int AxesCount = 0;
		private int ButtonCount = 0;
		private int HatCount = 0;
		private int BallCount = 0;
		private int width = 640;
		private int height = 480;
		private Joystick joystick;
		
		/// <summary>
		/// 
		/// </summary>
		public JoystickExample() 
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
			Events.JoystickAxisMotion += new JoystickAxisEventHandler(this.JoystickAxisChanged);
//			Events.JoystickBallMotion += new JoystickBallEventHandler(this.JoystickBallChanged);
//			Events.JoystickHatMotion += new JoystickHatEventHandler(this.JoystickHatChanged);
//			Events.JoystickButtonUp += new JoystickButtonEventHandler(this.JoystickButtonUpChanged);
//			Events.JoystickButtonDown += new JoystickButtonEventHandler(this.JoystickButtonDownChanged);

			try 
			{
				//Console.WriteLine(Joysticks.NumberOfJoysticks);
				joystick = Joysticks.OpenJoystick(0);
				//Console.WriteLine("NumberOfAxes: " + joystick.NumberOfAxes);
				//Console.WriteLine("NumberOfBalls: " + joystick.NumberOfBalls);
				//Console.WriteLine("NumberOfButtons: " + joystick.NumberOfButtons);
				//Console.WriteLine("NumberOfHats: " + joystick.NumberOfHats);
				Surface cursor = new Surface(filepath + "cursor.png");
				Surface Background = new Surface(filepath + "background.jpg");
				cursor.Transparent = true;
				// set the video mode
				Surface screen = Video.SetVideoModeWindow(width, height, true); 
				Video.WindowCaption = "Joystick Example";
				//video.HideMouseCursor();

				Surface surf = 
					screen.CreateCompatibleSurface(width, height, true);
				//fill the surface with black
				surf.Fill(new Rectangle(new Point(0, 0), surf.Size), Color.Black); 

				while (!quitFlag) 
				{
					while (Events.Poll(10) ) 
					{
						// handle events till the queue is empty
					} 
					
					try 
					{
						// Draw Background
						surf.Blit(Background,new Rectangle(new Point(0,0),Background.Size));
						surf.Blit(cursor,joystickPosition);

						// Draw frame to screen
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

		private void JoystickAxisChanged(object sender, JoystickAxisEventArgs e)
		{
			AxesCount++;
			if (e.AxisIndex == 0)
			{
				//joystickPosition.X = (int)(e.AxisValue * width);
				joystickPosition.X = (int)(Joysticks.OpenJoystick(e.Device).GetAxisPosition(JoystickAxes.Horizontal) * width);
			} else if (e.AxisIndex == 1)
			{
				//joystickPosition.Y = (int)(e.AxisValue * height);
				joystickPosition.Y = (int)(Joysticks.OpenJoystick(e.Device).GetAxisPosition(JoystickAxes.Vertical) * height);
			}
			Console.WriteLine("Joystick Axis Changed: " + AxesCount.ToString());
			Console.WriteLine("X: " + joystickPosition.X.ToString());
			Console.WriteLine("Y: " + joystickPosition.Y.ToString());
			Console.WriteLine("Axes: " + e.AxisIndex);
			Console.WriteLine("AxesValue: " + e.AxisValue);
		}

		private void JoystickBallChanged(object sender, JoystickBallEventArgs e)
		{
			BallCount++;
			Console.WriteLine("Joystick Ball Changed" + BallCount.ToString());
		}

		private void JoystickHatChanged(object sender, JoystickHatEventArgs e)
		{
			HatCount++;
			Console.WriteLine("Joystick Hat Changed" + HatCount.ToString());
		}

		private void JoystickButtonUpChanged(object sender, JoystickButtonEventArgs e)
		{
			
				Console.WriteLine("Joystick button up" + ButtonCount.ToString());
		}

		private void JoystickButtonDownChanged(object sender, JoystickButtonEventArgs e)
		{
			ButtonCount++;
				Console.WriteLine("Joystick button down" + ButtonCount.ToString());
		}


		[STAThread]
		static void Main() 
		{
			JoystickExample JoystickExample = new JoystickExample();
			JoystickExample.Run();
		}
	}
}
