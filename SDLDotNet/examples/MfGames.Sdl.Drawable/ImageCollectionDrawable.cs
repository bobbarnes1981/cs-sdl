using MfGames.Utility;
using SdlDotNet;
using System.Collections;
using System.Drawing;
using System.IO;

namespace MfGames.Sdl.Drawable
{
  /// <summary>
  /// Creates a drawable from a collection of ImageDrwables. This is
  /// assumes that there are image names such as base-0.png,
  /// base-1.png, and so on. The extension is anything SDL can handle.
  /// </summary>
  public class ImageCollectionDrawable : LoggedObject, IDrawable
  {
    public ImageCollectionDrawable(string baseName, string extension)
    {
      // Save the fields
      this.filename = baseName + "-*" + extension;
      
      // Load the images into memory
      int i = 0;

      while (true)
      {
	// Check the file
	string fn = baseName + "-" + i + extension;

	if (!File.Exists(fn))
	  break;

	// Load it
	//Debug("Loading " + fn);
	frames.Add(new Surface(fn));
	i++;
      }

      Debug("Loaded " + i + " frames of " + baseName);
    }

    #region Frames
    private ArrayList frames = new ArrayList();

    /// <summary>
    /// This gives an accessor to the surfaces of this
    /// drawable. This should throw an exception of there is no
    /// surface, but the system assumes that if there is at least one
    /// valid surface, then a drawable will always return the
    /// surface. This allows the system to request image #12 from a
    /// drawable with only one frame and get that same frame.
    /// </summary>
    public Surface this[int frame]
    {
      get { return (Surface) frames[(int) frame % frames.Count]; }
    }

    /// <summary>
    /// Property that contains the number of frames (surfaces) in the
    /// drawable.
    /// </summary>
    public int FrameCount { get { return frames.Count; } }

    /// <summary>
    /// Returns a cache key which represents a unique identifier of
    /// the drawable. This is used by CachedDrawable to keep the CPU
    /// generation low.
    /// </summary>
    public string GetCacheKey(int frame) { return filename; }
    #endregion

    #region Properties
    private string filename = null;

    /// <summary>
    /// Returns the height of the drawable, in pixels.
    /// </summary>
    public Dimension2 Size
    {
      get { return new Dimension2(this[0].Width, this[0].Height); }
    }
    #endregion
  }
}
