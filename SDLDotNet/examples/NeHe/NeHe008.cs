#region License
/*
MIT License
Copyright ©2003-2005 Tao Framework Team
http://www.taoframework.com
All rights reserved.

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/
#endregion License

using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Reflection;

using SdlDotNet;
using Tao.OpenGl;

namespace SdlDotNet.Examples
{
	class NeHe008 : NeHe006
	{    
		private float xrot;                                              
		// X Rotation ( NEW )
		private float yrot;                                              
		// Y Rotation ( NEW )
		private float zrot;                                              
		// Z Rotation ( NEW )
		private float xspeed;                                            
		// X Rotation Speed
		private float yspeed;                                            
		// Y Rotation Speed
		private float z = -5;
		private int filter;                                              
		// Which Filter To Use
		private int[] texture = new int[3];                              
		// Storage For 3 Textures

		string data_directory = @"Data/";
		string filepath = @"../../";

		public override void DrawGLScene()
		{
			Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);        // Clear The Screen And The Depth Buffer
			Gl.glLoadIdentity();                                                // Reset The View
			Gl.glTranslatef(0, 0, z);

			Gl.glRotatef(xrot, 1, 0, 0);
			Gl.glRotatef(yrot, 0, 1, 0);

			Gl.glBindTexture(Gl.GL_TEXTURE_2D, texture[filter]);

			Gl.glBegin(Gl.GL_QUADS);
			// Front Face
			Gl.glNormal3f(0, 0, 1);
			Gl.glTexCoord2f(0, 0); Gl.glVertex3f(-1, -1, 1);
			Gl.glTexCoord2f(1, 0); Gl.glVertex3f(1, -1, 1);
			Gl.glTexCoord2f(1, 1); Gl.glVertex3f(1, 1, 1);
			Gl.glTexCoord2f(0, 1); Gl.glVertex3f(-1, 1, 1);
			// Back Face
			Gl.glNormal3f(0, 0, -1);
			Gl.glTexCoord2f(1, 0); Gl.glVertex3f(-1, -1, -1);
			Gl.glTexCoord2f(1, 1); Gl.glVertex3f(-1, 1, -1);
			Gl.glTexCoord2f(0, 1); Gl.glVertex3f(1, 1, -1);
			Gl.glTexCoord2f(0, 0); Gl.glVertex3f(1, -1, -1);
			// Top Face
			Gl.glNormal3f(0, 1, 0);
			Gl.glTexCoord2f(0, 1); Gl.glVertex3f(-1, 1, -1);
			Gl.glTexCoord2f(0, 0); Gl.glVertex3f(-1, 1, 1);
			Gl.glTexCoord2f(1, 0); Gl.glVertex3f(1, 1, 1);
			Gl.glTexCoord2f(1, 1); Gl.glVertex3f(1, 1, -1);
			// Bottom Face
			Gl.glNormal3f(0, -1, 0);
			Gl.glTexCoord2f(1, 1); Gl.glVertex3f(-1, -1, -1);
			Gl.glTexCoord2f(0, 1); Gl.glVertex3f(1, -1, -1);
			Gl.glTexCoord2f(0, 0); Gl.glVertex3f(1, -1, 1);
			Gl.glTexCoord2f(1, 0); Gl.glVertex3f(-1, -1, 1);
			// Right face
			Gl.glNormal3f(1, 0, 0);
			Gl.glTexCoord2f(1, 0); Gl.glVertex3f(1, -1, -1);
			Gl.glTexCoord2f(1, 1); Gl.glVertex3f(1, 1, -1);
			Gl.glTexCoord2f(0, 1); Gl.glVertex3f(1, 1, 1);
			Gl.glTexCoord2f(0, 0); Gl.glVertex3f(1, -1, 1);
			// Left Face
			Gl.glNormal3f(-1, 0, 0);
			Gl.glTexCoord2f(0, 0); Gl.glVertex3f(-1, -1, -1);
			Gl.glTexCoord2f(1, 0); Gl.glVertex3f(-1, -1, 1);
			Gl.glTexCoord2f(1, 1); Gl.glVertex3f(-1, 1, 1);
			Gl.glTexCoord2f(0, 1); Gl.glVertex3f(-1, 1, -1);
			Gl.glEnd();

			xrot += xspeed;
			yrot += yspeed;
			Video.GLSwapBuffers();
		}

		#region void LoadGLTextures()
		/// <summary>
		///     Load bitmaps and convert to textures.
		/// </summary>
		protected override void LoadGLTextures() 
		{
			if (File.Exists(data_directory + "NeHe008.bmp"))
			{
				filepath = "";
			}                                              
			// Status Indicator
			Bitmap[] textureImage = new Bitmap[1];                              
			// Create Storage Space For The Texture

			textureImage[0] = new Bitmap(filepath + data_directory + "NeHe008.bmp");                
			// Load The Bitmap
			// Check For Errors, If Bitmap's Not Found, Quit
			if(textureImage[0] != null) 
			{
				Gl.glGenTextures(1, out texture[0]);                            
				// Create The Texture

				textureImage[0].RotateFlip(RotateFlipType.RotateNoneFlipY);     
				// Flip The Bitmap Along The Y-Axis
				// Rectangle For Locking The Bitmap In Memory
				Rectangle rectangle = 
					new Rectangle(0, 0, textureImage[0].Width, textureImage[0].Height);
				// Get The Bitmap's Pixel Data From The Locked Bitmap
				BitmapData bitmapData = 
					textureImage[0].LockBits(rectangle, ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);

				// Typical Texture Generation Using Data From The Bitmap
				Gl.glBindTexture(Gl.GL_TEXTURE_2D, texture[0]);
				Gl.glTexImage2D(Gl.GL_TEXTURE_2D, 0, Gl.GL_RGB8, textureImage[0].Width, textureImage[0].Height, 0, Gl.GL_BGR, Gl.GL_UNSIGNED_BYTE, bitmapData.Scan0);
				Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MIN_FILTER, Gl.GL_LINEAR);
				Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MAG_FILTER, Gl.GL_LINEAR);

				if(textureImage[0] != null) 
				{                                   
					// If Texture Exists
					textureImage[0].UnlockBits(bitmapData);                     
					// Unlock The Pixel Data From Memory
					textureImage[0].Dispose();                                  
					// Dispose The Bitmap
				}
			}                                                     
		}
		#endregion bool LoadGLTextures()

	}
}