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
using SdlDotNet;
using System.Drawing;

namespace SdlDotNet.Sprites
{
	/// <summary>
	/// 
	/// </summary>
	public class MouseArgs
	{
		/// <summary>
		/// 
		/// </summary>
		public MouseArgs()
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public MouseArgs Clone()
		{
			// Create a new one
			MouseArgs args = new MouseArgs();
			args.x = x;
			args.y = y;
			args.tx = tx;
			args.ty = ty;
			args.rx = rx;
			args.ry = ry;
			args.b1 = b1;
			return args;
		}

		#region Properties
		private bool b1 = false;
		private int tx = 0;
		private int ty = 0;
		private int rx = 0;
		private int ry = 0;
		private int x = 0;
		private int y = 0;

		/// <summary>
		/// 
		/// </summary>
		public bool IsButton1
		{
			get { return b1; }
			set { b1 = value; }
		}

		/// <summary>
		/// 
		/// </summary>
		//public Vector2 Coords
		public Point Coords
		{
//			get { return new Vector2(X, Y); }
			get { return new Point(X, Y); }
		}

		/// <summary>
		/// 
		/// </summary>
		public int X
		{
			get { return x + tx; }
		}

		/// <summary>
		/// 
		/// </summary>
		public int Y
		{
			get { return y + ty; }
		}

		/// <summary>
		/// 
		/// </summary>
		public int BaseX
		{
			get { return x; }
			set { x = value; }
		}

		/// <summary>
		/// 
		/// </summary>
		public int BaseY
		{
			get { return y; }
			set { y = value; }
		}

		/// <summary>
		/// 
		/// </summary>
		public int TranslateX
		{
			get { return tx; }
			set { tx = value; }
		}

		/// <summary>
		/// 
		/// </summary>
		public int TranslateY
		{
			get { return ty; }
			set { ty = value; }
		}

		/// <summary>
		/// 
		/// </summary>
		public int RelativeX
		{
			get { return rx; }
			set { rx = value; }
		}

		/// <summary>
		/// 
		/// </summary>
		public int RelativeY
		{
			get { return ry; }
			set { ry = value; }
		}
		#endregion
	}
}
