/*
 * $RCSfile$
 * Copyright (C) 2004 D. R. E. Moonfire (d.moonfire@mfgames.com)
 *
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later version.
 * 
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 * 
 * You should have received a copy of the GNU Lesser General Public
 * License along with this library; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
 */

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
