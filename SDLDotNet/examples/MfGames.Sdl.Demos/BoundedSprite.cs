using MfGames.Utility;
using MfGames.Sdl.Drawable;

namespace MfGames.Sdl.Demos
{
  public class BoundedSprite : AnimatedSprite
  {
    private Rectangle2 rect = new Rectangle2();

    public BoundedSprite(IDrawable d, Rectangle2 bounds, Vector3 coords)
      : base(d, coords)
    {
      this.rect = new Rectangle2(bounds);
      this.rect.Size.Width -= (int) d.Size.Width;
      this.rect.Size.Height -= (int) d.Size.Height;
    }

    public Rectangle2 SpriteBounds
    {
      get { return rect; }
    }

    public override void OnTick(TickArgs args)
    {
      // Animate
      base.OnTick(args);

      // Bounce off the left
      if (Coords.X < rect.Left)
	Coords.X = rect.Left;

      // Bounce off the top
      if (Coords.Y < rect.Top)
	Coords.Y = rect.Top;

      // Bounce off the bottom
      if (Coords.Y > rect.Bottom)
	Coords.Y = rect.Bottom;

      // Bounce off the right
      if (Coords.X > rect.Right)
	Coords.X = rect.Right;
    }
  }
}
