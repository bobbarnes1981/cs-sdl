using MfGames.Sdl.Sprites;
using MfGames.Utility;
using SdlDotNet;
using System;
using System.Collections;
using System.Drawing;

namespace MfGames.Sdl.Gui
{
  public class GuiMenuPopup : VerticalPacker
  {
    public GuiMenuPopup(GuiManager manager)
      : base(manager, new Vector3(1000))
    {
      IsHidden = true;
    }

    public override string ToString()
    {
      return String.Format("(menu {0})", base.ToString());
    }

    #region Drawing
    public override void Render(RenderArgs args)
    {
      // We use the same formula as the horizontal packer to find out
      // our own offset. This is used to handle the mouse events
      // because the EventLock does not translate the values before
      // sending it.
      RenderArgs args0 = args.Clone();
      args0.TranslateX += Coords.X + OuterPadding.Left + InnerPadding.Left;
      args0.TranslateY += Coords.Y + OuterPadding.Top + InnerPadding.Top;
      translate = args0.Vector;

      // Check for exceeding
      int right = translate.X + Size.Width + manager.MenuTitlePadding.Right;
      if (right > manager.Size.Width)
      {
	// We have to adjust things over
	int off = right - manager.Size.Width;
	args = args.Clone();
	args.TranslateX -= off;
	translate.X -= off;
      }

      // Draw the element
      manager.Render(args, this);
      base.Render(args);

      // Trace the items
      if (IsTraced)
      {
	foreach (Sprite s in new ArrayList(Sprites))
	{
	  Rectangle2 r = new Rectangle2(translate, GetSize(s));
	  GuiManager.DrawRect(args.Surface, r, manager.TraceColor);
	}
      }
    }

    public void ShowMenu()
    {
      IsHidden = false;
      manager.SpriteContainer.EventLock = this;
    }

    public void HideMenu()
    {
      IsHidden = true;
      manager.SpriteContainer.EventLock = null;

      if (controller != null)
	controller.IsSelected = false;
    }
    #endregion

    #region Sprites
    public void Add(GuiMenuItem gmi)
    {
      AddHead(gmi);
      gmi.Menu = this;
    }
    #endregion

    #region Geometry
    private Vector2 translate = new Vector2();

    public override Padding OuterPadding
    {
      get { return manager.MenuPopupPadding; }
    }

    public override Dimension2 Size
    {
      get
      {
	// Clear out the popups. If the GMI has a menu associated with
	// it, it uses that for the width, which would produce an
	// infinite loop for processing. Removing the association
	// allows the size to be retrieved properly.
	foreach (GuiMenuItem gmi in Sprites)
	  gmi.Menu = null;

	// Get the base
	Dimension2 d = base.Size;

	// Reassociate this item
	foreach (GuiMenuItem gmi in Sprites)
	  gmi.Menu = this;

	// Return the dimension
	return d;
      }
    }
    #endregion

    #region Events
    /// <summary>
    /// Menus are a special case of a sprite. If the mouse is
    /// selected, then it shows the entire sprite, regardless of the
    /// packing size.
    /// </summary>
    public override bool OnMouseButton(object sender, MouseArgs args)
    {
      // If we are being held down, pick up the marble
      if (args.IsButton1)
      {
	ShowMenu();
      }
      else
      {
	// Check for an item
	if (selected != null)
	{
	  selected.OnMenuSelected(selectedIndex);
	  selected.IsSelected = false;
	  selected = null;
	}

	// Remove the menu
	HideMenu();
      }

      // We are finished
      return true;
    }

    /// <summary>
    /// Uses the mouse motion to determine what menu item is actually
    /// selected and hilight it. If the menu is not selected, it does
    /// nothing.
    /// </summary>
    public override bool OnMouseMotion(object sender, MouseArgs args)
    {
      // Pull out some stuff
      int x = args.X - translate.X - Coords.X;
      int y = args.Y - translate.Y - Coords.Y;
      int relx = args.RelativeX;
      int rely = args.RelativeY;

      // Don't bother if we are not selected
      if (IsHidden)
	return false;

      // Retrieve the item for these coordinates
      int ndx = 0;
      GuiMenuItem gmi = (GuiMenuItem) SelectSprite(new Vector2(x, y), ref ndx);

      // Check to see if we need to deselect an old one
      if (selected != null && (gmi == null || gmi != selected))
      {
	// Clear out selected
	selected.IsSelected = false;
	selected = null;
      }

      // If we have a menu, select it
      if (gmi != null)
      {
	gmi.IsSelected = true;
	selected = gmi;
	selectedIndex = ndx;
	return true;
      }

      /*
      // We don't have a menu item and we are not selecting
      // anything. See if we are current over a title of another menu.
      int x1 = x + Coords.X;
      int y1 = y + Coords.Y;
      ArrayList moreSprites =
	manager.SpriteContainer.GetSprites(new Vector2(x1, y1));

      foreach (Sprite s in moreSprites)
      {
	if (s is GuiMenuTitle)
	{
	  // This is the menu we should select
	  GuiMenuTitle gmt = (GuiMenuTitle) s;
	  IsSelected = false;
	  HideMenu();
	  gm.IsSelected = true;
	  gm.Popup.ShowMenu();
	}
      }
      */

      // We are done processing
      return true;
    }
    #endregion

    #region Properties
    private GuiMenuItem selected = null;
    private IMenuPopupController controller = null;
    private int selectedIndex = 0;

    public IMenuPopupController Controller
    {
      get { return controller; }
      set { controller = value; }
    }
    #endregion
  }
}
