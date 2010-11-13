#region LICENSE
//
// Authors:
// Chris Toshok (toshok@hungry.com)
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
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

using System.Drawing;
using System.Drawing.Imaging;
using SdlDotNet.Graphics;
using SdlDotNet.Audio;
using SCSharp;
using SCSharp.MpqLib;

/* for the surface creation hack below */
using System.Reflection;
using Tao.Sdl;

namespace SCSharp.UI
{
    ///// <summary>
    /////
    ///// </summary>
    //public delegate void ReadyEventHandler(object sender, EventArgs e);

    /// <summary>
    ///
    /// </summary>
    public static class GuiUtility
    {

        static SCFont[] fonts;

        static string[] BroodwarFonts = {
"files\\font\\font8.fnt",
"files\\font\\font10.fnt",
"files\\font\\font14.fnt",
"files\\font\\font16.fnt",
"files\\font\\font16x.fnt"
};

        /// <summary>
        ///
        /// </summary>
        /// <param name="mpq"></param>
        /// <returns></returns>
        public static SCFont[] GetFonts(Mpq mpq)
        {
            if (fonts == null)
            {
                string[] fontList;
                fontList = BroodwarFonts;

                fonts = new SCFont[fontList.Length];

                for (int i = 0; i < fonts.Length; i++)
                {
                    fonts[i] = (SCFont)mpq.GetResource(fontList[i]);
                    Console.WriteLine("fonts[{0}] = {1}", i, fonts[i] == null ? "null" : "not null");
                }
            }
            return fonts;
        }

                //public static Surface RenderGlyph(Fnt font, Glyph g, byte[] palette, int offset)

