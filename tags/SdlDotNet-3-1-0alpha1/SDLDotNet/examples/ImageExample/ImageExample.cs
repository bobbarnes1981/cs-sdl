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
using System.IO;
using SdlDotNet;
using System.Runtime.InteropServices;


namespace SdlDotNet.Examples
{
	/// <summary>
	/// 
	/// </summary>
	public class ImageExample
	{

		/// <summary>
		/// Private field. Indicated if mainloop is to be continued.
		/// </summary>
		private bool quitFlag;
		Point position = new Point(100,100);
		private int AxesCount = 0;
		private int ButtonCount = 0;
		private int HatCount = 0;
		private int BallCount = 0;
		private Joystick joystick;
		int width = 640;
		int height = 480;
		MouseMotionEventHandler MouseMotionHandler;
		JoystickAxisEventHandler JoystickAxisHandler;

		/// <summary>
		/// 
		/// </summary>
		public ImageExample() 
		{
			quitFlag = false;
		}

		/// <summary>
		/// 
		/// </summary>
		public void Run() 
		{
			Random rand = new Random();
			string imagepath;
			
			
			Events.KeyboardDown += 
				new KeyboardEventHandler(this.KeyboardDown); 
			Events.Quit += new QuitEventHandler(this.Quit);
			MouseMotionHandler = new MouseMotionEventHandler(this.MouseMotion);
			JoystickAxisHandler = new JoystickAxisEventHandler(this.JoystickAxisChanged);
			Events.MouseMotion += MouseMotionHandler;
			try 
			{
				string filepath = @"../../";
				if (File.Exists("fard-two.ogg"))
				{
					filepath = "";
				}
				SdlButton button = new SdlButton(200, 200, 75, 50, Color.Green, "Hello");
				SdlTextBox textBox = new SdlTextBox(300, 300, 300);
				button.Click +=new SdlButtonEventHandler(button_Click);
				
				Surface screen = Video.SetVideoModeWindow(width, height, true);
				Video.WindowCaption = "Surface Example";
				Video.Mouse.ShowCursor(false); // hide the cursor

				Surface surf = 
					screen.CreateCompatibleSurface(width, height, true);
				surf.Fill(new Rectangle(new Point(0, 0), 
					surf.Size), System.Drawing.Color.Black); 

				imagepath = @"images/";

				Surface Background = new Surface(filepath + imagepath + "background.png");

				Surface sdlimg = new Surface(filepath + imagepath +  "sdlimage.png");
				sdlimg.AlphaFlags = Alphas.RleEncoded| Alphas.SourceAlphaBlending;
				sdlimg.AlphaValue = 100;

				Surface Cursor = new Surface(filepath + imagepath +  "cursor.png");
				Cursor.Transparent = true;

				SurfaceCollection Jeep = new SurfaceCollection();

				for (int j = 1; j <= 16;j++) 
				{
					Jeep.Add(filepath + imagepath + @"jeep/jeep" + j.ToString() +".gif");
					Jeep[Jeep.Count-1].Transparent = true;
				}

				Surface Tree = new Surface(filepath + imagepath + "Tree.bmp");

				Tree.TransparentColor = System.Drawing.Color.Magenta;
				Tree.Transparent = true;

				Tree.AlphaFlags = Alphas.RleEncoded | Alphas.SourceAlphaBlending;
				Tree.AlphaValue = 0;

				int JeepFrame = 0;
				Mixer.Music.Load(filepath + "fard-two.ogg");
				Mixer.Music.Play(-1);

				while (!quitFlag) 
				{
					while (Events.Poll()) 
					{
						// handle events till the queue is empty
					}

					try 
					{
						// Draw Background
						surf.Blit(Background,new Rectangle(new Point(0,0),Background.Size));
						
						// Draw tree
						if (Tree.AlphaValue == 255) 
						{
							Tree.AlphaValue = 0;
						}
						Tree.AlphaValue++;

						surf.Blit(Tree, new Rectangle(0,0,20,20));

						// Draw Jeep;
						if (JeepFrame == 15) 
						{
							JeepFrame = 0;
						}
						JeepFrame++;

						surf.Blit(Jeep[JeepFrame], new Rectangle(new Point(100,100),Jeep[JeepFrame].Size));

						// Draw Textbox
						surf.Blit(sdlimg,new Rectangle(new Point(230,440),Background.Size));
						button.Draw(surf);
						
						textBox.Draw(surf);
						surf.Blit(Cursor,new Rectangle(position, screen.Size));
						

						// Draw frame to screen
						screen.Blit(surf, new Rectangle(new Point(0, 0), screen.Size));
						screen.Flip();
					} 
					catch (SurfaceLostException) 
					{
						// if we are fullscreen and the user hits alt-tab 
						// we can get this, for this simple app we can ignore it
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

		private void MouseMotion(
			object sender, 
			MouseMotionEventArgs e)
		{
			position.X = e.X;
			position.Y = e.Y;
		}

		private void KeyboardDown(object sender, KeyboardEventArgs e) 
		{
			if (e.Key == Key.Escape ||
				e.Key == Key.Q)
			{
				quitFlag = true;
			}
			else if (e.Key == Key.J)
			{
				Events.MouseMotion -= MouseMotionHandler;
				Events.JoystickAxisMotion += JoystickAxisHandler;
				joystick = Joysticks.OpenJoystick(0);
			}
			else if (e.Key == Key.M)
			{
				Events.MouseMotion += MouseMotionHandler;
				Events.JoystickAxisMotion -= JoystickAxisHandler;
			}
		}

		/// <summary>
		/// Quits the application
		/// </summary>
		private void Quit(object sender, QuitEventArgs e) 
		{
			quitFlag = true;
		}

		private void button_Click(object source, SdlButtonEventArgs e)
		{
			Console.WriteLine("Button was clicked");
		}

		private void JoystickAxisChanged(object sender, JoystickAxisEventArgs e)
		{
			AxesCount++;
			if (e.AxisIndex == 0)
			{
				//joystickPosition.X = (int)(e.AxisValue * width);
				position.X = (int)(Joysticks.OpenJoystick(e.Device).GetAxisPosition(JoystickAxis.Horizontal) * width);
			} 
			else if (e.AxisIndex == 1)
			{
				//joystickPosition.Y = (int)(e.AxisValue * height);
				position.Y = (int)(Joysticks.OpenJoystick(e.Device).GetAxisPosition(JoystickAxis.Vertical) * height);
			}
			Console.WriteLine("Joystick Axis Changed: " + AxesCount.ToString());
			Console.WriteLine("X: " + position.X.ToString());
			Console.WriteLine("Y: " + position.Y.ToString());
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
