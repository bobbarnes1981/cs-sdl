using System;

namespace MfGames.Sdl.Drawable
{
  public class DrawableException : Exception
  {
    public DrawableException(string msg)
      : base(msg)
    {
    }

    public DrawableException(string msg, Exception e)
      : base(msg, e)
    {
    }
  }
}
