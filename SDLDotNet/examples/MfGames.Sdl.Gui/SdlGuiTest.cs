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
