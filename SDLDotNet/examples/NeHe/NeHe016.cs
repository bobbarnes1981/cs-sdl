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
	class NeHe016 : NeHe001
	{
		static NeHe016()
		{
			m_Title = "Lesson 16: Cool Looking Fog";
		}
		public bool	light = true;				// Lighting ON/OFF

		public float xrot = 0.0f;				// X Rotation
		public float yrot = 0.0f;				// Y Rotation
		public float xspeed = 0.0f;				// X Rotation Speed
		public float yspeed = 0.0f;				// Y Rotation Speed
		public float z = -5.0f;					// Depth Into The Screen

		public float[] LightAmbient = {0.5f, 0.5f, 0.5f, 1.0f};
		public float[] LightDiffuse = {1.0f, 1.0f, 1.0f, 1.0f};
		public float[] LightPosition = {0.0f, 0.0f, 2.0f, 1.0f};

		public int filter = 0;					// Which Filter To Use
		public uint[] texture = new uint[3];	// Texture array

		public uint[] fogMode = {Gl.GL_EXP, Gl.GL_EXP2, Gl.GL_LINEAR};	// Storage For Three Types Of Fog
		public int fogfilter = 0;										// Which Fog Mode To Use 
		public float[] fogColor = {0.5f, 0.5f, 0.5f, 1.0f};				// Fog Color

		public NeHe016()
		{
			Events.KeyboardDown += new KeyboardEventHandler(Events_KeyboardDown);
			Keyboard.EnableKeyRepeat(30,30);
		}

		public override void InitGL()
		{
			LoadTextures();

			Gl.glEnable(Gl.GL_TEXTURE_2D);									// Enable Texture Mapping
			Gl.glShadeModel(Gl.GL_SMOOTH);									// Enable Smooth Shading
			Gl.glClearColor(0.5f, 0.5f, 0.5f, 1.0f);						// Black Background
			Gl.glClearDepth(1.0f);											// Depth Buffer Setup
			Gl.glEnable(Gl.GL_DEPTH_TEST);									// Enables Depth Testing
			Gl.glDepthFunc(Gl.GL_LEQUAL);									// The Type Of Depth Testing To Do
			Gl.glHint(Gl.GL_PERSPECTIVE_CORRECTION_HINT, Gl.GL_NICEST);		// Really Nice Perspective Calculations

			Gl.glLightfv(Gl.GL_LIGHT1, Gl.GL_AMBIENT,  this.LightAmbient);	// Setup The Ambient Light
			Gl.glLightfv(Gl.GL_LIGHT1, Gl.GL_DIFFUSE,  this.LightDiffuse);	// Setup The Diffuse Light
			Gl.glLightfv(Gl.GL_LIGHT1, Gl.GL_POSITION, this.LightPosition);	// Position The Light
			Gl.glEnable(Gl.GL_LIGHT1);										// Enable Light One

			Gl.glFogi(Gl.GL_FOG_MODE, (int)this.fogMode[this.fogfilter]);	// Fog Mode
			Gl.glFogfv(Gl.GL_FOG_COLOR, this.fogColor);						// Set Fog Color
			Gl.glFogf(Gl.GL_FOG_DENSITY, 0.35f);							// How Dense Will The Fog Be
			Gl.glHint(Gl.GL_FOG_HINT, Gl.GL_DONT_CARE);						// Fog Hint Value
			Gl.glFogf(Gl.GL_FOG_START, 1.0f);								// Fog Start Depth
			Gl.glFogf(Gl.GL_FOG_END, 5.0f);									// Fog End Depth
			Gl.glEnable(Gl.GL_FOG);											// Enables GL_FOG
			
			if (this.light)													// If lighting, enable it to start
				Gl.glEnable(Gl.GL_LIGHTING);
		}

		private void LoadTextures()
		{
			// Load The Bitmap
			string file1 = "NeHe016.bmp";
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

				Gl.glGenTextures(3, this.texture);
			
				// Create Nearest Filtered Texture
				Gl.glBindTexture(Gl.GL_TEXTURE_2D, this.texture[0]);
				Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MAG_FILTER, Gl.GL_NEAREST);
				Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MIN_FILTER, Gl.GL_NEAREST);
				Gl.glTexImage2D(Gl.GL_TEXTURE_2D, 0, (int)Gl.GL_RGB, image.Width, image.Height, 0, Gl.GL_BGR_EXT, Gl.GL_UNSIGNED_BYTE, bitmapdata.Scan0);

				// Create Linear Filtered Texture
				Gl.glBindTexture(Gl.GL_TEXTURE_2D, this.texture[1]);
				Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MAG_FILTER, Gl.GL_LINEAR);
				Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MIN_FILTER, Gl.GL_LINEAR);
				Gl.glTexImage2D(Gl.GL_TEXTURE_2D, 0, (int)Gl.GL_RGB, image.Width, image.Height, 0, Gl.GL_BGR_EXT, Gl.GL_UNSIGNED_BYTE, bitmapdata.Scan0);

				// Create MipMapped Texture
				Gl.glBindTexture(Gl.GL_TEXTURE_2D, this.texture[2]);
				Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MAG_FILTER, Gl.GL_LINEAR);
				Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MIN_FILTER, Gl.GL_LINEAR_MIPMAP_NEAREST);
				Glu.gluBuild2DMipmaps(Gl.GL_TEXTURE_2D, (int)Gl.GL_RGB, image.Width, image.Height, Gl.GL_BGR_EXT, Gl.GL_UNSIGNED_BYTE, bitmapdata.Scan0);

				image.UnlockBits(bitmapdata);
			}
		}

		public override void DrawGLScene()
		{
			Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
			Gl.glLoadIdentity();
			Gl.glTranslatef(0.0f, 0.0f, this.z);

			Gl.glRotatef(this.xrot, 1.0f, 0.0f, 0.0f);
			Gl.glRotatef(this.yrot, 0.0f, 1.0f, 0.0f);

			Gl.glBindTexture(Gl.GL_TEXTURE_2D, this.texture[filter]);

			Gl.glBegin(Gl.GL_QUADS);
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
			// Top Face
			Gl.glNormal3f( 0.0f, 1.0f, 0.0f);
			Gl.glTexCoord2f(0.0f, 1.0f); Gl.glVertex3f(-1.0f,  1.0f, -1.0f);
			Gl.glTexCoord2f(0.0f, 0.0f); Gl.glVertex3f(-1.0f,  1.0f,  1.0f);
			Gl.glTexCoord2f(1.0f, 0.0f); Gl.glVertex3f( 1.0f,  1.0f,  1.0f);
			Gl.glTexCoord2f(1.0f, 1.0f); Gl.glVertex3f( 1.0f,  1.0f, -1.0f);
			// Bottom Face
			Gl.glNormal3f( 0.0f,-1.0f, 0.0f);
			Gl.glTexCoord2f(1.0f, 1.0f); Gl.glVertex3f(-1.0f, -1.0f, -1.0f);
			Gl.glTexCoord2f(0.0f, 1.0f); Gl.glVertex3f( 1.0f, -1.0f, -1.0f);
			Gl.glTexCoord2f(0.0f, 0.0f); Gl.glVertex3f( 1.0f, -1.0f,  1.0f);
			Gl.glTexCoord2f(1.0f, 0.0f); Gl.glVertex3f(-1.0f, -1.0f,  1.0f);
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
			Video.GLSwapBuffers();

			this.xrot += this.xspeed;
			this.yrot += this.yspeed;
		}

		private void Events_KeyboardDown(object sender, KeyboardEventArgs e)
		{
			switch(e.Key)
			{
				// L, F and G.
				case Key.L:
					this.light = !this.light;
					if (this.light)
						Gl.glEnable(Gl.GL_LIGHTING);
					else
						Gl.glDisable(Gl.GL_LIGHTING);
					break;
				case Key.F:
					this.filter = (filter + 1) % 3;
					break;
				case Key.G:
					this.fogfilter = (this.fogfilter + 1) % 3;
					Gl.glFogi(Gl.GL_FOG_MODE, (int)this.fogMode[this.fogfilter]);	
					break;

				// Zoom in cube with Page Up/Down
				case Key.PageUp:
					this.z -= 0.02f;
					break;
				case Key.PageDown:
					this.z += 0.02f;
					break;

				// Rotate cube with arrows.
				case Key.UpArrow:
					this.xspeed -= 0.1f;
					break;
				case Key.DownArrow:
					this.xspeed += 0.1f;
					break;
				case Key.LeftArrow:
					this.yspeed -= 0.1f;
					break;
				case Key.RightArrow:
					this.yspeed += 0.1f;
					break;
			}
		}
	}
}
