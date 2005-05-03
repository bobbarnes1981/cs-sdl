/*
 * $RCSfile$
 * Copyright (C) 2005 David Hudson (jendave@yahoo.com)
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

using SdlDotNet;
using System;
using System.Drawing;
using System.Collections;

namespace SdlDotNet.Sprites
{
	/// <summary>
	/// 
	/// </summary>
	public class Sprite : IComparable, IDisposable
	{
		/// <summary>
		/// 
		/// </summary>
		public Sprite()
		{
		}
//		/// <summary>
//		/// 
//		/// </summary>
//		/// <param name="coordinates"></param>
//		public Sprite(Vector coordinates)
//		{
//			this.rect = 
//				new Rectangle(coordinates.X, coordinates.Y, 0, 0);
//			this.coordinateZ = coordinates.Z;
//		}

//		/// <summary>
//		/// 
//		/// </summary>
//		/// <param name="rectangle"></param>
//		public Sprite(Rectangle rectangle)
//		{
//			this.rect = rectangle;
//		}
//
//		/// <summary>
//		/// 
//		/// </summary>
//		/// <param name="group"></param>
//		public Sprite(SpriteCollection group) : this()
//		{
//			this.InnerAdd(group);
//		}
//
//		/// <summary>
//		/// 
//		/// </summary>
//		/// <param name="coordinates"></param>
//		public Sprite(Vector coordinates)
//		{
//			this.rect = new Rectangle(coordinates.Point, new Size(0, 0));
//			this.coordinateZ = coordinates.Z;
//		}
//
//		/// <summary>
//		/// 
//		/// </summary>
//		/// <param name="coordinates"></param>
//		/// <param name="group"></param>
//		public Sprite(Vector coordinates, SpriteCollection group) : this(coordinates)
//		{
//			this.InnerAdd(group);
//		}
//
//		/// <summary>
//		/// 
//		/// </summary>
//		/// <param name="point"></param>
//		/// <param name="z"></param>
//		public Sprite(Point point, int z) : this(new Vector(point, z))
//		{
//		}
//
//		/// <summary>
//		/// 
//		/// </summary>
//		/// <param name="point"></param>
//		/// <param name="z"></param>
//		/// <param name="group"></param>
//		public Sprite(Point point, int z, SpriteCollection group) : this(new Vector(point, z))
//		{
//			this.InnerAdd(group);
//		}
//
//		/// <summary>
//		/// 
//		/// </summary>
//		/// <param name="point"></param>
//		public Sprite(Point point)
//		{
//			this.rect = new Rectangle(point, new Size(0, 0));
//		}
//
//		/// <summary>
//		/// 
//		/// </summary>
//		/// <param name="point"></param>
//		/// <param name="group"></param>
//		public Sprite(Point point, SpriteCollection group) : this(point)
//		{
//			this.InnerAdd(group);
//		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="position"></param>
		/// <param name="surface"></param>
		public Sprite(Surface surface, Point position)
		{
			this.rect = new Rectangle(position.X, position.Y, surface.Width, surface.Height);
			this.surf = surface;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="coordinates"></param>
		/// <param name="surface"></param>
		public Sprite(Surface surface, Vector coordinates) : this(surface, coordinates.Point)
		{
			this.coordinateZ = coordinates.Z;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="surface"></param>
		public Sprite(Surface surface)
		{
			this.rect = new Rectangle(0, 0, surface.Width, surface.Height);
			this.surf = surface;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="position"></param>
		/// <param name="z"></param>
		/// <param name="surface"></param>
		public Sprite(Surface surface, Point position, int z ) : this(surface, position)
		{
			this.coordinateZ = z;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="surface"></param>
		/// <param name="rectangle"></param>
		public Sprite(Surface surface, Rectangle rectangle)
		{
			this.surf = surface;
			this.rect = rectangle;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="surface"></param>
		/// <param name="rectangle"></param>
		/// <param name="z"></param>
		public Sprite(Surface surface, Rectangle rectangle, int z): this(surface, rectangle)
		{
			this.coordinateZ = z;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="coordinates"></param>
		/// <param name="surface"></param>
		/// <param name="group"></param>
		public Sprite(Surface surface, Vector coordinates, SpriteCollection group) : this(surface, coordinates)
		{
			this.InnerAdd(group);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="position"></param>
		/// <param name="z"></param>
		/// <param name="surface"></param>
		/// <param name="group"></param>
		public Sprite(Surface surface, Point position, int z, SpriteCollection group): this(surface, position, z)
		{
			this.InnerAdd(group);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="position"></param>
		/// <param name="surface"></param>
		/// <param name="group"></param>
		public Sprite(Surface surface, Point position , SpriteCollection group):this(surface, position)
		{
			this.InnerAdd(group);
		}

		#region Display
		/// <summary>
		/// 
		/// </summary>
		private Surface surf;
		/// <summary>
		/// 
		/// </summary>
		public Surface Surface
		{
			get
			{
				if (this.surf == null)
				{
					this.surf = new Surface(this.Width, this.Height);				
				}
				return surf;
			}
			set
			{
				surf = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public virtual Surface Render()
		{
			return this.surf;
		}
		#endregion

		#region Events
		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		public virtual void Update(ActiveEventArgs e)
		{
		}
		/// <summary>
		/// Processes the keyboard.
		/// </summary>
		public virtual void Update(KeyboardEventArgs e)
		{
		}

		/// <summary>
		/// Processes a mouse button. This event is trigger by the SDL
		/// system. 
		/// </summary>
		public virtual void Update(MouseButtonEventArgs args)
		{
		}

		/// <summary>
		/// Processes a mouse motion event. This event is triggered by
		/// SDL. Only
		/// sprites that are MouseSensitive are processed.
		/// </summary>
		public virtual void Update(MouseMotionEventArgs args)
		{
		}
		
		/// <summary>
		/// Processes a joystick motion event. This event is triggered by
		/// SDL. Only
		/// sprites that are JoystickSensitive are processed.
		/// </summary>
		public virtual void Update(JoystickAxisEventArgs args)
		{
		}
		
		/// <summary>
		/// Processes a joystick button event. This event is triggered by
		/// SDL. Only
		/// sprites that are JoystickSensitive are processed.
		/// </summary>
		public virtual void Update(JoystickButtonEventArgs args)
		{
		}
		
		/// <summary>
		/// Processes a joystick hat motion event. This event is triggered by
		/// SDL. Only
		/// sprites that are JoystickSensitive are processed.
		/// </summary>
		public virtual void Update(JoystickHatEventArgs args)
		{
		}
		
		/// <summary>
		/// Processes a joystick hat motion event. This event is triggered by
		/// SDL. Only
		/// sprites that are JoystickSensitive are processed.
		/// </summary>
		public virtual void Update(JoystickBallEventArgs args)
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		public virtual void Update(QuitEventArgs e)
		{
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		public virtual void Update(UserEventArgs e)
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		public virtual void Update(VideoExposeEventArgs e)
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		public virtual void Update(VideoResizeEventArgs e)
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		public virtual void Update(ChannelFinishedEventArgs e)
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		public virtual void Update(MusicFinishedEventArgs e)
		{
		}
		
		/// <summary>
		/// All sprites are tickable, regardless if they actual do
		/// anything. This ensures that the functionality is there, to be
		/// overridden as needed.
		/// </summary>
		public virtual void Update(TickEventArgs args)
		{
		}
		#endregion

		#region Geometry

		private Rectangle rect;
		/// <summary>
		/// 
		/// </summary>
		public Rectangle Rectangle
		{
			get 
			{ 
				if (rect.IsEmpty)
				{
					this.rect = this.Surface.Rectangle;
				}
				return this.rect;
			}
			set
			{
				this.rect = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public Point Position
		{
			get 
			{ 
				return new Point(rect.X, rect.Y); 
			}
			set
			{
				rect.X = value.X;
				rect.Y = value.Y;
			}
		}

		private int coordinateZ;

		/// <summary>
		/// 
		/// </summary>
		public Vector Coordinates
		{
			get
			{
				return new Vector(this.Position, this.coordinateZ);
			}
			set
			{
				this.Position = new Point(value.X, value.Y);
				this.coordinateZ = value.Z;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public int X
		{
			get
			{
				return this.rect.X;
			}
			set
			{
				this.rect.X = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public int Y
		{
			get
			{
				return this.rect.Y;
			}
			set
			{
				this.rect.Y = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public int Z
		{
			get
			{
				return this.coordinateZ;
			}
			set
			{
				this.coordinateZ = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public Size Size
		{
			get 
			{ 
				return new Size(rect.Width, rect.Height);
			}
			set
			{
				rect.Width = value.Width;
				rect.Height = value.Height;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public int Height
		{
			get
			{
				return this.rect.Height;
			}
			set
			{
				this.rect.Height = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public int Width
		{
			get
			{
				return this.rect.Width;
			}
			set
			{
				this.rect.Width = value;
			}
		}

//		private bool dirty = false;
//
//		/// <summary>
//		/// 
//		/// </summary>
//		public virtual bool Dirty
//		{
//			get
//			{
//				return dirty;
//			}
//			set
//			{
//				dirty = value;
//			}
//		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="point"></param>
		/// <returns></returns>
		public virtual bool IntersectsWith(Point point)
		{
			return this.rect.IntersectsWith(new Rectangle(point, new Size(0, 0)));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="rectangle"></param>
		/// <returns></returns>
		public virtual bool IntersectsWith(Rectangle rectangle)
		{
			return this.rect.IntersectsWith(rectangle);
		}
		#endregion

		#region Operators
		/// <summary>
		/// 
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public int CompareTo(object obj)
		{
			// Cast the resulting object into a sprite, since that is the
			// only thing comparable to this sprite.
			Sprite s = (Sprite) obj;

			// Compare the Z-Order first
			int res = this.Coordinates.Z.CompareTo(s.Coordinates.Z);

			if (res != 0)
			{
				return res;
			}

			// Compare the hashes
			return GetHashCode().CompareTo(s.GetHashCode());
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals (Object obj)
		{
			if (!(obj is Sprite) || obj == null)
			{
				return false;
			}
			return (this.CompareTo(obj) == 0);
		}  

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode()
		{
			return this.Coordinates.X ^ this.Coordinates.Y ^ 
				this.Coordinates.Z; 
				//^ this.surf.Size.Height ^ 
				//this.surf.Size.Width ^ this.surf.GetHashCode();
		}  

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sprite1"></param>
		/// <param name="sprite2"></param>
		/// <returns></returns>
		public static bool operator == (Sprite sprite1, Sprite sprite2)
		{
			try
			{
				return sprite1.Equals(sprite2);
			}
			catch (NullReferenceException)
			{
				try
				{
					return sprite2.Equals(sprite1);
				}
				catch (NullReferenceException)
				{
					return false;
				}
			}
		}  
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sprite1"></param>
		/// <param name="sprite2"></param>
		/// <returns></returns>
		public static bool operator != (Sprite sprite1, Sprite sprite2)
		{
			try
			{
				return !sprite1.Equals(sprite2);
			}
			catch (NullReferenceException)
			{
				try
				{
					return !sprite2.Equals(sprite1);
				}
				catch (NullReferenceException)
				{
					return false;
				}
			}
		}  
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sprite1"></param>
		/// <param name="sprite2"></param>
		/// <returns></returns>
		public static bool operator < (Sprite sprite1, Sprite sprite2)
		{
			return (sprite1.CompareTo(sprite2) < 0);
		}  
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sprite1"></param>
		/// <param name="sprite2"></param>
		/// <returns></returns>
		public static bool operator > (Sprite sprite1, Sprite sprite2)
		{
			return (sprite1.CompareTo(sprite2) > 0);
		} 

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return this.Rectangle.ToString();
		}
		#endregion

		#region Properties
		/// <summary>
		/// 
		/// </summary>
		public virtual bool Alive
		{
			get
			{
				return (groups.Count > 0);
			}
		}

		ArrayList groups = new ArrayList();
		/// <summary>
		/// 
		/// </summary>
		public virtual ArrayList Collections
		{
			get
			{
				return groups;
			}
		}

//		/// <summary>
//		/// 
//		/// </summary>
//		public virtual void Kill()
//		{
//			for (int i = 0; i < groups.Count; i++)
//			{
//				groups[i].Remove(this);
//			}
//		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="group"></param>
		public virtual void Add(SpriteCollection group)
		{
			group.Add(this);
			this.groups.Add(group);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="group"></param>
		protected void InnerAdd(SpriteCollection group)
		{
			group.Add(this);
			this.groups.Add(group);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="group"></param>
		public virtual void Remove(SpriteCollection group)
		{
			group.Remove(this);
			this.groups.Remove(group);
		}

		private bool allowDrag;
		/// <summary>
		/// 
		/// </summary>
		public bool AllowDrag
		{
			get
			{
				return allowDrag;
			}
			set
			{
				allowDrag = value;
			}
		}

		private bool beingDragged;

		/// <summary>
		/// 
		/// </summary>
		public bool BeingDragged
		{
			get
			{
				return beingDragged;
			}
			set
			{
				beingDragged = value;
			}
		}

		private bool visible = true;

		/// <summary>
		/// 
		/// </summary>
		public bool Visible
		{
			get
			{
				return visible;
			}
			set
			{
				visible = value;
			}
		}
		#endregion

		#region IDisposable Members

		/// <summary>
		/// 
		/// </summary>
		public void Dispose()
		{
			this.surf.Dispose();
		}

		#endregion
	}
}
