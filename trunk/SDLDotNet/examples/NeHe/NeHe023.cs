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

using SdlDotNet;
using Tao.OpenGl;

namespace SdlDotNet.Examples
{
	class NeHe023 : NeHe001
	{
		public new static string Title
		{
			get
			{
				return "Lesson 23: Sphere Mapping, Multi-Texturing and Extensions";
			}
		}
		bool light;                                              // Lighting ON/OFF
		float xrot;                                              // X Rotation
		float yrot;                                              // Y Rotation
		float xspeed;                                            // X Rotation Speed
		float yspeed;                                            // Y Rotation Speed
		float z = -10;                                           // Depth Into The Screen
		Glu.GLUquadric quadratic;                                // Storage For Our Quadratic Objects
		float[] LightAmbient = {0.5f, 0.5f, 0.5f, 1};
		float[] LightDiffuse = {1, 1, 1, 1};
		float[] LightPosition = {0, 0, 2, 1};
		int filter;                                              // Which Filter To Use
		int[] texture = new int[6];                              // Storage For 6 Textures (MODIFIED)
		int objectToDraw = 1;                                    // Which Object To Draw
		
		public NeHe023()
		{
			Events.KeyboardDown += new KeyboardEventHandler(Events_KeyboardDown);
			Keyboard.EnableKeyRepeat(150,50);
		}

		private void Events_KeyboardDown(object sender, KeyboardEventArgs e)
		{
			switch(e.Key)
			{
				case Key.L:
					light = !light;
					if(light)
						Gl.glDisable(Gl.GL_LIGHTING);
					else 
						Gl.glEnable(Gl.GL_LIGHTING);
					break;
				case Key.F:
					filter += 1;
					if(filter > 2) 
						filter = 0;
					break;
				case Key.Space:
					if(++objectToDraw > 3) 
						objectToDraw = 0;
					break;

				case Key.PageUp:
					z -= 0.02f;
					break;
				case Key.PageDown:
					z += 0.02f;
					break;

				case Key.UpArrow:
					xspeed -= 0.01f;
					break;
				case Key.DownArrow:
					xspeed += 0.01f;
					break;
				case Key.RightArrow:
					yspeed += 0.01f;
					break;
				case Key.LeftArrow: 
					yspeed -= 0.01f;
					break;
			}
		}

		public override void InitGL()
		{
			LoadTextures();

			Gl.glEnable(Gl.GL_TEXTURE_2D);                                      // Enable Texture Mapping
			Gl.glShadeModel(Gl.GL_SMOOTH);                                      // Enable Smooth Shading
			Gl.glClearColor(0, 0, 0, 0.5f);                                     // Black Background
			Gl.glClearDepth(1);                                                 // Depth Buffer Setup
			Gl.glEnable(Gl.GL_DEPTH_TEST);                                      // Enables Depth Testing
			Gl.glDepthFunc(Gl.GL_LEQUAL);                                       // The Type Of Depth Testing To Do
			Gl.glHint(Gl.GL_PERSPECTIVE_CORRECTION_HINT, Gl.GL_NICEST);         // Really Nice Perspective Calculations

			Gl.glLightfv(Gl.GL_LIGHT1, Gl.GL_AMBIENT, LightAmbient);            // Setup The Ambient Light
			Gl.glLightfv(Gl.GL_LIGHT1, Gl.GL_DIFFUSE, LightDiffuse);            // Setup The Diffuse Light
			Gl.glLightfv(Gl.GL_LIGHT1, Gl.GL_POSITION,LightPosition);           // Position The Light
			Gl.glEnable(Gl.GL_LIGHT1);                                          // Enable Light One

			quadratic = Glu.gluNewQuadric();                                    // Create A Pointer To The Quadric Object (Return 0 If No Memory)
			Glu.gluQuadricNormals(quadratic, Glu.GLU_SMOOTH);                   // Create Smooth Normals 
			Glu.gluQuadricTexture(quadratic, Gl.GL_TRUE);                       // Create Texture Coords 

			Gl.glTexGeni(Gl.GL_S, Gl.GL_TEXTURE_GEN_MODE, Gl.GL_SPHERE_MAP);    // Set The Texture Generation Mode For S To Sphere Mapping (NEW)
			Gl.glTexGeni(Gl.GL_T, Gl.GL_TEXTURE_GEN_MODE, Gl.GL_SPHERE_MAP);    // Set The Texture Generation Mode For T To Sphere Mapping (NEW)
		}

