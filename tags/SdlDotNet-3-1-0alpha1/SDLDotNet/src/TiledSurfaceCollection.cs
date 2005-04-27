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
using System.Collections;
using System.Drawing;

namespace SdlDotNet.Sprites
{
	/// <summary>
	/// A basic drawable that takes another one and splits it into one
	/// or more parts. The individual parts are converted into multiple frames.
	/// </summary>
	public class TiledSurfaceCollection : SurfaceCollection, ICollection
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="surfaceTile"></param>
		/// <param name="size"></param>
		public TiledSurfaceCollection(Surface surfaceTile,
			Size size)
		{
			// Save the fields
			this.surfaceTile = surfaceTile;
			this.size = size;
			this.rows = surfaceTile.Height / size.Height;
			this.cols = surfaceTile.Width / size.Width;
			RenderTiles();
		}

		#region Frames
		private void RenderTiles()
		{
			for (int i = 0; i < cols; i++)
			{
				for (int j = 0; j < rows; j++)
				{
					// Adjust the frame to make sure we don't have anything
					// outside of our limits

					// Figure out the row and colum to use
					int sx = (int) (((i + (j * i)) % cols) * size.Width);
					int sy = (int) (((i + (j * i)) / cols) * size.Height);

					// Create the smaller version
					Surface s1 = 
						this.surfaceTile.CreateCompatibleSurface((int) size.Width, (int) size.Height, true);
					Rectangle dRect = new Rectangle(new Point(0, 0), size);
					Rectangle sRect = new Rectangle(new Point(sx, sy), size);
					s1.SetAlpha(Alphas.RleEncoded, 255);
					s1.Blit(this.surfaceTile, dRect, sRect);
					this.Add(s1);
				}
			}
		}
		#endregion

		#region Properties
		private Surface surfaceTile;
		private Size size;
		private int rows;
		private int cols;

		// Provide the explicit interface member for ICollection.
		void ICollection.CopyTo(Array array, int index)
		{
			this.List.CopyTo(array, index);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="array"></param>
		/// <param name="index"></param>
		public override void CopyTo(Surface[] array, int index)
		{
			((ICollection)this).CopyTo(array, index);
		}
   
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
		public Surface SurfaceTile
		{
			get 
			{ 
				return surfaceTile; 
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
		public new Size Size
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
