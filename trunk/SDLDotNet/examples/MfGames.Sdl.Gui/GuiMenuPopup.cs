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

namespace MFGames.Sdl.Gui
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
			: base(manager, new Vector(1000))
		{
			//IsHidden = true;
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
//		public override void Render(RenderArgs args)
//		{
//			// We use the same formula as the horizontal packer to find out
//			// our own offset. This is used to handle the mouse events
//			// because the EventLock does not translate the values before
//			// sending it.
//			RenderArgs args0 = args.Clone();
//			args0.TranslateX += Coordinates.X + OuterPadding.Left + InnerPadding.Left;
//			args0.TranslateY += Coordinates.Y + OuterPadding.Top + InnerPadding.Top;
//			translate = args0.Point;
//
//			// Check for exceeding
//			int right = translate.X + Size.Width + manager.MenuTitlePadding.Right;
//			if (right > manager.Size.Width)
//			{
//				// We have to adjust things over
//				int off = right - manager.Size.Width;
//				args = args.Clone();
//				args.TranslateX -= off;
//				translate.X -= off;
//			}
//
//			// Draw the element
//			manager.Render(args, this);
//			base.Render(args);
//
//			// Trace the items
//			if (IsTraced)
//			{
//				foreach (Sprite s in new ArrayList(Sprites))
//				{
//					Rectangle r = new Rectangle(translate, GetSize(s));
//					GuiManager.DrawRect(args.Surface, r, manager.TraceColor);
//				}
//			}
//		}

		/// <summary>
		/// 
		/// </summary>
		public void ShowMenu()
		{
			//IsHidden = false;
			//manager.SpriteContainer.EventLock = this;
		}

		/// <summary>
		/// 
		/// </summary>
		public void HideMenu()
		{
//			IsHidden = true;
//			manager.SpriteContainer.EventLock = null;

			if (controller != null)
				controller.IsSelected = false;
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
			//gmi.Menu = this;
		}
		#endregion

		#region Geometry
		private Point translate = new Point();

		/// <summary>
		/// 
		/// </summary>
		public override Padding OuterPadding
		{
			get { return manager.MenuPopupPadding; }
		}

		/// <summary>
		/// 
		/// </summary>
		public override Size Size
		{
			get
			{
				// Clear out the popups. If the GMI has a menu associated with
				// it, it uses that for the width, which would produce an
				// infinite loop for processing. Removing the association
				// allows the size to be retrieved properly.
//				foreach (GuiMenuItem gmi in Sprites)
//					gmi.Menu = null;

				// Get the base
				Size d = base.Size;

				// Reassociate this item
//				foreach (GuiMenuItem gmi in Sprites)
//					gmi.Menu = this;

				// Return the dimension
				return d;
			}
		}
		#endregion

		#region Events
		/// <summary>
		/// Menus are a special case of a sprite. If the mouse is
		/// selected, then it shows the entire sprite, regardless of the
		/// packing size.
		/// </summary>
		public override void Update(object sender, MouseButtonEventArgs args)
		{
			// If we are being held down, pick up the marble
			if (args.ButtonPressed)
			{
				ShowMenu();
			}
			else
			{
				// Check for an item
				if (selected != null)
				{
					selected.OnMenuSelected(selectedIndex);
					selected.IsSelected = false;
					selected = null;
				}

				// Remove the menu
				HideMenu();
			}
		}

//		public override void Update(object sender, MouseButtonEventArgs args)
//		{
//				// Check for an item
//				if (selected != null)
//				{
//					selected.OnMenuSelected(selectedIndex);
//					selected.IsSelected = false;
//					selected = null;
//				}
//
//				// Remove the menu
//				HideMenu();
//		}

		/// <summary>
		/// Uses the mouse motion to determine what menu item is actually
		/// selected and hilight it. If the menu is not selected, it does
		/// nothing.
		/// </summary>
		public override void Update(object sender, MouseMotionEventArgs args)
		{
			// Pull out some stuff
			int x = args.X - translate.X - Coordinates.X;
			int y = args.Y - translate.Y - Coordinates.Y;
			int relx = args.RelativeX;
			int rely = args.RelativeY;

			// Don't bother if we are not selected
//			if (IsHidden)
//				return false;

			// Retrieve the item for these coordinates
			int ndx = 0;
			//GuiMenuItem gmi = (GuiMenuItem) SelectSprite(new Vector2(x, y), ref ndx);
			GuiMenuItem gmi = (GuiMenuItem) SelectSprite(new Point(x, y), ref ndx);

			// Check to see if we need to deselect an old one
			if (selected != null && (gmi == null || gmi != selected))
			{
				// Clear out selected
				selected.IsSelected = false;
				selected = null;
			}

			// If we have a menu, select it
			if (gmi != null)
			{
				gmi.IsSelected = true;
				selected = gmi;
				selectedIndex = ndx;
			//	return true;
			}

			/*
			// We don't have a menu item and we are not selecting
			// anything. See if we are current over a title of another menu.
			int x1 = x + Coordinates.X;
			int y1 = y + Coordinates.Y;
			ArrayList moreSprites =
		  manager.SpriteContainer.GetSprites(new Vector2(x1, y1));

			foreach (Sprite s in moreSprites)
			{
		  if (s is GuiMenuTitle)
		  {
			// This is the menu we should select
			GuiMenuTitle gmt = (GuiMenuTitle) s;
			IsSelected = false;
			HideMenu();
			gm.IsSelected = true;
			gm.Popup.ShowMenu();
		  }
			}
			*/

			// We are done processing
			//return true;
		}
		#endregion

		#region Properties
		private GuiMenuItem selected = null;
		private IMenuPopupController controller = null;
		private int selectedIndex = 0;

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
