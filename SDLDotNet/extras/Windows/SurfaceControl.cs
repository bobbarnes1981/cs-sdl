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

		/// <summary>
		/// Constructor
		/// </summary>
		public SurfaceControl()
		{
			this.surface = new Surface(this.Width,this.Height);
			this.Image = this.surface.Bitmap;
		}

		/// <summary>
		/// The Surface of the control
		/// </summary>
		public Surface Surface
		{
			get
			{
				this.Image.Dispose();
				this.Image = surface.Bitmap;
				return surface;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				surface = value;
			}
		}
		
		/// <summary>
		/// Raises the OnResize event
		/// </summary>
		/// <param name="e">Contains the event data</param>
		protected override void OnResize(EventArgs e)
		{
			base.OnResize (e);
			this.surface = new Surface(this.Width,this.Height);
			this.surface.Update();
			this.Image.Dispose();
			this.Image = surface.Bitmap;
			SdlDotNet.Events.Add(new VideoResizeEventArgs(this.Width,this.Height));
		}

		/// <summary>
		/// Raises the SizeChanged event
		/// </summary>
		/// <param name="e">Contains the event data</param>
		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged (e);
			this.surface = new Surface(this.Width,this.Height);
			this.surface.Update();
			this.Image.Dispose();
			this.Image = surface.Bitmap;
			SdlDotNet.Events.Add(new VideoResizeEventArgs(this.Width,this.Height));
		}
		
		/// <summary>
		/// Raises the MouseDown event
		/// </summary>
		/// <param name="e">Contains the event data</param>
		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown (e);
			SdlDotNet.Events.Add(new MouseButtonEventArgs(SurfaceControl.ConvertMouseButtons(e), true, (short)e.X, (short)e.Y));
		}

		/// <summary>
		/// Raises the MouseUp event
		/// </summary>
		/// <param name="e">Contains the event data</param>
		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp (e);
			SdlDotNet.Events.Add(new MouseButtonEventArgs(SurfaceControl.ConvertMouseButtons(e), false, (short)e.X, (short)e.Y));
		}
		
		int lastX;
		int lastY;

		/// <summary>
		/// Raises the MouseMove event
		/// </summary>
		/// <param name="e">Contains the event data</param>
		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove (e);
			
			if (e.Button != MouseButtons.None)
			{
				SdlDotNet.Events.Add(new MouseMotionEventArgs(true, SurfaceControl.ConvertMouseButtons(e), (short)e.X, (short)e.Y, (short)(e.X - lastX), (short)(e.Y - lastY)));
			}
			lastX = e.X;
			lastY = e.Y;
		}

		private static MouseButton ConvertMouseButtons(MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				return MouseButton.PrimaryButton;
			}
			else if (e.Button == MouseButtons.Right)
			{
				return MouseButton.SecondaryButton;
			}
			else if (e.Button == MouseButtons.Middle)
			{
				return MouseButton.MiddleButton;
			}
			else
			{
				return MouseButton.None;
			}
		}
	}
}
