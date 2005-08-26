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
****************************************************************************/

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
		SpriteCollection snowflakes = new SpriteCollection();
		static Texts texts;
		Surface screen;
		Surface background;
		int lastframe = Timer.Ticks;
		int newframe;
		float seconds;

		float frametime = 0;
		int frames = 0;


		/// <summary>
		/// 
		/// </summary>
		public static Texts Texts
		{
			get
			{
				return SnowDemo.texts;
			}
		}

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
			snowflakes.EnableTickEvent();
			texts = new Texts();
		}

		[STAThread]
		static void Main()
		{
			SnowDemo snowdemo = new SnowDemo();
			snowdemo.Run();
		}

		/// <summary>
		/// 
		/// </summary>
		public void Run()
		{
			screen = Video.SetVideoModeWindow(640, 480, 16, true);
			background = new Surface("../../Data/background.png");
			background.SetColorKey(Color.FromArgb(255, 0, 255), true);
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

			newframe = Timer.Ticks;
			seconds = (newframe - lastframe) / 1000.0f;

			frames += 1;
			frametime += seconds;

			if(frametime >= 5)
			{
				frametime = 0;
				frames = 0;
			}

			for(int i = 0; i < SnowDemo.texts.Length; i++)
			{
				SnowDemo.Texts[i].Update(seconds);
			}

			screen.Fill(Color.FromArgb(64, 175, 239));
			screen.Blit(snowflakes);
			screen.Blit(background, new Point(0, 280));
			
			for(int i = 0; i < SnowDemo.Texts.Length; i++)
			{
				screen.Blit(SnowDemo.Texts[i].Surface, SnowDemo.Texts[i].Position);
			}
			screen.Update();

			lastframe = newframe;
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
