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

using SdlDotNet;
using System;
using System.Drawing;
using System.Globalization;

namespace SdlDotNet.Sprites
{
	/// <summary>
	/// Implements a basic font that is given a font and a string and
	/// generates an appropriate surface from that font.
	/// </summary>
	public class TextSprite : Sprite
	{
//		/// <summary>
//		/// 
//		/// </summary>
//		public TextSprite()
//			: base()
//		{
//		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="textItem"></param>
		/// <param name="font"></param>
		public TextSprite(string textItem, SdlDotNet.Font font)
		{
			base.Position = new Point(0, 0);
			this.textItem = textItem;
			this.font = font;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="textItem"></param>
		/// <param name="font"></param>
		/// <param name="color"></param>
		public TextSprite(string textItem, SdlDotNet.Font font, Color color)
		{
			base.Position = new Point(0, 0);
			this.textItem = textItem;
			this.font = font;
			this.color = color;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="textItem"></param>
		/// <param name="font"></param>
		/// <param name="position"></param>
		public TextSprite(string textItem, SdlDotNet.Font font, Point position)
		{
			base.Position = position;
			this.textItem = textItem;
			this.font = font;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="textItem"></param>
		/// <param name="font"></param>
		/// <param name="color"></param>
		/// <param name="position"></param>
		public TextSprite(string textItem, SdlDotNet.Font font, Color color,
			Point position)
			: this(textItem, font, position)
		{
			this.color = color;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="textItem"></param>
		/// <param name="font"></param>
		/// <param name="coordinates"></param>
		public TextSprite(string textItem, SdlDotNet.Font font,
			Vector coordinates) : this(textItem, font)
		{
			this.Coordinates = coordinates;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="textItem"></param>
		/// <param name="font"></param>
		/// <param name="color"></param>
		/// <param name="coordinates"></param>
		public TextSprite(string textItem, SdlDotNet.Font font, Color color,
			Vector coordinates)
			: this(textItem, font, coordinates)
		{
			this.color = color;
		}

		#region Drawing
		/// <summary>
		/// Renders the font, if both the text and color and font are
		/// set. It stores the render in memory until it is used.
		/// </summary>
		private Surface RenderText()
		{
			// Clear it
			this.Surface = null;

			// Don't bother rendering if we don't have a text and a font
			if (TextString == null || font == null)
			{
				return null;
			}

			// Render it (Solid or Blended)
			try
			{
				Surface surf = font.Render(TextString, color);
				this.Size = new Size(surf.Width, surf.Height);
				return surf;
			}
			catch (SpriteException e)
			{
				this.Surface = null;
				throw new SdlException("Text sprite exception", e);
			}
		}
		#endregion

		#region Font Rendering
		/// <summary>
		/// 
		/// </summary>
		//private Surface surface = null;

		private SdlDotNet.Font font;

		private string textItem;

		private Color color = Color.White;

		/// <summary>
		/// 
		/// </summary>
		public Color Color
		{
			get 
			{ 
				return color; 
			}
			set 
			{ 
				color = value; 
				this.Surface = null; 
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public SdlDotNet.Font Font
		{
			get 
			{ 
				return font; 
			}
			set 
			{ 
				font = value; 
				this.Surface = null; 
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public override Surface Surface
		{
			get
			{
				if (base.Surface == null)
				{
					base.Surface = RenderText();
				}
				return base.Surface;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string TextString
		{
			get 
			{ 
				return textItem; 
			}
			set 
			{ 
				textItem = value; 
				base.Surface = null; 
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
			return String.Format(CultureInfo.CurrentCulture, "(text \"{0}\",{1})", textItem, base.ToString());
		}
		#endregion
	}
}
