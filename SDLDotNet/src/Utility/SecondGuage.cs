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

namespace SdlDotNet.Utility
{
	/// <summary>
	/// A simple gauge which attempts to keep track of how many ticks or
	/// activations per second. This is used to calculate the FPS of a
	/// program.
	/// </summary>
	public class SecondGauge : ITickable
	{
		private int span = 5;

		private int [] buckets = null;

		/// <summary>
		/// 
		/// </summary>
		public SecondGauge()
		{
			buckets = new int [span];

			for (int i = 0; i < span; i++)
				buckets[i] = 0;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="newSpan"></param>
		public SecondGauge(int newSpan)
		{
			span = newSpan;
			buckets = new int [span];

			for (int i = 0; i < span; i++)
				buckets[i] = 0;
		}

		#region Counting
		private long lastSecond = DateTime.Now.Second;

		private long loops = 0;

		/// <summary>
		/// 
		/// </summary>
		public bool IsTickable { get { return true; } }

		/// <summary>
		/// Activates the counter for the current second.
		/// </summary>
		public void Activate()
		{
			lock (this) 
			{
				// Figure if we need a roll-over
				long now = DateTime.Now.Second;

				if (now != lastSecond)
				{
					// Set up the new time
					lastSecond = now;
					loops++;

					// Move the buckets over
					for (int i = 1; i < span; i++)
					{
						buckets[i] = buckets[i - 1];
					}

					// Reset the last one
					buckets[0] = 0;
				}
	
				// Add to the bucket
				buckets[0]++;
			}
		}

		/// <summary>
		/// Enables the guage to be added directly to a tick manager.
		/// </summary>
		public void OnTick(TickArgs args)
		{
			Activate();
		}
		#endregion

		#region Results
		/// <summary>
		/// 
		/// </summary>
		public double Average
		{
			get
			{
				double total = 0.0;

				for (int i = 1; i < span; i++)
					total += buckets[i];

				return total / span;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public bool IsFull { get { return loops >= span; } }
		#endregion
	}
}
