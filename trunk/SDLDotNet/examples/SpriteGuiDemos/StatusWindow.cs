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

using SdlDotNet.Examples.GuiExample;
using SdlDotNet.Sprites;
using SdlDotNet;
using System;
using System.Drawing;

namespace SdlDotNet.Examples
{
	/// <summary>
	/// This is a status window which shows the current state of the
	/// system at any time. This allows the system to report anything
	/// required.
	/// </summary>
	public class StatusWindow : GuiWindow
	{
		/// <summary>
		/// Creates a basic status window above everything else.
		/// </summary>
		public StatusWindow(GuiManager manager)
			: base(manager, new Rectangle(625, 475, 150, 100))
		{
			// Set up our title
			//Coordinates.Z = 2000;
			Title = "Demo Status";

			// Add some text
			int labelOffset = 2;
			int dataOffset = 54;
			int labelHeight = manager.GetTitleHeight("test");
			int labelPad = 2;
			int labelWidth = 48;
			int dataWidth = 96;
			int i = 0;

			// Add the ticks per second
			Contents.Add(new BoundedTextSprite("TPS:", manager.TitleFont,
				new Size(labelWidth, labelHeight),
				1.0, 0.5,
				new Point(labelOffset,
				(labelHeight
				+ labelPad) * i + 2)));
			tps = new BoundedTextSprite("---", manager.BaseFont,
				new Size(dataWidth, labelHeight),
				0.0, 0.5,
				new Point(dataOffset,
				(labelHeight + labelPad) * i + 2));
			Contents.Add(tps);

			// Add the frames per second
			i++;
			Contents.Add(new BoundedTextSprite("FPS:", manager.TitleFont,
				new Size(labelWidth, labelHeight),
				1.0, 0.5,
				new Point(labelOffset,
				(labelHeight
				+ labelPad) * i + 2)));
			fps = new BoundedTextSprite("---", manager.BaseFont,
				new Size(dataWidth, labelHeight),
				0.0, 0.5,
				new Point(dataOffset,
				(labelHeight + labelPad) * i + 2));
			Contents.Add(fps);

			// Add the current mode
			i++;
			Contents.Add(new BoundedTextSprite("Mode:", manager.TitleFont,
				new Size(labelWidth, labelHeight),
				1.0, 0.5,
				new Point(labelOffset,
				(labelHeight
				+ labelPad) * i + 2)));
			mode = new BoundedTextSprite("---", manager.BaseFont,
				new Size(dataWidth, labelHeight),
				0.0, 0.5,
				new Point(dataOffset,
				(labelHeight + labelPad)
				* i + 2));
			Contents.Add(mode);

			// Add the instructions
			i++;
			Contents.Add(new BoundedTextSprite("Press the number keys",
				manager.BaseFont,
				new Size(150, labelHeight),
				0.5, 0.5,
				new Point(labelOffset,
				(labelHeight
				+ labelPad) * i + 2)));

			// Add ourselves to the ticker
			this.EnableTickEvent();

			// Adjust our height
			i++;
			int tempHeight = (labelHeight + labelPad) * i + 4;
			Size = new Size(this.Size.Width, tempHeight); 
		}

		#region Data Components
		private BoundedTextSprite tps = null;
		private BoundedTextSprite fps = null;
		private BoundedTextSprite mode = null;
		#endregion

		#region Animation
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		public new void OnTick(object sender, TickEventArgs args)
		{
			tps.TextString = String.Format("{0}", Events.TicksPerSecond);

			//if (SdlDemo.Fps.IsFull)
		//	{
			//	fps.TextString = SdlDemo.Fps.FramesPerSecond.ToString("#0.00");
			//}
		//	else
		//	{
				fps.TextString = "---";
		//	}

			if (SdlDemo.CurrentDemo == null)
			{
				mode.TextString = "<none>";
			}
			else
			{
				mode.TextString = SdlDemo.CurrentDemo.ToString();
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public void EnableTickEvent()
		{
			Events.TickEvent += new TickEventHandler(this.OnTick);
		}
		#endregion
	}
}
