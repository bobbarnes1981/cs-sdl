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

using MfGames.Sdl.Sprites;
using MfGames.Utility;
using NUnit.Framework;
using SdlDotNet;
using System.Drawing;
using System;

namespace MfGames.Sdl.Gui
{
  [TestFixture] public class SdlGuiTest
  {
    private SpriteContainer sm = null;
    private GuiManager gui = null;

    [SetUp] public void Setup()
    {
      // Create the gui used through the tst
      sm = new SpriteContainer();
      gui = new GuiManager(sm,
			   new SdlDotNet.Font("../test/comic.ttf", 12),
			   new Size(800, 600));
    }

    [Test] public void TestWindowBounds()
    {
      GuiWindow win = new GuiWindow(gui, new Rectangle2(10, 11, 100, 101));

      Assert.Equals(10 - gui.GetPadding(win).Left, win.Coords.X);
      Assert.Equals(11 - gui.GetPadding(win).Top, win.Coords.Y);
      Assert.Equals(100 + gui.GetPadding(win).Horizontal,
			     win.Size.Width);
      Assert.Equals(101 + gui.GetPadding(win).Vertical,
			     win.Size.Height);
    }
  }
}
