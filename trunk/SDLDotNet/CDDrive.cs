/*
 * $RCSfile$
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

namespace SDLDotNet {
	/// <summary>
	/// Indicates the current CD Status
	/// </summary>
	public enum CDStatus {
		/// <summary>
		/// The CD tray is empty
		/// </summary>
		TrayEmpty,
		/// <summary>
		/// Playing is stopped
		/// </summary>
		Stopped,
		/// <summary>
		/// CD is playing 
		/// </summary>
		Playing,
		/// <summary>
		/// CD is paused
		/// </summary>
		Paused,
		/// <summary>
		/// An error occured while fetching the status
		/// </summary>
		Error = -1
	}

	/// <summary>
	/// Represents a CD-ROM drive on the system
	/// </summary>
	public class CDDrive : IDisposable {
		private bool _disposed;
		private IntPtr _handle;
		private int _index;

		internal CDDrive(IntPtr handle, int index) {
			_disposed = false;
			_handle = handle;
			_index = index;
		}

		/// <protected/>
		~CDDrive() {
			Natives.SDL_CDClose(_handle);
		}

		/// <summary>
		/// Closes and destroys this CDDrive object
		/// </summary>
		public void Dispose() {
			if (!_disposed) {
				_disposed = true;
				Natives.SDL_CDClose(_handle);
				GC.SuppressFinalize(this);
			}
		}

		/// <summary>
		/// The drive number
		/// </summary>
		public int Index { get { return _index; } }

		/// <summary>
		/// Closes and destroys this CDDrive object
		/// </summary>
		public void Close() {
			Dispose();
		}

		/// <summary>
		/// Gets the current drive status
		/// </summary>
		public CDStatus Status {
			get { return (CDStatus)Natives.SDL_CDStatus(_handle); }
		}

		/// <summary>
		/// Plays the CD in the drive
		/// </summary>
		/// <param name="startmins">The number of minutes on the CD to start playing from</param>
		/// <param name="startsecs">The number of seconds on the CD to start playing from</param>
		/// <param name="lengthmins">The number of minutes on the CD to play</param>
		/// <param name="lengthsecs">The number of seconds on the CD to play</param>
		public void Play(int startmins, int startsecs, int lengthmins, int lengthsecs) {
			if (Natives.SDL_CDPlay(_handle, CDAudio.MinSecFramesToFrames(startmins, startsecs, 0),
				CDAudio.MinSecFramesToFrames(lengthmins, lengthsecs, 0)) == -1)
				throw SDLException.Generate();
		}
		/// <summary>
		/// Plays the CD in the drive
		/// </summary>
		/// <param name="startframes">The number of frames (75th of a second increments) on the CD to start playing from</param>
		/// <param name="lengthframes">The number of frames (75th of a second increments) to play</param>
		public void Play(int startframes, int lengthframes) {
			if (Natives.SDL_CDPlay(_handle, startframes, lengthframes) == -1)
				throw SDLException.Generate();
		}
		/// <summary>
		/// Plays the tracks on a CD in the drive
		/// </summary>
		/// <param name="starttrack">The starting track to play (numbered 0-99)</param>
		/// <param name="numtracks">The number of tracks to play</param>
		public void PlayTracks(int starttrack, int numtracks) {
			if (Natives.SDL_CDPlayTracks(_handle, starttrack, 0, numtracks, 0) == -1)
				throw SDLException.Generate();
		}
		/// <summary>
		/// Plays the tracks on a CD in the drive
		/// </summary>
		/// <param name="starttrack">The starting track to play (numbered 0-99)</param>
		/// <param name="startframe">The frame (75th of a second increment) offset from the starting track to play from</param>
		/// <param name="numtracks">The number of tracks to play</param>
		/// <param name="numframes">The frame (75th of a second increment) offset after the last track to stop playing after</param>
		public void PlayTracks(int starttrack, int startframe, int numtracks, int numframes) {
			if (Natives.SDL_CDPlayTracks(_handle, starttrack, startframe, numtracks, numframes) == -1)
				throw SDLException.Generate();
		}
		/// <summary>
		/// Pauses the CD in this drive
		/// </summary>
		public void Pause() {
			if (Natives.SDL_CDPause(_handle) == -1)
				throw SDLException.Generate();
		}
		/// <summary>
		/// Resumes a previously paused CD in this drive
		/// </summary>
		public void Resume() {
			if (Natives.SDL_CDResume(_handle) == -1)
				throw SDLException.Generate();
		}
		/// <summary>
		/// Stops playing the CD in this drive
		/// </summary>
		public void Stop() {
			if (Natives.SDL_CDStop(_handle) == -1)
				throw SDLException.Generate();
		}
		/// <summary>
		/// Ejects this drive
		/// </summary>
		public void Eject() {
			if (Natives.SDL_CDEject(_handle) == -1)
				throw SDLException.Generate();
		}

		/// <summary>
		/// Gets the number of tracks in the currently inserted CD
		/// </summary>
		public int NumTracks {
			get {
				Natives.SDL_CDStatus(_handle);
				Natives.SDL_CD cd = (Natives.SDL_CD)Marshal.PtrToStructure(_handle, typeof(Natives.SDL_CD));
				return cd.numtracks;
			}
		}
		/// <summary>
		/// Gets the currently playing track
		/// </summary>
		public int CurrentTrack {
			get {
				Natives.SDL_CDStatus(_handle);
				Natives.SDL_CD cd = (Natives.SDL_CD)Marshal.PtrToStructure(_handle, typeof(Natives.SDL_CD));
				return cd.cur_track;
			}
		}
		/// <summary>
		/// Gets the currently playing frame (75th of a second increment)
		/// </summary>
		public int CurrentFrame {
			get {
				Natives.SDL_CDStatus(_handle);
				Natives.SDL_CD cd = (Natives.SDL_CD)Marshal.PtrToStructure(_handle, typeof(Natives.SDL_CD));
				return cd.cur_frame;
			}
		}
	}
}
