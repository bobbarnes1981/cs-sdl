using System;

namespace MfGames.Sdl.Sprites
{
  public class SpriteException : Exception
  {
    public SpriteException(string msg)
      : base(msg)
    {
    }

    public SpriteException(string msg, Exception e)
      : base(msg, e)
    {
    }
  }
}
