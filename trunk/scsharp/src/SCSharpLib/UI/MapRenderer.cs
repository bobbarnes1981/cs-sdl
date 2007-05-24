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
using System.IO;

using SdlDotNet.Graphics;
using SCSharp;
using SCSharp.MpqLib;


namespace SCSharp.UI
{
    /// <summary>
    ///
    /// </summary>
    public static class MapRenderer
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="mpq"></param>
        /// <param name="chk"></param>
        /// <returns></returns>
        public static Surface RenderToSurface(Mpq mpq, Chk chk)
        {
            ushort pixelWidth = 0;
            ushort pixelHeight = 0;

            //byte[] bitmap = RenderToBitmap(mpq, chk, out pixelWidth, out pixelHeight);
            BitmapImage bitmap = RenderToBitmap(mpq, chk, pixelWidth, pixelHeight);

            return GuiUtility.CreateSurfaceFromRgbaData(bitmap.Image, bitmap.PixelWidth, bitmap.PixelHeight, 32, bitmap.PixelWidth * 4);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="mpq"></param>
        /// <param name="chk"></param>
        /// <param name="pixelWidth"></param>
        /// <param name="pixelHeight"></param>
        /// <returns></returns>
        [CLSCompliant(false)]
        public static BitmapImage RenderToBitmap(Mpq mpq, Chk chk, ushort pixelWidth, ushort pixelHeight)
        {
            BitmapImage bitmapImage = new BitmapImage();
            //public static byte[] RenderToBitmap(Mpq mpq, Chk chk, out ushort pixelWidth, out ushort pixelHeight)
            if (chk == null)
            {
                throw new ArgumentNullException("chk");
            }
            if (mpq == null)
            {
                throw new ArgumentNullException("mpq");
            }
            ushort[,] mapTiles = chk.MapTiles;

            bitmapImage.Image = new byte[chk.Width * 32 * chk.Height * 32 * 4];

            bitmapImage.PixelWidth = (ushort)(chk.Width * 32);
            bitmapImage.PixelHeight = (ushort)(chk.Height * 32);

            Stream cv5_fs = (Stream)mpq.GetResource(String.Format("tileset\\{0}.cv5", Utilities.TileSetNames[(int)chk.TileSet]));
            Stream vx4_fs = (Stream)mpq.GetResource(String.Format("tileset\\{0}.vx4", Utilities.TileSetNames[(int)chk.TileSet]));
            Stream vr4_fs = (Stream)mpq.GetResource(String.Format("tileset\\{0}.vr4", Utilities.TileSetNames[(int)chk.TileSet]));
            Stream wpe_fs = (Stream)mpq.GetResource(String.Format("tileset\\{0}.wpe", Utilities.TileSetNames[(int)chk.TileSet]));

            byte[] cv5 = new byte[cv5_fs.Length];
            cv5_fs.Read(cv5, 0, (int)cv5_fs.Length);

            byte[] vx4 = new byte[vx4_fs.Length];
            vx4_fs.Read(vx4, 0, (int)vx4_fs.Length);

            byte[] vr4 = new byte[vr4_fs.Length];
            vr4_fs.Read(vr4, 0, (int)vr4_fs.Length);

            byte[] wpe = new byte[wpe_fs.Length];
            wpe_fs.Read(wpe, 0, (int)wpe_fs.Length);

            for (int map_y = 0; map_y < chk.Height; map_y++)
            {
                for (int map_x = 0; map_x < chk.Width; map_x++)
                {
                    int mapTile = mapTiles[map_x, map_y];

                    // bool odd = (mapTile & 0x10) == 0x10;

                    int tile_group = mapTile >> 4; /* the tile's group in the cv5 file */
                    int tile_number = mapTile & 0x0F; /* the megatile within the tile group */

                    int megatile_id = Utilities.ReadWord(cv5, (tile_group * 26 + 10 + tile_number) * 2);

                    int minitile_x, minitile_y;

                    for (minitile_y = 0; minitile_y < 4; minitile_y++)
                    {
                        for (minitile_x = 0; minitile_x < 4; minitile_x++)
                        {
                            ushort minitile_id = Utilities.ReadWord(vx4, megatile_id * 32 + minitile_y * 8 + minitile_x * 2);
                            bool flipped = (minitile_id & 0x01) == 0x01;
                            minitile_id >>= 1;

                            int pixel_x, pixel_y;
                            if (flipped)
                            {
                                for (pixel_y = 0; pixel_y < 8; pixel_y++)
                                    for (pixel_x = 0; pixel_x < 8; pixel_x++)
                                    {
                                        int x = map_x * 32 + (minitile_x + 1) * 8 - pixel_x - 1;
                                        int y = (map_y * 32 + minitile_y * 8) * chk.Width * 32 + pixel_y * chk.Width * 32;

                                        byte palette_entry = vr4[minitile_id * 64 + pixel_y * 8 + pixel_x];

                                        bitmapImage.Image[0 + 4 * (x + y)] = (byte)(255 - wpe[palette_entry * 4 + 3]);
                                        bitmapImage.Image[1 + 4 * (x + y)] = wpe[palette_entry * 4 + 2];
                                        bitmapImage.Image[2 + 4 * (x + y)] = wpe[palette_entry * 4 + 1];
                                        bitmapImage.Image[3 + 4 * (x + y)] = wpe[palette_entry * 4 + 0];
                                    }
                            }
                            else
                            {
                                for (pixel_y = 0; pixel_y < 8; pixel_y++)
                                {
                                    for (pixel_x = 0; pixel_x < 8; pixel_x++)
                                    {
                                        int x = map_x * 32 + minitile_x * 8 + pixel_x;
                                        int y = (map_y * 32 + minitile_y * 8) * chk.Width * 32 + pixel_y * chk.Width * 32;

                                        byte palette_entry = vr4[minitile_id * 64 + pixel_y * 8 + pixel_x];

                                        bitmapImage.Image[0 + 4 * (x + y)] = (byte)(255 - wpe[palette_entry * 4 + 3]);
                                        bitmapImage.Image[1 + 4 * (x + y)] = wpe[palette_entry * 4 + 2];
                                        bitmapImage.Image[2 + 4 * (x + y)] = wpe[palette_entry * 4 + 1];
                                        bitmapImage.Image[3 + 4 * (x + y)] = wpe[palette_entry * 4 + 0];
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return bitmapImage;
        }
    }
}
