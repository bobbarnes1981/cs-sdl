using System;

using SdlDotNet.Gui.Widgets;

namespace SdlDotNet.Gui
{
	/// <summary>
	/// Used as a factory and manager of all other GUI elements.
	/// </summary>
	/// <remarks>This is based off of the Irrlicht Gui system.</remarks>
	public class GuiEnvironment
	{
		public GuiEnvironment()
		{
		}

		private WidgetCollection m_Widgets;
		public WidgetCollection Widgets
		{
			get
			{
				return m_Widgets;
			}
		}

		private GuiSkin m_Skin;
		public GuiSkin Skin
		{
			get
			{
				return m_Skin;
			}
			set
			{
				m_Skin = value;
			}
		}
	}
}
