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
using System.Globalization;

using Tao.Sdl;

namespace SdlDotNet {
	/// <summary>
	/// 
	/// </summary>
	public struct CDFrames
	{
		int frames;
		int minutes;
		int seconds;
		int framesLeftover;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="frames">
		/// The number of frames to convert</param>
		public CDFrames(int frames)
		{
			this.frames = frames;
			this.minutes = 0;
			this.seconds = 0;
			this.framesLeftover = 0;
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
			this.frames = 0;
			this.minutes = minutes;
			this.seconds = seconds;
			this.framesLeftover = framesLeftover;
			this.frames = this.TimeToFrames(minutes, seconds, framesLeftover);
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
				return minutes;
			}
			set
			{
				if (value >= 0)
				{
					minutes = value;
					this.TimeToFrames(value, seconds, framesLeftover);
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
				return seconds;
			}
			set 
			{
				if (value >= 0)
				{
					seconds = value;
					frames = this.TimeToFrames(minutes, value, framesLeftover);
					this.FramesToTime(frames);
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
				return framesLeftover;
			}
			set
			{
				if (value >=0)
				{
					framesLeftover = value;
					frames = this.TimeToFrames(minutes, seconds, value);
					this.FramesToTime(frames);
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
				return frames;
			}
			set
			{
				if (value >=0)
				{
					frames = value;
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
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return String.Format(
				CultureInfo.CurrentCulture, 
				"({0},{1}, {2})", frames, minutes, seconds, framesLeftover);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			if (obj.GetType() != typeof(CDFrames))
				return false;
                
			CDFrames cdFrames = (CDFrames)obj;   
			return (
				(this.minutes == cdFrames.minutes) &&
				(this.seconds == cdFrames.seconds) && 
				(this.frames == cdFrames.frames) && 
				(this.framesLeftover == cdFrames.framesLeftover));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="cdFrames1"></param>
		/// <param name="cdFrames2"></param>
		/// <returns></returns>
		public static bool operator== (CDFrames cdFrames1, CDFrames cdFrames2)
		{
			return (
				(cdFrames1.minutes == cdFrames2.minutes) && 
				(cdFrames1.seconds == cdFrames2.seconds) && 
				(cdFrames1.frames == cdFrames2.frames)&& 
				(cdFrames1.framesLeftover == cdFrames2.framesLeftover));
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="cdFrames1"></param>
		/// <param name="cdFrames2"></param>
		/// <returns></returns>
		public static bool operator!= (CDFrames cdFrames1, CDFrames cdFrames2)
		{
			return !(cdFrames1 == cdFrames2);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode()
		{
			return minutes ^ seconds ^ frames ^ framesLeftover;

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
	public sealed class CDAudio 
	{
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
		public CDDrive OpenDrive(int index) 
		{
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
