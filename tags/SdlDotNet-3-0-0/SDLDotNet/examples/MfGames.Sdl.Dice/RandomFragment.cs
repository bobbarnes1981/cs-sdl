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
using MfGames.Utility;

namespace MfGames.Utility.Dice
{
  public class RandomFragment : IDice
  {
    private int count = 1;
    private int sides = 1;

    public RandomFragment(int count, int sides)
    {
      this.count = count;
      this.sides = sides;
    }

    public override string ToString()
    {
      return String.Format("{0}d{1}", count, sides);
    }

    /// <summary>
    /// This simple property rolls a number of dice equal to the
    /// count, with each die being 1 to sides. The total is returned
    /// as the result.
    /// </summary>
    public int Roll
    {
      get
      {
	int total = 0;

		  for (int i = 0; i < count; i++)
		  {
			  total += Entropy.Next(1, sides);
		  }

	return total;
      }
    }
  }
}
