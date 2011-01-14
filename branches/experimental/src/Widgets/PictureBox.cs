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

    public class PictureBox : Widget
    {
        #region Fields

        Surface highlightImage;
        Surface image;
        string imageLocation;
        ImageSizeMode sizeMode;

        #endregion Fields

        #region Constructors

        public PictureBox(string name)
            : base(name) {
            base.BackColor = Color.Transparent;
            sizeMode = ImageSizeMode.Normal;
            base.MouseEnter += new EventHandler(PictureBox_MouseEnter);
            base.MouseLeave += new EventHandler(PictureBox_MouseLeave);

            base.Paint += new EventHandler(PictureBox_Paint);
        }



        #endregion Constructors

        #region Properties

        public Surface HighlightImage {
            get { return highlightImage; }
            set {
                if (value != null) {
                    if (highlightImage != null) {
                        highlightImage.Close();
                    }
                    highlightImage = new Surface(value);
                    highlightImage.Transparent = value.Transparent;
                    highlightImage.TransparentColor = value.TransparentColor;
                } else {
                    if (highlightImage != null) {
                        highlightImage.Dispose();
                    }
                    highlightImage = null;
                }
                RequestRedraw();
            }
        }

        public SdlDotNet.Graphics.Surface Image {
            get { return image; }
            set {
                if (value != null) {
                    if (image != null) {
                        image.Close();
                    }
                    image = new Surface(value);
                    image.Transparent = value.Transparent;
                    image.TransparentColor = value.TransparentColor;
                } else {
                    if (image != null) {
                        image.Dispose();
                    }
                    image = null;
                }
                if (sizeMode == ImageSizeMode.AutoSize) {
                    if (base.Size != image.Size) {
                        base.Size = image.Size;
                    }
                }
                RequestRedraw();
            }
        }

        public string ImageLocation {
            get { return imageLocation; }
            set {
                imageLocation = value;
                this.Image = new SdlDotNet.Graphics.Surface(imageLocation);
            }
        }

        public ImageSizeMode SizeMode {
            get { return sizeMode; }
            set {
                sizeMode = value;
                RequestRedraw();
            }
        }

        #endregion Properties

        #region Methods

        public void BlitToBuffer(Surface sourceSurface, Rectangle sourceRectangle) {
            base.TriggerPaint();
            base.Buffer.Blit(sourceSurface, new Point(0, 0), sourceRectangle);

            base.DrawBorder();
        }

        public override void FreeResources() {
            base.FreeResources();
            if (image != null) {
                image.Close();
                image = null;
            }
            if (highlightImage != null) {
                highlightImage.Close();
                highlightImage = null;
            }
        }

        void PictureBox_Paint(object sender, EventArgs e) {
            if (highlightImage != null && base.MouseInBounds) {
                Point drawPoint = new Point(0, 0);
                switch (sizeMode) {
                    case ImageSizeMode.AutoSize: {
                            if (base.Size != highlightImage.Size) {
                                base.Size = highlightImage.Size;
                            }
                            base.Buffer.Blit(highlightImage, drawPoint);
                            break;
                        }
                    case ImageSizeMode.CenterImage: {
                            drawPoint = DrawingSupport.GetCenter(base.Buffer, highlightImage.Size);
                            base.Buffer.Blit(highlightImage, drawPoint, new Rectangle(0, 0, base.Width, base.Height));
                            break;
                        }
                    case ImageSizeMode.Normal: {
                            base.Buffer.Blit(highlightImage, drawPoint, new Rectangle(0, 0, base.Width, base.Height));
                            break;
                        }
                    case ImageSizeMode.StretchImage: {
                            Surface stretchedSurface = highlightImage.CreateStretchedSurface(base.Size);
                            base.Buffer.Blit(stretchedSurface, drawPoint);
                            stretchedSurface.Close();
                            stretchedSurface = null;
                            break;
                        }
                    default: {
                            base.Buffer.Blit(highlightImage, drawPoint);
                            break;
                        }
                }
            } else if (image != null) {
                Point drawPoint = new Point(0, 0);
                switch (sizeMode) {
                    case ImageSizeMode.AutoSize: {
                            if (base.Size != image.Size) {
                                base.Size = image.Size;
                            }
                            base.Buffer.Blit(image, drawPoint);
                            break;
                        }
                    case ImageSizeMode.CenterImage: {
                            drawPoint = DrawingSupport.GetCenter(base.Buffer, image.Size);
                            base.Buffer.Blit(image, drawPoint, new Rectangle(0, 0, base.Width, base.Height));
                            break;
                        }
                    case ImageSizeMode.Normal: {
                            base.Buffer.Blit(image, drawPoint, new Rectangle(0, 0, base.Width, base.Height));
                            break;
                        }
                    case ImageSizeMode.StretchImage: {
                            Surface strechedSurface = image.CreateStretchedSurface(base.Size);
                            base.Buffer.Blit(strechedSurface, drawPoint);
                            strechedSurface.Close();
                            strechedSurface = null;
                            break;
                        }
                    default: {
                            base.Buffer.Blit(image, drawPoint);
                            break;
                        }
                }
            }
            base.DrawBorder();
        }

        void PictureBox_MouseEnter(object sender, EventArgs e) {
            if (highlightImage != null) {
                RequestRedraw();
            }
        }

        void PictureBox_MouseLeave(object sender, EventArgs e) {
            if (highlightImage != null) {
                RequestRedraw();
            }
        }

        #endregion Methods
    }
}