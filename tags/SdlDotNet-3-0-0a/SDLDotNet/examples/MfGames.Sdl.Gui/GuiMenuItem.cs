/*
 * $RCSfile$
 * Copyright (C) 2004 D. R. E. Moonfire (d.moonfire@mfgames.com)
 *
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later version.
 * 
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 * 
 * You should have received a copy of the GNU Lesser General Public
 * License along with this library; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
 */

using MfGames.Utility;
using MfGames.Sdl.Sprites;
using SdlDotNet;
using System;

namespace MfGames.Sdl.Gui
{
  public class GuiMenuItem : HorizontalPacker
  {
    public GuiMenuItem(GuiManager manager)
      : base(manager)
    {
    }

    public GuiMenuItem(GuiManager manager, string text)
      : base(manager)
    {
      AddLeft(new TextSprite(text, manager.BaseFont));
    }

    #region Sprites
    public void AddLeft(Sprite s)
    {
      AddHead(s);
    }

    public void AddRight(Sprite s)
    {
      AddTail(s);
    }
    #endregion

    #region Drawing
    public override void Render(RenderArgs args)
    {
      // We draw the rectangle normally
      manager.Render(args, this);
      base.Render(args);
    }
    #endregion

    #region Geometry
    public override bool IntersectsWith(Vector2 point)
    {
      // Menu items are packed by their outer padding instead of the
      // normal inner, so this has to be adjusted.
      Vector2 v = new Vector2(point);
      v.X -= OuterPadding.Left;
      v.Y -= OuterPadding.Top;
      return OuterBounds.IntersectsWith(v);
    }

    public override bool IntersectsWith(Rectangle2 rect)
    {
      // Menu items are packed by their outer padding instead of the
      // normal inner, so this has to be adjusted.
      Rectangle2 r = new Rectangle2(rect);
      r.Coords.X -= OuterPadding.Left;
      r.Coords.Y -= OuterPadding.Top;
     return OuterBounds.IntersectsWith(r);
    }
    #endregion

    #region Events
    public event MenuItemHandler ItemSelectedEvent;

    public virtual void OnMenuSelected(int index)
    {
      if (ItemSelectedEvent != null)
      {
	ItemSelectedEvent(index);
      }
    }
    #endregion

    #region Operators
    public override string ToString()
    {
      return String.Format("(menu-item {0})", base.ToString());
    }
    #endregion

    #region Properties
    private bool isSelected = false;
    private GuiMenuPopup menu = null;

    public bool IsSelected
    {
      get { return isSelected; }
      set { isSelected = value; }
    }

    public GuiMenuPopup Menu
    {
      get { return menu; }
      set { menu = value; }
    }

    public override Padding InnerPadding
    {
      get { return manager.MenuItemInnerPadding; }
    }

    public override Padding MarginPadding
    {
      get { return manager.MenuItemPadding; }
    }

    public override int HorizontalWidth
    {
      get
      {
	if (menu == null)
	  return base.HorizontalWidth;
	else
	  return menu.Size.Width;
      }
    }
    #endregion
  }

  public delegate void MenuItemHandler(int index);
}
