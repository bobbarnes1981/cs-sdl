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

using System.Drawing;

namespace MfGames.Sdl.Sprites
{
  /// <summary>
  /// This creates a viewport that centers a sprite, but ensures that
  /// a specific region is always visible. This attempts to center,
  /// the normalize the view (to keep from showing off the "map" or
  /// bounds).
  /// </summary>
  public class BoundedCenterViewport : CenteredViewport
  {
    private Rectangle region = Rectangle.Empty;

    /// <summary>
    /// Constructs a viewport to center on a specific sprite.
    /// </summary>
    public BoundedCenterViewport(Sprite centerOnSprite, Rectangle region)
      : base(centerOnSprite)
    {
      this.region = region;
    }

    /// <summary>
    /// This gets the upper-left corner of the viewport, based on the
    /// given coordinates of the actual screen. This enables a
    /// viewport to only show in a specific part of the screen. The
    /// point returned is relative to the sprite manager.
    /// </summary>
    public override void AdjustViewport(ref RenderArgs args)
    {
      // Get the center point
      base.AdjustViewport(ref args);

      // Check to see if the window is too small
      bool doWidth = true;
      bool doHeight = true;

      if (region.Width < args.Size.Width)
	doWidth = false;

      if (region.Height < args.Size.Height)
	doHeight = false;

      if (!doWidth && !doHeight)
	return;

      // Find out the "half" point for the sprite in the view
      int mx = Sprite.Coords.X + Sprite.Size.Width / 2;
      int my = Sprite.Coords.Y + Sprite.Size.Height / 2;

      // Figure out the coordinates
      int x1 = mx - args.Size.Width / 2;
      int x2 = mx + args.Size.Width / 2;
      int y1 = my - args.Size.Height / 2;
      int y2 = my + args.Size.Height / 2;

      // Make sure we don't exceed the bounds
      if (doWidth && x1 < region.Left)
	args.TranslateX -= region.Left - x1;

      if (doHeight && y1 < region.Top)
	args.TranslateY -= region.Top - y1;

      if (doWidth && x2 > region.Right)
	args.TranslateX += x2 - region.Right;

      if (doHeight && y2 > region.Bottom)
	args.TranslateY += y2 - region.Bottom;
    }
  }
}
