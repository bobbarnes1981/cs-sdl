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
using System.Globalization;

namespace SdlDotNet
{
	/// <summary>
	/// Class for coordinates in three dimensions.
	/// </summary>
	public class Vector
	{
		/// <summary>
		/// Creates point at 0, 0, 0
		/// </summary>
		public Vector()
		{
			this.x = 0;
			this.y = 0;
			this.z = 0;
		}

		/// <summary>
		/// Creates point on Z axis
		/// </summary>
		/// <param name="z">Coordinate on Z-axis</param>
		public Vector(int z)
		{
			this.x = 0;
			this.y = 0;
			this.z = z;
		}

		/// <summary>
		/// Creates point in X-Y plane. Z-axis equals zero
		/// </summary>
		/// <param name="x">Coordinate on X-axis</param>
		/// <param name="y">Coordinate on Y-axis</param>
		public Vector(int x, int y)
		{
			this.x = x;
			this.y = y;
			this.z = 0;
		}

		/// <summary>
		/// Creates point in XYZ space
		/// </summary>
		/// <param name="x">Coordinate on X-axis</param>
		/// <param name="y">Coordinate on Y-axis</param>
		/// <param name="z">Coordinate on Z-axis</param>
		public Vector(int x, int y, int z)
		{
			this.x = x;
			this.y = y;
			this.z = z;
		}

		/// <summary>
		/// Creates point in X-Y plane
		/// </summary>
		/// <param name="v">Point structure in X-Y plane</param>
		public Vector(Point v)
		{
			this.x = v.X;
			this.y = v.Y;
			this.z = 0;
		}

		/// <summary>
		/// Creates point in XYZ space.
		/// </summary>
		/// <param name="v">Point in X-Y plane</param>
		/// <param name="z">Coordinate on Z-axis</param>
		public Vector(Point v, int z)
		{
			this.x = v.X;
			this.y = v.Y;
			this.z = z;
		}

		#region Operators
		/// <summary>
		/// Converts to String
		/// </summary>
		/// <returns>string</returns>
		public override string ToString()
		{
			return String.Format(CultureInfo.CurrentCulture, "({0}, {1}, {2})", X, Y, Z);
		}
		#endregion

		#region Properties
		int x;
		int y;
		int z;
		/// <summary>
		/// Contains the x coordinate of the vector.
		/// </summary>
		public int X
		{
			get { return x; }
			set { x = value; }
		}

		/// <summary>
		/// Contains the x coordinate of the vector.
		/// </summary>
		public int Y
		{
			get { return y; }
			set { y = value; }
		}
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
