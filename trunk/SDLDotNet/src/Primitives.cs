/*
 * $RCSfile$
 * Copyright (C) 2004, 2005 David Hudson (jendave@yahoo.com)
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
using System.Collections;
using System.Drawing;
using System.Globalization;


namespace SdlDotNet
{
	/// <summary>
	/// Circle Primitive
	/// </summary>
	public struct Circle : IPrimitive
	{
		short x;
		short y;
		short r;

		/// <summary>
		/// Constructor for Circle
		/// </summary>
		/// <param name="x">X coordinate of Center</param>
		/// <param name="y">Y coordinate of Center</param>
		/// <param name="r">Radius</param>
		public Circle(short x, short y, short r)
		{
			this.x = x;
			this.y = y;
			this.r = r;
		}

		/// <summary>
		/// Constructor for Circle
		/// </summary>
		/// <param name="p">Center point</param>
		/// <param name="r">Radius</param>
		public Circle(Point p, short r)
		{
			this.x = (short)p.X;
			this.y = (short)p.Y;
			this.r = r;
		}

		/// <summary>
		/// Center of circle
		/// </summary>
		public Point Center
		{
			get
			{
				return new Point(this.x, this.y);
			}
			set
			{
				this.x = (short)value.X;
				this.y = (short)value.Y;
			}
		}

		/// <summary>
		/// X position of circle
		/// </summary>
		public short XPosition
		{
			get
			{
				return this.x;
			}
			set
			{
				this.x = value;
			}
		}

		/// <summary>
		/// Y position of circle
		/// </summary>
		public short YPosition
		{
			get
			{
				return this.y;
			}
			set
			{
				this.y = value;
			}
		}

		/// <summary>
		/// Radius of circle
		/// </summary>
		public short Radius
		{
			get
			{
				return this.r;
			}
			set
			{
				this.r = value;
			}
		}
		/// <summary>
		/// String representation of circle
		/// </summary>
		/// <returns>string representation of circle</returns>
		public override string ToString()
		{
			return String.Format(CultureInfo.CurrentCulture, "({0},{1}, {2})", x, y, r);
		}
		/// <summary>
		/// Equals operator
		/// </summary>
		/// <param name="obj">Circle to compare</param>
		/// <returns>true if circles are equal</returns>
		public override bool Equals(object obj)
		{
			if (obj.GetType() != typeof(Circle))
				return false;
                
			Circle c = (Circle)obj;   
			return ((this.x == c.x) && (this.y == c.y) && (this.r == c.r));
		}

		/// <summary>
		/// Equals operator
		/// </summary>
		/// <param name="c1">Circle to compare</param>
		/// <param name="c2">Circle to compare</param>
		/// <returns>True if circles are equal</returns>
		public static bool operator== (Circle c1, Circle c2)
		{
			return ((c1.x == c2.x) && (c1.y == c2.y) && (c1.r == c2.r));
		}

		/// <summary>
		/// Not equals operator
		/// </summary>
		/// <param name="c1">Circle to compare</param>
		/// <param name="c2">Circle to compare</param>
		/// <returns>True if circles are not equal</returns>
		public static bool operator!= (Circle c1, Circle c2)
		{
			return !(c1 == c2);
		}

		/// <summary>
		/// Hash Code
		/// </summary>
		/// <returns>Hash code</returns>
		public override int GetHashCode()
		{
			return x ^ y ^ r;

		}
	}

	/// <summary>
	/// Ellipse Primitive
	/// </summary>
	public struct Ellipse : IPrimitive
	{
		short x;
		short y;
		short radiusX;
		short radiusY;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="x">X coordinate of center</param>
		/// <param name="y">Y coordinate of center</param>
		/// <param name="radiusX">Radius on X axis</param>
		/// <param name="radiusY">Radius on Y axis</param>
		public Ellipse(short x, short y, short radiusX, short radiusY)
		{
			this.x = x;
			this.y = y;
			this.radiusX = radiusX;
			this.radiusY = radiusY;
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="point">Center</param>
		/// <param name="size">Width and height of ellipse</param>
		public Ellipse(Point point, Size size)
		{
			this.x = (short)point.X;
			this.y = (short)point.Y;
			this.radiusX = (short)size.Width;
			this.radiusY = (short)size.Height;
		}

		/// <summary>
		/// Center of ellipse
		/// </summary>
		public Point Center
		{
			get
			{
				return new Point(this.x, this.y);
			}
			set
			{
				this.x = (short)value.X;
				this.y = (short)value.Y;
			}
		}

		/// <summary>
		/// Size struct representing width and height of ellipse
		/// </summary>
		public Size Size
		{
			get
			{
				return new Size(this.radiusX, this.radiusY);
			}
			set
			{
				this.radiusX = (short)value.Width;
				this.radiusY = (short)value.Height;
			}
		}

		/// <summary>
		/// X position of center of ellipse
		/// </summary>
		public short XPosition
		{
			get
			{
				return this.x;
			}
			set
			{
				this.x = value;
			}
		}

		/// <summary>
		/// Y position of center of ellipse
		/// </summary>
		public short YPosition
		{
			get
			{
				return this.y;
			}
			set
			{
				this.y = value;
			}
		}

		/// <summary>
		/// Width of ellipse
		/// </summary>
		public short RadiusX
		{
			get
			{
				return this.radiusX;
			}
			set
			{
				this.radiusX = value;
			}
		}

		/// <summary>
		/// Height of ellipse
		/// </summary>
		public short RadiusY
		{
			get
			{
				return this.radiusY;
			}
			set
			{
				this.radiusY = value;
			}
		}
		/// <summary>
		/// String representation of ellipse
		/// </summary>
		/// <returns>string representation of ellipse</returns>
		public override string ToString()
		{
			return String.Format(
				CultureInfo.CurrentCulture,
				"({0},{1}, {2}, {3}, {4})", x, y, radiusX, radiusY);
		}
		/// <summary>
		/// Equals operator
		/// </summary>
		/// <param name="obj">Ellipse to compare</param>
		/// <returns>True if ellipses are equal</returns>
		public override bool Equals(object obj)
		{
			if (obj.GetType() != typeof(Ellipse))
				return false;
                
			Ellipse e = (Ellipse)obj;   
			return (
				(this.x == e.x) && 
				(this.y == e.y) && 
				(this.radiusX == e.radiusX) && 
				(this.radiusY == e.radiusY)
				);
		}

		/// <summary>
		/// Equals operator
		/// </summary>
		/// <param name="e1">Ellipse to compare</param>
		/// <param name="e2">Ellipse to compare</param>
		/// <returns>True if ellipses are equal</returns>
		public static bool operator== (Ellipse e1, Ellipse e2)
		{
			return (
				(e1.x == e2.x) && 
				(e1.y == e2.y) && 
				(e1.radiusX == e2.radiusX) && 
				(e1.radiusY == e2.radiusY)
				);
		}
		
		/// <summary>
		/// Not equals operator
		/// </summary>
		/// <param name="e1">Ellipse to compare</param>
		/// <param name="e2">Ellipse to compare</param>
		/// <returns>True if ellipses are not equal</returns>
		public static bool operator!= (Ellipse e1, Ellipse e2)
		{
			return !(e1 == e2);
		}

		/// <summary>
		/// Hash code
		/// </summary>
		/// <returns>hash code</returns>
		public override int GetHashCode()
		{
			return x ^ y ^ radiusX ^ radiusY;

		}
	}

	/// <summary>
	/// Line primitive
	/// </summary>
	public struct Line : IPrimitive
	{
		short x1;
		short y1;
		short x2;
		short y2;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="x1">X coordinate of first point</param>
		/// <param name="y1">Y coordinate of first point</param>
		/// <param name="x2">X coordinate of second point</param>
		/// <param name="y2">Y coordinate of second point</param>
		public Line(short x1, short y1, short x2, short y2)
		{
			this.x1 = x1;
			this.y1 = y1;
			this.x2 = x2;
			this.y2 = y2;
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="point1">First point</param>
		/// <param name="point2">Second point</param>
		public Line(Point point1, Point point2)
		{
			this.x1 = (short)point1.X;
			this.y1 = (short)point1.Y;
			this.x2 = (short)point2.X;
			this.y2 = (short)point2.Y;
		}

		/// <summary>
		/// X position of first point
		/// </summary>
		public short XPosition1
		{
			get
			{
				return this.x1;
			}
			set
			{
				this.x1 = value;
			}
		}

		/// <summary>
		/// Y position of first point
		/// </summary>
		public short YPosition1
		{
			get
			{
				return this.y1;
			}
			set
			{
				this.y1 = value;
			}
		}
		/// <summary>
		/// X position of second point
		/// </summary>
		public short XPosition2
		{
			get
			{
				return this.x2;
			}
			set
			{
				this.x2 = value;
			}
		}

		/// <summary>
		/// Y position of second point
		/// </summary>
		public short YPosition2
		{
			get
			{
				return this.y2;
			}
			set
			{
				this.y2 = value;
			}
		}

		/// <summary>
		/// First point
		/// </summary>
		public Point Point1
		{
			get
			{
				return new Point(this.x1, this.y1);
			}
			set
			{
				this.x1 = (short)value.X;
				this.y1 = (short)value.Y;
			}
		}

		/// <summary>
		/// Second point
		/// </summary>
		public Point Point2
		{
			get
			{
				return new Point(this.x2, this.y2);
			}
			set
			{
				this.x2 = (short)value.X;
				this.y2 = (short)value.Y;
			}
		}

		/// <summary>
		/// Set to vertical line
		/// </summary>
		public void Vertical()
		{
			XPosition2 = XPosition1;
		}

		/// <summary>
		/// Set to horizontal line
		/// </summary>
		public void Horizontal()
		{
			YPosition2 = YPosition1;
		}

		/// <summary>
		/// String representation of line
		/// </summary>
		/// <returns>String represenation of line</returns>
		public override string ToString()
		{
			return String.Format(CultureInfo.CurrentCulture, "({0},{1}, {2}, {3})", x1, y1, x2, y2);
		}
		/// <summary>
		/// Equals operator
		/// </summary>
		/// <param name="obj">line to compare</param>
		/// <returns>True if lines are equal</returns>
		public override bool Equals(object obj)
		{
			if (obj.GetType() != typeof(Line))
				return false;
                
			Line line = (Line)obj;   
			return (
				(this.x1 == line.x1) && 
				(this.y1 == line.y1) && 
				(this.x2 == line.x2) && 
				(this.y2 == line.y2)
				);
		}

		/// <summary>
		/// Equals operator
		/// </summary>
		/// <param name="line1">Line to compare</param>
		/// <param name="line2">Line to compare</param>
		/// <returns>True if lines are equal</returns>
		public static bool operator== (Line line1, Line line2)
		{
			return (
				(line1.x1 == line2.x1) && 
				(line1.y1 == line2.y1) && 
				(line1.x2 == line2.x2) && 
				(line1.y2 == line2.y2)
				);
		}
		
		/// <summary>
		/// Not equals operator
		/// </summary>
		/// <param name="line1">Line to compare</param>
		/// <param name="line2">Line to compare</param>
		/// <returns>True if lines are equal</returns>
		public static bool operator!= (Line line1, Line line2)
		{
			return !(line1 == line2);
		}

		/// <summary>
		/// hash code
		/// </summary>
		/// <returns>hash code</returns>
		public override int GetHashCode()
		{
			return x1 ^ y1 ^ x2 ^ y2;

		}
		#region IPrimitive Members

		/// <summary>
		/// Center of line
		/// </summary>
		public Point Center
		{
			get
			{
				return new Point((x1 + x2)/2, (y1 + y2)/2);
			}
			set
			{
				this.x1 += (short)(value.X - (x1 + x2)/2);
				this.x2 += (short)(value.X - (x1 + x2)/2);
				this.y1 += (short)(value.Y - (y1 + y2)/2);
				this.y2 += (short)(value.Y - (y1 + y2)/2);
			}
		}

		#endregion
	}

	/// <summary>
	/// Triangle primitive
	/// </summary>
	public struct Triangle : IPrimitive
	{
		short x1;
		short y1;
		short x2;
		short y2;
		short x3;
		short y3;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="x1"></param>
		/// <param name="y1"></param>
		/// <param name="x2"></param>
		/// <param name="y2"></param>
		/// <param name="x3"></param>
		/// <param name="y3"></param>
		public Triangle(short x1, short y1, short x2, short y2, short x3, short y3)
		{
			this.x1 = x1;
			this.y1 = y1;
			this.x2 = x2;
			this.y2 = y2;
			this.x3 = x3;
			this.y3 = y3;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="point1"></param>
		/// <param name="point2"></param>
		/// <param name="point3"></param>
		public Triangle(Point point1, Point point2, Point point3)
		{
			this.x1 = (short)point1.X;
			this.y1 = (short)point1.Y;
			this.x2 = (short)point2.X;
			this.y2 = (short)point2.Y;
			this.x3 = (short)point3.X;
			this.y3 = (short)point3.Y;
		}

		/// <summary>
		/// 
		/// </summary>
		public Point Point1
		{
			get
			{
				return new Point(this.x1, this.y1);
			}
			set
			{
				this.x1 = (short)value.X;
				this.y1 = (short)value.Y;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public Point Point2
		{
			get
			{
				return new Point(this.x2, this.y2);
			}
			set
			{
				this.x2 = (short)value.X;
				this.y2 = (short)value.Y;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public Point Point3
		{
			get
			{
				return new Point(this.x3, this.y3);
			}
			set
			{
				this.x3 = (short)value.X;
				this.y3 = (short)value.Y;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public short XPosition1
		{
			get
			{
				return this.x1;
			}
			set
			{
				this.x1 = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public short YPosition1
		{
			get
			{
				return this.y1;
			}
			set
			{
				this.y1 = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public short XPosition2
		{
			get
			{
				return this.x2;
			}
			set
			{
				this.x2 = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public short YPosition2
		{
			get
			{
				return this.y2;
			}
			set
			{
				this.y2 = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public short XPosition3
		{
			get
			{
				return this.x3;
			}
			set
			{
				this.x3 = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public short YPosition3
		{
			get
			{
				return this.y3;
			}
			set
			{
				this.y3 = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return String.Format(
				CultureInfo.CurrentCulture, 
				"({0}, {1}, {2}, {3}, {4}, {5})", x1, y1, x2, y2, x3, y3);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			if (obj.GetType() != typeof(Triangle))
				return false;
                
			Triangle triangle = (Triangle)obj;   
			return (
				(this.x1 == triangle.x1) && 
				(this.y1 == triangle.y1) && 
				(this.x2 == triangle.x2) && 
				(this.y2 == triangle.y2) &&
				(this.x2 == triangle.x3) && 
				(this.y2 == triangle.y3)
				);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="triangle1"></param>
		/// <param name="triangle2"></param>
		/// <returns></returns>
		public static bool operator== (Triangle triangle1, Triangle triangle2)
		{
			return (
				(triangle1.x1 == triangle2.x1) && 
				(triangle1.y1 == triangle2.y1) && 
				(triangle1.x2 == triangle2.x2) && 
				(triangle1.y2 == triangle2.y2) && 
				(triangle1.x3 == triangle2.x3) && 
				(triangle1.y3 == triangle2.y3)
				);
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="triangle1"></param>
		/// <param name="triangle2"></param>
		/// <returns></returns>
		public static bool operator!= (Triangle triangle1, Triangle triangle2)
		{
			return !(triangle1 == triangle2);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode()
		{
			return x1 ^ y1 ^ x2 ^ y2 ^ x3 ^ y3;

		}
		#region IPrimitive Members

		/// <summary>
		/// 
		/// </summary>
		public Point Center
		{
			get
			{
				return new Point ((this.x1 + this.x2 + this.x3)/3, (this.y1 + this.y2 + this.y3)/3);
			}
			set
			{
				this.x1 += (short)(value.X - (x1 + x2 + x3)/3);
				this.x2 += (short)(value.X - (x1 + x2 + x3)/3);
				this.x3 += (short)(value.X - (x1 + x2 + x3)/3);
				this.y1 += (short)(value.Y - (y1 + y2 + y3)/3);
				this.y2 += (short)(value.Y - (y1 + y2 + y3)/3);
				this.y3 += (short)(value.Y - (y1 + y2 + y3)/3);
			}
		}

		#endregion
	}

	/// <summary>
	/// 
	/// </summary>
	public struct Polygon : IPrimitive
	{
		short[] x;
		short[] y;
		int n;
		ArrayList list;
		short xTotal;
		short yTotal;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		public Polygon(short[] x, short[] y)
		{
			this.x = x;
			this.y = y;
			this.n = 0;
			this.list = new ArrayList();
			this.xTotal = 0;
			this.yTotal = 0;
			
			if (x.Length != y.Length)
			{
				throw SdlException.Generate();
			}
			else
			{
				this.n = x.Length;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="points"></param>
		public Polygon(ArrayList points)
		{
			this.x = new short[points.Count];
			this.y = new short[points.Count];
			this.n = 0;
			this.list = new ArrayList();
			this.xTotal = 0;
			this.yTotal = 0;
			this.Points = points;
		}

		/// <summary>
		/// 
		/// </summary>
		public short[] XPositions()
		{
			return this.x;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="x"></param>
		public void XPositions(short[] x)
		{
			if (x.Length != y.Length)
			{
				throw SdlException.Generate();
			}
			else
			{
				this.x = x;
				this.n = x.Length;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public short[] YPositions()
		{
			return this.y;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="y"></param>
		public void YPositions(short[] y)
		{
			if (this.x.Length != y.Length)
			{
				throw SdlException.Generate();
			}
			else
			{
				this.y = y;
				this.n = x.Length;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public int NumberOfSides
		{
			get
			{
				return this.n;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public ArrayList Points
		{
			get
			{
				list.Clear();
				for (int i = 0; i < this.n; i++)
				{
					list.Add(new Point(x[i], y[i]));	
				}
				return list;
			}
			set
			{
				for (int i = 0; i < value.Count; i++)
				{
					x[i] = (short)((Point)value[i]).X;
					y[i] = (short)((Point)value[i]).Y;
				}
			}
		}
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return String.Format(
				CultureInfo.CurrentCulture, 
				"({0}, {1}, {2})", 
				x, y, n);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			if (obj.GetType() != typeof(Polygon))
				return false;
                
			Polygon polygon = (Polygon)obj;   
			return (
				(this.x == polygon.x) && 
				(this.y == polygon.y) && 
				(this.n == polygon.n) 
				);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="polygon1"></param>
		/// <param name="polygon2"></param>
		/// <returns></returns>
		public static bool operator== (Polygon polygon1, Polygon polygon2)
		{
			return (
				(polygon1.x == polygon2.x) && 
				(polygon1.y == polygon2.y) && 
				(polygon1.n == polygon2.n)  
				);
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="polygon1"></param>
		/// <param name="polygon2"></param>
		/// <returns></returns>
		public static bool operator!= (Polygon polygon1, Polygon polygon2)
		{
			return !(polygon1 == polygon2);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode()
		{
			return x.GetHashCode() ^ y.GetHashCode() ^ n;

		}
		#region IPrimitive Members

		/// <summary>
		/// 
		/// </summary>
		public Point Center
		{
			get
			{
				xTotal = 0;
				yTotal = 0;
				
				for (int i = 0; i < list.Count; i++)
				{
					xTotal += (short)((Point)list[i]).X;
					yTotal += (short)((Point)list[i]).Y;
				}
				return new Point((xTotal/this.n), (yTotal/this.n));
			}
			set
			{
				Point centerTemp = new Point(this.Center.X, this.Center.Y);
				foreach (Point i in this.Points)
				{
					i.Offset(value.X - centerTemp.X, value.Y - centerTemp.Y);
				}
			}
		}

		#endregion
	}

	/// <summary>
	/// 
	/// </summary>
	public struct Pie : IPrimitive
	{
		short x;
		short y;
		short r;
		short startingAngle;
		short endingAngle;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="r"></param>
		/// <param name="startingAngle"></param>
		/// <param name="endingAngle"></param>
		public Pie(short x, short y, short r, short startingAngle, short endingAngle)
		{
			this.x = x;
			this.y = y;
			this.r = r;
			this.startingAngle = startingAngle;
			this.endingAngle = endingAngle;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="point"></param>
		/// <param name="r"></param>
		/// <param name="startingAngle"></param>
		/// <param name="endingAngle"></param>
		public Pie(Point point, short r, short startingAngle, short endingAngle)
		{
			this.x = (short)point.X;
			this.y = (short)point.Y;
			this.r = r;
			this.startingAngle = startingAngle;
			this.endingAngle = endingAngle;
		}

		/// <summary>
		/// 
		/// </summary>
		public short XPosition
		{
			get
			{
				return this.x;
			}
			set
			{
				this.x = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public short YPosition
		{
			get
			{
				return this.y;
			}
			set
			{
				this.y = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public Point Point
		{
			get
			{
				return new Point(this.x, this.y);
			}
			set
			{
				this.x = (short)value.X;
				this.y = (short)value.Y;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public short Radius
		{
			get
			{
				return this.r;
			}
			set
			{
				this.r = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public short StartingAngle
		{
			get
			{
				return this.startingAngle;
			}
			set
			{
				this.startingAngle = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public short EndingAngle
		{
			get
			{
				return this.endingAngle;
			}
			set
			{
				this.endingAngle = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return String.Format(
				CultureInfo.CurrentCulture, 
				"({0}, {1}, {2}, {3}, {4}, {5})", 
				x, y, r, startingAngle, endingAngle);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			if (obj.GetType() != typeof(Pie))
				return false;
                
			Pie pie = (Pie)obj;   
			return (
				(this.x == pie.x) && 
				(this.y == pie.y) && 
				(this.r == pie.r) && 
				(this.startingAngle == pie.startingAngle) &&
				(this.endingAngle == pie.endingAngle) 
				);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="pie1"></param>
		/// <param name="pie2"></param>
		/// <returns></returns>
		public static bool operator== (Pie pie1, Pie pie2)
		{
			return (
				(pie1.x == pie2.x) && 
				(pie1.y == pie2.y) && 
				(pie1.r == pie2.r) && 
				(pie1.startingAngle == pie2.startingAngle) && 
				(pie1.endingAngle == pie2.endingAngle) 
				);
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="pie1"></param>
		/// <param name="pie2"></param>
		/// <returns></returns>
		public static bool operator!= (Pie pie1, Pie pie2)
		{
			return !(pie1 == pie2);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode()
		{
			return x ^ y ^ r ^ startingAngle ^ endingAngle;

		}
		#region IPrimitive Members

		/// <summary>
		/// 
		/// </summary>
		public Point Center
		{
			get
			{
				// TODO:  Add Pie.Center getter implementation
				return new Point((int)(this.r * Math.Sin(((this.startingAngle - this.endingAngle)*2*Math.PI/360)/(this.startingAngle - this.endingAngle)*2*Math.PI/360)),(int)(this.r * Math.Cos(1-(this.startingAngle - this.endingAngle)*2*Math.PI/360)/(this.startingAngle - this.endingAngle)*2*Math.PI/360));
			}
			set
			{
				Point centerTemp = new Point(this.Center.X, this.Center.Y);
				this.Point.Offset(value.X - centerTemp.X, value.Y - centerTemp.Y);
			}
		}

		#endregion
	}

	/// <summary>
	/// 
	/// </summary>
	public struct Bezier : IPrimitive
	{
		const int MINIMUMSTEPS = 2;
		short[] x;
		short[] y;
		int n;
		int steps;
		ArrayList list;
		short xTotal;
		short yTotal;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="steps"></param>
		public Bezier(short[] x, short[] y, int steps)
		{
			this.x = x;
			this.y = y;
			this.n = 0;
			this.xTotal = 0;
			this.yTotal = 0;
			this.steps = steps;
			this.list = new ArrayList();

			if (steps < MINIMUMSTEPS)
			{
				this.steps = MINIMUMSTEPS;
			}
			else
			{
				this.steps = steps;
			}
			
			if (x.Length != y.Length)
			{
				throw SdlException.Generate();
			}
			else
			{
				this.n = x.Length;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="points"></param>
		public Bezier(ArrayList points)
		{
			this.x = new short[points.Count];
			this.y = new short[points.Count];
			this.n = 0;
			this.list = new ArrayList();
			this.xTotal = 0;
			this.yTotal = 0;
			this.steps = points.Count;
			this.Points = points;
		}

		/// <summary>
		/// 
		/// </summary>
		public short[] XPositions()
		{
			return this.x;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="x"></param>
		public void XPositions(short[] x)
		{
			if (x.Length != y.Length)
			{
				throw SdlException.Generate();
			}
			else
			{
				this.x = x;
				this.n = x.Length;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public short[] YPositions()
		{
			return this.y;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="y"></param>
		public void YPositions(short[] y)
		{
			if (this.x.Length != y.Length)
			{
				throw SdlException.Generate();
			}
			else
			{
				this.y = y;
				this.n = x.Length;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public int NumberOfPoints
		{
			get
			{
				return this.n;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public int Steps
		{
			get
			{
				return this.steps;
			}
			set
			{
				if (value < 2)
				{
					steps = 2;
				}
				else
				{
					steps = value;
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return String.Format(
				CultureInfo.CurrentCulture, 
				"({0}, {1}, {2}, {3})", 
				x, y, n, steps);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			if (obj.GetType() != typeof(Bezier))
				return false;
                
			Bezier bezier = (Bezier)obj;   
			return (
				(this.x == bezier.x) && 
				(this.y == bezier.y) && 
				(this.n == bezier.n) && 
				(this.steps == bezier.steps)
				);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="bezier1"></param>
		/// <param name="bezier2"></param>
		/// <returns></returns>
		public static bool operator== (Bezier bezier1, Bezier bezier2)
		{
			return (
				(bezier1.x == bezier2.x) && 
				(bezier1.y == bezier2.y) && 
				(bezier1.n == bezier2.n) && 
				(bezier1.steps == bezier2.steps) 
				);
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="bezier1"></param>
		/// <param name="bezier2"></param>
		/// <returns></returns>
		public static bool operator!= (Bezier bezier1, Bezier bezier2)
		{
			return !(bezier1 == bezier2);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode()
		{
			return x.GetHashCode() ^ y.GetHashCode() ^ n ^ steps;

		}
		/// <summary>
		/// 
		/// </summary>
		public ArrayList Points
		{
			get
			{
				list.Clear();
				for (int i = 0; i < this.n; i++)
				{
					list.Add(new Point(x[i], y[i]));	
				}
				return list;
			}
			set
			{
				for (int i = 0; i < value.Count; i++)
				{
					x[i] = (short)((Point)value[i]).X;
					y[i] = (short)((Point)value[i]).Y;
				}
			}
		}
		#region IPrimitive Members

		/// <summary>
		/// 
		/// </summary>
		public Point Center
		{
			get
			{
				xTotal = 0;
				yTotal = 0;
				
				for (int i = 0; i < list.Count; i++)
				{
					xTotal += (short)((Point)list[i]).X;
					yTotal += (short)((Point)list[i]).Y;
				}
				return new Point((xTotal/this.n), (yTotal/this.n));
			}
			set
			{
				Point centerTemp = new Point(this.Center.X, this.Center.Y);
				foreach (Point i in this.Points)
				{
					i.Offset(value.X - centerTemp.X, value.Y - centerTemp.Y);
				}
			}
		}

		#endregion
	}
	/// <summary>
	/// 
	/// </summary>
	public struct Box : IPrimitive
	{
		short x1;
		short y1;
		short x2;
		short y2;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="x1"></param>
		/// <param name="y1"></param>
		/// <param name="x2"></param>
		/// <param name="y2"></param>
		public Box(short x1, short y1, short x2, short y2)
		{
			this.x1 = x1;
			this.y1 = y1;
			this.x2 = x2;
			this.y2 = y2;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="point"></param>
		/// <param name="size"></param>
		public Box(Point point, Size size)
		{
			this.x1 = (short)point.X;
			this.y1 = (short)point.Y;
			this.x2 = (short)(point.X + size.Width);
			this.y2 = (short)(point.Y + size.Height);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="point1"></param>
		/// <param name="point2"></param>
		public Box(Point point1, Point point2)
		{
			this.x1 = (short)point1.X;
			this.y1 = (short)point1.Y;
			this.x2 = (short)point2.X;
			this.y2 = (short)point2.Y;
		}

		/// <summary>
		/// 
		/// </summary>
		public short XPosition1
		{
			get
			{
				return this.x1;
			}
			set
			{
				this.x1 = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public short YPosition1
		{
			get
			{
				return this.y1;
			}
			set
			{
				this.y1 = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public short XPosition2
		{
			get
			{
				return this.x2;
			}
			set
			{
				this.x2 = value;
			}
		}

		
		/// <summary>
		/// 
		/// </summary>
		public short YPosition2
		{
			get
			{
				return this.y2;
			}
			set
			{
				this.y2 = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public short Height
		{
			get
			{

				return (short)(this.y2 - this.y1);
			}
			set
			{
				this.y2 = (short)(this.y1 + value);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public short Width
		{
			get
			{

				return (short)(this.x2 - this.x1);
			}
			set
			{
				this.x2 = (short)(this.x1 + value);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public Point Location
		{
			get
			{
				return new Point(x1, y1);
			}
			set
			{
				this.y2 = (short)(value.Y + this.Height);
				this.x2 = (short)(value.X + this.Width);
				this.x1 = (short)value.X;
				this.y1 = (short)value.Y;

			}
		}

		/// <summary>
		/// 
		/// </summary>
		public Size Size
		{
			get
			{
				return new Size(this.Width, this.Height);
			}
			set
			{
				this.Width = (short)value.Width;
				this.Height = (short)value.Height;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return String.Format(CultureInfo.CurrentCulture, "({0},{1}, {2}, {3})", x1, y1, x2, y2);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			if (obj.GetType() != typeof(Box))
				return false;
                
			Box box = (Box)obj;   
			return (
				(this.x1 == box.x1) && 
				(this.y1 == box.y1) && 
				(this.x2 == box.x2) && 
				(this.y2 == box.y2)
				);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="box1"></param>
		/// <param name="box2"></param>
		/// <returns></returns>
		public static bool operator== (Box box1, Box box2)
		{
			return (
				(box1.x1 == box2.x1) && 
				(box1.y1 == box2.y1) && 
				(box1.x2 == box2.x2) && 
				(box1.y2 == box2.y2)
				);
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="box1"></param>
		/// <param name="box2"></param>
		/// <returns></returns>
		public static bool operator!= (Box box1, Box box2)
		{
			return !(box1 == box2);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode()
		{
			return x1 ^ y1 ^ x2 ^ y2;

		}
		#region IPrimitive Members

		/// <summary>
		/// 
		/// </summary>
		public Point Center
		{
			get
			{
				return new Point((x1 + x2)/2, (y1 + y2)/2);
			}
			set
			{
				this.x1 += (short)(value.X - (x1 + x2)/2);
				this.x2 += (short)(value.X - (x1 + x2)/2);
				this.y1 += (short)(value.Y - (y1 + y2)/2);
				this.y2 += (short)(value.Y - (y1 + y2)/2);
			}
		}

		#endregion
	}
}
