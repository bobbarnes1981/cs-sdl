using System;
using System.Drawing;

namespace MfGames.Utility
{
  /// <summary>
  /// A non-struct version of Rectangle in two dimensions.
  /// </summary>
  public class Rectangle2
  {
    public Rectangle2()
    {
    }

    public Rectangle2(Vector2 coords, Dimension2 size)
    {
      Coords = coords;
      Size = size;
    }

    public Rectangle2(Dimension2 size)
    {
      Coords = new Vector2(0, 0);
      Size = size;
    }

    public Rectangle2(int x, int y, int width, int height)
    {
      Coords = new Vector2(x, y);
      Size = new Dimension2(width, height);
    }

    public Rectangle2(Rectangle2 rect)
    {
      Coords = new Vector2(rect.Coords.X, rect.Coords.Y);
      Size = new Dimension2(rect.Size.Width, rect.Size.Height);
    }

    #region Geometry
    public bool IntersectsWith(Vector2 vector)
    {
      return vector.X >= Left && vector.X <= Right &&
	vector.Y >= Top && vector.Y <= Bottom;
    }

    public bool IntersectsWith(Rectangle2 rect)
    {
      // DO fix me
      return false;
    }
    #endregion

    #region Boundary Properties
    public int Left
    {
      get { return Coords.X; }
      set { Coords.X = value; }
    }

    public int Right
    {
      get { return Coords.X + Size.Width; }
    }

    public int Top
    {
      get { return Coords.Y; }
      set { Coords.Y = value; }
    }

    public int Bottom
    {
      get { return Coords.Y + Size.Height; }
    }

    public int Height
    {
      get { return Size.Height; }
      set { Size.Height = value; }
    }

    public int Width
    {
      get { return Size.Width; } 
      set { Size.Width = value; }
    }
    #endregion

    #region Operators
    /// <summary>
    /// A vector may be converted into a System.Drawing.Rectangle without
    /// any additional casting.
    /// </summary>
    public static implicit operator Rectangle(Rectangle2 rect)
    {
      return new Rectangle(rect.Coords, rect.Size);
    }

    public override string ToString()
    {
      return String.Format("({0},{1})", Coords, Size);
    }
    #endregion

    #region Properties
    private Dimension2 size = null;
    private Vector2 coords = null;

    /// <summary>
    /// Contains the coordinates of the rectangle.
    /// </summary>
    public Vector2 Coords
    {
      get { return coords; }
      set
      {
	if (value == null)
	  coords = new Vector2();
	else
	  coords = value;
      }
    }

    /// <summary>
    /// Contains the size of the dimension.
    /// </summary>
    public Dimension2 Size
    {
      get { return size; }
      set
      {
	if (value == null)
	  size = new Dimension2();
	else
	  size = value;
      }
    }
    #endregion
  }
}
