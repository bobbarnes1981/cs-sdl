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
	/// A basic drawable that takes another one and splits it into one
	/// or more parts. The individual parts are converted into multiple frames.
	/// </summary>
	public class TiledDrawable : IDrawable
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="drawable"></param>
		/// <param name="size"></param>
		/// <param name="columns"></param>
		/// <param name="rows"></param>
		public TiledDrawable(IDrawable drawable,
			Size size,
			int columns, int rows)
		{
			// Save the fields
			this.drawable = drawable;
			Size = size;
			this.rows = rows;
			this.cols = columns;
		}

		#region Frames
		/// <summary>
		/// This gives an accessor to the surfaces of this
		/// drawable. This should throw an exception of there is no
		/// surface, but the system assumes that if there is at least one
		/// valid surface, then a drawable will always return the
		/// surface. This allows the system to request image #12 from a
		/// drawable with only one frame and get that same frame.
		/// </summary>
		public Surface this[int frame]
		{
			get
			{
				// Adjust the frame to make sure we don't have anything
				// outside of our limits
				frame = frame % (rows * cols);

				// Get the drawable's zero frame
				Surface s = drawable[0];

				// Figure out the row and colum to use
				int width = size.Width;
				int height = size.Height;
				int sx = (int) ((frame % cols) * width);
				int sy = (int) ((frame / cols) * height);

				// Create the smaller version
				Surface s1 = s.CreateCompatibleSurface((int) width, (int) height,
					true);
				Rectangle dRect = new Rectangle(new Point(0, 0),
					new Size((int) width, (int) height));
				Rectangle sRect = new Rectangle(new Point(sx, sy),
					new Size((int) width, (int) height));
				s1.SetAlpha(Alphas.RleEncoded, 255);
				s1.Blit(s, dRect, sRect);
				return s1;
			} 
		}

		/// <summary>
		/// Property that contains the number of frames (surfaces) in the
		/// drawable.
		/// </summary>
		public int FrameCount 
		{ 
			get 
			{ 
				return rows * cols; 
			} 
		}

		/// <summary>
		/// Returns a cache key which represents a unique identifier of
		/// the drawable. This is used by CachedDrawable to keep the CPU
		/// generation low.
		/// </summary>
		public string GetCacheKey(int frame)
		{
			return frame + "/" + FrameCount + "@" + drawable.GetCacheKey(0);
		}
		#endregion

		#region Properties
		private IDrawable drawable = null;
		private Size size = new Size();
		private int rows = 0;
		private int cols = 0;

		/// <summary>
		/// Contains the number of columns.
		/// </summary>
		public int Columns
		{
			get 
			{ 
				return cols; 
			}
			set 
			{ 
				cols = value; 
			}
		}

		/// <summary>
		/// Contains the drawable for this tiled drawable.
		/// </summary>
		public IDrawable Drawable
		{
			get 
			{ 
				return drawable; 
			}
			set 
			{ 
				drawable = value; 
			}
		}

		/// <summary>
		/// Contains the number of rows
		/// </summary>
		public int Rows
		{
			get 
			{ 
				return rows; 
			}
			set 
			{ 
				rows = value; 
			}
		}

		/// <summary>
		/// Returns the size of the drawable, in pixels.
		/// </summary>
		public Size Size
		{
			get 
			{ 
				return size; 
			}
			set
			{
				size = value;
			}
		}
		#endregion
	}
}
