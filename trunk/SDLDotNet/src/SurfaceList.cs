/*
 * $RCSfile$
 * Copyright (C) 2004 David Hudson (jendave@yahoo.com)
 * Copyright (C) 2003 Klavs Martens
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

using System;
using System.Drawing;
using System.Collections;
using SdlDotNet;
using Tao.Sdl;

namespace SdlDotNet
{
	/// <summary>
	/// Provides methods to manage a collection of SdlImage objects. 
	/// </summary>
	public class SurfaceList
	{
		/// <summary>
		/// Private field that holds ImageList.ImageCollection for this image list.
		/// </summary>
		private SurfaceCollection surfaces;

		/// <summary>
		/// Create a new instance of a SdlImageList
		/// </summary>
		public SurfaceList() 
		{
			surfaces = new SurfaceCollection();
		}

		/// <summary>
		/// Gets the ImageList.ImageCollection for this image list.
		/// </summary>
		public SurfaceCollection Surfaces 
		{
			get 
			{
				return surfaces;
			}
		}
	}
}
