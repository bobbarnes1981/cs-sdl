/*
 * $RCSfile$
 * Copyright (C) 2003 Lucas Maloney
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

using System.Runtime.InteropServices;
using System.Drawing;
using Tao.Sdl;

namespace SdlDotNet
{
	/// <summary>
	/// 
	/// </summary>
	public struct SdlColor
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="color"></param>
		/// <returns></returns>
		public static Sdl.SDL_Color ConvertColor(Color color)
		{
			return new Sdl.SDL_Color(color.R, color.G, color.B);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="color"></param>
		/// <returns></returns>
		public static Color ConvertColor(Sdl.SDL_Color color)
		{
			return Color.FromArgb(0, color.r, color.g, color.b);
		}
	}
}
