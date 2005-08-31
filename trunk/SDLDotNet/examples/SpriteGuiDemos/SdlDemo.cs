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
using SdlDotNet.Examples.GuiExample;
using SdlDotNet;
using System;
using System.Collections;
using System.Drawing;
using System.Threading;
using System.Globalization;

namespace SdlDotNet.Examples
{
	/// <summary>
	/// The SdlDemo is a general testbed and display of various features
	/// in the MFGames.Sdl library. It includes animated sprites and
	/// movement. To run, it currently assumes that the current
	/// directory has a "test/" directory underneath it containing
	/// various images.
	/// </summary>
	class SdlDemo : IDisposable
	{
		/// <summary>
		/// 
		/// </summary>
		public static void Main()
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
			// Start up the SDL
			Video.WindowCaption = "SdlDotNet - Sprite and Gui Demo";
			Video.Mouse.ShowCursor = false;
      
			Events.KeyboardDown +=
				new KeyboardEventHandler(this.OnKeyboardDown);
			Events.Quit += new QuitEventHandler(this.OnQuit);
			Events.Tick += new TickEventHandler(this.OnTick);

			// Create the screen
			int width = 800;
			int height = 600;

			screen = Video.SetVideoModeWindow(width, height, true);
			cursor = 
				new Surface(@"../../Data/cursor.png");

			MouseMotionHandler = new MouseMotionEventHandler(this.MouseMotion);
			Events.MouseMotion += MouseMotionHandler;

			// Set up the master sprite container
			SetupGui();

			// Load demos
			LoadDemos();

			// Start up the ticker (and animation)
			Events.FPS = 30;
			Events.Run();

			// Loop until the system indicates it should stop
			Console.WriteLine("Welcome to the SDL.NET Demo!");

			Events.Run();

			// Stop the ticker and the current demo
			SwitchDemo(-1);
		//	Events.StopTicker();
		}

		#region GUI
		private GuiMenuTitle demoMenu;
		private GuiMenuTitle gm;

		private int [] fpsSpeeds = 
			new int [] {1, 5, 10, 15, 20, 30, 40, 50, 60, 100 };
		private void SetupGui()
		{
			// Set up the demo sprite containers
			master.EnableMouseButtonEvent();
			master.EnableMouseMotionEvent();
			master.EnableTickEvent();

			// Set up the gui manager
			gui = new GuiManager(master,
				new SdlDotNet.Font("../../Data/comic.ttf", 12),
				Size);
			gui.TitleFont = new SdlDotNet.Font("../../Data/comicbd.ttf", 12);

			// Set up the ticker
			statusTicker = new GuiTicker(gui, new Vector(0, Video.Screen.Height - 20, 3000), 20);
			master.Add(statusTicker);
			Report("SDL.NET Demo started");

			statusWindow = new StatusWindow(gui);
			// Set up the status window
			master.Add(statusWindow);
			//statusWindow.Add(fps);
			//fps.EnableTickEvent();
			

			// Create the menu
			CreateMenu(gui);
		}

		private void CreateMenu(GuiManager gui)
		{
			// Create the menu
			gmb = new GuiMenuBar(gui, 0, 1, 20);
			gmb.Sprites.EnableTickEvent();
			gmb.Sprites.EnableMouseButtonEvent();
			master.Add(gmb);

			// Create the demo menu
			demoMenu = new GuiMenuTitle(gui, gmb, "Demo");
			master.Add(demoMenu.Popup);
			gmb.AddLeft(demoMenu);

			// Create the FPS menu
			gm = new GuiMenuTitle(gui, gmb, "FPS");
			master.Add(gm.Popup);
			gmb.AddLeft(gm);

			GuiMenuItem fmi;
      
			for (int i = 0; i < fpsSpeeds.Length; i++)
			{
				int spd = fpsSpeeds[i];

				fmi = new GuiMenuItem(gui, spd.ToString(CultureInfo.CurrentCulture) + " FPS");
				fmi.ItemSelectedEvent += new MenuItemEventHandler(OnMenuFps);
				gm.Add(fmi);
			}
		}

