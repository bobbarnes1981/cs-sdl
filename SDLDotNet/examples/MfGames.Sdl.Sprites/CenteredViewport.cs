using MfGames.Utility;
using System.Drawing;

namespace MfGames.Sdl.Sprites
{
  /// <summary>
  /// The centered viewport centers the viewport on the given sprite.
  /// </summary>
  public class CenteredViewport : LoggedObject, IViewport
  {
    // The sprite the follow
    private Sprite sprite = null;

    /// <summary>
    /// Constructs a viewport to center on a specific sprite.
    /// </summary>
    public CenteredViewport(Sprite centerOnSprite)
    {
      sprite = centerOnSprite;
    }

    public Sprite Sprite
    {
      get { return sprite; }
      set
      {
	if (value == null)
	  throw new SpriteException("Cannot assign null sprite");

	sprite = value;
      }
    }

    /// <summary>
    /// This gets the upper-left corner of the viewport, based on the
    /// given coordinates of the actual screen. This enables a
    /// viewport to only show in a specific part of the screen. The
    /// point returned is relative to the sprite manager.
    /// </summary>
    public virtual void AdjustViewport(ref RenderArgs args)
    {
      // Get the midpoint of the surface
      int mx = GetMidX(args);
      int my = GetMidY(args);

      // Return the offset point
      args.TranslateX += mx - sprite.Coords.X;
      args.TranslateY += my - sprite.Coords.Y;
    }

    public int GetMidX(RenderArgs args)
    {
      return args.Size.Width / 2 - sprite.Size.Width / 2;
    }

    public int GetMidY(RenderArgs args)
    {
      return args.Size.Height / 2 - sprite.Size.Height / 2;
    }
  }
}
