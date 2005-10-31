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

using SdlDotNet;
using Tao.OpenGl;

namespace SdlDotNet.Examples
{
	class NeHe020 : NeHeBase
	{

		bool scene = false;				// Which Scene To Draw
		bool masking = true;
		float roll = 0.0f;				// Rolling Texture
		
		uint[] texture = new uint[5];	// Storage For Our Five Textures

		public NeHe020()
		{
			Events.KeyboardDown += new KeyboardEventHandler(Events_KeyboardDown);
		}

		public override void InitGL()
		{
			LoadTextures();

			Gl.glEnable(Gl.GL_TEXTURE_2D);									// Enable Texture Mapping
			Gl.glShadeModel(Gl.GL_SMOOTH);									// Enable Smooth Shading
			Gl.glClearColor(0.0f, 0.0f, 0.0f, 0.0f);						// Black Background
			Gl.glClearDepth(1.0f);											// Depth Buffer Setup
			Gl.glEnable(Gl.GL_DEPTH_TEST);
		}

		public void LoadTextures()
		{
			string[] file = {"NeHe020.Logo.bmp", "NeHe020.Mask1.bmp", "NeHe020.Image1.bmp", "NeHe020.Mask2.bmp", "NeHe020.Image2.bmp"};
			Bitmap[] image = new Bitmap[5];

			for(int i = 0; i < file.Length; i++)
			{
				string finalFile = "";
				string file2 = "Data" + Path.DirectorySeparatorChar + file[i];
				string file3 = ".." + Path.DirectorySeparatorChar + ".." + Path.DirectorySeparatorChar + file2;
				if(File.Exists(file[i]))
					finalFile = file[i];
				else if(File.Exists(file2))
					finalFile = file2;
				else if(File.Exists(file3))
					finalFile = file3;
				else
					throw new FileNotFoundException(file[i]);
				image[i] = new Bitmap(finalFile);
			}

			Gl.glGenTextures(image.Length, this.texture);
		
			for (int i=0; i < image.Length; i++)
			{
				image[i].RotateFlip(RotateFlipType.RotateNoneFlipY);
				System.Drawing.Imaging.BitmapData bitmapdata;
				Rectangle rect = new Rectangle(0, 0, image[i].Width, image[i].Height);

				bitmapdata = image[i].LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

				// Create Linear Filtered Texture
				Gl.glBindTexture(Gl.GL_TEXTURE_2D, this.texture[i]);
				Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MAG_FILTER, Gl.GL_LINEAR);
				Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MIN_FILTER, Gl.GL_LINEAR);
				Gl.glTexImage2D(Gl.GL_TEXTURE_2D, 0, (int)Gl.GL_RGB, image[i].Width, image[i].Height, 0, Gl.GL_BGR_EXT, Gl.GL_UNSIGNED_BYTE, bitmapdata.Scan0);

