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
	/// One of the core drawables, the ImageDrawable is used to load an
	/// image into memory and use it as a drawable. Unlike most
	/// drawables, the same image data is returned with all requests, so
	/// care must be taken not to change the image data.
	/// </summary>
	public class ImageDrawable : IDrawable
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="imageFile"></param>
		public ImageDrawable(string imageFile)
		{
			// Save the fields
			this.filename = imageFile;
      
			// Load the image
			image = new Surface(imageFile);
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
				return image; 
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
				return 1; 
			} 
		}

		/// <summary>
		/// Returns a cache key which represents a unique identifier of
		/// the drawable. This is used by CachedDrawable to keep the CPU
		/// generation low.
		/// </summary>
		public string GetCacheKey(int frame) 
		{ 
			return filename; 
		}
		#endregion

		#region Properties
		private string filename = null;

		private Surface image = null;

		/// <summary>
		/// Returns the height of the drawable, in pixels.
		/// </summary>
		public Size Size
		{
			get 
			{ 
				return new Size(image.Width, image.Height); 
			}
		}
		#endregion
	}
}
