/*
 * $RCSfile$
 * Copyright (C) 2006 - 2007 David Hudson (jendave@yahoo.com)
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

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Diagnostics.CodeAnalysis;

using SdlDotNet.Graphics;
using Tao.OpenGl;

namespace SdlDotNet.OpenGl
{
    /// <summary>
    /// Summary description for SurfaceGl.
    /// </summary>
    [SuppressMessage("Microsoft.Naming", "CA1706:ShortAcronymsShouldBeUppercase", Justification = "Correct Spelling")]
    public class SurfaceGl
    {
        //Surface surface;
        Surface textureImage;
        int textureID;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="surface"></param>
        public SurfaceGl(Surface surface)
        {
            this.textureImage = this.LoadInternal(surface);
        }

        private Surface LoadInternal(Surface surface)
        {
            try
            {
                surface.FlipVertical();
                surface.Resize();
                int[] texture = new int[1];
                Gl.glGenTextures(1, texture);
                this.textureID = texture[0];
                Gl.glBindTexture(Gl.GL_TEXTURE_2D, textureID);
                Gl.glTexImage2D(Gl.GL_TEXTURE_2D, 0, surface.BytesPerPixel, surface.Width, surface.Height, 0, Gl.GL_RGBA, Gl.GL_UNSIGNED_BYTE, surface.Pixels);

                Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MIN_FILTER, Gl.GL_LINEAR);
                Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MAG_FILTER, Gl.GL_LINEAR);
            }
            catch (AccessViolationException e)
            {
                e.ToString();
            }

            return surface;
        }

        /// <summary>
        /// 
        /// </summary>
        public int TextureId
        {
            get
            {
                return this.textureID;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Surface TextureImage
        {
            get
            {
                return this.textureImage;
            }
        }


        /// <summary>
        /// Load bitmaps and convert to textures.
        /// </summary>
        public void Draw()
        {
            this.Draw(new Point(0, 0));
        }

        /// <summary>
        /// 
        /// </summary>
        public void Load(Surface surface)
        {
            try
            {
                if (surface == null)
                {
                    throw new ArgumentNullException("surface");
                }

                int[] textures = { this.textureID };
                Gl.glDeleteTextures(1, textures);
                if (this.textureImage != null)
                {
                    this.textureImage.Dispose();
                    this.textureImage = null;
                }
                this.textureImage = this.LoadInternal(surface);
            }
            catch (AccessViolationException e)
            {
                e.ToString();
            }
        }

        static bool mode2D;

        /// <summary>
        /// 
        /// </summary>
        public static bool Mode2D
        {
            get
            {
                return mode2D;
            }
            set
            {
                if (value)
                {
                    Gl.glPushAttrib(Gl.GL_ENABLE_BIT);
                    Gl.glDisable(Gl.GL_DEPTH_TEST);
                    Gl.glDisable(Gl.GL_CULL_FACE);
                    Gl.glEnable(Gl.GL_TEXTURE_2D);

                    /* This allows alpha blending of 2D textures with the scene */
                    Gl.glEnable(Gl.GL_BLEND);
                    Gl.glBlendFunc(Gl.GL_SRC_ALPHA, Gl.GL_ONE_MINUS_SRC_ALPHA);

                    Gl.glViewport(0, 0, Video.Screen.Width, Video.Screen.Height);

                    Gl.glMatrixMode(Gl.GL_PROJECTION);
                    Gl.glPushMatrix();
                    Gl.glLoadIdentity();

                    Gl.glOrtho(0.0, (double)Video.Screen.Width, (double)Video.Screen.Height, 0.0, 0.0, 1.0);

                    Gl.glMatrixMode(Gl.GL_MODELVIEW);
                    Gl.glPushMatrix();
                    Gl.glLoadIdentity();

                    Gl.glTexEnvf(Gl.GL_TEXTURE_ENV, Gl.GL_TEXTURE_ENV_MODE, Gl.GL_DECAL);
                    mode2D = value;
                }
                else
                {
                    Gl.glDisable(Gl.GL_BLEND);
                    Gl.glEnable(Gl.GL_DEPTH_TEST);
                    Gl.glMatrixMode(Gl.GL_MODELVIEW);
                    Gl.glPopMatrix();
                    Gl.glMatrixMode(Gl.GL_PROJECTION);
                    Gl.glPopMatrix();
                    Gl.glPopAttrib();
                    mode2D = value;
                }
            }
        }

        /// <summary>
        /// Load bitmaps and convert to textures.
        /// </summary>
        public void Draw(Point location)
        {
            //if (textureImage != null)
            //{
                Gl.glBindTexture(Gl.GL_TEXTURE_2D, this.textureID);
                Gl.glBegin(Gl.GL_QUADS);
                Gl.glTexCoord2f(0.0f, 1.0f);
                Gl.glVertex2f(location.X, location.Y);
                Gl.glTexCoord2f(1.0f, 1.0f);
                Gl.glVertex2f(location.X + textureImage.Width, location.Y);
                Gl.glTexCoord2f(1.0f, 0.0f);
                Gl.glVertex2f(location.X + textureImage.Width, location.Y + textureImage.Height);
                Gl.glTexCoord2f(0.0f, 0.0f);
                Gl.glVertex2f(location.X, location.Y + textureImage.Height);
                Gl.glEnd();
            //}
        }
    }
}