		public void LoadTextures()
		{
			string[] file = {"NeHe023.BG.bmp", "NeHe023.Reflect.bmp"};
			Bitmap[] textureImage = new Bitmap[2];

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
				textureImage[i] = new Bitmap(finalFile);
			}

			Gl.glGenTextures(6, texture);                                   // Create Three Textures
			for(int i = 0; i < textureImage.Length; i++) 
			{
				// Flip The Bitmap Along The Y-Axis
				textureImage[i].RotateFlip(RotateFlipType.RotateNoneFlipY);
				// Rectangle For Locking The Bitmap In Memory
				Rectangle rectangle = new Rectangle(0, 0, textureImage[i].Width, textureImage[i].Height);
				// Get The Bitmap's Pixel Data From The Locked Bitmap
				BitmapData bitmapData = textureImage[i].LockBits(rectangle, ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);

				// Create Nearest Filtered Texture
				Gl.glBindTexture(Gl.GL_TEXTURE_2D, texture[i]);          // Gen Tex 0 and 1
				Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MAG_FILTER, Gl.GL_NEAREST);
				Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MIN_FILTER, Gl.GL_NEAREST);
				Gl.glTexImage2D(Gl.GL_TEXTURE_2D, 0, Gl.GL_RGB8, textureImage[i].Width, textureImage[i].Height, 0, Gl.GL_BGR, Gl.GL_UNSIGNED_BYTE, bitmapData.Scan0);

