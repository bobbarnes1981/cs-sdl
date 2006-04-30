/*
 * $RCSfile$
 * Copyright (C) 2006 David Hudson (jendave@yahoo.com)
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
using SdlDotNet;
using Tao.OpenGl;

namespace SdlDotNet.OpenGl
{
	/// <summary>
	/// Summary description for FontGl.
	/// </summary>
	public class SurfaceGl
	{
		Surface surface;
		Bitmap textureImage;
		int textureID;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="surface"></param>
		public SurfaceGl(Surface surface)
		{
			this.Surface = surface;
		}

		/// <summary>
		/// 
		/// </summary>
		public SurfaceGl()
		{
		}

		/// <summary>
		/// 
		/// </summary>
		public int TextureID
		{
			get
			{
				if (this.textureID == 0)
				{
					Gl.glGenTextures(1, out this.textureID);
				}
				return this.textureID;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public Bitmap TextureImage
		{
			get
			{
				return this.textureImage;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public Surface ResizeSurface(Surface surface)
		{
			return surface.Resize(new Size(NextPowerOfTwo(this.surface.Width), NextPowerOfTwo(this.surface.Height)));
		}

		private static int NextPowerOfTwo(int x)
		{
//			int bit = 1;
//			while ( bit < x)
//			{
//				bit <<=1;
//			}
//			return bit;
			double logbase2 = (Math.Log(x) / Math.Log(2));
			return (int)Math.Round(Math.Pow(2, Math.Ceiling(logbase2)));
		}

		/// <summary>
		/// Load bitmaps and convert to textures.
		/// </summary>
		public void Draw() 
		{
			this.Draw(new Point(0,0));
		}

		/// <summary>
		/// 
		/// </summary>
		public Surface Surface
		{
			get
			{
				return surface;
			}
			set
			{
				this.surface = value.Resize(new Size(NextPowerOfTwo(value.Width), NextPowerOfTwo(value.Height)));;
				this.textureID = this.TextureID;
				this.textureImage = this.surface.Bitmap;
			}
		}

		/// <summary>
		/// Load bitmaps and convert to textures.
		/// </summary>
		public void Draw(Point location) 
		{
			if (textureImage != null)
			{
				Surface screen = Video.Screen;

				this.textureImage.RotateFlip(RotateFlipType.RotateNoneFlipY); 
				/* Note, there may be other things you need to change,
				   depending on how you have your OpenGL state set up.
				*/
				Gl.glPushAttrib(Gl.GL_ENABLE_BIT);
				Gl.glDisable(Gl.GL_DEPTH_TEST);
				Gl.glDisable(Gl.GL_CULL_FACE);
				Gl.glEnable(Gl.GL_TEXTURE_2D);

				/* This allows alpha blending of 2D textures with the scene */
				Gl.glEnable(Gl.GL_BLEND);
				Gl.glBlendFunc(Gl.GL_SRC_ALPHA, Gl.GL_ONE_MINUS_SRC_ALPHA);

				Gl.glViewport(0, 0, screen.Width, screen.Height);

				Gl.glMatrixMode(Gl.GL_PROJECTION);
				Gl.glPushMatrix();
				Gl.glLoadIdentity();

				Gl.glOrtho(0.0, (double)screen.Width, (double)screen.Height, 0.0, 0.0, 1.0);

				Gl.glMatrixMode(Gl.GL_MODELVIEW);
				Gl.glPushMatrix();
				Gl.glLoadIdentity();

				Gl.glTexEnvf(Gl.GL_TEXTURE_ENV, Gl.GL_TEXTURE_ENV_MODE, Gl.GL_DECAL);

				// Rectangle For Locking The Bitmap In Memory
				Rectangle rectangle = 
					new Rectangle(0, 0, textureImage.Width, textureImage.Height);

				// Get The Bitmap's Pixel Data From The Locked Bitmap
				BitmapData bitmapData = 
					textureImage.LockBits(rectangle, ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
						
				// Typical Texture Generation Using Data From The Bitmap
				Gl.glBindTexture(Gl.GL_TEXTURE_2D, this.textureID);
				Gl.glTexImage2D(Gl.GL_TEXTURE_2D, 0, Gl.GL_RGBA, textureImage.Width, textureImage.Height, 0, Gl.GL_BGR, Gl.GL_UNSIGNED_BYTE, bitmapData.Scan0);
					
				Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MIN_FILTER, Gl.GL_LINEAR);
				Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MAG_FILTER, Gl.GL_LINEAR);
	
				/* Draw a quad at location */
				Gl.glBegin(Gl.GL_QUADS);
				/* Recall that the origin is in the lower-left corner
					   That is why the TexCoords specify different corners
					   than the Vertex coors seem to. */
				Gl.glTexCoord2f(0.0f, 1.0f); 
				Gl.glVertex2f(location.X, location.Y);
				Gl.glTexCoord2f(1.0f, 1.0f); 
				Gl.glVertex2f(location.X + textureImage.Width, location.Y);
				Gl.glTexCoord2f(1.0f, 0.0f); 
				Gl.glVertex2f(location.X + textureImage.Width, location.Y + textureImage.Height);
				Gl.glTexCoord2f(0.0f, 0.0f); 
				Gl.glVertex2f(location.X, location.Y+ textureImage.Height);
				Gl.glEnd();
	
				/* Bad things happen if we delete the texture before it finishes */
				Gl.glFinish();

				Gl.glDeleteTextures(1, ref this.textureID);
				Gl.glDisable(Gl.GL_BLEND);	
				Gl.glEnable(Gl.GL_DEPTH_TEST);
				Gl.glMatrixMode(Gl.GL_MODELVIEW);
				Gl.glPopMatrix();
				Gl.glMatrixMode(Gl.GL_PROJECTION);
				Gl.glPopMatrix();
				Gl.glPopAttrib();

				if(textureImage != null) 
				{
					// If Texture Exists
					textureImage.UnlockBits(bitmapData); 
					// Unlock The Pixel Data From Memory
					textureImage.Dispose();   
					// Dispose The Bitmap
				}
			}
		}
	}
}
