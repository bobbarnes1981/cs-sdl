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
using System.Drawing;

namespace MfGames.Sdl.Gui
{
  /// <summary>
  /// Base class to manage all graphical GUI elements.
  /// </summary>
  public class GuiComponent : Sprite
  {
    public GuiComponent(GuiManager manager)
      : base()
    {
      this.manager = manager;
      IsTickable = true;
    }

    public GuiComponent(GuiManager manager, int z)
      : base(new Vector3(z))
    {
      this.manager = manager;
      IsTickable = true;
    }

    public GuiComponent(GuiManager manager, Vector2 loc)
      : base(loc)
    {
      this.manager = manager;
      IsTickable = true;
    }

    public GuiComponent(GuiManager manager, Rectangle2 rect)
      : base(rect.Coords)
    {
      this.manager = manager;
      this.size = rect.Size;
      IsTickable = true;
    }

    public GuiComponent(GuiManager manager, Vector3 loc)
      : base(loc)
    {
      this.manager = manager;
      IsTickable = true;
    }

    public override string ToString()
    {
      return String.Format("(gui {0})", Bounds, base.ToString());
    }

    #region Drawing
    public override void Render(RenderArgs args)
    {
      if (!IsTraced)
	return;

      // Draw the outer and the inner bounds
      GuiManager.DrawRect(args.Surface,
			  args.Translate(Bounds),
			  manager.BoundsTraceColor);
      GuiManager.DrawRect(args.Surface,
			  args.Translate(OuterBounds),
			  manager.OuterBoundsTraceColor);
    }
    #endregion

    #region Events
    private bool isDragable = false;
    private bool beingDragged = false;

    public virtual bool IsDragable
    {
      get { return isDragable; }
      set { isDragable = value; }
    }

    /// <summary>
    /// GUI components default to mouse sensitive.
    /// </summary>
    public override bool IsMouseSensitive { get { return true; } }

    public override bool OnMouseButton(object sender, MouseArgs args)
    {
      // If we cannot be dragged, don't worry about it
      if (!isDragable)
	return false;

      // If we are being held down, pick up the marble
      if (args.IsButton1)
      {
	// Change the Z-order
	Coords.Z += manager.DragZOrder;
	beingDragged = true;
	manager.SpriteContainer.EventLock = this;
      }
      else
      {
	// Drop it
	Coords.Z -= manager.DragZOrder;
	beingDragged = false;
	manager.SpriteContainer.EventLock = null;
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
      // Pull out some stuff
      int x = args.X;
      int y = args.Y;
      int relx = args.RelativeX;
      int rely = args.RelativeY;

      // If we cannot be dragged, don't worry about it
      if (!isDragable)
	return false;

      // Move the window as appropriate
      if (beingDragged)
      {
	Coords.X += relx;
	Coords.Y += rely;
	return true;
      }
      else
      {
	return false;
      }
    }
    #endregion

    #region Geometry
    private Dimension2 size = new Dimension2();

    public Rectangle2 OuterBounds
    {
      get
      {
	return new Rectangle2(OuterCoords, OuterSize);
      }
    }

    public virtual Dimension2 OuterSize
    {
      get
      {
	return new Dimension2(Size.Width + OuterPadding.Horizontal,
				  Size.Height + OuterPadding.Vertical);
      }
    }

    public virtual Vector3 OuterCoords
    {
      get
      {
	return new Vector3(Coords.X - OuterPadding.Left,
			   Coords.Y - OuterPadding.Top,
			   Coords.Z);
      }
    }

    public virtual Padding OuterPadding
    {
      get { return new Padding(0); }
    }

    public override Dimension2 Size
    {
      get { return size; }
      //set { size = value; }
    }

    public override bool IntersectsWith(Vector2 point)
    {
      return OuterBounds.IntersectsWith(point);
    }

    public override bool IntersectsWith(Rectangle2 rect)
    {
      return OuterBounds.IntersectsWith(rect);
    }
    #endregion

    #region Properties
    protected GuiManager manager = null;

    /// <summary>
    /// Contains the manager for this component.
    /// </summary>
    public GuiManager GuiManager
    {
      get { return manager; }
      set
      {
	if (value == null)
	  throw new Exception("Cannot assign a null manager");

	manager = value;
      }
    }
    #endregion
  }
}
