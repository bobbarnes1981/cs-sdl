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
using MfGames.Sdl.Gui;
using MfGames.Sdl.Sprites;
using System.Drawing;

namespace MfGames.Sdl.Demos
{
  public class ViewportMode : DemoMode
  {
    private Sprite sprite = null;
    //private GuiMenuTitle viewMenu = null;
    private Dimension2 size = null;
    private bool created = false;

    /// <summary>
    /// Constructs the internal sprites needed for our demo.
    /// </summary>
    public ViewportMode()
    {
      // Create the fragment marbles
      IDrawable td = LoadMarble("marble1");
      IDrawable td2 = LoadMarble("marble2");

      // Load the floor
      int numberOfFloors = 4;
      IDrawable [] floorTiles = new IDrawable [4];

      for (int i = 0; i < numberOfFloors; i++)
	floorTiles[i] = LoadFloor(i + 1);

      // Place the floors
      int rows = 15;
      int cols = 25;
      size = new Dimension2(floorTiles[0].Size.Width * cols,
			    floorTiles[0].Size.Height * rows);
      Rectangle2 rect = new Rectangle2(size);
      Debug("rect={0} size={1}", rect, size);

      for (int i = 0; i < cols; i++)
      {
	for (int j = 0; j < rows; j++)
	{
	  // Create the sprite
	  DrawableSprite dw =
	    new DrawableSprite(floorTiles[Entropy.Next(0, numberOfFloors -1)],
			       new Vector3(i * floorTiles[0].Size.Width,
					   j * floorTiles[0].Size.Height,
					   -1000));
	  sm.Add(dw);
	}
      }

      // Load the trigger sprite
      sprite = new BounceSprite(td2, rect, 100);
      sm.Add(sprite);
      //OnMenuTranslated(0);
      //OnMenuCentered(0);
      OnMenuBounded(0);

      // Load the bouncing sprites
      for (int i = 0; i < 53; i++)
      {
	sm.Add(new BounceSprite(td, rect));
      }

      // Create the menus
      /*
      viewMenu = new GuiMenu(SdlDemo.MenuBar, "Viewport");
      viewMenu.IsTitleHidden = true;
      SdlDemo.MenuBar.Add(viewMenu);

      GuiMenuItem gmi = new GuiMenuItem(viewMenu, "None");
      gmi.ItemSelectedEvent += new MenuItemHandler(OnMenuNone);
      viewMenu.Add(gmi);

      gmi = new GuiMenuItem(viewMenu, "Translated (0, 0)");
      gmi.ItemSelectedEvent += new MenuItemHandler(OnMenuTranslated0);
      viewMenu.Add(gmi);

      gmi = new GuiMenuItem(viewMenu, "Translated (25, 25)");
      gmi.ItemSelectedEvent += new MenuItemHandler(OnMenuTranslated);
      viewMenu.Add(gmi);

      gmi = new GuiMenuItem(viewMenu, "Centered");
      gmi.ItemSelectedEvent += new MenuItemHandler(OnMenuCentered);
      viewMenu.Add(gmi);

      gmi = new GuiMenuItem(viewMenu, "Bounded Center");
      gmi.ItemSelectedEvent += new MenuItemHandler(OnMenuBounded);
      viewMenu.Add(gmi);
      */

      created = true;
    }

    /// <summary>
    /// Adds the internal sprite manager to the outer one.
    /// </summary>
    public override void Start(SpriteContainer manager)
    {
      base.Start(manager);
    }

    /// <summary>
    /// Removes the internal manager from the controlling manager.
    /// </summary>
    public override void Stop(SpriteContainer manager)
    {
      base.Stop(manager);
    }

    public override string ToString() { return "Viewport"; }

    #region Events
    private void OnMenuNone(int index)
    {
      sm.Viewport = null;
    }

    private void OnMenuBounded(int index)
    {
      Rectangle2 rect = new Rectangle2(new Vector2(0, 0), size);
      sm.Viewport = new BoundedCenterViewport(sprite, rect);
      if (created) SdlDemo.Report("center(" + rect + ")");
    }

    private void OnMenuCentered(int index)
    {
      sm.Viewport = new CenteredViewport(sprite);
      if (created) SdlDemo.Report("center()");
    }

    private void OnMenuTranslated(int index)
    {
      sm.Viewport = new TranslatedViewport(25, 25);
      if (created) SdlDemo.Report("translate(25, 25)");
    }

    private void OnMenuTranslated0(int index)
    {
      sm.Viewport = new TranslatedViewport(0, 0);
      if (created) SdlDemo.Report("translate(0, 0)");
    }
    #endregion
  }
}
