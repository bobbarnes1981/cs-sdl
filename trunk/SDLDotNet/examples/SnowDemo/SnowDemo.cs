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
	public class SnowDemo
	{
		static Snowflake[] snowflakes;
		static Texts texts;

		/// <summary>
		/// 
		/// </summary>
		public static Snowflake[] Snowflakes
		{
			get
			{
				return SnowDemo.snowflakes;
			}
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
		/// <param name="NumberOfSnowflakes"></param>
		public void Init(int NumberOfSnowflakes)
		{
			snowflakes = new Snowflake[NumberOfSnowflakes];

			for(int i = 0; i < snowflakes.Length; i++)
			{
				snowflakes[i] = new Snowflake();
			}
			texts = new Texts();
		}

		/// <summary>
		/// 
		/// </summary>
		public int Length
		{
			get
			{
				return snowflakes.Length;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public Snowflake this[int Index]
		{
			get
			{
				return snowflakes[Index];
			}
		}

		static bool run = true;
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
			Graphics.Init();
			this.Init(250);
			Console.WriteLine("Setting video mode");
			Console.WriteLine("Initializing game data");
			Events.KeyboardDown +=
				new KeyboardEventHandler(this.KeyboardDown);
			Console.WriteLine("Binding quit event");
			Events.Quit += new QuitEventHandler(this.Quit);
			Console.WriteLine("Starting the loop");

			int lastframe = Timer.Ticks;
			int newframe;
			float seconds;

			float frametime = 0;
			int frames = 0;

			
			try
			{
				while(run)
				{

					while (Events.Poll()) 
					{
						// handle events till the queue is empty
					}
			
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

					Events.Poll();

					for(int i = 0; i < SnowDemo.Snowflakes.Length; i++)
					{
						SnowDemo.Snowflakes[i].Update(seconds);
					}

					for(int i = 0; i < SnowDemo.texts.Length; i++)
					{
						SnowDemo.Texts[i].Update(seconds);
					}

					Graphics.DrawFrame();

					lastframe = newframe;
				}
			}
			catch
			{}

		}

		private void KeyboardDown(object sender, KeyboardEventArgs e)
		{
			if (e.Key == Key.Escape ||
				e.Key == Key.Q)
			{
				run = false;
			}
		}

		private void Quit(object sender, QuitEventArgs e)
		{
			run = false;
		}
	}
}
