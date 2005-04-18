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

namespace SdlDotNet.Examples.GuiExample
{
	/// <summary>
	/// Base class to manage all graphical GUI elements.
	/// </summary>
	public class GuiComponent : SpriteContainer
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
			: base(new Vector(z))
		{
			this.manager = manager;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="manager"></param>
		/// <param name="position"></param>
		public GuiComponent(GuiManager manager, Point position)
			: base(new Vector(position))
		{
			this.manager = manager;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="manager"></param>
		/// <param name="rect"></param>
		public GuiComponent(GuiManager manager, Rectangle rect)
			: base(rect)
		{
			this.manager = manager;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="manager"></param>
		/// <param name="rect"></param>
		/// <param name="z"></param>
		public GuiComponent(GuiManager manager, Rectangle rect, int z)
			: base(rect)
		{
			this.manager = manager;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="manager"></param>
		/// <param name="coordinates"></param>
		public GuiComponent(GuiManager manager, Vector coordinates)
			: base(coordinates)
		{
			this.manager = manager;
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
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override Surface Render()
		{
			this.Surface.Fill(manager.BackgroundColor);
			this.Surface.DrawBox(new Rectangle(0, 0, this.Rectangle.Width, this.Rectangle.Height), manager.FrameColor);
			base.Sprites.Draw(this.Surface);
			return this.Surface;
		}
		#endregion

		#region Events
//		/// <summary>
//		/// GUI components default to mouse sensitive.
//		/// </summary>
//		public override bool IsMouseSensitive { get { return true; } }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="args"></param>
		public override void Update(MouseButtonEventArgs args)
		{
			// If we cannot be dragged, don't worry about it
			if (!AllowDrag)
			{
				return;
			}
			if (this.IntersectsWith(new Point(args.X, args.Y)))
			{
				// If we are being held down, pick up the marble
				if (args.ButtonPressed)
				{
					// Change the Z-order
					this.Z += manager.DragZOrder;
					this.BeingDragged = true;
					//manager.SpriteContainer.EventLock = this;
				}
				else
				{
					// Drop it
					this.Z -= manager.DragZOrder;
					this.BeingDragged = false;
					//manager.SpriteContainer.EventLock = null;
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="args"></param>
		public override void Update(MouseMotionEventArgs args)
		{
			int x = args.X;
			int y = args.Y;
			int relx = args.RelativeX;
			int rely = args.RelativeY;

			if (!AllowDrag)
			{
				return;
			}

			// Move the window as appropriate
			if (this.BeingDragged)
			{
				this.X += relx;
				this.Y += rely;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="args"></param>
		public override void Update(TickEventArgs args)
		{
			if (this.Sprites.Count != 0)
			{
				// Go through all the displayed sprites
				foreach (Sprite s in this.Sprites)
				{
					// Tick the sprite and wrap it in a translator
					s.Update(args);
				}
			}
		}
		#endregion

		#region Geometry
//		/// <summary>
//		/// 
//		/// </summary>
//		public Rectangle OuterBounds
//		{
//			get
//			{
//				return new Rectangle(new Point(OuterCoords.X, OuterCoords.Y), OuterSize);
//			}
//		}
//
//		/// <summary>
//		/// 
//		/// </summary>
//		public virtual Size OuterSize
//		{
//			get
//			{
//				return new Size(Size.Width + OuterPadding.Horizontal,
//					Size.Height + OuterPadding.Vertical);
//			}
//		}
//
//		/// <summary>
//		/// 
//		/// </summary>
//		public virtual Vector OuterCoords
//		{
//			get
//			{
//				return new Vector(this.X - OuterPadding.Left,
//					this.Y - OuterPadding.Top,
//					this.Z);
//			}
//		}
//
		/// <summary>
		/// 
		/// </summary>
		public virtual Padding OuterPadding
		{
			get { return new Padding(0); }
		}
//
//		/// <summary>
//		/// 
//		/// </summary>
//		/// <param name="point"></param>
//		/// <returns></returns>
//		public override bool IntersectsWith(Point point)
//		{
//			return OuterBounds.IntersectsWith(new Rectangle(point, new Size(0, 0)));
//		}
//
//		/// <summary>
//		/// 
//		/// </summary>
//		/// <param name="rect"></param>
//		/// <returns></returns>
//		public override bool IntersectsWith(Rectangle rect)
//		{
//			return OuterBounds.IntersectsWith(rect);
//		}
		#endregion

		#region Properties
		/// <summary>
		/// 
		/// </summary>
		private GuiManager manager;

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
		#endregion
	}
}
