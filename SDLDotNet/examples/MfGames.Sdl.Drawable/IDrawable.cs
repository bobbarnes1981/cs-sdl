using MfGames.Utility;
using SdlDotNet;

namespace MfGames.Sdl.Drawable
{
  /// <summary>
  /// Defines the interface to the generic drawable system. These
  /// drawables have multiple uses, ranging from representing an image
  /// in memory or some data value, to more specific operations, such
  /// as adding a red filter to the image.
  /// </summary>
  public interface IDrawable
  {
    #region Frames
    /// <summary>
    /// This gives an accessor to the surfaces of this
    /// drawable. This should throw an exception of there is no
    /// surface, but the system assumes that if there is at least one
    /// valid surface, then a drawable will always return the
    /// surface. This allows the system to request image #12 from a
    /// drawable with only one frame and get that same frame.
    /// </summary>
    Surface this[int frame] { get; }

    /// <summary>
    /// Property that contains the number of frames (surfaces) in the
    /// drawable.
    /// </summary>
    int FrameCount { get; }

    /// <summary>
    /// Returns a cache key which represents a unique identifier of
    /// the drawable. This is used by CachedDrawable to keep the CPU
    /// generation low.
    /// </summary>
    string GetCacheKey(int frame);
    #endregion

    #region Geometry
    /// <summary>
    /// Returns the size of the drawable, in pixels.
    /// </summary>
    Dimension2 Size { get; }
    #endregion
  }
}
