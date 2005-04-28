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
using System.Globalization;

namespace SdlDotNet.Examples.GuiExample
{
	/// <summary>
	/// A fairly complicated class that creates a menu bar that
	/// stretches across the top of a region. This bar handles menus in
	/// addition to normal sprites, allowing for a fairly complicated
	/// menubar system. The region itself determines where a menu may appear.
	/// </summary>
	public class GuiMenuBar : HorizontalPacker
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="gui"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="height"></param>
		public GuiMenuBar(GuiManager gui, int x, int y, int height)
			: base(gui, x, y, height)
		{
			this.Z = 10000;
		}

		#region Drawing
		#endregion

		#region Sprites
		/// <summary>
		/// 
		/// </summary>
		/// <param name="s"></param>
		public void AddLeft(SpriteContainer s)
		{
			AddHead(s);
			s.Position = new Point(0, 0);

			if (s.GetType().Name == "GuiMenuTitle")
			{
				((GuiMenuTitle) s).MenuBar = this;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="s"></param>
		public void AddRight(SpriteContainer s)
		{
			AddTail(s);

			if (s.GetType().Name == "GuiMenuTitle")
			{
				((GuiMenuTitle) s).MenuBar = this;
			}
		}
		#endregion

		#region Geometry
		/// <summary>
		/// 
		/// </summary>
		public override Padding InnerPadding
		{
			get 
			{ 
				return base.GuiManager.MenuTitlePadding; 
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public override Padding MarginPadding
		{
			get 
			{ 
				return base.GuiManager.MenuBarPadding; 
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
			return String.Format(CultureInfo.CurrentCulture, "(menu-bar {0})", base.ToString());
		}
		#endregion
	}
}
