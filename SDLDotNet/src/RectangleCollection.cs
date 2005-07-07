using System;
using System.Drawing;
using System.Collections;

namespace SdlDotNet
{
	/// <summary>
	/// 
	/// </summary>
	public class RectangleCollection : CollectionBase, ICollection
	{
		/// <summary>
		/// 
		/// </summary>
		public RectangleCollection( ) : base( )
		{
		}

		/// <summary>
		/// 
		/// </summary>
		public Rectangle this[int index]  
		{
			get  
			{
				return ((Rectangle)List[index]);
			}
			set  
			{
				List[index] = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public int Add(Rectangle item)  
		{
			return (List.Add(item));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public int IndexOf(Rectangle item)  
		{
			return(List.IndexOf(item));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="index"></param>
		/// <param name="item"></param>
		public void Insert(int index, Rectangle item)  
		{
			List.Insert(index, item);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="item"></param>
		public void Remove(Rectangle item)  
		{
			List.Remove(item);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public bool Contains(Rectangle item)
		{
			return(List.Contains(item));
		}
		#region ICollection Members

		/// <summary>
		/// 
		/// </summary>
		/// <param name="array"></param>
		/// <param name="index"></param>
		public void CopyTo(Array array, int index)
		{
			((ICollection)this).CopyTo(array, index);
		}

		/// <summary>
		/// 
		/// </summary>
		public object SyncRoot
		{
			get
			{
				// TODO:  Add RectangleCollection.SyncRoot getter implementation
				return null;
			}
		}
		#endregion
	}

}
