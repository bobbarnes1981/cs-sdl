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

namespace MfGames.Sdl.Gui
{
	/// <summary>
	/// A rather simple component, the ticker scrolls text left or right
	/// across the screen according to the given rate. It may have an
	/// option to hide itself if there is nothing in the ticker,
	/// otherwise it stays on the screen.
	/// </summary>
	public class GuiTicker : GuiComponent
	{
		public GuiTicker(GuiManager gui, int x1, int x2, int baselineY)
			: base(gui)
		{
			// Save our bottom point
			this.x1 = x1;
			this.x2 = x2;
			this.baselineY = baselineY;
			this.lastSize = new Size(x2 - x1, 1);
		}

		public GuiTicker(GuiManager gui, int x1, int x2, int baselineY, int z)
			: base(gui, z)
		{
			// Save our bottom point
			this.x1 = x1;
			this.x2 = x2;
			this.baselineY = baselineY;
			this.lastSize = new Size(x2 - x1, 1);
		}

		#region Drawing
		public override void Render(RenderArgs args)
		{
			// Don't bother if we are hidden
			if (IsHidden)
				return;

			// Draw ourselves, then our components
			manager.Render(args, this);

			// Draw our sprites
			RenderArgs args1 = args.Clone();
			args1.TranslateX += Coordinates.X;
			args1.TranslateY += Coordinates.Y + manager.TickerPadding.Top;

			foreach (Sprite s in new ArrayList(display))
			{
				s.Render(args1);
			}
		}
		#endregion

		#region Sprites
		private Queue queue = new Queue();
		private ArrayList display = new ArrayList();

		public void Add(Sprite sprite)
		{
			queue.Enqueue(sprite);
		}

		public ArrayList GetSprites()
		{
			ArrayList list = new ArrayList(display);
			list.Add(this);
			return list;
		}

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

		public override Vector Coordinates
		{
			get
			{
				return new Vector(x1, baselineY - Size.Height, base.Coordinates.Z);
			}
		}

		public override Size Size
		{
			get
			{
				// Check for last-size
				if (display.Count == 0)
					return lastSize;

				// Build up a height
				int height = 0;

				foreach (Sprite s in new ArrayList(display))
				{
					if (s.Size.Height > height)
						height = s.Size.Height;
				}
	
				// Return a new height
				lastSize = new Size(x2 - x1,
					height + manager.TickerPadding.Vertical);
				return lastSize;
			}
		}
		#endregion

		#region Events
		public override void OnTick(object sender, TickEventArgs args)
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
					s.OnTick(this, args);
	  
					// Move the sprite along
					s.Coordinates.X += offset;
					s.Coordinates.Y = 0;

					// See if the sprite is out of bounds
					if ((delta < 0 && s.Bounds.Left < x1 - s.Size.Width) || 
						(delta > 0 && s.Bounds.Right > x2))
					{
						Remove(s);
						continue;
					}
	  
					// Check for the edges
					if (s.Bounds.Left < minX)
					{
						minX = s.Bounds.Left;
					}
					if (s.Bounds.Right > maxX)
					{
						maxX = s.Bounds.Right;
					}
				}
			}

			// Add anything into the queue
			if (queue.Count != 0)
			{
				// Check which side to add
				if (delta > 0 && minX > minSpace)
				{
					// We have room on the left
					Sprite ns = (Sprite) queue.Dequeue();
					ns.Coordinates.Y = manager.TickerPadding.Top + Coordinates.Y;
					ns.Coordinates.X = x1 - ns.Size.Width;
					display.Add(ns);
				}
				else if (delta < 0 && x2 - maxX > minSpace)
				{
					// We have room on the right
					Sprite ns = (Sprite) queue.Dequeue();
					ns.Coordinates.Y = manager.TickerPadding.Top + Coordinates.Y;
					ns.Coordinates.X = x2;
					display.Add(ns);
				}
			}
		}
		#endregion

		#region Properties
		private int delta = -10;
		private int minSpace = 10;
		private bool isAutoHide = false;

		public bool IsAutoHide
		{
			get { return isAutoHide; }
			set { isAutoHide = value; }
		}

		public override bool IsHidden
		{
			get { return (isAutoHide && display.Count == 0); }
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
