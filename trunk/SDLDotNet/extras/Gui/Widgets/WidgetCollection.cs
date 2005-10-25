using System;
using System.Collections;

namespace SdlDotNet.Gui.Widgets
{
	/// <summary>
	/// A collection of widgets.
	/// </summary>
	public class WidgetCollection : CollectionBase, ICollection
	{
		/// <summary>
		/// 
		/// </summary>
		public WidgetCollection()
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="widgets"></param>
		public WidgetCollection(WidgetCollection widgets)
		{
			Add(widgets);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="widget"></param>
		public WidgetCollection(Widget widget)
		{
			Add(widget);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="widgets"></param>
		public void Add(WidgetCollection widgets)
		{
			if (widgets == null)
			{
				throw new ArgumentNullException("widgets");
			}
			foreach(Widget widget in widgets)
			{
				Add(widget);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="child"></param>
		public void Remove(Widget child)
		{
			List.Remove(child);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="widget"></param>
		public void Add(Widget widget)
		{
			List.Add(widget);
		}

		/// <summary>
		/// 
		/// </summary>
		public Widget this[int index]
		{
			get
			{
				return (Widget)List[index];
			}
			set
			{
				List[index] = value;
			}
		}

		/// <summary>
		/// Provide the explicit interface member for ICollection.
		/// </summary>
		/// <param name="array">Array to copy collection to</param>
		/// <param name="index">Index at which to insert the collection items</param>
		void ICollection.CopyTo(Array array, int index)
		{
			this.List.CopyTo(array, index);
		}

		/// <summary>
		/// Provide the explicit interface member for ICollection.
		/// </summary>
		/// <param name="array">Array to copy collection to</param>
		/// <param name="index">Index at which to insert the collection items</param>
		public virtual void CopyTo(Widget[] array, int index)
		{
			((ICollection)this).CopyTo(array, index);
		}

		/// <summary>
		/// Insert a widget into the collection
		/// </summary>
		/// <param name="index">Index at which to insert the widget</param>
		/// <param name="widget">Particle to insert</param>
		public virtual void Insert(int index, Widget widget)
		{
			List.Insert(index, widget);
		} 

		/// <summary>
		/// Gets the index of the given Widget in the collection.
		/// </summary>
		/// <param name="widget">The widget to search for.</param>
		/// <returns>The index of the given widget.</returns>
		public virtual int IndexOf(Widget widget)
		{
			return List.IndexOf(widget);
		} 

		/// <summary>
		/// Checks if widget is in the container
		/// </summary>
		/// <param name="widget">Widget to query for</param>
		/// <returns>True is the Widget is in the container.</returns>
		public bool Contains(Widget widget)
		{
			return (List.Contains(widget));
		}
	}
}
