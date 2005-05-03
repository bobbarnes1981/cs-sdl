/*
 * $RCSfile$
 * Copyright (C) 2004, 2005 David Hudson (jendave@yahoo.com)
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
	/// Represents a movie mpg file.
	/// </summary>
	/// <remarks>
	/// Before instantiating an instance of Movie,
	/// you must call Mxier.Close() to turn off the default mixer.
	/// If you do not do this, any movie will play very slowly. 
	/// Smpeg uses a custom mixer for audio playback. 
	/// </remarks>
	public class Movie : BaseSdlResource
	{
		private Smpeg.SMPEG_Info movieInfo;
		private bool disposed;

		internal Movie(IntPtr handle) 
		{
			base.Handle = handle;
		}

		/// <summary>
		/// Create movie object from file
		/// </summary>
		/// <param name="file"></param>
		public Movie(string file)
		{
			base.Handle =
				Smpeg.SMPEG_new(file, out movieInfo, 
				(int) SdlFlag.TrueValue);
			if (base.Handle == IntPtr.Zero)
			{
				throw MovieStatusException.Generate();
			}
			this.movieInfo = movieInfo;
		}

//		/// <summary>
//		/// 
//		/// </summary>
//		internal IntPtr GetHandle
//		{ 
//			get
//			{
//				GC.KeepAlive(this);
//				return handle; 
//			}
//		}

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
					CloseHandle();
					//GC.KeepAlive(this);
					GC.SuppressFinalize(this);
					this.disposed = true;
				}
				finally
				{
					base.Dispose(disposing);
					this.disposed = true;
				}
			}
		}

		/// <summary>
		/// Closes Music handle
		/// </summary>
		protected override void CloseHandle() 
		{
			if (base.Handle != IntPtr.Zero)
			{
				Smpeg.SMPEG_delete(base.Handle);
				GC.KeepAlive(this);
				base.Handle = IntPtr.Zero;
			}
		}

		/// <summary>
		/// Enable video during playback
		/// </summary>
		/// <remarks>Enabled by default</remarks>
		public void EnableVideo()
		{
			Smpeg.SMPEG_enablevideo(base.Handle, (int)SdlFlag.TrueValue);
		}

		/// <summary>
		/// Disable video during playback
		/// </summary>
		public void DisableVideo()
		{
			Smpeg.SMPEG_enablevideo(base.Handle, (int)SdlFlag.FalseValue);
		}

		/// <summary>
		/// Enable audio during playback
		/// </summary>
		/// <remarks>Enabled by default</remarks>
		public void EnableAudio()
		{
			Smpeg.SMPEG_enableaudio(base.Handle, (int)SdlFlag.TrueValue);
		}

		/// <summary>
		/// Disable audio during playback
		/// </summary>
		public void DisableAudio()
		{
			Smpeg.SMPEG_enableaudio(base.Handle, (int)SdlFlag.FalseValue);
		}

		/// <summary>
		/// Display video surface
		/// </summary>
		/// <param name="surface"></param>
		public void Display(Surface surface)
		{
			Smpeg.SMPEG_setdisplay(
				base.Handle, 
				surface.Handle, 
				IntPtr.Zero, null);
		}

		/// <summary>
		/// Sets the volume for the movie.
		/// </summary>
		/// <param name="volume"></param>
		public void AdjustVolume(int volume)
		{
			Smpeg.SMPEG_setvolume(base.Handle, volume);
		}

		/// <summary>
		/// Returns Size of movie
		/// </summary>
		public System.Drawing.Size Size
		{
			get
			{
				return new System.Drawing.Size(this.movieInfo.width, this.movieInfo.height);
			}
		}

		/// <summary>
		/// Width of movie
		/// </summary>
		public int Width
		{
			get
			{
				return movieInfo.width;
			}
		}

		/// <summary>
		/// Height of movie
		/// </summary>
		public int Height
		{
			get
			{
				return movieInfo.height;
			}
		}

		/// <summary>
		/// Returns the length of the movie in seconds
		/// </summary>
		public double Length
		{
			get
			{
				return movieInfo.total_time;
			}
		}

		/// <summary>
		/// Get the movie file size in bytes.
		/// </summary>
		public int FileSize
		{
			get
			{
				return movieInfo.total_size;
			}
		}

		/// <summary>
		/// Returns current frames per second
		/// </summary>
		public double CurrentFps
		{
			get
			{
				return movieInfo.current_fps;
			}
		}

		/// <summary>
		/// Returns current frame number in movie.
		/// </summary>
		public double CurrentFrame
		{
			get
			{
				return movieInfo.current_frame;
			}
		}

		/// <summary>
		/// Returns current offset??? in movie.
		/// </summary>
		public double CurrentOffset
		{
			get
			{
				return movieInfo.current_offset;
			}
		}

		/// <summary>
		/// Returns the current time in the movie in seconds.
		/// </summary>
		public double CurrentTime
		{
			get
			{
				return movieInfo.current_time;
			}
		}

		/// <summary>
		/// Returns current audio frame
		/// </summary>
		public int CurrentAudioFrame
		{
			get
			{
				return movieInfo.audio_current_frame;
			}
		}

		/// <summary>
		/// Returns audio string
		/// </summary>
		public string AudioString
		{
			get
			{
				return movieInfo.audio_string;
			}
		}

		/// <summary>
		/// Returns true if the movie has a valid audio stream.
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
		/// If true, movie is playing
		/// </summary>
		public bool IsPlaying
		{
			get
			{
				if (Smpeg.SMPEG_status(base.Handle) == (int)MovieStatus.Playing)
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
		/// If true, movie has been stopped
		/// </summary>
		public bool IsStopped
		{
			get
			{
				if (Smpeg.SMPEG_status(base.Handle) == (int)MovieStatus.Stopped)
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
		/// Returns true if the movie has a valid video stream.
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
		/// Starts playback of a movie.
		/// </summary>
		public void Play()
		{
			this.Loop(false);
			Smpeg.SMPEG_play(base.Handle);
		}

		/// <summary>
		/// Starts playback of a movie.
		/// </summary>
		/// <param name="repeat">Loop movie while playing</param>
		public void Play(bool repeat)
		{
			this.Loop(repeat);
			Smpeg.SMPEG_play(base.Handle);
		}

		/// <summary>
		/// Stops playback of a movie.
		/// </summary>
		public void Stop()
		{
			Smpeg.SMPEG_stop(base.Handle);
		}

		/// <summary>
		/// This pauses playback of the movie
		/// </summary>
		public void Pause()
		{
			Smpeg.SMPEG_pause(base.Handle);
		}

		/// <summary>
		/// Sets the movie playback position to the start of the movie.
		/// </summary>
		public void Rewind()
		{
			Smpeg.SMPEG_rewind(base.Handle);
		}

		/// <summary>
		/// Loop movie
		/// </summary>
		private void Loop(bool repeat)
		{
			if (repeat)
			{
				Smpeg.SMPEG_loop(base.Handle, (int) SdlFlag.TrueValue);
			}
			else
			{
				Smpeg.SMPEG_loop(base.Handle, (int) SdlFlag.FalseValue);
			}
		}

		/// <summary>
		/// Resize movie
		/// </summary>
		/// <param name="width"></param>
		/// <param name="height"></param>
		public void ScaleXY( int width, int height)
		{
			Smpeg.SMPEG_scaleXY(base.Handle, width, height );
		}

		/// <summary>
		/// Change size of movie by the same amount in both axes
		/// </summary>
		/// <param name="scalingFactor"></param>
		public void ScaleSize(int scalingFactor)
		{
			Smpeg.SMPEG_scale(base.Handle, scalingFactor);
		}

		/// <summary>
		/// Double size of movie
		/// </summary>
		public void ScaleDouble()
		{
			Smpeg.SMPEG_scale(base.Handle, 2);
		}

		/// <summary>
		/// Return size to normal
		/// </summary>
		public void ScaleNormal()
		{
			Smpeg.SMPEG_scale(base.Handle, 1);
		}

		/// <summary>
		/// Move the video display area within the destination surface
		/// </summary>
		/// <param name="axisX"></param>
		/// <param name="axisY"></param>
		public void Move( int axisX, int axisY)
		{
			Smpeg.SMPEG_move(base.Handle, axisX, axisY );
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
			Smpeg.SMPEG_setdisplayregion(base.Handle, axisX, axisY, width, height );
		}

		/// <summary>
		/// Skip forward a certain number of seconds
		/// </summary>
		/// <param name="seconds"></param>
		public void Skip(float seconds)
		{
			Smpeg.SMPEG_skip(base.Handle, seconds);
		}

		/// <summary>
		/// Seeks a specified number of bytes forward in the movie stream.
		/// </summary>
		/// <param name="bytes"></param>
		public void Seek(int bytes)
		{
			Smpeg.SMPEG_seek(base.Handle, bytes);
		}

		/// <summary>
		/// Renders specified frame of movie.
		/// </summary>
		/// <param name="frameNumber"></param>
		public void RenderFrame(int frameNumber)
		{
			Smpeg.SMPEG_renderFrame(base.Handle, frameNumber);
		}

		/// <summary>
		/// Renders specified frame of movie.
		/// </summary>
		public void RenderFirstFrame()
		{
			Smpeg.SMPEG_renderFrame(base.Handle, 0);
		}

		/// <summary>
		/// Renders final frame of movie and puts it on a surface
		/// </summary>
		/// <param name="surface"></param>
		public void RenderFinalFrame(Surface surface)
		{
			Smpeg.SMPEG_renderFinal(base.Handle, surface.Handle, 0, 0);
		}

	}
}
