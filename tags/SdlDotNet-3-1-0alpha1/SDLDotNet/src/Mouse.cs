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

using System;
using System.Drawing;

using Tao.Sdl;

namespace SdlDotNet
{
	/// <summary>
	/// Mouse.
	/// </summary>
	public sealed class Mouse
	{
		static readonly Mouse instance = new Mouse();

//		static Mouse()
//		{
//		}

		Mouse()
		{
		}

		internal static Mouse Instance
		{
			get
			{
				return instance;
			}
		}

		/// <summary>
		/// Shows the mouse cursor
		/// </summary>
		public void ShowCursor(bool visible) 
		{
			if (visible)
			{
				Sdl.SDL_ShowCursor(Sdl.SDL_ENABLE);
			}
			else
			{
				Sdl.SDL_ShowCursor(Sdl.SDL_DISABLE);
			}
		}

		/// <summary>
		/// Queries the current cursor state
		/// </summary>
		/// <returns>True if the cursor is visible, otherwise False</returns>
		public bool IsCursorVisible() 
		{
			return (Sdl.SDL_ShowCursor(Sdl.SDL_QUERY) == Sdl.SDL_ENABLE);
		}

		/// <summary>
		/// Move the mouse cursor to a specific location
		/// </summary>
		/// <param name="x">The X coordinate</param>
		/// <param name="y">The Y coordinate</param>
		public static void MoveCursor(short x, short y) 
		{
			Sdl.SDL_WarpMouse(x, y);
		}

		/// <summary>
		/// Returns current mouse position
		/// </summary>
		/// <returns></returns>
		public Point MousePosition
		{
			get
			{
				int x;
				int y;
				Sdl.SDL_GetMouseState(out x, out y);
				return new Point(x, y);
			}
		}

		/// <summary>
		/// Returns change in mouse position
		/// </summary>
		/// <returns></returns>
		public Point MousePositionChange
		{
			get
			{
				int x;
				int y;
				Sdl.SDL_GetRelativeMouseState(out x, out y);
				return new Point(x, y);
			}
		}

		/// <summary>
		/// Returns true if app has mouse focus
		/// </summary>
		public bool HasMouseFocus
		{
			get
			{
				return (Sdl.SDL_GetAppState() & Sdl.SDL_APPMOUSEFOCUS) !=0;
			}
		}

		/// <summary>
		/// Gets the pressed or released state of a mouse button
		/// </summary>
		/// <param name="button">The mouse button to check</param>
		/// <returns>
		/// If the button is pressed, returns True, otherwise returns False
		/// </returns>
		public bool IsButtonPressed(MouseButton button) 
		{
			int dummyX;
			int dummyY;
			return (Sdl.SDL_GetMouseState(out dummyX, out dummyY) & Sdl.SDL_BUTTON((byte)button)) != 0;
		}
	}
}
