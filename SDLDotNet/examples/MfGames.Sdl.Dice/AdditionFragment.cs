using System;
using MfGames.Utility;

namespace MfGames.Utility.Dice
{
  public class AdditionFragment : IDice
  {
    private IDice d1 = null;
    private IDice d2 = null;

    public AdditionFragment(IDice d1, IDice d2)
    {
      this.d1 = d1;
      this.d2 = d2;
    }

    public override string ToString()
    {
      return String.Format("{0}+{1}", d1, d2);
    }

    /// <summary>
    /// This adds the results of the two dice and returns the results.
    /// </summary>
    public int Roll
    {
      get
      {
	return d1.Roll + d2.Roll;
      }
    }
  }
}