        /// <summary>
        ///
        /// </summary>
        /// <param name="glyph"></param>
        /// <param name="palette"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static Surface RenderGlyph(Glyph glyph, byte[] palette, int offset)
        {
            if (glyph == null)
            {
                throw new ArgumentNullException("glyph");
            }
            if (palette == null)
            {
                throw new ArgumentNullException("palette");
            }
            byte[] buf = new byte[glyph.Width * glyph.Height * 4];
            int i = 0;

            for (int y = glyph.Height - 1; y >= 0; y--)
            {
                for (int x = glyph.Width - 1; x >= 0; x--)
                {
                    if (glyph.Bitmap[y, x] == 0)
                    {
                        buf[i + 0] = 0;
                    }
                    else if (glyph.Bitmap[y, x] == 1)
                    {
                        buf[i + 0] = 255;
                    }
                    else
                    {
                        buf[i + 0] = 128;
                    }

                    buf[i + 1] = palette[(glyph.Bitmap[y, x] + offset) * 3 + 2];
                    buf[i + 2] = palette[(glyph.Bitmap[y, x] + offset) * 3 + 1];
                    buf[i + 3] = palette[(glyph.Bitmap[y, x] + offset) * 3];

                    if (buf[i + 1] == 252 && buf[i + 2] == 0 && buf[i + 3] == 252)
                    {
                        buf[i + 0] = 0;
                    }

                    i += 4;
                }
            }

            return CreateSurfaceFromRgbaData(buf, (ushort)glyph.Width, (ushort)glyph.Height, 32, glyph.Width * 4);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="text"></param>
        /// <param name="font"></param>
        /// <param name="palette"></param>
        /// <returns></returns>
        public static Surface ComposeText(string text, SCFont font, byte[] palette)
        {
            return ComposeText(text, font, palette, -1, -1, 4);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="text"></param>
        /// <param name="font"></param>
        /// <param name="palette"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static Surface ComposeText(string text, SCFont font, byte[] palette, int offset)
        {
            return ComposeText(text, font, palette, -1, -1, offset);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="text"></param>
        /// <param name="font"></param>
        /// <param name="palette"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static Surface ComposeText(string text, SCFont font, byte[] palette, int width, int height,
        int offset)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }
            if (font == null)
            {
                throw new ArgumentNullException("font");
            }

            int i;
            /* create a run of text, for now ignoring any control codes in the string */
            StringBuilder run = new StringBuilder();
            for (i = 0; i < text.Length; i++)
            {
                if (text[i] == 0x0a /* allow newlines */||
                !Char.IsControl(text[i]))
                {
                    run.Append(text[i]);
                }
            }

            string rs = run.ToString();
            byte[] r = Encoding.ASCII.GetBytes(rs);

            int x;
            int y;
            int textHeight;
            int textWidth;

            /* measure the text first, optionally wrapping at width */
            textWidth = textHeight = 0;
            x = y = 0;

            for (i = 0; i < r.Length; i++)
            {
                int glyphWidth = 0;

                if (r[i] != 0x0a) /* newline */
                {
                    if (r[i] == 0x20) /* space */
                    {
                        glyphWidth = font.SpaceSize;
                    }
                    else
                    {
                        Glyph g = font[r[i] - 1];

                        glyphWidth = g.Width + g.XOffset;
                    }
                }

                if (r[i] == 0x0a ||
                (width != -1 && x + glyphWidth > width))
                {
                    if (x > textWidth)
                    {
                        textWidth = x;
                    }
                    x = 0;
                    textHeight += font.LineSize;
                }

                x += glyphWidth;
            }

            if (x > textWidth)
            {
                textWidth = x;
            }
            textHeight += font.LineSize;

            Surface surf = new Surface(textWidth, textHeight);
            surf.TransparentColor = Color.Black;
            surf.Transparent = true;

            /* the draw it */
            x = y = 0;
            for (i = 0; i < r.Length; i++)
            {
                int glyphWidth = 0;
                Glyph g = null;

                if (r[i] != 0x0a) /* newline */
                {
                    if (r[i] == 0x20) /* space */
                    {
                        glyphWidth = font.SpaceSize;
                    }
                    else
                    {
                        g = font[r[i] - 1];
                        glyphWidth = g.Width + g.XOffset;

                        Surface gs = RenderGlyph(g, palette, offset);
                        surf.Blit(gs, new Point(x, y + g.YOffset));
                    }
                }

                if (r[i] == 0x0a ||
                x + glyphWidth > textWidth)
                {
                    x = 0;
                    y += font.LineSize;
                }

                x += glyphWidth;
            }

            return surf;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="palette"></param>
        /// <param name="withAlpha"></param>
        /// <returns></returns>
        [CLSCompliant(false)]
        public static byte[] GetBitmapData(byte[,] grid, ushort width, ushort height, byte[] palette, bool withAlpha)
        {
            if (palette == null)
            {
                throw new ArgumentNullException("palette");
            }
            byte[] buf = new byte[width * height * (3 + (withAlpha ? 1 : 0))];
            int i = 0;
            int x;
            int y;

            for (y = height - 1; y >= 0; y--)
            {
                for (x = width - 1; x >= 0; x--)
                {
                    if (withAlpha)
                        i++;
                    buf[i++] = palette[grid[y, x] * 3 + 2];
                    buf[i++] = palette[grid[y, x] * 3 + 1];
                    buf[i++] = palette[grid[y, x] * 3];
                    if (withAlpha)
                    {
                        if (buf[i - 3] == 0
                        && buf[i - 2] == 0
                        && buf[i - 1] == 0)
                        {
                            buf[i - 4] = 0x00;
                        }
                        else
                        {
                            buf[i - 4] = 0xff;
                        }
                    }
                }
            }

            return buf;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="data"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="depth"></param>
        /// <param name="stride"></param>
        /// <param name="rmask"></param>
        /// <param name="gmask"></param>
        /// <param name="bmask"></param>
        /// <param name="amask"></param>
        /// <returns></returns>
        [CLSCompliant(false)]
        public static Surface CreateSurface(byte[] data, ushort width, ushort height, int depth, int stride,
        int rmask, int gmask, int bmask, int amask)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            /* beware, kind of a gross hack below */
            Surface surf;

            IntPtr blob = Marshal.AllocCoTaskMem(data.Length);
            Marshal.Copy(data, 0, blob, data.Length);

            IntPtr handle = Sdl.SDL_CreateRGBSurfaceFrom(blob,
            width, height, depth,
            stride,
            rmask, gmask, bmask, amask);

            surf = (Surface)Activator.CreateInstance(typeof(Surface),
            BindingFlags.NonPublic | BindingFlags.Instance,
            null,
            new object[] { handle },
            null);

            return surf;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="data"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="depth"></param>
        /// <param name="stride"></param>
        /// <returns></returns>
        [CLSCompliant(false)]
        public static Surface CreateSurfaceFromRgbaData(byte[] data, ushort width, ushort height, int depth, int stride)
        {
            return CreateSurface(data, width, height, depth, stride,
                /* XXX this needs addressing in Tao.Sdl - these arguments should be uints */
            unchecked((int)0xff000000),
            (int)0x00ff0000,
            (int)0x0000ff00,
            (int)0x000000ff);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="data"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="depth"></param>
        /// <param name="stride"></param>
        /// <returns></returns>
        [CLSCompliant(false)]
        public static Surface CreateSurfaceFromRgbData(byte[] data, ushort width, ushort height, int depth, int stride)
        {
            return CreateSurface(data, width, height, depth, stride,
            (int)0x00ff0000,
            (int)0x0000ff00,
            (int)0x000000ff,
            (int)0x00000000);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="palette"></param>
        /// <param name="translucentIndex"></param>
        /// <param name="transparentIndex"></param>
        /// <returns></returns>
        [CLSCompliant(false)]
        public static byte[] GetBitmapData(byte[,] grid, ushort width, ushort height, byte[] palette, int translucentIndex, int transparentIndex)
        {
            if (palette == null)
            {
                throw new ArgumentNullException("palette");
            }
            byte[] buf = new byte[width * height * 4];
            int i = 0;
            int x, y;

            for (y = height - 1; y >= 0; y--)
            {
                for (x = width - 1; x >= 0; x--)
                {
                    if (grid[y, x] == translucentIndex)
                    {
                        buf[i + 0] = 0x05; /* keep this in sync with Pcx.cs */
                    }
                    else if (grid[y, x] == transparentIndex)
                    {
                        buf[i + 0] = 0x00;
                    }
                    else
                    {
                        buf[i + 0] = 0xff;
                    }
                    buf[i + 1] = palette[grid[y, x] * 3 + 2];
                    buf[i + 2] = palette[grid[y, x] * 3 + 1];
                    buf[i + 3] = palette[grid[y, x] * 3 + 0];
                    i += 4;
                }
            }

            return buf;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="palette"></param>
        /// <param name="withAlpha"></param>
        /// <returns></returns>
        [CLSCompliant(false)]
        public static Surface CreateSurfaceFromBitmap(byte[,] grid, ushort width, ushort height, byte[] palette, bool withAlpha)
        {
            byte[] buf = GetBitmapData(grid, width, height, palette, withAlpha);

            return CreateSurfaceFromRgbaData(buf, width, height, withAlpha ? 32 : 24, width * (3 + (withAlpha ? 1 : 0)));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="palette"></param>
        /// <param name="translucentIndex"></param>
        /// <param name="transparentIndex"></param>
        /// <returns></returns>
        [CLSCompliant(false)]
        public static Surface CreateSurfaceFromBitmap(byte[,] grid, ushort width, ushort height, byte[] palette, int translucentIndex, int transparentIndex)
        {
            byte[] buf = GetBitmapData(grid, width, height, palette, translucentIndex, transparentIndex);

            return CreateSurfaceFromRgbaData(buf, width, height, 32, width * 4);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static byte[] ReadStream(Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }
            MemoryStream newStream = stream as MemoryStream;
            if (newStream != null)
            {
                return newStream.ToArray();
            }
            else
            {
                byte[] buf = new byte[stream.Length];
                stream.Read(buf, 0, buf.Length);
                return buf;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="translucentIndex"></param>
        /// <param name="transparentIndex"></param>
        /// <returns></returns>
        public static Surface SurfaceFromStream(Stream stream, int translucentIndex, int transparentIndex)
        {
            Pcx pcx = new Pcx();
            pcx.ReadFromStream(stream, translucentIndex, transparentIndex);
            return CreateSurfaceFromRgbaData(pcx.RgbaData, pcx.Width, pcx.Height, pcx.Depth, pcx.Stride);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static Surface SurfaceFromStream(Stream stream)
        {
            return GuiUtility.SurfaceFromStream(stream, -1, -1);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static Sound SoundFromStream(Stream stream)
        {
            byte[] buf = GuiUtility.ReadStream(stream);
            return Mixer.Sound(buf);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="mpq"></param>
        /// <param name="resourcePath"></param>
        public static void PlaySound(Mpq mpq, string resourcePath)
        {
            if (mpq == null)
            {
                throw new ArgumentNullException("mpq");
            }
            Stream stream = (Stream)mpq.GetResource(resourcePath);
            if (stream == null)
            {
                return;
            }
            Sound s = GuiUtility.SoundFromStream(stream);
            s.Play();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="mpq"></param>
        /// <param name="resourcePath"></param>
        /// <param name="numberOfLoops"></param>
        public static void PlayMusic(Mpq mpq, string resourcePath, int numberOfLoops)
        {
            if (mpq == null)
            {
                throw new ArgumentNullException("mpq");
            }
            Stream stream = (Stream)mpq.GetResource(resourcePath);
            if (stream == null)
            {
                return;
            }
            Sound s = GuiUtility.SoundFromStream(stream);
            s.Play(true);
        }
    }
}
