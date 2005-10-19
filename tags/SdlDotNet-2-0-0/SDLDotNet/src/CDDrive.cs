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

	#region CDstatus
	/// <summary>
	/// The possible states which a CD-ROM drive can be in.
	/// </summary>
	public enum CDStatus 
	{
		/// <summary>
		/// The CD tray is empty.
		/// </summary>
		TrayEmpty = Sdl.CD_TRAYEMPTY,
		/// <summary>
		/// The CD has stopped playing.
		/// </summary>
		Stopped = Sdl.CD_STOPPED,
		/// <summary>
		/// The CD is playing.
		/// </summary>
		Playing = Sdl.CD_PLAYING,
		/// <summary>
		/// The CD has been paused.
		/// </summary>
		Paused = Sdl.CD_PAUSED,
		/// <summary>
		/// An error occured while getting the status.
		/// </summary>
		Error = Sdl.CD_ERROR
	}
	#endregion CDstatus

	/// <summary>
	/// Represents a CD-ROM drive on the system
	/// </summary>
	public class CDDrive : BaseSdlResource
	{
		private bool disposed = false;
		private IntPtr handle;
		private int index;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="handle"></param>
		/// <param name="index"></param>
		public CDDrive(IntPtr handle, int index) 
		{
			this.handle = handle;
			this.index = index;
		}

		/// <summary>
		/// The drive number
		/// </summary>
		public int Index 
		{ 
			get 
			{ 
				return this.index; 
			} 
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="disposing"></param>
		protected override void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				try
				{
					if (disposing)
					{

					}
					CloseHandle(ref handle);
					this.disposed = true;
				}
				finally
				{
					base.Dispose(disposing);
				}
			}
		}

		/// <summary>
		/// Closes CDDrive handle
		/// </summary>
		protected override void CloseHandle(ref IntPtr handleToClose) 
		{
			Sdl.SDL_CDClose(handleToClose);
			GC.KeepAlive(this);
			handle = IntPtr.Zero;
		}

		/// <summary>
		/// Gets the current drive status
		/// </summary>
		public CDStatus Status 
		{
			get 
			{ 
				CDStatus status = (CDStatus) Sdl.SDL_CDStatus(this.handle);
				return (status); 
			}
		}

