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
	public class VerticalPacker : Packer
	{
		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		/// <param name="manager"></param>
		public VerticalPacker(GuiManager manager)
			: base(manager)
		{
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="manager"></param>
		/// <param name="p"></param>
		public VerticalPacker(GuiManager manager, Point p)
			: base(manager, p)
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="manager"></param>
		/// <param name="p"></param>
		public VerticalPacker(GuiManager manager, Vector p)
			: base(manager, p)
		{
		}
		#endregion

		#region Drawing
//		public override void Render(/*RenderArgs args*/)
//		{
//			// Call the base
//			base.Render(args);
//
//			// Handle our arguments
//			RenderArgs args0 = args.Clone();
//			args0.TranslateX += Coordinates.X + MarginPadding.Left + InnerPadding.Left;
//			args0.TranslateY += Coordinates.Y + MarginPadding.Top + InnerPadding.Top;
//
//			// Draw all of our left components
//			int y = 0;
//
//			foreach (Sprite s in HeadSprites)
//			{
//				// Ignore hidden
//				if (s.IsHidden)
//				{
//					continue;
//				}
//	
//				// Translate it and blit
//				Size size = GetSize(s);
//
//				s.Coordinates.Y = y;
//				s.Render(args0);
//
//				// Debugging
//				if (IsTraced)
//				{
//					args.Surface.DrawPixel(0 + args0.TranslateX,
//						y + args0.TranslateY,
//						System.Drawing.Color.CornflowerBlue);
//
//					args.Surface.DrawPixel(0 + args0.TranslateX,
//						y + args0.TranslateY
//						+ size.Height,
//						System.Drawing.Color.CornflowerBlue);
//				}
//
//				// Update the coordinates for the next one
//				y += size.Height + InnerPadding.Vertical;
//			}
//
//			// Draw our right components
//			y = Coordinates.Y + Size.Height - MarginPadding.Bottom;
//
//			foreach (Sprite s in TailSprites)
//			{
//				// Ignore hidden
//				if (s.IsHidden)
//				{
//					continue;
//				}
//	
//				// Translate it and blit
//				y -= s.Size.Height + InnerPadding.Vertical;
//				s.Coordinates.Y = y;
//				s.Render(args0);
//			}
//		}
		#endregion

		#region Geometry
//		/// <summary>
//		/// 
//		/// </summary>
//		public override Size Size
//		{
//			get
//			{
//				// Get the height
//				int width = 0;
//
//				// Get the sprites
//				foreach (Sprite s in new ArrayList(Sprites))
//				{
//					// Ignore hidden ones
////					if (s.IsHidden)
////					{
////						continue;
////					}
//
//					// Get the width
//					int w = GetSize(s).Width;
//
//					// Adjust the size
//					if (w > width)
//					{
//						width = w;
//					}
//				}
//
//				// Add the padding
//				width += InnerPadding.Horizontal;
//				return new Size(width, VerticalHeight);
//			}
//		}

		/// <summary>
		/// 
		/// </summary>
		public virtual int VerticalHeight
		{
			get
			{
				// Go through the sprites
				int height = 0;

				foreach (Sprite s in new ArrayList(Sprites))
				{
					// Ignore hidden ones
//					if (s.IsHidden)
//					{
//						continue;
//					}

					// Get the height
					int h = s.Size.Height;

					height += h + InnerPadding.Vertical;
				}

				return height;
			}
		}
		#endregion
	}
}
