using System;

namespace SdlDotNet.Gui.Widgets
{
	/// <summary>
	/// Summary description for Button.
	/// </summary>
	public class Button : Widget
	{
		/// <summary>
		/// 
		/// </summary>
		public Button()
		{

			//
			// TODO: Add constructor logic here
			//
		}
		private bool m_Pressed;

		/// <summary>
		/// 
		/// </summary>
		public bool Pressed
		{
			get
			{
				return m_Pressed;
			}
			set
			{
				m_Pressed = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="destination"></param>
		/// <param name="skin"></param>
		public override void Render(Surface destination, GuiSkin skin)
		{
			//TODO: provide implementation
		}

	}
}
