using MfGames.Utility;
using MfGames.Sdl.Sprites;
using MfGames.Sdl.Drawable;
using SdlDotNet;
using System;
using System.Drawing;

namespace MfGames.Sdl.Demos
{
  public class ClickSprite : AnimatedSprite
  {
    private IDrawable d1 = null;
    private IDrawable d2 = null;

    public ClickSprite(IDrawable d1, IDrawable d2, Vector2 coords)
      : base(d1, coords)
    {
      this.d1 = d1;
      this.d2 = d2;
    }

    public override string ToString()
    {
      return String.Format("(click {0})", base.ToString());
    }

    #region Events
    public override bool IsMouseSensitive { get { return true; } }

    public override bool OnMouseButton(object sender, MouseArgs args)
    {
      // If we are not being released, don't bother
      if (args.IsButton1)
	return true;

      // Switch the image
      if (Drawable == d1)
	Drawable = d2;
      else
	Drawable = d1;

      // We are done processing
      return true;
    }
    #endregion
  }
}
