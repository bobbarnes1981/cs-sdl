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
using Tao.Sdl;

namespace SdlDotNet 
{
	/// <summary>
	/// 
	/// </summary>
	public enum MovieStatus
	{
		/// <summary>
		/// 
		/// </summary>
		Playing = Smpeg.SMPEG_PLAYING,
		/// <summary>
		/// 
		/// </summary>
		Stopped = Smpeg.SMPEG_STOPPED,
		/// <summary>
		/// 
		/// </summary>
		Error = Smpeg.SMPEG_ERROR
	}

	/// <summary>
	/// Represents a movie mpg file.
	/// </summary>
	/// <remarks>
	/// Before instantiating an instance of Movie,
	/// you must call Mxier.Close() to turn off the default mixer.
	/// If you do not do this, any movie will play very slowly. 
	/// Smpeg uses a custom mixer for audio playback. 
	/// </remarks>
	public class Movie : BaseSdlResource, IDisposable
	{
		private IntPtr handle;
		private Smpeg.SMPEG_Info movieInfo;
		private bool disposed = false;

		internal Movie(IntPtr handle) 
		{
			this.handle = handle;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="file"></param>
		public Movie(string file)
		{
			this.handle =
				Smpeg.SMPEG_new(file, out movieInfo, 
				(int) SdlFlag.TrueValue);
			this.movieInfo = movieInfo;
		}

		/// <summary>
		/// 
		/// </summary>
		public IntPtr GetHandle
		{ 
			get
			{
				GC.KeepAlive(this);
				return handle; 
			}
		}

		/// <summary>
		/// Destroys the surface object and frees its memory
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
					CloseHandle(handle);
					GC.KeepAlive(this);
					this.disposed = true;
				}
				finally
				{
					base.Dispose(disposing);
				}
			}
		}

		/// <summary>
		/// Closes Music handle
		/// </summary>
		protected override void CloseHandle(IntPtr handleToClose) 
		{
			if (handleToClose != IntPtr.Zero)
			{
				Smpeg.SMPEG_delete(handleToClose);
				GC.KeepAlive(this);
				handleToClose = IntPtr.Zero;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public void EnableVideo()
		{
			Smpeg.SMPEG_enablevideo(this.handle, (int)SdlFlag.TrueValue);
		}

		/// <summary>
		/// 
		/// </summary>
		public void DisableVideo()
		{
			Smpeg.SMPEG_enablevideo(this.handle, (int)SdlFlag.FalseValue);
		}

		/// <summary>
		/// 
		/// </summary>
		public void EnableAudio()
		{
			Smpeg.SMPEG_enableaudio(this.handle, (int)SdlFlag.TrueValue);
		}

		/// <summary>
		/// 
		/// </summary>
		public void DisableAudio()
		{
			Smpeg.SMPEG_enableaudio(this.handle, (int)SdlFlag.FalseValue);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="surface"></param>
		public void Display(Surface surface)
		{
			Smpeg.SMPEG_setdisplay(
				this.handle, 
				surface.SurfacePointer, 
				IntPtr.Zero, null);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="volume"></param>
		public void AdjustVolume(int volume)
		{
			Smpeg.SMPEG_setvolume(handle, volume);
		}

		/// <summary>
		/// 
		/// </summary>
		public int Width
		{
			get
			{
				return movieInfo.width;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public int Height
		{
			get
			{
				return movieInfo.height;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public double TotalTime
		{
			get
			{
				return movieInfo.total_time;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public int Size
		{
			get
			{
				return movieInfo.total_size;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public double CurrentFps
		{
			get
			{
				return movieInfo.current_fps;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public double CurrentFrame
		{
			get
			{
				return movieInfo.current_frame;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public double CurrentTime
		{
			get
			{
				return movieInfo.current_time;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public int CurrentAudioFrame
		{
			get
			{
				return movieInfo.audio_current_frame;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string AudioString
		{
			get
			{
				return movieInfo.audio_string;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public bool HasAudio
		{
			get
			{
				if (movieInfo.has_audio != (int)SdlFlag.FalseValue)
				{
					return true;
				}
				else
				{
					return false;
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public bool IsPlaying
		{
			get
			{
				if (Smpeg.SMPEG_status(handle) == (int)MovieStatus.Playing)
				{
					return true;
				}
				else
				{
					return false;
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public bool IsStopped
		{
			get
			{
				if (Smpeg.SMPEG_status(handle) == (int)MovieStatus.Stopped)
				{
					return true;
				}
				else
				{
					return false;
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public bool HasVideo
		{
			get
			{
				if (movieInfo.has_video != (int)SdlFlag.FalseValue)
				{
					return true;
				}
				else
				{
					return false;
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public void Play()
		{
			Smpeg.SMPEG_play(handle);
		}

		/// <summary>
		/// 
		/// </summary>
		public void Stop()
		{
			Smpeg.SMPEG_stop(handle);
		}

		/// <summary>
		/// 
		/// </summary>
		public void Pause()
		{
			Smpeg.SMPEG_pause(handle);
		}

		/// <summary>
		/// 
		/// </summary>
		public void Rewind()
		{
			Smpeg.SMPEG_rewind(handle);
		}

		/// <summary>
		/// 
		/// </summary>
		public void Loop(bool repeat)
		{
			if (repeat)
			{
				Smpeg.SMPEG_loop(handle, (int) SdlFlag.TrueValue);
			}
			else
			{
				Smpeg.SMPEG_loop(handle, (int) SdlFlag.FalseValue);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="width"></param>
		/// <param name="height"></param>
		public void ScaleXY( int width, int height)
		{
			Smpeg.SMPEG_scaleXY(handle, width, height );
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="scalingFactor"></param>
		public void ScaleSize(int scalingFactor)
		{
			Smpeg.SMPEG_scale(handle, scalingFactor);
		}

		/// <summary>
		/// 
		/// </summary>
		public void ScaleDouble()
		{
			Smpeg.SMPEG_scale(handle, 2);
		}

		/// <summary>
		/// 
		/// </summary>
		public void ScaleNormal()
		{
			Smpeg.SMPEG_scale(handle, 1);
		}

		/// <summary>
		/// Move the video display area within the destination surface
		/// </summary>
		/// <param name="axisX"></param>
		/// <param name="axisY"></param>
		public void Move( int axisX, int axisY)
		{
			Smpeg.SMPEG_move(handle, axisX, axisY );
		}

		/// <summary>
		/// Move the video display area within the destination surface
		/// </summary>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <param name="axisX"></param>
		/// <param name="axisY"></param>
		public void DisplayRegion( 
			int width, int height, 
			int axisX, int axisY)
		{
			Smpeg.SMPEG_setdisplayregion(handle, axisX, axisY, width, height );
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="seconds"></param>
		public void SkipSeconds(float seconds)
		{
			Smpeg.SMPEG_skip(handle, seconds);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="bytes"></param>
		public void Seek(int bytes)
		{
			Smpeg.SMPEG_seek(handle, bytes);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="frameNumber"></param>
		public void RenderFrame(int frameNumber)
		{
			Smpeg.SMPEG_renderFrame(handle, frameNumber);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="surface"></param>
		/// <param name="frameNumber"></param>
		public void RenderFinalFrame(Surface surface, int frameNumber)
		{
			Smpeg.SMPEG_renderFinal(handle, surface.SurfacePointer, 0, 0);
		}

	}
}
