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
	public class SurfaceControl : PictureBox
	{
		Surface surface;

		/// <summary>
		/// 
		/// </summary>
		public SurfaceControl()
		{
			surface = new Surface(0,0);
		}

		/// <summary>
		/// Get/set the transparency of the image.  
		/// </summary>
		[Category("Sdl Properties"), Description("Set Transparency")]
		public bool Transparent
		{
			get 
			{
				return surface.Transparent;
			}
			set	
			{
				surface.Transparent = value;
			}
		}

		/// <summary>
		/// Get/set the Alpha flags of the image.
		/// </summary>
		[Category("Sdl Properties"), Description("Set Alpha flags")]
		public Alphas AlphaFlags
		{
			get 
			{
				return surface.AlphaFlags;
			}
			set	
			{
				surface.AlphaFlags = value;
			}
		}
	}
}
