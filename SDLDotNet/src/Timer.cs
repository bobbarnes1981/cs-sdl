/*
 * $RCSfile$
 * Copyright (C) 2004 David Hudson (jendave@yahoo.com)
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

namespace SdlDotNet
{
	/// <summary>
	/// Summary description for Timer.
	/// </summary>
	public class Timer
	{
		static readonly Timer instance = new Timer();

		static Timer()
		{
		}

		Timer()
		{
			Initialize();
		}

		/// <summary>
		/// 
		/// </summary>
		public static void Initialize()
		{
			if (Sdl.SDL_Init(Sdl.SDL_INIT_TIMER) != (int) SdlFlag.Success)
			{
				throw SdlException.Generate();
			}
		}

		/// <summary>
		/// Gets the number of ticks since Sdl was initialized.  
		/// This is not a high-resolution timer.
		/// </summary>
		public static int Ticks 
		{
			get
			{
				return Sdl.SDL_GetTicks();
			}
		}

		/// <summary>
		/// Wait a number of milliseconds.
		/// </summary>
		/// <param name="delayTime">Delay time in milliseconds</param>
		public static void Delay(int delayTime)
		{
			Sdl.SDL_Delay(delayTime);
		}
	}
}
