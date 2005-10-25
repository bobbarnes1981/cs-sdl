using System;
using System.Collections;

namespace SdlDotNet.Gui.Widgets
{
	/// <summary>
	/// A collection of widgets.
	/// </summary>
	public class WidgetCollection : CollectionBase
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
	}
}
