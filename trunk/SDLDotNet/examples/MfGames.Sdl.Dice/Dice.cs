using PerCederberg.Grammatica.Parser;
using System.IO;

namespace MfGames.Utility.Dice
{
  public class Dice : LoggedObject
  {
    public static IDice Parse(string fmt)
    {
      // Parse the string and generate a node object
      Parser parser = new DiceParser(new StringReader(fmt),
				     new DiceFactory());
      Node node = parser.Parse();
      IDice dice = (IDice) node.GetChildAt(0).GetValue(0);

      // Return the dice
      return dice;
    }

    public static int Roll(string fmt)
    {
      // Parse the format string and roll the results
      return Parse(fmt).Roll;
    }
  }
}
