using System;

namespace SdlDotNet.Gui.Widgets
{
	/// <summary>
	/// Summary description for Button.
	/// </summary>
	public class Button : Widget
	{
		public Button()
		{

			//
			// TODO: Add constructor logic here
			//
		}
		private bool m_Pressed;
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
	}
}
