using MfGames.Utility;
using MfGames.Sdl.Drawable;
using System.Drawing;

namespace MfGames.Sdl.Demos
{
  public class BounceSprite : BoundedSprite
  {
    private int dx = Entropy.Next(-10, 10);
    private int dy = Entropy.Next(-10, 10);

    public BounceSprite(IDrawable d, Rectangle2 rect)
      : base(d, rect,
	     new Vector3(Entropy.Next(rect.Left, rect.Right
				      - (int) d.Size.Width),
			 Entropy.Next(rect.Top, rect.Bottom
				      - (int) d.Size.Height),
			 0))
    {
    }

    public BounceSprite(IDrawable d, Rectangle2 rect, int z)
      : base(d, rect,
	     new Vector3(Entropy.Next(rect.Left, rect.Right
				    - (int) d.Size.Width),
			 Entropy.Next(rect.Top, rect.Bottom
				      - (int) d.Size.Height),
			 z))
    {
    }

    public override bool IsTickable { get { return true; } }

    public override void OnTick(TickArgs args)
    {
      // Move our direction a little
      int x = Coords.X;
      int y = Coords.Y;

      Coords.X += args.RatePerSecond(dx);
      Coords.Y += args.RatePerSecond(dy);

      // Adjust our entropy
      dx += Entropy.Next(-5, 5);
      dy += Entropy.Next(-5, 5);

      // Call the base which also normalizes the bounds
      base.OnTick(args);

      // Normalize the directions
      if (Coords.X == SpriteBounds.Left)
	dx = Entropy.Next(1, 10);

      if (Coords.X == SpriteBounds.Right)
	dx = Entropy.Next(-1, -10);

      if (Coords.Y == SpriteBounds.Top)
	dy = Entropy.Next(1, 10);

      if (Coords.Y == SpriteBounds.Bottom)
	dy = Entropy.Next(-1, -10);
    }
  }
}
