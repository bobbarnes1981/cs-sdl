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
    /// The texture minifying function is used whenever
    /// the	pixel being textured maps to an	area greater
    /// than one texture element. There are	six defined
    /// minifying functions.  Two of them use the nearest
    /// one	or nearest four	texture	elements to compute
    /// the	texture	value. The other four use mipmaps.
    /// 
    /// A mipmap is	an ordered set of arrays representing
    /// the	same image at progressively lower resolutions.
    /// If the texture has dimensions 2nx2m, there are
    /// max(n,m)+1 mipmaps.	The first mipmap is the
    /// original texture, with dimensions 2nx2m. Each
    /// subsequent mipmap has dimensions 2k-1x2l-1,	where
    /// 2kx2l are the dimensions of	the previous mipmap,
    /// until either k=0 or	l=0.  At that point,
    /// subsequent mipmaps have dimension 1x2l-1 or	2k-1x1
    /// until the final mipmap, which has dimension	1x1.
    /// To define the mipmaps, call	glTexImage1D,
    /// glTexImage2D, glCopyTexImage1D, or
    /// glCopyTexImage2D with the level argument
    /// indicating the order of the	mipmaps.  Level	0 is
    /// the	original texture; level	max(n,m) is the	final
    /// 1x1	mipmap.
    /// </summary>
    public enum MinifyingOption : int
    {
        /// <summary>
        /// Returns the value	of the texture element
		///	that is nearest (in Manhattan distance)
		///	to the center of the pixel being
		///	textured.
        /// </summary>
        GL_NEAREST = Gl.GL_NEAREST,
        /// <summary>
        /// Returns the weighted average of the four
		/// texture elements that are closest	to the
		/// center of the pixel being textured.
		/// These can include border texture
		/// elements, depending on the values	of
		/// GL_TEXTURE_WRAP_S and GL_TEXTURE_WRAP_T,
        /// and on the exact mapping.
        /// </summary>
        GL_LINEAR = Gl.GL_LINEAR,
        /// <summary>
        /// Chooses the mipmap that most closely
		/// matches the size of the pixel being
		/// textured and uses the GL_NEAREST
		/// criterion (the texture element nearest
		/// to the center of the pixel) to produce a
        /// texture value.
        /// </summary>
        GL_NEAREST_MIPMAP_NEAREST = Gl.GL_NEAREST_MIPMAP_NEAREST,
        /// <summary>
        /// Chooses the mipmap that most closely
		/// matches the size of the pixel being
		/// textured and uses the GL_LINEAR
		/// criterion (a weighted average of the
		/// four texture elements that are closest
		/// to the center of the pixel) to produce a
        /// texture value.
        /// </summary>
        GL_LINEAR_MIPMAP_NEAREST = Gl.GL_LINEAR_MIPMAP_NEAREST,
        /// <summary>
        /// Chooses the two mipmaps that most
		/// closely match the	size of	the pixel
		/// being textured and uses the GL_NEAREST
		/// criterion	(the texture element nearest
		/// to the center of the pixel) to produce a
		/// texture value from each mipmap. The
		/// final texture value is a weighted
        /// average of those two values.
        /// </summary>
        GL_NEAREST_MIPMAP_LINEAR = Gl.GL_NEAREST_MIPMAP_LINEAR,
        /// <summary>
        /// Chooses the two mipmaps that most
		/// closely match the size of the pixel
		/// being textured and uses the GL_LINEAR
		/// criterion (a weighted average of the
		/// four texture elements that are closest
		/// to the center of the pixel) to produce a
		/// texture value from each mipmap. The
		/// final texture value is a weighted
        /// average of those two values.
        /// </summary>
        GL_LINEAR_MIPMAP_LINEAR = Gl.GL_LINEAR_MIPMAP_LINEAR
    }
    /// <summary>
    /// The texture magnification function is used when
    /// the	pixel being textured maps to an	area less than
    /// or equal to	one texture element.  It sets the
    /// texture magnification function to either
    /// GL_NEAREST or GL_LINEAR. GL_NEAREST is
    /// generally faster than GL_LINEAR, but it can
    /// produce textured images with sharper edges because
    /// the	transition between texture elements is not as
    /// smooth. The	initial	value of GL_TEXTURE_MAG_FILTER
    /// is GL_LINEAR.
    /// </summary>
    public enum MagnificationOption : int
    {
        /// <summary>
        /// Returns the value	of the texture element
        ///	that is nearest (in Manhattan distance)
        ///	to the center of the pixel being
        ///	textured.
        /// </summary>
        GL_NEAREST = Gl.GL_NEAREST,
        /// <summary>
        /// Returns the weighted average of the four
        /// texture elements that are closest	to the
        /// center of the pixel being textured.
        /// These can include border texture
        /// elements, depending on the values	of
        /// GL_TEXTURE_WRAP_S and GL_TEXTURE_WRAP_T,
        /// and on the exact mapping.
        /// </summary>
        GL_LINEAR = Gl.GL_LINEAR
    }
    /// <summary>
    /// The wrap parameter for a texture coordinate
    /// </summary>
    public enum WrapOption : int
    {
        /// <summary>
        /// Causes texture coordinates to be clamped to the range [0,1] and
		/// is useful for preventing wrapping artifacts	when
        /// mapping a single image onto	an object.
        /// </summary>
        GL_CLAMP = Gl.GL_CLAMP,
        /// <summary>
        /// Causes texture coordinates to loop around so to remain in the 
        /// range [0,1] where 1.5 would be .5. this is useful for repeating
        /// a texture for a tiled floor.
        /// </summary>
        GL_REPEAT = Gl.GL_REPEAT
    }

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

        private static bool IsMipMap(MinifyingOption option)
        {
            return option != MinifyingOption.GL_LINEAR && option != MinifyingOption.GL_NEAREST;
        }
        
        #endregion

        #region Fields
        Surface surface;
        bool isFlipped;
        int textureId;
        int textureWidth;
        int textureHeight;
        float widthRatio;
        float heightRatio;

        bool needRefresh;
        bool needSetOptions;
        MinifyingOption minFilter;
        MagnificationOption magFilter;
        WrapOption wrapS;
        WrapOption wrapT;
        #endregion

        #region Constructors
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
        /// <param name="isFlipped">States if the surface should be flipped when copied into a OpenGl Texture.</param>
        public SurfaceGl(Surface surface, bool isFlipped)
        {
            if (surface == null) { throw new ArgumentNullException("surface"); }
            this.surface = surface;
            this.isFlipped = isFlipped;
            this.textureId = -1;
            this.textureWidth = -1;
            this.textureHeight = -1;
            this.widthRatio = -1;
            this.heightRatio = -1;
            this.minFilter = MinifyingOption.GL_LINEAR;
            this.magFilter = MagnificationOption.GL_LINEAR;
            this.wrapS = WrapOption.GL_REPEAT;
            this.wrapT = WrapOption.GL_REPEAT;
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
                    needRefresh = true;
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
        /// Gets the Percent of the OpenGl Texture the original Surface is utilizing along it's Width.
        /// </summary>
        public float WidthRatio
        {
            get { Check(); return widthRatio; }
        }

        /// <summary>
        /// Gets the Percent of the OpenGl Texture the original Surface  is utilizing along it's Height.
        /// </summary>
        public float HeightRatio
        {
            get { Check(); return heightRatio; }
        }

        /// <summary>
        /// Gets the OpenGl Texture Name.
        /// </summary>
        public int TextureId
        {
            get { Check(); return textureId; }
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
                    isFlipped = value;
                    needRefresh = true;
                }
            }
        }

        /// <summary>
        /// Gets and Sets 
        /// The texture minifying function is used whenever
		/// the	pixel being textured maps to an	area greater
		/// than one texture element. There are	six defined
		/// minifying functions.  Two of them use the nearest
		/// one	or nearest four	texture	elements to compute
		/// the	texture	value. The other four use mipmaps.
        /// 
		/// A mipmap is	an ordered set of arrays representing
		/// the	same image at progressively lower resolutions.
		/// If the texture has dimensions 2nx2m, there are
		/// max(n,m)+1 mipmaps.	The first mipmap is the
		/// original texture, with dimensions 2nx2m. Each
		/// subsequent mipmap has dimensions 2k-1x2l-1,	where
		/// 2kx2l are the dimensions of	the previous mipmap,
		/// until either k=0 or	l=0.  At that point,
		/// subsequent mipmaps have dimension 1x2l-1 or	2k-1x1
		/// until the final mipmap, which has dimension	1x1.
		/// To define the mipmaps, call	glTexImage1D,
		/// glTexImage2D, glCopyTexImage1D, or
		/// glCopyTexImage2D with the level argument
		/// indicating the order of the	mipmaps.  Level	0 is
		/// the	original texture; level	max(n,m) is the	final
        /// 1x1	mipmap.
        /// </summary>
        public MinifyingOption MinFilter
        {
            get { return minFilter; }
            set
            {
                if (minFilter != value)
                {
                    if (IsMipMap(minFilter) ^ IsMipMap(value))
                    {
                        needRefresh = true;
                    }
                    else
                    {
                        needSetOptions = true;
                    }
                    minFilter = value;
                }
            }
        }
        
        /// <summary>
        /// Gets and Sets 
        /// The texture magnification function is used when
		/// the	pixel being textured maps to an	area less than
		/// or equal to	one texture element.  It sets the
		/// texture magnification function to either
		/// GL_NEAREST or GL_LINEAR. GL_NEAREST is
		/// generally faster than GL_LINEAR, but it can
		/// produce textured images with sharper edges because
		/// the	transition between texture elements is not as
		/// smooth. The	initial	value of GL_TEXTURE_MAG_FILTER
        /// is GL_LINEAR.
        /// </summary>
        public MagnificationOption MagFilter
        {
            get { return magFilter; }
            set
            {
                if (magFilter != value)
                {
                    magFilter = value;
                    needSetOptions = true;
                }
            }
        }

        /// <summary>
        /// Gets and Sets
        /// The wrap parameter for texture coordinate s
		/// to either GL_CLAMP or GL_REPEAT.  GL_CLAMP causes
		/// s coordinates to be	clamped	to the range [0,1] and
		/// is useful for preventing wrapping artifacts	when
		/// mapping a single image onto	an object. GL_REPEAT
		/// causes the integer part of the s coordinate	to be
		/// ignored; the GL uses only the fractional part,
		/// thereby creating a repeating pattern. Border
		/// texture elements are accessed only if wrapping is
		/// set	to GL_CLAMP.  Initially, GL_TEXTURE_WRAP_S is
        /// set	to GL_REPEAT.
        /// </summary>
        public WrapOption WrapS
        {
            get { return wrapS; }
            set
            {
                if (wrapS != value)
                {
                    wrapS = value;
                    needSetOptions = true;
                }
            }
        }
        
        /// <summary>
        /// Gets and Sets
        /// The wrap parameter for	texture	coordinate t
        /// to either GL_CLAMP or GL_REPEAT.  GL_CLAMP causes
        /// t coordinates to be	clamped	to the range [0,1] and
        /// is useful for preventing wrapping artifacts	when
        /// mapping a single image onto	an object. GL_REPEAT
        /// causes the integer part of the s coordinate	to be
        /// ignored; the GL uses only the fractional part,
        /// thereby creating a repeating pattern. Border
        /// texture elements are accessed only if wrapping is
        /// set	to GL_CLAMP.  Initially, GL_TEXTURE_WRAP_T is
        /// set	to GL_REPEAT.
        /// </summary>
        public WrapOption WrapT
        {
            get { return wrapT; }
            set
            {
                if (wrapT != value)
                {
                    wrapT = value;
                    needSetOptions = true;
                }
            }
        }

        #endregion

        #region Methods

        private void Check()
        {
            if (needRefresh || textureId <= 0)
            {
                Refresh();
            }
            else if (needSetOptions)
            {
                BindOptions();
            }
        }

        private void BindOptions()
        {
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, textureId);
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MIN_FILTER, (int)minFilter);
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MAG_FILTER, (int)magFilter);
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_WRAP_S, (int)wrapS);
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_WRAP_T, (int)wrapT);
            needSetOptions = false;
        }

        private Surface TransformSurface(bool isFlipped)
        {
            byte alpha = surface.Alpha;
            Surface textureSurface2 = null;
            surface.Alpha = 0;
            try
            {
                Surface textureSurface = surface;
                if (isFlipped)
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
                if (isFlipped)
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
            if (Gl.glIsTexture(this.textureId) != 0)
            {
                int[] texId = new int[] { textureId };
                Gl.glDeleteTextures(1, texId);
            }
            this.textureId = -1;
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
            Refresh(this.surface, this.isFlipped, this.minFilter, this.magFilter, this.wrapS, this.wrapT);
        }

        /// <summary>
        /// Reloads the OpenGl Texture from the Surface.
        /// </summary>
        /// <param name="surface">The surface to load from.</param>
        /// <param name="isFlipped">States if the surface should be flipped when moved into the OpenGl Texture.</param>
        public void Refresh(Surface surface, bool isFlipped)
        {
            Refresh(surface, isFlipped, this.minFilter, this.magFilter, this.wrapS, this.wrapT);
        }
        
        /// <summary>
        /// Reloads the OpenGl Texture from the Surface.
        /// </summary>
        /// <param name="surface">The surface to load from.</param>
        /// <param name="isFlipped">States if the surface should be flipped when moved into the OpenGl Texture.</param>
        /// <param name="minFilter">"The openGl filter used for minifying"</param>
        /// <param name="magFilter">"The openGl filter used for magnification"</param>
        /// <param name="wrapS">The wrap parameter for texture coordinate S</param>
        /// <param name="wrapT">The wrap parameter for texture coordinate T</param>
        public void Refresh(Surface surface, bool isFlipped, MinifyingOption minFilter, MagnificationOption magFilter, WrapOption wrapS, WrapOption wrapT)
        {
            if (surface == null) { throw new ArgumentNullException("surface"); }
            this.surface = surface;
            this.Delete();
            using (Surface textureSurface = TransformSurface(isFlipped))
            {
                int[] textures = new int[1];
                Gl.glGenTextures(1, textures);
                this.textureId = textures[0];

                this.textureWidth = textureSurface.Width;
                this.textureHeight = textureSurface.Height;
                this.isFlipped = isFlipped;
                this.minFilter = minFilter;
                this.magFilter = magFilter;
                this.wrapS = wrapS;
                this.wrapT = wrapT;
                this.widthRatio = (float)surface.Width / textureWidth;
                this.heightRatio = (float)surface.Height / textureHeight;

                Gl.glBindTexture(Gl.GL_TEXTURE_2D, textureId);
                Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MIN_FILTER, (int)minFilter);
                Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MAG_FILTER, (int)magFilter);

                Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_WRAP_S, (int)wrapS);
                Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_WRAP_T, (int)wrapT);



                if (minFilter == MinifyingOption.GL_LINEAR || minFilter == MinifyingOption.GL_NEAREST)
                {
                    Gl.glTexImage2D(Gl.GL_TEXTURE_2D, 0, textureSurface.BytesPerPixel, textureWidth, textureHeight, 0, Gl.GL_RGBA, Gl.GL_UNSIGNED_BYTE, textureSurface.Pixels);
                }
                else
                {
                    Glu.gluBuild2DMipmaps(Gl.GL_TEXTURE_2D, textureSurface.BytesPerPixel, textureWidth, textureHeight, Gl.GL_RGBA, Gl.GL_UNSIGNED_BYTE, textureSurface.Pixels);
                }

                needRefresh = false;
                needSetOptions = false;
            }
        }

        /// <summary>
        /// Draws the Texture.
        /// </summary>
        public void Draw()
        {
            this.Draw(0, 0, surface.Width, surface.Height);
        }

        /// <summary>
        /// Draws the Texture.
        /// </summary>
        /// <param name="location">The offset for the Texture.</param>
        public void Draw(Point location)
        {
            Draw(location.X, location.Y,surface.Width,surface.Height);
        }

        /// <summary>
        /// Draws the Texture.
        /// </summary>
        /// <param name="rectangle">the rectagle where the texture will be drawn.</param>
        public void Draw(Rectangle rectangle)
        {
            Draw(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
        }
       
        /// <summary>
        /// Draws the Texture.
        /// </summary>
        /// <param name="locationX">The x offset for the Texture.</param>
        /// <param name="locationY">The y offset for the Texture.</param>
        /// <param name="width">The width fo the area where the Texture will be drawn.</param>
        /// <param name="height">The height fo the area where the Texture will be drawn.</param>
        public void Draw(float locationX, float locationY, float width, float height)
        {
            Check();
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, this.textureId);
            Gl.glBegin(Gl.GL_QUADS);
            Gl.glTexCoord2f(0, heightRatio);
            Gl.glVertex2f(locationX, locationY);
            Gl.glTexCoord2f(widthRatio, heightRatio);
            Gl.glVertex2f(locationX + width, locationY);
            Gl.glTexCoord2f(widthRatio, 0);
            Gl.glVertex2f(locationX + width, locationY + height);
            Gl.glTexCoord2f(0, 0);
            Gl.glVertex2f(locationX, locationY + height);
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
