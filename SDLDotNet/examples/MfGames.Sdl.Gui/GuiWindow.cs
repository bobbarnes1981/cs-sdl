using MfGames.Sdl.Sprites;
using MfGames.Utility;
using SdlDotNet;
using System.Drawing;
using System;

namespace MfGames.Sdl.Gui
{
  /// <summary>
  /// Handles a simple window element, which displays its contents in
  /// a frame.
  /// </summary>
  public class GuiWindow : GuiContainer
  {
    public GuiWindow(GuiManager manager)
      : base(manager)
    {
    }

    public GuiWindow(GuiManager manager, Rectangle2 rect)
      : base(manager, rect)
    {
    }

    #region Drawing
    public override Padding OuterPadding
    {
      get { return manager.GetPadding(this); }
    }

    public override void Render(RenderArgs args)
    {
      // Render the window using the GUI manager
      manager.Render(args, this);

      // Render the components
      base.Render(args);
    }
    #endregion

    #region Operators
    public override string ToString()
    {
      return String.Format("(window \"{0}\" {1})",
			   Title, base.ToString());
    }
    #endregion

    #region Properties
    private string title = null;
    private Dimension2 titleSize = new Dimension2();

    public string Title
    {
      get { return title; }
      set
      {
	// Set the title size
	title = value;

	// Set the bounds
	titleSize = manager.GetTextSize(manager.TitleFont, title);
      }
    }
    #endregion
  }
}
