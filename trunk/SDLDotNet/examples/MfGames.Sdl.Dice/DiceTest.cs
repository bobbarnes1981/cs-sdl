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