				// Create Linear Filtered Texture
				Gl.glBindTexture(Gl.GL_TEXTURE_2D, texture[i+2]);        // Gen Tex 2 and 3
				Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MAG_FILTER, Gl.GL_LINEAR);
				Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MIN_FILTER, Gl.GL_LINEAR);
				Gl.glTexImage2D(Gl.GL_TEXTURE_2D, 0, Gl.GL_RGB8, textureImage[i].Width, textureImage[i].Height, 0, Gl.GL_BGR, Gl.GL_UNSIGNED_BYTE, bitmapData.Scan0);

				// Create MipMapped Texture
				Gl.glBindTexture(Gl.GL_TEXTURE_2D, texture[i+4]);        // Gen Tex 4 and 5
				Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MAG_FILTER, Gl.GL_LINEAR);
				Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MIN_FILTER, Gl.GL_LINEAR_MIPMAP_NEAREST);
				Glu.gluBuild2DMipmaps(Gl.GL_TEXTURE_2D, Gl.GL_RGB8, textureImage[i].Width, textureImage[i].Height, Gl.GL_BGR, Gl.GL_UNSIGNED_BYTE, bitmapData.Scan0);

				if(textureImage[i] != null) 
				{                            // If Texture Exists
					textureImage[i].UnlockBits(bitmapData);              // Unlock The Pixel Data From Memory
					textureImage[i].Dispose();                           // Dispose The Bitmap
				}
			}
		}

		public override void DrawGLScene()
		{
			Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);        // Clear The Screen And The Depth Buffer
			Gl.glLoadIdentity();                                                // Reset The View

			Gl.glTranslatef(0, 0, z);

			Gl.glEnable(Gl.GL_TEXTURE_GEN_S);                                   // Enable Texture Coord Generation For S (NEW)
			Gl.glEnable(Gl.GL_TEXTURE_GEN_T);                                   // Enable Texture Coord Generation For T (NEW)

			Gl.glBindTexture(Gl.GL_TEXTURE_2D, texture[filter + (filter + 1)]); // This Will Select The Sphere Map
			Gl.glPushMatrix();
			Gl.glRotatef(xrot, 1, 0, 0);
			Gl.glRotatef(yrot, 0, 1, 0);
			switch(objectToDraw) 
			{
				case 0:
					GlDrawCube();
					break;
				case 1:
					Gl.glTranslatef(0, 0, -1.5f);                           // Center The Cylinder
					Glu.gluCylinder(quadratic, 1, 1, 3, 32, 32);            // A Cylinder With A Radius Of 0.5 And A Height Of 2
					break;
				case 2:
					Glu.gluSphere(quadratic, 1.3, 32, 32);                  // Draw A Sphere With A Radius Of 1 And 16 Longitude And 16 Latitude Segments
					break;
				case 3:
					Gl.glTranslatef(0, 0, -1.5f);                           // Center The Cone
					Glu.gluCylinder(quadratic, 1, 0, 3, 32, 32);            // A Cone With A Bottom Radius Of .5 And A Height Of 2
					break;
			};
			Gl.glPopMatrix();
			Gl.glDisable(Gl.GL_TEXTURE_GEN_S);
			Gl.glDisable(Gl.GL_TEXTURE_GEN_T);

			Gl.glBindTexture(Gl.GL_TEXTURE_2D, texture[filter * 2]);            // This Will Select The BG Maps...
			Gl.glPushMatrix();
			Gl.glTranslatef(0, 0, -24);
			Gl.glBegin(Gl.GL_QUADS);
			Gl.glNormal3f(0, 0, 1);
			Gl.glTexCoord2f(0, 0); Gl.glVertex3f(-13.3f, -10, 10);
			Gl.glTexCoord2f(1, 0); Gl.glVertex3f( 13.3f, -10, 10);
			Gl.glTexCoord2f(1, 1); Gl.glVertex3f( 13.3f, 10, 10);
			Gl.glTexCoord2f(0, 1); Gl.glVertex3f(-13.3f, 10, 10);
			Gl.glEnd();
			Gl.glPopMatrix();

			Video.GLSwapBuffers();

			xrot += xspeed;
			yrot += yspeed;
		}

		public void GlDrawCube()
		{
			Gl.glBegin(Gl.GL_QUADS);
			// Front Face
			Gl.glNormal3f(0, 0, 0.5f);
			Gl.glTexCoord2f(0, 0); Gl.glVertex3f(-1, -1, 1);
			Gl.glTexCoord2f(1, 0); Gl.glVertex3f(1, -1, 1);
			Gl.glTexCoord2f(1, 1); Gl.glVertex3f(1, 1, 1);
			Gl.glTexCoord2f(0, 1); Gl.glVertex3f(-1, 1, 1);
			// Back Face
			Gl.glNormal3f(0, 0,-0.5f);
			Gl.glTexCoord2f(1, 0); Gl.glVertex3f(-1, -1, -1);
			Gl.glTexCoord2f(1, 1); Gl.glVertex3f(-1, 1, -1);
			Gl.glTexCoord2f(0, 1); Gl.glVertex3f(1, 1, -1);
			Gl.glTexCoord2f(0, 0); Gl.glVertex3f(1, -1, -1);
			// Top Face
			Gl.glNormal3f(0, 0.5f, 0);
			Gl.glTexCoord2f(0, 1); Gl.glVertex3f(-1, 1, -1);
			Gl.glTexCoord2f(0, 0); Gl.glVertex3f(-1, 1, 1);
			Gl.glTexCoord2f(1, 0); Gl.glVertex3f( 1, 1, 1);
			Gl.glTexCoord2f(1, 1); Gl.glVertex3f( 1, 1, -1);
			// Bottom Face
			Gl.glNormal3f(0,-0.5f, 0);
			Gl.glTexCoord2f(1, 1); Gl.glVertex3f(-1, -1, -1);
			Gl.glTexCoord2f(0, 1); Gl.glVertex3f(1, -1, -1);
			Gl.glTexCoord2f(0, 0); Gl.glVertex3f(1, -1, 1);
			Gl.glTexCoord2f(1, 0); Gl.glVertex3f(-1, -1, 1);
			// Right Face
			Gl.glNormal3f(0.5f, 0, 0);
			Gl.glTexCoord2f(1, 0); Gl.glVertex3f(1, -1, -1);
			Gl.glTexCoord2f(1, 1); Gl.glVertex3f(1, 1, -1);
			Gl.glTexCoord2f(0, 1); Gl.glVertex3f(1, 1, 1);
			Gl.glTexCoord2f(0, 0); Gl.glVertex3f(1, -1, 1);
			// Left Face
			Gl.glNormal3f(-0.5f, 0, 0);
			Gl.glTexCoord2f(0, 0); Gl.glVertex3f(-1, -1, -1);
			Gl.glTexCoord2f(1, 0); Gl.glVertex3f(-1, -1, 1);
			Gl.glTexCoord2f(1, 1); Gl.glVertex3f(-1, 1, 1);
			Gl.glTexCoord2f(0, 1); Gl.glVertex3f(-1, 1, -1);
			Gl.glEnd();

		}
	}
}
