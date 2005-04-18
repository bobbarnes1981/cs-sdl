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

namespace SdlDotNet.Examples.GuiExample
{
	/// <summary>
	/// Class to manager internal sprites, such as window
	/// components. This uses a sprite manager at its core, but does
	/// have some additional functionality.
	/// </summary>
	public class HorizontalPacker : Packer
	{
		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		/// <param name="manager"></param>
		public HorizontalPacker(GuiManager manager)
			: base(manager)
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="manager"></param>
		/// <param name="p"></param>
		public HorizontalPacker(GuiManager manager, Point p)
			: base(manager, p)
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="manager"></param>
		/// <param name="p"></param>
		public HorizontalPacker(GuiManager manager, Vector p)
			: base(manager, p)
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="gui"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="height"></param>
		public HorizontalPacker(GuiManager gui, int x, int y, int height)
			: base(gui, x, y, height)
		{
		}
		#endregion

		#region Drawing
//		/// <summary>
//		/// 
//		/// </summary>
//		public override void Render(/*RenderArgs args*/)
//		{
//			// Handle our arguments
//			RenderArgs args0 = args.Clone();
//			args0.TranslateX += Coordinates.X + MarginPadding.Left + InnerPadding.Left;
//			args0.TranslateY += Coordinates.Y + MarginPadding.Top + InnerPadding.Top;
//
//			// Call the base
//			base.Render(args0);
//
//			// Draw all of our left components
//			int x = 0;
//
//			foreach (Sprite s in HeadSprites)
//			{
//				// Ignore hidden
//				if (s.IsHidden)
//					continue;
//	
//				// Translate it and blit
//				s.Coordinates.X = x;
//				s.Render(args0);
//
//				// Update the coordinates for the next one
//				x += s.Size.Width + InnerPadding.Horizontal;
//			}
//
//			// Draw our right components
//			x = Coordinates.X + Size.Width - MarginPadding.Right;
//
//			foreach (Sprite s in TailSprites)
//			{
//				// Ignore hidden
//				if (s.IsHidden)
//					continue;
//	
//				// Translate it and blit
//				x -= s.Size.Width + InnerPadding.Horizontal;
//				s.Coordinates.X = x;
//				s.Render(args0);
//			}
//		}
		#endregion

		#region Geometry
		/// <summary>
		/// 
		/// </summary>
		public override Size Size
		{
			get
			{
				// Get the height
				//int height = 20;
				//int width = 0;

				// Get the sprites
				foreach (Sprite s in this.Sprites)
				{
					int h = s.Size.Height;
					int w = s.Size.Width;

					if (h > this.Height)
					{
						this.Height = h;
					}
					if (w > this.Width)
					{
						this.Width = w;
					}
				}

				// Add the padding
//				height += 
//					InnerPadding.Vertical + MarginPadding.Vertical;

				//return new Size(HorizontalWidth, height);
				return new Size(this.Width, this.Height);

			}
		}

//		/// <summary>
//		/// 
//		/// </summary>
//		public virtual int HorizontalWidth
//		{
//			get
//			{
//				// Go through the sprites
//				int width = 0;
//
//				foreach (Sprite s in new ArrayList(Sprites))
//				{
//					int w = s.Size.Width;
//
//					width += w + InnerPadding.Horizontal;
//				}
//
//				return width + MarginPadding.Horizontal;
//			}
//		}
		#endregion
	}
}
