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

using SdlDotNet.Utility;
using SdlDotNet.Sprites;
using SdlDotNet;
using System;
using System.Drawing;

namespace MfGames.Sdl.Gui
{
	/// <summary>
	/// Class to manager internal sprites, such as window
	/// components. This uses a sprite manager at its core, but does
	/// have some additional functionality.
	/// </summary>
	public class GuiContainer : GuiComponent
	{
		#region Constructors
		public GuiContainer(GuiManager manager)
			: base(manager)
		{
		}

		public GuiContainer(GuiManager manager, Rectangle rect)
			: base(manager, rect)
		{
		}
		#endregion

		#region Drawing
		public override void Render(RenderArgs args)
		{
			// Modify the arguments
			RenderArgs args1 = args.Clone();
			args1.TranslateX += Coords.X;
			args1.TranslateY += Coords.Y;
			args1.SetClipping(new Rectangle(args.TranslateX + Coords.X,
				args.TranslateY + Coords.Y,
				Size.Width,
				Size.Height));

			// Render the contents
			contents.Render(args1);
			args1.ClearClipping();
		}
		#endregion

		#region Events
		public override void OnTick(TickArgs args)
		{
			base.OnTick(args);
			contents.OnTick(args);
		}
		#endregion

		#region Properties
		private SpriteContainer contents = new SpriteContainer();

		public SpriteContainer Contents
		{
			get { return contents; }
		}
		#endregion
	}
}
