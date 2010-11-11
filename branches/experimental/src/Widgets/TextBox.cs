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

    using Sdl = SdlDotNet;

    public class TextBox : Widget
    {
        #region Fields

        Point cursor;
        int cursorTextPos = -1;
        Sdl.Graphics.Font font;
        Object lockObject = new Object();
        char passwordChar = '\0';
        int renderStartX = 0;
        string text;
        int textWidth = 0;

        #endregion Fields

        #region Constructors

        public TextBox(string name)
            : base(name) {
            cursor = new Point(0, 0);
            text = "";

            base.KeyRepeatInterval = 25;

            base.BeforeKeyDown += new EventHandler<BeforeKeyDownEventArgs>(TextBox_BeforeKeyDown);
            base.KeyDown += new EventHandler<Input.KeyboardEventArgs>(TextBox_KeyDown);
        }

        #endregion Constructors

        #region Properties

        public Sdl.Graphics.Font Font {
            get { return font; }
            set {
                if (font != null) {
                    font.Close();
                }
                font = value;
            }
        }

        public new Color ForeColor {
            get { return base.ForeColor; }
            set {
                base.ForeColor = value;
                RequestRedraw();
            }
        }

        public Char PasswordChar {
            get { return passwordChar; }
            set {
                passwordChar = value;
                RequestRedraw();
            }
        }

        public string Text {
            get { return text; }
            set {
                if (value == null) {
                    value = "";
                }
                SetText(value);
                //text = value;
            }
        }

        #endregion Properties

        #region Methods

        public override void FreeResources() {
            base.FreeResources();
            if (font != null) {
                font.Close();
            }
        }

        protected override void DrawBuffer() {
            lock (lockObject) {
                base.DrawBuffer();
                if (!string.IsNullOrEmpty(text) && font != null) {
                    int totalWidth = 0;
                    int startChar = -1;
                    for (int i = 0; i < text.Length; i++) {
                        int charWidth = font.SizeText(text[i].ToString()).Width;
                        if (totalWidth + charWidth >= renderStartX) {
                            startChar = i;
                            break;
                        } else {
                            totalWidth += charWidth;
                        }
                    }
                    if (renderStartX == 0) {
                        startChar = 0;
                    }
                    if (startChar > -1) {
                        Sdl.Graphics.Surface textSurface;
                        if (passwordChar == '\0') {
                            textSurface = font.Render(text.Substring(startChar), base.ForeColor, true);
                        } else {
                            textSurface = font.Render(new string(passwordChar, text.Length - startChar), base.ForeColor, true);
                        }
                        base.Buffer.Blit(textSurface, new Point(0, 0));
                        textSurface.Close();
                    }
                }
                Sdl.Graphics.Primitives.Line line = new Sdl.Graphics.Primitives.Line(
                    (short)cursor.X, (short)(cursor.Y + 5), (short)cursor.X, (short)(cursor.Y + 10));
                base.Buffer.Draw(line, Color.Blue);
            }
        }

        void CheckFont() {
            if (font == null) {
                font = new Sdl.Graphics.Font(Widgets.DefaultFontPath, Widgets.DefaultFontSize);
            }
        }

        void SetText(string newText) {
            CheckFont();
            cursor.X = 0;
            cursorTextPos = -1;
            textWidth = 0;
            renderStartX = 0;
            text = "";
            for (int i = 0; i < newText.Length; i++) {
                string charString = newText[i].ToString();
                if (charString.Length == 1) {
                    if (cursorTextPos == text.Length - 1) {
                        text += charString;
                    } else {
                        text = text.Insert(cursorTextPos, charString);
                    }
                    int charWidth = font.SizeText(charString).Width;
                    cursor.X += charWidth;
                    cursorTextPos += 1;
                    textWidth += charWidth;

                    if (cursor.X > this.Width) {
                        cursor.X -= charWidth;
                        renderStartX += charWidth;
                    }

                }
            }
            RequestRedraw();
        }

        void TextBox_BeforeKeyDown(object sender, BeforeKeyDownEventArgs e) {
            switch (e.KeyboardEventArgs.Key) {
                case Input.Key.LeftArrow:
                case Input.Key.RightArrow:
                    e.UseKeyRepeat = true;
                    break;
                default:
                    e.UseKeyRepeat = false;
                    break;
            }
        }

        void TextBox_KeyDown(object sender, Input.KeyboardEventArgs e) {
            lock (lockObject) {
                CheckFont();
                switch (e.Key) {
                    case Input.Key.Backspace: {
                            if (cursorTextPos > -1) {
                                string charString = text[cursorTextPos].ToString();
                                text = text.Remove(cursorTextPos, 1);
                                int charWidth = font.SizeText(charString).Width;
                                cursor.X -= charWidth;
                                textWidth -= charWidth;
                                cursorTextPos -= 1;

                                if (cursor.X < 0) {
                                    cursor.X += charWidth;
                                    renderStartX -= charWidth;
                                }

                                RequestRedraw();
                            }
                        }
                        break;
                    case Input.Key.LeftArrow: {
                            if (cursorTextPos > 0) {
                                string charString = text[cursorTextPos].ToString();
                                int charWidth = font.SizeText(charString).Width;
                                cursor.X -= charWidth;
                                cursorTextPos -= 1;

                                if (cursor.X < 0) {
                                    renderStartX -= charWidth;
                                    cursor.X += charWidth;
                                }

                                RequestRedraw();
                            }
                        }
                        break;
                    case Input.Key.RightArrow: {
                            if (cursorTextPos < text.Length - 1) {
                                string charString = text[cursorTextPos].ToString();
                                int charWidth = font.SizeText(charString).Width;
                                cursor.X += charWidth;
                                cursorTextPos += 1;

                                if (cursor.X > this.Width) {
                                    renderStartX += charWidth;
                                    cursor.X -= charWidth;
                                }

                                RequestRedraw();
                            }
                        }
                        break;
                    default: {
                            string charString = Keyboard.GetCharString(e, base.keysDown);
                            if (charString.Length == 1) {
                                if (cursorTextPos == text.Length - 1) {
                                    text += charString;
                                } else {
                                    text = text.Insert(cursorTextPos, charString);
                                }
                                int charWidth = font.SizeText(charString).Width;
                                cursor.X += charWidth;
                                cursorTextPos += 1;
                                textWidth += charWidth;

                                if (cursor.X > this.Width) {
                                    cursor.X -= charWidth;
                                    renderStartX += charWidth;
                                }

                                RequestRedraw();
                            }
                        }
                        break;
                }
            }
        }

        #endregion Methods
    }
}