/*
 * $RCSfile$
 * Copyright (C) 2004 D. R. E. Moonfire (d.moonfire@mfgames.com)
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

using SdlDotNet.Sprites;
using SdlDotNet.Utility;
using SdlDotNet;
using System.Drawing;
using System;

namespace MfGames.Sdl.Gui
{
	/// <summary>
	/// Handles a simple window element, which displays its contents in
	/// a frame.
	/// </summary>
	public class GuiWindow : GuiContainer
	{
		public GuiWindow(GuiManager manager)
			: base(manager)
		{
		}

		public GuiWindow(GuiManager manager, Rectangle rect)
			: base(manager, rect)
		{
		}

		#region Drawing
		public override Padding OuterPadding
		{
			get { return manager.GetPadding(this); }
		}

		public override void Render(RenderArgs args)
		{
			// Render the window using the GUI manager
			manager.Render(args, this);

			// Render the components
			base.Render(args);
		}
		#endregion

		#region Operators
		public override string ToString()
		{
			return String.Format("(window \"{0}\" {1})",
				Title, base.ToString());
		}
		#endregion

		#region Properties
		private string title = null;
		private Size titleSize = new Size();

		public string Title
		{
			get { return title; }
			set
			{
				// Set the title size
				title = value;

				// Set the bounds
				titleSize = manager.GetTextSize(manager.TitleFont, title);
			}
		}
		#endregion
	}
}
