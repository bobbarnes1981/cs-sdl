using MfGames.Utility;
using SdlDotNet;
using System.Drawing;

namespace MfGames.Sdl.Sprites
{
  /// <summary>
  /// Implements a text sprite that has a bounded box to define its
  /// size and an orientation (as a float) for vertical and horizontal
  /// alignment.
  /// </summary>
  public class BoundedTextSprite : TextSprite
  {
    public BoundedTextSprite(string text, SdlDotNet.Font font,
			     Dimension2 size)
      : base(text, font)
    {
      this.size = size;
    }

    public BoundedTextSprite(string text, SdlDotNet.Font font,
			     Dimension2 size,
			     Vector2 coords)
      : base(text, font, coords)
    {
      this.size = size;
    }

    public BoundedTextSprite(string text, SdlDotNet.Font font,
			     Dimension2 size,
			     Vector3 coords)
      : base(text, font, coords)
    {
      this.size = size;
    }

    public BoundedTextSprite(string text, SdlDotNet.Font font,
			     Dimension2 size,
			     double horz, double vert)
      : base(text, font)
    {
      this.size = size;
      this.horz = horz;
      this.vert = vert;
    }

    public BoundedTextSprite(string text, SdlDotNet.Font font,
			     Dimension2 size,
			     double horz, double vert,
			     Vector2 coords)
      : base(text, font, coords)
    {
      this.size = size;
      this.horz = horz;
      this.vert = vert;
    }

    public BoundedTextSprite(string text, SdlDotNet.Font font,
			     Dimension2 size,
			     double horz, double vert,
			     Vector3 coords)
      : base(text, font, coords)
    {
      this.size = size;
      this.horz = horz;
      this.vert = vert;
    }

    #region Drawing
    public override void Render(RenderArgs args)
    {
      // Determine the offset
      Surface render = Surface;
      int width = Size.Width;
      int height = Size.Height;
      double dw = width - render.Width;
      double dh = height - render.Height;
      int offsetX = 0;
      int offsetY = 0;

      if (dw > 0.0)
	offsetX += (int) (dw * horz);

      if (dh > 0.0)
	offsetY += (int) (dh * vert);

      // Render the image itself
      args.Surface.Blit(render,
		  new Rectangle(new Point(Coords.X
					  + offsetX + args.TranslateX,
					  Coords.Y
					  + offsetY + args.TranslateY),
				Size));
    }
    #endregion

    #region Properties
    private double horz = 0.5;

    private double vert = 0.5;

    private Dimension2 size = null;

    public double HorizontalWeight
    {
      get { return horz; }
      set { horz = value; render = null; }
    }

    public double VerticalWeight
    {
      get { return vert; }
      set { vert = value; render = null; }
    }

    public override Dimension2 Size
    {
      get
      {
	if (size == null)
	  return base.Size;
	else
	  return size;
      }
    }
    #endregion
  }
}
