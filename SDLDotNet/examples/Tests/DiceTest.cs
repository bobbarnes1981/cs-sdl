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
using log4net.Config;

namespace MfGames.Utility.Dice
{
  public class Test : LoggedObject
  {
    /// <summary>
    /// Application entry point
    /// </summary>
    /// <param name="args">command line arguments</param>
    public static void Main(string[] args)
    {
      // Set up log4net logging
      BasicConfigurator.Configure();

      // Create the object
      Test t = new Test();

      // Make some noise
      t.Info("Dice Testing");
      t.Try("6");
      t.Try("d6");
      t.Try("2d6");
      t.Try("1+2");
      t.Try("1d2+10");
      t.Try("1d6+1d4");

      // Test the formatting
      IDice dice = Dice.Parse("10d10");
      t.Info("{0}: {1}, {2}, {3}",
	     dice,
	     dice.Roll,
	     dice.Roll,
	     dice.Roll
	     );
    }

    public void Try(string fmt)
    {
      IDice d = Dice.Parse(fmt);

      Info("{0}: {1}: {2}", fmt, d, d.Roll);
    }

    protected override string LoggingID { get { return ""; } }
  }
}
