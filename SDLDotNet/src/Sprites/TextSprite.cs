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
		/// <summary>
		/// 
		/// </summary>
		public TextSprite()
			: base()
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="textItem"></param>
		/// <param name="font"></param>
		public TextSprite(string textItem, SdlDotNet.Font font)
			: base()
		{
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
			: base()
		{
			this.textItem = textItem;
			this.font = font;
			this.color = color;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="textItem"></param>
		/// <param name="font"></param>
		/// <param name="coordinates"></param>
		public TextSprite(string textItem, SdlDotNet.Font font, Point coordinates)
			: base(coordinates)
		{
			this.textItem = textItem;
			this.font = font;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="textItem"></param>
		/// <param name="font"></param>
		/// <param name="color"></param>
		/// <param name="coordinates"></param>
		public TextSprite(string textItem, SdlDotNet.Font font, Color color,
			Point coordinates)
			: base(coordinates)
		{
			this.textItem = textItem;
			this.font = font;
			this.color = color;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="textItem"></param>
		/// <param name="font"></param>
		/// <param name="coordinates"></param>
		public TextSprite(string textItem, SdlDotNet.Font font,
			Vector coordinates)
			: base(coordinates)
		{
			this.textItem = textItem;
			this.font = font;
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
			: base(coordinates)
		{
			this.textItem = textItem;
			this.font = font;
			this.color = color;
		}

		#region Drawing
		/// <summary>
		/// Renders the font, if both the text and color and font are
		/// set. It stores the render in memory until it is used.
		/// </summary>
		private void RenderSurface()
		{
			// Clear it
			renderSurf = null;

			// Don't bother rendering if we don't have a text and a font
			if (TextString == null || font == null)
			{
				return;
			}

			// Render it (Solid or Blended)
			try
			{
				renderSurf = font.Render(TextString, color);
			}
			catch (Exception e)
			{
				renderSurf = null;
				throw e;
			}
		}

		/// <summary>
		/// Displays the font using the arguments given.
		/// </summary>
		public override void Render(RenderArgs args)
		{
			// Blit out the render
			args.Surface.Blit(Surface,
				new Rectangle(new Point(Coordinates.X + args.TranslateX,
				Coordinates.Y + args.TranslateY),
				renderSurf.Size));
		}
		#endregion

		#region Font Rendering
		/// <summary>
		/// 
		/// </summary>
		private Surface renderSurf = null;

		private SdlDotNet.Font font = null;

		private string textItem = null;

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
				renderSurf = null; 
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
				renderSurf = null; 
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public Surface Surface
		{
			get
			{
				if (renderSurf == null)
				{
					RenderSurface();
				}
				return renderSurf;
			}
			set
			{
				renderSurf = value;
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
				renderSurf = null; 
			}
		}
		#endregion

		#region Geometry
		/// <summary>
		/// 
		/// </summary>
		public override Size Size
		{
			get
			{
				if (renderSurf == null)
				{
					RenderSurface();
				}

				if (renderSurf == null)
				{
					return new Size(0, 0);
				}
				else
				{
					return new Size(renderSurf.Width, renderSurf.Height);
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
			return String.Format(CultureInfo.CurrentCulture, "(text \"{0}\",{1})", textItem, base.ToString());
		}
		#endregion
	}
}
