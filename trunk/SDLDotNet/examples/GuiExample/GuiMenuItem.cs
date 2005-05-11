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
			TextSprite ts = new TextSprite(text, manager.BaseFont);
			this.Surface = ts.Surface;
			AddLeft(ts);
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
		#endregion

		#region Geometry
		#endregion

		#region Events
		/// <summary>
		/// 
		/// </summary>
		public event MenuItemEventHandler ItemSelectedEvent;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="index"></param>
		public virtual void OnMenuSelected(int index)
		{
			if (ItemSelectedEvent != null)
			{
				ItemSelectedEvent(this, new MenuItemEventArgs(index));
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
			return String.Format(CultureInfo.CurrentCulture, "(menu-item {0})", base.ToString());
		}
		#endregion

		#region Properties
		private bool isSelected;

		/// <summary>
		/// 
		/// </summary>
		public bool IsSelected
		{
			get 
			{ 
				return isSelected; 
			}
			set 
			{ 
				isSelected = value; 
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public override Padding InnerPadding
		{
			get 
			{ 
				return base.GuiManager.MenuItemInnerPadding; 
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public override Padding MarginPadding
		{
			get 
			{ 
				return base.GuiManager.MenuItemPadding; 
			}
		}
		#endregion
		private bool disposed;
		/// <summary>
		/// 
		/// </summary>
		/// <param name="disposing"></param>
		protected override void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				try
				{
					if (disposing)
					{
						this.Surface.Dispose();
						foreach (Sprite s in this.Sprites)
						{
							IDisposable disposableObj = s as IDisposable;
							if (disposableObj != null)
							{
								disposableObj.Dispose( );
							}
						}
					}
					this.disposed = true;
				}
				finally
				{
					base.Dispose(disposing);
					this.disposed = true;
				}
			}
		}
	}

	/// <summary>
	/// 
	/// </summary>
	public delegate void MenuItemEventHandler(object sender, MenuItemEventArgs e);
}
