using System;

namespace SdlDotNet
{
	/// <summary>
	/// Summary description for TickArgs.
	/// </summary>
	public class TickEventArgs : SdlEventArgs
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="skipped"></param>
		/// <param name="lastTick"></param>
		public TickEventArgs(int skipped, long lastTick)
		{
			this.skipped = skipped;
			this.lastTick = lastTick;
		}

		private int skipped = 0;

		/// <summary>
		/// 
		/// </summary>
		public int Skipped
		{
			get
			{
				return skipped;
			}
			set
			{
				skipped = value;
			}
		}

		private long lastTick = 0;

		/// <summary>
		/// 
		/// </summary>
		public long LastTick
		{
			get
			{
				return lastTick;
			}
			set
			{
				lastTick = value;
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
