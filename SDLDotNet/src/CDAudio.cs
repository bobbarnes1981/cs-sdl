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
using System.Runtime.InteropServices;
using Tao.Sdl;

namespace SdlDotNet {

	/// <summary>
	/// Contains methods for playing audio CDs.
	/// Obtain an instance of this class by accessing the CDAudio property of the main Sdl object
	/// </summary>
	/// <remarks>
	/// Contains methods for playing audio CDs.
	/// </remarks>
	public sealed class CDAudio {
		static readonly CDAudio instance = new CDAudio();

		CDAudio()
		{
		}

		/// <summary>
		/// 
		/// </summary>
		public static CDAudio Instance 
		{
			get
			{
				if (Sdl.SDL_Init(Sdl.SDL_INIT_CDROM) != 0)
				{
					throw SdlException.Generate();
				}
				return instance;
			}
		}

		/// <summary>
		/// Gets the number of CD-ROM drives available on the system
		/// </summary>
		/// <remarks>
		/// </remarks>
		public int NumDrives 
		{
			get {
				int ret = Sdl.SDL_CDNumDrives();
				if (ret == -1)
				{
					throw SdlException.Generate();
				}
				return ret;
			}
		}

		/// <summary>
		/// Opens a CD-ROM drive for manipulation
		/// </summary>
		/// <param name="index">The number of the drive to open, from 0 - CDAudio.NumDrives</param>
		/// <returns>The CDDrive object representing the CD-ROM drive</returns>
		/// <remarks>
		/// Opens a CD-ROM drive for manipulation
		/// </remarks>
		public CDDrive OpenDrive(int index) {
			IntPtr cd = Sdl.SDL_CDOpen(index);
			if (cd == IntPtr.Zero)
			{
				throw SdlException.Generate();
			}
			return new CDDrive(cd, index);
		}

		/// <summary>
		/// Returns a platform-specific name for a CD-ROM drive
		/// </summary>
		/// <param name="index">The number of the drive</param>
		/// <returns>A platform-specific name, i.e. "D:\"</returns>
		/// <remarks>
		/// </remarks>
		public string DriveName(int index) 
		{
			if (index < 0 || index >= NumDrives)
			{
				throw new SdlException("Device index out of range");
			}
			return Sdl.SDL_CDName(index);
		}

		/// <summary>
		/// Converts frames (75th of a second) to minutes, seconds and leftover frames
		/// </summary>
		/// <param name="f">The number of frames to convert</param>
		/// <param name="M">A reference to a variable to receive the number of minutes</param>
		/// <param name="S">A reference to a variable to receive the number of seconds</param>
		/// <param name="F">A reference to a variable to receive the number of leftover frames</param>
		/// <remarks>
		/// </remarks>
		public static void FramesToMinSecFrames(int f, out int M, out int S, out int F) 
		{
			Sdl.FRAMES_TO_MSF(f, out M, out S, out F);
		}
		/// <summary>
		/// Converts minutes, seconds and frames into a frames value
		/// </summary>
		/// <param name="M">The number of minutes to convert</param>
		/// <param name="S">The number of seconds to convert</param>
		/// <param name="F">The number of frames to convert</param>
		/// <returns>The total number of frames</returns>
		/// <remarks>
		/// </remarks>
		public static int MinSecFramesToFrames(int M, int S, int F) 
		{
			return Sdl.MSF_TO_FRAMES(M, S, F);
		}
		/// <summary>
		/// Gets a static value indicating the number of frames in a second (75)
		/// </summary>
		/// <remarks>
		/// </remarks>
		public static int FramesPerSecond 
		{ 
			get 
			{ 
				return Sdl.CD_FPS; 
			} 
		}
	}
}
