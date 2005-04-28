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
using System.Drawing;
using System;
using System.Globalization;

namespace SdlDotNet.Examples.GuiExample
{
	/// <summary>
	/// Handles a simple window element, which displays its contents in
	/// a frame.
	/// </summary>
	public class GuiWindow : GuiComponent
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="manager"></param>
		public GuiWindow(GuiManager manager)
			: base(manager)
		{
			titleSprite = new TextSprite(" ", base.GuiManager.TitleFont,
				new Vector(0, 0, this.Z));
			base.Sprites.Add(titleSprite);
			this.title = string.Empty;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="manager"></param>
		/// <param name="rectangle"></param>
		/// <param name="coordinateZ"></param>
		public GuiWindow(GuiManager manager, Rectangle rectangle, int coordinateZ)
			: base(manager, rectangle, coordinateZ)
		{
			titleSprite = new TextSprite(" ", base.GuiManager.TitleFont,
				new Vector(0, 0, coordinateZ));
			base.Sprites.Add(titleSprite);
			this.title = string.Empty;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="manager"></param>
		/// <param name="rectangle"></param>
		public GuiWindow(GuiManager manager, Rectangle rectangle)
			: this(manager, rectangle, 0)
		{
			titleSprite = new TextSprite(" ", base.GuiManager.TitleFont,
				new Vector(0, 0, 0));
			base.Sprites.Add(titleSprite);
			this.title = string.Empty;
		}

		#region Drawing
//		/// <summary>
//		/// 
//		/// </summary>
//		public Padding OuterPadding
//		{
//			get { return manager.GetPadding(this); }
//		}

		#endregion

		#region Operators
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return String.Format(CultureInfo.CurrentCulture, "(window \"{0}\" {1})",
				Title, base.ToString());
		}
		#endregion

		#region Properties
		private string title;
		private Size titleSize = new Size();
		private TextSprite titleSprite;
		//public TextSprite TitleSprite


		/// <summary>
		/// 
		/// </summary>
		public string Title
		{
			get 
			{ 
				return title; 
			}
			set
			{
				// Set the title size
				title = value;

				// Set the bounds
				titleSize = GuiManager.GetTextSize(this.GuiManager.TitleFont, title);
				//this.Sprites.Remove(titleSprite);
				titleSprite.TextString = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public Color TitleColor
		{
			get
			{
				return titleSprite.Color;
			}
			set
			{
				titleSprite.Color = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public Color TitleBackgroundColor
		{
			get
			{
				return titleSprite.BackgroundColor;
			}
			set
			{
				titleSprite.BackgroundColor = value;
			}
		}
		#endregion
	}
}
