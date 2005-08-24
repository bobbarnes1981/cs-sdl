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
using Tao.Sdl; 

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
		private System.Windows.Forms.Label label1;
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
			this.label1 = new System.Windows.Forms.Label();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(16, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(80, 24);
			this.label1.TabIndex = 0;
			this.label1.Text = "Select Drive:";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
			Sdl.SDL_SysWMinfo_Windows info;
			Sdl.SDL_GetWMInfo(out info);
			IntPtr frmhandle = new IntPtr(info.window);
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
				ControlPaint.DrawButton(System.Drawing.Graphics.FromHwnd(frmhandle), 0, 0, 100, 100, ButtonState.Normal); 
			} 
		} 
		static void Main() 
		{ 
			WindowsExample t = new WindowsExample(); 
			t.Run(); 
		} 
	} 
}