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
using System.Drawing;

namespace SdlDotNet.Sprites
{
	/// <summary>
	/// 
	/// </summary>
	public class DrawableSprite : Sprite
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="surfaces"></param>
		public DrawableSprite(SurfaceCollection surfaces)
			: base(surfaces[0])
		{
			this.surfaces.Add(surfaces);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="surfaces"></param>
		/// <param name="frame"></param>
		public DrawableSprite(SurfaceCollection surfaces, int frame)
			: base(surfaces[0])
		{
			this.surfaces.Add(surfaces);
			this.frame = frame;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="surfaces"></param>
		/// <param name="position"></param>
		public DrawableSprite(SurfaceCollection surfaces, Point position)
			: base(surfaces[0], position)
		{
			this.surfaces.Add(surfaces);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="surfaces"></param>
		/// <param name="coordinates"></param>
		public DrawableSprite(SurfaceCollection surfaces, Vector coordinates)
			: base(surfaces[0], coordinates)
		{
			this.surfaces.Add(surfaces);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="surfaces"></param>
		/// <param name="frame"></param>
		/// <param name="position"></param>
		public DrawableSprite(SurfaceCollection surfaces, int frame, Point position)
			: base(surfaces[0], position)
		{
			this.surfaces = surfaces;
			this.frame = frame;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="surfaces"></param>
		/// <param name="frame"></param>
		/// <param name="coordinates"></param>
		public DrawableSprite(SurfaceCollection surfaces, int frame, Vector coordinates)
			: base(surfaces[0], coordinates)
		{
			this.surfaces = surfaces;
			this.frame = frame;
		}

		#region Drawable
		private SurfaceCollection surfaces = new SurfaceCollection();

		private int frame;

		/// <summary>
		/// Returns the current frame. For almost all drawables, this is
		/// the same as the "[0]" accessor. For sprites and other animated
		/// drawables, this returns whatever frame is consider "current".
		/// </summary>
		public override Surface Surface
		{
			get
			{
				if (surfaces == null || surfaces[frame] == null)
				{
					throw new DrawableException("No drawable to return");
				}

				return surfaces[frame];
			}
			set
			{
				this.surfaces.Insert(0, value);
			}
		}

		/// <summary>
		/// Retrieves the drawable associated with this sprite. This will
		/// never return null (it will throw an exception if there is no
		/// drawable).
		/// </summary>
		public SurfaceCollection Surfaces
		{
			get
			{
				if (surfaces == null)
				{
					throw new DrawableException("No surface to return");
				}
				return surfaces;
			}
		}

		/// <summary>
		/// Frame is the current frame number (drawable image) for the
		/// current sprite.
		/// </summary>
		public int Frame
		{
			get 
			{ 
				return frame; 
			}
			set 
			{ 
				frame = value; 
			}
		}

		/// <summary>
		/// Contains a read-only count of the number of frames in the
		/// sprite.
		/// </summary>
		public int FrameCount
		{
			get
			{
				if (surfaces == null)
				{
					return 0;
				}
				else
				{
					return surfaces.Count;
				}
			}
		}
		#endregion

		#region Properties
		/// <summary>
		/// 
		/// </summary>
		public override Size Size
		{
			get
			{
				if (surfaces == null)
				{
					throw new DrawableException("No size for this drawable");
				}
				else
				{
					return surfaces.Size;
				}
			}
		}
		#endregion
	}
}
