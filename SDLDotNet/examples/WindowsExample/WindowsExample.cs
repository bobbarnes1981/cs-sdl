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
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Threading;
using System.Drawing.Imaging;

namespace SdlDotNet.Examples
{
	/// <summary>
	/// Summary description for WindowsExample.
	/// </summary>
	public class WindowsExample : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button button1;
		private SdlDotNet.Windows.SurfaceControl surfaceControl1;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		/// <summary>
		/// 
		/// </summary>
		public WindowsExample()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			surf = new Surface(this.ClientSize.Width,this.ClientSize.Height, false);
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.button1 = new System.Windows.Forms.Button();
			this.surfaceControl1 = new SdlDotNet.Windows.SurfaceControl();
			this.SuspendLayout();
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(128, 96);
			this.button1.Name = "button1";
			this.button1.TabIndex = 0;
			this.button1.Text = "button1";
			// 
			// surfaceControl1
			// 
			this.surfaceControl1.Alpha = ((System.Byte)(0));
			this.surfaceControl1.Location = new System.Drawing.Point(16, 136);
			this.surfaceControl1.Name = "surfaceControl1";
			this.surfaceControl1.Size = new System.Drawing.Size(272, 152);
			this.surfaceControl1.TabIndex = 1;
			this.surfaceControl1.TabStop = false;
			// 
			// WindowsExample
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(300, 300);
			this.Controls.Add(this.surfaceControl1);
			this.Controls.Add(this.button1);
			this.Name = "WindowsExample";
			this.Text = "SDL.NET - WindowsExample";
			this.Load += new System.EventHandler(this.WindowsExample_Load);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			SdlDotNet.Events.Fps = 30;
			SdlDotNet.Events.Tick += new SdlDotNet.TickEventHandler(Events_Tick);
			Application.Run(WindowsExample.Instance = new WindowsExample());
		}

		private static System.Random rand = new Random();
		private static WindowsExample Instance;
		private static SdlDotNet.Surface surf;

		private static void Events_Tick(object sender, SdlDotNet.TickEventArgs e)
		{
			surf.Fill(Color.FromArgb(rand.Next(256), rand.Next(256), rand.Next(256)));
			surf.Update();
			WindowsExample.Instance.UpdateForm();
		}

		/// <summary>
		/// 
		/// </summary>
		public void UpdateForm()
		{
			//img = surf.Bitmap;
			//WindowsExample.Instance.gfx.DrawImage(img,0,0);
			this.surfaceControl1.Surface.Blit(surf);
		}

		private void WindowsExample_Load(object sender, System.EventArgs e)
		{
			//WindowsExample.Instance.gfx = Graphics.FromHwnd(this.Handle);
			Thread a = new Thread(new ThreadStart(SdlDotNet.Events.Run));
			a.IsBackground = true;
			a.Name = "SDL";
			a.Priority = ThreadPriority.Normal;
			a.Start();
		}
	}
}
