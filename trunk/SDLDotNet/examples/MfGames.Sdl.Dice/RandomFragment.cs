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
