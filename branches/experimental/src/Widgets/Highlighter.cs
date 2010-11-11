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

    public class Highlighter : Widget
    {
        #region Fields

        Surface highlightImage;

        #endregion Fields

        #region Constructors

        public Highlighter(string name)
            : base(name)
        {
            base.BackColor = Color.Transparent;
            this.MouseEnter += new EventHandler(Highlighter_MouseEnter);
            this.MouseLeave += new EventHandler(Highlighter_MouseLeave);
        }

        #endregion Constructors

        #region Properties

        public Surface HighlightImage
        {
            get { return highlightImage; }
            set {
                if (highlightImage != null) {
                    highlightImage.Close();
                }
                highlightImage = value;
                highlightImage.TransparentColor = Color.Transparent;
                highlightImage.Transparent = true;
                this.Size = highlightImage.Size;
                RequestRedraw();
            }
        }

        #endregion Properties

        #region Methods

        public override void FreeResources()
        {
            base.FreeResources();
            if (highlightImage != null) {
                highlightImage.Close();
                highlightImage = null;
            }
        }

        protected override void DrawBuffer()
        {
            base.DrawBuffer();
            if (base.MouseInBounds) {
                base.Buffer.Blit(highlightImage, new Point(0, 0));
            }
            base.DrawBorder();
        }

        void Highlighter_MouseEnter(object sender, EventArgs e)
        {
            RequestRedraw();
        }

        void Highlighter_MouseLeave(object sender, EventArgs e)
        {
            RequestRedraw();
        }

        #endregion Methods
    }
}