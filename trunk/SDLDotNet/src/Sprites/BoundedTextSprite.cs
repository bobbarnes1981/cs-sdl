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
using System.Drawing;

namespace SdlDotNet.Sprites
{
	/// <summary>
	/// Implements a text sprite that has a bounded box to define its
	/// size and an orientation (as a float) for vertical and horizontal
	/// alignment.
	/// </summary>
	public class BoundedTextSprite : TextSprite
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="text"></param>
		/// <param name="font"></param>
		/// <param name="size"></param>
		public BoundedTextSprite(string text, SdlDotNet.Font font,
			Size size)
			: base(text, font)
		{
			this.size = size;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="text"></param>
		/// <param name="font"></param>
		/// <param name="size"></param>
		/// <param name="coords"></param>
//		public BoundedTextSprite(string text, SdlDotNet.Font font,
//			Dimension2 size,
//			Vector2 coords)
//			: base(text, font, coords)
		public BoundedTextSprite(string text, SdlDotNet.Font font,
						Size size,
						Point coords)
						: base(text, font, coords)
		{
			this.size = size;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="text"></param>
		/// <param name="font"></param>
		/// <param name="size"></param>
		/// <param name="coords"></param>
		public BoundedTextSprite(string text, SdlDotNet.Font font,
			
			Size size,
			Vector coords)
			: base(text, font, coords)
		{
			this.size = size;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="text"></param>
		/// <param name="font"></param>
		/// <param name="size"></param>
		/// <param name="horz"></param>
		/// <param name="vert"></param>
		public BoundedTextSprite(string text, SdlDotNet.Font font,
			Size size,
			double horz, double vert)
			: base(text, font)
		{
			this.size = size;
			this.horz = horz;
			this.vert = vert;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="text"></param>
		/// <param name="font"></param>
		/// <param name="size"></param>
		/// <param name="horz"></param>
		/// <param name="vert"></param>
		/// <param name="coords"></param>
//		public BoundedTextSprite(string text, SdlDotNet.Font font,
//			Dimension2 size,
//			double horz, double vert,
//			Vector2 coords)
		public BoundedTextSprite(string text, SdlDotNet.Font font,
			Size size,
			double horz, double vert,
			Point coords)
			: base(text, font, coords)
		{
			this.size = size;
			this.horz = horz;
			this.vert = vert;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="text"></param>
		/// <param name="font"></param>
		/// <param name="size"></param>
		/// <param name="horz"></param>
		/// <param name="vert"></param>
		/// <param name="coords"></param>
		public BoundedTextSprite(string text, SdlDotNet.Font font,
			Size size,
			double horz, double vert,
			Vector coords)
			: base(text, font, coords)
		{
			this.size = size;
			this.horz = horz;
			this.vert = vert;
		}

		#region Drawing
		/// <summary>
		/// 
		/// </summary>
		/// <param name="args"></param>
		public override void Render(RenderArgs args)
		{
			// Determine the offset
			Surface render = Surface;
			int width = Size.Width;
			int height = Size.Height;
			double dw = width - render.Width;
			double dh = height - render.Height;
			int offsetX = 0;
			int offsetY = 0;

			if (dw > 0.0)
				offsetX += (int) (dw * horz);

			if (dh > 0.0)
				offsetY += (int) (dh * vert);

			// Render the image itself
			args.Surface.Blit(render,
				new Rectangle(new Point(Coords.X
				+ offsetX + args.TranslateX,
				Coords.Y
				+ offsetY + args.TranslateY),
				Size));
		}
		#endregion

		#region Properties
		private double horz = 0.5;

		private double vert = 0.5;

		private Size size;

		/// <summary>
		/// 
		/// </summary>
		public double HorizontalWeight
		{
			get { return horz; }
			set { horz = value; renderSurf = null; }
		}

		/// <summary>
		/// 
		/// </summary>
		public double VerticalWeight
		{
			get { return vert; }
			set { vert = value; renderSurf = null; }
		}

		/// <summary>
		/// 
		/// </summary>
		public override Size Size
		{
			get
			{
				if (size.IsEmpty)
				{
					return base.Size;
				}
				else
				{
					return size;
				}
			}
		}
		#endregion
	}
}
