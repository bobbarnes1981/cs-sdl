/*
 * $RCSfile$
 * Copyright (C) 2004 D. R. E. Moonfire (d.moonfire@mfgames.com)
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

using SdlDotNet.Sprites;
using MFGames.Sdl.Gui;
using SdlDotNet;
using System;
using System.Collections;
using System.Drawing;
using System.Threading;

namespace MFGames.Sdl.Demos
{
	/// <summary>
	/// The SdlDemo is a general testbed and display of various features
	/// in the MFGames.Sdl library. It includes animated sprites and
	/// movement. To run, it currently assumes that the current
	/// directory has a "test/" directory underneath it containing
	/// various images.
	/// </summary>
	public class SdlDemo
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="args"></param>
		public static void Main(string [] args)
		{
			// Create the demo application
			SdlDemo demo = new SdlDemo();
			demo.Start();
		}

		/// <summary>
		/// 
		/// </summary>
		public void Start()
		{
			sdlDemo = this;

			// Start up the SDL
			Video.WindowCaption = "SDL.NET Demonstration";
      
			Events.KeyboardDown +=
				new KeyboardEventHandler(this.OnKeyboardDown);
			Events.Quit += new QuitEventHandler(this.OnQuit);
			Events.TickEvent += new TickEventHandler(this.OnTick);

			// Create the screen
			int width = 800;
			int height = 600;

			screen = Video.SetVideoModeWindow(width, height, true);

			// Set up the master sprite container
			SetupGui();

			// Load demos
			LoadDemos();

			// Start up the ticker (and animation)
			Events.TicksPerSecond = 15;
			Events.StartTicker();

			// Loop until the system indicates it should stop
			Console.WriteLine("Welcome to the SDL.NET Demo!");

			while (running)
			{
				// Sleep a little (60 fps)
				while(Events.Poll())
				{}
				Thread.Sleep(1000 / 60);
			}

			// Stop the ticker and the current demo
			SwitchDemo(-1);
			Events.StopTicker();
		}

		#region GUI
		private void SetupGui()
		{
			// Set up the master container
			//	master.EnableEvents();

			// Set up the demo sprite containers
			master.Add(manager);
			master.EnableMouseButtonEvent();
			master.EnableTickEvent();

			// Set up the gui manager
			gui = new GuiManager(master,
				new SdlDotNet.Font("../../Data/comic.ttf", 12),
				Size);
			//GuiManager.Singleton = gui;
			gui.TitleFont = new SdlDotNet.Font("../../Data/comicbd.ttf", 12);

			// Set up the ticker
			status = new GuiTicker(gui, 0, Size.Width, Size.Height);
			status.IsAutoHide = true;
			status.Z = 3000;
			status.Rectangle = new Rectangle(100,100,100,100);
			master.Add(status);

			// Set up the status window
			master.Add(new StatusWindow(gui));
			//ticker.Add(fps);
			//fps.EnableTickEvent();
			

			// Create the menu
			//CreateMenu(gui);

			// Create a viewport
			//			manager.Coordinates.Y = gmb.OuterSize.Height + 5;
			//			manager.Coordinates.X = 5;
			//			manager.Size = new Size(Size.Width - 10,
			//				Size.Height - 10 - gmb.OuterSize.Height);
		}
		#endregion

		#region Demos
		private ArrayList demos = new ArrayList();

		private static DemoMode currentDemo = null;

		/// <summary>
		/// 
		/// </summary>
		public static DemoMode CurrentDemo
		{
			get 
			{ 
				return currentDemo; 
			}
		}

		private void LoadDemo(DemoMode mode)
		{
			// Add to the array list
			demos.Add(mode);

			// Figure out the counter
			int cnt = demos.Count;
		}

		private void LoadDemos()
		{
			// Add the sprite manager to the master
			master.Add(manager);

			// Load the actual demos
			LoadDemo(new StaticMode());
			LoadDemo(new FontMode());
			LoadDemo(new BounceMode());
			LoadDemo(new ClickMode());
			LoadDemo(new DragMode());
			LoadDemo(new ViewportMode());
			LoadDemo(new MultipleMode());
			LoadDemo(new GuiMode());
		}

		private void StopDemo()
		{
			// Stop the demo, if any
			if (currentDemo != null)
			{
				currentDemo.Stop(manager);
				currentDemo = null;
			}
		}

		private void SwitchDemo(int demo)
		{
			// Stop the demo, if any
			StopDemo();

			// Ignore if the demo request is too high
			if (demo < 0 || demo + 1 > demos.Count)
			{
				return;
			}

			// Start it
			currentDemo = (DemoMode) demos[demo];
			currentDemo.Start(manager);
			Console.WriteLine("Switched to " + currentDemo + " mode");
		}
		#endregion

		private static GuiManager gui = null;
		/// <summary>
		/// 
		/// </summary>
		public static GuiManager GuiManager 
		{ 
			get 
			{ 
				return gui; 
			} 
		}

		#region Events
		private void OnKeyboardDown(object sender, KeyboardEventArgs e) 
		{
			switch (e.Key)
			{
				case Key.Escape:
				case Key.Q:
					running = false;
					break;
				case Key.C:
					StopDemo();
					break;
				case Key.One: 
					SwitchDemo(0); 
					break;
				case Key.Two: 
					SwitchDemo(1); 
					break;
				case Key.Three: 
					SwitchDemo(2); 
					break;
				case Key.Four: 
					SwitchDemo(3); 
					break;
				case Key.Five: 
					SwitchDemo(4); 
					break;
				case Key.Six: 
					SwitchDemo(5); 
					break;
				case Key.Seven: 
					SwitchDemo(6); 
					break;
				case Key.Eight: 
					SwitchDemo(7); 
					break;
			}
		}

		private void OnQuit(object sender, QuitEventArgs e) 
		{
			// Mark the system to quit
			running = false;
		}

		//		StatusWindow statuswin;
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		public void OnTick(object sender, TickEventArgs args)
		{	
			screen.Fill(Color.Black);
			if (currentDemo != null)
			{
				screen.Blit(currentDemo.RenderSurface());
			}
			//	screen.Blit(status.Surface);
			screen.Update();
		}
		#endregion

		#region Properties
		private static SdlDemo sdlDemo = null;
		private bool running = true;
		private static SpriteCollection master = new SpriteCollection();
		private static SpriteCollection manager = new SpriteCollection();
		private Surface screen = null;
		private static GuiTicker status = null;

		/// <summary>
		/// 
		/// </summary>
		public static SdlDemo Instance 
		{ 
			get 
			{ 
				return sdlDemo; 
			} 
		}

		/// <summary>
		/// The master sprite manager is the top-most manager that
		/// contains everything else. It includes the GUI elements common
		/// throughout the site and also the sprite manager that frames
		/// the actual output.
		/// </summary>
		public static SpriteCollection MasterSpriteContainer 
		{ 
			get 
			{ 
				return master; 
			} 
		}

		/// <summary>
		/// The sprite manager is the manager that handles the demo
		/// pages. This may be translated or moved as needed. All
		/// DemoModes should attached their sprite manager to this one.
		/// </summary>
		public static SpriteCollection Sprites
		{ 
			get 
			{ 
				return manager; 
			} 
		}

		/// <summary>
		/// 
		/// </summary>
		public static Size Size
		{
			get 
			{ 
				return new Size(800, 600); 
			}
		}
		#endregion
	}
}
