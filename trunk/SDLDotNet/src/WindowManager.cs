/*
 * $RCSfile$
 * Copyright (C) 2005 Christopher E. Granade (cgranade@greens.org)
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
using System.Reflection;

using Tao.Sdl;

namespace SdlDotNet
{

	/// <summary>
	/// 
	/// </summary>
	public struct Version {
		/// <summary>
		/// 
		/// </summary>
		public byte Major;
		/// <summary>
		/// 
		/// </summary>
		public byte Minor;
		/// <summary>
		/// 
		/// </summary>
		public byte Patch;
		
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override string ToString() {
			return String.Format("{0}.{1}.{2}", Major, Minor, Patch);
		}
		
		internal Version(Sdl.SDL_version taover)
		{
			Major = taover.major;
			Minor = taover.minor;
			Patch = taover.patch;
		}
	}
	
	/// <summary>
	/// 
	/// </summary>
	public struct WMInfo {
		
		#region OS-Neutral Members
		/// <summary>
		/// 
		/// </summary>
		public int     Data;
		/// <summary>
		/// 
		/// </summary>
		public Version WMVersion;
		#endregion
		
		#region Internal Constructors
		internal WMInfo(Sdl.SDL_SysWMinfo taoinf)
		{
			Data = taoinf.data;
			WMVersion = new Version(taoinf.version);
		}
		#endregion
	}
	
	/// <summary>
	/// This class provides access to the currently running window manager in an OS-dependant
	/// manner.
	/// </summary>
	public class WindowManager
	{
		
		/// <summary>
		/// This property allows access to a structure containing information about the window
		/// manager currently running.
		/// </summary>
		public static WMInfo Info 
		{
			get 
			{
				// Get the info struct from Tao.
				//Sdl.SDL_SysWMinfo inf = new Sdl.SDL_SysWMinfo();
				object inf = new object();
				//int result;
					
				//result = Sdl.SDL_GetWMInfo(ref inf);
				Sdl.SDL_GetWMInfo(ref inf);
				return new WMInfo((Sdl.SDL_SysWMinfo)inf);
				
				// branch for client OS.
			}
		}
		
	}
}
