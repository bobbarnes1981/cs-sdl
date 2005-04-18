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

using SdlDotNet.Sprites;
using SdlDotNet;
using System.Collections;
using System.Drawing;

namespace SdlDotNet.Examples.GuiExample
{
	/// <summary>
	/// This class handles one or more sprites packed into a line. The
	/// sprites may be added to the beginning or the end of the line, as
	/// the program desirs.
	/// </summary>
	public class Packer : GuiComponent
	{
		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		/// <param name="manager"></param>
		public Packer(GuiManager manager)
			: base(manager)
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="manager"></param>
		/// <param name="p"></param>
		public Packer(GuiManager manager, Point p)
			: base(manager, p)
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="manager"></param>
		/// <param name="p"></param>
		public Packer(GuiManager manager, Vector p)
			: base(manager, p)
		{
		}
		#endregion

		#region Sprites
		/// <summary>
		/// 
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		protected Size GetSize(Sprite s)
		{
			// Get the size
			Size d = s.Size;

//			if (s is GuiComponent)
//			{
//				//d = ((GuiComponent) s).OuterSize;
//				d = ((GuiComponent) s).Size;
//			}

			return d;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="point"></param>
		/// <param name="index"></param>
		/// <returns></returns>
		protected Sprite SelectSprite(Point point, ref int index)
		{
			index = 0;

			foreach (Sprite s in new ArrayList(Sprites))
			{
				if (s.IntersectsWith(point))
				{
					return s;
				}

				index++;
			}

			return null;
		}
		#endregion

		#region Events
		/// <summary>
		/// 
		/// </summary>
		/// <param name="args"></param>
		public override void Update(MouseButtonEventArgs args)
		{
			// We assume that the coordinates are set by the packing
			// processing, so we can use them with the offset and
			// coordinates.
			Point p = new Point(args.X - (Coordinates.X + MarginPadding.Left + InnerPadding.Left), args.Y - ((Coordinates.Y + MarginPadding.Top + InnerPadding.Top)));

			if (args.ButtonPressed)
			{
				foreach (Sprite s in Sprites)
				{
					// Check the region. If it contains the point, we call the
					// basic sprite (and not its parent).
					if (s.IntersectsWith(p))
					{
						//s.OnMouseButtonDown(this, new MouseButtonEventArgs(args.Button, args.ButtonPressed, (short)(args.X - (Coordinates.X + MarginPadding.Left + InnerPadding.Left)), (short)(args.Y - (Coordinates.Y + MarginPadding.Top + InnerPadding.Top))));
					}
				}
			}
			else 
			{
				
				foreach (Sprite s in Sprites)
				{
					// Check the region. If it contains the point, we call the
					// basic sprite (and not its parent).
					if (s.IntersectsWith(p))
					{
						//s.OnMouseButtonUp(this, new MouseButtonEventArgs(args.Button, args.ButtonPressed, (short)(args.X - (Coordinates.X + MarginPadding.Left + InnerPadding.Left)), (short)(args.Y - (Coordinates.Y + MarginPadding.Top + InnerPadding.Top))));
					}
				}
			}
		}

//		public override void Update(object sender, MouseButtonEventArgs args)
//		{
//			// We assume that the coordinates are set by the packing
//			// processing, so we can use them with the offset and
//			// coordinates.
//			Point p = new Point(args.X - (Coordinates.X + MarginPadding.Left + InnerPadding.Left), args.Y - (Coordinates.Y + MarginPadding.Top + InnerPadding.Top));
//
//			foreach (Sprite s in Sprites)
//			{
//				// Check the region. If it contains the point, we call the
//				// basic sprite (and not its parent).
//				if (s.IntersectsWith(p))
//				{
//					s.OnMouseButtonUp(this, new MouseButtonEventArgs(args.Button, args.ButtonPressed, (short)(args.X - (Coordinates.X + MarginPadding.Left + InnerPadding.Left)), (short)(args.Y - (Coordinates.Y + MarginPadding.Top + InnerPadding.Top))));
//				}
//			}
//		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="args"></param>
		public override void Update(MouseMotionEventArgs args)
		{
			// Build up the new point   
			MouseMotionEventArgs args1 = new MouseMotionEventArgs(args.ButtonPressed, (short)(args.X + Coordinates.X + MarginPadding.Left + InnerPadding.Left), (short)(args.Y + Coordinates.Y + MarginPadding.Top + InnerPadding.Top), args.RelativeX, args.RelativeY);
			// We assume that the coordinates are set by the packing
			// processing, so we can use them with the offset and
			// coordinates.
			Point p = new Point(args1.X, args1.Y);

			foreach (Sprite s in Sprites)
			{
				// Check the region. If it contains the point, we call the
				// basic sprite (and not its parent).
				if (s.IntersectsWith(p))
				{
					//s.OnMouseMotion(this, args1);
				}
			}
		}
		#endregion

		#region Properties
		private ArrayList head = new ArrayList();
		private ArrayList tail = new ArrayList();

		/// <summary>
		/// 
		/// </summary>
		/// <param name="s"></param>
		protected void AddHead(Sprite s)
		{
			head.Add(s);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="s"></param>
		protected void AddTail(Sprite s)
		{
			tail.Add(s);
		}

		/// <summary>
		/// 
		/// </summary>
		public virtual ArrayList HeadSprites
		{
			get 
			{ 
				return head; 
			}
		}
    
		/// <summary>
		/// 
		/// </summary>
		public virtual ArrayList TailSprites
		{
			get 
			{ 
				return tail; 
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public new virtual ArrayList Sprites
		{
			get
			{
				ArrayList list = new ArrayList(head);
				list.AddRange(tail);
				return list;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public virtual Padding MarginPadding
		{
			get 
			{ 
				return new Padding(0); 
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public virtual Padding InnerPadding
		{
			get 
			{ 
				return new Padding(0); 
			}
		}
		#endregion
	}
}
