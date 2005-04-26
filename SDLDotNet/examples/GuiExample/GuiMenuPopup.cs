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
using System.Collections;
using System.Drawing;

namespace SdlDotNet.Examples.GuiExample
{
	/// <summary>
	/// 
	/// </summary>
	public class GuiMenuPopup : VerticalPacker
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="manager"></param>
		public GuiMenuPopup(GuiManager manager)
			: base(manager, new Vector(12000))
		{
			this.Visible = true;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return String.Format("(menu {0})", base.ToString());
		}

		#region Drawing
		/// <summary>
		/// 
		/// </summary>
		public void ShowMenu()
		{
			this.Visible = true;
		}

		/// <summary>
		/// 
		/// </summary>
		public void HideMenu()
		{
			this.Visible = false;

			if (controller != null)
			{
				controller.IsSelected = false;
			}
		}
		#endregion

		#region Sprites
		/// <summary>
		/// 
		/// </summary>
		/// <param name="gmi"></param>
		public void Add(GuiMenuItem gmi)
		{
			AddHead(gmi);
		}
		#endregion

		#region Geometry
		private Point translate = new Point();
		#endregion

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override Surface Render()
		{
			this.Surface = base.Render();
			Video.Screen.Blit(this.Surface);
			return this.Surface;
		}


		#region Events
		/// <summary>
		/// Menus are a special case of a sprite. If the mouse is
		/// selected, then it shows the entire sprite, regardless of the
		/// packing size.
		/// </summary>
		public override void Update(MouseButtonEventArgs args)
		{
			// If we are being held down, pick up the marble
			if (args.ButtonPressed)
			{
				ShowMenu();
				this.Render();
			}
			else
			{
				// Check for an item
//				if (selected != null)
//				{
//					selected.OnMenuSelected(selectedIndex);
//					selected.IsSelected = false;
//					selected = null;
//				}

				// Remove the menu
				HideMenu();
			}
		}

		/// <summary>
		/// Uses the mouse motion to determine what menu item is actually
		/// selected and hilight it. If the menu is not selected, it does
		/// nothing.
		/// </summary>
		public override void Update(MouseMotionEventArgs args)
		{
			// Pull out some stuff
			int x = args.X - translate.X - Coordinates.X;
			int y = args.Y - translate.Y - Coordinates.Y;
			int relx = args.RelativeX;
			int rely = args.RelativeY;

			// Retrieve the item for these coordinates
			int ndx = 0;
			GuiMenuItem gmi = (GuiMenuItem) SelectSprite(new Point(x, y), ref ndx);

			// Check to see if we need to deselect an old one
//			if (selected != null && (gmi == null || gmi != selected))
//			{
//				// Clear out selected
//				selected.IsSelected = false;
//				selected = null;
//			}
//
//			// If we have a menu, select it
//			if (gmi != null)
//			{
//				gmi.IsSelected = true;
//				selected = gmi;
//				selectedIndex = ndx;
//			}
		}
		#endregion

		#region Properties
		//private GuiMenuItem selected;
		private IMenuPopupController controller;
		//private int selectedIndex = 0;

		/// <summary>
		/// 
		/// </summary>
		public IMenuPopupController Controller
		{
			get { return controller; }
			set { controller = value; }
		}
		#endregion
	}
}
