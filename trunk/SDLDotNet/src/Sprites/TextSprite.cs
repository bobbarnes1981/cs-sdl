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

using SdlDotNet.Utility;
using SdlDotNet;
using System;
using System.Drawing;

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
		/// <param name="text"></param>
		/// <param name="font"></param>
		public TextSprite(string text, SdlDotNet.Font font)
			: base()
		{
			this.text = text;
			this.font = font;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="text"></param>
		/// <param name="font"></param>
		/// <param name="color"></param>
		public TextSprite(string text, SdlDotNet.Font font, Color color)
			: base()
		{
			this.text = text;
			this.font = font;
			this.color = color;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="text"></param>
		/// <param name="font"></param>
		/// <param name="coords"></param>
//		public TextSprite(string text, SdlDotNet.Font font, Vector2 coords)
		public TextSprite(string text, SdlDotNet.Font font, Point coords)
			: base(coords)
		{
			this.text = text;
			this.font = font;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="text"></param>
		/// <param name="font"></param>
		/// <param name="color"></param>
		/// <param name="coords"></param>
//		public TextSprite(string text, SdlDotNet.Font font, Color color,
//			Vector2 coords)
		public TextSprite(string text, SdlDotNet.Font font, Color color,
			Point coords)
			: base(coords)
		{
			this.text = text;
			this.font = font;
			this.color = color;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="text"></param>
		/// <param name="font"></param>
		/// <param name="coords"></param>
		public TextSprite(string text, SdlDotNet.Font font,
			Vector coords)
			: base(coords)
		{
			this.text = text;
			this.font = font;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="text"></param>
		/// <param name="font"></param>
		/// <param name="color"></param>
		/// <param name="coords"></param>
		public TextSprite(string text, SdlDotNet.Font font, Color color,
			Vector coords)
			: base(coords)
		{
			this.text = text;
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
			if (Text == null || font == null)
				return;

			// Render it (Solid or Blended)
			try
			{
				renderSurf = font.Render(Text, color);
			}
			catch
			{
				renderSurf = null;
				//throw new SdlException("Cannot render text: {0}", e);
			}
		}

		/// <summary>
		/// Displays the font using the arguments given.
		/// </summary>
		public override void Render(RenderArgs args)
		{
			// Blit out the render
			args.Surface.Blit(Surface,
				//new Rectangle(new Vector2(Coords.X + args.TranslateX,
				new Rectangle(new Point(Coords.X + args.TranslateX,
				Coords.Y + args.TranslateY),
				renderSurf.Size));
		}
		#endregion

		#region Font Rendering
		/// <summary>
		/// 
		/// </summary>
		protected Surface renderSurf = null;

		private SdlDotNet.Font font = null;

		private string text = null;

		private Color color = Color.White;

		/// <summary>
		/// 
		/// </summary>
		public Color Color
		{
			get { return color; }
			set { color = value; renderSurf = null; }
		}

		/// <summary>
		/// 
		/// </summary>
		public SdlDotNet.Font Font
		{
			get { return font; }
			set { font = value; renderSurf = null; }
		}

		/// <summary>
		/// 
		/// </summary>
		public Surface Surface
		{
			get
			{
				if (renderSurf == null)
					RenderSurface();

				if (renderSurf == null)
					throw new SpriteException("Cannot render text");

				return renderSurf;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string Text
		{
			get { return text; }
			set { text = value; renderSurf = null; }
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
					RenderSurface();

				if (renderSurf == null)
					return new Size(0, 0);
				else
					return new Size(renderSurf.Width, renderSurf.Height);
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
			return String.Format("(text \"{0}\" {1})", text, base.ToString());
		}
		#endregion
	}
}
