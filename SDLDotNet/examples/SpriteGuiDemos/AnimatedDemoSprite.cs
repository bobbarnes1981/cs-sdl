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
using System;
using System.Drawing;
using System.Globalization;

namespace SdlDotNet.Examples
{
	/// <summary>
	/// 
	/// </summary>
	public class AnimatedDemoSprite : AnimatedSprite
	{
		static Random rand = new Random();

		/// <summary>
		/// 
		/// </summary>
		/// <param name="surfaces"></param>
		/// <param name="position"></param>
		public AnimatedDemoSprite(SurfaceCollection surfaces, Point position)
			: base(surfaces, position)
		{
			if (surfaces == null)
			{
				throw new ArgumentNullException("surfaces");
			}
			base.Frame = rand.Next(surfaces.Count);
			if (rand.Next(2) % 2 == 0)
			{
				this.AnimateForward = true;
			}
			else
			{
				this.AnimateForward = false;
			}

			this.Animate = true;
		}

//		/// <summary>
//		/// 
//		/// </summary>
//		/// <param name="d"></param>
//		/// <param name="coordinates"></param>
//		public AnimatedDemoSprite(SurfaceCollection d, Point coordinates)
//			: base(d, coordinates)
//		{
//			base.Frame = rand.Next(d.Count);
//			this.frameRight = (rand.Next(2) % 2 == 0);
//		}

		#region Animation and Drawing
		/// <summary>
		/// 
		/// </summary>
		/// <param name="args"></param>
		public override void Update(TickEventArgs args)
		{
//			// Increment the frame
//			if (frameRight)
//			{
//				Frame++;
//			}
//			else if (Frame == 0)
//			{
//				//Frame = FrameCount -1;
//			}
//			else
//			{
//				Frame--;
//			}
		}
		#endregion

		#region Operators
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return String.Format(CultureInfo.CurrentCulture, "(animated {0})", base.ToString());
		}
		#endregion

		private bool disposed;
		/// <summary>
		/// Destroys the surface object and frees its memory
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
					}
				}
				finally
				{
					base.Dispose(disposing);
					this.disposed = true;
				}
			}
			base.Dispose(disposing);
		}
	}
}