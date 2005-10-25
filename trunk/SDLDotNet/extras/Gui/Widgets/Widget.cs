using System;
using System.Drawing;
using SdlDotNet;
using SdlDotNet.Gui.Widgets;

namespace SdlDotNet.Gui
{
	/// <summary>
	/// A simple base GUI element.
	/// </summary>
	public abstract class Widget
	{
		/// <summary>
		/// 
		/// </summary>
		public Widget()
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="destination"></param>
		/// <param name="skin"></param>
		public abstract void Render(Surface destination, GuiSkin skin);

		private Rectangle m_RelativePosition;

		/// <summary>
		/// 
		/// </summary>
		public Rectangle RelativePosition
		{
			get
			{
				return m_RelativePosition;
			}
			set
			{
				m_RelativePosition = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public Rectangle AbsolutePosition
		{
			get
			{
				Rectangle par = m_Parent.RelativePosition;
				return new Rectangle(
					par.X + m_RelativePosition.X, par.Y + m_RelativePosition.Y,
					m_RelativePosition.Width, m_RelativePosition.Height);
			}
			set
			{
				Rectangle par = m_Parent.RelativePosition;
				m_RelativePosition = new Rectangle(
					value.X - par.X, value.Y - par.Y,
					value.Width, value.Height);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public void Remove()
		{
			m_Parent.Children.Remove(this);
		}

		private bool m_Visible = true;

		/// <summary>
		/// 
		/// </summary>
		public bool Visible
		{
			get
			{
				return m_Visible;
			}
			set
			{
				m_Visible = value;
			}
		}

		private string m_Text = "";
		/// <summary>
		/// 
		/// </summary>
		public string Text
		{
			get
			{
				return m_Text;
			}
			set
			{
				m_Text = value;
			}
		}


		private bool m_Enabled = true;
		/// <summary>
		/// 
		/// </summary>
		public bool Enabled
		{
			get
			{
				return m_Enabled;
			}
			set
			{
				m_Enabled = value;
			}
		}

		private WidgetCollection m_Children;

		/// <summary>
		/// 
		/// </summary>
		public WidgetCollection Children
		{
			get
			{
				return m_Children;
			}
		}

		private Widget m_Parent;
		/// <summary>
		/// 
		/// </summary>
		public Widget Parent
		{
			get
			{
				return m_Parent;
			}
			set
			{
				m_Parent = value;
			}
		}
	}
}
