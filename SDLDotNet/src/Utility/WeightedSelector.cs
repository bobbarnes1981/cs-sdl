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
using System.Collections;

namespace SdlDotNet.Utility
{
	/// <summary>
	/// This class contains a list of keys and their weights. It allows
	/// a random selection of elements, based on the weights.
	/// </summary>
	public class WeightedSelector
	{
		#region Operators
		/// <summary>
		/// 
		/// </summary>
		public int this[object key]
		{
			get
			{
				// Do some sanity checking
				if (key == null)
					return 0;

				// Find the weight, but make sure we always return an integer
				if (weights[key] == null)
					return 0;
				else
					return (int) weights[key];
			}

			set
			{
				// Make sure we are not negative
				if (key == null)
					throw new Exception("Cannot assign a weight to a null key");

				if (value < 0)
					throw new Exception("Cannot assign a negative weight to " + key);

				// Get the previous key and remove its weight
				int w = this[key];
				total -= w;

				// Assign the new one
				weights[key] = value;
				total += value;
			}
		}
		#endregion

		#region Random
		/// <summary>
		/// 
		/// </summary>
		public object RandomObject
		{
			get
			{
				// Just a random element, based on weights
				int sel = Entropy.Next(0, (int) total);
	
				return Select(sel);
			}
		}
		#endregion

		#region Weights
		// Contains a hashtable of weights
		private Hashtable weights = new Hashtable();

		// Contains the total weights
		private long total = 0;

		/// <summary>
		/// 
		/// </summary>
		public long Total { get { return total; } }

		/// <summary>
		/// Selects a specific object from the weighted chart, based on
		/// the given index.
		/// </summary>
		protected object Select(int index)
		{
			// Go through the keys
			long running = 0;

			foreach (object key in weights.Keys)
			{
				// Get the weight and add it
				running += this[key];

				// If we passed the results, return it
				if (running >= index)
					return key;
			}

			// Return it
			throw new Exception("Could not select random");
		}
		#endregion
	}
}
