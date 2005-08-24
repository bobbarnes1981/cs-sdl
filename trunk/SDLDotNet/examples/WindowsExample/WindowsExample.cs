/*
 * $RCSfile$
 * Copyright (C) 2005 didius
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

using System; 
using System.Drawing; 
using System.Windows.Forms; 
using System.Runtime.InteropServices; 
using System.Text; 

using SdlDotNet; 

namespace SdlDotNet.Examples
{
	/// <summary>
	/// 
	/// </summary>
	public class WindowsExample 
	{ 
		private const int width = 640; 
		private const int height = 480; 
		private const int bpp = 32; 
		private Random rand; 
		private bool quit; 
		private Surface screen; 

		/// <summary>
		/// 
		/// </summary>
		public WindowsExample() 
		{ 
			Events.KeyboardDown += new KeyboardEventHandler(this.KeyDown); 
			Events.Quit += new QuitEventHandler(this.Quit); 
			screen = Video.SetVideoModeWindow(width, height); 
			Video.WindowCaption = "SdlDotNet - Windows Example";
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

		/// <summary>
		/// 
		/// </summary>
		public void Run() 
		{
			while (quit == false) 
			{ 
				while (Events.Poll()) 
				{ 
				} 
				screen.Lock(); 
				screen.Fill(Color.FromArgb(rand.Next(255), rand.Next(255), rand.Next(255))); 
				System.Threading.Thread.Sleep(100); 
				screen.Unlock(); 
				screen.Flip(); 
				ControlPaint.DrawButton(System.Drawing.Graphics.FromHwnd(Video.WindowHandle), 0, 0, 100, 100, ButtonState.Normal); 
			} 
		} 
		static void Main() 
		{ 
			WindowsExample t = new WindowsExample(); 
			t.Run(); 
		} 
	} 
}