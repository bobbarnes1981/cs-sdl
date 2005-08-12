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
using Tao.Sdl;
using System.Threading;

namespace SdlDotNet
{
	/// <summary>
	/// 
	/// </summary>
	public delegate void FramerateTickEventHandler(object sender, FramerateTickEventArgs e);

	/// <summary>
	/// Summary description for Framerate.
	/// </summary>
	public sealed class Framerate
	{
		private static int m_Framecount;
		private static float m_Framerate;
		private static int m_LastTick;
		private static int m_Rate;
		private static int m_FPS;

		private static Thread m_Thread;

		static Framerate()
		{
			Rate = 60;
			m_LastTick = 0;
			m_Thread = new Thread(new ThreadStart(ThreadTicker));
			m_Thread.Priority = ThreadPriority.Normal;
			m_Thread.IsBackground = true;
			m_Thread.Name = "SDL.NET - Framerate Manager";
		}

		/// <summary>
		/// Starts the framerate ticker. Must be called to start the manager interface.
		/// </summary>
		public static void StartTicker()
		{
			Thread.Sleep(1); // Give a gap so that any other initialization threads can catch up.
			m_Thread.Start();
		}

		/// <summary>
		/// Gets the current FPS and sets the wanted framerate.
		/// </summary>
		public static int FPS
		{
			get
			{
				return m_FPS;
			}
			set
			{
				Rate = value;
			}
		}

		/// <summary>
		/// Gets and sets the wanted framerate of the ticker.
		/// </summary>
		public static int Rate
		{
			get{
				return m_Rate;
			}
			set
			{
				m_Framecount = 0;
				m_Rate = value;
				m_Framerate = (1000.0F / (float)m_Rate);
			}
		}

		/// <summary>
		/// Gets and sets whether the framerate manager is running.
		/// </summary>
		public static bool IsRunning
		{
			get
			{
				return (m_Thread.ThreadState == ThreadState.Running) || (m_Thread.ThreadState == ThreadState.WaitSleepJoin);
			}
			set
			{
				if(value)
					m_Thread.Resume();
				else
					m_Thread.Suspend();
			}
		}

		private static void ThreadTicker()
		{
			int frames = 0;
			int lastTime = Sdl.SDL_GetTicks();
			int curTime;
			while(m_Thread.IsAlive)
			{
				int current_ticks;
				int target_ticks;
				int the_delay;

				m_Framecount++;

				current_ticks = Sdl.SDL_GetTicks();
				target_ticks = m_LastTick + (int)((float)m_Framecount * m_Framerate);

				if (current_ticks <= target_ticks) 
				{
					the_delay = target_ticks - current_ticks;
					//Sdl.SDL_Delay(the_delay);
					Thread.Sleep(the_delay);
				} 
				else 
				{
					m_Framecount = 0;
					m_LastTick = current_ticks;
				}

				
				if(FrameTick != null)
					FrameTick(null, new FramerateTickEventArgs(current_ticks, m_LastTick));


				curTime = Sdl.SDL_GetTicks();
				frames++;
				if(lastTime + 1000 <= curTime)
				{
					m_FPS = frames;
					frames = 0;
					lastTime = curTime;
				}
			}
		}

		/// <summary>
		/// Fires whenever a frame tick is allowed.
		/// </summary>
		public static event FramerateTickEventHandler FrameTick;


		
	}

	/// <summary>
	/// Event arguments for a Framerate tick.
	/// </summary>
	public class FramerateTickEventArgs : SdlEventArgs 
	{
		private int m_LastTick;
		private int m_Tick;

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
		/// 
		/// </summary>
		/// <param name="tick"></param>
		/// <param name="lastTick"></param>
		public FramerateTickEventArgs(int tick, int lastTick)
		{
			m_Tick = tick;
			m_LastTick = lastTick;
		}



	}
}
