namespace SdlDotNet.Graphics
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public struct GlyphData
    {
        #region Fields

        int advance;
        char glyph;
        int height;
        int width;
        int xMax;
        int xMin;
        int yMax;
        int yMin;

        #endregion Fields

        #region Constructors

        public GlyphData(char glyph, int xMin, int xMax, int yMin, int yMax, int advance) {
            this.glyph = glyph;
            this.xMin = xMin;
            this.xMax = xMax;
            this.yMin = yMin;
            this.yMax = yMax;
            this.advance = advance;

            this.width = xMax - xMin;
            this.height = yMax - yMin;
        }

        #endregion Constructors

        #region Properties

        public int Advance {
            get { return advance; }
            set { advance = value; }
        }

        public char Glyph {
            get { return glyph; }
            set { glyph = value; }
        }

        public int Height {
            get { return height; }
            set { height = value; }
        }

        public int Width {
            get { return width; }
            set { width = value; }
        }

        public int XMax {
            get { return xMax; }
            set { xMax = value; }
        }

        public int XMin {
            get { return xMin; }
            set { xMin = value; }
        }

        public int YMax {
            get { return yMax; }
            set { yMax = value; }
        }

        public int YMin {
            get { return yMin; }
            set { yMin = value; }
        }

        #endregion Properties
    }
}