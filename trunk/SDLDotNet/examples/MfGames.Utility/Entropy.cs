using System;

namespace MfGames.Utility
{
  public class Entropy
  {
    private static IEntropy entropy = new MersenneEntropy();

    public static int Next()
    {
      return entropy.Next();
    }

    public static int Next(int min, int max)
    {
      return entropy.Next(min, max);
    }
  }
}
