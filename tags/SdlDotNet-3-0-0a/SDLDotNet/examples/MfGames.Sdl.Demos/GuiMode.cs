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

using MfGames.Utility;
using MfGames.Sdl.Drawable;
using MfGames.Sdl.Sprites;
using MfGames.Sdl.Gui;
using System.Drawing;

namespace MfGames.Sdl.Demos
{
  public class GuiMode : DemoMode
  {
    private GuiTicker ticker = null;

    /// <summary>
    /// Constructs the internal sprites needed for our demo.
    /// </summary>
    public GuiMode()
    {
      // Create the manager and the marble base
      GuiManager manager = SdlDemo.GuiManager;
      manager.TitleFont = new SdlDotNet.Font("../../Data/comicbd.ttf", 12);

      // Create a new dragable window
      GuiWindow gw = new GuiWindow(manager, new Rectangle2(200, 65, 100, 100));
      gw.IsDragable = true;
      gw.Title = "Dragable Window";
      gw.Contents.Add(new AnimatedSprite(LoadRandomMarble(),
					 new Vector2(18, 18)));
      sm.Add(gw);

      // Create a dragable window without a title
      gw = new GuiWindow(manager, new Rectangle2(25, 120, 32, 32));
      gw.IsDragable = true;
      gw.Contents.Add(new AnimatedSprite(LoadRandomMarble(),
					 new Vector2(0, 0)));
      sm.Add(gw);

      // Create a dragable window with a long title
      gw = new GuiWindow(manager, new Rectangle2(100, 415, 64, 64));
      gw.Title = "Non-Dragable Window with a Really Long Title";
      gw.IsDragable = false;
      gw.Contents.Add(new AnimatedSprite(LoadRandomMarble(),
					 new Vector2(0, 0)));
      sm.Add(gw);

      // Create the menus
      CreateMenus(manager, sm);

      // Create the ticker
      ticker = new GuiTicker(manager, 0, 800, 550);
      sm.Add(ticker);
    }

    public void CreateMenus(GuiManager gui, SpriteContainer sm)
    {
      // Create the menubar
      Dimension2 dd = SdlDemo.SpriteContainer.Size;
      GuiMenuBar gmb = new GuiMenuBar(gui, 0, dd.Width, 30);
      sm.Add(gmb);

      // First menu
      GuiMenuTitle gm = new GuiMenuTitle(gui, "Test Menu");
      gmb.AddLeft(gm);

      Debug("GM {0}: {1}", gm, gm.Size);
      Debug("GMB {0}: {1}", gmb, gmb.Size);

      // Create a menu items
      gm.Add(new GuiMenuItem(gui, "Test #1"));
      gm.Add(new GuiMenuItem(gui, "Test #2"));

      GuiMenuItem gmi3 = new GuiMenuItem(gui);
      gmi3.AddLeft(new AnimatedSprite(LoadRandomMarble(), new Vector2(0, 0)));
      gmi3.AddLeft(new TextSprite("Create New Window", gui.BaseFont));
      gm.Add(gmi3);
      gmi3.ItemSelectedEvent += new MenuItemHandler(OnCreateNewWindow);

      // Create the first menu
      gm.Add(new GuiMenuItem(gui, "Test #3"));
      gm.Add(new GuiMenuItem(gui, "Test #4"));

      // Create the second
      GuiMenuTitle gm2 = new GuiMenuTitle(gui, "Test #2");
      GuiMenuItem gmi2 = new GuiMenuItem(gui, "Test 2.1");
      gmb.AddLeft(gm2);
      gm2.Popup.Add(gmi2);
      gm2.Popup.Add(new GuiMenuItem(gui, "Test 2.2"));
      //gm2.IsTraced = true;
      //gm2.Popup.IsTraced = true;
      //gmi2.IsTraced = true;

      // Create a third menu
      GuiMenuTitle gm3 = new GuiMenuTitle(gui, "Right Menu");
      gm3.Popup.Add(new GuiMenuItem(gui, "Test #6"));
      gm3.Popup.Add(new GuiMenuItem(gui, "Really Long Title for a Menu Item"));
      gmb.AddRight(new TextSprite("NonMenu", gui.BaseFont));
      gmb.AddRight(gm3);
    }

    /// <summary>
    /// Adds the internal sprite manager to the outer one.
    /// </summary>
    public override void Start(SpriteContainer manager)
    {
      manager.Add(sm);
      base.Start(manager);
    }

    /// <summary>
    /// Removes the internal manager from the controlling manager.
    /// </summary>
    public override void Stop(SpriteContainer manager)
    {
      manager.Remove(sm);
      base.Stop(manager);
    }

    public override string ToString() { return "GUI"; }

    #region Events
    private double threshold = 100.0;
    private double rate = 0.009;
    //private bool running = false;

    public void OnCreateNewWindow(int index)
    {
      Debug("Selecting: " + index + ": " + this);

      IDrawable m1 = LoadRandomMarble();
      GuiManager manager = SdlDemo.GuiManager;
      GuiWindow gw = new GuiWindow(manager,
				   new Rectangle2(Entropy.Next() % 600,
						  Entropy.Next() % 400 + 50,
						  70, 70));
      gw.IsDragable = true;
      gw.Title = "Created Window";
      gw.Contents.Add(new AnimatedSprite(m1, new Vector2(3, 3)));
      sm.Add(gw);
    }

    public override void OnTick(TickArgs args)
    {
      threshold += args.RatePerSecond(rate);

      // Keep track of the counter (the point to trigger)
      if (threshold > 1.0)
      {
	threshold = 0.0;

	switch (Entropy.Next() % 5)
	{
	case 0: // Switch autohide
	  if (ticker.IsAutoHide)
	    ticker.Add(new TextSprite("AutoHide off",
				      SdlDemo.GuiManager.BaseFont));
	  else
	    ticker.Add(new TextSprite("AutoHide on",
				      SdlDemo.GuiManager.BaseFont));
	  
	  ticker.IsAutoHide = !ticker.IsAutoHide;
	  break;
	case 1: // Simple message
	  // Add a message
	  ticker.Add(new TextSprite("*tick*", SdlDemo.GuiManager.BaseFont));
	  Debug("*tick*");
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
	  Debug("Delta changed to {0}", ticker.Delta);
	  break;
	case 4: // Add two marbles
	  Debug("*marbles*");
	  ticker.Add(new AnimatedSprite(LoadRandomMarble(),
					new Vector2(0, 0)));
	  ticker.Add(new AnimatedSprite(LoadRandomMarble(),
					new Vector2(0, 0)));
	  break;
	}
      }
    }
    #endregion
  }
}
