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
using MfGames.Sdl.Drawable;
using SdlDotNet;
using System;
using System.Drawing;

namespace MfGames.Sdl.Demos
{
  public class ClickSprite : AnimatedSprite
  {
    private IDrawable d1 = null;
    private IDrawable d2 = null;

    public ClickSprite(IDrawable d1, IDrawable d2, Vector2 coords)
      : base(d1, coords)
    {
      this.d1 = d1;
      this.d2 = d2;
    }

    public override string ToString()
    {
      return String.Format("(click {0})", base.ToString());
    }

    #region Events
    public override bool IsMouseSensitive { get { return true; } }

    public override bool OnMouseButton(object sender, MouseArgs args)
    {
      // If we are not being released, don't bother
      if (args.IsButton1)
	return true;

      // Switch the image
      if (Drawable == d1)
	Drawable = d2;
      else
	Drawable = d1;

      // We are done processing
      return true;
    }
    #endregion
  }
}
