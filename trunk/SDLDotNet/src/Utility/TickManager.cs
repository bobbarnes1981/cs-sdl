/*
 * $RCSfile$
 * Copyright (C) 2004 D. R. E. Moonfire (d.moonfire@mfgames.com)
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

namespace SdlDotNet.Utility
{
	/// <summary>
	/// This simple class handles a single "tick" thread. It is given a
	/// certain time to sleep between the tick. On each tick, it sends a
	/// TickEvent to all delegates.
	/// </summary>summary>
	public class TickManager
	{
		#region Thread Management
		private Thread serverThread = null;

		private Thread tickerThread = null;

		private bool stopThread = true;

		private int skippedTicks = 0;

		private int processSkipped = 0;

		private bool processing = false;

		private long lastTick = DateTime.Now.Ticks;

		/// <summary>
		/// Processes the tick server thread. This keeps track of the
		/// number of skipped ticks, to give a more accurate count or
		/// status of each tick.
		/// </summary>
		private void Run()
		{
			// Loops until the system indicates a stop
			while (!stopThread)
			{
				// Sleep for a little bit
				Thread.Sleep(tickSpan);

				// Lock to see if we are processing
				lock (this) 
				{
					// If we are processing, just increment it
					if (processing)
					{
						skippedTicks++;
						continue;
					}

					// Process the counter
					processSkipped = skippedTicks;
					skippedTicks = 0;
					processing = true;
				}

				// Perform the actual threaded tick
				//RunTicker();
				tickerThread = new Thread(new ThreadStart(RunTicker));
				tickerThread.Start();
			}
		}

		/// <summary>
		/// Executes a single tick statement in a second thread.
		/// </summary>
		private void RunTicker()
		{
			// Execute the tick
			if (TickEvent != null)
			{
				// Create the arguments
				TickArgs args = new TickArgs();
				args.Skipped = processSkipped;
				long now = DateTime.Now.Ticks;
				args.LastTick = now - lastTick;
				lastTick = now;

				// Trigger the ticker
				TickEvent(args);
			}
      
			// Clear the flag. This is in a locked block because the testing
			// of it is also in the same lock.
			lock (this)
				processing = false;
		}

		/// <summary>
		/// 
		/// </summary>
		public void Start()
		{
			// Prepare for the server thread
			//Debug("Starting tick manager thread");
			stopThread = false;
			serverThread = new Thread(new ThreadStart(Run));
			serverThread.IsBackground = true;
			serverThread.Priority = ThreadPriority.Lowest;
			serverThread.Start();
		}

		/// <summary>
		/// 
		/// </summary>
		public void Stop()
		{
			// Mark everything to stop
			stopThread = true;

			// Join the ticker
			if (tickerThread != null)
			{
				try
				{
					tickerThread.Join();
				}
				catch { }
			}

			// Join the server
			try
			{
				serverThread.Join();
				serverThread = null;
			}
			catch { }

			// Make noise
			while (processing)
				Thread.Sleep(tickSpan);

			//Debug("Stopped tick manager thread");
		}
		#endregion

		#region Tick Duration
		private int tickSpan = 1000;

		/// <summary>
		/// 
		/// </summary>
		public event TickHandler TickEvent;

		/// <summary>
		/// Convienance function to add an ITickable object into the tick
		/// manager.
		/// </summary>
		public void Add(ITickable tickable)
		{
			TickEvent += new TickHandler(tickable.OnTick);
		}

		/// <summary>
		/// 
		/// </summary>
		public int TicksPerSecond
		{
			get { return 1000 / tickSpan; }
			set { tickSpan = 1000 / value; }
		}

		/// <summary>
		/// 
		/// </summary>
		public int TickSpan
		{
			get { return tickSpan; }
			set
			{
				if (value < 1)
					throw new Exception("Cannot set a negative or zero sleep time");

				tickSpan = value;
			}
		}
		#endregion
	}

	/// <summary>
	/// Handles the ticks as they are processed by the system.
	/// </summary>
	public delegate void TickHandler(TickArgs args);

	/// <summary>
	/// 
	/// </summary>
	public class TickArgs
	{
		/// <summary>
		/// 
		/// </summary>
		public int Skipped = 0;
		/// <summary>
		/// 
		/// </summary>
		public long LastTick = 0;

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
