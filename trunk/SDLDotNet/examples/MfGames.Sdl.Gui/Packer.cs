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

using SdlDotNet.Utility;
using SdlDotNet.Sprites;
using SdlDotNet;
using System.Collections;
using System.Drawing;

namespace MfGames.Sdl.Gui
{
	/// <summary>
	/// This class handles one or more sprites packed into a line. The
	/// sprites may be added to the beginning or the end of the line, as
	/// the program desirs.
	/// </summary>
	public class Packer : GuiComponent
	{
		#region Constructors
		public Packer(GuiManager manager)
			: base(manager)
		{
		}

		//public Packer(GuiManager manager, Vector2 p)
		public Packer(GuiManager manager, Point p)
			: base(manager, p)
		{
		}

		public Packer(GuiManager manager, Vector p)
			: base(manager, p)
		{
		}
		#endregion

		#region Sprites
		protected Size GetSize(Sprite s)
		{
			// Get the size
			Size d = s.Size;

			if (s is GuiComponent)
				d = ((GuiComponent) s).OuterSize;

			return d;
		}

		//protected Sprite SelectSprite(Vector2 point, ref int index)
		protected Sprite SelectSprite(Point point, ref int index)
		{
			index = 0;

			foreach (Sprite s in new ArrayList(Sprites))
			{
				if (s.IntersectsWith(point))
					return s;

				index++;
			}

			return null;
		}
		#endregion

		#region Events
		public override bool OnMouseButton(object sender, MouseArgs args)
		{
			// Create a mapping
			MouseArgs args1 = args.Clone();
			args1.TranslateX -= Coords.X + MarginPadding.Left + InnerPadding.Left;
			args1.TranslateY -= Coords.Y + MarginPadding.Top + InnerPadding.Top;
			MouseArgs args2 = args.Clone();
			args2.TranslateX += Coords.X + MarginPadding.Left + InnerPadding.Left;
			args2.TranslateY += Coords.Y + MarginPadding.Top + InnerPadding.Top;

			// We assume that the coordinates are set by the packing
			// processing, so we can use them with the offset and
			// coordinates.
			//Vector2 p = args1.Coords;
			Point p = args1.Coords;

			foreach (Sprite s in Sprites)
			{
				// Check the region. If it contains the point, we call the
				// basic sprite (and not its parent).
				if (s.IntersectsWith(p))
				{
					if (s.OnMouseButton(this, args2))
						return true;
				}
			}

			// We didn't finish it
			return false;
		}

		public override bool OnMouseMotion(object sender, MouseArgs args)
		{
			// Build up the new point
			int x = args.X;
			int y = args.Y;
			int relx = args.RelativeX;
			int rely = args.RelativeY;
      
			MouseArgs args1 = args.Clone();
			args1.TranslateX += Coords.X + MarginPadding.Left + InnerPadding.Left;
			args1.TranslateY += Coords.Y + MarginPadding.Top + InnerPadding.Top;
	 
			// We assume that the coordinates are set by the packing
			// processing, so we can use them with the offset and
			// coordinates.
			//Vector2 p = args1.Coords;
			Point p = args1.Coords;

			foreach (Sprite s in Sprites)
			{
				// Check the region. If it contains the point, we call the
				// basic sprite (and not its parent).
				if (s.IntersectsWith(p))
				{
					if (s.OnMouseMotion(this, args1))
						return true;
				}
			}

			// We didn't finish it
			return false;
		}

		public override void OnTick(TickArgs args)
		{
			base.OnTick(args);

			foreach (Sprite s in Sprites)
				if (s.IsTickable)
					s.OnTick(args);
		}
		#endregion

		#region Properties
		private ArrayList head = new ArrayList();
		private ArrayList tail = new ArrayList();

		protected void AddHead(Sprite s)
		{
			head.Add(s);
		}

		protected void AddTail(Sprite s)
		{
			tail.Add(s);
		}

		public virtual ArrayList HeadSprites
		{
			get { return head; }
		}
    
		public virtual ArrayList TailSprites
		{
			get { return tail; }
		}

		public virtual ArrayList Sprites
		{
			get
			{
				ArrayList list = new ArrayList(head);
				list.AddRange(tail);
				return list;
			}
		}

		public virtual Padding MarginPadding
		{
			get { return new Padding(0); }
		}

		public virtual Padding InnerPadding
		{
			get { return new Padding(0); }
		}
		#endregion
	}
}
