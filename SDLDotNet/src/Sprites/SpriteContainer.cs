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
using System;
using System.Collections;
using System.Drawing;
using System.Globalization;

namespace SdlDotNet.Sprites
{
	/// <summary>
	/// The SpriteCollection is a special case of sprite. It is used to
	/// group other sprites into an easily managed whole. The sprite
	/// manager has no size.
	/// </summary>
	public class SpriteContainer : Sprite
	{
		/// <summary>
		/// 
		/// </summary>
		public SpriteContainer() : base()
		{
		}

//		/// <summary>
//		/// 
//		/// </summary>
//		/// <param name="coordinates"></param>
//		public SpriteContainer(Vector coordinates) : base(coordinates)
//		{
//		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="coordinates"></param>
		/// <param name="surface"></param>
		public SpriteContainer(Surface surface, Vector coordinates) : base(surface, coordinates)
		{
		}

//		/// <summary>
//		/// 
//		/// </summary>
//		/// <param name="rectangle"></param>
//		public SpriteContainer(Rectangle rectangle) : base(rectangle)
//		{
//		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="surface"></param>
		public SpriteContainer(Surface surface) : base(surface)
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="surface"></param>
		/// <param name="rectangle"></param>
		public SpriteContainer(Surface surface, Rectangle rectangle) : base(surface, rectangle)
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="surface"></param>
		/// <param name="rectangle"></param>
		/// <param name="z"></param>
		public SpriteContainer(Surface surface, Rectangle rectangle, int z) : base(surface, rectangle, z)
		{
		}

		private SpriteCollection sprites = new SpriteCollection();
		/// <summary>
		/// 
		/// </summary>
		public SpriteCollection Sprites
		{
			get
			{
				return sprites;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override Surface Render()
		{
			this.sprites.Draw(this.Surface);
			return this.Surface;
		}
	}
}