using System;
using System.Collections;

namespace SdlDotNet.Gui.Widgets
{
	/// <summary>
	/// A collection of widgets.
	/// </summary>
	public class WidgetCollection : CollectionBase
	{
		public WidgetCollection()
		{
		}

		public WidgetCollection(WidgetCollection widgets)
		{
			Add(widgets);
		}

		public WidgetCollection(Widget widget)
		{
			Add(widget);
		}

		public void Add(WidgetCollection widgets)
		{
			foreach(Widget widget in widgets)
			{
				Add(widget);
			}
		}

		public void Remove(Widget child)
		{
			List.Remove(child);
		}


		public void Add(Widget widget)
		{
			List.Add(widget);
		}

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
