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
	class NeHe012 : NeHe001
	{
		public new static string Title
		{
			get
			{
				return "Lesson 12: Display Lists";
			}
		}
		uint box = 0;			// Storage For The Box Display List
		uint top = 0;			// Storage For The Top Display List
		
		float xrot = 0.0f;		// Rotates Cube On The X Axis
		float yrot = 0.0f;		// Rotates Cube On The Y Axis

		float[][] boxcol = new float[5][] { 
			new float[3] {1.0f, 0.0f, 0.0f}, 
			new float[3] {1.0f, 0.5f, 0.0f},
			new float[3] {1.0f, 1.0f, 0.0f},
			new float[3] {0.0f, 1.0f, 0.0f},
			new float[3] {0.0f, 1.0f, 1.0f} };
		float[][] topcol = new float[5][] { 
			new float[3] {0.5f, 0.0f, 0.0f},
			new float[3] {0.5f, 0.25f, 0.0f},
			new float[3] {0.5f, 0.5f, 0.0f},
			new float[3] {0.0f, 0.5f, 0.0f},
			new float[3] {0.0f, 0.5f, 0.5f} };

		uint[] texture = new uint[1];	

		KeyboardState keyData = new KeyboardState(false);

		public NeHe012()
		{
			Events.KeyboardDown += new KeyboardEventHandler(keyData.Update);
			Events.KeyboardUp += new KeyboardEventHandler(keyData.Update);
		}

		public override void InitGL()
		{
			LoadTextures();

			Gl.glEnable(Gl.GL_TEXTURE_2D);									// Enable Texture Mapping
			Gl.glShadeModel(Gl.GL_SMOOTH);									// Enable Smooth Shading
			Gl.glClearColor(0.0f, 0.0f, 0.0f, 0.5f);						// Black Background
			Gl.glClearDepth(1.0f);											// Depth Buffer Setup
			Gl.glEnable(Gl.GL_DEPTH_TEST);									// Enables Depth Testing
			Gl.glDepthFunc(Gl.GL_LEQUAL);									// The Type Of Depth Testing To Do
			Gl.glHint(Gl.GL_PERSPECTIVE_CORRECTION_HINT, Gl.GL_NICEST);		// Really Nice Perspective Calculations

			Gl.glEnable(Gl.GL_LIGHT0);										// Quick and dirty lighting
			Gl.glEnable(Gl.GL_LIGHTING);									// Enable lighting
			Gl.glEnable(Gl.GL_COLOR_MATERIAL);								// Enable material coloring

			BuildLists();
		}

		private bool LoadTextures()
		{
			// Load The Bitmap
			string file1 = "NeHe012.bmp";
			string file2 = "Data" + Path.DirectorySeparatorChar + file1;
			string file3 = ".." + Path.DirectorySeparatorChar + ".."  + Path.DirectorySeparatorChar + file2;
			string file = "";
			if(File.Exists(file1))
				file = file1;
			else if(File.Exists(file2))
				file = file2;
			else if(File.Exists(file3))
				file = file3;
			else
				throw new FileNotFoundException(file1);

			using(Bitmap image = new Bitmap(file))
			{
				image.RotateFlip(RotateFlipType.RotateNoneFlipY);
				System.Drawing.Imaging.BitmapData bitmapdata;
				Rectangle rect = new Rectangle(0, 0, image.Width, image.Height);

				bitmapdata = image.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

				Gl.glGenTextures(1, this.texture);
			
				// Create Linear Filtered Texture
				Gl.glBindTexture(Gl.GL_TEXTURE_2D, this.texture[0]);
				Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MAG_FILTER, Gl.GL_LINEAR);
				Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MIN_FILTER, Gl.GL_LINEAR);
				Gl.glTexImage2D(Gl.GL_TEXTURE_2D, 0, (int)Gl.GL_RGB, image.Width, image.Height, 0, Gl.GL_BGR_EXT, Gl.GL_UNSIGNED_BYTE, bitmapdata.Scan0);

				image.UnlockBits(bitmapdata);
			}
			return true;
		}


		public void BuildLists()
		{
			this.box = (uint)Gl.glGenLists(2);					// Generate 2 Different Lists
			Gl.glNewList(this.box, Gl.GL_COMPILE);				// Start With The Box List
			Gl.glBegin(Gl.GL_QUADS);
			// Bottom Face
			Gl.glNormal3f( 0.0f,-1.0f, 0.0f);
			Gl.glTexCoord2f(1.0f, 1.0f); Gl.glVertex3f(-1.0f, -1.0f, -1.0f);
			Gl.glTexCoord2f(0.0f, 1.0f); Gl.glVertex3f( 1.0f, -1.0f, -1.0f);
			Gl.glTexCoord2f(0.0f, 0.0f); Gl.glVertex3f( 1.0f, -1.0f,  1.0f);
			Gl.glTexCoord2f(1.0f, 0.0f); Gl.glVertex3f(-1.0f, -1.0f,  1.0f);
			// Front Face
			Gl.glNormal3f( 0.0f, 0.0f, 1.0f);
			Gl.glTexCoord2f(0.0f, 0.0f); Gl.glVertex3f(-1.0f, -1.0f,  1.0f);
			Gl.glTexCoord2f(1.0f, 0.0f); Gl.glVertex3f( 1.0f, -1.0f,  1.0f);
			Gl.glTexCoord2f(1.0f, 1.0f); Gl.glVertex3f( 1.0f,  1.0f,  1.0f);
			Gl.glTexCoord2f(0.0f, 1.0f); Gl.glVertex3f(-1.0f,  1.0f,  1.0f);
			// Back Face
			Gl.glNormal3f( 0.0f, 0.0f,-1.0f);
			Gl.glTexCoord2f(1.0f, 0.0f); Gl.glVertex3f(-1.0f, -1.0f, -1.0f);
			Gl.glTexCoord2f(1.0f, 1.0f); Gl.glVertex3f(-1.0f,  1.0f, -1.0f);
			Gl.glTexCoord2f(0.0f, 1.0f); Gl.glVertex3f( 1.0f,  1.0f, -1.0f);
			Gl.glTexCoord2f(0.0f, 0.0f); Gl.glVertex3f( 1.0f, -1.0f, -1.0f);
			// Right face
			Gl.glNormal3f( 1.0f, 0.0f, 0.0f);
			Gl.glTexCoord2f(1.0f, 0.0f); Gl.glVertex3f( 1.0f, -1.0f, -1.0f);
			Gl.glTexCoord2f(1.0f, 1.0f); Gl.glVertex3f( 1.0f,  1.0f, -1.0f);
			Gl.glTexCoord2f(0.0f, 1.0f); Gl.glVertex3f( 1.0f,  1.0f,  1.0f);
			Gl.glTexCoord2f(0.0f, 0.0f); Gl.glVertex3f( 1.0f, -1.0f,  1.0f);
			// Left Face
			Gl.glNormal3f(-1.0f, 0.0f, 0.0f);
			Gl.glTexCoord2f(0.0f, 0.0f); Gl.glVertex3f(-1.0f, -1.0f, -1.0f);
			Gl.glTexCoord2f(1.0f, 0.0f); Gl.glVertex3f(-1.0f, -1.0f,  1.0f);
			Gl.glTexCoord2f(1.0f, 1.0f); Gl.glVertex3f(-1.0f,  1.0f,  1.0f);
			Gl.glTexCoord2f(0.0f, 1.0f); Gl.glVertex3f(-1.0f,  1.0f, -1.0f);
			Gl.glEnd();
			Gl.glEndList();
			this.top = this.box + 1;							// Storage For "Top" Is "Box" Plus One
			Gl.glNewList(this.top, Gl.GL_COMPILE);				// Now The "Top" Display List
			Gl.glBegin(Gl.GL_QUADS);
			// Top Face
			Gl.glNormal3f( 0.0f, 1.0f, 0.0f);
			Gl.glTexCoord2f(0.0f, 1.0f); Gl.glVertex3f(-1.0f,  1.0f, -1.0f);
			Gl.glTexCoord2f(0.0f, 0.0f); Gl.glVertex3f(-1.0f,  1.0f,  1.0f);
			Gl.glTexCoord2f(1.0f, 0.0f); Gl.glVertex3f( 1.0f,  1.0f,  1.0f);
			Gl.glTexCoord2f(1.0f, 1.0f); Gl.glVertex3f( 1.0f,  1.0f, -1.0f);
			Gl.glEnd();
			Gl.glEndList();
		}

		public override void DrawGLScene()
		{
			// Update the rotation
			if(keyData[Key.UpArrow])
				this.xrot -= 4.2f;
			else if(keyData[Key.DownArrow])
				this.xrot += 4.2f;
			if(keyData[Key.RightArrow])
				this.yrot += 4.2f;
			else if(keyData[Key.LeftArrow])
				this.yrot -= 4.2f;
			
			// Draw the scene
			Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
			Gl.glBindTexture(Gl.GL_TEXTURE_2D, this.texture[0]);
			for (int yloop=1; yloop < 6; yloop++)
			{
				for (int xloop=0; xloop < yloop; xloop++)
				{
					Gl.glLoadIdentity();							// Reset The View
					Gl.glTranslatef(1.4f + ((float)xloop * 2.8f) - ((float)yloop * 1.4f), ((6.0f - (float)yloop) * 2.4f) - 7.0f, -20.0f);
					Gl.glRotatef(45.0f - (2.0f * yloop) + this.xrot, 1.0f, 0.0f, 0.0f);
					Gl.glRotatef(45.0f + this.yrot, 0.0f, 1.0f, 0.0f);
					Gl.glColor3fv(boxcol[yloop-1]);
					Gl.glCallList(this.box);
					Gl.glColor3fv(topcol[yloop-1]);
					Gl.glCallList(this.top);
				}
			}
			Video.GLSwapBuffers();
		}
	}
}
