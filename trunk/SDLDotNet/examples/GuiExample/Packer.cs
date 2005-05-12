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
using System.Collections;
using System.Drawing;

namespace SdlDotNet.Examples.GuiExample
{
	/// <summary>
	/// This class handles one or more sprites packed into a line. The
	/// sprites may be added to the beginning or the end of the line, as
	/// the program desires.
	/// </summary>
	public class Packer : GuiComponent
	{
		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		/// <param name="manager"></param>
		public Packer(GuiManager manager)
			: base(manager)
		{
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="gui"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="height"></param>
		public Packer(GuiManager gui, int x, int y, int height)
			: base(gui, new Rectangle(x, y, Video.Screen.Width, height))
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="manager"></param>
		/// <param name="p"></param>
		public Packer(GuiManager manager, Point p)
			: base(manager, p)
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="manager"></param>
		/// <param name="rectangle"></param>
		public Packer(GuiManager manager, Rectangle rectangle)
			: base(manager, rectangle)
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="manager"></param>
		/// <param name="p"></param>
		public Packer(GuiManager manager, Vector p)
			: base(manager, p)
		{
		}
		#endregion

		#region Sprites
		/// <summary>
		/// 
		/// </summary>
		/// <param name="point"></param>
		/// <param name="index"></param>
		/// <returns></returns>
		protected Sprite SelectSprite(Point point, ref int index)
		{
			index = 0;

			foreach (Sprite s in Sprites)
			{
				if (s.IntersectsWith(point))
				{
					return s;
				}

				index++;
			}

			return null;
		}
		#endregion

		#region Events
		#endregion

		#region Properties
		private SpriteCollection head = new SpriteCollection();
		private SpriteCollection tail = new SpriteCollection();

		/// <summary>
		/// 
		/// </summary>
		/// <param name="s"></param>
		protected void AddHead(Sprite s)
		{
			head.Add(s);
			this.Sprites.Add(s);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="s"></param>
		protected void AddTail(Sprite s)
		{
			tail.Add(s);
			this.Sprites.Add(s);
		}

		/// <summary>
		/// 
		/// </summary>
		public SpriteCollection HeadSprites
		{
			get 
			{ 
				return head; 
			}
		}
    
		/// <summary>
		/// 
		/// </summary>
		public SpriteCollection TailSprites
		{
			get 
			{ 
				return tail; 
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public virtual Padding MarginPadding
		{
			get 
			{ 
				return new Padding(0); 
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public virtual Padding InnerPadding
		{
			get 
			{ 
				return new Padding(0); 
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
			if (!this.disposed)
			{
				try
				{
					if (disposing)
					{
						this.Surface.Dispose();
						foreach (Sprite s in this.Sprites)
						{
							IDisposable disposableObj = s as IDisposable;
							if (disposableObj != null)
							{
								disposableObj.Dispose( );
							}
						}
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
