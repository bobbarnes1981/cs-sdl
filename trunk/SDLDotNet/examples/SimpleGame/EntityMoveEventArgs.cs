// Copyright 2005 David Hudson (jendave@yahoo.com)
// This file is part of Iron Crown.
//
// Iron Crown is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
//
// Iron Crown is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with Iron Crown; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA

using System;

namespace SdlDotNet.Examples
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	public class EntityMoveEventArgs : EventArgs
	{
		Entity entity;

		/// <summary>
		/// 
		/// </summary>
		public EntityMoveEventArgs(Entity entity)
		{
			this.entity = entity;
		}

		/// <summary>
		/// 
		/// </summary>
		public Entity Entity
		{
			get
			{
				return this.entity;
			}
		}
	}
}
