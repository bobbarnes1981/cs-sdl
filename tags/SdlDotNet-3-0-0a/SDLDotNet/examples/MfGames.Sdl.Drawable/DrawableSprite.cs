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

namespace MfGames.Sdl.Drawable
{
  public class DrawableSprite : Sprite
  {
    public DrawableSprite()
      : base()
    {
    }

    public DrawableSprite(IDrawable d)
      : base()
    {
      this.drawable = d;
    }

    public DrawableSprite(IDrawable d, int frame)
      : base()
    {
      this.drawable = d;
      this.frame = frame;
    }

    public DrawableSprite(IDrawable d, Vector2 coords)
      : base(coords)
    {
      this.drawable = d;
    }

    public DrawableSprite(IDrawable d, Vector3 coords)
      : base(coords)
    {
      this.drawable = d;
    }

    public DrawableSprite(IDrawable d, int frame, Vector2 coords)
      : base(coords)
    {
      this.drawable = d;
      this.frame = frame;
    }

    public DrawableSprite(IDrawable d, int frame, Vector3 coords)
      : base(coords)
    {
      this.drawable = d;
      this.frame = frame;
    }

    #region Display
    public override void Render(RenderArgs args)
    {
      // Blit the image on the surface
      args.Surface.Blit(CurrentFrame,
			new Rectangle(Coords.X + args.TranslateX,
				      Coords.Y + args.TranslateY,
				      Size.Width,
				      Size.Height));
    }
    #endregion

    #region Drawable
    private IDrawable drawable = null;

    private int frame = 0;

    /// <summary>
    /// Returns the current frame. For almost all drawables, this is
    /// the same as the "[0]" accessor. For sprites and other animated
    /// drawables, this returns whatever frame is consider "current".
    /// </summary>
    public Surface CurrentFrame
    {
      get
      {
	if (drawable == null || drawable[frame] == null)
	  throw new DrawableException("No drawable to return");

	return drawable[frame];
      }
    }

    /// <summary>
    /// Retrieves the drawable associated with this sprite. This will
    /// never return null (it will throw an exception if there is no
    /// drawable).
    /// </summary>
    public IDrawable Drawable
    {
      get
      {
	if (drawable == null)
	  throw new DrawableException("No drawable to return");

	return drawable;
      }

      set
      {
	drawable = value;
      }
    }

    /// <summary>
    /// Frame is the current frame number (drawable image) for the
    /// current sprite.
    /// </summary>
    public int Frame
    {
      get { return frame; }
      set { frame = value; }
    }

    /// <summary>
    /// Contains a read-only count of the number of frames in the
    /// sprite.
    /// </summary>
    public int FrameCount
    {
      get
      {
	if (drawable == null)
	  return 0;
	else
	  return drawable.FrameCount;
      }
    }
    #endregion

    #region Properties
    public override Dimension2 Size
    {
      get
      {
	if (drawable == null)
	  throw new DrawableException("No size for this drawable");
	else
	  return drawable.Size;
      }
    }
    #endregion
  }
}
