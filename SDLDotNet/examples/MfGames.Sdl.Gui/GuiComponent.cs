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
using System;
using System.Drawing;

namespace MFGames.Sdl.Gui
{
	/// <summary>
	/// Base class to manage all graphical GUI elements.
	/// </summary>
	public class GuiComponent : Sprite
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="manager"></param>
		public GuiComponent(GuiManager manager)
			: base()
		{
			this.manager = manager;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="manager"></param>
		/// <param name="z"></param>
		public GuiComponent(GuiManager manager, int z)
			: base()
		{
			this.manager = manager;
			this.Coordinates = new Vector(z);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="manager"></param>
		/// <param name="loc"></param>
		public GuiComponent(GuiManager manager, Point loc)
			: base()
		{
			this.manager = manager;
			this.Coordinates = new Vector(loc);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="manager"></param>
		/// <param name="rectangle"></param>
		public GuiComponent(GuiManager manager, Rectangle rectangle)
			: base()
		{
			this.manager = manager;
			this.Rectangle = rectangle;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="manager"></param>
		/// <param name="loc"></param>
		public GuiComponent(GuiManager manager, Vector loc)
			: base()
		{
			this.manager = manager;
			this.Coordinates = loc;
		}

//		/// <summary>
//		/// 
//		/// </summary>
//		/// <returns></returns>
//		public override string ToString()
//		{
//			return String.Format("(gui {0})", Bounds, base.ToString());
//		}

		#region Drawing
//		public virtual void Render(RenderArgs args)
//		{
////			if (!IsTraced)
////			{
////				return;
////			}
//
//			// Draw the outer and the inner bounds
//			GuiManager.DrawRect(args.Surface,
//				args.Translate(Bounds),
//				manager.BoundsTraceColor);
//			GuiManager.DrawRect(args.Surface,
//				args.Translate(OuterBounds),
//				manager.OuterBoundsTraceColor);
//		}
		#endregion

		#region Events
		private bool isDraggable = false;
		private bool beingDragged = false;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		public override void Update(object sender, MouseButtonEventArgs args)
		{
			// If we cannot be dragged, don't worry about it
			if (!isDraggable)
			{
				return;
			}

			// If we are being held down, pick up the marble
			// Change the Z-order
			if (args.ButtonPressed)
			{
				this.Z += manager.DragZOrder;
				beingDragged = true;
//				manager.SpriteContainer.EventLock = this;
			}
			else
			{
				this.Z -= manager.DragZOrder;
				beingDragged = false;
//				manager.SpriteContainer.EventLock = null;
			}
		}

		/// <summary>
		/// If the sprite is picked up, this moved the sprite to follow
		/// the mouse.
		/// </summary>
		public override void Update(object sender, MouseMotionEventArgs args)
		{
			// If we cannot be dragged, don't worry about it
			if (!isDraggable)
			{
				return;
			}

			// Move the window as appropriate
			if (beingDragged)
			{
				this.X += args.RelativeX;
				this.Y += args.RelativeY;;
			}
		}
		#endregion

		#region Geometry
//		private Size size = new Size();

		/// <summary>
		/// 
		/// </summary>
		public Rectangle OuterBounds
		{
			get
			{
				return new Rectangle(new Point(OuterCoordinates.X, OuterCoordinates.Y), OuterSize);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public virtual Size OuterSize
		{
			get
			{
				return new Size(Size.Width + OuterPadding.Horizontal,
					Size.Height + OuterPadding.Vertical);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public virtual Vector OuterCoordinates
		{
			get
			{
				return new Vector(Coordinates.X - OuterPadding.Left,
					Coordinates.Y - OuterPadding.Top,
					Coordinates.Z);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public virtual Padding OuterPadding
		{
			get { return new Padding(0); }
		}
//
//		public override Size Size
//		{
//			get { return size; }
//			set { size = value; }
//		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="point"></param>
		/// <returns></returns>
		public override bool IntersectsWith(Point point)
		{
			return OuterBounds.IntersectsWith(new Rectangle(point, new Size(0, 0)));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="rectangle"></param>
		/// <returns></returns>
		public override bool IntersectsWith(Rectangle rectangle)
		{
			return OuterBounds.IntersectsWith(rectangle);
		}
		#endregion

		#region Properties
		/// <summary>
		/// 
		/// </summary>
		protected GuiManager manager = null;

		/// <summary>
		/// Contains the manager for this component.
		/// </summary>
		public GuiManager GuiManager
		{
			get 
			{ 
				return manager;
			}
			set
			{
				if (value == null)
				{
					throw new Exception("Cannot assign a null manager");
				}
				manager = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public bool IsMouseMotionLocked
		{
			get
			{
				return beingDragged;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public  bool IsMouseButtonLocked
		{
			get
			{
				return true;
			}
		}
		#endregion
	}
}
