#region LICENSE
/*
 * $RCSfile$
 * Copyright (C) 2006 - 2007 David Hudson (jendave@yahoo.com)
 * Copyright (C) 2007 Jonathan Porter (jono.porter@gmail.com)
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
#endregion LICENSE

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Diagnostics.CodeAnalysis;

using SdlDotNet.Graphics;
using Tao.OpenGl;

namespace SdlDotNet.OpenGl
{
    /// <summary>
    /// Loads a Surface into a OpenGl Texture.   
    /// </summary>
    [SuppressMessage("Microsoft.Naming", "CA1706:ShortAcronymsShouldBeUppercase", Justification = "Correct Spelling")]
    public class SurfaceGl : IDisposable 
    {
        #region Static
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
        #endregion

        #region Fields 
        Surface surface;
        bool isFlipped;
        int textureID;
        int textureWidth;
        int textureHeight;
        float widthRatio;
        float heightRatio; 
        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new Instance of SurfaceGl.
        /// </summary>
        /// <param name="surface">The surface to be copied into a OpenGl Texture.</param>
        public SurfaceGl(Surface surface)
            : this(surface, true)
        { }

        /// <summary>
        /// Creates a new Instance of SurfaceGl.
        /// </summary>
        /// <param name="surface">The surface to be copied into a OpenGl Texture.</param>
        /// <param name="flipSurface">States if the surface should be flipped when copied into a OpenGl Texture.</param>
        public SurfaceGl(Surface surface, bool flipSurface)
        {
            if (surface == null) { throw new ArgumentNullException("surface"); }
            this.surface = surface;
            this.isFlipped = flipSurface;
            this.textureID = -1;
            this.textureWidth = -1;
            this.textureHeight = -1;
            this.widthRatio = -1;
            this.heightRatio = -1;
        } 
        #endregion

        #region Properties
        /// <summary>
        /// Gets and Sets the surface the Texture is made from.
        /// </summary>
        public Surface Surface
        {
            get { return surface; }
            set
            {
                if (value == null) { throw new ArgumentNullException("value"); }
                if (surface != value)
                {
                    surface = value;
                    Refresh();
                }
            }
        }

        /// <summary>
        /// Gets the Width of the texture.
        /// </summary>
        public int TextureWidth
        {
            get { Check(); return textureWidth; }
        }

        /// <summary>
        /// Gets the Height of the texture.
        /// </summary>
        public int TextureHeight
        {
            get { Check(); return textureHeight; }
        }

        /// <summary>
        /// The Percent of the OpenGl Texture the original Surface is utilizing along it's Width.
        /// </summary>
        public float WidthRatio
        {
            get { Check(); return widthRatio; }
        }

        /// <summary>
        /// The Percent of the OpenGl Texture the original Surface  is utilizing along it's Height.
        /// </summary>
        public float HeightRatio
        {
            get { Check(); return heightRatio; }
        }

        /// <summary>
        /// Gets the OpenGl Texture Name.
        /// </summary>
        public int TextureID
        {
            get { Check(); return textureID; }
        }

        /// <summary>
        /// Gets and Sets if the texture is Flipped.
        /// </summary>
        public bool IsFlipped
        {
            get { return isFlipped; }
            set
            {
                if (isFlipped ^ value)
                {
                    Refresh(surface, value);
                }
            }
        } 

        #endregion

        #region Methods
        private void Check()
        {
            if (textureID <= 0)
            {
                Refresh();
            }
        }

        private Surface TransformSurface(bool flipSurface)
        {
            byte alpha = surface.Alpha;
            Surface textureSurface2 = null;
            surface.Alpha = 0;
            try
            {
                Surface textureSurface = surface;
                if (flipSurface)
                {
                    textureSurface = textureSurface.CreateFlippedVerticalSurface();
                    textureSurface2 = textureSurface;
                    textureSurface2.Alpha = 0;
                }
                return textureSurface.CreateResizedSurface();
            }
            finally
            {
                surface.Alpha = alpha;
                if (flipSurface)
                {
                    textureSurface2.Dispose();
                }
            }
        }

        /// <summary>
        /// Deletes the texture from OpenGl memory.
        /// </summary>
        public void Delete()
        {
            if (Gl.glIsTexture(this.textureID) != 0)
            {
                int[] texId = new int[] { textureID };
                Gl.glDeleteTextures(1, texId);
            }
            this.textureID = -1;
            this.textureWidth = -1;
            this.textureHeight = -1;
            this.widthRatio = -1;
            this.heightRatio = -1;
        }

        /// <summary>
        /// Reloads the OpenGl Texture from the Surface.
        /// </summary>
        public void Refresh()
        {
            Refresh(surface, isFlipped);
        }

        /// <summary>
        /// Reloads the OpenGl Texture from the Surface.
        /// </summary>
        /// <param name="surface">The surface to load from.</param>
        /// <param name="flipSurface">States if the surface should be flipped when moved into the OpenGl Texture.</param>
        public void Refresh(Surface surface, bool flipSurface)
        {
            if (surface == null) { throw new ArgumentNullException("surface"); }
            this.surface = surface;
            this.Delete();
            using (Surface textureSurface = TransformSurface(flipSurface))
            {
                int[] textures = new int[1];
                Gl.glGenTextures(1, textures);
                this.textureID = textures[0];

                this.textureWidth = textureSurface.Width;
                this.textureHeight = textureSurface.Height;
                this.isFlipped = flipSurface;
                this.widthRatio = (float)surface.Width / textureWidth;
                this.heightRatio = (float)surface.Height / textureHeight;

                Gl.glBindTexture(Gl.GL_TEXTURE_2D, textureID);
                Gl.glTexImage2D(Gl.GL_TEXTURE_2D, 0, textureSurface.BytesPerPixel, textureWidth, textureHeight, 0, Gl.GL_RGBA, Gl.GL_UNSIGNED_BYTE, textureSurface.Pixels);
                Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MIN_FILTER, Gl.GL_LINEAR);
                Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MAG_FILTER, Gl.GL_LINEAR);
            }
        }

        /// <summary>
        /// Should Be removed as soon as possible.
        /// Need to change the OpenGl Font Example to stop its reliance on this method.
        /// </summary>
        /// <param name="surface"> ;) </param>
        [Obsolete("The Load method is Obsolete. Use the Surface property.")]
        public void Load(Surface surface)
        {
            this.Surface = surface;
        }

        /// <summary>
        /// Draws the Texture.
        /// </summary>
        public void Draw()
        {
            this.Draw(new Point(0, 0));
        }

        /// <summary>
        /// Draws the Texture.
        /// </summary>
        /// <param name="location">The offset for the Texture.</param>
        public void Draw(Point location)
        {
            Check();
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, this.textureID);
            Gl.glBegin(Gl.GL_QUADS);
            //Gl.glColor4f(1, 1, 1, 1);
            Gl.glTexCoord2f(0, heightRatio);
            Gl.glVertex2f(location.X, location.Y);
            Gl.glTexCoord2f(widthRatio, heightRatio);
            Gl.glVertex2f(location.X + textureWidth, location.Y);
            Gl.glTexCoord2f(widthRatio, 0);
            Gl.glVertex2f(location.X + textureWidth, location.Y + textureHeight);
            Gl.glTexCoord2f(0, 0);
            Gl.glVertex2f(location.X, location.Y + textureHeight);
            Gl.glEnd();
        }

        /// <summary>
        ///  Disposes the Object by freeing all OpenGl memory allocated to it.
        /// </summary>
        /// <param name="disposing">States if it is disposing.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Delete();
            }
        }

        /// <summary>
        /// Disposes the Object by freeing all OpenGl memory allocated to it.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
