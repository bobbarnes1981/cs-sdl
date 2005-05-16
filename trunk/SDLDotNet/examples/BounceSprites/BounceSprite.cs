/*
 * $RCSfile$
 * Copyright (C) 2005 David Hudson (jendave@yahoo.com)
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
using System;
using System.Threading;

namespace SdlDotNet.Examples
{
	/// <summary>
	/// 
	/// </summary>
	public class BounceSprite : AnimatedSprite
	{
		#region Fields
		private int dx = 10;
		private int dy = 10;
		private Rectangle bounds = new Rectangle();
		#endregion Fields

		#region Constructor
		/// <summary>
		/// 
		/// </summary>
		/// <param name="d"></param>
		/// <param name="coordinates"></param>
		public BounceSprite(SurfaceCollection d, Vector coordinates)
			: base(d, coordinates)
		{
			this.bounds = 
				new Rectangle(0, 0, Video.Screen.Rectangle.Width - 
				(int) d.Size.Width, Video.Screen.Rectangle.Height - 
				(int) d.Size.Height);
			this.AllowDrag = true;
		}
		#endregion Constructor

		#region Event Update Methods
		/// <summary>
		/// 
		/// </summary>
		/// <param name="args"></param>
		public override void Update(TickEventArgs args)
		{
			base.Update(args);

			// Increment the frame
			if (Frame == 0)
			{
				Frame = FrameCount -1;
			}
			else
			{
				Frame--;
			}
			this.Surface = this.Surfaces[Frame];

			if (!this.BeingDragged)
			{
				this.X += dx;
				this.Y += dy;

				// Bounce off the left
				if (this.X < bounds.Left)
				{
					this.X = bounds.Left;
				}

				// Bounce off the top
				if (this.Y < bounds.Top)
				{
					this.Y = bounds.Top;
				}

				// Bounce off the bottom
				if (this.Y > bounds.Bottom)
				{
					this.Y = bounds.Bottom;
				}
				// Bounce off the right
				if (this.X > bounds.Right)
				{
					this.X = bounds.Right;
				}

				// Normalize the directions
				if (this.X == bounds.Left)
				{
					dx = (Math.Abs(this.dx));
				}

				if (this.X == bounds.Right)
				{
					dx = -1*(Math.Abs(this.dx));
				}

				if (this.Y == bounds.Top)
				{
					dy = (Math.Abs(this.dy));
				}

				if (this.Y == bounds.Bottom)
				{
					dy = -1*(Math.Abs(this.dy));
				}
			}
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="args"></param>
		public override void Update(MouseButtonEventArgs args)
		{
			if (this.IntersectsWith(new Point(args.X, args.Y)))
			{
				// If we are being held down, pick up the marble
				if (args.ButtonPressed)
				{
					this.BeingDragged = true;
				}
				else
				{
					this.BeingDragged = false;
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
		#endregion Event Update Methods

		#region IDisposable
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
		#endregion IDisposable
	}
}
