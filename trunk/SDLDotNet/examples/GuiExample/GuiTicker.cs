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
using System.Collections;
using System.Drawing;

namespace SdlDotNet.Examples.GuiExample
{
	/// <summary>
	/// A rather simple component, the ticker scrolls text left or right
	/// across the screen according to the given rate. It may have an
	/// option to hide itself if there is nothing in the ticker,
	/// otherwise it stays on the screen.
	/// </summary>
	public class GuiTicker : GuiComponent
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="gui"></param>
		/// <param name="x1"></param>
		/// <param name="x2"></param>
		/// <param name="baselineY"></param>
		public GuiTicker(GuiManager gui, int x1, int x2, int baselineY)
			: base(gui)
		{
			// Save our bottom point
			this.x1 = x1;
			this.x2 = x2;
			this.baselineY = baselineY;
			//this.Y = baselineY;
			this.lastSize = new Size(x2 - x1, 1);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="gui"></param>
		/// <param name="x1"></param>
		/// <param name="x2"></param>
		/// <param name="baselineY"></param>
		/// <param name="z"></param>
		public GuiTicker(GuiManager gui, int x1, int x2, int baselineY, int z)
			: base(gui, z)
		{
			// Save our bottom point
			this.x1 = x1;
			this.x2 = x2;
			this.baselineY = baselineY;
			this.lastSize = new Size(x2 - x1, 1);
		}

		#region Sprites
		private Queue queue = new Queue();
		private ArrayList display = new ArrayList();

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sprite"></param>
		public void Add(Sprite sprite)
		{
			if (queue.Count != 0)
			{
				queue.Dequeue();
			}
			queue.Enqueue(sprite);
		}

//		/// <summary>
//		/// 
//		/// </summary>
//		/// <returns></returns>
//		public ArrayList GetSprites()
//		{
//			ArrayList list = new ArrayList(display);
//			list.Add(this);
//			return list;
//		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sprite"></param>
		public void Remove(Sprite sprite)
		{
			// Cannot remove from queue
			display.Remove(sprite);
		}
		#endregion

		#region Geometry
		private int x1 = 0;
		private int x2 = 0;
		private int baselineY = 0;
		private Size lastSize = new Size();

		/// <summary>
		/// 
		/// </summary>
		public override Vector Coordinates
		{
			get
			{
				return new Vector(x1, baselineY - Size.Height, base.Coordinates.Z);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public override Rectangle Rectangle
		{
			get
			{
				return new Rectangle(Coordinates.X, Coordinates.Y, this.Size.Width, this.Size.Height);
			}
			set
			{
				base.Rectangle = value;
			}
		}


		/// <summary>
		/// 
		/// </summary>
		public override Size Size
		{
			get
			{
				// Check for last-size
				if (display.Count == 0)
				{
					return lastSize;
				}

				// Build up a height
				int height = 0;

				foreach (Sprite s in new ArrayList(display))
				{
					if (s.Size.Height > height)
					{
						height = s.Size.Height;
					}
				}
	
				// Return a new height
				lastSize = new Size(x2 - x1,
					height + manager.TickerPadding.Vertical);
				return lastSize;
			}
		}
		#endregion

		#region Events
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		public override void Update(object sender, TickEventArgs args)
		{
			// Don't bother if there is nothing
			if (display.Count == 0 && queue.Count == 0)
			{
				return;
			}

			// Figure out the rates. The min and max start on opposite sides
			// of the ticker.
			int minX = x2;
			int maxX = x1;
			int offset = args.RatePerSecond(Delta);

			if (display.Count != 0)
			{
				// Go through all the displayed sprites
				foreach (Sprite s in new ArrayList(display))
				{
					// Tick the sprite and wrap it in a translator
					s.Update(this, args);
	  
					// Move the sprite along
					s.X += offset;
					s.Y = 0;
//
					// See if the sprite is out of bounds
//					if ((delta < 0 && s.Bounds.Left < x1 - s.Size.Width) || 
//						(delta > 0 && s.Bounds.Right > x2))
//					{
//						Remove(s);
//						continue;
//					}
//	  
//					// Check for the edges
//					if (s.Bounds.Left < minX)
//					{
//						minX = s.Bounds.Left;
//					}
//					if (s.Bounds.Right > maxX)
//					{
//						maxX = s.Bounds.Right;
//					}
				}
			}

			// Add anything into the queue
			if (queue.Count != 0)
			{
				// Check which side to add
				if (delta > 0 && minX > minSpace)
				{
					// We have room on the left
					Sprite ns = (Sprite) queue.Peek();
					ns.Y = manager.TickerPadding.Top + Coordinates.Y;
					ns.X = x1 - ns.Size.Width;
					display.Add(ns);
				}
				else if (delta < 0 && x2 - maxX > minSpace)
				{
					// We have room on the right
					Sprite ns = (Sprite) queue.Peek();
					ns.Y = manager.TickerPadding.Top + Coordinates.Y;
					ns.X = x2;
					ns.Rectangle = new Rectangle(ns.X, ns.Y, ns.Size.Width, ns.Size.Height);
					display.Add(ns);
				}
			}
		}
		#endregion

		#region Properties
		private int delta = -10;
		private int minSpace = 10;
		private bool isAutoHide = false;

		/// <summary>
		/// 
		/// </summary>
		public bool IsAutoHide
		{
			get { return isAutoHide; }
			set { isAutoHide = value; }
		}

		/// <summary>
		/// 
		/// </summary>
		public bool IsHidden
		{
			get { return (isAutoHide && display.Count == 0); }
		}

		/// <summary>
		/// 
		/// </summary>
		public override Surface Surface
		{
			get
			{
				Sprite s = new Sprite(Video.Screen.CreateCompatibleSurface(this.Size)); 
				if (this.queue.Count != 0)
				{
					s = (Sprite)this.queue.Peek();
				}
				//Surface surf = Video.Screen.CreateCompatibleSurface(this.Size);
				//surf.Blit(s.Surface, s.Rectangle);

				return s.Surface;
				//return surf;
			}
			set
			{
				base.Surface = value;
			}
		}


		/// <summary>
		/// Delta is the number of pixels that the ticker should move per
		/// second. This should be independant of actual frame rate.
		/// </summary>
		public int Delta
		{
			get { return delta; }
			set { delta = value; }
		}
		#endregion
	}
}
