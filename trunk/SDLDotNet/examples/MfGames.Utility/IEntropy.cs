using System;

namespace MfGames.Utility
{
  public interface IEntropy
  {
    int Next();
    int Next(int min, int max);
  }
}
