using System;

namespace MfGames.Utility
{
  public class MersenneEntropy : IEntropy
  {
    private MersenneTwister twister = null;

    public MersenneEntropy()
    {
      twister = new MersenneTwister();
    }

    public int Next()
    {
      return twister.Next();
    }

    public int Next(int min, int max)
    {
      return twister.Next(min, max);
    }
  }
}
