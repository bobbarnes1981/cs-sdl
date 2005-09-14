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
using System.Globalization;

namespace SdlDotNet.Examples
{
	/// <summary>
	/// 
	/// </summary>
	public class DragSprite : BoundedSprite
	{
		private SurfaceCollection d1;
		private SurfaceCollection d2;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="d1"></param>
		/// <param name="d2"></param>
		/// <param name="coordinates"></param>
		/// <param name="bounds"></param>
		public DragSprite(SurfaceCollection d1, SurfaceCollection d2, Point coordinates,
			Rectangle bounds)
			: base(d1, bounds, new Vector(coordinates))
		{
			this.d1 = d1;
			this.d2 = d2;
			this.Size = d1.Size;
			this.AllowDrag = true;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return String.Format(CultureInfo.CurrentCulture, "(drag {0} {1})", this.BeingDragged, base.ToString());
		}

		#region Events
		/// <summary>
		/// 
		/// </summary>
		/// <param name="args"></param>
		public override void Update(MouseButtonEventArgs args)
		{
			if (this.IntersectsWith(new Point(args.X, args.Y)))
			{
				// If we are being held down, pick up the marble
				// Change the Z-order
				if (args.ButtonPressed)
				{
					this.Z += 100;
					this.BeingDragged = true;
					this.Surfaces.Clear();
					this.Surfaces.Add(d2);		
				}
				else
				{
					this.Z -= 100;
					this.BeingDragged = false;
					this.Surfaces.Clear();
					this.Surfaces.Add(d1);	
				}
			}
		}

		/// <summary>
		/// If the sprite is picked up, this moved the sprite to follow
		/// the mouse.
		/// </summary>
		public override void Update(MouseMotionEventArgs args)
		{
			if (!AllowDrag)
			{
				return;
			}

			// Move the window as appropriate
			if (this.BeingDragged)
			{
				this.X += args.RelativeX;
				this.Y += args.RelativeY;
			}
		}
		#endregion

		/// <summary>
		/// 
		/// </summary>
		public bool IsMouseMotionLocked
		{
			get
			{
				return this.BeingDragged;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public bool IsMouseButtonLocked
		{
			get
			{
				return true;
			}
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
