using MfGames.Utility;
using MfGames.Sdl.Drawable;
using MfGames.Sdl.Sprites;
using System.Drawing;

namespace MfGames.Sdl.Demos
{
  public class DragMode : DemoMode
  {
    /// <summary>
    /// Constructs the internal sprites needed for our demo.
    /// </summary>
    public DragMode()
    {
      // Create the fragment marbles
      int rows = 5;
      int cols = 5;
      int sx = (800 - cols * 64) / 2;
      int sy = (600 - rows * 64) / 2;
      IDrawable m1 = LoadMarble("marble1");
      IDrawable m2 = LoadMarble("marble2");

      for (int i = 0; i < cols; i++)
      {
	for (int j = 0; j < rows; j++)
	{
	  sm.Add(new DragSprite(m1, m2,
				new Vector2(sx + i * 64, sy + j * 64),
				new Rectangle2(SdlDemo.SpriteContainer.Size)));
	}
      }
    }

    public override string ToString() { return "Drag"; }
  }
}
