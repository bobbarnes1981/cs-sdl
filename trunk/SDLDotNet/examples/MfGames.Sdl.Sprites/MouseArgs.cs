using MfGames.Utility;
using SdlDotNet;

namespace MfGames.Sdl.Sprites
{
  public class MouseArgs
  {
    public MouseArgs()
    {
    }

    public MouseArgs Clone()
    {
      // Create a new one
      MouseArgs args = new MouseArgs();
      args.x = x;
      args.y = y;
      args.tx = tx;
      args.ty = ty;
      args.rx = rx;
      args.ry = ry;
      args.b1 = b1;
      return args;
    }

    #region Properties
    private bool b1 = false;
    private int tx = 0;
    private int ty = 0;
    private int rx = 0;
    private int ry = 0;
    private int x = 0;
    private int y = 0;

    public bool IsButton1
    {
      get { return b1; }
      set { b1 = value; }
    }

    public Vector2 Coords
    {
      get { return new Vector2(X, Y); }
    }

    public int X
    {
      get { return x + tx; }
    }

    public int Y
    {
      get { return y + ty; }
    }

    public int BaseX
    {
      get { return x; }
      set { x = value; }
    }

    public int BaseY
    {
      get { return y; }
      set { y = value; }
    }

    public int TranslateX
    {
      get { return tx; }
      set { tx = value; }
    }

    public int TranslateY
    {
      get { return ty; }
      set { ty = value; }
    }

    public int RelativeX
    {
      get { return rx; }
      set { rx = value; }
    }

    public int RelativeY
    {
      get { return ry; }
      set { ry = value; }
    }
    #endregion
  }
}
