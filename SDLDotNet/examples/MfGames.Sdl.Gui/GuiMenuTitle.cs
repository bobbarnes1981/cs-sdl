using MfGames.Utility;
using MfGames.Sdl.Sprites;
using SdlDotNet;
using System;

namespace MfGames.Sdl.Gui
{
  /// <summary>
  /// Despite its name, the menubar title is actually the part in the
  /// menubarbar that gives the name.
  /// </summary>
  public class GuiMenuTitle : HorizontalPacker, IMenuPopupController
  {
    public GuiMenuTitle(GuiManager manager)
      : base(manager)
    {
      this.menubar = menubar;
      this.popup = new GuiMenuPopup(manager);
      this.popup.Controller = this;
    }

    public GuiMenuTitle(GuiManager manager, string title)
      : base(manager)
    {
      this.menubar = menubar;
      this.popup = new GuiMenuPopup(manager);
      this.popup.Controller = this;
      
      TextSprite ts = new TextSprite(title, manager.BaseFont);
      AddHead(ts);
    }

    #region Drawing
    private int PopupX
    {
      get
      {
	return OuterBounds.Left - manager.MenuTitlePadding.Left;
      }
    }

    private int PopupY
    {
      get
      {
	return OuterBounds.Bottom
	  + popup.OuterPadding.Top
	  + manager.MenuTitlePadding.Bottom
	  + manager.MenuBarPadding.Bottom;
      }
    }

    public override void Render(RenderArgs args)
    {
      // Draw ourselves
      manager.Render(args, this);
      base.Render(args);

      // Push the args
      RenderArgs args1 = args.Clone();
      args1.TranslateX += PopupX;
      args1.TranslateY += PopupY;

      // Check for tracing renders
      if (IsTraced)
      {
	// Put a small dot where the menu will show up
	int px = 0 + args.TranslateX;
	int py = 0 + args.TranslateY;

	//Debug("ar: {0}, ar1: {1}", args.Vector, args1.Vector);

	args.Surface.DrawPixel(px, py, System.Drawing.Color.BlanchedAlmond);
	GuiManager.DrawRect(args.Surface,
			    args1.Translate(popup.OuterBounds),
			    manager.OuterBoundsTraceColor);
      }

      // Check for menu
      if (IsSelected)
      {
	popup.Coords.X = 0;
	popup.Coords.Y = 0;
	popup.Render(args1);
      }
    }
    #endregion

    #region Sprites
    public void Add(GuiMenuItem gmi)
    {
      popup.Add(gmi);
    }
    #endregion

    #region Events
    public override bool OnMouseButton(object sender, MouseArgs args)
    {
      // Build up the translations
      MouseArgs args1 = args.Clone();
      args1.TranslateX += PopupX;
      args1.TranslateY += PopupY;

      // Check for clicking down
      if (!popup.IsHidden)
      {
	return popup.OnMouseButton(this, args1);
      }

      if (args.IsButton1)
      {
	IsSelected = true;
	popup.ShowMenu();
	return true;
      }

      return false;
    }

    public override void OnTick(TickArgs args)
    {
      // Call our base's tick processing
      base.OnTick(args);

      // If we are showing our menu, display it
      /* TODO This kills things
      if (IsSelected)
	popup.OnTick(args);
      */
    }
    #endregion

    #region Operators
    public override string ToString()
    {
      return String.Format("(menu-title {0})", base.ToString());
    }
    #endregion

    #region Properties
    private GuiMenuBar menubar = null;
    private GuiMenuPopup popup = null;

    private bool selected = false;

    public bool IsSelected
    {
      get { return selected; }
      set { selected = value; }
    }

    public GuiMenuBar MenuBar
    {
      get { return menubar; }
      set { menubar = value; }
    }

    public GuiMenuPopup Popup
    {
      get { return popup; }
    }
    #endregion
  }
}
