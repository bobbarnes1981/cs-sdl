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
	/// 
	/// </summary>
	public struct CDFrames
	{
		int _frames;
		int _minutes;
		int _seconds;
		int _framesLeftover;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="frames">
		/// The number of frames to convert</param>
		public CDFrames(int frames)
		{
			_frames = frames;
			_minutes = 0;
			_seconds = 0;
			_framesLeftover = 0;
			this.FramesToTime(frames);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="minutes">
		/// A reference to a variable to receive the number of minutes
		/// </param>
		/// <param name="seconds">
		/// A reference to a variable to receive the number of seconds
		/// </param>
		public CDFrames(int minutes, int seconds) : this(minutes, seconds, 0)
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="minutes">
		/// A reference to a variable to receive the number of minutes
		/// </param>
		/// <param name="seconds">
		/// A reference to a variable to receive the number of seconds
		/// </param>
		/// <param name="framesLeftover">
		/// A reference to a variable to receive the number of leftover frames
		/// </param>
		public CDFrames(int minutes, int seconds, int framesLeftover)
		{
			_frames = 0;
			_minutes = minutes;
			_seconds = seconds;
			_framesLeftover = framesLeftover;
			_frames = this.TimeToFrames(minutes, seconds, _framesLeftover);
		}

		/// <summary>
		/// Converts frames (75th of a second) to minutes, 
		/// seconds and leftover frames
		/// </summary>
		/// <param name="frames">
		/// The number of frames to convert</param>
		private void FramesToTime(int frames) 
		{
			int minutes;
			int seconds;
			int framesLeftover;

			Sdl.FRAMES_TO_MSF(
				frames, out minutes, out seconds, out framesLeftover);
			this.Minutes = minutes;
			this.Seconds = seconds;
			this.FramesLeftover = framesLeftover;
		}

		/// <summary>
		/// Converts minutes, seconds and frames into a frames value
		/// </summary>
		/// <param name="minutes">The number of minutes to convert</param>
		/// <param name="seconds">The number of seconds to convert</param>
		/// <param name="framesLeftover">The number of frames to convert</param>
		/// <returns>The total number of frames</returns>
		public int TimeToFrames(
			int minutes, int seconds, int framesLeftover) 
		{
			return Sdl.MSF_TO_FRAMES(minutes, seconds, framesLeftover);
		}

		/// <summary>
		/// 
		/// </summary>
		public int Minutes
		{
			get
			{
				return _minutes;
			}
			set
			{
				if (value >= 0)
				{
					_minutes = value;
					this.TimeToFrames(value, _seconds, _framesLeftover);
				}
			}

		}

		/// <summary>
		/// 
		/// </summary>
		public int Seconds
		{
			get
			{
				return _seconds;
			}
			set 
			{
				if (value >= 0)
				{
					_seconds = value;
					_frames = this.TimeToFrames(_minutes, value, _framesLeftover);
					this.FramesToTime(_frames);
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public int FramesLeftover
		{
			get
			{
				return _framesLeftover;
			}
			set
			{
				if (value >=0)
				{
					_framesLeftover = value;
					_frames = this.TimeToFrames(_minutes, _seconds, value);
					this.FramesToTime(_frames);
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public int Frames
		{
			get
			{
				return _frames;
			}
			set
			{
				if (value >=0)
				{
					_frames = value;
					this.FramesToTime(value);
				}
			}
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

	/// <summary>
	/// Contains methods for playing audio CDs.
	/// Obtain an instance of this class by accessing 
	/// the CDAudio property of the main Sdl object
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
		public int NumberOfDrives 
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
		/// <param name="index">
		/// The number of the drive to open, from 0 - CDAudio.NumDrives
		/// </param>
		/// <returns>
		/// The CDDrive object representing the CD-ROM drive
		/// </returns>
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
			if (index < 0 || index >= NumberOfDrives)
			{
				throw new SdlException("Device index out of range");
			}
			return Sdl.SDL_CDName(index);
		}
	}
}
