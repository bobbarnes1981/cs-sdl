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
			
			Video video = Video.Instance;
			WindowManager wm = WindowManager.Instance;
			Joysticks joysticks = Joysticks.Instance;
			Events events = Events.Instance;
			
			events.KeyboardDown += 
				new KeyboardEventHandler(this.KeyboardDown); 
			events.Quit += new QuitEventHandler(this.Quit);	
			events.JoystickAxisMotion += new JoystickAxisEventHandler(this.JoystickAxisChanged);
			events.JoystickHorizontalAxisMotion += 
				new JoystickAxisEventHandler(this.JoystickHorizontalAxisChanged);
			events.JoystickVerticalAxisMotion += 
				new JoystickAxisEventHandler(this.JoystickVerticalAxisChanged);
			events.JoystickBallMotion += new JoystickBallEventHandler(this.JoystickBallChanged);
			events.JoystickHatMotion += new JoystickHatEventHandler(this.JoystickHatChanged);
			events.JoystickButtonUp += new JoystickButtonEventHandler(this.JoystickButtonUpChanged);
			events.JoystickButtonDown += new JoystickButtonEventHandler(this.JoystickButtonDownChanged);

			try 
			{
				Console.WriteLine(joysticks.NumberOfJoysticks);
				Joystick joystick = joysticks.OpenJoystick(0);
				Console.WriteLine("NumberOfAxes: " + joystick.NumberOfAxes);
				Console.WriteLine("NumberOfBalls: " + joystick.NumberOfBalls);
				Console.WriteLine("NumberOfButtons: " + joystick.NumberOfButtons);
				Console.WriteLine("NumberOfHats: " + joystick.NumberOfHats);
					Image cursor = new Image(filepath + "cursor.png");
				Image Background = new Image(filepath + "background.jpg");
				cursor.Transparent = true;
				// set the video mode
				Surface screen = video.SetVideoModeWindow(width, height, true); 
				wm.Caption = "Joystick Example";
				//video.HideMouseCursor();

				Surface surf = 
					screen.CreateCompatibleSurface(width, height, true);
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
						// Draw Background
						Background.Draw(surf,new Rectangle(new Point(0,0),Background.Size));
						cursor.Draw(surf,new Rectangle(joystickPosition, screen.Size));

						// Draw frame to screen
						surf.Blit(screen, new Rectangle(new Point(0, 0), screen.Size));
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
			Console.WriteLine("Joystick Axis Changed: " + AxesCount.ToString());
			Console.WriteLine("Axes: " + e.AxisIndex);
			Console.WriteLine("AxesValue: " + e.AxisValue);
		}

		private void JoystickHorizontalAxisChanged(object sender, JoystickAxisEventArgs e)
		{
			AxesCount++;
			Console.WriteLine("Joystick Horizontal Axis Changed: " + AxesCount.ToString());
			Console.WriteLine("Axes: " + e.AxisIndex);
			Console.WriteLine("AxesValue: " + e.AxisValue);
			joystickPosition.X = (int)(e.AxisValue * width);
			Console.WriteLine("X: " + joystickPosition.X.ToString());
		}

		private void JoystickVerticalAxisChanged(object sender, JoystickAxisEventArgs e)
		{
			AxesCount++;
			Console.WriteLine("Joystick Vertical Axis Changed: " + AxesCount.ToString());
			Console.WriteLine("Axes: " + e.AxisIndex);
			Console.WriteLine("AxesValue: " + e.AxisValue);
			joystickPosition.Y = (int)(e.AxisValue * height);
			Console.WriteLine("Y: " + joystickPosition.Y.ToString());
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
