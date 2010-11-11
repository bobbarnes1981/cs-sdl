#region Header

/*
 * Copyright (C) 2010 Pikablu
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

#endregion Header

namespace SdlDotNet.Widgets
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Text;

    using SdlDotNet.Graphics;

    public class ListBoxTextItem : IListBoxItem
    {
        #region Fields

        SdlDotNet.Graphics.Surface buffer;
        SdlDotNet.Graphics.Font font;
        Color foreColor;
        Surface image;
        bool selected;
        string text;

        #endregion Fields

        #region Constructors

        public ListBoxTextItem(SdlDotNet.Graphics.Font font, string text) {
            this.font = font;
            this.text = text;
            this.foreColor = Color.Black;

            DrawBuffer();
        }

        #endregion Constructors

        #region Properties

        public SdlDotNet.Graphics.Surface Buffer {
            get { return buffer; }
        }

        public SdlDotNet.Graphics.Font Font {
            get { return font; }
            set {
                if (font != null) {
                    font.Close();
                }
                font = value;
                DrawBuffer();
            }
        }

        public Color ForeColor {
            get { return foreColor; }
            set {
                foreColor = value;
                DrawBuffer();
            }
        }

        public Surface Image {
            get { return image; }
            set {
                image = value;
                DrawBuffer();
            }
        }

        public bool Selected {
            get { return selected; }
            set {
                selected = value;
            }
        }

        public Object Tag {
            get;
            set;
        }

        public string Text {
            get { return text; }
            set {
                text = value;
                DrawBuffer();
            }
        }

        public string TextIdentifier {
            get { return Text; }
            set { Text = value; }
        }

        #endregion Properties

        #region Methods

        public void FreeResources() {
            if (buffer != null) {
                buffer.Close();
            }
            if (font != null) {
                font.Close();
            }
        }

        private void DrawBuffer() {
            if (buffer != null) {
                buffer.Close();
            }
            Surface textSurface = TextRenderer.RenderTextBasic(font, text, null, foreColor, false, 0, 0, 0, 0);
            if (image != null) {
                Surface newBuffer = new Surface(new Size(image.Width + (5 * 2) + textSurface.Width, image.Height + (5 * 2)));
                newBuffer.Transparent = true;
                newBuffer.TransparentColor = Color.Transparent;
                newBuffer.Fill(Color.Transparent);
                newBuffer.Blit(image, new Point(5, 5));
                newBuffer.Blit(textSurface, new Point(5 + image.Width + 5, 5));
                buffer = newBuffer;
                textSurface.Close();
            } else {
                buffer = textSurface;
            }
        }

        #endregion Methods
    }
}