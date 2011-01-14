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

    public class TextRenderer
    {
        #region Constructors

        // This class and its methods are still experimental. I'm still working on figuring out
        // how to render text as quickly as possible with the features that I want ~Pikablu
        public TextRenderer() {
        }

        #endregion Constructors

        #region Methods

        public static Size RenderText(Surface destSurf, SdlDotNet.Graphics.Font font, string textItem, Color textColor, bool antiAlias, int textWidth, int maxLines) {
            return RenderText(destSurf, font, textItem, textColor, antiAlias, textWidth, maxLines, 0, 0);
        }

        public static Size RenderText(Surface destSurf, SdlDotNet.Graphics.Font font, string textItem, Color textColor, bool antiAlias, int textWidth, int maxLines, int startX, int startY) {
            int x = startX;
            int y = startY;
            int tempWidth = 0;
            int stringlength;
            int stringpos;
            int countback;
            int newX;
            string templine;
            int countLines;
            string[] splitline;

            splitline = textItem.Replace("\r", string.Empty).Split('\n');
            for (int k = 0; k <= splitline.GetUpperBound(0); k++) {
                splitline[k] = splitline[k].Trim();
            }

            if (textWidth == 0 && splitline.Length == 1) {
                Surface textSurface = font.Render(textItem, textColor, antiAlias);
                destSurf.Blit(textSurface, new Point(startX, startY));
                textSurface.Close();
                return font.SizeText(textItem);
            } else {
                // it is faster to set aside a large amount of temporary surface
                // than recreate a new one every time the surface is to expand.

                // if no textWidth is given, then set the textWidth to ludicrusly high
                // in all cases, set the height to an impossible high number.

                // a possible improvement would be to pass in the the temporary
                // textWidth & height for performance
                // the height of fonts are always the same from what I can tell
                // no matter which letter is used. It's the widths that cause trouble
                int fontHeight = font.SizeText(" ").Height;

                countLines = 1;

                int maxWidth = 0;
                for (int k = 0; k <= splitline.GetUpperBound(0); k++) {
                    // no word wrap, only newline wrap
                    if (textWidth == 0) {
                        if (maxLines == 0 || countLines <= maxLines) {
                            if (!String.IsNullOrEmpty(splitline[k])) {
                                Surface textSurface = font.Render(splitline[k], textColor, antiAlias);
                                destSurf.Blit(textSurface, new Point(x, y));
                                textSurface.Close();
                            }
                            y = y + fontHeight;
                            countLines = countLines + 1;
                        }
                        textWidth = tempWidth;
                        if (font.SizeText(splitline[k]).Width > maxWidth) {
                            maxWidth = font.SizeText(splitline[k]).Width;
                        }
                    }
                        // word wrapping & new line wrapping
                    else {
                        stringpos = 0;
                        //indented = true;
                        if (String.IsNullOrEmpty(splitline[k])) {
                            y = y + fontHeight;
                        }
                        while (stringpos < splitline[k].Length) {
                            if (maxLines == 0 || countLines <= maxLines) {
                                stringlength = 1;
                                templine = splitline[k].Substring(stringpos, stringlength);

                                // this is the secret: keep checking the width of the string to blit, adding
                                // one letter at a time, until the width has been hit
                                while (font.SizeText(templine).Width <= textWidth & (stringpos + stringlength < splitline[k].Length)) {
                                    stringlength = stringlength + 1;
                                    templine = splitline[k].Substring(stringpos, stringlength);
                                }
                                countback = 0;
                                // now count backwards, until a space is found
                                while (!(templine.EndsWith(" ")) & (stringpos + stringlength < splitline[k].Length)) {
                                    countback = countback + 1;
                                    if (countback >= splitline[k].Length) {
                                        countback = 0;
                                        templine = splitline[k].Substring(stringpos, stringlength);
                                        break;
                                    }
                                    templine = stringlength - countback > 0 ? templine.Substring(0, stringlength - countback) : "";
                                }

                                // move the current string position forward
                                stringpos = stringpos + stringlength - countback;
                                //if (indented)
                                //{
                                //	newX = x + indent;
                                //}
                                //else
                                //{
                                newX = x;
                                //}
                                // render the wrapped line
                                Surface textSurface = font.Render(templine, textColor, antiAlias);
                                destSurf.Blit(textSurface, new Point(newX, y));
                                textSurface.Close();
                                if (font.SizeText(templine).Width > maxWidth) {
                                    maxWidth = font.SizeText(templine).Width;
                                }
                                y = y + fontHeight;
                                countLines = countLines + 1;
                            } else {
                                stringpos = stringpos + 1;
                            }
                            //indented = false;
                        }
                    }
                }
                return new Size(maxWidth, y);
            }
        }

        public static SdlDotNet.Graphics.Surface RenderTextBasic(SdlDotNet.Graphics.Font font, string textItem, CharRenderOptions[] charRenderOptions, Color textColor, bool antiAlias, int maxWidth, int maxHeight, int startX, int startY) {
            //int maxWidth = 0;
            int width = 0;
            int height = font.Height;
            int longestWidth = 0;
            if (startX >= textItem.Length) {
                startX = textItem.Length;
            }
            for (int i = 0; i < textItem.Length; i++) {
                if (textItem[i] != '\n') {

                    bool moved = false;
                    if (maxWidth > 0) {
                        int z = i;
                        int wordWidth = width;
                        while (true) {
                            if (z >= textItem.Length || textItem[z] == ' ' || textItem[z] == '\n') {
                                if (wordWidth > maxWidth) {
                                    height += font.Height;
                                    width = 0;

                                    i = z;

                                    moved = true;
                                }
                                break;
                            }
                            wordWidth += font.SizeText(textItem[z].ToString()).Width;

                            z++;
                        }
                    }

                    if (!moved) {
                        width += font.SizeText(textItem[i].ToString()).Width;

                        if (width > longestWidth) {
                            longestWidth = width;
                        }
                        if (maxWidth > 0 && width > maxWidth) {
                            height += font.Height;
                            width = 0;
                        }
                    }
                } else {
                    height += font.Height;
                    if (width > longestWidth) {
                        longestWidth = width;
                    }
                    width = 0;
                }
            }

            SdlDotNet.Graphics.Surface surf2;
            int surfaceWidth;
            int surfaceHeight;
            if (maxWidth > 0 && longestWidth > maxWidth) {
                surfaceWidth = maxWidth;
            } else {
                surfaceWidth = longestWidth;
            }
            if (maxHeight > 0 && height > maxHeight) {
                surfaceHeight = maxHeight;
            } else {
                surfaceHeight = height;
            }
            surf2 = new SdlDotNet.Graphics.Surface(surfaceWidth, surfaceHeight);
            surf2.Fill(Color.Transparent);
            surf2.TransparentColor = Color.Transparent;
            surf2.Transparent = true;
            int lastX = 0;
            int lastY = 0;

            bool skippingChars = false;
            int charsSkipped = 0;
            bool skippingLines = false;
            int linesSkipped = 0;
            if (startX > 0) {
                skippingChars = true;
            }
            if (startY > 0) {
                skippingLines = true;
            }
            int val = 0;
            if (skippingLines || skippingChars) {
                for (int i = 0; i < textItem.Length; i++) {
                    // We need to skip lines
                    if (skippingLines) {
                        if (textItem[i] != '\n') {
                            // Check if we need to move full words to another line
                            if (maxWidth > 0) {
                                int z = i;
                                int wordWidth = lastX;
                                while (true) {
                                    if (z >= textItem.Length || textItem[z] == ' ' || textItem[z] == '\n') {
                                        if (wordWidth > maxWidth) {
                                            linesSkipped++;
                                            lastX = 0;
                                            i = z;
                                        }
                                        break;
                                    }
                                    wordWidth += font.SizeText(textItem[z].ToString()).Width;

                                    z++;
                                }
                            }

                            lastX += font.SizeText(textItem[i].ToString()).Width;
                        }
                        if (linesSkipped >= startY) {
                            linesSkipped = 0;
                            skippingLines = false;
                            val = i;
                        }
                    }
                }
            }
            for (int i = val; i < textItem.Length; i++) {
                if (textItem[i] != '\n') {
                    int letterWidth = font.SizeText(textItem[i].ToString()).Width;

                    //if (skippingLines) {

                    //    bool moved = false;
                    //    if (maxWidth > 0) {
                    //        int z = i;
                    //        int wordWidth = lastX;
                    //        while (true) {
                    //            if (z >= textItem.Length || textItem[z] == ' ' || textItem[z] == '\n') {
                    //                if (wordWidth > maxWidth) {
                    //                    linesSkipped++;
                    //                    //if (linesSkipped >= startY) {
                    //                    //    skippingLines = false;
                    //                    //    linesSkipped = 0;
                    //                    //}
                    //                    i = z;
                    //                    lastX = wordWidth - lastX;
                    //                    moved = true;
                    //                }
                    //                break;
                    //            }
                    //            wordWidth += font.SizeText(textItem[z].ToString()).Width;

                    //            z++;
                    //        }
                    //    }

                    //    if (!moved) {
                    //        if (lastX + letterWidth > surfaceWidth) {
                    //            lastX = lastX + letterWidth - surfaceWidth;
                    //            if (skippingLines) {
                    //                linesSkipped++;
                    //                if (linesSkipped >= startY) {
                    //                    skippingLines = false;
                    //                    linesSkipped = 0;
                    //                }
                    //            }
                    //        } else {
                    //            lastX += letterWidth;
                    //        }
                    //    }
                    //    if (skippingLines) {
                    //        if (linesSkipped >= startY) {
                    //            skippingLines = false;
                    //            linesSkipped = 0;
                    //            lastX = 0;
                    //        }
                    //    }
                    //    //if (lastX + letterWidth > surfaceWidth) {
                    //    //    lastY += font.Height;
                    //    //    lastX = 0;
                    //    //    if (skippingLines) {
                    //    //        linesSkipped++;
                    //    //        if (linesSkipped >= startY) {
                    //    //            skippingLines = false;
                    //    //            linesSkipped = 0;
                    //    //        }
                    //    //    }
                    //    //} else {
                    //    //    if (maxWidth > 0) {
                    //    //        int z = i;
                    //    //        int wordWidth = lastX;
                    //    //        while (true) {
                    //    //            if (z >= textItem.Length || textItem[z] == ' ' || textItem[z] == '\n') {
                    //    //                if (wordWidth > maxWidth) {
                    //    //                    linesSkipped++;
                    //    //                    if (linesSkipped >= startY) {
                    //    //                        skippingLines = false;
                    //    //                        linesSkipped = 0;
                    //    //                    }
                    //    //                    i = z;
                    //    //                    lastX = 0;
                    //    //                }
                    //    //                break;
                    //    //            }
                    //    //            wordWidth += font.SizeText(textItem[z].ToString()).Width;

                    //    //            z++;
                    //    //        }
                    //    //    }
                    //    //    lastX += letterWidth;
                    //    //}
                    //} else if (!skippingChars) {
                    //    if (lastX + letterWidth > surfaceWidth) {
                    //        lastY += font.Height;
                    //        lastX = 0;
                    //        if (startX > 0) {
                    //            skippingChars = true;
                    //            charsSkipped = 0;
                    //        }
                    //        if (skippingLines) {
                    //            linesSkipped++;
                    //            if (linesSkipped >= startY) {
                    //                skippingLines = false;
                    //                linesSkipped = 0;
                    //            }
                    //        }
                    //    }
                    //} else {
                    //    charsSkipped++;
                    //    if (charsSkipped >= startX) {
                    //        skippingChars = false;
                    //        charsSkipped = 0;
                    //    }
                    //}
                    if (!skippingChars && !skippingLines) {
                        if (maxHeight > 0) {
                            if (lastY + font.Height <= maxHeight) {
                                Color foreColor = textColor;
                                Color backColor = Color.Empty;
                                if (charRenderOptions != null && charRenderOptions.Length > i) {
                                    if (charRenderOptions[i].ForeColor != Color.Empty) {
                                        foreColor = charRenderOptions[i].ForeColor;
                                    }
                                    font.Bold = charRenderOptions[i].Bold;
                                    font.Italic = charRenderOptions[i].Italic;
                                    font.Underline = charRenderOptions[i].Underline;
                                    backColor = charRenderOptions[i].BackColor;
                                }
                                SdlDotNet.Graphics.Surface fontSurf = font.Render(textItem[i].ToString(), foreColor, backColor, antiAlias);
                                if (font.Bold == true) {
                                    font.Bold = false;
                                }
                                if (font.Italic == true) {
                                    font.Italic = false;
                                }
                                if (font.Underline == true) {
                                    font.Underline = false;
                                }
                                if (maxWidth > 0) {
                                    int z = i;
                                    int wordWidth = lastX;
                                    while (true) {
                                        if (z >= textItem.Length || textItem[z] == ' ' || textItem[z] == '\n') {
                                            if (wordWidth > maxWidth) {
                                                lastY += font.Height;
                                                lastX = 0;
                                            }
                                            break;
                                        }
                                        wordWidth += font.SizeText(textItem[z].ToString()).Width;

                                        z++;
                                    }
                                }
                                surf2.Blit(fontSurf, new Point(lastX, lastY));
                                lastX += letterWidth;
                                fontSurf.Dispose();
                            }
                        } else {
                            SdlDotNet.Graphics.Surface fontSurf = font.Render(textItem[i].ToString(), textColor, antiAlias);
                            if (maxWidth > 0) {
                                int z = i;
                                int wordWidth = lastX;
                                while (true) {
                                    if (z >= textItem.Length || textItem[z] == ' ' || textItem[z] == '\n') {
                                        if (wordWidth > maxWidth) {
                                            lastY += font.Height;
                                            lastX = 0;
                                        }
                                        break;
                                    }
                                    wordWidth += font.SizeText(textItem[z].ToString()).Width;

                                    z++;
                                }
                            }
                            surf2.Blit(fontSurf, new Point(lastX, lastY));
                            lastX += letterWidth;
                            fontSurf.Dispose();
                        }
                    }
                } else {
                    lastX = 0;
                    if (startX > 0) {
                        skippingChars = true;
                        charsSkipped = 0;
                    }
                    if (skippingLines) {
                        linesSkipped++;
                        if (linesSkipped >= startY) {
                            skippingLines = false;
                            linesSkipped = 0;
                        }
                    } else {
                        lastY += font.Height;
                    }
                }
            }
            return surf2;
        }

        public static Size RenderSizeData(SdlDotNet.Graphics.Font font, string textItem, CharRenderOptions[] charRenderOptions, Color textColor, bool antiAlias, int maxWidth, int maxHeight, int startX, int startY) {
            int width = 0;
            int height = font.Height;
            int longestWidth = 0;
            int linesSkipped = 0;
            bool skippingLines = startY > 0;
            if (startX >= textItem.Length) {
                startX = textItem.Length;
            }

            if (maxWidth > 0) {
                //longestWidth = maxWidth;
            }
            if (maxHeight > 0) {
                height = maxHeight;
            }

            GlyphData[] glyphDatas = new GlyphData[textItem.Length];
            for (int i = 0; i < textItem.Length; i++) {
                glyphDatas[i] = font.GetGlyphMetrics(textItem[i]);
                //glyphDatas[i] = new Size(glyphData.Advance, glyphData.Height);
                //sizes[i] = font.SizeText(textItem[i].ToString());
            }

            if (true) {//maxWidth == 0 || maxHeight == 0) {
                for (int i = 0; i < textItem.Length; i++) {
                    if (textItem[i] != '\n') {
                        if (maxWidth == 0) {
                            width += glyphDatas[i].Advance;
                            if (width > longestWidth) {
                                longestWidth = width;
                            }
                        } else {
                            int z = i;
                            int wordWidth = width;
                            while (true) {
                                if (z >= textItem.Length || textItem[z] == ' ' || textItem[z] == '\n') {
                                    if (wordWidth > maxWidth) {
                                        //if (skippingLines) {
                                        //    linesSkipped++;
                                        //    width = wordWidth - width;
                                        //} else {
                                        if (!skippingLines) {
                                            height += font.Height;
                                        } else {
                                            linesSkipped++;
                                            if (skippingLines && linesSkipped >= startY) {
                                                skippingLines = false;
                                                linesSkipped = 0;
                                            }
                                        }
                                        width = 0;
                                        for (int n = i; n < z; n++) {
                                            width += glyphDatas[n].Advance;
                                            if (width > maxWidth) {
                                                if (!skippingLines) {
                                                    height += font.Height;
                                                } else {
                                                    linesSkipped++;
                                                    if (skippingLines && linesSkipped >= startY) {
                                                        skippingLines = false;
                                                        linesSkipped = 0;
                                                    }
                                                }
                                                if (width > longestWidth) {
                                                    longestWidth = width;
                                                }
                                                width = 0;
                                            }
                                        }
                                        //}
                                    } else {
                                        width = wordWidth;
                                        if (width > longestWidth) {
                                            longestWidth = width;
                                        }
                                    }
                                    break;
                                }
                                wordWidth += glyphDatas[z].Advance;

                                z++;
                            }
                            i = z;
                            if (z < textItem.Length && textItem[z] != '\n') {
                                width += glyphDatas[z].Advance;
                            } else if (z < textItem.Length && textItem[z] == '\n') {
                                if (width > longestWidth) {
                                    longestWidth = width;
                                }
                                width = 0;
                                if (skippingLines) {
                                    linesSkipped++;
                                    if (skippingLines && linesSkipped >= startY) {
                                        skippingLines = false;
                                        linesSkipped = 0;
                                    }
                                } else {
                                    height += font.Height;
                                }
                            }
                        }
                        //if (i < textItem.Length) {
                        //    width += font.SizeText(textItem[i].ToString()).Width;
                        //}
                    } else {
                        height += font.Height;
                        if (width > longestWidth) {
                            longestWidth = width;
                        }
                        width = 0;
                    }
                }
            }
            if (width > longestWidth) {
                longestWidth = width;
            }
            if (maxWidth == 0) {
                if (longestWidth == 0) {
                    longestWidth = width;
                }
            }

            return new Size(longestWidth, height);
        }

        public static SdlDotNet.Graphics.Surface RenderTextBasic2(SdlDotNet.Graphics.Font font, string textItem, CharRenderOptions[] charRenderOptions, Color textColor, bool antiAlias, int maxWidth, int maxHeight, int startX, int startY) {
            int width = 0;
            int height = font.Height;
            int longestWidth = 0;
            int linesSkipped = 0;
            bool skippingLines = startY > 0;
            if (startX >= textItem.Length) {
                startX = textItem.Length;
            }

            if (maxWidth > 0) {
                //longestWidth = maxWidth;
            }
            if (maxHeight > 0) {
                height = maxHeight;
            }

            GlyphData[] glyphDatas = new GlyphData[textItem.Length];
            for (int i = 0; i < textItem.Length; i++) {
                glyphDatas[i] = font.GetGlyphMetrics(textItem[i]);
                //glyphDatas[i] = new Size(glyphData.Advance, glyphData.Height);
                //sizes[i] = font.SizeText(textItem[i].ToString());
            }

            if (true) {//maxWidth == 0 || maxHeight == 0) {
                for (int i = 0; i < textItem.Length; i++) {
                    if (textItem[i] != '\n') {
                        if (maxWidth == 0) {
                            width += glyphDatas[i].Advance;
                            if (width > longestWidth) {
                                longestWidth = width;
                            }
                        } else {
                            int z = i;
                            int wordWidth = width;
                            while (true) {
                                if (z >= textItem.Length || textItem[z] == ' ' || textItem[z] == '\n') {
                                    if (wordWidth > maxWidth) {
                                        //if (skippingLines) {
                                        //    linesSkipped++;
                                        //    width = wordWidth - width;
                                        //} else {
                                        if (!skippingLines) {
                                            height += font.Height;
                                        } else {
                                            linesSkipped++;
                                            if (skippingLines && linesSkipped >= startY) {
                                                skippingLines = false;
                                                linesSkipped = 0;
                                            }
                                        }
                                        width = 0;
                                        for (int n = i; n < z; n++) {
                                            width += glyphDatas[n].Advance;
                                            if (width > maxWidth) {
                                                if (!skippingLines) {
                                                    height += font.Height;
                                                } else {
                                                    linesSkipped++;
                                                    if (skippingLines && linesSkipped >= startY) {
                                                        skippingLines = false;
                                                        linesSkipped = 0;
                                                    }
                                                }
                                                if (width > longestWidth) {
                                                    longestWidth = width;
                                                }
                                                width = 0;
                                            }
                                        }
                                        //}
                                    } else {
                                        width = wordWidth;
                                        if (width > longestWidth) {
                                            longestWidth = width;
                                        }
                                    }
                                    break;
                                }
                                wordWidth += glyphDatas[z].Advance;

                                z++;
                            }
                            i = z;
                            if (z < textItem.Length && textItem[z] != '\n') {
                                width += glyphDatas[z].Advance;
                            } else if (z < textItem.Length && textItem[z] == '\n') {
                                if (width > longestWidth) {
                                    longestWidth = width;
                                }
                                width = 0;
                                if (skippingLines) {
                                    linesSkipped++;
                                    if (skippingLines && linesSkipped >= startY) {
                                        skippingLines = false;
                                        linesSkipped = 0;
                                    }
                                } else {
                                    height += font.Height;
                                }
                            }
                        }
                        //if (i < textItem.Length) {
                        //    width += font.SizeText(textItem[i].ToString()).Width;
                        //}
                    } else {
                        height += font.Height;
                        if (width > longestWidth) {
                            longestWidth = width;
                        }
                        width = 0;
                    }
                }
            }
            if (width > longestWidth) {
                longestWidth = width;
            }
            if (maxWidth == 0) {
                if (longestWidth == 0) {
                    longestWidth = width;
                }
            }

            Surface surf = new Surface(longestWidth, height);
            surf.Fill(Color.Transparent);
            surf.TransparentColor = Color.Transparent;
            surf.Transparent = true;

            int lastX = 0;
            int lastY = 0;

            linesSkipped = 0;
            skippingLines = startY > 0;
            for (int i = 0; i < textItem.Length; i++) {
                if (textItem[i] != '\n') {
                    if (maxWidth > 0) {
                        int z = i;
                        int wordWidth = lastX;
                        while (true) {
                            if (z >= textItem.Length || textItem[z] == ' ' || textItem[z] == '\n') {
                                if (wordWidth > maxWidth) {
                                    if (!skippingLines) {
                                        lastY += font.Height;
                                    } else {
                                        linesSkipped++;
                                        if (skippingLines && linesSkipped >= startY) {
                                            skippingLines = false;
                                            linesSkipped = 0;
                                        }
                                    }
                                    lastX = 0;
                                    CharRenderOptions renderOptions = null;
                                    for (int n = i; n < z; n++) {
                                        if (charRenderOptions != null && charRenderOptions.Length > n) {
                                            renderOptions = charRenderOptions[n];
                                        }
                                        if (!skippingLines) {
                                            RenderLetter(surf, font, textItem[n], glyphDatas[n], lastX, lastY, textColor, antiAlias, renderOptions);
                                        }
                                        lastX += glyphDatas[n].Advance;
                                        if (lastX > maxWidth) {
                                            if (!skippingLines) {
                                                lastY += font.Height;
                                            } else {
                                                linesSkipped++;
                                                if (skippingLines && linesSkipped >= startY) {
                                                    skippingLines = false;
                                                    linesSkipped = 0;
                                                }
                                            }
                                            lastX = 0;
                                        }
                                    }
                                    i = z;
                                    #region New Stuff
                                    if (z < textItem.Length && textItem[z] != '\n') {
                                        renderOptions = null;
                                        if (charRenderOptions != null && charRenderOptions.Length > z) {
                                            renderOptions = charRenderOptions[z];
                                        }
                                        if (!skippingLines) {
                                            RenderLetter(surf, font, textItem[z], glyphDatas[z], lastX, lastY, textColor, antiAlias, renderOptions);
                                        }
                                        lastX += glyphDatas[z].Advance;
                                        if (lastX > maxWidth) {
                                            if (!skippingLines) {
                                                lastY += font.Height;
                                            } else {
                                                linesSkipped++;
                                                if (skippingLines && linesSkipped >= startY) {
                                                    skippingLines = false;
                                                    linesSkipped = 0;
                                                }
                                            }
                                            lastX = 0;
                                        }
                                    } else if (z < textItem.Length && textItem[z] == '\n') {
                                        lastX = 0;
                                        if (skippingLines) {
                                            linesSkipped++;
                                            if (skippingLines && linesSkipped >= startY) {
                                                skippingLines = false;
                                                linesSkipped = 0;
                                            }
                                        } else {
                                            lastY += font.Height;
                                        }
                                    }
                                    if (linesSkipped >= startY && skippingLines) {
                                        skippingLines = false;
                                        //if (linesSkipped > startY) {
                                        //    lastY += (linesSkipped * font.Height);
                                        //}
                                        linesSkipped = 0;
                                        if (z < textItem.Length && !(textItem[z] == '\n' || textItem[z] == ' ')) {
                                            //lastY += font.Height;
                                            i--;
                                        }
                                    }
                                    #endregion
                                    //if (z < textItem.Length && textItem[z] != '\n') {
                                    //    renderOptions = null;
                                    //    if (charRenderOptions != null && charRenderOptions.Length > z) {
                                    //        renderOptions = charRenderOptions[z];
                                    //    }
                                    //    RenderLetter(surf, font, textItem[z], lastX, lastY, textColor, antiAlias, renderOptions);
                                    //    lastX += font.SizeText(textItem[z].ToString()).Width;
                                    //} else if (z < textItem.Length && textItem[z] == '\n') {
                                    //    lastX = 0;
                                    //    lastY += font.Height;
                                    //}
                                } else {
                                    CharRenderOptions renderOptions = null;
                                    for (int n = i; n < z; n++) {
                                        if (charRenderOptions != null && charRenderOptions.Length > n) {
                                            renderOptions = charRenderOptions[n];
                                        }
                                        if (!skippingLines) {
                                            RenderLetter(surf, font, textItem[n], glyphDatas[n], lastX, lastY, textColor, antiAlias, renderOptions);
                                        }
                                        lastX += glyphDatas[n].Advance;
                                        if (lastX > maxWidth) {
                                            if (!skippingLines) {
                                                lastY += font.Height;
                                            } else {
                                                linesSkipped++;
                                                if (linesSkipped >= startY && skippingLines) {
                                                    skippingLines = false;
                                                    linesSkipped = 0;
                                                }
                                            }
                                            lastX = 0;
                                        }
                                    }
                                    i = z;
                                    //if (i < textItem.Length) {
                                    //    if (textItem[i] == '\n') {
                                    //        i--;
                                    //        //if (skippingLines) {
                                    //        //    linesSkipped++;
                                    //        //    if (linesSkipped >= startY && skippingLines) {
                                    //        //        skippingLines = false;
                                    //        //        linesSkipped = 0;
                                    //        //    }
                                    //        //} else {
                                    //        //    lastY += font.Height;
                                    //        //    lastX = 0;
                                    //        //}
                                    //    } else if (textItem[i] == ' ') {
                                    //        int textWidth = font.SizeText(textItem[i].ToString()).Width;
                                    //        if (lastX + textWidth <= maxWidth) {
                                    //            if (!skippingLines) {
                                    //                RenderLetter(surf, font, textItem[i], lastX, lastY, textColor, antiAlias, renderOptions);
                                    //            }
                                    //            lastX += textWidth;
                                    //        } else {
                                    //            if (skippingLines) {
                                    //                linesSkipped++;
                                    //                if (linesSkipped >= startY && skippingLines) {
                                    //                    skippingLines = false;
                                    //                    linesSkipped = 0;
                                    //                }
                                    //            } else {
                                    //                lastY += font.Height;
                                    //                lastX = 0;
                                    //            }
                                    //        }
                                    //    }
                                    //}
                                    if (z < textItem.Length && textItem[z] != '\n') {
                                        renderOptions = null;
                                        if (charRenderOptions != null && charRenderOptions.Length > z) {
                                            renderOptions = charRenderOptions[z];
                                        }
                                        if (!skippingLines) {
                                            RenderLetter(surf, font, textItem[z], glyphDatas[z], lastX, lastY, textColor, antiAlias, renderOptions);
                                        }
                                        lastX += glyphDatas[z].Advance;
                                    } else if (z < textItem.Length && textItem[z] == '\n') {
                                        lastX = 0;
                                        if (skippingLines) {
                                            linesSkipped++;
                                            if (skippingLines && linesSkipped >= startY) {
                                                skippingLines = false;
                                                linesSkipped = 0;
                                            }
                                        } else {
                                            lastY += font.Height;
                                        }
                                    }
                                    if (linesSkipped >= startY && skippingLines) {
                                        skippingLines = false;
                                        //if (linesSkipped > startY) {
                                        //    lastY += (linesSkipped * font.Height);
                                        //}
                                        linesSkipped = 0;
                                        if (z < textItem.Length && !(textItem[z] == '\n' || textItem[z] == ' ')) {
                                            //lastY += font.Height;
                                            i--;
                                        }
                                    }
                                }
                                break;
                            }
                            wordWidth += glyphDatas[z].Advance;

                            z++;
                        }
                        //lastX += font.SizeText(textItem[i].ToString()).Width;

                    } else {
                        CharRenderOptions renderOptions = null;
                        if (charRenderOptions != null && charRenderOptions.Length > i) {
                            renderOptions = charRenderOptions[i];
                        }
                        RenderLetter(surf, font, textItem[i], glyphDatas[i], lastX, lastY, textColor, antiAlias, renderOptions);
                        lastX += glyphDatas[i].Advance;
                    }

                } else {
                    lastX = 0;
                    if (skippingLines) {
                        linesSkipped++;
                        if (linesSkipped >= startY && skippingLines) {
                            skippingLines = false;
                            linesSkipped = 0;
                        }
                    } else {
                        lastY += font.Height;
                    }
                }
            }
            return surf;
        }

        public static SdlDotNet.Graphics.Surface RenderTextBasic2ByGlyph(SdlDotNet.Graphics.Font font, string textItem, CharRenderOptions[] charRenderOptions, Color textColor, bool antiAlias, int maxWidth, int maxHeight, int startX, int startY) {
            int width = 0;
            int height = font.Height;
            int longestWidth = 0;
            int linesSkipped = 0;
            bool skippingLines = startY > 0;
            if (startX >= textItem.Length) {
                startX = textItem.Length;
            }

            if (maxWidth > 0) {
                //longestWidth = maxWidth;
            }
            if (maxHeight > 0) {
                height = maxHeight;
            }

            Size[] sizes = new Size[textItem.Length];
            for (int i = 0; i < textItem.Length; i++) {
                GlyphData glyphData = font.GetGlyphMetrics(textItem[i]);
                sizes[i] = new Size(glyphData.Width, glyphData.Height);
                //sizes[i] = font.SizeText(textItem[i].ToString());
            }

            if (true) {//maxWidth == 0 || maxHeight == 0) {
                for (int i = 0; i < textItem.Length; i++) {
                    if (textItem[i] != '\n') {
                        if (maxWidth == 0) {
                            width += sizes[i].Width;
                            if (width > longestWidth) {
                                longestWidth = width;
                            }
                        } else {
                            int z = i;
                            int wordWidth = width;
                            while (true) {
                                if (z >= textItem.Length || textItem[z] == ' ' || textItem[z] == '\n') {
                                    if (wordWidth > maxWidth) {
                                        //if (skippingLines) {
                                        //    linesSkipped++;
                                        //    width = wordWidth - width;
                                        //} else {
                                        if (!skippingLines) {
                                            height += font.Height;
                                        } else {
                                            linesSkipped++;
                                            if (skippingLines && linesSkipped >= startY) {
                                                skippingLines = false;
                                                linesSkipped = 0;
                                            }
                                        }
                                        width = 0;
                                        for (int n = i; n < z; n++) {
                                            width += sizes[n].Width;
                                            if (width > maxWidth) {
                                                if (!skippingLines) {
                                                    height += font.Height;
                                                } else {
                                                    linesSkipped++;
                                                    if (skippingLines && linesSkipped >= startY) {
                                                        skippingLines = false;
                                                        linesSkipped = 0;
                                                    }
                                                }
                                                if (width > longestWidth) {
                                                    longestWidth = width;
                                                }
                                                width = 0;
                                            }
                                        }
                                        //}
                                    } else {
                                        width = wordWidth;
                                        if (width > longestWidth) {
                                            longestWidth = width;
                                        }
                                    }
                                    break;
                                }
                                wordWidth += sizes[z].Width;

                                z++;
                            }
                            i = z;
                            if (z < textItem.Length && textItem[z] != '\n') {
                                width += sizes[z].Width;
                            } else if (z < textItem.Length && textItem[z] == '\n') {
                                if (width > longestWidth) {
                                    longestWidth = width;
                                }
                                width = 0;
                                if (skippingLines) {
                                    linesSkipped++;
                                    if (skippingLines && linesSkipped >= startY) {
                                        skippingLines = false;
                                        linesSkipped = 0;
                                    }
                                } else {
                                    height += font.Height;
                                }
                            }
                        }
                        //if (i < textItem.Length) {
                        //    width += font.SizeText(textItem[i].ToString()).Width;
                        //}
                    } else {
                        height += font.Height;
                        if (width > longestWidth) {
                            longestWidth = width;
                        }
                        width = 0;
                    }
                }
            }
            if (width > longestWidth) {
                longestWidth = width;
            }
            if (maxWidth == 0) {
                if (longestWidth == 0) {
                    longestWidth = width;
                }
            }

            Surface surf = new Surface(longestWidth, height);
            surf.Fill(Color.Transparent);
            surf.TransparentColor = Color.Transparent;
            surf.Transparent = true;

            int lastX = 0;
            int lastY = 0;

            linesSkipped = 0;
            skippingLines = startY > 0;
            for (int i = 0; i < textItem.Length; i++) {
                if (textItem[i] != '\n') {
                    if (maxWidth > 0) {
                        int z = i;
                        int wordWidth = lastX;
                        while (true) {
                            if (z >= textItem.Length || textItem[z] == ' ' || textItem[z] == '\n') {
                                if (wordWidth > maxWidth) {
                                    if (!skippingLines) {
                                        lastY += font.Height;
                                    } else {
                                        linesSkipped++;
                                        if (skippingLines && linesSkipped >= startY) {
                                            skippingLines = false;
                                            linesSkipped = 0;
                                        }
                                    }
                                    lastX = 0;
                                    CharRenderOptions renderOptions = null;
                                    for (int n = i; n < z; n++) {
                                        if (charRenderOptions != null && charRenderOptions.Length > n) {
                                            renderOptions = charRenderOptions[n];
                                        }
                                        if (!skippingLines) {
                                            RenderLetterByGlyph(surf, font, textItem[n], lastX, lastY, textColor, antiAlias, renderOptions);
                                        }
                                        lastX += sizes[n].Width;
                                        if (lastX > maxWidth) {
                                            if (!skippingLines) {
                                                lastY += font.Height;
                                            } else {
                                                linesSkipped++;
                                                if (skippingLines && linesSkipped >= startY) {
                                                    skippingLines = false;
                                                    linesSkipped = 0;
                                                }
                                            }
                                            lastX = 0;
                                        }
                                    }
                                    i = z - 1;
                                    //if (z < textItem.Length && textItem[z] != '\n') {
                                    //    renderOptions = null;
                                    //    if (charRenderOptions != null && charRenderOptions.Length > z) {
                                    //        renderOptions = charRenderOptions[z];
                                    //    }
                                    //    RenderLetter(surf, font, textItem[z], lastX, lastY, textColor, antiAlias, renderOptions);
                                    //    lastX += font.SizeText(textItem[z].ToString()).Width;
                                    //} else if (z < textItem.Length && textItem[z] == '\n') {
                                    //    lastX = 0;
                                    //    lastY += font.Height;
                                    //}
                                } else {
                                    CharRenderOptions renderOptions = null;
                                    for (int n = i; n < z; n++) {
                                        if (charRenderOptions != null && charRenderOptions.Length > n) {
                                            renderOptions = charRenderOptions[n];
                                        }
                                        if (!skippingLines) {
                                            RenderLetterByGlyph(surf, font, textItem[n], lastX, lastY, textColor, antiAlias, renderOptions);
                                        }
                                        lastX += sizes[n].Width;
                                        if (lastX > maxWidth) {
                                            if (!skippingLines) {
                                                lastY += font.Height;
                                            } else {
                                                linesSkipped++;
                                                if (linesSkipped >= startY && skippingLines) {
                                                    skippingLines = false;
                                                    linesSkipped = 0;
                                                }
                                            }
                                            lastX = 0;
                                        }
                                    }
                                    i = z;
                                    //if (i < textItem.Length) {
                                    //    if (textItem[i] == '\n') {
                                    //        i--;
                                    //        //if (skippingLines) {
                                    //        //    linesSkipped++;
                                    //        //    if (linesSkipped >= startY && skippingLines) {
                                    //        //        skippingLines = false;
                                    //        //        linesSkipped = 0;
                                    //        //    }
                                    //        //} else {
                                    //        //    lastY += font.Height;
                                    //        //    lastX = 0;
                                    //        //}
                                    //    } else if (textItem[i] == ' ') {
                                    //        int textWidth = font.SizeText(textItem[i].ToString()).Width;
                                    //        if (lastX + textWidth <= maxWidth) {
                                    //            if (!skippingLines) {
                                    //                RenderLetter(surf, font, textItem[i], lastX, lastY, textColor, antiAlias, renderOptions);
                                    //            }
                                    //            lastX += textWidth;
                                    //        } else {
                                    //            if (skippingLines) {
                                    //                linesSkipped++;
                                    //                if (linesSkipped >= startY && skippingLines) {
                                    //                    skippingLines = false;
                                    //                    linesSkipped = 0;
                                    //                }
                                    //            } else {
                                    //                lastY += font.Height;
                                    //                lastX = 0;
                                    //            }
                                    //        }
                                    //    }
                                    //}
                                    if (z < textItem.Length && textItem[z] != '\n') {
                                        renderOptions = null;
                                        if (charRenderOptions != null && charRenderOptions.Length > z) {
                                            renderOptions = charRenderOptions[z];
                                        }
                                        if (!skippingLines) {
                                            RenderLetterByGlyph(surf, font, textItem[z], lastX, lastY, textColor, antiAlias, renderOptions);
                                        }
                                        lastX += sizes[z].Width;
                                    } else if (z < textItem.Length && textItem[z] == '\n') {
                                        lastX = 0;
                                        if (skippingLines) {
                                            linesSkipped++;
                                            if (skippingLines && linesSkipped >= startY) {
                                                skippingLines = false;
                                                linesSkipped = 0;
                                            }
                                        } else {
                                            lastY += font.Height;
                                        }
                                    }
                                    if (linesSkipped >= startY && skippingLines) {
                                        skippingLines = false;
                                        //if (linesSkipped > startY) {
                                        //    lastY += (linesSkipped * font.Height);
                                        //}
                                        linesSkipped = 0;
                                        if (z < textItem.Length && !(textItem[z] == '\n' || textItem[z] == ' ')) {
                                            //lastY += font.Height;
                                            i--;
                                        }
                                    }
                                }
                                break;
                            }
                            wordWidth += sizes[z].Width;

                            z++;
                        }
                        //lastX += font.SizeText(textItem[i].ToString()).Width;

                    } else {
                        CharRenderOptions renderOptions = null;
                        if (charRenderOptions != null && charRenderOptions.Length > i) {
                            renderOptions = charRenderOptions[i];
                        }
                        RenderLetterByGlyph(surf, font, textItem[i], lastX, lastY, textColor, antiAlias, renderOptions);
                        lastX += sizes[i].Width;
                    }

                } else {
                    lastX = 0;
                    if (skippingLines) {
                        linesSkipped++;
                        if (linesSkipped >= startY && skippingLines) {
                            skippingLines = false;
                            linesSkipped = 0;
                        }
                    } else {
                        lastY += font.Height;
                    }
                }
            }
            return surf;
        }

        public static Size SizeText(SdlDotNet.Graphics.Font font, string textItem, bool antiAlias, int maxWidth) {
            int maxHeight = 0;
            int startY = 0;

            int width = 0;
            int height = font.Height;
            int longestWidth = 0;
            int linesSkipped = 0;
            bool skippingLines = startY > 0;

            if (maxWidth > 0) {
                longestWidth = maxWidth;
            }
            if (maxHeight > 0) {
                height = maxHeight;
            }
            if (maxWidth == 0 || maxHeight == 0) {
                for (int i = 0; i < textItem.Length; i++) {
                    if (textItem[i] != '\n') {
                        if (maxWidth == 0) {
                            width += font.SizeText(textItem[i].ToString()).Width;
                        } else {
                            int z = i;
                            int wordWidth = width;
                            while (true) {
                                if (z >= textItem.Length || textItem[z] == ' ' || textItem[z] == '\n') {
                                    if (wordWidth >= maxWidth) {
                                        if (skippingLines) {
                                            linesSkipped++;
                                            width = wordWidth - width;
                                        } else {
                                            height += font.Height;
                                            width = 0;
                                            for (int n = i; n < z; n++) {
                                                width += font.SizeText(textItem[n].ToString()).Width;
                                                if (width > maxWidth) {
                                                    height += font.Height;
                                                    width = 0;
                                                }
                                            }
                                        }
                                    } else {
                                        width = wordWidth;
                                        //if (textItem[z] == '\n') {
                                        //    height += font.Height;
                                        //    width = 0;
                                        //}
                                    }
                                    break;
                                }
                                wordWidth += font.SizeText(textItem[z].ToString()).Width;

                                z++;
                            }
                            i = z;
                            if (z < textItem.Length && textItem[z] != '\n') {
                                width += font.SizeText(textItem[z].ToString()).Width;
                            } else if (z < textItem.Length && textItem[z] == '\n') {
                                width = 0;
                                if (skippingLines) {
                                    linesSkipped++;
                                } else {
                                    height += font.LineSize;
                                }
                            }
                            //if (i < textItem.Length) {
                            //    width += font.SizeText(textItem[i].ToString()).Width;
                            //}
                        }
                    } else {
                        height += font.LineSize;
                        if (width > longestWidth) {
                            longestWidth = width;
                        }
                        width = 0;
                    }
                }
            }
            if (maxWidth == 0) {
                if (longestWidth == 0) {
                    longestWidth = width;
                }
            }

            return new Size(longestWidth, height);
        }

        public static Size SizeText2(SdlDotNet.Graphics.Font font, string textItem, bool antiAlias, int maxWidth) {
            int width = 0;
            int height = font.Height;
            int longestWidth = 0;
            GlyphData[] glyphDataCache = new GlyphData[textItem.Length];
            for (int i = 0; i < textItem.Length; i++) {
                if (textItem[i] != '\n') {
                    bool moved = false;
                    if (maxWidth > 0) {
                        int z = i;
                        int wordWidth = width;
                        while (true) {
                            if (z >= textItem.Length || textItem[z] == ' ' || textItem[z] == '\n') {
                                if (wordWidth > maxWidth) {
                                    height += font.Height;
                                    width = 0;

                                    for (int n = i; n < z; n++) {
                                        width += glyphDataCache[n].Advance;
                                        if (width > maxWidth) {
                                            height += font.Height;
                                            width = 0;
                                        }
                                    }


                                    i = z - 1;
                                    moved = true;
                                }
                                break;
                            }
                            if (glyphDataCache[z].Advance == 0) {
                                glyphDataCache[z] = font.GetGlyphMetrics(textItem[z]);
                            }
                            //wordWidth += font.SizeText(textItem[z].ToString()).Width;
                            wordWidth += glyphDataCache[z].Advance;

                            //if (wordWidth > maxWidth) {
                            //    height += font.Height;
                            //    wordWidth = 0;
                            //    width = 0;
                            //}

                            z++;
                        }
                    }

                    if (!moved) {
                        if (glyphDataCache[i].Advance == 0) {
                            glyphDataCache[i] = font.GetGlyphMetrics(textItem[i]);
                        }
                        width += glyphDataCache[i].Advance;
                        //width += font.SizeText(textItem[i].ToString()).Width;
                        if (width > longestWidth) {
                            longestWidth = width;
                        }
                        if (maxWidth > 0 && width > maxWidth) {
                            height += font.Height;
                            width = 0;
                        }
                    }
                } else {
                    height += font.Height;
                    if (width > longestWidth) {
                        longestWidth = width;
                    }
                    width = 0;
                }
            }

            int surfaceWidth;
            if (maxWidth > 0) {
                surfaceWidth = maxWidth;
            } else {
                surfaceWidth = longestWidth;
            }

            return new Size(surfaceWidth, height);
        }

        private static void RenderLetter(SdlDotNet.Graphics.Surface destSurf, SdlDotNet.Graphics.Font font, char letter, GlyphData glyphData, int x, int y, Color textColor, bool antiAlias, CharRenderOptions renderOptions) {
            Color foreColor = textColor;
            Color backColor = Color.Empty;
            if (renderOptions != null) {
                if (renderOptions.ForeColor != Color.Empty) {
                    foreColor = renderOptions.ForeColor;
                }
                if (font.Bold != renderOptions.Bold) {
                    font.Bold = renderOptions.Bold;
                }
                if (font.Italic != renderOptions.Italic) {
                    font.Italic = renderOptions.Italic;
                }
                if (font.Underline != renderOptions.Underline) {
                    font.Underline = renderOptions.Underline;
                }
                backColor = renderOptions.BackColor;
            }
            //if (foreColor != Color.Empty) {
                //SdlDotNet.Graphics.Surface fontSurf = font.Render(letter.ToString(), foreColor, backColor, antiAlias);
                // Render by glyph, it's 5x faster than using the normal font.Render method
                //GlyphData glyphData = font.GetGlyphMetrics(letter);
                SdlDotNet.Graphics.Surface fontSurf = font.RenderGlyph(letter, foreColor, backColor, false);
                if (fontSurf.Handle != IntPtr.Zero) {
                    if (font.Bold == true) {
                        font.Bold = false;
                    }
                    if (font.Italic == true) {
                        font.Italic = false;
                    }
                    if (font.Underline == true) {
                        font.Underline = false;
                    }
                    destSurf.Blit(fontSurf, new Point(x, y + font.Ascent - glyphData.YMax));
                    //destSurf.Blit(fontSurf, new Point(x, y));
                    fontSurf.Close();
                } else {
                    Console.WriteLine("Test!");
                }
            //} else {
            //    Console.WriteLine("Test!");
            //}
        }

        private static void RenderLetterByGlyph(SdlDotNet.Graphics.Surface destSurf, SdlDotNet.Graphics.Font font, char letter, int x, int y, Color textColor, bool antiAlias, CharRenderOptions renderOptions) {
            Color foreColor = textColor;
            Color backColor = Color.Empty;
            if (renderOptions != null) {
                if (renderOptions.ForeColor != Color.Empty) {
                    foreColor = renderOptions.ForeColor;
                }
                if (font.Bold != renderOptions.Bold) {
                    font.Bold = renderOptions.Bold;
                }
                if (font.Italic != renderOptions.Italic) {
                    font.Italic = renderOptions.Italic;
                }
                if (font.Underline != renderOptions.Underline) {
                    font.Underline = renderOptions.Underline;
                }
                backColor = renderOptions.BackColor;
            }
            //SdlDotNet.Graphics.Surface fontSurf = font.Render(letter.ToString(), foreColor, backColor, antiAlias);
            GlyphData glyphData = font.GetGlyphMetrics(letter);
            SdlDotNet.Graphics.Surface fontSurf = font.RenderGlyph(letter, foreColor, backColor, false);
            if (font.Bold == true) {
                font.Bold = false;
            }
            if (font.Italic == true) {
                font.Italic = false;
            }
            if (font.Underline == true) {
                font.Underline = false;
            }
            destSurf.Blit(fontSurf, new Point(x, y + font.Ascent - glyphData.YMax));
            fontSurf.Close();
        }

        #endregion Methods
    }
}