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
	public class ImageList
	{

		/// <summary>
		/// Private field that holds ImageList.ImageCollection for this image list.
		/// </summary>
		private ImageCollection images;

		/// <summary>
		/// Create a new instance of a SdlImageList
		/// </summary>
		public ImageList() 
		{
			images = new ImageCollection();
		}


		/// <summary>
		/// Gets the ImageList.ImageCollection for this image list.
		/// </summary>
		public ImageCollection Images 
		{
			get {return images;}
		}

		/// <summary>
		/// Encapsulates the collection of SdlImage objects in an SdlImageList.
		/// </summary>
		public class ImageCollection : CollectionBase 
		{


			/// <summary>
			/// Indexer for the Items in the Collection
			/// </summary>
			public Image this[int index] 
			{
				get { return (Image)List[index];	}
			}


			/// <summary>
			/// Adds the specified SdlImage to the end of the SdlImageList.
			/// </summary>
			/// <param name="image">The SdlImage to be added to the end of the SdlImageList.</param>
			/// <returns>The index at which the SdlImage has been added.</returns>
			public int Add(Image image)
			{
				return List.Add(image);
			} 


			/// <summary>
			/// Load a SdlImage with the specified filename and add it to the end of the SdlImageList.
			/// </summary>
			/// <param name="filename">The filename of the SdlImage to be added to the end of the SdlImageList.</param>
			/// <returns>The index at which the SdlImage has been added.</returns>
			public int Add(string filename)
			{
				return List.Add(new Image(filename));
			} 


			/// <summary>
			/// Create a SdlImage from a byte array and add it to the end of the SdlImageList.
			/// </summary>
			/// <param name="arr">The array of byte to create the SdlImage from.</param>
			/// <returns>The index at which the SdlImage has been added.</returns>
			public int Add(byte[] arr)
			{
				return List.Add(new Image(arr));
			} 


			
			#if !__MONO__
			/// <summary>
			/// Create a SdlImage from a System.Drawing.Bitmap and add it to the end of the SdlImageList.
			/// </summary>
			/// <param name="bitmap">The System.Drawing.Bitmap to create the SdlImage from.</param>
			/// <returns>The index at which the SdlImage has been added.</returns>
			public int Add(Bitmap bitmap)
			{
				return List.Add(new Image(bitmap));
			} 
			#endif
			

			/// <summary>
			/// Adds the specified SdlImage to the SdlImageList.
			/// </summary>
			/// <param name="index"></param>
			/// <param name="image"></param>
			public void Insert(int index, Image image)
			{
				List.Insert(index,image);
			} 


			/// <summary>
			/// Removes a specified SdlImage from the list.
			/// </summary>
			/// <param name="image">The SdlImage to remove from the SdlImageList. </param>
			public void Remove(SdlImage image)
			{
				List.Remove(image);
			} 


			/// <summary>
			/// Returns the index of a specified SdlImage in the list.
			/// </summary>
			/// <param name="image">The image object</param>
			/// <returns>The index of specified image in the list</returns>
			public int IndexOf(SdlImage image)
			{
				return List.IndexOf(image);
			} 


			/// <summary>
			/// Indicates whether a specified SdlImage is contained in the list.
			/// </summary>
			/// <param name="image">The SdlImage to find in the list.</param>
			/// <returns>true if the SdlImage is found in the list; otherwise, false.</returns>
			public bool Contains(SdlImage image)
			{
				return List.Contains(image);
			} 
			

		}
		
	}

}
