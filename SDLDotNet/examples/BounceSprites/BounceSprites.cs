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
using System.Threading;
using System.Collections;

using SdlDotNet;
using SdlDotNet.Sprites;

namespace SdlDotNet.Examples 
{
	/// <summary>
	/// Demo of Bouncing Balls using Sprites
	/// </summary>
	class BounceSprites
	{
		private Surface screen;
		private bool quitflag;
		private SpriteCollection master = new SpriteCollection();
		private int width = 800;
		private int height = 600;
		private int maxBalls = 10;
		private Random rand = new Random();

		private void OnKeyboardDown(object sender, KeyboardEventArgs e) 
		{
			if (e.Key == Key.Escape || e.Key == Key.Q)
			{
				quitflag = true;
			}
		}
		private void OnQuit(object sender, QuitEventArgs e) 
		{
			quitflag = true;
		}

		private void OnTick(object sender, TickEventArgs args)
		{	
			screen.Fill(Color.Black);
			screen.Blit(master); 
			screen.Update();
		}

		/// <summary>
		/// 
		/// </summary>
		private void Go() 
		{
			screen = Video.SetVideoModeWindow(width, height, true);
			Video.WindowCaption = "Bounce Sprites";

			for (int i = 0; i < this.maxBalls; i++)
			{
				Thread.Sleep(10);

				SurfaceCollection marbleSurfaces = 
					new SurfaceCollection("../../Data/marble" + (rand.Next() % 2 + 1), ".png");
				BounceSprite bounceSprite = 
					new BounceSprite(marbleSurfaces,
					screen.Rectangle, 
					new Vector(rand.Next(screen.Rectangle.Left, screen.Rectangle.Right),
					rand.Next(screen.Rectangle.Top, screen.Rectangle.Bottom),
					0));
				master.Add(bounceSprite);
			}
			master.EnableMouseButtonEvent();
			master.EnableMouseMotionEvent();
			master.EnableTickEvent();
      
			Events.KeyboardDown +=
				new KeyboardEventHandler(this.OnKeyboardDown);
			Events.Quit += new QuitEventHandler(this.OnQuit);
			Events.TickEvent += new TickEventHandler(this.OnTick);
			Events.StartTicker();

			try 
			{
				while (!quitflag) 
				{
					// handle events till the queue is empty
					while (Events.Poll()) 
					{} 
				}
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
	}
}
