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

namespace MfGames.Sdl.Demos
{
	public class AnimatedSprite : DrawableSprite
	{
		Random rand = new Random();
		// Randomly assign the direction we show the frames
		private bool frameRight;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="d"></param>
		/// <param name="coordinates"></param>
		public AnimatedSprite(IDrawable d, Point coordinates)
			: base(d, coordinates)
		{
			this.Frame = rand.Next();
			this.frameRight = (rand.Next()  % 2 == 0);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="d"></param>
		/// <param name="coordinates"></param>
		public AnimatedSprite(IDrawable d, Vector coordinates)
			: base(d, coordinates)
		{
			this.Frame = rand.Next();
		}

		#region Animation and Drawing
		/// <summary>
		/// 
		/// </summary>
		public override bool IsTickable
		{
			get { return Drawable.FrameCount > 1; }
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="args"></param>
		public override void OnTick(object sender, TickEventArgs args)
		{
			// Increment the frame
			if (frameRight)
			{
				Frame++;
			}
			else if (Frame == 0)
			{
				Frame = FrameCount -1;
			}
			else
			{
				Frame--;
			}
		}
		#endregion

		#region Operators
		public override string ToString()
		{
			return String.Format("(animated {0})", base.ToString());
		}
		#endregion
	}
}
