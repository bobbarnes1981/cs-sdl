/*
 * $RCSfile$
 * Copyright (C) 2005 Rob Loach (http://www.robloach.net)
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
using System.Threading;

using Tao.Sdl;

namespace SdlDotNet
{
	/// <summary>
	/// Event arguments for a Framerate tick.
	/// </summary>
	public class TickEventArgs : SdlEventArgs 
	{
		private int m_LastTick;
		private int m_Tick;
		private int m_FPS;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="tick">
		/// The current tick.
		/// </param>
		/// <param name="lastTick">
		/// The tick count that it was at last frame.
		/// </param>
		/// <param name="fps">Frames per second</param>
		public TickEventArgs(int tick, int lastTick, int fps)
		{
			m_Tick = tick;
			m_LastTick = lastTick;
			m_FPS = fps;
		}

		/// <summary>
		/// Gets when the last frame tick occurred.
		/// </summary>
		public int LastTick
		{
			get
			{
				return m_LastTick;
			}
		}
		
		/// <summary>
		/// Gets the FPS as of the event call. Events.FPS is an alternative.
		/// </summary>
		public int FPS
		{
			get
			{
				return m_FPS;
			}
		}

		/// <summary>
		/// Gets the current SDL tick time.
		/// </summary>
		public int Tick
		{
			get
			{
				return m_Tick;
			}
		}
		
		/// <summary>
		/// Gets the difference in time between the 
		/// current tick and the last tick.
		/// </summary>
		public int TicksElapsed
		{
			get
			{
				return m_Tick - m_LastTick;
			}
		}

		/// <summary>
		/// Seconds elapsed between the last tick and the current tick
		/// </summary>
		public float SecondsElapsed
		{
			get
			{
				return (this.TicksElapsed / 1000);
			}
		}

		/// <summary>
		/// Calculates a rate, adjusted for seconds. This method takes the
		/// rate that something should happen every second and adjusts it
		/// by the amount of time since the last tick (typically less than
		/// the rate, but potential by more if there are a lot of skipped
		/// cycles).
		/// </summary>
		public int RatePerSecond(int rate)
		{
			double off = (double) LastTick / 1000000.0 * (double) rate;
			return (int) off;
		}

		/// <summary>
		/// Calculates a rate, adjusted for seconds. This method takes the
		/// rate that something should happen every second and adjusts it
		/// by the amount of time since the last tick (typically less than
		/// the rate, but potential by more if there are a lot of skipped
		/// cycles).
		/// </summary>
		public double RatePerSecond(double rate)
		{
			return (double) LastTick / 1000000.0 * (double) rate;
		}
	}
}