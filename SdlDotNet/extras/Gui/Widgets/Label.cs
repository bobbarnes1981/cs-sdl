using System;
using System.Drawing;

using SdlDotNet;

namespace SdlDotNet.Gui.Widgets
{
	/// <summary>
	/// Summary description for Label.
	/// </summary>
	public class Label : Widget
	{
		/// <summary>
		/// 
		/// </summary>
		public Label()
		{
		}

		private SdlDotNet.Font m_Font;
		/// <summary>
		/// 
		/// </summary>
		public SdlDotNet.Font Font
		{
			get
			{
				return m_Font;
			}
			set
			{
				m_Font = value;
			}
		}

		private Color m_Color;
		/// <summary>
		/// 
		/// </summary>
		public Color Color
		{
			get
			{
				return m_Color;
			}
			set
			{
				m_Color = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="destination"></param>
		/// <param name="skin"></param>
		public override void Render(Surface destination, GuiSkin skin)
		{
			if (destination == null)
			{
				throw new ArgumentNullException("destination");
			}
			destination.Blit(m_Font.Render(this.Text, m_Color),AbsolutePosition);
		}
	}
}
