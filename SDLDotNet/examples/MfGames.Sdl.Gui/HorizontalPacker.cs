using MfGames.Utility;
using MfGames.Sdl.Sprites;
using SdlDotNet;
using System;
using System.Collections;
using System.Drawing;

namespace MfGames.Sdl.Gui
{
  /// <summary>
  /// Class to manager internal sprites, such as window
  /// components. This uses a sprite manager at its core, but does
  /// have some additional functionality.
  /// </summary>
  public class HorizontalPacker : Packer
  {
    #region Constructors
    public HorizontalPacker(GuiManager manager)
      : base(manager)
    {
    }

    public HorizontalPacker(GuiManager manager, Vector2 p)
      : base(manager, p)
    {
    }

    public HorizontalPacker(GuiManager manager, Vector3 p)
      : base(manager, p)
    {
    }
    #endregion

    #region Drawing
    public override void Render(RenderArgs args)
    {
      // Handle our arguments
      RenderArgs args0 = args.Clone();
      args0.TranslateX += Coords.X + MarginPadding.Left + InnerPadding.Left;
      args0.TranslateY += Coords.Y + MarginPadding.Top + InnerPadding.Top;

      // Call the base
      base.Render(args0);

      // Draw all of our left components
      int x = 0;

      foreach (Sprite s in HeadSprites)
      {
	// Ignore hidden
	if (s.IsHidden)
	  continue;
	
	// Translate it and blit
	s.Coords.X = x;
	s.Render(args0);

	// Update the coordinates for the next one
	x += s.Size.Width + InnerPadding.Horizontal;
      }

      // Draw our right components
      x = Coords.X + Size.Width - MarginPadding.Right;

      foreach (Sprite s in TailSprites)
      {
	// Ignore hidden
	if (s.IsHidden)
	  continue;
	
	// Translate it and blit
	x -= s.Size.Width + InnerPadding.Horizontal;
	s.Coords.X = x;
	s.Render(args0);
      }
    }
    #endregion

    #region Geometry
    public override Dimension2 Size
    {
      get
      {
	// Get the height
	int height = 0;

	// Get the sprites
	foreach (Sprite s in new ArrayList(Sprites))
	{
	  int h = GetSize(s).Height;

	  if (h > height)
	    height = h;
	}

	// Add the padding
	height += InnerPadding.Vertical + MarginPadding.Vertical;

	return new Dimension2(HorizontalWidth, height);
      }
    }

    public virtual int HorizontalWidth
    {
      get
      {
	// Go through the sprites
	int width = 0;

	foreach (Sprite s in new ArrayList(Sprites))
	{
	  int w = GetSize(s).Width;

	  width += w + InnerPadding.Horizontal;
	}

	return width + MarginPadding.Horizontal;
      }
    }
    #endregion
  }
}
