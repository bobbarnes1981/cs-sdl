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
	/// Despite its name, the menubar title is actually the part in the
	/// menubarbar that gives the name.
	/// </summary>
	public class GuiMenuTitle : HorizontalPacker, IMenuPopupController
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="manager"></param>
		/// <param name="title"></param>
		/// <param name="menubar"></param>
		public GuiMenuTitle(GuiManager manager, GuiMenuBar menubar, string title)
			: base(manager)
		{
			this.menubar = menubar;
			this.popup = new GuiMenuPopup(manager);
			this.popup.Controller = this;
			//AddHead(this.popup);
      
			TextSprite ts = new TextSprite(title, manager.MenuFont);
			//this.Surface = ts.Surface;
			this.Rectangle = ts.Rectangle;
			AddHead(ts);
		}

		#region Drawing
		private int PopupX
		{
			get
			{
				return this.Rectangle.Left - this.GuiManager.MenuTitlePadding.Left;
			}
		}

		private int PopupY
		{
			get
			{
				return 
					this.Rectangle.Bottom + 
					this.GuiManager.MenuTitlePadding.Bottom + 
					this.GuiManager.MenuBarPadding.Bottom;
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
			if (!popup.Visible)
			{
				popup.Update(args);
				Video.Screen.Blit(popup.Render(), popup.Rectangle);
			}
			if (!args.ButtonPressed)
			{
				IsSelected = false;
				popup.HideMenu();
			}
			if (this.IntersectsWith(new Point(args.X + this.menubar.X, args.Y + this.menubar.Y)))
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
					Console.WriteLine("TitleClicked");
					IsSelected = true;
					popup.ShowMenu();
					//popup.Rectangle = new Rectangle(0,0,100,100);
					//Video.Screen.Blit(popup.Render(), popup.Rectangle);
				}

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
