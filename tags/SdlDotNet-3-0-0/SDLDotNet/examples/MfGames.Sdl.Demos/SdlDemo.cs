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

using log4net.Config;
using MfGames.Utility;
using MfGames.Sdl.Sprites;
using MfGames.Sdl.Gui;
using SdlDotNet;
using System;
using System.Collections;
using System.Drawing;
using System.Threading;

namespace MfGames.Sdl.Demos
{
	/// <summary>
	/// The SdlDemo is a general testbed and display of various features
	/// in the MfGames.Sdl library. It includes animated sprites and
	/// movement. To run, it currently assumes that the current
	/// directory has a "test/" directory underneath it containing
	/// various images.
	/// </summary>
	public class SdlDemo : LoggedObject
	{
		public static void Main(string [] args)
		{
			// Set up loggin
			BasicConfigurator.Configure();

			// Create the demo application
			SdlDemo demo = new SdlDemo();
			demo.Start();
		}

		public void Start()
		{
			// Noise
			Info("Staring up SDL demo...");
			sdlDemo = this;

			// Start up the SDL
			Video.WindowCaption = "Moonfire Games' SDL Demo";
      
			Events.KeyboardDown +=
				new KeyboardEventHandler(this.OnKeyboardDown);
			Events.Quit += new QuitEventHandler(this.OnQuit);

			// Create the screen
			int width = 800;
			int height = 600;

			screen = Video.SetVideoModeWindow(width, height, true);
			ss = new SpriteSurface(screen, master);

			// Set up the master sprite container
			SetupGui();

			// Load demos
			LoadDemos();

			/*
			master.Add(CreateMenu(gui));

			// Set up the viewport
			manager.Viewport = new LimitedViewport(Bounds);
			*/

			// Start up the ticker (and animation)
			TickManager.TicksPerSecond = 15;
			TickManager.TickEvent += new TickHandler(OnTick);
			TickManager.Start();

			// Loop until the system indicates it should stop
			Report("Welcome to the Moonfire Games' SDL Demo!");

			while (running)
			{
				// Sleep a little (60 fps)
				while(Events.Poll())
				{}
				Thread.Sleep(1000 / 60);
			}

			// Stop the ticker and the current demo
			SwitchDemo(-1);
			TickManager.Stop();
		}

		#region GUI
		private GuiMenuTitle demoMenu = null;

		private int [] fpsSpeeds = new int [] {1, 5, 10, 15,
												  20, 30, 40, 50, 60, 100 };

		private void SetupGui()
		{
			// Set up the master container
			master.ListenToSdlEvents();

			// Set up the demo sprite containers
			master.Add(manager);

			// Set up the gui manager
			gui = new GuiManager(master,
				new SdlDotNet.Font("../../Data/comic.ttf", 12),
				Size);
			GuiManager.Singleton = gui;
			gui.TitleFont = new SdlDotNet.Font("../../Data/comicbd.ttf", 12);

			// Set up the ticker
			status = new GuiTicker(gui, 0, Size.Width, Size.Height);
			status.IsAutoHide = true;
			status.Coords.Z = 3000;
			master.Add(status);

			// Set up the status window
			master.Add(new StatusWindow(gui));
			ticker.Add(fps);

			// Create the menu
			CreateMenu(gui);

			// Create a viewport
			manager.Coords.Y = gmb.OuterSize.Height + 5;
			manager.Coords.X = 5;
			manager.Size = new Dimension2(Size.Width - 10,
				Size.Height - 10 - gmb.OuterSize.Height);
		}

		private void CreateMenu(GuiManager gui)
		{
			// Create the menu
			gmb = new GuiMenuBar(gui, 0, 800, 0);
			gmb.IsTickable = false;
			master.Add(gmb);

			// Create the demo menu
			demoMenu = new GuiMenuTitle(gui, "Demo");
			gmb.AddLeft(demoMenu);

			// Create the FPS menu
			GuiMenuTitle gm = new GuiMenuTitle(gui, "FPS");
			gmb.AddLeft(gm);
      
			for (int i = 0; i < fpsSpeeds.Length; i++)
			{
				int spd = fpsSpeeds[i];

				GuiMenuItem fmi = new GuiMenuItem(gui, spd + " FPS");
				fmi.ItemSelectedEvent += new MenuItemHandler(OnMenuFps);
				fmi.IsTickable = false;
				gm.Add(fmi);
			}
		}

		private void CreateMenuQuit(GuiManager gui)
		{
			//demoMenu.Add(new GuiMenuSpacer(demoMenu));
			GuiMenuItem gmi = new GuiMenuItem(gui, "Quit");
			gmi.AddRight(new TextSprite("Q", gui.BaseFont));
			gmi.ItemSelectedEvent += new MenuItemHandler(OnMenuQuit);
			gmi.IsTickable = false;
			demoMenu.Add(gmi);
		}

