/* This file is part of SnowDemo
 * SnowDemo.cs, (c) 2005 David Hudson
 * based on code by (c) 2003 Sijmen Mulder
 *
 * This program is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 2 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
 */

using System;
using System.Drawing;

using SdlDotNet;
using SdlDotNet.Sprites;

namespace SdlDotNet.Examples
{
	/// <summary>
	/// 
	/// </summary>
	class SnowDemo
	{
		static string[] textArray = {
										"when the cold of winter comes",
										"starless night", "will cover day",
										"in the veiling of the sun", 
										"we will walk",
										"in bitter rain"
									};
		SpriteCollection snowflakes = new SpriteCollection();
		SpriteCollection textItems = new SpriteCollection();
		Surface screen;
		Surface background;
		Surface tree;
		Surface treeStretch;
		string fontName = @"../../Data/FreeSans.ttf";

		/// <summary>
		/// 
		/// </summary>
		public SnowDemo()
		{
		}

		void Initialize(int numberOfSnowflakes)
		{
			for(int i = 0; i < numberOfSnowflakes; i++)
			{
				snowflakes.Add(new Snowflake());
			}
			Font font = new Font(fontName, 24);

			textItems.Add(new TextItem(textArray[0], font, 25, 0));
			for (int i = 1; i < textArray.Length; i++)
			{
				textItems.Add(
					new TextItem(textArray[i], 
					font, textItems[i-1].Rectangle.Bottom + 10, 
					i * 2));
			}
			snowflakes.EnableTickEvent();
			textItems.EnableTickEvent();
		}

		[STAThread]
		static void Main()
		{
			SnowDemo snowDemo = new SnowDemo();
			snowDemo.Run();
		}

		/// <summary>
		/// 
		/// </summary>
		public void Run()
		{
			screen = Video.SetVideoModeWindow(640, 480, 16, true);
			background = new Surface(@"../../Data/background.png");
			background.TransparentColor = Color.Magenta;
			tree = new Surface(@"../../Data/Tree.bmp");
			tree.TransparentColor = Color.Magenta;
			treeStretch = tree.Stretch(new Size(100,100));
			Video.WindowCaption = "SdlDotNet - Snow Demo";
			Initialize(250);
			Events.KeyboardDown +=
				new KeyboardEventHandler(this.OnKeyboardDown);
			Events.KeyboardDown +=
				new KeyboardEventHandler(this.OnKeyboardDown);
			Events.Tick += new TickEventHandler(this.OnTick);
			Events.Run();
		}
		
		private void OnTick(object sender, TickEventArgs args)
		{	
			screen.Fill(Color.FromArgb(64, 175, 239));
			screen.Blit(snowflakes);
			screen.Blit(background, new Point(0, 280));
			screen.Blit(textItems);
			screen.Blit(tree, new Point(100, 300));
			screen.Blit(tree, new Point(130, 295));
			screen.Blit(tree, new Point(155, 302));
			screen.Blit(tree, new Point(230, 302));
			screen.Blit(treeStretch, new Point(180, 290));
			screen.Update();
		}

		private void OnKeyboardDown(object sender, KeyboardEventArgs e)
		{
			if (e.Key == Key.Escape ||
				e.Key == Key.Q)
			{
				Events.QuitApp();
			}
		}
	}
}
