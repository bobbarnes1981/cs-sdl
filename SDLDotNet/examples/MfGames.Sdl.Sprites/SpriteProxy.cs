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
using SdlDotNet;
using System;

namespace MfGames.Sdl.Sprites
{
  public class SpriteProxy : Sprite
  {
    public SpriteProxy(Sprite sprite)
    {
      Sprite = sprite;
    }

    #region Display
    public override void Render(RenderArgs args)
    {
      sprite.Render(args);
    }
    #endregion

    #region Events
    public override bool IsMouseSensitive
    {
      get { return sprite.IsMouseSensitive; }
    }

    public override bool IsKeyboardSensitive
    {
      get { return sprite.IsKeyboardSensitive; }
    }

    public override bool IsTickable
    {
      get { return sprite.IsTickable; }
      set { sprite.IsTickable = value; }
    }

    /// <summary>
    /// Processes the keyboard. If this function returns true, then
    /// the system no longer processes the keyboard event. If it is
    /// not true, then the next sprite that is keyboard sensitive is
    /// processed.
    /// </summary>
    public override bool OnKeyboard(object sender, KeyboardEventArgs e)
    {
      return sprite.OnKeyboard(this, e);
    }

    /// <summary>
    /// Processes a mouse button. This event is trigger by the SDL
    /// system. If this function returns true, then processing is
    /// stopped. If it returns false, then the next sprite is
    /// processed.
    /// </summary>
    public override bool OnMouseButton(object sender, MouseArgs args)
    {
      return sprite.OnMouseButton(this, args);
    }

    /// <summary>
    /// Processes a mouse motion event. This event is triggered by
    /// SDL. If this returns true, then processing of the motion event
    /// is stopped, otherwise the next sprite is processed. Only
    /// sprites that are MouseSensitive are processed.
    /// </summary>
    public override bool OnMouseMotion(object sender, MouseArgs args)
    {
      return sprite.OnMouseMotion(this, args);
    }

    /// <summary>
    /// All sprites are tickable, regardless if they actual do
    /// anything. This ensures that the functionality is there, to be
    /// overriden as needed.
    /// </summary>
    public override void OnTick(TickArgs args)
    {
      sprite.OnTick(args);
    }
    #endregion

    #region Geometry
    public override Vector3 Coords
    {
      get { return sprite.Coords; }
      set { sprite.Coords = value; }
    }

    public override Dimension2 Size
    {
      get { return sprite.Size; }
    }

    public override bool IntersectsWith(Vector2 point)
    {
      return Bounds.IntersectsWith(point);
    }

    public override bool IntersectsWith(Rectangle2 rect)
    {
      return Bounds.IntersectsWith(rect);
    }
    #endregion

    #region Properties
    private Sprite sprite = null;

    public Sprite Sprite
    {
      get { return sprite; }
      set
      {
	if (value == null)
	  throw new SpriteException("Cannot assign a null sprite");

	sprite = value;
      }
    }

    public override bool IsHidden
    {
      get { return sprite.IsHidden; }
      set { sprite.IsHidden = value; }
    }

    public override bool IsTraced
    {
      get { return sprite.IsTraced; }
      set { sprite.IsTraced = value; }
    }
    #endregion
  }
}
