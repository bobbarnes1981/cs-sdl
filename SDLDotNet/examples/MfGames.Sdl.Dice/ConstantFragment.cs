using System;
using MfGames.Utility;

namespace MfGames.Utility.Dice
{
  public class ConstantFragment : IDice
  {
    private int value = 0;

    public ConstantFragment(int value)
    {
      this.value = value;
    }

    public override string ToString()
    {
      return value.ToString();
    }

    /// <summary>
    /// This simply returns the value of the constant.
    /// </summary>
    public int Roll { get { return value; } }
  }
}
