using MfGames.Utility;
using System.Drawing;

namespace MfGames.Sdl.Sprites
{
  /// <summary>
  /// This viewport only draws and displays sprites in a specific
  /// viewport. It works as a combination translating viewport (moving
  /// things down) and clipping the outersides.
  /// </summary>
  public class LimitedViewport : LoggedObject, IViewport
  {
    private Rectangle rect = Rectangle.Empty;

    /// <summary>
    /// Constructs a viewport.
    /// </summary>
    public LimitedViewport(Rectangle area)
    {
      this.rect = area;
    }

    /// <summary>
    /// This gets the upper-left corner of the viewport, based on the
    /// given coordinates of the actual screen. This enables a
    /// viewport to only show in a specific part of the screen. The
    /// point returned is relative to the sprite manager.
    /// </summary>
    public void AdjustViewport(ref RenderArgs args)
    {
      Error("Cannot process viewport");
    }
  }
}
