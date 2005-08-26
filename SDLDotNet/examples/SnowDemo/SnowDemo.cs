/* This file is part of SnowDemo
* SnowDemo.cs, (c) 2003 Sijmen Mulder
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

using SdlDotNet;
using System;
using System.Drawing;

namespace SdlDotNet.Examples
{
	/// <summary>
	/// 
	/// </summary>
	class SnowDemo
	{
		static Snowflake[] snowflakes;
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
		public static Snowflake[] GetSnowflakes()
		{
			return snowflakes;
		}

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

		/// <summary>
		/// 
		/// </summary>
		/// <param name="numberOfSnowflakes"></param>
		public static void Init(int numberOfSnowflakes)
		{
			snowflakes = new Snowflake[numberOfSnowflakes];

			for(int i = 0; i < snowflakes.Length; i++)
			{
				snowflakes[i] = new Snowflake();
			}
			texts = new Texts();
		}

		static int numberOfSnowflakes = 250;

		/// <summary>
		/// 
		/// </summary>
		public static int NumberOfSnowflakes
		{
			get
			{
				return numberOfSnowflakes;
			}
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
			Init(250);
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
				Console.WriteLine("Frames per second: {0}",
					(int)(frames / frametime));

				frametime = 0;
				frames = 0;
			}

			for(int i = 0; i < SnowDemo.GetSnowflakes().Length; i++)
			{
				SnowDemo.GetSnowflakes()[i].Update(seconds);
			}

			for(int i = 0; i < SnowDemo.texts.Length; i++)
			{
				SnowDemo.Texts[i].Update(seconds);
			}


			screen.Fill(new Rectangle(new Point(0, 0), screen.Size),
				Color.FromArgb(64, 175, 239));
			
			for(int i = 0; i < SnowDemo.NumberOfSnowflakes; i++)
			{
				screen.Blit(SnowDemo.GetSnowflakes()[i].Surface, SnowDemo.GetSnowflakes()[i].Position);
			}
			
			screen.Blit(background, new Rectangle(new Point(0, 280), background.Size));
			
			for(int i = 0; i < SnowDemo.Texts.Length; i++)
			{
				screen.Blit(SnowDemo.Texts[i].Surface, SnowDemo.Texts[i].Position);
			}
			screen.Flip();

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
