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
	/// Encapsulates the collection of SdlImage objects in an SdlImageList.
	/// </summary>
	public class ChannelCollection : CollectionBase, ICollection
	{
		private ArrayList data;

		/// <summary>
		/// 
		/// </summary>
		public ChannelCollection()
		{
			data = new ArrayList();
		}

		/// <summary>
		/// Indexer for the Items in the Collection
		/// </summary>
		public Channel this[int index] 
		{
			get 
			{ 
				return (Channel)List[index];	
			}
		}

		/// <summary>
		/// Adds the specified SdlImage to the end of the SdlImageList.
		/// </summary>
		/// <param name="channel">
		/// The SdlImage to be added to the end of the SdlImageList.
		/// </param>
		/// <returns>
		/// The index at which the SdlImage has been added.
		/// </returns>
		public int Add(Channel channel)
		{
			return List.Add(channel);
		} 

		/// <summary>
		/// Load a SdlImage with the specified filename and add 
		/// it to the end of the SdlImageList.
		/// </summary>
		/// <param name="index"></param>
		/// <returns>
		/// The index at which the SdlImage has been added.
		/// </returns>
		public int Add(int index)
		{
			return List.Add(new Channel(index));
		} 			

		/// <summary>
		/// Adds the specified SdlImage to the SdlImageList.
		/// </summary>
		/// <param name="index"></param>
		/// <param name="channel"></param>
		public void Insert(int index, Channel channel)
		{
			List.Insert(index,channel);
		} 

		/// <summary>
		/// Removes a specified SdlImage from the list.
		/// </summary>
		/// <param name="channel">The SdlImage to remove from the SdlImageList. </param>
		public void Remove(Channel channel)
		{
			List.Remove(channel);
		} 

		/// <summary>
		/// Returns the index of a specified SdlImage in the list.
		/// </summary>
		/// <param name="channel">The channel object</param>
		/// <returns>The index of specified channel in the list</returns>
		public int IndexOf(Channel channel)
		{
			return List.IndexOf(channel);
		} 

		/// <summary>
		/// Indicates whether a specified SdlImage is contained in the list.
		/// </summary>
		/// <param name="channel">The SdlImage to find in the list.</param>
		/// <returns>
		/// true if the SdlImage is found in the list; otherwise, false.
		/// </returns>
		public bool Contains(Channel channel)
		{
			return List.Contains(channel);
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
