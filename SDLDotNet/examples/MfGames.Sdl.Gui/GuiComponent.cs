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

namespace MfGames.Sdl.Gui
{
	/// <summary>
	/// Base class to manage all graphical GUI elements.
	/// </summary>
	public class GuiComponent : Sprite
	{
		public GuiComponent(GuiManager manager)
			: base()
		{
			this.manager = manager;
			IsTickable = true;
		}

		public GuiComponent(GuiManager manager, int z)
			: base(new Vector(z))
		{
			this.manager = manager;
			IsTickable = true;
		}

		public GuiComponent(GuiManager manager, Point loc)
			: base(loc)
		{
			this.manager = manager;
			IsTickable = true;
		}

		public GuiComponent(GuiManager manager, Rectangle rect)
			: base(rect.Location)
		{
			this.manager = manager;
			this.size = rect.Size;
			IsTickable = true;
		}

		public GuiComponent(GuiManager manager, Vector loc)
			: base(loc)
		{
			this.manager = manager;
			IsTickable = true;
		}

		public override string ToString()
		{
			return String.Format("(gui {0})", Bounds, base.ToString());
		}

		#region Drawing
		public override void Render(RenderArgs args)
		{
			if (!IsTraced)
			{
				return;
			}

			// Draw the outer and the inner bounds
			GuiManager.DrawRect(args.Surface,
				args.Translate(Bounds),
				manager.BoundsTraceColor);
			GuiManager.DrawRect(args.Surface,
				args.Translate(OuterBounds),
				manager.OuterBoundsTraceColor);
		}
		#endregion

		#region Events
		private bool isDragable = false;
		private bool beingDragged = false;

		public virtual bool IsDragable
		{
			get { return isDragable; }
			set { isDragable = value; }
		}

		/// <summary>
		/// GUI components default to mouse sensitive.
		/// </summary>
		public override bool IsMouseSensitive { get { return true; } }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		public override void OnMouseButtonDown(object sender, MouseButtonEventArgs args)
		{
			// If we cannot be dragged, don't worry about it
			if (!isDragable)
			{
				return;
			}

			// If we are being held down, pick up the marble
			// Change the Z-order
			Coordinates.Z += manager.DragZOrder;
			beingDragged = true;
			manager.SpriteContainer.EventLock = this;
			
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		public override void OnMouseButtonUp(object sender, MouseButtonEventArgs args)
		{
			// If we cannot be dragged, don't worry about it
			if (!isDragable)
			{
				return;
			}
			// Drop it
			Coordinates.Z -= manager.DragZOrder;
			beingDragged = false;
			manager.SpriteContainer.EventLock = null;
		}

		/// <summary>
		/// If the sprite is picked up, this moved the sprite to follow
		/// the mouse.
		/// </summary>
		public override void OnMouseMotion(object sender, MouseMotionEventArgs args)
		{
			// If we cannot be dragged, don't worry about it
			if (!isDragable)
			{
				return;
			}

			// Move the window as appropriate
			if (beingDragged)
			{
				Coordinates.X += args.RelativeX;
				Coordinates.Y += args.RelativeY;;
			}
		}
		#endregion

		#region Geometry
		private Size size = new Size();

		public Rectangle OuterBounds
		{
			get
			{
				return new Rectangle(new Point(OuterCoordinates.X, OuterCoordinates.Y), OuterSize);
			}
		}

		public virtual Size OuterSize
		{
			get
			{
				return new Size(Size.Width + OuterPadding.Horizontal,
					Size.Height + OuterPadding.Vertical);
			}
		}

		public virtual Vector OuterCoordinates
		{
			get
			{
				return new Vector(Coordinates.X - OuterPadding.Left,
					Coordinates.Y - OuterPadding.Top,
					Coordinates.Z);
			}
		}

		public virtual Padding OuterPadding
		{
			get { return new Padding(0); }
		}

		public override Size Size
		{
			get { return size; }
			set { size = value; }
		}

		public override bool IntersectsWith(Point point)
		{
			return OuterBounds.IntersectsWith(new Rectangle(point, new Size(0, 0)));
		}

		public override bool IntersectsWith(Rectangle rect)
		{
			return OuterBounds.IntersectsWith(rect);
		}
		#endregion

		#region Properties
		protected GuiManager manager = null;

		/// <summary>
		/// Contains the manager for this component.
		/// </summary>
		public GuiManager GuiManager
		{
			get { return manager; }
			set
			{
				if (value == null)
					throw new Exception("Cannot assign a null manager");

				manager = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public override bool IsMouseMotionLocked
		{
			get
			{
				return beingDragged;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public override bool IsMouseButtonLocked
		{
			get
			{
				return true;
			}
		}
		#endregion
	}
}
