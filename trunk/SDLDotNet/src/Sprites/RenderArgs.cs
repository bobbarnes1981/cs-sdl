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
using System;
using System.Drawing;

namespace SdlDotNet.Sprites
{
	/// <summary>
	/// 
	/// </summary>
	public class RenderArgs
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="surface"></param>
		/// <param name="size"></param>
		public RenderArgs(Surface surface, Size size)
		{
			this.Surface = surface;
			this.Size = size;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public RenderArgs Clone()
		{
			// Create a new one
			RenderArgs args = new RenderArgs(Surface, Size);
			args.tx = tx;
			args.ty = ty;
			return args;
		}

		#region Clipping
		/// <summary>
		/// 
		/// </summary>
		public void ClearClipping()
		{
				Surface.ClipRectangle  = new Rectangle(0, 0,
					Surface.Width, Surface.Height);
		}

		/// <summary>
		/// 
		/// </summary>
		public Rectangle Clipping
		{
			get
			{
				return Surface.ClipRectangle;
			}
			set
			{
				Surface.ClipRectangle = value;
			}
		}
		#endregion

		#region Geometry
		/// <summary>
		/// 
		/// </summary>
		/// <param name="rectangle"></param>
		/// <returns></returns>
		public Rectangle Translate(Rectangle rectangle)
		{
			int transX;
			int transY;

			Rectangle r = rectangle;
			transX = r.Location.X + TranslateX;
			transY = r.Location.Y + TranslateY;
			r.Location = new Point(transX, transY);
			return r;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="s"></param>
		public void TranslateBy(Sprite s)
		{
			TranslateX += s.Coordinates.X;
			TranslateY += s.Coordinates.Y;
		}
		#endregion

		#region Properties
		private Surface surface = null;
		private int tx = 0;
		private int ty = 0;
		private Size size;

		/// <summary>
		/// 
		/// </summary>
		public Point Point
		{
			get 
			{ 
				return new Point(tx, ty); 
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public Surface Surface
		{
			get 
			{ 
				return surface; 
			}
			set
			{
				if (value == null)
				{
					throw new SpriteException("Cannot assign a null surface");
				}

				surface = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public Size Size
		{
			get 
			{ 
				return size; 
			}
			set
			{
				if (value.IsEmpty)
				{
					//Console.WriteLine("Cannot assign a null size!");
				}
				else
				{
					size = value;
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public int TranslateX
		{
			get 
			{ 
				return tx; 
			}
			set 
			{ 
				tx = value; 
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public int TranslateY
		{
			get 
			{ 
				return ty; 
			}
			set 
			{ 
				ty = value; 
			}
		}
		#endregion
	}
}
