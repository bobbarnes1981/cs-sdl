/*
 * $RCSfile$
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
using SDLDotNet;


namespace SDLDotNet.Image
{

	/// <summary>
	/// Provides methods to manage a collection of SDLImage objects. 
	/// </summary>
	public class SDLImageList
	{

		/// <summary>
		/// Private field that holds SDLImageList.ImageCollection for this image list.
		/// </summary>
		private SDLImageCollection images;

		/// <summary>
		/// Create a new instance of a SDLImageList
		/// </summary>
		public SDLImageList() 
		{
			images = new SDLImageCollection();
		}


		/// <summary>
		/// Gets the SDLImageList.ImageCollection for this image list.
		/// </summary>
		public SDLImageCollection Images 
		{
			get {return images;}
		}

		/// <summary>
		/// Encapsulates the collection of SDLImage objects in an SDLImageList.
		/// </summary>
		public class SDLImageCollection : CollectionBase 
		{


			/// <summary>
			/// Indexer for the Items in the Collection
			/// </summary>
			public SDLImage this[int index] 
			{
				get { return (SDLImage)List[index];	}
			}


			/// <summary>
			/// Adds the specified SDLImage to the end of the SDLImageList.
			/// </summary>
			/// <param name="image">The SDLImage to be added to the end of the SDLImageList.</param>
			/// <returns>The index at which the SDLImage has been added.</returns>
			public int Add(SDLImage image)
			{
				return List.Add(image);
			} 


			/// <summary>
			/// Load a SDLImage with the specified filename and add it to the end of the SDLImageList.
			/// </summary>
			/// <param name="filename">The filename of the SDLImage to be added to the end of the SDLImageList.</param>
			/// <returns>The index at which the SDLImage has been added.</returns>
			public int Add(string filename)
			{
				return List.Add(new SDLImage(filename));
			} 


			/// <summary>
			/// Create a SDLImage from a byte array and add it to the end of the SDLImageList.
			/// </summary>
			/// <param name="arr">The array of byte to create the SDLImage from.</param>
			/// <returns>The index at which the SDLImage has been added.</returns>
			public int Add(byte[] arr)
			{
				return List.Add(new SDLImage(arr));
			} 


			
			#if !__MONO__
			/// <summary>
			/// Create a SDLImage from a System.Drawing.Bitmap and add it to the end of the SDLImageList.
			/// </summary>
			/// <param name="bitmap">The System.Drawing.Bitmap to create the SDLImage from.</param>
			/// <returns>The index at which the SDLImage has been added.</returns>
			public int Add(Bitmap bitmap)
			{
				return List.Add(new SDLImage(bitmap));
			} 
			#endif
			

			/// <summary>
			/// Adds the specified SDLImage to the SDLImageList.
			/// </summary>
			/// <param name="index"></param>
			/// <param name="image"></param>
			public void Insert(int index, SDLImage image)
			{
				List.Insert(index,image);
			} 


			/// <summary>
			/// Removes a specified SDLImage from the list.
			/// </summary>
			/// <param name="image">The SDLImage to remove from the SDLImageList. </param>
			public void Remove(SDLImage image)
			{
				List.Remove(image);
			} 


			/// <summary>
			/// Returns the index of a specified SDLImage in the list.
			/// </summary>
			/// <param name="image">The image object</param>
			/// <returns>The index of specified image in the list</returns>
			public int IndexOf(SDLImage image)
			{
				return List.IndexOf(image);
			} 


			/// <summary>
			/// Indicates whether a specified SDLImage is contained in the list.
			/// </summary>
			/// <param name="image">The SDLImage to find in the list.</param>
			/// <returns>true if the SDLImage is found in the list; otherwise, false.</returns>
			public bool Contains(SDLImage image)
			{
				return List.Contains(image);
			} 
			

		}
		
	}

}
