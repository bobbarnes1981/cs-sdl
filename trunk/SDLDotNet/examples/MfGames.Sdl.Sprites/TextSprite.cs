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
using System.Drawing;

namespace MfGames.Sdl.Sprites
{
  /// <summary>
  /// Implements a basic font that is given a font and a string and
  /// generates an appropriate surface from that font.
  /// </summary>
  public class TextSprite : Sprite
  {
    public TextSprite()
      : base()
    {
    }

    public TextSprite(string text, SdlDotNet.Font font)
      : base()
    {
      this.text = text;
      this.font = font;
    }

    public TextSprite(string text, SdlDotNet.Font font, Color color)
      : base()
    {
      this.text = text;
      this.font = font;
      this.color = color;
    }

    public TextSprite(string text, SdlDotNet.Font font, Vector2 coords)
      : base(coords)
    {
      this.text = text;
      this.font = font;
    }

    public TextSprite(string text, SdlDotNet.Font font, Color color,
		      Vector2 coords)
      : base(coords)
    {
      this.text = text;
      this.font = font;
      this.color = color;
    }

    public TextSprite(string text, SdlDotNet.Font font,
		      Vector3 coords)
      : base(coords)
    {
      this.text = text;
      this.font = font;
    }

    public TextSprite(string text, SdlDotNet.Font font, Color color,
		      Vector3 coords)
      : base(coords)
    {
      this.text = text;
      this.font = font;
      this.color = color;
    }

    #region Drawing
    /// <summary>
    /// Renders the font, if both the text and color and font are
    /// set. It stores the render in memory until it is used.
    /// </summary>
    private void RenderSurface()
    {
      // Clear it
      render = null;

      // Don't bother rendering if we don't have a text and a font
      if (Text == null || font == null)
	return;

      // Render it (Solid or Blended)
      try
      {
	//SDLColor c = new SDLColor(color.R, color.G, color.B);
	render = font.Render(Text, color);
      }
      catch (Exception e)
      {
	Error("Cannot render text: {0}", e);
	render = null;
      }
    }

    /// <summary>
    /// Displays the font using the arguments given.
    /// </summary>
    public override void Render(RenderArgs args)
    {
      // Blit out the render
      args.Surface.Blit(Surface,
		   new Rectangle(new Vector2(Coords.X + args.TranslateX,
					     Coords.Y + args.TranslateY),
				 render.Size));
    }
    #endregion

    #region Font Rendering
    protected Surface render = null;

    private SdlDotNet.Font font = null;

    private string text = null;

    private Color color = Color.White;

    public Color Color
    {
      get { return color; }
      set { color = value; render = null; }
    }

    public SdlDotNet.Font Font
    {
      get { return font; }
      set { font = value; render = null; }
    }

    public Surface Surface
    {
      get
      {
	if (render == null)
	  RenderSurface();

	if (render == null)
	  throw new SpriteException("Cannot render text");

	return render;
      }
    }

    public string Text
    {
      get { return text; }
      set { text = value; render = null; }
    }
    #endregion

    #region Geometry
    public override Dimension2 Size
    {
      get
      {
	if (render == null)
	  RenderSurface();

	if (render == null)
	  return new Dimension2(0, 0);
	else
	  return new Dimension2(render.Width, render.Height);
      }
    }
    #endregion

    #region Operators
    public override string ToString()
    {
      return String.Format("(text \"{0}\" {1})", text, base.ToString());
    }
    #endregion
  }
}