		public static void Report(string msg)
		{
			if (status != null)
				status.Add(new TextSprite(msg, GuiManager.BaseFont));

			Instance.Info(msg);
		}
		#endregion

		#region Demos
		private ArrayList demos = new ArrayList();

		private static DemoMode currentDemo = null;

		public static DemoMode CurrentDemo
		{
			get { return currentDemo; }
		}

		private void LoadDemo(DemoMode mode)
		{
			// Add to the array list
			Debug("Loading demo {0}", mode);
			demos.Add(mode);

			// Figure out the counter
			int cnt = demos.Count;

			// Add the graphical menu
			GuiMenuItem gmi = new GuiMenuItem(gui, mode.ToString());
			gmi.AddRight(new TextSprite(String.Format("{0}", cnt),
				gui.BaseFont));
			gmi.ItemSelectedEvent += new MenuItemHandler(OnMenuDemo);
			gmi.IsTickable = false;
			demoMenu.Add(gmi);
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
      
			// Make some noise
			Debug("Loaded all demos");

			// Finish up the gui
			CreateMenuQuit(gui);
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
				return;

			// Start it
			currentDemo = (DemoMode) demos[demo];
			currentDemo.Start(manager);
			Info("Switched mode to {0}", currentDemo);
			Report("Switched to " + currentDemo + " mode");
		}
		#endregion

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
				case Key.One: SwitchDemo(0); break;
				case Key.Two: SwitchDemo(1); break;
				case Key.Three: SwitchDemo(2); break;
				case Key.Four: SwitchDemo(3); break;
				case Key.Five: SwitchDemo(4); break;
				case Key.Six: SwitchDemo(5); break;
				case Key.Seven: SwitchDemo(6); break;
				case Key.Eight: SwitchDemo(7); break;
				case Key.Nine: SwitchDemo(8); break;
				case Key.Zero: SwitchDemo(9); break;
			}
		}
    
		private void OnMenuDemo(int index)
		{
			SwitchDemo(index);
		}

		private void OnMenuFps(int index)
		{
			SdlDemo.TickManager.TicksPerSecond = fpsSpeeds[index];
		}

		private void OnMenuQuit(int index)
		{
			running = false;
		}

		private void OnQuit(object sender, QuitEventArgs e) 
		{
			// Mark the system to quit
			running = false;
		}

		public void OnTick(TickArgs args)
		{
			// Process the SDL events
			while (Events.Poll());
      
			// Process sprites
			master.OnTick(args);

			// Tick the demo
			if (currentDemo != null)
				currentDemo.OnTick(args);

			// Tick the status
			if (status != null)
				status.OnTick(args);

			// Process our own animation
			ss.Blit();
		}
		#endregion

		#region Properties
		private static SdlDemo sdlDemo = null;
		private bool running = true;
		private static SpriteContainer master = new SpriteContainer();
		private static SpriteContainer manager = new SpriteContainer();
		private Surface screen = null;
		private static SpriteSurface ss = null;
		private static GuiTicker status = null;
		private static GuiMenuBar gmb = null;
		private static SecondGuage fps = new SecondGuage(13);
		private static TickManager ticker = new TickManager();
		private static GuiManager gui = null;

		public static SecondGuage Fps { get { return fps; } }

		public static SdlDemo Instance { get { return sdlDemo; } }

		/// <summary>
		/// The master sprite manager is the top-most manager that
		/// contains everything else. It includes the GUI elements common
		/// throughout the site and also the sprite manager that frames
		/// the actual output.
		/// </summary>
		public static SpriteContainer MasterSpriteContainer { get { return master; } }

		/// <summary>
		/// The sprite manager is the manager that handles the demo
		/// pages. This may be translated or moved as needed. All
		/// DemoModes should attached their sprite manager to this one.
		/// </summary>
		public static SpriteContainer SpriteContainer { get { return manager; } }

		public static TickManager TickManager { get { return ticker; } }

		public static GuiManager GuiManager { get { return gui; } }

		public static GuiMenuBar MenuBar { get { return gmb; } }

		public static Dimension2 Size
		{
			get { return new Dimension2(800, 600); }
		}

		public static Rectangle2 Bounds
		{
			get
			{
				return new Rectangle2(new Vector2(0, gmb.Bounds.Bottom),
					new Dimension2(Size.Width,
					Size.Height - gmb.Bounds.Bottom));
			}
		}

		public static SpriteSurface Surface
		{
			get { return ss; }
		}
		#endregion
	}
}
