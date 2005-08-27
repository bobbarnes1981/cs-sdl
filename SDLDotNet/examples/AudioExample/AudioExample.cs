/*
 * $RCSfile$
 * Copyright (C) 2005 Rob Loach (http://www.robloach.net)
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
using SdlDotNet.Sprites;

// SDL.NET Audio Example
// Simple example to demonstrate audio in SDL.NET.
// Click plays the sound, space changes the music, arrows change volume.

namespace SdlDotNet.Examples
{ 
	/// <summary>
	/// 
	/// </summary>
	public class AudioExample 
	{ 
		private const int width = 640; 
		private const int height = 480; 
		private Random rand = new Random(); 
		private Surface screen; 

		// Load the music and sound.
		private Music music1 = new Music("../../Data/mason2.mid");
		private Music music2 = new Music("../../Data/fard-two.ogg");
		private Sound boing = new Sound("../../Data/boing.wav");

		private Sprites.TextSprite instructions = new TextSprite(" ", new Font("../../Data/FreeSans.ttf", 20), Color.Red);

		/// <summary>
		/// 
		/// </summary>
		public AudioExample() 
		{ 
			// Setup events
			Events.Tick += new TickEventHandler(Events_TickEvent);
			Events.KeyboardDown += 
				new KeyboardEventHandler(Events_KeyboardDown); 
			Events.MouseButtonDown += 
				new MouseButtonEventHandler(Events_MouseButtonDown);

			Events.ChannelFinished +=
				new ChannelFinishedEventHandler(Events_ChannelFinished);
			Events.MusicFinished +=
				new MusicFinishedEventHandler(Events_MusicFinished);

			// Start up SDL
			screen = Video.SetVideoModeWindow(width, height); 
			Video.WindowCaption = "SdlDotNet - AudioExample";

			// Play the music and setup the queues.
			music1.Play();

			music1.QueuedMusic = music2; // Play music2 when music1 finishes.
			music2.QueuedMusic = music1; // Play music1 when music2 finishes.

			Music.EnableMusicFinishedCallback(); // Enable queueing

//				Sound queuedSound = Mixer.Sound(filepath + "boing.wav");
//				//Sound sound2 = Mixer.Sound(filepath + "test.wav");
//				Channel channel = new Channel(0);
//				//Channel channel2 = new Channel(1);
//				channel.EnableChannelFinishedCallback();
//				//channel2.EnableChannelFinishedCallback();
//				//channel.QueuedSound = queuedSound;
//				channel.Volume = 32;
//				channel.Play(sound);
//				//channel2.Play(sound);
          
			// Begin the SDL ticker
			Events.FPS = 50;

			/// TODO: Instructions and other descriptions.
			instructions.Text = "Do Stuff";
		} 

		/// <summary>
		/// 
		/// </summary>
		public void Run() 
		{ 
			Events.Run(); 
		} 

		private void Events_TickEvent(object sender, TickEventArgs e) 
		{ 
			screen.Fill(Color.Black);
			
			screen.Blit(instructions);

			screen.Flip(); 
		} 

		/// <summary>
		/// 
		/// </summary>
		public static void Main() 
		{ 
			AudioExample t = new AudioExample(); 
			t.Run(); 
		} 

		private void Events_KeyboardDown(object sender, KeyboardEventArgs e) 
		{ 
			switch(e.Key)
			{ 
				case Key.Escape:
					Events.QuitApp(); 
					break; 
				case Key.Space: 

					// Switch the music 
					Music.Fadeout(1500);
 
					// The next music sample plays because queuing is enabled.

					break; 

				case Key.UpArrow: 
					Music.Volume += 20; 
					break; 
				case Key.DownArrow: 
					Music.Volume -= 20;
					break; 
				case Key.Return:
					boing.Play();
					break;
			} 
		} 

		private void Events_MouseButtonDown(object sender, MouseButtonEventArgs e) 
		{ 
			switch(e.Button) 
			{ 
				case MouseButton.PrimaryButton: 
					// Play on left side
					boing.Play().SetPanning(205, 50); 
					break; 
				case MouseButton.SecondaryButton: 
					// Play on right side 
					boing.Play().SetPanning(50, 205);
					break;
			} 
		}

		private void Events_ChannelFinished(object sender, ChannelFinishedEventArgs e)
		{
			Console.WriteLine("Channel: " + e.Channel.ToString() + " Finished");
		}

		private void Events_MusicFinished(object sender, MusicFinishedEventArgs e)
		{
			Console.WriteLine("Music Finished");
		}
	} 
} 
