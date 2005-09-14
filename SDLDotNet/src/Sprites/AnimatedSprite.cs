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

using System;
using System.Collections;
using System.Drawing;

using SdlDotNet;
using SdlDotNet.Sprites;

namespace SdlDotNet.Sprites
{
	/// <summary>
	/// 
	/// </summary>
	public class AnimatedSprite : Sprite
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="surfaces"></param>
		public AnimatedSprite(SurfaceCollection surfaces)
			: base(surfaces[0])
		{
			this.frameCollections["default"] = surfaces;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="frameCollections"></param>
		public AnimatedSprite(Hashtable frameCollections)
			: base(((SurfaceCollection)frameCollections[0])[0])
		{
			this.frameCollections = frameCollections;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="frameCollections"></param>
		public AnimatedSprite(Hashtable frameCollections, string frameKey)
			: base(((SurfaceCollection)frameCollections[0])[0])
		{
			this.frameCollections = frameCollections;
			this.key = frameKey;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="surfaces"></param>
		/// <param name="frame"></param>
		public AnimatedSprite(SurfaceCollection surfaces, int frame)
			: base(surfaces[0])
		{
			this.frameCollections["default"] = surfaces;
			this.frame = frame;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="surfaces"></param>
		/// <param name="position"></param>
		public AnimatedSprite(SurfaceCollection surfaces, Point position)
			: base(surfaces[0], position)
		{
			this.frameCollections["default"] = surfaces;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="surfaces"></param>
		/// <param name="coordinates"></param>
		public AnimatedSprite(SurfaceCollection surfaces, Vector coordinates)
			: base(surfaces[0], coordinates)
		{
			this.frameCollections["default"] = surfaces;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="surfaces"></param>
		/// <param name="frame"></param>
		/// <param name="position"></param>
		public AnimatedSprite(SurfaceCollection surfaces, int frame, Point position)
			: base(surfaces[0], position)
		{
			this.frameCollections["default"] = surfaces;
			this.frame = frame;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="surfaces"></param>
		/// <param name="frame"></param>
		/// <param name="coordinates"></param>
		public AnimatedSprite(SurfaceCollection surfaces, int frame, Vector coordinates)
			: base(surfaces[0], coordinates)
		{
			//this.surfaces = surfaces;
			this.frameCollections["default"] = surfaces;
			this.frame = frame;
		}

		#region Drawable
		private SurfaceCollection surfaces = new SurfaceCollection();

		private int frame;

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

		Hashtable frameCollections = new Hashtable();
		string key = "default";

		/// <summary>
		/// 
		/// </summary>
		public string FrameCollectionKey
		{
			get
			{
				return key;
			}
			set
			{
				key = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public Hashtable FrameCollections
		{
			get
			{
				return frameCollections;
			}
			set
			{
				frameCollections = value;
			}
		}

		/// <summary>
		/// Retrieves the current frame's surface.
		/// </summary>
		public override Surface Render()
		{
			return ((SurfaceCollection)this.frameCollections[key])[frame];
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
					return ((SurfaceCollection)this.frameCollections[key]).Count;
				}
			}
		}
		#endregion

		#region Properties
		bool loop = true;

		/// <summary>
		/// If true, the animation will loop.
		/// </summary>
		public bool Loop
		{
			get
			{
				return loop;
			}
			set
			{
				loop = value;
			}
		}

		bool animate = true;

		/// <summary>
		/// If true, sprite will be animated
		/// </summary>
		public bool Animate
		{
			get
			{
				return animate;
			}
			set
			{
				animate = value;
			}
		}
		#endregion

		private bool disposed;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="disposing"></param>
		protected override void Dispose(bool disposing)
		{
			if (!disposed)
			{
				try
				{
					if (disposing)
					{
						foreach (Surface s in this.surfaces)
						{
							s.Dispose();
						}
					}
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
