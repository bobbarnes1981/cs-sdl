using MfGames.Sdl.Sprites;
using MfGames.Sdl.Drawable;
using MfGames.Utility;
using System;
using System.Drawing;

namespace MfGames.Sdl.Demos
{
  public class AnimatedSprite : DrawableSprite
  {
    // Randomly assign the direction we show the frames
    private bool frameRight = Entropy.Next() % 2 == 0;

    public AnimatedSprite(IDrawable d, Vector2 coords)
      : base(d, Entropy.Next(), coords)
    {
    }

    public AnimatedSprite(IDrawable d, Vector3 coords)
      : base(d, Entropy.Next(), coords)
    {
    }

    #region Animation and Drawing
    public override bool IsTickable
    {
      get { return Drawable.FrameCount > 1; }
    }

    public override void OnTick(TickArgs args)
    {
      // Increment the frame
      if (frameRight)
	Frame++;
      else if (Frame == 0)
	Frame = FrameCount -1;
      else
	Frame--;
    }
    #endregion

    #region Operators
    public override string ToString()
    {
      return String.Format("(animated {0})", base.ToString());
    }
    #endregion
  }
}
