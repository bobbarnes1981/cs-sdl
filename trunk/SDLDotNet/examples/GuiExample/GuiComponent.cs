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
using System.Globalization;

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
			: base()
		{
			this.manager = manager;
			this.Z = z;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="manager"></param>
		/// <param name="position"></param>
		public GuiComponent(GuiManager manager, Point position)
			: base()
		{
			this.manager = manager;
			this.Position = position;
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
		/// <param name="rectangle"></param>
		/// <param name="z"></param>
		public GuiComponent(GuiManager manager, Rectangle rectangle, int z)
			: this(manager, rectangle)
		{
			this.Z = z;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="manager"></param>
		/// <param name="coordinates"></param>
		public GuiComponent(GuiManager manager, Vector coordinates)
			: base()
		{
			this.manager = manager;
			this.Coordinates = coordinates;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return String.Format(CultureInfo.CurrentCulture, "(gui {0})", this.Rectangle, base.ToString());
		}

		#region Drawing
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override Surface Render()
		{
			this.Surface.Fill(manager.BackgroundColor);
			this.Surface.DrawBox(
				new Rectangle(0, 0, this.Rectangle.Width, this.Rectangle.Height),
				manager.FrameColor);
			return base.Render();
		}
		#endregion

		#region Events
		/// <summary>
		/// 
		/// </summary>
		/// <param name="args"></param>
		public override void Update(MouseButtonEventArgs args)
		{
			// Return immediately if component cannt be dragged
			if (!AllowDrag)
			{
				return;
			}
			if (this.IntersectsWith(new Point(args.X, args.Y)))
			{
				// If we are being held down, pick up the component
				if (args.ButtonPressed)
				{
					// Change the Z-order
					this.Z += manager.DragZOrder;
					this.BeingDragged = true;
				}
				else
				{
					// Drop it
					this.Z -= manager.DragZOrder;
					this.BeingDragged = false;
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="args"></param>
		public override void Update(MouseMotionEventArgs args)
		{
			if (!AllowDrag)
			{
				return;
			}

			// Move the window as appropriate
			if (this.BeingDragged)
			{
				this.X += args.RelativeX;
				this.Y += args.RelativeY;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="args"></param>
		public override void Update(TickEventArgs args)
		{
			for (int i = 0; i < this.Sprites.Count; i++)
			{
				this.Sprites[i].Update(args);
			}
		}
		#endregion

		#region Geometry
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
					throw new SdlException("Cannot assign a null manager");
				}

				manager = value;
			}
		}
		#endregion
	}
}
