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

using SdlDotNet.Utility;
using SdlDotNet.Sprites;
using SdlDotNet;
using System;
using System.Collections;
using System.Drawing;

namespace MfGames.Sdl.Gui
{
  /// <summary>
  /// A fairly complicated class that creates a menu bar that
  /// stretches across the top of a region. This bar handles menus in
  /// addition to normal sprites, allowing for a fairly complicated
  /// menubar system. The region itself determines where a menu may appear.
  /// </summary>
  public class GuiMenuBar : HorizontalPacker
  {
    public GuiMenuBar(GuiManager manager, int x1, int x2, int baselineY)
      : base(manager)
    {
      Coords.Z = 10000;
      this.x1 = x1;
      this.x2 = x2;
      this.baselineY = baselineY;
    }

    #region Drawing
    public override void Render(RenderArgs args)
    {
      // Draw ourselves, then our components
      manager.Render(args, this);
      base.Render(args);
    }
    #endregion

    #region Sprites
    public void AddLeft(Sprite s)
    {
      AddHead(s);

      if (s is GuiMenuTitle)
	((GuiMenuTitle) s).MenuBar = this;
    }

    public void AddRight(Sprite s)
    {
      AddTail(s);

      if (s is GuiMenuTitle)
	((GuiMenuTitle) s).MenuBar = this;
    }
    #endregion

    #region Geometry
    private int x1 = 0;
    private int x2 = 0;
    private int baselineY = 0;

    public override Vector Coords
    {
      get { return new Vector(x1, baselineY, base.Coords.Z); }
    }

    public override int HorizontalWidth
    {
      get { return x2 - x1; }
    }

    public override Padding InnerPadding
    {
      get { return manager.MenuTitlePadding; }
    }

    public override Padding MarginPadding
    {
      get { return manager.MenuBarPadding; }
    }
    #endregion

    #region Operators
    public override string ToString()
    {
      return String.Format("(menu-bar {0})", base.ToString());
    }
    #endregion
  }
}
