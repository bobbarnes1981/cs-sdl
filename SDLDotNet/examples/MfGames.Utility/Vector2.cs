using System;
using System.Drawing;

namespace MfGames.Utility
{
  /// <summary>
  /// A non-structure version of the Point structure. This is more
  /// suited toward being placed as a property. This is implemented as
  /// a two-dimensional vector.
  /// </summary>
  public class Vector2
  {
    public Vector2()
    {
    }

    public Vector2(int x, int y)
    {
      X = x;
      Y = y;
    }

    public Vector2(Vector2 v)
    {
      X = v.X;
      Y = v.Y;
    }

    #region Operators
    /// <summary>
    /// A vector may be converted into a System.Drawing.Point without
    /// any additional casting.
    /// </summary>
    public static implicit operator Point(Vector2 vector)
    {
      return new Point(vector.X, vector.Y);
    }

    public override string ToString()
    {
      return String.Format("({0},{1})", X, Y);
    }
    #endregion

    #region Properties
    private int x = 0;
    private int y = 0;

    /// <summary>
    /// Contains the x coordinate of the vector.
    /// </summary>
    public int X
    {
      get { return x; }
      set { x = value; }
    }

    /// <summary>
    /// Contains the y coordinate of the vector.
    /// </summary>
    public int Y
    {
      get { return y; }
      set { y = value; }
    }
    #endregion
  }
}
