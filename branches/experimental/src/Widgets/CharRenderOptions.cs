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

    public class CharRenderOptions
    {
        #region Fields

        Color backColor;
        bool bold;
        Color foreColor;
        bool italic;
        bool underline;

        #endregion Fields

        #region Constructors

        public CharRenderOptions(Color foreColor) {
            this.foreColor = foreColor;
            this.backColor = Color.Empty;
            this.bold = false;
            this.italic = false;
            this.underline = false;
        }

        #endregion Constructors

        #region Properties

        public Color BackColor {
            get { return backColor; }
            set { backColor = value; }
        }

        public bool Bold {
            get { return bold; }
            set { bold = value; }
        }

        public Color ForeColor {
            get { return foreColor; }
            set { foreColor = value; }
        }

        public bool Italic {
            get { return italic; }
            set { italic = value; }
        }

        public bool Underline {
            get { return underline; }
            set { underline = value; }
        }

        #endregion Properties
    }
}