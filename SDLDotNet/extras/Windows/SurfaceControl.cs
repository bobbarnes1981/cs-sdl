#region License
/*
 * $RCSfile$
 * Copyright (C) 2005 David Hudson (jendave@yahoo.com)
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
#endregion License

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using SdlDotNet;

namespace SdlDotNet.Windows 
{
	#region Class Documentation
	/// <summary>
	///     Provides a simple Sdl Surface control allowing 
	///     quick development of Windows Forms-based
	///     Sdl Surface applications.
	/// </summary>
	#endregion Class Documentation
	[ToolboxBitmap(typeof(Bitmap),"SurfaceControl.bmp")]
	public class SurfaceControl : System.Windows.Forms.PictureBox
	{
		Surface surface;
		Bitmap bitmap;

		/// <summary>
		/// 
		/// </summary>
		public SurfaceControl()
		{
			SdlDotNet.Events.Tick +=new TickEventHandler(OnTick);
			surface = new Surface(this.Width,this.Height, false);
			this.bitmap = surface.Bitmap;
		}

		/// <summary>
		/// 
		/// </summary>
		public Surface Surface
		{
			get
			{
				return surface;
			}
			set
			{
				surface = value;
				this.Image = surface.Bitmap;
			}
		}

		/// <summary>
		/// Get/set the Alpha flags of the image.
		/// </summary>
		[Category("Sdl Properties"), Description("Set Alpha")]
		public byte Alpha
		{
			get 
			{
				return surface.Alpha;
			}
			set	
			{
				surface.Alpha = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		protected override void OnPaint(PaintEventArgs e)
		{
			//this.Image = this.bitmap;
			base.OnPaint (e);			
		}

		private void OnTick(object sender, TickEventArgs e)
		{
			//this.bitmap = this.surface.Bitmap;
			this.Image = this.surface.Bitmap;
		}
	}
}
