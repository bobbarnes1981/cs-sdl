namespace MfGames.Sdl.Gui
{
  public class Padding
  {
    private int [] padding = new int [] { 0, 0, 0, 0 };

    public Padding()
    {
    }

    public Padding(int each)
    {
      padding = new int [] { each, each, each, each };
    }

    public Padding(int sides, int height)
    {
      padding = new int [] { sides, height, sides, height };
    }

    public Padding(int left, int top, int right, int bottom)
    {
      padding = new int [] { left, top, right, bottom };
    }

    public int Horizontal
    {
      get { return Left + Right; }
    }

    public int Vertical
    {
      get { return Top + Bottom; }
    }

    public int Left
    {
      get { return padding[0]; }
      set { padding[0] = value; }
    }

    public int Right
    {
      get { return padding[2]; }
      set { padding[2] = value; }
    }

    public int Top
    {
      get { return padding[1]; }
      set { padding[1] = value; }
    }

    public int Bottom
    {
      get { return padding[3]; }
      set { padding[3] = value; }
    }
  }
}
