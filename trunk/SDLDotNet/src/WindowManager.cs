/*
 * $RCSfile$
 * Copyright (C) 2004 David Hudson (jendave@yahoo.com)
 * Copyright (C) 2003 Will Weisser (ogl@9mm.com)
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
using Tao.Sdl;

namespace SdlDotNet {
	/// <summary>
	/// Contains methods for interacting with the window title frame and for grabbing input focus.
	/// These methods do not need a windowed display to be called.
	/// You can obtain an instance of this class by accessing the WindowManager property of the main Sdl
	/// object.
	/// </summary>
	public sealed class WindowManager {

		static readonly WindowManager instance = new WindowManager();

		WindowManager()
		{
		}

		/// <summary>
		/// 
		/// </summary>
		public static WindowManager Instance
		{
			get
			{
				if (Sdl.SDL_Init(Sdl.SDL_INIT_VIDEO)!= 0)
				{
					throw SdlException.Generate();
				}
				return instance;
			}
		}

		/// <summary>
		/// gets or sets the text for the current window
		/// </summary>
		public string Caption {
			get
			{
				string ret;
				string dummy;

				Sdl.SDL_WM_GetCaption(out ret, out dummy);
				return ret;
			}
			set
			{
				Sdl.SDL_WM_SetCaption(value, "");
			}
		}
		/// <summary>
		/// sets the icon for the current window
		/// </summary>
		/// <param name="icon">the surface containing the image</param>
		public void SetIcon(Surface icon) {
			Sdl.SDL_WM_SetIcon(icon.GetPtr(), null);
		}
		/// <summary>
		/// Iconifies (minimizes) the current window
		/// </summary>
		/// <returns>True if the action succeeded, otherwise False</returns>
		public bool IconifyWindow() {
			return (Sdl.SDL_WM_IconifyWindow() != 0);
		}
		/// <summary>
		/// Forces keyboard focus and prevents the mouse from leaving the window
		/// </summary>
		public void GrabInput() {
			Sdl.SDL_WM_GrabInput(Sdl.SDL_GrabMode.SDL_GRAB_ON);
		}
		/// <summary>
		/// Releases keyboard and mouse focus from a previous call to GrabInput()
		/// </summary>
		public void ReleaseInput() {
			Sdl.SDL_WM_GrabInput(Sdl.SDL_GrabMode.SDL_GRAB_OFF);
		}
	}
}
