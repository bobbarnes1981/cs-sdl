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
	public class SpriteSingleCollection : SpriteCollection, ICollection
	{
		/// <summary>
		/// 
		/// </summary>
		public SpriteSingleCollection() : base()
		{
			this.InnerList.Capacity = 1;
		}

		#region Display
		#endregion

		#region Sprites
		/// <summary>
		/// Adds sprite to group
		/// </summary>
		/// <param name="sprite">Sprite to add</param>
		public override int Add(Sprite sprite)
		{
			this.Clear();
			return List.Add(sprite);
		}
		#endregion

		#region Properties
//		private IViewport viewport = null;
		#endregion

		// Provide the explicit interface member for ICollection.
		void ICollection.CopyTo(Array array, int index)
		{
			this.List.CopyTo(array, index);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="array"></param>
		/// <param name="index"></param>
		public override void CopyTo(Sprite[] array, int index)
		{
			((ICollection)this).CopyTo(array, index);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="index"></param>
		/// <param name="surface"></param>
		public override void Insert(int index, Surface surface)
		{
			List.Insert(index, surface);
		} 

		/// <summary>
		/// 
		/// </summary>
		/// <param name="surface"></param>
		/// <returns></returns>
		public override int IndexOf(Surface surface)
		{
			return List.IndexOf(surface);
		} 
	}
}
