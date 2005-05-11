
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
using SdlDotNet.Sprites;
using System.Drawing;
using System.Globalization;

namespace SdlDotNet.Examples
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
		/// <param name="horizontal"></param>
		/// <param name="vertical"></param>
		/// <param name="coordinates"></param>
		public BoundedTextSprite(
			string textItem, 
			SdlDotNet.Font font,
			Size size,
			double horizontal, 
			double vertical,
			Point coordinates)
			: base(textItem, font, coordinates)
		{
			this.horizontal = horizontal;
			this.vertical = vertical;
			this.Size = size;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="textItem"></param>
		/// <param name="font"></param>
		/// <param name="size"></param>
		public BoundedTextSprite(
			string textItem, 
			SdlDotNet.Font font,
			Size size)
			: base(textItem, font)
		{
			this.Size = size;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="textItem"></param>
		/// <param name="font"></param>
		/// <param name="size"></param>
		/// <param name="coordinates"></param>
		public BoundedTextSprite(
			string textItem, 
			SdlDotNet.Font font,
			Size size,
			Point coordinates)	
			: base(textItem, font, coordinates)
		{
			this.Size = size;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="textItem"></param>
		/// <param name="font"></param>
		/// <param name="size"></param>
		/// <param name="coordinates"></param>
		public BoundedTextSprite(
			string textItem, 
			SdlDotNet.Font font,
			Size size,
			Vector coordinates)
			: base(textItem, font, coordinates)
		{
			this.Size = size;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="textItem"></param>
		/// <param name="font"></param>
		/// <param name="size"></param>
		/// <param name="horizontal"></param>
		/// <param name="vertical"></param>
		public BoundedTextSprite(
			string textItem, 
			SdlDotNet.Font font,
			Size size,
			double horizontal, 
			double vertical)
			: base(textItem, font)
		{
			this.Size = size;
			this.horizontal = horizontal;
			this.vertical = vertical;
		}

		#region Properties
		private double horizontal = 0.5;

		private double vertical = 0.5;

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
		#endregion

		private double delta = 0.01;

		private int move;
		private int direction = 1;

		/// <summary>
		/// All sprites are tickable, regardless if they actual do
		/// anything. This ensures that the functionality is there, to be
		/// overridden as needed.
		/// </summary>
		public override void Update(TickEventArgs args)
		{
			double dx = args.RatePerSecond(delta);
			this.HorizontalWeight += dx;

			if (this.HorizontalWeight > 1.0)
			{
				this.HorizontalWeight = 1.0;
				delta *= -1;
			}

			if (this.HorizontalWeight < 0.0)
			{
				this.HorizontalWeight = 0.0;
				delta *= -1;
			}

			Rectangle rectangle = this.Rectangle;
			rectangle.Offset(3 * direction * move, 0);
			this.Rectangle = rectangle;

			if (this.Position.X >= (Video.Screen.Width - 150))
			{
				move = 0;
				direction = -1;
			}
			else if (this.Position.X < 50)
			{
				move = 0;
				direction = 1;
			}
			move++;
      
			// Change the text
			this.TextString = 
				this.HorizontalWeight.ToString("#0.0000000", CultureInfo.CurrentCulture);
		}
		private bool disposed;


		/// <summary>
		/// Destroys the surface object and frees its memory
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
						this.Font.Dispose();
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
}
