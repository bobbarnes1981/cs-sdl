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
		/// <param name="textItem"></param>
		/// <param name="font"></param>
		/// <param name="size"></param>
		public BoundedTextSprite(string textItem, SdlDotNet.Font font,
			Size size)
			: base(textItem, font)
		{
			this.size = size;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="textItem"></param>
		/// <param name="font"></param>
		/// <param name="size"></param>
		/// <param name="coordinates"></param>
		public BoundedTextSprite(string textItem, SdlDotNet.Font font,
						Size size,
						Point coordinates)
						: base(textItem, font, coordinates)
		{
			this.size = size;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="textItem"></param>
		/// <param name="font"></param>
		/// <param name="size"></param>
		/// <param name="coordinates"></param>
		public BoundedTextSprite(string textItem, SdlDotNet.Font font,
			
			Size size,
			Vector coordinates)
			: base(textItem, font, coordinates)
		{
			this.size = size;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="textItem"></param>
		/// <param name="font"></param>
		/// <param name="size"></param>
		/// <param name="horizontal"></param>
		/// <param name="vertical"></param>
		public BoundedTextSprite(string textItem, SdlDotNet.Font font,
			Size size,
			double horizontal, double vertical)
			: base(textItem, font)
		{
			this.size = size;
			this.horizontal = horizontal;
			this.vertical = vertical;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="textItem"></param>
		/// <param name="font"></param>
		/// <param name="size"></param>
		/// <param name="horizontal"></param>
		/// <param name="vertical"></param>
		/// <param name="coordinates"></param>
		public BoundedTextSprite(string textItem, SdlDotNet.Font font,
			Size size,
			double horizontal, double vertical,
			Point coordinates)
			: base(textItem, font, coordinates)
		{
			this.size = size;
			this.horizontal = horizontal;
			this.vertical = vertical;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="textItem"></param>
		/// <param name="font"></param>
		/// <param name="size"></param>
		/// <param name="horizontal"></param>
		/// <param name="vertical"></param>
		/// <param name="coordinates"></param>
		public BoundedTextSprite(string textItem, SdlDotNet.Font font,
			Size size,
			double horizontal, double vertical,
			Vector coordinates)
			: base(textItem, font, coordinates)
		{
			this.size = size;
			this.horizontal = horizontal;
			this.vertical = vertical;
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
			{
				offsetX += (int) (dw * horizontal);
			}

			if (dh > 0.0)
			{
				offsetY += (int) (dh * vertical);
			}

			// Render the image itself
			args.Surface.Blit(render,
				new Rectangle(new Point(Coordinates.X
				+ offsetX + args.TranslateX,
				Coordinates.Y
				+ offsetY + args.TranslateY),
				Size));
		}
		#endregion

		#region Properties
		private double horizontal = 0.5;

		private double vertical = 0.5;

		private Size size;

		/// <summary>
		/// 
		/// </summary>
		public double HorizontalWeight
		{
			get 
			{ 
				return horizontal; 
			}
			set 
			{ 
				horizontal = value; 
				this.Surface = null; 
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public double VerticalWeight
		{
			get 
			{ 
				return vertical; 
			}
			set 
			{ 
				vertical = value; 
				this.Surface = null; 
			}
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
