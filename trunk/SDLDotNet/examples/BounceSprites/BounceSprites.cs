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
using System.Collections;

using SdlDotNet;
using SdlDotNet.Sprites;

namespace SdlDotNet.Examples 
{
	/// <summary>
	/// Demo of Bouncing Balls using Sprites. 
	/// The Bouncesprites will respond to Tick Events by spinning. 
	/// You can click on each sprite and move them around the 
	/// screen as well (MouseButton and MouseMotion events).
	/// </summary>
	class BounceSprites
	{
		#region Fields
		private Surface screen; //video screen
		private bool quitflag; //flag to tell the app to shutdown
		private SpriteCollection master = new SpriteCollection(); //holds all sprites
		private int width = 640; //screen width
		private int height = 480; //screen height
		private int maxBalls = 2; //number of balls to display
		private Random rand = new Random(); //randomizer
		private Surface background = new Surface("../../Data/background.png");
		#endregion Fields

		#region EventHandler Methods
		//Handles keyboard events. The 'Escape' and 'Q'keys will cause the app to exit
		private void OnKeyboardDown(object sender, KeyboardEventArgs e) 
		{
			if (e.Key == Key.Escape || e.Key == Key.Q)
			{
				quitflag = true;
			}
		}

		//The app will exit if the 'x' in the window is clicked
		private void OnQuit(object sender, QuitEventArgs e) 
		{
			quitflag = true;
		}

		RectangleCollection rects = new RectangleCollection();

		//A ticker is running to update the sprites constantly.
		//This method will fill the screen with black to clear it of the sprites.
		//Then it will Blit all of the sprites to the screen.
		//Then it will refresh the screen and display it.
		private void OnTick(object sender, TickEventArgs args)
		{	
			rects = screen.Blit(master);
			screen.Update(rects);	
			screen.Erase(rects, background);
		}
		#endregion EventHandler Methods

		#region Methods
		//Main program loop
		private void Go() 
		{
			//Set up screen
			screen = Video.SetVideoModeWindow(width, height, true);
			screen.Blit(background);
			Video.WindowCaption = "Bounce Sprites";

			//instantiate each marble
			for (int i = 0; i < this.maxBalls; i++)
			{
				//This loads the various images (provided by Moonfire) 
				// into a SurfaceCollection for animation
				SurfaceCollection marbleSurfaces = 
					new SurfaceCollection("../../Data/marble" + (i % 2 + 1), ".png");
				//Create a new Sprite at a random lcation on the screen
				BounceSprite bounceSprite = 
					new BounceSprite(marbleSurfaces,
					new Vector(rand.Next(screen.Rectangle.Left, screen.Rectangle.Right),
					rand.Next(screen.Rectangle.Top, screen.Rectangle.Bottom),
					0));
				//Add the sprite to the SpriteCollection
				master.Add(bounceSprite);
			}
			//The collection will respond to mouse button clicks, mouse movement and the ticker.
			master.EnableMouseButtonEvent();
			master.EnableMouseMotionEvent();
			master.EnableTickEvent();
      
			//These bind the events to the above methods.
			Events.KeyboardDown +=
				new KeyboardEventHandler(this.OnKeyboardDown);
			Events.Quit += new QuitEventHandler(this.OnQuit);
			Events.TickEvent += new TickEventHandler(this.OnTick);

			//Start the event ticker
			Events.StartTicker();

			try 
			{
				while (!quitflag) 
				{
					// handle events till the queue is empty
					while (Events.Poll()) 
					{} 
				}
				//Stop the ticker when the app quits.
				Events.StopTicker();
			} 
			catch 
			{
				throw;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		static void Main() 
		{
			BounceSprites bounce = new BounceSprites();
			bounce.Go();
		}
		#endregion Methods
	}
}
