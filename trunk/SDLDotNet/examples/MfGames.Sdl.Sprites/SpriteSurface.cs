using MfGames.Utility;
using SdlDotNet;
using System;
using System.Collections;
using System.Drawing;

namespace MfGames.Sdl.Sprites
{
  public class SpriteSurface : LoggedObject
  {
    public SpriteSurface(Surface screen, SpriteContainer manager)
    {
      // Sanity checking
      if (screen == null)
	throw new Exception("Cannot create a sprite surface with a null "
			    + "screen");

      if (manager == null)
	throw new Exception("Cannot create a sprite surface with a null "
			    + "sprite manager");

      // Save our fields
      this.screen = screen;
      this.manager = manager;

      // Create a compatiable surface
      surface = screen.CreateCompatibleSurface(screen.Width,
					       screen.Height,
					       true);
      surface.Fill(new Rectangle(new Point(0, 0), surface.Size),
		       Color.Black);
    }

    #region SDL
    /// <summary>
    /// This is the primary function of the sprite surface, to get
    /// all sprites in a specific viewable region and display them to
    /// a screen. This draws the sprites in the Z-order, then flips
    /// the screen to prevent any flicker.
    /// </summary>
    public void Blit()
    {
      // Clear the screen
      surface.Fill(new Rectangle(new Point(0, 0), surface.Size),
		       Color.Black);

      // Create the rendering args
      RenderArgs args = new RenderArgs(surface, new Dimension2(surface.Size));
      manager.Render(args);

      // Draw ourselves on the screen
      screen.Blit(surface, new Rectangle(new Point(0, 0), screen.Size));
      screen.Flip();
    }
    #endregion

    #region Properties
    private SpriteContainer manager = null;

    private Surface screen = null;

    private Surface surface = null;
    #endregion
  }
}
