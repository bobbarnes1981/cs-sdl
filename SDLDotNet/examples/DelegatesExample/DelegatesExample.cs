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
	public class DelegatesExample 
	{
		private bool quitFlag;
		private bool musicFinishedFlag;
		private bool channelFinishedFlag;
		private int currentChannel;
		private int positionY = 200;
		
		/// <summary>
		/// 
		/// </summary>
		public DelegatesExample() 
		{
			quitFlag = false;
			musicFinishedFlag = false;
			channelFinishedFlag = false;
		}

		/// <summary>
		/// 
		/// </summary>
		public void Run() 
		{
			string filepath = @"../../";
			if (File.Exists("fard-two.ogg"))
			{
				filepath = "";
			}
			Font font;
			Surface text;
			string FontName = "Vera.ttf";
			int size = 12;
			int width = 640;
			int height = 480;
			Random rand = new Random();

			Music.EnableMusicFinishedCallback();
			
			Events.KeyboardDown += 
				new KeyboardEventHandler(this.KeyboardDown); 
			Events.Quit += new QuitEventHandler(this.Quit);
			Events.MusicFinished += new MusicFinishedEventHandler(this.MusicFinished);
			Events.ChannelFinished += new ChannelFinishedEventHandler(this.ChannelFinished);			

			try 
			{
				font = new Font(filepath + FontName, size);
				Music.Load(filepath + "fard-two.ogg");
				Music.Play(1);
				Sound sound = Mixer.Sound(filepath + "test.wav");
				Sound queuedSound = Mixer.Sound(filepath + "boing.wav");
				//Sound sound2 = Mixer.Sound(filepath + "test.wav");
				Channel channel = new Channel(0);
				Channel channel2 = new Channel(1);
				channel.EnableChannelFinishedCallback();
				//channel2.EnableChannelFinishedCallback();
				//channel.QueuedSound = queuedSound;
				channel.Play(sound);
				channel2.Play(sound);
				
				
				// set the video mode
				Surface screen = Video.SetVideoModeWindow(width, height, true); 
				WindowManager.Caption = "Delegates Example";
				Video.HideMouseCursor();

				while (!quitFlag) 
				{
					while (Events.PollAndDelegate()) 
					{
						// handle events till the queue is empty
						//sound.Stop();
					} 
					
					try 
					{
						if (musicFinishedFlag)
						{
							text = font.Render(
								"MusicChannelFinishedDelegate was called.", 
								Color.FromArgb(0, 254, 
								254,254));
							text.Blit(
								screen, 
								new Rectangle(new Point(100,300), 
								text.Size));
							screen.Flip();
						}
						if (channelFinishedFlag)
						{
							text = font.Render(
								"ChannelFinishedDelegate was called for channel " + currentChannel.ToString(), 
								Color.FromArgb(0, 154, 
								154,154));
							text.Blit(
								screen, 
								new Rectangle(new Point(100,positionY + 50 * currentChannel), 
								text.Size));
							screen.Flip();
						}
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
				//sdl.Dispose(); 
				// quit sdl so the window goes away, then handle the error...
				throw; // for this example we'll just throw it to the debugger
			}
		}

		private void KeyboardDown(
			object sender,
			KeyboardEventArgs e) {
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

		[STAThread]
		static void Main() {
			DelegatesExample delegatesExample = new DelegatesExample();
			delegatesExample.Run();
		}
		private void ChannelFinished(object sender, ChannelFinishedEventArgs e)
		{
			Console.WriteLine("channel: " + e.Channel.ToString());
			Console.WriteLine("Channel Finished");
			channelFinishedFlag = true;
			currentChannel = e.Channel;
		}

		private void MusicFinished(object sender, MusicFinishedEventArgs e)
		{
			Console.WriteLine("Music Finished");
			musicFinishedFlag = true;
		}
	}
}
