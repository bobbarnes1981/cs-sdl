using System.Drawing;

namespace MfGames.Sdl.Sprites
{
  /// <summary>
  /// This creates a viewport that centers a sprite, but ensures that
  /// a specific region is always visible. This attempts to center,
  /// the normalize the view (to keep from showing off the "map" or
  /// bounds).
  /// </summary>
  public class BoundedCenterViewport : CenteredViewport
  {
    private Rectangle region = Rectangle.Empty;

    /// <summary>
    /// Constructs a viewport to center on a specific sprite.
    /// </summary>
    public BoundedCenterViewport(Sprite centerOnSprite, Rectangle region)
      : base(centerOnSprite)
    {
      this.region = region;
    }

    /// <summary>
    /// This gets the upper-left corner of the viewport, based on the
    /// given coordinates of the actual screen. This enables a
    /// viewport to only show in a specific part of the screen. The
    /// point returned is relative to the sprite manager.
    /// </summary>
    public override void AdjustViewport(ref RenderArgs args)
    {
      // Get the center point
      base.AdjustViewport(ref args);

      // Check to see if the window is too small
      bool doWidth = true;
      bool doHeight = true;

      if (region.Width < args.Size.Width)
	doWidth = false;

      if (region.Height < args.Size.Height)
	doHeight = false;

      if (!doWidth && !doHeight)
	return;

      // Find out the "half" point for the sprite in the view
      int mx = Sprite.Coords.X + Sprite.Size.Width / 2;
      int my = Sprite.Coords.Y + Sprite.Size.Height / 2;

      // Figure out the coordinates
      int x1 = mx - args.Size.Width / 2;
      int x2 = mx + args.Size.Width / 2;
      int y1 = my - args.Size.Height / 2;
      int y2 = my + args.Size.Height / 2;

      // Make sure we don't exceed the bounds
      if (doWidth && x1 < region.Left)
	args.TranslateX -= region.Left - x1;

      if (doHeight && y1 < region.Top)
	args.TranslateY -= region.Top - y1;

      if (doWidth && x2 > region.Right)
	args.TranslateX += x2 - region.Right;

      if (doHeight && y2 > region.Bottom)
	args.TranslateY += y2 - region.Bottom;
    }
  }
}
