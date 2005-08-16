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
using System.IO;
using System.Globalization;

using SdlDotNet;

namespace SdlDotNet.Examples
{
	class FontExample
	{
		Surface text;
		bool quitFlag;
		string FontName = "FreeSans.ttf";
		int size = 12;
		int width = 640;
		int height = 480;
//		bool musicFinishedFlag;
//		bool channelFinishedFlag;
//		private int currentChannel;
//		private int positionY = 200;

		string[] textArray = {"Hello World!","This is a test", "FontExample"};
		int[] styleArray = {0, 1, 2, 4};

		/// <summary>
		/// 
		/// </summary>
		public void Run()
		{
			string filepath = @"../../";
			if (File.Exists("FreeSans.ttf"))
			{
				filepath = @"./";
			}

			Font font;
			Random rand = new Random();

			
			Events.KeyboardDown += new KeyboardEventHandler(this.KeyboardDown);
			Events.KeyboardUp += 
				new KeyboardEventHandler(this.KeyboardUp);
			Events.Quit += new QuitEventHandler(this.Quit);
//			Events.MusicFinished += new MusicFinishedEventHandler(this.MusicFinished);
//			Events.ChannelFinished += new ChannelFinishedEventHandler(this.ChannelFinished);	
			//timerDelegate = new Sdl.SDL_TimerCallback(this.TimerCall);
			//int result2 = Sdl.SDL_SetTimer(500, timerDelegate);

			font = new Font(filepath + FontName, size);
//			Mixer.Music.Load(filepath + "fard-two.ogg");
//			Mixer.Music.Volume = 128;
//			Mixer.Music.EnableMusicFinishedCallback();
//			try
//			{
//				Mixer.Music.Play(1);
//				Sound sound = Mixer.Sound(filepath + "test.wav");
//				//Sound queuedSound = Mixer.Sound(filepath + "boing.wav");
//				//Sound sound2 = Mixer.Sound(filepath + "test.wav");
//				Channel channel = new Channel(0);
//				//Channel channel2 = new Channel(1);
//				channel.EnableChannelFinishedCallback();
//				//channel2.EnableChannelFinishedCallback();
//				//channel.QueuedSound = queuedSound;
//				channel.Volume = 32;
//				channel.Play(sound);
//				//channel2.Play(sound);
//			}
//			catch (DivideByZeroException)
//			{
//				// Linux audio problem
//			}
//			catch (SdlException)
//			{
//				// Linux audio problem
//			}

			Surface screen = Video.SetVideoModeWindow(width, height, true); 
			Video.WindowCaption = "SdlDotNet - Font Example";
			//Video.Mouse.ShowCursor(false);
			System.Drawing.Text.FontCollection installedFonts = FontSystem.SystemFontNames;
			Console.WriteLine("Installed Fonts: " + installedFonts.ToString());
			Console.WriteLine("Installed Fonts: " + installedFonts.Families[0].ToString());

			Surface surf = screen.CreateCompatibleSurface(width, height, true);
			//fill the surface with black
			surf.Fill(new Rectangle(new Point(0, 0), surf.Size), Color.Black); 

			Console.WriteLine("Bpp: " + Video.Screen.BitsPerPixel);

			SdlEventArgs[] events = new SdlEventArgs[3];
			events[0] = new KeyboardEventArgs(Key.Space, true);
			events[1] = new KeyboardEventArgs(Key.Space, false);
			events[2] = new KeyboardEventArgs(Key.Space, true);
			Events.Add(events);
			//Events.PushUserEvent(new MusicFinishedEventArgs());

			//SdlEventArgs[] eventArrayDown = Events.Peek(EventMask.KeyDown, 10);
			//SdlEventArgs[] eventArrayUp = Events.Peek(EventMask.KeyUp, 10);
			//	SdlEventArgs[] eventArrayMusic = Events.Peek(EventMask.AllEvents, 10);

			while (!quitFlag) 
			{
				while (Events.Poll()) 
				{
					// handle events till the queue is empty
				}
				try
				{
					font.Style = (Styles)styleArray[rand.Next(styleArray.Length)];
					Console.WriteLine("Style: " + font.Style);
					Console.WriteLine(font.Bold);
					text = font.Render(
						textArray[rand.Next(textArray.Length)], 
						Color.FromArgb(0, (byte)rand.Next(255), 
						(byte)rand.Next(255),(byte)rand.Next(255)));

					if (Video.Mouse.IsButtonPressed(MouseButton.PrimaryButton))
					{
						Console.WriteLine("Primary button is pressed");
					}
					if (Video.Mouse.IsButtonPressed(MouseButton.SecondaryButton))
					{
						Console.WriteLine("Secondary button is pressed");
					}
					if (Keyboard.IsKeyPressed(Key.Space))
					{
						Console.WriteLine("space bar is currently pressed");
					}
					Console.WriteLine("Pos: " + Video.Mouse.MousePosition.ToString());
					Console.WriteLine("Change: " + Video.Mouse.MousePositionChange.ToString());
					Console.WriteLine("Has Mousefocus: " + Video.Mouse.HasMouseFocus);
					switch (rand.Next(4))
					{
						case 1:
							text.FlipVertical();
							break;
						case 2:
							text.FlipHorizontal();
							break;
						case 3:
							text.Rotate(90);
							break;
						default:
							break;
					}
					screen.Blit(
						text, 
						new Rectangle(new Point(rand.Next(width - 100),rand.Next(height - 100)), 
						text.Size));
					screen.Flip();
					
//					if (musicFinishedFlag)
//					{
//						text = font.Render(
//							"MusicChannelFinishedDelegate was called.", 
//							Color.FromArgb(0, 254, 
//							254,254));
//						screen.Blit(
//							text, 
//							new Rectangle(new Point(100,100), 
//							text.Size));
//						screen.Flip();
//						musicFinishedFlag = false;
//					} 
//					else if (channelFinishedFlag)
//					{
//						text = font.Render(
//							"ChannelFinishedDelegate was called for channel " + currentChannel.ToString(CultureInfo.CurrentCulture), 
//							Color.FromArgb(0, 154, 
//							154,154));
//						screen.Blit(
//							text, 
//							new Rectangle(new Point(100,positionY + 50 * currentChannel), 
//							text.Size));
//						screen.Flip();
//						channelFinishedFlag = false;
//						positionY += 20;
//					}
					Thread.Sleep(1000);
					text.Dispose();
					//GC.Collect();
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
			if (e.Key == Key.Space)
			{
				Console.WriteLine("Space bar was pressed");
			}
		}

		private void KeyboardUp(
			object sender,
			KeyboardEventArgs e) 
		{
			if (e.Key == Key.Space)
			{
				Console.WriteLine("Space bar was released");
			}
		}

		private void Quit(object sender, QuitEventArgs e) 
		{
			quitFlag  = true;
		}

//		private void ChannelFinished(object sender, ChannelFinishedEventArgs e)
//		{
//			//Console.WriteLine("channel: " + e.Channel.ToString());
//			//Console.WriteLine("Channel Finished");
//			channelFinishedFlag = true;
//			currentChannel = e.Channel;
//		}
//
//		private void MusicFinished(object sender, MusicFinishedEventArgs e)
//		{
//			//Console.WriteLine("Music Finished");
//			musicFinishedFlag = true;
//		}

		[STAThread]
		static void Main() 
		{
			FontExample fontExample = new FontExample();
			fontExample.Run();
		}
	}
}
