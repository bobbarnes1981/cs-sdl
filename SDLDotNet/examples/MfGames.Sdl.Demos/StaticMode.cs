using MfGames.Utility;
using MfGames.Sdl.Drawable;
using MfGames.Sdl.Sprites;
using System.Drawing;

namespace MfGames.Sdl.Demos
{
  public class StaticMode : DemoMode
  {
    /// <summary>
    /// Constructs the internal sprites needed for our demo.
    /// </summary>
    public StaticMode()
    {
      // Create our image and add it to our sprite manager
      ImageDrawable id = new ImageDrawable("../../Data/marble1.png");
      DrawableSprite s = new DrawableSprite(id, new Vector2(5, 5));
      Debug("s {0}: {1}", s, s.IsHidden);
      sm.Add(s);

      // Create the fragment image
      TiledDrawable td = new TiledDrawable(id, new Dimension2(64, 64), 6, 6);
      AnimatedSprite an = new AnimatedSprite(td, new Vector3(200, 32, 100));
      an.Coords.X = 250;
      sm.Add(an);

      // Create the full marble, but test order
      IDrawable m1 = LoadMarble("marble1");

      for (int i = 0; i < 10; i++)
      {
	AnimatedSprite as1 = new AnimatedSprite(m1,
						new Vector3(50 + i * 32,
							    436, i));
	AnimatedSprite as2 = new AnimatedSprite(m1,
						new Vector3(50 + i * 32,
							    468, 10 - i));
	sm.Add(as1);
	sm.Add(as2);
      }
    }

    public override string ToString() { return "Static"; }
  }
}
