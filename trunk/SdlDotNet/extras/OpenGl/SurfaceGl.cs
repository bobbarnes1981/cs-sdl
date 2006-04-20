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
		Surface tempSurface;
		Bitmap textureImage;
		int texture;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="surface"></param>
		public SurfaceGl(Surface surface)
		{
			this.surface = surface;
		}

		/// <summary>
		/// 
		/// </summary>
		public SurfaceGl()
		{
		}

		static SurfaceGl()
		{
		}

		/// <summary>
		/// 
		/// </summary>
		public int TextureID
		{
			get
			{
				return this.texture;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public int GenerateTexture()
		{
			this.tempSurface = new Surface(NextPowerOfTwo(surface.Width), NextPowerOfTwo(surface.Height));
			this.tempSurface.Blit(surface);
			// Status Indicator
			this.textureImage = tempSurface.Bitmap;   
			// Create Storage Space For The Texture	
			// Load The Bitmap
			// Check For Errors, If Bitmap's Not Found, Quit
			if(textureImage != null) 
			{
				// Create The Texture
				Gl.glGenTextures(1, out this.texture); 
				return this.texture;
				
			}
			else
			{
				return 0;
			}
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
				this.surface = value;
				this.GenerateTexture();
			}
		}

		/// <summary>
		/// Load bitmaps and convert to textures.
		/// </summary>
		public void Draw(Point location) 
		{
			// Rectangle For Locking The Bitmap In Memory
			Rectangle rectangle = 
				new Rectangle(0, 0, textureImage.Width, textureImage.Height);
			// Get The Bitmap's Pixel Data From The Locked Bitmap
			BitmapData bitmapData = 
				textureImage.LockBits(rectangle, ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
						
			// Typical Texture Generation Using Data From The Bitmap
			Gl.glBindTexture(Gl.GL_TEXTURE_2D, texture);
			Gl.glTexImage2D(Gl.GL_TEXTURE_2D, 0, Gl.GL_RGB8, textureImage.Width, textureImage.Height, 0, Gl.GL_BGR, Gl.GL_UNSIGNED_BYTE, bitmapData.Scan0);
					
			Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MIN_FILTER, Gl.GL_LINEAR);
			Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MAG_FILTER, Gl.GL_LINEAR);

			int[] vPort= new int[4];
  
			Gl.glGetIntegerv(Gl.GL_VIEWPORT, vPort);
  
			Gl.glMatrixMode(Gl.GL_PROJECTION);
			Gl.glPushMatrix();
			Gl.glLoadIdentity();
  
			Gl.glOrtho(0, vPort[2], 0, vPort[3], -1, 1);
			Gl.glMatrixMode(Gl.GL_MODELVIEW);
			Gl.glPushMatrix();
			Gl.glLoadIdentity();

			Gl.glDisable(Gl.GL_DEPTH_TEST);
			Gl.glEnable(Gl.GL_TEXTURE_2D);
			Gl.glColor3f(1.0f, 1.0f, 1.0f);
	
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

			Gl.glDeleteTextures(1, ref texture);

			Gl.glEnable(Gl.GL_DEPTH_TEST);
			Gl.glMatrixMode(Gl.GL_PROJECTION);
			Gl.glPopMatrix();   
			Gl.glMatrixMode(Gl.GL_MODELVIEW);
			Gl.glPopMatrix();

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