				image[i].UnlockBits(bitmapdata);
				image[i].Dispose();
			}
		}

		public override void DrawGLScene()
		{
			Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
			Gl.glLoadIdentity();
			
			Gl.glTranslatef(0.0f, 0.0f, -2.0f);						// Move Into The Screen 5 Units

			Gl.glBindTexture(Gl.GL_TEXTURE_2D, this.texture[0]);			// Select Our Logo Texture
			Gl.glBegin(Gl.GL_QUADS);									// Start Drawing A Textured Quad
			Gl.glTexCoord2f(0.0f, -this.roll+0.0f); Gl.glVertex3f(-1.1f, -1.1f,  0.0f);	// Bottom Left
			Gl.glTexCoord2f(3.0f, -this.roll+0.0f); Gl.glVertex3f( 1.1f, -1.1f,  0.0f);	// Bottom Right
			Gl.glTexCoord2f(3.0f, -this.roll+3.0f); Gl.glVertex3f( 1.1f,  1.1f,  0.0f);	// Top Right
			Gl.glTexCoord2f(0.0f, -this.roll+3.0f); Gl.glVertex3f(-1.1f,  1.1f,  0.0f);	// Top Left
			Gl.glEnd();											// Done Drawing The Quad

			Gl.glEnable(Gl.GL_BLEND);									// Enable Blending
			Gl.glDisable(Gl.GL_DEPTH_TEST);							// Disable Depth Testing

			if (this.masking)										// Is Masking Enabled?
				Gl.glBlendFunc(Gl.GL_DST_COLOR, Gl.GL_ZERO);				// Blend Screen Color With Zero (Black)
			
			if (this.scene)											// Are We Drawing The Second Scene?
			{
				Gl.glTranslatef(0.0f, 0.0f, -1.0f);					// Translate Into The Screen One Unit
				Gl.glRotatef(this.roll*360, 0.0f, 0.0f, 1.0f);				// Rotate On The Z Axis 360 Degrees.
				if (this.masking)									// Is Masking On?
				{
					Gl.glBindTexture(Gl.GL_TEXTURE_2D, this.texture[3]);	// Select The Second Mask Texture
					Gl.glBegin(Gl.GL_QUADS);							// Start Drawing A Textured Quad
					Gl.glTexCoord2f(0.0f, 0.0f); Gl.glVertex3f(-1.1f, -1.1f,  0.0f);	// Bottom Left
					Gl.glTexCoord2f(1.0f, 0.0f); Gl.glVertex3f( 1.1f, -1.1f,  0.0f);	// Bottom Right
					Gl.glTexCoord2f(1.0f, 1.0f); Gl.glVertex3f( 1.1f,  1.1f,  0.0f);	// Top Right
					Gl.glTexCoord2f(0.0f, 1.0f); Gl.glVertex3f(-1.1f,  1.1f,  0.0f);	// Top Left
					Gl.glEnd();									// Done Drawing The Quad
				}

				Gl.glBlendFunc(Gl.GL_ONE, Gl.GL_ONE);					// Copy Image 2 Color To The Screen
				Gl.glBindTexture(Gl.GL_TEXTURE_2D, this.texture[4]);		// Select The Second Image Texture
				Gl.glBegin(Gl.GL_QUADS);								// Start Drawing A Textured Quad
				Gl.glTexCoord2f(0.0f, 0.0f); Gl.glVertex3f(-1.1f, -1.1f,  0.0f);	// Bottom Left
				Gl.glTexCoord2f(1.0f, 0.0f); Gl.glVertex3f( 1.1f, -1.1f,  0.0f);	// Bottom Right
				Gl.glTexCoord2f(1.0f, 1.0f); Gl.glVertex3f( 1.1f,  1.1f,  0.0f);	// Top Right
				Gl.glTexCoord2f(0.0f, 1.0f); Gl.glVertex3f(-1.1f,  1.1f,  0.0f);	// Top Left
				Gl.glEnd();										// Done Drawing The Quad
			}
			else												// Otherwise
			{
				if (this.masking)									// Is Masking On?
				{
					Gl.glBindTexture(Gl.GL_TEXTURE_2D, this.texture[1]);	// Select The First Mask Texture
					Gl.glBegin(Gl.GL_QUADS);							// Start Drawing A Textured Quad
					Gl.glTexCoord2f(this.roll+0.0f, 0.0f); Gl.glVertex3f(-1.1f, -1.1f,  0.0f);	// Bottom Left
					Gl.glTexCoord2f(this.roll+4.0f, 0.0f); Gl.glVertex3f( 1.1f, -1.1f,  0.0f);	// Bottom Right
					Gl.glTexCoord2f(this.roll+4.0f, 4.0f); Gl.glVertex3f( 1.1f,  1.1f,  0.0f);	// Top Right
					Gl.glTexCoord2f(this.roll+0.0f, 4.0f); Gl.glVertex3f(-1.1f,  1.1f,  0.0f);	// Top Left
					Gl.glEnd();									// Done Drawing The Quad
				}

				Gl.glBlendFunc(Gl.GL_ONE, Gl.GL_ONE);					// Copy Image 1 Color To The Screen
				Gl.glBindTexture(Gl.GL_TEXTURE_2D, this.texture[2]);		// Select The First Image Texture
				Gl.glBegin(Gl.GL_QUADS);								// Start Drawing A Textured Quad
				Gl.glTexCoord2f(roll+0.0f, 0.0f); Gl.glVertex3f(-1.1f, -1.1f,  0.0f);	// Bottom Left
				Gl.glTexCoord2f(roll+4.0f, 0.0f); Gl.glVertex3f( 1.1f, -1.1f,  0.0f);	// Bottom Right
				Gl.glTexCoord2f(roll+4.0f, 4.0f); Gl.glVertex3f( 1.1f,  1.1f,  0.0f);	// Top Right
				Gl.glTexCoord2f(roll+0.0f, 4.0f); Gl.glVertex3f(-1.1f,  1.1f,  0.0f);	// Top Left
				Gl.glEnd();										// Done Drawing The Quad
			}

			Gl.glEnable(Gl.GL_DEPTH_TEST);							// Enable Depth Testing
			Gl.glDisable(Gl.GL_BLEND);								// Disable Blending

			Video.GLSwapBuffers();

			this.roll += 0.002f;										// Increase Our Texture Roll Variable
			if (this.roll > 1.0f)										// Is Roll Greater Than One
				this.roll -= 1.0f;
		}

		private void Events_KeyboardDown(object sender, KeyboardEventArgs e)
		{
			switch(e.Key)
			{
				case Key.Space:
					this.scene = !this.scene;
					break;
				case Key.M:
					this.masking = !this.masking;
					break;
			}
		}
	}
}
