using MfGames.Utility;
using SdlDotNet;
using System.Drawing;

namespace MfGames.Sdl.Drawable
{
  /// <summary>
  /// One of the core drawables, the ImageDrawable is used to load an
  /// image into memory and use it as a drawable. Unlike most
  /// drawables, the same image data is returned with all requests, so
  /// care must be taken not to change the image data.
  /// </summary>
  public class ImageDrawable : IDrawable
  {
    public ImageDrawable(string imageFile)
    {
      // Save the fields
      this.filename = imageFile;
      
      // Load the image
      image = new Surface(imageFile);
    }

    #region Frames
    /// <summary>
    /// This gives an accessor to the surfaces of this
    /// drawable. This should throw an exception of there is no
    /// surface, but the system assumes that if there is at least one
    /// valid surface, then a drawable will always return the
    /// surface. This allows the system to request image #12 from a
    /// drawable with only one frame and get that same frame.
    /// </summary>
    public Surface this[int frame] { get { return image; } }

    /// <summary>
    /// Property that contains the number of frames (surfaces) in the
    /// drawable.
    /// </summary>
    public int FrameCount { get { return 1; } }

    /// <summary>
    /// Returns a cache key which represents a unique identifier of
    /// the drawable. This is used by CachedDrawable to keep the CPU
    /// generation low.
    /// </summary>
    public string GetCacheKey(int frame) { return filename; }
    #endregion

    #region Properties
    private string filename = null;

    private Surface image = null;

    /// <summary>
    /// Returns the height of the drawable, in pixels.
    /// </summary>
    public Dimension2 Size
    {
      get { return new Dimension2(image.Width, image.Height); }
    }
    #endregion
  }
}
