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



namespace SCSharp.UI
{
    /// <summary>
    /// 
    /// </summary>
    public class Pcx
    {
        /// <summary>
        /// 
        /// </summary>
        public Pcx()
        {
        }

        ushort xmin;
        ushort xmax;
        ushort ymin;
        ushort ymax;

        bool withAlpha;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="translucentIndex"></param>
        /// <param name="transparentIndex"></param>
        public void ReadFromStream(Stream stream, int translucentIndex, int transparentIndex)
        {
            withAlpha = translucentIndex != -1 || transparentIndex != -1;

            byte magic = Utilities.ReadByte(stream);
            if (magic != 0x0A)
            {
                throw new Exception("stream is not a valid .pcx file");
            }

            /*version =*/
            Utilities.ReadByte(stream);
            /*encoding =*/
            Utilities.ReadByte(stream);
            ushort bpp = Utilities.ReadByte(stream);
            xmin = Utilities.ReadWord(stream);
            ymin = Utilities.ReadWord(stream);
            xmax = Utilities.ReadWord(stream);
            ymax = Utilities.ReadWord(stream);
            /*ushort h_dpi =*/
            Utilities.ReadWord(stream);
            /*ushort v_dpi =*/
            Utilities.ReadWord(stream);
            stream.Position += 48; /* skip the header palette */
            stream.Position++;    /* skip the reserved byte */
            ushort numplanes = Utilities.ReadByte(stream);
            /*ushort stride =*/
            Utilities.ReadWord(stream);
            /*headerInterp =*/
            Utilities.ReadWord(stream);
            /*videoWidth =*/
            Utilities.ReadWord(stream);
            /*videoHeight =*/
            Utilities.ReadWord(stream);
            stream.Position += 54;

            if (bpp != 8 || numplanes != 1)
                throw new Exception("unsupported .pcx image type");

            width = (ushort)(xmax - xmin + 1);
            height = (ushort)(ymax - ymin + 1);

            long imageData = stream.Position;

            stream.Position = stream.Length - 256 * 3;
            /* read the palette */
            palette = new byte[256 * 3];
            stream.Read(palette, 0, 256 * 3);

            stream.Position = imageData;

            /* now read the image data */
            data = new byte[width * height * 4];

            int idx = 0;
            while (idx < data.Length)
            {
                byte b = Utilities.ReadByte(stream);
                byte count;
                byte value;

                if ((b & 0xC0) == 0xC0)
                {
                    /* it's a count byte */
                    count = (byte)(b & 0x3F);
                    value = Utilities.ReadByte(stream);
                }
                else
                {
                    count = 1;
                    value = b;
                }

                for (int i = 0; i < count; i++)
                {
                    /* this stuff is endian
                     * dependent... for big endian
                     * we need the "idx +"'s
                     * reversed */
                    data[idx + 3] = palette[value * 3 + 0];
                    data[idx + 2] = palette[value * 3 + 1];
                    data[idx + 1] = palette[value * 3 + 2];
                    if (withAlpha)
                    {
                        if (value == translucentIndex)
                        {
                            data[idx + 0] = 0xd0;
                        }
                        else if (value == transparentIndex)
                        {
                            data[idx + 0] = 0x00;
                        }
                        else
                        {
                            data[idx + 0] = 0xff;
                        }
                    }

                    idx += 4;
                }
            }
        }


        byte[] data;
        byte[] palette;

        ushort width;
        ushort height;

        /// <summary>
        /// 
        /// </summary>
        public byte[] RgbaData
        {
            get { return data; }
        }

        /// <summary>
        /// 
        /// </summary>
        public byte[] RgbData
        {
            get
            {
                byte[] foo = new byte[width * height * 3];
                int i = 0;
                int j = 0;
                while (i < data.Length)
                {
                    foo[j++] = data[i++];
                    foo[j++] = data[i++];
                    foo[j++] = data[i++];
                    i++;
                }
                return foo;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public byte[] Palette
        {
            get { return palette; }
        }

        /// <summary>
        /// 
        /// </summary>
        [CLSCompliant(false)]
        public ushort Width
        {
            get { return width; }
        }

        /// <summary>
        /// 
        /// </summary>
        [CLSCompliant(false)]
        public ushort Height
        {
            get { return height; }
        }

        /// <summary>
        /// 
        /// </summary>
        [CLSCompliant(false)]
        public ushort Depth
        {
            get { return (ushort)(withAlpha ? 32 : 24); }
        }

        /// <summary>
        /// 
        /// </summary>
        [CLSCompliant(false)]
        public ushort Stride
        {
            get { return (ushort)(width * (3 + (withAlpha ? 1 : 0))); }
        }
    }
}
