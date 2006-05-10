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
	/// Summary description for SurfaceGl.
	/// </summary>
	public class SurfaceGl
	{
		Surface surface;
		Bitmap textureImage;
		int textureID;
		BitmapData bitmapData;
		bool mode2D;

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
				int texture;
				Gl.glGenTextures(1, out texture);
				Gl.glBindTexture(Gl.GL_TEXTURE_2D, texture);
				// Rectangle For Locking The Bitmap In Memory
				Rectangle rectangle = 
					new Rectangle(0, 0, textureImage.Width, textureImage.Height);
				this.textureImage.RotateFlip(RotateFlipType.RotateNoneFlipY); 

				// Get The Bitmap's Pixel Data From The Locked Bitmap
				bitmapData = 
					textureImage.LockBits(rectangle, ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
						
				// Typical Texture Generation Using Data From The Bitmap
				Gl.glTexImage2D(Gl.GL_TEXTURE_2D, 0, Gl.GL_RGBA, textureImage.Width, textureImage.Height, 0, Gl.GL_BGR, Gl.GL_UNSIGNED_BYTE, bitmapData.Scan0);
				Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MIN_FILTER, Gl.GL_LINEAR);
				Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MAG_FILTER, Gl.GL_LINEAR);
				
				//}
				return texture;
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
			return surface.Resize(new Size(NextPowerOfTwo(surface.Width), NextPowerOfTwo(surface.Height)));
		}

		private static int NextPowerOfTwo(int x)
		{
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
				this.surface = this.ResizeSurface(value);
				this.textureImage = this.surface.Bitmap;
				this.textureID = this.TextureID;
			}
		}

		private bool Mode2D
		{
			get
			{
				return this.mode2D;
			}
			set
			{
				if (value)
				{
					Surface screen = Video.Screen;

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
					this.mode2D = value;
				}
				else
				{
					//Gl.glDeleteTextures(1, ref this.textureID);
					//Gl.glDisable(Gl.GL_BLEND);	
					//Gl.glEnable(Gl.GL_DEPTH_TEST);
					Gl.glMatrixMode(Gl.GL_MODELVIEW);
					Gl.glPopMatrix();
					Gl.glMatrixMode(Gl.GL_PROJECTION);
					Gl.glPopMatrix();
					Gl.glPopAttrib();
					this.mode2D = value;
				}
			}
		}

		/// <summary>
		/// Load bitmaps and convert to textures.
		/// </summary>
		public void Draw(Point location) 
		{
			if (textureImage != null)
			{
				Mode2D = true;

				Gl.glBindTexture(Gl.GL_TEXTURE_2D, this.textureID);
				Gl.glBegin(Gl.GL_QUADS);
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
				//Gl.glFinish();

				Mode2D = false;

				if(textureImage != null) 
				{
					// If Texture Exists
					//textureImage.UnlockBits(bitmapData); 
					// Unlock The Pixel Data From Memory
					//textureImage.Dispose();   
					// Dispose The Bitmap
				}
			}
		}
	}
}
