using MfGames.Utility;
using MfGames.Sdl.Drawable;
using MfGames.Sdl.Sprites;
using System.Drawing;

namespace MfGames.Sdl.Demos
{
  public class BounceMode : DemoMode
  {
    /// <summary>
    /// Constructs the internal sprites needed for our demo.
    /// </summary>
    public BounceMode()
    {
      // Create the fragment marbles
      for (int i = 0; i < 50; i++)
      {
	sm.Add(new BounceSprite(LoadRandomMarble(),
				new Rectangle2(SdlDemo.SpriteContainer.Size)));
      }
    }

    public override string ToString() { return "Bounce"; }
  }
}
