using PerCederberg.Grammatica.Parser;
using System;

namespace MfGames.Utility.Dice
{
  internal class DiceFactory : DiceAnalyzer
  {
    #region Tokens
    public override Node ExitNumber(Token node)
    {
      node.AddValue(Int32.Parse(node.GetImage()));
      return node;
    }
    #endregion

    #region Productions
    public override Node ExitExpression(Production node)
    {
      // Pass the results up
      node.AddValue(node.GetChildAt(0).GetValue(0));
      return node;
    }
    #endregion

    #region Operation Fragments
    public override Node ExitAdditionFragment(Production node)
    {
      // We have two different formats, either a single production or
      // three (d + d). For the single, just pass it up.
      if (node.GetChildCount() == 1)
      {
	node.AddValue(node.GetChildAt(0).GetValue(0));
	return node;
      }

      // Pull out the dice (d + d)
      IDice d1 = (IDice) node.GetChildAt(0).GetValue(0);
      IDice d2 = (IDice) node.GetChildAt(2).GetValue(0);

      // Create the additional fragment, add it to the node, and
      // return the node.
      node.AddValue(new AdditionFragment(d1, d2));
      return node;
    }
    #endregion

    #region Numeric Fragments
    public override Node ExitNumericFragment(Production node)
    {
      // Pass the results up
      node.AddValue(node.GetChildAt(0).GetValue(0));
      return node;
    }

    public override Node ExitConstantFragment(Production node)
    {
      // This only has one child (#). So, just pull out the value,
      // create the constant fragment, and return it.
      int value = (Int32) node.GetChildAt(0).GetValue(0);
      node.AddValue(new ConstantFragment(value));
      return node;
    }

    public override Node ExitRandomFragment(Production node)
    {
      // This can have either 2 (d#) or 3 arguments (#d#).
      int count = 1;
      int sides = 1;

      if (node.GetChildCount() == 2)
      {
	sides = (int) node.GetChildAt(1).GetValue(0);
      }
      else if (node.GetChildCount() == 3)
      {
	count = (int) node.GetChildAt(0).GetValue(0);
	sides = (int) node.GetChildAt(2).GetValue(0);
      }

      // Set the fragment in the node and return it
      node.AddValue(new RandomFragment(count, sides));
      return node;
    }
    #endregion
  }
}
