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
using SdlDotNet.Drawable;
using SdlDotNet.Utility;
using System;
using System.Drawing;

namespace MfGames.Sdl.Demos
{
	public class AnimatedSprite : DrawableSprite
	{
		// Randomly assign the direction we show the frames
		private bool frameRight = Entropy.Next() % 2 == 0;

		public AnimatedSprite(IDrawable d, Point coords)
			: base(d, Entropy.Next(), coords)
		{
		}

		public AnimatedSprite(IDrawable d, Vector coords)
			: base(d, Entropy.Next(), coords)
		{
		}

		#region Animation and Drawing
		public override bool IsTickable
		{
			get { return Drawable.FrameCount > 1; }
		}

		public override void OnTick(TickArgs args)
		{
			// Increment the frame
			if (frameRight)
				Frame++;
			else if (Frame == 0)
				Frame = FrameCount -1;
			else
				Frame--;
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
