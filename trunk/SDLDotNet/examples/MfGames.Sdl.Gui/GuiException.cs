using System;

namespace MfGames.Sdl.Gui
{
  public class GuiException : Exception
  {
    public GuiException(string msg)
      : base(msg)
    {
    }
  }
}
