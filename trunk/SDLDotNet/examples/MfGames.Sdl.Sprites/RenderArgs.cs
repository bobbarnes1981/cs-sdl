using MfGames.Utility;
using SdlDotNet;
using System.Drawing;

namespace MfGames.Sdl.Sprites
{
  public class RenderArgs
  {
    public RenderArgs(Surface surface, Dimension2 size)
    {
      Surface = surface;
      Size = size;
    }

    public RenderArgs Clone()
    {
      // Create a new one
      RenderArgs args = new RenderArgs(Surface, Size);
      args.tx = tx;
      args.ty = ty;
      return args;
    }

    #region Clipping
    private Rectangle2 origClip = null;
    private Rectangle2 clip = null;

    public void ClearClipping()
    {
      if (origClip == null)
	Surface.ClipRectangle  = new Rectangle(0, 0,
					  Surface.Width, Surface.Height);
      else
	Surface.ClipRectangle = origClip;
    }

    public void SetClipping(Rectangle2 rect)
    {
      clip = rect;
      Surface.ClipRectangle = clip;
    }
    #endregion

    #region Geometry
    public Rectangle2 Translate(Rectangle2 rect)
    {
      Rectangle2 r = new Rectangle2(rect);
      r.Coords.X += TranslateX;
      r.Coords.Y += TranslateY;
      return r;
    }

    public void TranslateBy(Sprite s)
    {
      TranslateX += s.Coords.X;
      TranslateY += s.Coords.Y;
    }
    #endregion

    #region Properties
    private Surface surface = null;
    private int tx = 0;
    private int ty = 0;
    private Dimension2 size = new Dimension2();

    public Vector2 Vector
    {
      get { return new Vector2(tx, ty); }
    }

    public Surface Surface
    {
      get { return surface; }
      set
      {
	if (value == null)
	  throw new SpriteException("Cannot assign a null surface");

	surface = value;
      }
    }

    public Dimension2 Size
    {
      get { return size; }
      set
      {
	if (value == null)
	  throw new SpriteException("Cannot assign a null size!");

	size = value;
      }
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
    #endregion
  }
}
