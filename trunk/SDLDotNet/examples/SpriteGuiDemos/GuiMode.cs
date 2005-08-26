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

using SdlDotNet;
using SdlDotNet.Sprites;
using SdlDotNet.Examples.GuiExample;
using System.Drawing;
using System;

namespace SdlDotNet.Examples
{
	/// <summary>
	/// 
	/// </summary>
	public class GuiMode : DemoMode
	{
		private GuiTicker ticker;
		Random rand = new Random();
		GuiMenuTitle gm;
		GuiMenuTitle gm2;
		GuiMenuTitle gm3;

		/// <summary>
		/// Constructs the internal sprites needed for our demo.
		/// </summary>
		public GuiMode()
		{
			// Create the manager and the marble base
			GuiManager manager = SdlDemo.GuiManager;

			// Create a new draggable window
			GuiWindow gw = new GuiWindow(manager, new Rectangle(200, 65, 110, 100));
			gw.AllowDrag = true;
			gw.Title = "Draggable Window";
			gw.Sprites.Add(new AnimatedDemoSprite(LoadRandomMarble(),
				new Point(18, 18)));
			gw.TitleBackgroundColor = manager.FrameColor;
			gw.Sprites.EnableTickEvent();
			Sprites.Add(gw);

			// Create a draggable window without a title
			gw = new GuiWindow(manager, new Rectangle(25, 120, 32, 32));
			gw.AllowDrag = true;
			gw.Sprites.Add(new AnimatedDemoSprite(LoadRandomMarble(),
				new Point(0, 0)));
			gw.Sprites.EnableTickEvent();
			Sprites.Add(gw);

			// Create a non-draggable window with a long title
			gw = new GuiWindow(manager, new Rectangle(100, 390, 256, 90));
			gw.Title = "Non-Draggable Window with a Long Title";
			gw.AllowDrag = false;
			gw.Sprites.Add(new AnimatedDemoSprite(LoadRandomMarble(),
				new Point(0, 18)));
			gw.TitleBackgroundColor = manager.FrameColor;
			gw.Sprites.EnableTickEvent();
			Sprites.Add(gw);

			// Create the menus
			CreateMenus(manager, Sprites);

			// Create the ticker
			ticker = new GuiTicker(manager, 0, 500, 64);
			Sprites.Add(ticker);
			this.EnableTickEvent();
			Sprites.EnableMouseButtonEvent();
			Sprites.EnableMouseMotionEvent();
			Sprites.EnableTickEvent();
			ticker.Sprites.EnableTickEvent();
		}
		/// <summary>
		/// 
		/// </summary>
		public void EnableTickEvent()
		{
			Events.Tick += new TickEventHandler(OnTick);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="gui"></param>
		/// <param name="sm"></param>
		public void CreateMenus(GuiManager gui, SpriteCollection sm)
		{
			// Create the menubar
			//Size dd = SdlDemo.Sprites.Size;
			GuiMenuBar gmb = new GuiMenuBar(gui, 0, 40, 20);
			gmb.Sprites.EnableMouseButtonEvent();
			sm.Add(gmb);
		
			// First menu
			gm = new GuiMenuTitle(gui, gmb, "Test Menu");
			//gm.MenuBar = gmb;
			gm.Sprites.EnableMouseButtonEvent();
			gmb.AddLeft(gm);
			//sm.Add(gm.Popup);

		
			// Create a menu items
			gm.Add(new GuiMenuItem(gui, "Test #1"));
			gm.Add(new GuiMenuItem(gui, "Test #2"));
		
			GuiMenuItem gmi3 = new GuiMenuItem(gui);
			gmi3.AddLeft(new AnimatedDemoSprite(LoadRandomMarble(), new Point(0, 0)));
			gmi3.AddLeft(new TextSprite("Create New Window", gui.BaseFont));
			gm.Add(gmi3);
			gmi3.ItemSelectedEvent += new MenuItemEventHandler(OnCreateNewWindow);
		
			// Create the first menu
			gm.Add(new GuiMenuItem(gui, "Test #3"));
			gm.Add(new GuiMenuItem(gui, "Test #4"));
		
			// Create the second
			gm2 = new GuiMenuTitle(gui, gmb, "Test #2");
			GuiMenuItem gmi2 = new GuiMenuItem(gui, "Test 2.1");
			gmb.AddLeft(gm2);
			gm2.Popup.Add(gmi2);
			gm2.Popup.Add(new GuiMenuItem(gui, "Test 2.2"));
			//gm2.IsTraced = true;
			//gm2.Popup.IsTraced = true;
			//gmi2.IsTraced = true;
			//sm.Add(gm2.Popup);
		
			// Create a third menu
			gm3 = new GuiMenuTitle(gui, gmb, "Right Menu");
			gm3.Popup.Add(new GuiMenuItem(gui, "Test #6"));
			gm3.Popup.Add(new GuiMenuItem(gui, "Really Long Title for a Menu Item"));
			//gmb.AddRight(new TextSprite("NonMenu", gui.BaseFont));
			gmb.AddRight(gm3);
			//sm.Add(gm3.Popup);
		}

		/// <summary>
		/// Adds the internal sprite manager to the outer one.
		/// </summary>
		public override void Start(SpriteCollection manager)
		{
			manager.Add(Sprites);
			base.Start(manager);
			manager.Add(gm.Popup);
			manager.Add(gm2.Popup);
			manager.Add(gm3.Popup);
		}

		/// <summary>
		/// Removes the internal manager from the controlling manager.
		/// </summary>
		public override void Stop(SpriteCollection manager)
		{
			manager.Remove(Sprites);
			base.Stop(manager);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override string ToString() { return "GUI"; }

		#region Events
		private double threshold = 100.0;
		//private double rate = 0.009;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		/// <param name="sender"></param>
		private void OnCreateNewWindow(object sender, MenuItemEventArgs e)
		{
			SurfaceCollection m1 = LoadRandomMarble();
			GuiManager manager = SdlDemo.GuiManager;
			GuiWindow gw = new GuiWindow(manager,
				new Rectangle(rand.Next() % 600,
				rand.Next() % 400 + 50,
				70, 70));
			gw.AllowDrag = true;
			gw.Title = "Created Window";
			gw.Sprites.Add(new AnimatedDemoSprite(m1, new Point(3, 3)));
			Sprites.Add(gw);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		private void OnTick(object sender, TickEventArgs args)
		{
			//threshold += args.RatePerSecond(rate);
			threshold += 10;

			// Keep track of the counter (the point to trigger)
			if (threshold > 1.0)
			{
				threshold = 0.0;

				switch (rand.Next() % 5)
				{
					case 0: // Switch autohide
						if (ticker.IsAutoHide)
						{
							ticker.Add(new TextSprite("AutoHide off",
								SdlDemo.GuiManager.BaseFont));
						}
						else
						{
							ticker.Add(new TextSprite("AutoHide on",
								SdlDemo.GuiManager.BaseFont));
						}	  
						ticker.IsAutoHide = !ticker.IsAutoHide;
						break;
					case 1: // Simple message
						// Add a message
						ticker.Add(new TextSprite("*tick*", SdlDemo.GuiManager.BaseFont));
						break;
					case 2: // Bunch of messages
						ticker.Add(new TextSprite("*", SdlDemo.GuiManager.BaseFont));
						ticker.Add(new TextSprite("*", SdlDemo.GuiManager.BaseFont));
						ticker.Add(new TextSprite("*", SdlDemo.GuiManager.BaseFont));
						ticker.Add(new TextSprite("*", SdlDemo.GuiManager.BaseFont));
						ticker.Add(new TextSprite("*", SdlDemo.GuiManager.BaseFont));
						ticker.Add(new TextSprite("*", SdlDemo.GuiManager.BaseFont));
						ticker.Add(new TextSprite("*", SdlDemo.GuiManager.BaseFont));
						break;
					case 3: // Reverse direction
						ticker.Delta *= -1;
						ticker.Add(new TextSprite("Delta " + ticker.Delta,
							SdlDemo.GuiManager.BaseFont));
						break;
					case 4: // Add two marbles
						ticker.Add(new AnimatedDemoSprite(LoadRandomMarble(),
							new Point(0, 0)));
						ticker.Add(new AnimatedDemoSprite(LoadRandomMarble(),
							new Point(0, 0)));
						break;
				}
				
			}
		}
		#endregion
	}
}
