/*
 * $RCSfile: Vector3D.cs,v $
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
using System.Runtime.Serialization;

namespace SdlDotNet
{
	/// <summary>
	/// Class for coordinates in three dimensions.
	/// </summary>
	[Serializable]
	public class Vector3D : ISerializable, ICloneable, IComparable
	{

		#region Constructors

		/// <summary>
		/// Creates point at 0, 0, 0
		/// </summary>
		public Vector3D()
		{
			m_x = m_y = m_z = 0;
		}

		/// <summary>
		/// Creates point on Z axis
		/// </summary>
		/// <param name="z">Coordinate on Z-axis</param>
		public Vector3D(double z)
		{
			this.m_x = 0;
			this.m_y = 0;
			this.m_z = z;
		}

		/// <summary>
		/// Creates point in X-Y plane. Z-axis equals zero
		/// </summary>
		/// <param name="x">Coordinate on X-axis</param>
		/// <param name="y">Coordinate on Y-axis</param>
		public Vector3D(double x, double y)
		{
			this.m_x = x;
			this.m_y = y;
			this.m_z = 0;
		}

		/// <summary>
		/// Creates point in XYZ space
		/// </summary>
		/// <param name="x">Coordinate on X-axis</param>
		/// <param name="y">Coordinate on Y-axis</param>
		/// <param name="z">Coordinate on Z-axis</param>
		public Vector3D(double x, double y, double z)
		{
			this.m_x = x;
			this.m_y = y;
			this.m_z = z;
		}

		/// <summary>
		/// Creates point in X-Y plane
		/// </summary>
		/// <param name="v">Point structure in X-Y plane</param>
		public Vector3D(Point v)
		{
			this.m_x = v.X;
			this.m_y = v.Y;
			this.m_z = 0;
		}

		/// <summary>
		/// Creates point in XYZ space.
		/// </summary>
		/// <param name="v">Point in X-Y plane</param>
		/// <param name="z">Coordinate on Z-axis</param>
		public Vector3D(Point v, double z)
		{
			this.m_x = v.X;
			this.m_y = v.Y;
			this.m_z = z;
		}

		/// <summary>
		/// Copy constructor
		/// </summary>
		/// <param name="vIn">The set values to use in the new vector.</param>
		public Vector3D(Vector3D vIn)
		{
			if(vIn != null)
			{
				m_x = vIn.m_x;
				m_y = vIn.m_y;
				m_z = vIn.m_z;
			}
			else
				m_x = m_y = m_z = 0;
		}

		#endregion Constructors


		#region Operators

		/// <summary>
		/// Converts to String
		/// </summary>
		/// <returns>A string containing something like "X, Y, Z".</returns>
		public override string ToString()
		{
			return String.Format(CultureInfo.CurrentCulture, "({0}, {1}, {2})", X.ToString("D.000"), Y.ToString("D.000"), Z.ToString("D.000"));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			if (obj.GetType() != typeof(Vector3D))
			{
				return false;
			}
			Vector3D c = (Vector3D)obj;   
			return this == c;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="c1"></param>
		/// <param name="c2"></param>
		/// <returns></returns>
		public static bool operator== (Vector3D c1, Vector3D c2)
		{
			return ((c1.m_x == c2.m_x) && (c1.m_y == c2.m_y) && (c1.m_z == c2.m_z));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="c1"></param>
		/// <param name="c2"></param>
		/// <returns></returns>
		public static bool operator >= (Vector3D c1, Vector3D c2)
		{
			return (c1.Length >= c2.Length) && (c1.Length >= c2.Length);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="c1"></param>
		/// <param name="c2"></param>
		/// <returns></returns>
		public static bool operator > (Vector3D c1, Vector3D c2)
		{
			return (c1.Length > c2.Length) && (c1.Length > c2.Length);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="c1"></param>
		/// <param name="c2"></param>
		/// <returns></returns>
		public static bool operator < (Vector3D c1, Vector3D c2)
		{
			return (c1.Length < c2.Length) && (c1.Length < c2.Length);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="c1"></param>
		/// <param name="c2"></param>
		/// <returns></returns>
		public static bool operator <= (Vector3D c1, Vector3D c2)
		{
			return (c1.Length <= c2.Length) && (c1.Length <= c2.Length);
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="c1"></param>
		/// <param name="c2"></param>
		/// <returns></returns>
		public static bool operator!= (Vector3D c1, Vector3D c2)
		{
			return !(c1 == c2);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="c1"></param>
		/// <param name="c2"></param>
		/// <returns></returns>
		public static Vector3D operator + (Vector3D c1, Vector3D c2)
		{
			return new Vector3D(c1.m_x + c2.m_x, c1.m_y + c2.m_y, c1.m_z + c2.m_z);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static Vector3D operator +(Vector3D a, double b)
		{
			return new Vector3D(a.m_x + b, a.m_y + b, a.m_z + b);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static Vector3D operator -(Vector3D a, double b)
		{
			return new Vector3D(a.m_x - b, a.m_y - b, a.m_z - b);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static Vector3D operator *(Vector3D a, double b)
		{
			return new Vector3D(a.m_x * b, a.m_y * b, a.m_z * b);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static Vector3D operator /(Vector3D a, double b)
		{
			return new Vector3D(a.m_x / b, a.m_y / b, a.m_z / b);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static Vector3D operator +(double a, Vector3D b)
		{
			return new Vector3D(a + b.m_x, a + b.m_y, a + b.m_z);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static Vector3D operator -(double a, Vector3D b)
		{
			return new Vector3D(a - b.m_x, a - b.m_y, a - b.m_z);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static Vector3D operator *(double a, Vector3D b)
		{
			return new Vector3D(a * b.m_x, a * b.m_y, a * b.m_z);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static Vector3D operator /(double a, Vector3D b)
		{
			return new Vector3D(a / b.m_x, a / b.m_y, a / b.m_z);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="c1"></param>
		/// <param name="c2"></param>
		/// <returns></returns>
		public static Vector3D operator - (Vector3D c1, Vector3D c2)
		{
			return new Vector3D(c1.m_x - c2.m_x, c1.m_y - c2.m_y, c1.m_z - c2.m_z);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="c1"></param>
		/// <param name="c2"></param>
		/// <returns></returns>
		public static Vector3D operator * (Vector3D c1, Vector3D c2)
		{
			return new Vector3D(c1.m_x * c2.m_x, c1.m_y * c2.m_y, c1.m_z * c2.m_z);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="c1"></param>
		/// <param name="c2"></param>
		/// <returns></returns>
		public static Vector3D operator / (Vector3D c1, Vector3D c2)
		{
			return new Vector3D(c1.m_x / c2.m_x, c1.m_y / c2.m_y, c1.m_z / c2.m_z);
		}

		/// <summary>
		/// Gets the hash code used by the vector.
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode()
		{
			return (int)m_x ^ (int)m_y ^ (int)m_z;
		}
		#endregion


		#region Properties

		/// <summary>
		/// The x coordinate
		/// </summary>
		private double m_x;
		/// <summary>
		/// The y coordinate
		/// </summary>
		private double m_y;
		/// <summary>
		/// The z coordinate
		/// </summary>
		private double m_z;

		/// <summary>
		/// Contains the x coordinate of the vector.
		/// </summary>
		public double X
		{
			get { return m_x; }
			set { m_x = value; }
		}

		/// <summary>
		/// Contains the y coordinate of the vector.
		/// </summary>
		public double Y
		{
			get { return m_y; }
			set { m_y = value; }
		}
		/// <summary>
		/// Contains the z coordinate of the vector.
		/// </summary>
		public double Z
		{
			get { return m_z; }
			set { m_z = value; }
		}

		/// <summary>
		/// Gets and sets the length of the vector.
		/// </summary>
		public double Length
		{
			get
			{
				return Math.Sqrt(m_x * m_x + m_y * m_y + m_z * m_z);
			}
			set
			{
				// TODO: Set length of 3D vector.
			}
		}

		/// <summary>
		/// Returns true if all coordinates are equal to zero.
		/// </summary>
		public bool IsEmpty
		{
			get
			{
				return (m_x == 0 && m_y == 0 && m_z == 0);
			}
		}

		/// <summary>
		/// Gets and sets the vectors x and y points.
		/// </summary>
		public Point Point
		{
			get
			{
				return new Point((int)m_x, (int)m_y);
			}
			set
			{
				m_x = value.X;
				m_y = value.Y;
			}
		}

		/// <summary>
		/// Offsets the vector by the given x, y and z coordinates.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		public void Offset(double x, double y, double z)
		{
			this.m_x += x;
			this.m_y += y;
			this.m_z += z;
		}
		#endregion


		#region Math

		/// <summary>
		/// Creates a new vector describing the cross product of two vectors.
		/// </summary>
		/// <param name="other">The other vector to use when getting the cross product.</param>
		/// <example><code>
		/// Vector a = new Vector(50, 25, 10);
		/// Vector b = new Vector(25, 50, 100);
		/// Vector cross = a.CrossProduct(b);
		/// </code></example>
		public Vector3D CrossProduct(Vector3D other)
		{
			double xh = m_y * other.m_z - other.m_y * m_z;
			double yh = m_z * other.m_x - other.m_z * m_x;
			double zh = m_x * other.m_y - other.m_x * m_y;
			return new Vector3D(xh, yh, zh);
		}

		/// <summary>
		/// Inverts the vector
		/// </summary>
		public void Invert()
		{
			m_x*=-1;
			m_y*=-1;
			m_z*=-1;
		}

		/// <summary>
		/// Gets the dot product of the current vector along with the given one.
		/// </summary>	
		/// <param name="other">The other vector to use when getting the dot product.</param>
		/// <returns>The dot product of the two vectors.</returns>
		public double DotProduct(Vector3D other) 
		{
			return (m_x * other.m_x) + (m_y * other.m_y) + (m_z * other.m_z);
		}

		/// <summary>
		/// Normalizes the vector, making the length equal to one.
		/// </summary>
		/// <returns></returns>
		public void Normalize()
		{
			double len = this.Length;
			m_x /= len;
			m_y /= len;
			m_z /= len;
		}
		
		/// <summary>
		/// Returns a new vector equal to the normalized version of this one.
		/// </summary>
		/// <returns></returns>
		public Vector3D Normalized()
		{
			double len = this.Length;
			return new Vector3D(m_x / len, m_y / len, m_z / len);
		}

		#endregion Math


		#region Interface Members
		#region ICloneable Members

		/// <summary>
		/// Clones the base object vector.
		/// </summary>
		/// <returns>A new instance with the same values.</returns>
		public Object Clone()
		{
			return new Vector3D(this);
		}

		#endregion ICloneable Members
		#region ISerializable Members
		/// <summary>
		/// Deserialization constructor.
		/// </summary>
		/// <param name="info"></param>
		/// <param name="ctxt"></param>
		public Vector3D(SerializationInfo info, StreamingContext ctxt)
		{
			//Get the values from info and assign them to the appropriate properties
			m_x = (double)info.GetValue("x", typeof(double));
			m_y = (double)info.GetValue("y", typeof(double));
			m_z = (double)info.GetValue("z", typeof(double));
		}

		/// <summary>
		/// Serialization function
		/// </summary>
		/// <param name="info"></param>
		/// <param name="ctxt"></param>
		public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
		{
			//You can use any custom name for your name-value pair. But make sure you
			// read the values with the same name. For ex:- If you write EmpId as "EmployeeId"
			// then you should read the same with "EmployeeId"
			info.AddValue("x", m_x);
			info.AddValue("y", m_y);
			info.AddValue("z", m_z);
		}
		#endregion ISerializable Members
		#region IComparable Members

		/// <summary>
		/// IComparable.CompareTo implementation.
		/// </summary>
		/// <param name="obj">The object to compare to.</param>
		/// <returns>-1 if its length is less then obj, 0 if it's equal and 1 if it's greater.</returns>
		public int CompareTo(object obj)
		{
			if(obj is Vector3D) 
			{
				Vector3D temp = (Vector3D)obj;
				return Length.CompareTo(temp.Length);
			}
			throw new ArgumentException("object is not a Vector3D");    
		}

		#endregion IComparable Members
		#endregion Interface Members
	}
}
