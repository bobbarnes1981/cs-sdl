using System.Drawing;

namespace MfGames.Sdl.Sprites
{
  /// <summary>
  /// The translated viewport just translates all sprites by a given amount.
  /// </summary>
  public class TranslatedViewport : IViewport
  {
    private int tx = 0;
    private int ty = 0;

    /// <summary>
    /// Constructs a viewport to center on a specific sprite.
    /// </summary>
    public TranslatedViewport(int tx, int ty)
    {
      this.tx = tx;
      this.ty = ty;
    }

    /// <summary>
    /// This gets the upper-left corner of the viewport, based on the
    /// given coordinates of the actual screen. This enables a
    /// viewport to only show in a specific part of the screen. The
    /// point returned is relative to the sprite manager.
    /// </summary>
    public void AdjustViewport(ref RenderArgs args)
    {
      args.TranslateX += tx;
      args.TranslateY += ty;
    }
  }
}
