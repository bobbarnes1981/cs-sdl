using System;
using System.Drawing;

namespace MfGames.Utility
{
  /// <summary>
  /// A non-struct version of Size in two dimensions.
  /// </summary>
  public class Dimension2
  {
    public Dimension2()
    {
    }

    public Dimension2(int width, int height)
    {
      Width = width;
      Height = height;
    }

    public Dimension2(Size size)
    {
      Width = size.Width;
      Height = size.Height;
    }


    #region Operators
    /// <summary>
    /// A vector may be converted into a System.Drawing.Size without
    /// any additional casting.
    /// </summary>
    public static implicit operator Size(Dimension2 dimension)
    {
      return new Size(dimension.Width, dimension.Height);
    }

    public override string ToString()
    {
      return String.Format("({0}x{1})", Width, Height);
    }
    #endregion

    #region Properties
    private int width = 0;
    private int height = 0;

    /// <summary>
    /// Contains the height of the dimension.
    /// </summary>
    public int Height
    {
      get { return height; }
      set { height = value; }
    }

    /// <summary>
    /// Contains the width of the dimension.
    /// </summary>
    public int Width
    {
      get { return width; }
      set { width = value; }
    }
    #endregion
  }
}
