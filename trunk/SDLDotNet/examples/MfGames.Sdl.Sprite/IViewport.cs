using System.Drawing;

namespace MfGames.Sdl.Sprites
{
  /// <summary>
  /// A viewport is a way of moving the sprite view around the sprite
  /// manager. This can be as simple as being centered on a sprite or
  /// something more complicated involving a scriptable interface.
  /// </summary>
  public interface IViewport
  {
    /// <summary>
    /// This returns the viewport of the system, which changes what is
    /// viewable in the sprite manager.
    /// </summary>
    void AdjustViewport(ref RenderArgs args);
  }
}