		private void CreateMenuQuit(GuiManager gui)
		{
			GuiMenuItem gmi = new GuiMenuItem(gui, "Quit");
			gmi.AddRight(new TextSprite("Q", gui.BaseFont));
			gmi.ItemSelectedEvent += new MenuItemEventHandler(OnMenuQuit);
			demoMenu.Add(gmi);
		}
		#endregion

		#region Demos
		private ArrayList demos = new ArrayList();

		private static DemoMode currentDemo;

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

			// Add the graphical menu
			GuiMenuItem gmi = new GuiMenuItem(gui, mode.ToString());
			gmi.AddRight(new TextSprite(String.Format(CultureInfo.CurrentCulture, "{0}", cnt),
				gui.BaseFont));
			gmi.ItemSelectedEvent += new MenuItemEventHandler(OnMenuDemo);
			demoMenu.Add(gmi);
		}

		private void LoadDemos()
		{
			// Add the sprite manager to the master
			master.Add(manager);

			// Load the actual demos
			LoadDemo(new FontMode());
			LoadDemo(new BounceMode());
			LoadDemo(new DragMode());
			LoadDemo(new ViewportMode());
			LoadDemo(new MultipleMode());
			LoadDemo(new GuiMode());

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
			{
				return;
			}

			// Start it
			currentDemo = (DemoMode) demos[demo];
			currentDemo.Start(manager);
			Console.WriteLine("Switched to " + currentDemo + " mode");
			Report("Switched to " + currentDemo + " mode");
		}
		#endregion

		private static GuiManager gui;
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
					Events.QuitApp();
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
			Events.QuitApp();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		private void OnTick(object sender, TickEventArgs args)
		{	
			screen.Fill(Color.Black);
			if (currentDemo != null)
			{
				screen.Blit(currentDemo.RenderSurface());
			}
			screen.Blit(master);
			screen.Blit(cursor, position);
			screen.Update();
		}

		private void OnMenuDemo(object sender, MenuItemEventArgs e)
		{
			SwitchDemo(e.Index);
		}

		private void OnMenuFps(object sender, MenuItemEventArgs e)
		{
			Events.FPS = fpsSpeeds[e.Index];
		}

		private void OnMenuQuit(object sender, MenuItemEventArgs e)
		{
			Events.QuitApp();
		}

		private void MouseMotion(
			object sender, 
			MouseMotionEventArgs e)
		{
			position.X = e.X;
			position.Y = e.Y;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="msg"></param>
		public void Report(string msg)
		{
			if (statusTicker != null)
			{
				TextSprite textSprite = new TextSprite(msg, GuiManager.BaseFont);
				statusTicker.Add(textSprite);
			}
		}
		#endregion

		#region Properties
		private static SpriteCollection master = new SpriteCollection();
		private static SpriteCollection manager = new SpriteCollection();
		MouseMotionEventHandler MouseMotionHandler;
		private Surface screen;
		private GuiWindow statusWindow;
		private GuiTicker statusTicker;
		private static GuiMenuBar gmb;
		Surface cursor;
		Point position = new Point(100,100);
//		private static Clock clock = new Clock(5);
//		/// <summary>
//		/// 
//		/// </summary>
//		public static Clock Fps
//		{
//			get
//			{
//				return clock;
//			}
//		}

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

		#region IDisposable Members

		private bool disposed;

		
		/// <summary>
		/// Closes and destroys this object
		/// </summary>
		/// <remarks>Destroys managed and unmanaged objects</remarks>
		public void Dispose() 
		{
			Dispose(true);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="disposing"></param>
		protected virtual void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
					if (disposing)
					{
						this.screen.Dispose();
						foreach (Sprite s in SdlDemo.manager)
						{
							IDisposable disposableObj = s as IDisposable;
							if (disposableObj != null)
							{
								disposableObj.Dispose( );
							}
						}
						foreach (Sprite s in SdlDemo.master)
						{
							IDisposable disposableObj = s as IDisposable;
							if (disposableObj != null)
							{
								disposableObj.Dispose( );
							}
						}
						statusTicker.Dispose();
						demoMenu.Dispose();
						gm.Dispose();
						statusWindow.Dispose();
					}
					this.disposed = true;
			}
		}
		#endregion
	}
}
