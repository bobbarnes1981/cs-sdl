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
using SdlDotNet;
using System;
using System.Drawing;

namespace SdlDotNet.Examples.GuiExample
{
	/// <summary>
	/// Class to manager internal sprites, such as window
	/// components. This uses a sprite manager at its core, but does
	/// have some additional functionality.
	/// </summary>
	public class GuiContainer : GuiComponent
	{
		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		/// <param name="manager"></param>
		public GuiContainer(GuiManager manager)
			: base(manager)
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="manager"></param>
		/// <param name="rectangle"></param>
		public GuiContainer(GuiManager manager, Rectangle rectangle)
			: base(manager, rectangle)
		{
		}
		#endregion

		#region Drawing
		/// <summary>
		/// 
		/// </summary>
		public void Render(/*RenderArgs args*/)
		{
			// Modify the arguments
//			RenderArgs args1 = args.Clone();
//			args1.TranslateX += Coordinates.X;
//			args1.TranslateY += Coordinates.Y;
//			args1.Clipping = new Rectangle(args.TranslateX + Coordinates.X,
//				args.TranslateY + Coordinates.Y,
//				Size.Width,
//				Size.Height);
//
//			// Render the contents
//			contents.Render(args1);
//			args1.ClearClipping();
		}
		#endregion

		#region Events
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		public  void OnTick(object sender, TickEventArgs args)
		{
//			base.OnTick(this, args);
//			contents.OnTick(this, args);
		}
		#endregion

		#region Properties
		private SpriteCollection contents = new SpriteCollection();

		/// <summary>
		/// 
		/// </summary>
		public SpriteCollection Contents
		{
			get 
			{ 
				return contents; 
			}
		}
		#endregion
	}
}
