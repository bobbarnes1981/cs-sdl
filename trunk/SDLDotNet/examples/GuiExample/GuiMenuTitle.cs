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

namespace SdlDotNet.Examples.GuiExample
{
	/// <summary>
	/// Despite its name, the menubar title is actually the part in the
	/// menubarbar that gives the name.
	/// </summary>
	public class GuiMenuTitle : HorizontalPacker, IMenuPopupController
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="manager"></param>
		public GuiMenuTitle(GuiManager manager)
			: base(manager)
		{
			this.menubar = menubar;
			this.popup = new GuiMenuPopup(manager);
			this.popup.Controller = this;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="manager"></param>
		/// <param name="title"></param>
		public GuiMenuTitle(GuiManager manager, string title)
			: base(manager)
		{
			this.menubar = menubar;
			this.popup = new GuiMenuPopup(manager);
			this.popup.Controller = this;
      
			TextSprite ts = new TextSprite(title, manager.BaseFont);
			this.Size = ts.Size;
			AddHead(ts);
		}

		#region Drawing
		private int PopupX
		{
			get
			{
				//return OuterBounds.Left - manager.MenuTitlePadding.Left;
				return this.Rectangle.Left - - base.GuiManager.MenuTitlePadding.Left;
			}
		}

		private int PopupY
		{
			get
			{
				return this.Rectangle.Bottom;
				//return OuterBounds.Bottom;
					//+ popup.OuterPadding.Top
					//+ manager.MenuTitlePadding.Bottom
					//+ manager.MenuBarPadding.Bottom;
			}
		}

//		public new void Render(RenderArgs args)
//		{
//			// Draw ourselves
//			manager.Render(args, this);
//			base.Render(args);
//
//			// Push the args
//			RenderArgs args1 = args.Clone();
//			args1.TranslateX += PopupX;
//			args1.TranslateY += PopupY;
//
//			// Check for tracing renders
//			if (IsTraced)
//			{
//				// Put a small dot where the menu will show up
//				int px = 0 + args.TranslateX;
//				int py = 0 + args.TranslateY;
//
//				//Debug("ar: {0}, ar1: {1}", args.Vector, args1.Vector);
//
//				args.Surface.DrawPixel(px, py, System.Drawing.Color.BlanchedAlmond);
//				GuiManager.DrawRect(args.Surface,
//					args1.Translate(popup.OuterBounds),
//					manager.OuterBoundsTraceColor);
//			}
//
//			// Check for menu
//			if (IsSelected)
//			{
//				popup.Coordinates.X = 0;
//				popup.Coordinates.Y = 0;
//				popup.Render(args1);
//			}
//		}
		#endregion

		#region Sprites
		/// <summary>
		/// 
		/// </summary>
		/// <param name="gmi"></param>
		public void Add(GuiMenuItem gmi)
		{
			popup.Add(gmi);
		}
		#endregion

		#region Events
		/// <summary>
		/// 
		/// </summary>
		/// <param name="args"></param>
		public override void Update(MouseButtonEventArgs args)
		{
			//			// Build up the translations
			//			MouseButtonEventArgs args1 = args.Clone();
			//			args1.TranslateX += PopupX;
			//			args1.TranslateY += PopupY;

			// Check for clicking down
			//			if (!popup.IsHidden)
			//			{
			//				popup.OnMouseButtonDown(this, args);
			//			}

			if (args.ButtonPressed)
			{
				IsSelected = true;
				//popup.ShowMenu();
				//return true;
			}

			//return false;
		}

		//		/// <summary>
		//		/// 
		//		/// </summary>
		//		/// <param name="sender"></param>
		//		/// <param name="args"></param>
		//		public override void Update(object sender, MouseButtonEventArgs args)
		//		{
		//			if (popup.IsHidden)
		//			{
		//				popup.OnMouseButtonUp(this, args);
		//			}
		//			if (!args.ButtonPressed)
		//			{
		//				IsSelected = false;
		//				popup.HideMenu();
		//				//return true;
		//			}
		//}

//		/// <summary>
//		/// 
//		/// </summary>
//		/// <param name="args"></param>
//		public override void Update(TickEventArgs args)
//		{
//			// Call our base's tick processing
//			base.OnTick(this, args);
//
//			// If we are showing our menu, display it
//			/* TODO This kills things
//					if (IsSelected)
//				  popup.OnTick(args);
//					*/
//		}

		#endregion

		#region Operators
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return String.Format("(menu-title {0})", base.ToString());
		}
		#endregion

		#region Properties
		private GuiMenuBar menubar;
		private GuiMenuPopup popup;

		private bool selected = false;

		/// <summary>
		/// 
		/// </summary>
		public bool IsSelected
		{
			get { return selected; }
			set { selected = value; }
		}

		/// <summary>
		/// 
		/// </summary>
		public GuiMenuBar MenuBar
		{
			get { return menubar; }
			set { menubar = value; }
		}

		/// <summary>
		/// 
		/// </summary>
		public GuiMenuPopup Popup
		{
			get { return popup; }
		}
		#endregion
	}
}
