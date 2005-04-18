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
	/// 
	/// </summary>
	public class GuiMenuItem : HorizontalPacker
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="manager"></param>
		public GuiMenuItem(GuiManager manager)
			: base(manager)
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="manager"></param>
		/// <param name="text"></param>
		public GuiMenuItem(GuiManager manager, string text)
			: base(manager)
		{
			AddLeft(new TextSprite(text, manager.BaseFont));
		}

		#region Sprites
		/// <summary>
		/// 
		/// </summary>
		/// <param name="s"></param>
		public void AddLeft(Sprite s)
		{
			AddHead(s);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="s"></param>
		public void AddRight(Sprite s)
		{
			AddTail(s);
		}
		#endregion

		#region Drawing
//		public void Render(RenderArgs args)
//		{
//			// We draw the rectangle normally
//			manager.Render(args, this);
//			base.Render(args);
//		}
		#endregion

		#region Geometry
//		//    public override bool IntersectsWith(Vector2 point)
//		/// <summary>
//		/// 
//		/// </summary>
//		/// <param name="point"></param>
//		/// <returns></returns>
//		public override bool IntersectsWith(Point point)
//		{
//			// Menu items are packed by their outer padding instead of the
//			// normal inner, so this has to be adjusted.
//			//Vector2 v = new Vector2(point);
//			point.X -= OuterPadding.Left;
//			point.Y -= OuterPadding.Top;
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
//			// Menu items are packed by their outer padding instead of the
//			// normal inner, so this has to be adjusted.
//			Rectangle r = rect;
////			r.Coordinates.X -= OuterPadding.Left;
////			r.Coordinates.Y -= OuterPadding.Top;
//			int transX;
//			int transY;
//			transY = r.Location.Y - OuterPadding.Top;
//			transX = r.Location.X - OuterPadding.Left;
//			r.Location = new Point(transX, transY);
//			return OuterBounds.IntersectsWith(r);
//		}
		#endregion

		#region Events
		/// <summary>
		/// 
		/// </summary>
		public event MenuItemHandler ItemSelectedEvent;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="index"></param>
		public virtual void OnMenuSelected(int index)
		{
			if (ItemSelectedEvent != null)
			{
				ItemSelectedEvent(index);
			}
		}
		#endregion

		#region Operators
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return String.Format("(menu-item {0})", base.ToString());
		}
		#endregion

		#region Properties
		private bool isSelected = false;
		//private GuiMenuPopup menu = null;

		/// <summary>
		/// 
		/// </summary>
		public bool IsSelected
		{
			get { return isSelected; }
			set { isSelected = value; }
		}

//		/// <summary>
//		/// 
//		/// </summary>
//		public GuiMenuPopup Menu
//		{
//			get { return menu; }
//			set { menu = value; }
//		}

		/// <summary>
		/// 
		/// </summary>
		public override Padding InnerPadding
		{
			get { return base.GuiManager.MenuItemInnerPadding; }
		}

		/// <summary>
		/// 
		/// </summary>
		public override Padding MarginPadding
		{
			get { return base.GuiManager.MenuItemPadding; }
		}

		/// <summary>
		/// 
		/// </summary>
		public override int HorizontalWidth
		{
			get
			{
				//if (menu == null)
				//{
					return base.HorizontalWidth;
				//}
				//else
				//{
				//	return menu.Size.Width;
				//}
			}
		}
		#endregion
	}

	/// <summary>
	/// 
	/// </summary>
	public delegate void MenuItemHandler(int index);
}
