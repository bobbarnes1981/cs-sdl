/*
 * $RCSfile$
 * Copyright (C) 2004, 2005 David Hudson (jendave@yahoo.com)
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
	/// Encapsulates the collection of SdlImage objects in an SdlImageList.
	/// </summary>
	public class SurfaceCollection : CollectionBase, ICollection
	{
		private ArrayList data;

		/// <summary>
		/// 
		/// </summary>
		public SurfaceCollection()
		{
			data = new ArrayList();
		}

		/// <summary>
		/// Indexer for the Items in the Collection
		/// </summary>
		public Surface this[int index] 
		{
			get 
			{ 
				return (Surface)List[index];	
			}
		}

		/// <summary>
		/// Adds the specified SdlImage to the end of the SdlImageList.
		/// </summary>
		/// <param name="surface">
		/// The SdlImage to be added to the end of the SdlImageList.
		/// </param>
		/// <returns>
		/// The index at which the SdlImage has been added.
		/// </returns>
		public int Add(Surface surface)
		{
			return List.Add(surface);
		} 

		/// <summary>
		/// Load a SdlImage with the specified filename and add 
		/// it to the end of the SdlImageList.
		/// </summary>
		/// <param name="filename">The filename of the SdlImage 
		/// to be added to the end of the SdlImageList.</param>
		/// <returns>
		/// The index at which the SdlImage has been added.
		/// </returns>
		public int Add(string filename)
		{
			return List.Add(new Surface(filename));
		} 


		/// <summary>
		/// Create a SdlImage from a byte array and add it to the end 
		/// of the SdlImageList.
		/// </summary>
		/// <param name="array">
		/// The array of byte to create the Image 
		/// from.
		/// </param>
		/// <returns>
		/// The index at which the SdlImage has been added.
		/// </returns>
		public int Add(byte[] array)
		{
			return List.Add(new Surface(array));
		} 

		/// <summary>
		/// Create a SdlImage from a System.Drawing.Bitmap and add it 
		/// to the end of the SdlImageList.
		/// </summary>
		/// <param name="bitmap">The System.Drawing.Bitmap to create the 
		/// Image from.</param>
		/// <returns>
		/// The index at which the SdlImage has been added.
		/// </returns>
		public int Add(Bitmap bitmap)
		{
			return List.Add(new Surface(bitmap));
		} 			

		/// <summary>
		/// Adds the specified SdlImage to the SdlImageList.
		/// </summary>
		/// <param name="index"></param>
		/// <param name="surface"></param>
		public void Insert(int index, Surface surface)
		{
			List.Insert(index,surface);
		} 

		/// <summary>
		/// Removes a specified SdlImage from the list.
		/// </summary>
		/// <param name="surface">The SdlImage to remove from the SdlImageList. </param>
		public void Remove(Surface surface)
		{
			List.Remove(surface);
		} 

		/// <summary>
		/// Returns the index of a specified SdlImage in the list.
		/// </summary>
		/// <param name="surface">The surface object</param>
		/// <returns>The index of specified surface in the list</returns>
		public int IndexOf(Surface surface)
		{
			return List.IndexOf(surface);
		} 

		/// <summary>
		/// Indicates whether a specified SdlImage is contained in the list.
		/// </summary>
		/// <param name="surface">The SdlImage to find in the list.</param>
		/// <returns>
		/// true if the SdlImage is found in the list; otherwise, false.
		/// </returns>
		public bool Contains(Surface surface)
		{
			return List.Contains(surface);
		} 

		// Provide the explicit interface member for ICollection.
		void ICollection.CopyTo(Array array, int index)
		{
			data.CopyTo(array, index);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="array"></param>
		/// <param name="index"></param>
		// Provide the strongly typed member for ICollection.
		public void CopyTo(Exception[] array, int index)
		{
			((ICollection)this).CopyTo(array, index);
		}

	}	
}
