// Copyright 2005 Terry Price (Tarryp_AT_meldstat_DOT_com)
//
// SimpleExample is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
//
// SimpleExample is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with SimpleGame; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA

using System;
using System.Drawing;
using Tao.Sdl;

using SdlDotNet;

namespace SdlDotNet.Examples
{
	class SimpleExample
	{
		#region Variables
		private const int width = 640;
		private const int height = 480;
		private const int bpp = 32;
		private Random rand;
		private bool quit;

		#region sdlvars
		private Surface screen;
		#endregion
		#endregion

		/// <summary>
		/// 
		/// </summary>
		public SimpleExample()
		{
			Events.KeyboardDown += new KeyboardEventHandler(this.KeyDown);
			Events.Quit += new QuitEventHandler(this.Quit);
			screen = Video.SetVideoModeWindow(width, height);
			rand = new Random();
		}

		private void KeyDown(object sender, KeyboardEventArgs e)
		{
			if (e.Key == Key.Escape)
			{
				quit = true;
			}
		}

		private void Quit(object sender, QuitEventArgs e)
		{
			quit = true;
		}

		public void Run()
		{
			while (quit == false)
			{
				while(Events.Poll())
				{
				}
				screen.Lock();
				screen.Fill(Color.FromArgb(rand.Next(255),rand.Next(255),rand.Next(255)));
				System.Threading.Thread.Sleep(100);
				screen.Unlock();
				screen.Flip();
			}
		}

		public static void Main()
		{
			SimpleExample t = new SimpleExample();
			t.Run();
		}
	}
} 