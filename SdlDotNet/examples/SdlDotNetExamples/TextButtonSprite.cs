#region LICENSE
/*
 * Copyright (C) 2004 - 2007 David Hudson (jendave@yahoo.com)
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
#endregion LICENSE

using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

using SdlDotNet.Core;
using SdlDotNet.Input;
using SdlDotNet.Graphics;
using SdlDotNet.Graphics.Sprites;

namespace SdlDotNetExamples
{
    /// <summary>
    /// 
    /// </summary>
    public class TextButtonSprite : TextSprite
    {
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<TextButtonSpriteEventArgs> TextButtonSpriteSelected;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textItem"></param>
        /// <param name="font"></param>
        public TextButtonSprite(
            string textItem,
            SdlDotNet.Graphics.Font font)
            : base(textItem, font)
        { }

        void OnTextButtonSpriteSelect(TextButtonSpriteEventArgs e)
        {
            if (TextButtonSpriteSelected != null)
            {
                TextButtonSpriteSelected(this, e);
            }
        }

        /// <summary>
        /// If the mouse click hits a sprite, 
        /// then the sprite will be marked as 'being dragged'
        /// </summary>
        /// <param name="args"></param>
        public override void Update(MouseButtonEventArgs args)
        {
            if (args == null)
            {
                throw new ArgumentNullException("args");
            }
            if (this.IntersectsWith(new Point(args.X, args.Y)))
            {
                if (args.ButtonPressed)
                {
                    if (args.Button == MouseButton.PrimaryButton)
                    {
                        this.BackgroundColor = Color.Red;
                        this.BeingDragged = true;
                        OnTextButtonSpriteSelect(new TextButtonSpriteEventArgs(this, this.Text));
                    }
                }
                else
                {
                    this.BackgroundColor = Color.Violet;
                    this.BeingDragged = false;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        public override void Update(MouseMotionEventArgs args)
        {
            if (args == null)
            {
                throw new ArgumentNullException("args");
            }
            if (this.IntersectsWith(new Point(args.X, args.Y)) && !this.BeingDragged)
            {
                this.BackgroundColor = Color.Violet;
            }
            else if (!this.BeingDragged)
            {
                this.BackgroundColor = Color.Black;
            }
            else
            {
                this.BackgroundColor = Color.Red;
            }
        }
    }
}