//		/// <summary>
//		/// Plays the CD in the drive
//		/// </summary>
//		/// <param name="startmins">The number of minutes on the CD to start playing from</param>
//		/// <param name="startsecs">The number of seconds on the CD to start playing from</param>
//		/// <param name="lengthmins">The number of minutes on the CD to play</param>
//		/// <param name="lengthsecs">The number of seconds on the CD to play</param>
//		public void Play(int startmins, int startsecs, int lengthmins, int lengthsecs) {
//			if (Sdl.SDL_CDPlay(this.handle, CDAudio.MinSecFramesToFrames(startmins, startsecs, 0),
//				CDAudio.MinSecFramesToFrames(lengthmins, lengthsecs, 0)) == -1)
//				throw SdlException.Generate();
//		}
//		/// <summary>
//		/// Plays the CD in the drive
//		/// </summary>
//		/// <param name="startframes">The number of frames (75th of a second increments) on the CD to start playing from</param>
//		/// <param name="lengthframes">The number of frames (75th of a second increments) to play</param>
//		public void Play(int startframes, int lengthframes) {
//			if (Sdl.SDL_CDPlay(this.handle, startframes, lengthframes) == -1)
//				throw SdlException.Generate();
//		}

		/// <summary>
		/// Plays the tracks on a CD in the drive
		/// </summary>
		/// <param name="startTrack">
		/// The starting track to play (numbered 0-99)
		/// </param>
		/// <param name="numberOfTracks">
		/// The number of tracks to play
		/// </param>
		public void PlayTracks(int startTrack, int numberOfTracks) 
		{
			int result = Sdl.SDL_CDPlayTracks(
				this.handle, startTrack, 0, numberOfTracks, 0);
			GC.KeepAlive(this);
			if (result== -1)
			{
				throw SdlException.Generate();
			}
		}

		/// <summary>
		/// Plays the tracks on a CD in the drive
		/// </summary>
		/// <param name="startTrack">
		/// The starting track to play 
		/// (numbered 0-99)
		/// </param>
		/// <param name="startFrame">
		/// The frame (75th of a second increment) offset from the 
		/// starting track to play from
		/// </param>
		/// <param name="numberOfTracks">
		/// The number of tracks to play
		/// </param>
		/// <param name="numberOfFrames">
		/// The frame (75th of a second increment) offset after the last 
		/// track to stop playing after
		/// </param>
		public void PlayTracks(
			int startTrack, int startFrame, 
			int numberOfTracks, int numberOfFrames) 
		{
			int result = Sdl.SDL_CDPlayTracks(
				this.handle, startTrack, startFrame, 
				numberOfTracks, numberOfFrames);
			GC.KeepAlive(this);
			if (result== -1)
			{
				throw SdlException.Generate();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="startTrack"></param>
		public void PlayTracks(int startTrack)
		{
			int result = Sdl.SDL_CDPlayTracks(
				this.handle, startTrack, 0, 0, 0);
			GC.KeepAlive(this);
			if (result== -1)
			{
				throw SdlException.Generate();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public void Play()
		{
			int result = Sdl.SDL_CDPlayTracks(
				this.handle, 0, 0, 0, 0);
			GC.KeepAlive(this);
			if (result == -1)
			{
				throw SdlException.Generate();
			}
		}

		/// <summary>
		/// Pauses the CD in this drive
		/// </summary>
		public void Pause() 
		{
			int result = Sdl.SDL_CDPause(this.handle);
			GC.KeepAlive(this);
			if (result == -1)
			{
				throw SdlException.Generate();
			}
		}
		/// <summary>
		/// Resumes a previously paused CD in this drive
		/// </summary>
		public void Resume() 
		{
			int result = Sdl.SDL_CDResume(this.handle);
			GC.KeepAlive(this);
			if (result == -1)
			{
				throw SdlException.Generate();
			}
		}
		/// <summary>
		/// Stops playing the CD in this drive
		/// </summary>
		public void Stop() 
		{
			int result = Sdl.SDL_CDStop(this.handle);
			GC.KeepAlive(this);
			if ( result == -1)
			{
				throw SdlException.Generate();
			}

		}
		/// <summary>
		/// Ejects this drive
		/// </summary>
		public void Eject() 
		{
			int result = Sdl.SDL_CDEject(this.handle);
			GC.KeepAlive(this);
			if ( result == -1)
			{
				throw SdlException.Generate();
			}
		}

		/// <summary>
		/// Gets the number of tracks in the currently inserted CD
		/// </summary>
		public int NumberOfTracks 
		{
			get 
			{
				int result = Sdl.CD_INDRIVE((int)this.Status);
				GC.KeepAlive(this);
				if (result == 1)
				{
					Sdl.SDL_CD cd = 
						(Sdl.SDL_CD)Marshal.PtrToStructure(
						this.handle, typeof(Sdl.SDL_CD));
					return cd.numtracks;
				}
				else 
				{ 
					return 0;
				}
				
			}
		}
		/// <summary>
		/// Gets the currently playing track
		/// </summary>
		public int CurrentTrack 
		{
			get 
			{
				Sdl.SDL_CDStatus(this.handle);
				GC.KeepAlive(this);
				Sdl.SDL_CD cd = 
					(Sdl.SDL_CD)Marshal.PtrToStructure(
					this.handle, typeof(Sdl.SDL_CD));
				
				return cd.cur_track;
			}
		}
		/// <summary>
		/// Gets the currently playing frame (75th of a second increment)
		/// </summary>
		public int CurrentFrame 
		{
			get 
			{
				Sdl.SDL_CDStatus(this.handle);
				GC.KeepAlive(this);
				Sdl.SDL_CD cd = 
					(Sdl.SDL_CD)Marshal.PtrToStructure(
					this.handle, typeof(Sdl.SDL_CD));
				return cd.cur_frame;
			}
		}
	}
}