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
