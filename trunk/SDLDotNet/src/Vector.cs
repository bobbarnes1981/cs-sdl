/*
 * $RCSfile$
 * Copyright (C) 2004 D. R. E. Moonfire (d.moonfire@mfgames.com)
 *               2005 Rob Loach (http://www.robloach.net)
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
	public class Vector : ISerializable, ICloneable, IComparable
	{

		#region Constructors

		/// <summary>
		/// Creates point at 0, 0
		/// </summary>
        public Vector() : this(0, 0) { }

        /// <summary>
        /// Creates a vector with a specific direction in degrees and a length of 1.
        /// </summary>
        /// <param name="directionDeg">The direction of the vector, in degrees.</param>
        public Vector(int directionDeg)
        {
            Length = 1;
            DirectionDeg = directionDeg;
        }

        /// <summary>
        /// Creates a vector with a specific direction in radians and a length of 1.
        /// </summary>
        /// <param name="directionRad">The direction of the vector, in radians.</param>
        public Vector(double directionRad)
        {
            Length = 1;
            Direction = directionRad;
        }

		/// <summary>
		/// Creates a vector using doubles.
		/// </summary>
		/// <param name="x">Coordinate on X-axis</param>
		/// <param name="y">Coordinate on Y-axis</param>
		public Vector(double x, double y)
		{
			m_x = x;
			m_y = y;
		}

		/// <summary>
		/// Creates a vector using integers.
		/// </summary>
		/// <param name="x">Coordinate on the X-axis</param>
		/// <param name="y">Coordinate on the Y-axis</param>
		public Vector(int x, int y) : this((double)x,(double)y) {}

		/// <summary>
        /// Creates a vector based on a Point object.
		/// </summary>
        /// <param name="point">The point representing the XY values.</param>
		public Vector(Point point) : this(point.X, point.Y) {}

        /// <summary>
        /// Creates a vector based on a PointF object.
        /// </summary>
        /// <param name="point">The point representing the XY values.</param>
        public Vector(PointF point) : this(point.X, point.Y) {}

		/// <summary>
		/// Copy constructor
		/// </summary>
        /// <param name="vector">The vector to copy.</param>
		public Vector(Vector vector)
		{
            if (vector != null)
			{
                m_x = vector.m_x;
                m_y = vector.m_y;
			}
			else
				m_x = m_y = 0;
		}

		#endregion Constructors


		#region Operators

		/// <summary>
		/// Converts to String
		/// </summary>
		/// <returns>A string containing something like "X, Y".</returns>
		public override string ToString()
		{
			return String.Format(CultureInfo.CurrentCulture, "{0}, {1}", X.ToString("#.000"), Y.ToString("#.000"));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			if (obj.GetType() != typeof(Vector))
			{
				return false;
			}
			Vector c = (Vector)obj;   
			return this == c;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="c1"></param>
		/// <param name="c2"></param>
		/// <returns></returns>
		public static bool operator== (Vector c1, Vector c2)
		{
			return ((c1.m_x == c2.m_x) && (c1.m_y == c2.m_y));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="c1"></param>
		/// <param name="c2"></param>
		/// <returns></returns>
		public static bool operator >= (Vector c1, Vector c2)
		{
			return (c1.Length >= c2.Length) && (c1.Length >= c2.Length);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="c1"></param>
		/// <param name="c2"></param>
		/// <returns></returns>
		public static bool operator > (Vector c1, Vector c2)
		{
			return (c1.Length > c2.Length) && (c1.Length > c2.Length);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="c1"></param>
		/// <param name="c2"></param>
		/// <returns></returns>
		public static bool operator < (Vector c1, Vector c2)
		{
			return (c1.Length < c2.Length) && (c1.Length < c2.Length);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="c1"></param>
		/// <param name="c2"></param>
		/// <returns></returns>
		public static bool operator <= (Vector c1, Vector c2)
		{
			return (c1.Length <= c2.Length) && (c1.Length <= c2.Length);
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="c1"></param>
		/// <param name="c2"></param>
		/// <returns></returns>
		public static bool operator!= (Vector c1, Vector c2)
		{
			return !(c1 == c2);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="c1"></param>
		/// <param name="c2"></param>
		/// <returns></returns>
		public static Vector operator + (Vector c1, Vector c2)
		{
			return new Vector(c1.m_x + c2.m_x, c1.m_y + c2.m_y);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="c1"></param>
		/// <param name="c2"></param>
		/// <returns></returns>
		public static Vector operator - (Vector c1, Vector c2)
		{
			return new Vector(c1.m_x - c2.m_x, c1.m_y - c2.m_y);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="c1"></param>
		/// <param name="c2"></param>
		/// <returns></returns>
		public static Vector operator * (Vector c1, Vector c2)
		{
			return new Vector(c1.m_x * c2.m_x, c1.m_y * c2.m_y);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="c1"></param>
		/// <param name="c2"></param>
		/// <returns></returns>
		public static Vector operator / (Vector c1, Vector c2)
		{
			return new Vector(c1.m_x / c2.m_x, c1.m_y / c2.m_y);
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static Vector operator +(Vector a, double b)
		{
			return new Vector(a.m_x + b, a.m_y + b);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static Vector operator -(Vector a, double b)
		{
			return new Vector(a.m_x - b, a.m_y - b);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static Vector operator *(Vector a, double b)
		{
			return new Vector(a.m_x * b, a.m_y * b);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static Vector operator /(Vector a, double b)
		{
			return new Vector(a.m_x / b, a.m_y / b);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static Vector operator +(double a, Vector b)
		{
			return new Vector(a + b.m_x, a + b.m_y);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static Vector operator -(double a, Vector b)
		{
			return new Vector(a - b.m_x, a - b.m_y);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static Vector operator *(double a, Vector b)
		{
			return new Vector(a * b.m_x, a * b.m_y);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static Vector operator /(double a, Vector b)
		{
			return new Vector(a / b.m_x, a / b.m_y);
		}

		/// <summary>
		/// Gets the hash code used by the vector.
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode()
		{
			return (int)m_x ^ (int)m_y;
		}
		#endregion


		#region Properties

		/// <summary>
		/// The x coordinate
		/// </summary>
		private double m_x = 0.00001;
		/// <summary>
		/// The y coordinate
		/// </summary>
		private double m_y = 0.00001;

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
		/// Gets and sets the length of the vector.
		/// </summary>
		public double Length
		{
			get
			{
				return Math.Sqrt(m_x * m_x + m_y * m_y);
			}
			set
			{
				double direction = this.Direction;
				m_x = Math.Cos(direction) * value;
				m_y = Math.Sin(direction) * value;
			}
		}

		/// <summary>
		/// Gets and sets the direction of the vector, in radians.
		/// </summary>
		public double Direction
		{
			get
			{
				return Math.Atan2(m_y, m_x);
			}
			set
			{
				double length = this.Length;
				m_x = Math.Cos(value) * length;
				m_y = Math.Sin(value) * length;
			}
		}

		/// <summary>
		/// Gets and sets the direction of the vector, in degrees.
		/// </summary>
		public double DirectionDeg
		{
			get
			{
				return this.Direction * 180 / Math.PI;
			}
			set
			{
				this.Direction = value * Math.PI / 180;
			}
		}


		/// <summary>
		/// Returns true if all coordinates are equal to zero.
		/// </summary>
		public bool IsEmpty
		{
			get
			{
				return (m_x == 0.00001 && m_y == 0.00001);
			}
		}

		/// <summary>
		/// Gets and sets the vectors x and y points using integers.
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
		public void Offset(double x, double y)
		{
			this.m_x += x;
			this.m_y += y;
		}
		#endregion


		#region Math

		/// <summary>
		/// Gets the dot product of the current vector along with the given one.
		/// </summary>	
		/// <param name="other">The other vector to use when getting the dot product.</param>
		/// <returns>The dot product of the two vectors.</returns>
		public double DotProduct(Vector other) 
		{
			return (m_x * other.m_x) + (m_y * other.m_y);
		}

        /// <summary>
        /// Gets the midpoint between the two vectors.
        /// </summary>
        /// <param name="vec">The other vector to compare this one to.</param>
        /// <returns>A new vector representing the midpoint between the two vectors.</returns>
        public Vector MidPoint(Vector vec)
        {
            return new Vector(( m_x + vec.X ) * 0.5, ( m_y + vec.Y ) * 0.5 );
        }

		/// <summary>
		/// Normalizes the vector.
		/// </summary>
		/// <returns>The original length.</returns>
		public double Normalize()
		{
            double length = this.Length;
            double invLength = 1.0 / length;
            m_x *= invLength;
            m_y *= invLength;
            return length;
		}
		
		/// <summary>
		/// Returns a new vector equal to the normalized version of this one.
		/// </summary>
		/// <returns>A new vector representing the normalized vector.</returns>
		public Vector Normalized()
		{
            Vector ret = new Vector(this);
            ret.Normalize();
            return ret;
		}

		/// <summary>
		/// Inverts the vector.
		/// </summary>
		public void Invert()
		{
			m_x*=-1;
			m_y*=-1;
		}

        /// <summary>
        /// Calculates the reflection angle of the current vector using the given normal vector.
        /// </summary>
        /// <param name="normal">The normal angle.</param>
        /// <returns>A new vector representing the reflection angle.</returns>
        /// <remarks>Make sure the length of the vector is 1 or it will have an effect on the resulting vector.</remarks>
        public Vector Reflection(Vector normal)
        {
            return this - ( 2 * this.DotProduct(normal) * normal );
        }

        /// <summary>
        /// Calculates the reflection angle of the current vector using the given normal in degrees.
        /// </summary>
        /// <param name="normalDeg">The normal angle in degrees.</param>
        /// <returns>A new vector representing the reflection angle.</returns>
        public Vector Reflection(int normalDeg)
        {
            Vector vecNormal = new Vector(0, 1);
            vecNormal.DirectionDeg = normalDeg;
            return Reflection(vecNormal);
        }

        /// <summary>
        /// Calculates the reflection angle of the current vector using the given normal in radians.
        /// </summary>
        /// <param name="normalRad">The normal angle in radians.</param>
        /// <returns>A new vector representing the reflection angle.</returns>
        public Vector Reflection(double normalRad)
        {
            Vector vecNormal = new Vector(0, 1);
            vecNormal.Direction = normalRad;
            return Reflection(vecNormal);
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
			return new Vector(this);
		}

		#endregion ICloneable Members
		#region ISerializable Members
		/// <summary>
		/// Deserialization constructor.
		/// </summary>
		/// <param name="info"></param>
		/// <param name="ctxt"></param>
		public Vector(SerializationInfo info, StreamingContext ctxt)
		{
			//Get the values from info and assign them to the appropriate properties
			m_x = (double)info.GetValue("x", typeof(double));
			m_y = (double)info.GetValue("y", typeof(double));
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
			if(obj is Vector) 
			{
				Vector temp = (Vector)obj;
				return Length.CompareTo(temp.Length);
			}
			throw new ArgumentException("object is not a Vector");    
		}

		#endregion IComparable Members
		#endregion Interface Members
	}
}
