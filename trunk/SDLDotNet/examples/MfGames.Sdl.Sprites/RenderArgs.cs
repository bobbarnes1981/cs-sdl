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
using System.Drawing;

namespace MfGames.Sdl.Sprites
{
  public class RenderArgs
  {
    public RenderArgs(Surface surface, Dimension2 size)
    {
      Surface = surface;
      Size = size;
    }

    public RenderArgs Clone()
    {
      // Create a new one
      RenderArgs args = new RenderArgs(Surface, Size);
      args.tx = tx;
      args.ty = ty;
      return args;
    }

    #region Clipping
    private Rectangle2 origClip = null;
    private Rectangle2 clip = null;

    public void ClearClipping()
    {
      if (origClip == null)
	Surface.ClipRectangle  = new Rectangle(0, 0,
					  Surface.Width, Surface.Height);
      else
	Surface.ClipRectangle = origClip;
    }

    public void SetClipping(Rectangle2 rect)
    {
      clip = rect;
      Surface.ClipRectangle = clip;
    }
    #endregion

    #region Geometry
    public Rectangle2 Translate(Rectangle2 rect)
    {
      Rectangle2 r = new Rectangle2(rect);
      r.Coords.X += TranslateX;
      r.Coords.Y += TranslateY;
      return r;
    }

    public void TranslateBy(Sprite s)
    {
      TranslateX += s.Coords.X;
      TranslateY += s.Coords.Y;
    }
    #endregion

    #region Properties
    private Surface surface = null;
    private int tx = 0;
    private int ty = 0;
    private Dimension2 size = new Dimension2();

    public Vector2 Vector
    {
      get { return new Vector2(tx, ty); }
    }

    public Surface Surface
    {
      get { return surface; }
      set
      {
	if (value == null)
	  throw new SpriteException("Cannot assign a null surface");

	surface = value;
      }
    }

    public Dimension2 Size
    {
      get { return size; }
      set
      {
	if (value == null)
	  throw new SpriteException("Cannot assign a null size!");

	size = value;
      }
    }

    public int TranslateX
    {
      get { return tx; }
      set { tx = value; }
    }

    public int TranslateY
    {
      get { return ty; }
      set { ty = value; }
    }
    #endregion
  }
}
