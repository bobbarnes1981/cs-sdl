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
using log4net.Config;

namespace MfGames.Utility
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
      t.Info("PreMark execution");
      t.Mark();
      t.Info("This is {0} executions {1} {2}", 1, "billy bob", t);

      // Check the weighted averages
      WeightedSelector ws = new WeightedSelector();
      t.Info("bob == 0: {0} (t=0={1})", ws["bob"], ws.Total);
      ws["bob"] = 5;
      ws["gary"] = 5;
      t.Info("bob == 5: {0} (t=10={1})", ws["bob"], ws.Total);
      ws["gary"] = 2;
      t.Info("gary == 2: {0} (t=7={1})", ws["gary"], ws.Total);

      for (int i = 0; i < 10; i++)
	t.Info("random: " + ws.RandomObject);
    }

    public void Mark()
    {
      Info("This is an informational message.");
      Error("An error message");
    }

    override public string ToString() { return "TestObject"; }
  }
}
