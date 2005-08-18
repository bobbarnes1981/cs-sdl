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
	/// This framerate manager is used to insert delays into the graphic loop to maintain a constant framerate.
	/// </summary>
	/// <remarks>This is pretty much a direct C# translation of SDL_gfx's framerate control, except for the delegates and events.</remarks>
	/// <example>
	/// <code>
	/// // Setup event calls
	/// Events.FramerateTick += new FramerateTickEventHandler(Render);
	/// Events.FramerateTick += new FramerateTickEventHandler(Update);
	/// 
	/// // Change constant framerate
	/// Framerate.Rate = 60;
	/// 
	/// // Start the ticker
	/// Framerate.Run();
	/// 
	/// // Get the FPS
	/// Debug.WriteLine("FPS: " + Framerate.FPS);
	/// </code>
	/// </example>
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
			Rate = 30;
			m_LastTick = 0;
			m_Thread = new Thread(new ThreadStart(ThreadTicker));
			m_Thread.Priority = ThreadPriority.Normal;
			m_Thread.IsBackground = true;
			m_Thread.Name = "SDL.NET - Framerate Manager";
		}

		/// <summary>
		/// Starts the framerate ticker. Must be called to start the manager interface.
		/// </summary>
		public static void Run()
		{
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
			get
			{
				return m_Rate;
			}
			set
			{
				m_Framecount = 0;
				if(value < 1)
					m_Rate = 1;
				else if(value > 200)
					m_Rate = 200;
				else
					m_Rate = value;
				m_Framerate = (1000.0F / (float)m_Rate);
			}
		}

		/// <summary>
		/// The private method, run by the ticker thread, that controls timing to call the event.
		/// </summary>
		private static void ThreadTicker()
		{
			int frames = 0;
			int lastTime = Sdl.SDL_GetTicks();
			int curTime;
			int current_ticks;
			int target_ticks;
			int the_delay;
			
			while(m_Thread.IsAlive)
			{
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

				Events.NotifyFramerateTickEvent(new FramerateTickEventArgs(current_ticks, m_LastTick, m_FPS));

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
	}
		
		
	/// <summary>
	/// Event arguments for a Framerate tick.
	/// </summary>
	public class FramerateTickEventArgs : SdlEventArgs 
	{
		private int m_LastTick;
		private int m_Tick;
		private int m_FPS;

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
		/// Gets the FPS as of the event call. Framerate.FPS is an alternative.
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
		/// Gets the difference in time between the current tick and the last tick.
		/// </summary>
		public int Delay
		{
			get
			{
				return m_Tick - m_LastTick;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="tick">The current tick.</param>
		/// <param name="lastTick">The tick count that it was at last frame.</param>
		public FramerateTickEventArgs(int tick, int lastTick, int fps)
		{
			m_Tick = tick;
			m_LastTick = lastTick;
			m_FPS = fps;
		}
	}
}
