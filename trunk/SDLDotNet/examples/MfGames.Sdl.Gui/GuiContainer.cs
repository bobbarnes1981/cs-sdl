using MfGames.Utility;
using MfGames.Sdl.Sprites;
using SdlDotNet;
using System;
using System.Drawing;

namespace MfGames.Sdl.Gui
{
  /// <summary>
  /// Class to manager internal sprites, such as window
  /// components. This uses a sprite manager at its core, but does
  /// have some additional functionality.
  /// </summary>
  public class GuiContainer : GuiComponent
  {
    #region Constructors
    public GuiContainer(GuiManager manager)
      : base(manager)
    {
    }

    public GuiContainer(GuiManager manager, Rectangle2 rect)
      : base(manager, rect)
    {
    }
    #endregion

    #region Drawing
    public override void Render(RenderArgs args)
    {
      // Modify the arguments
      RenderArgs args1 = args.Clone();
      args1.TranslateX += Coords.X;
      args1.TranslateY += Coords.Y;
      args1.SetClipping(new Rectangle2(args.TranslateX + Coords.X,
				       args.TranslateY + Coords.Y,
				       Size.Width,
				       Size.Height));

      // Render the contents
      contents.Render(args1);
      args1.ClearClipping();
    }
    #endregion

    #region Events
    public override void OnTick(TickArgs args)
    {
      base.OnTick(args);
      contents.OnTick(args);
    }
    #endregion

    #region Properties
    private SpriteContainer contents = new SpriteContainer();

    public SpriteContainer Contents
    {
      get { return contents; }
    }
    #endregion
  }
}
