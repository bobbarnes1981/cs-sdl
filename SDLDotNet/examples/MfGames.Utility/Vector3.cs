using System;
using System.Drawing;

namespace MfGames.Utility
{
  /// <summary>
  /// Extends the Vector2 class with a third dimension, Z.
  /// </summary>
  public class Vector3 : Vector2
  {
    public Vector3()
    {
    }

    public Vector3(int z)
      : base()
    {
      Z = z;
    }

    public Vector3(int x, int y)
      : base(x, y)
    {
    }

    public Vector3(int x, int y, int z)
      : base(x, y)
    {
      Z = z;
    }

    public Vector3(Vector2 v)
      : base(v)
    {
    }

    public Vector3(Vector2 v, int z)
      : base(v)
    {
      Z = z;
    }

    #region Operators
    public override string ToString()
    {
      return String.Format("({0},{1},{2})", X, Y, Z);
    }
    #endregion

    #region Properties
    private int z = 0;

    /// <summary>
    /// Contains the x coordinate of the vector.
    /// </summary>
    public int Z
    {
      get { return z; }
      set { z = value; }
    }
    #endregion
  }
}
