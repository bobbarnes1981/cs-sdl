using MfGames.Utility;
using MfGames.Sdl.Drawable;
using MfGames.Sdl.Sprites;
using SdlDotNet;
using System;
using System.Drawing;

namespace MfGames.Sdl.Demos
{
  public class DragSprite : BoundedSprite
  {
    private IDrawable d1 = null;
    private IDrawable d2 = null;

    public DragSprite(IDrawable d1, IDrawable d2, Vector2 coords,
		      Rectangle2 bounds)
      : base(d1, bounds, new Vector3(coords))
    {
      this.d1 = d1;
      this.d2 = d2;
    }

    public override string ToString()
    {
      return String.Format("(drag {0} {1})", beingDragged, base.ToString());
    }

    #region Events
    private bool beingDragged = false;

    public override bool IsMouseSensitive { get { return true; } }

    public override bool OnMouseButton(object sender, MouseArgs args)
    {
      // If we are being held down, pick up the marble
      if (args.IsButton1)
      {
	// Change the Z-order
	Coords.Z += 100;
	beingDragged = true;
	Drawable = d2;
	SdlDemo.MasterSpriteContainer.EventLock = this;
      }
      else
      {
	// Drop it
	Coords.Z -= 100;
	beingDragged = false;
	Drawable = d1;
	SdlDemo.MasterSpriteContainer.EventLock = null;
      }

      // We are finished
      return true;
    }

    /// <summary>
    /// If the sprite is picked up, this moved the sprite to follow
    /// the mouse.
    /// </summary>
    public override bool OnMouseMotion(object sender, MouseArgs args)
    {
      if (beingDragged)
      {
	Coords.X += args.RelativeX;
	Coords.Y += args.RelativeY; 
	return true;
      }
      else
      {
	return false;
      }
    }
    #endregion
  }
}
