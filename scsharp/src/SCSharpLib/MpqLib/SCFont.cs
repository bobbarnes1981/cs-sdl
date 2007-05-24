#region LICENSE
//
// Authors:
//	Chris Toshok (toshok@hungry.com)
//
// (C) 2006 The Hungry Programmers (http://www.hungry.com/)
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
#endregion LICENSE

using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

using SCSharp.UI;

namespace SCSharp.MpqLib
{
    /// <summary>
    /// 
    /// </summary>
    public class SCFont : IMpqResource
    {
        Stream stream;

        /// <summary>
        /// 
        /// </summary>
        public SCFont()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        public void ReadFromStream(Stream stream)
        {
            this.stream = stream;
            ReadFontHeader();
            ReadGlyphOffsets();

            glyphs = new Dictionary<int, Glyph>();
        }

        void ReadFontHeader()
        {
            /*uint name =*/
            Utilities.ReadDWord(stream);

            lowIndex = Utilities.ReadByte(stream);
            highIndex = Utilities.ReadByte(stream);

            if (lowIndex > highIndex)
            {
                byte tmp = lowIndex;
                lowIndex = highIndex;
                highIndex = tmp;
            }
            maxWidth = Utilities.ReadByte(stream);
            maxHeight = Utilities.ReadByte(stream);
            /*uint unknown =*/
            Utilities.ReadDWord(stream);
        }

        Dictionary<uint, uint> offsets;

        void ReadGlyphOffsets()
        {
            offsets = new Dictionary<uint, uint>();
            for (uint c = lowIndex; c < highIndex; c++)
            {
                offsets.Add(c, Utilities.ReadDWord(stream));
            }
        }

        Glyph GetGlyph(int glyphID)
        {
            if (glyphs.ContainsKey(glyphID))
            {
                return glyphs[glyphID];
            }

            stream.Position = offsets[(uint)glyphID];

            byte letterWidth = Utilities.ReadByte(stream);
            byte letterHeight = Utilities.ReadByte(stream);
            byte letterXOffset = Utilities.ReadByte(stream);
            byte letterYOffset = Utilities.ReadByte(stream);

            byte[,] bitmap = new byte[letterHeight, letterWidth];

            int x, y;
            x = letterWidth - 1;
            y = letterHeight - 1;
            while (true)
            {
                byte b = Utilities.ReadByte(stream);
                int count = (b & 0xF8) >> 3;
                byte cmap_entry = (byte)(b & 0x07);

                for (int i = 0; i < count; i++)
                {
                    bitmap[y, x] = 0;
                    x--;
                    if (x < 0)
                    {
                        x = letterWidth - 1;
                        y--;
                        if (y < 0)
                        {
                            goto done;
                        }
                    }
                }

                bitmap[y, x] = (byte)cmap_entry;
                x--;
                if (x < 0)
                {
                    x = letterWidth - 1;
                    y--;
                    if (y < 0)
                    {
                        goto done;
                    }
                }
            }
        done:
            glyphs.Add(glyphID,
                    new Glyph(letterWidth,
                           letterHeight,
                           letterXOffset,
                           letterYOffset,
                           bitmap));

            return glyphs[glyphID];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Glyph this[int index]
        {
            get
            {
                if (index < lowIndex || index > highIndex)
                {
                    throw new ArgumentOutOfRangeException("index",
                                   String.Format("value of {0} out of range of {1}-{2}", index, lowIndex, highIndex));
                }

                return GetGlyph(index);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int SpaceSize
        {
            get { return this[109 - 1].Width; /* 109 = ascii for 'm' */ }
        }

        /// <summary>
        /// 
        /// </summary>
        public int LineSize
        {
            get { return maxHeight; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int MaxWidth
        {
            get { return maxWidth; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int MaxHeight
        {
            get { return maxHeight; }
        }

        Dictionary<int, Glyph> glyphs;
        byte highIndex;
        byte lowIndex;
        byte maxWidth;
        byte maxHeight;

        /// <summary>
        /// 
        /// </summary>
        public void DumpGlyphs()
        {
            for (int c = lowIndex; c < highIndex; c++)
            {
                Console.WriteLine("Letter: {0}", c);
                DumpGlyph(c);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="glyphId"></param>
        public void DumpGlyph(int glyphId)
        {
            Glyph g = GetGlyph(glyphId);
            byte[,] bitmap = g.Bitmap;
            for (int y = g.Height - 1; y >= 0; y--)
            {
                for (int x = g.Width - 1; x >= 0; x--)
                {
                    if (bitmap[y, x] == 0)
                    {
                        Console.Write(" ");
                    }
                    else
                    {
                        Console.Write("#");
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}
