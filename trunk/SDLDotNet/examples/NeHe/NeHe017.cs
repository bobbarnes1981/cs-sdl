#region License
/*
MIT License
Copyright �2003-2005 Tao Framework Team
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
	class NeHe017 : NeHeBase
	{
		public int baseList = 0;		// Base Display List For The Font
		
		public float cnt1 = 0.0f;		// 1st Counter Used To Move Text & For Coloring
		public float cnt2 = 0.0f;		// 2nd Counter Used To Move Text & For Coloring

		public uint[] texture = new uint[2];	// Texture array

		public NeHe017()
		{
			Events.Quit += new QuitEventHandler(Events_Quit);
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
			Gl.glBlendFunc(Gl.GL_SRC_ALPHA, Gl.GL_ONE);						// Select The Type Of Blending
			Gl.glHint(Gl.GL_PERSPECTIVE_CORRECTION_HINT, Gl.GL_NICEST);		// Really Nice Perspective Calculations

			BuildFont();
		}


		public override void DrawGLScene()
		{
			Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
			Gl.glLoadIdentity();
			Gl.glBindTexture(Gl.GL_TEXTURE_2D, this.texture[1]);	// Select Our Second Texture
			Gl.glTranslatef(0.0f, 0.0f, -5.0f);						// Move Into The Screen 5 Units
			Gl.glRotatef(45.0f, 0.0f, 0.0f, 1.0f);					// Rotate On The Z Axis 45 Degrees (Clockwise)
			Gl.glRotatef(this.cnt1 * 30.0f, 1.0f, 1.0f, 0.0f);		// Rotate On The X & Y Axis By cnt1 (Left To Right)
			Gl.glDisable(Gl.GL_BLEND);								// Disable Blending Before We Draw In 3D
			Gl.glColor3f(1.0f, 1.0f, 1.0f);							// Bright White
			Gl.glBegin(Gl.GL_QUADS);								// Draw Our First Texture Mapped Quad
			Gl.glTexCoord2d(0.0f, 0.0f);						// First Texture Coord
			Gl.glVertex2f(-1.0f, 1.0f);							// First Vertex
			Gl.glTexCoord2d(1.0f, 0.0f);						// Second Texture Coord
			Gl.glVertex2f( 1.0f, 1.0f);							// Second Vertex
			Gl.glTexCoord2d(1.0f, 1.0f);						// Third Texture Coord
			Gl.glVertex2f( 1.0f, -1.0f);						// Third Vertex
			Gl.glTexCoord2d(0.0f, 1.0f);						// Fourth Texture Coord
			Gl.glVertex2f(-1.0f, -1.0f);						// Fourth Vertex
			Gl.glEnd();												// Done Drawing The First Quad
			Gl.glRotatef(90.0f, 1.0f, 1.0f, 0.0f);					// Rotate On The X & Y Axis By 90 Degrees (Left To Right)
			Gl.glBegin(Gl.GL_QUADS);									// Draw Our Second Texture Mapped Quad
			Gl.glTexCoord2d(0.0f, 0.0f);						// First Texture Coord
			Gl.glVertex2f(-1.0f, 1.0f);							// First Vertex
			Gl.glTexCoord2d(1.0f, 0.0f);						// Second Texture Coord
			Gl.glVertex2f( 1.0f, 1.0f);							// Second Vertex
			Gl.glTexCoord2d(1.0f, 1.0f);						// Third Texture Coord
			Gl.glVertex2f( 1.0f, -1.0f);						// Third Vertex
			Gl.glTexCoord2d(0.0f, 1.0f);						// Fourth Texture Coord
			Gl.glVertex2f(-1.0f, -1.0f);						// Fourth Vertex
			Gl.glEnd();												// Done Drawing Our Second Quad
			Gl.glEnable(Gl.GL_BLEND);									// Enable Blending

			Gl.glLoadIdentity();									// Reset The View
			// Pulsing Colors Based On Text Position
			Gl.glColor3f(1.0f*(float)Math.Cos(this.cnt1), 1.0f*(float)Math.Sin(this.cnt2), 1.0f-0.5f*(float)Math.Cos(this.cnt1+this.cnt2));
			glPrint((int)(280+250*Math.Cos(this.cnt1)), (int)(235+200*Math.Sin(this.cnt2)), "NeHe", 0);		// Pr(int) Gl.GL Text To The Screen

			Gl.glColor3f(1.0f*(float)(Math.Sin(this.cnt2)), 1.0f-0.5f*(float)(Math.Cos(this.cnt1+this.cnt2)), 1.0f*(float)(Math.Cos(this.cnt1)));
			glPrint((int)(280+230*Math.Cos(this.cnt2)), (int)(235+200*Math.Sin(this.cnt1)), "OpenGL", 0);	// Pr(int) Gl.GL Text To The Screen

			Gl.glColor3f(0.0f, 0.0f, 1.0f);							// Set Color To Blue
			glPrint((int)(240+200*Math.Cos((this.cnt2+this.cnt1)/5)), 2, "Giuseppe D'Agata", 0);

			Gl.glColor3f(1.0f, 1.0f, 1.0f);							// Set Color To White
			glPrint((int)(242+200*Math.Cos((this.cnt2+this.cnt1)/5)), 2, "Giuseppe D'Agata", 0);

			Video.GLSwapBuffers();

			this.cnt1 += 0.01f;										// Increase The First Counter
			this.cnt2 += 0.0081f;	
		}


		public void LoadTextures()
		{
			string[] file = {"NeHe017.Font.bmp", "NeHe017.Bumps.bmp"};
			Bitmap[] image = {null, null};

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

		public void BuildFont()
		{
			float cx;											// Holds Our X Character Coord
			float cy;											// Holds Our Y Character Coord

			this.baseList = Gl.glGenLists(256);					// Creating 256 Display Lists
			Gl.glBindTexture(Gl.GL_TEXTURE_2D, this.texture[0]);	// Select Our Font Texture
			for (int loop=0; loop < 256; loop++)				// Loop Through All 256 Lists
			{
				cx = (float)(loop % 16) / 16.0f;				// X Position Of Current Character
				cy = (float)(loop / 16) / 16.0f;				// Y Position Of Current Character

				Gl.glNewList((uint)(this.baseList+loop), Gl.GL_COMPILE);// Start Building A List
				Gl.glBegin(Gl.GL_QUADS);						// Use A Quad For Each Character
				Gl.glTexCoord2f(cx, 1 - cy - 0.0625f);			// Texture Coord (Bottom Left)
				Gl.glVertex2i(0, 0);							// Vertex Coord (Bottom Left)
				Gl.glTexCoord2f(cx + 0.0625f, 1 - cy - 0.0625f);// Texture Coord (Bottom Right)
				Gl.glVertex2i(16, 0);							// Vertex Coord (Bottom Right)
				Gl.glTexCoord2f(cx + 0.0625f, 1 - cy);			// Texture Coord (Top Right)
				Gl.glVertex2i(16, 16);							// Vertex Coord (Top Right)
				Gl.glTexCoord2f(cx, 1 - cy);					// Texture Coord (Top Left)
				Gl.glVertex2i(0, 16);							// Vertex Coord (Top Left)
				Gl.glEnd();										// Done Building Our Quad (Character)
				Gl.glTranslated(10, 0, 0);						// Move To The Right Of The Character
				Gl.glEndList();									// Done Building The Display List
			}
		}		

		public void glPrint(int x, int y, string str, int charSet)	// Where The Printing Happens
		{
			if (charSet > 1)
				charSet = 1;
			Gl.glBindTexture(Gl.GL_TEXTURE_2D, texture[0]);			// Select Our Font Texture
			Gl.glDisable(Gl.GL_DEPTH_TEST);							// Disables Depth Testing
			Gl.glMatrixMode(Gl.GL_PROJECTION);						// Select The Projection Matrix
			Gl.glPushMatrix();										// Store The Projection Matrix
			Gl.glLoadIdentity();									// Reset The Projection Matrix
			Gl.glOrtho(0, 640, 0, 480, -1, 1);						// Set Up An Ortho Screen
			Gl.glMatrixMode(Gl.GL_MODELVIEW);						// Select The Modelview Matrix
			Gl.glPushMatrix();										// Store The Modelview Matrix
			Gl.glLoadIdentity();									// Reset The Modelview Matrix
			Gl.glTranslated(x,y,0);									// Position The Text (0,0 - Bottom Left)
			Gl.glListBase(this.baseList - 32 + (128 * charSet));	// Choose The Font Set (0 or 1)
			Gl.glCallLists(str.Length, Gl.GL_UNSIGNED_BYTE, str);	// Write The Text To The Screen
			Gl.glMatrixMode(Gl.GL_PROJECTION);						// Select The Projection Matrix
			Gl.glPopMatrix();										// Restore The Old Projection Matrix
			Gl.glMatrixMode(Gl.GL_MODELVIEW);						// Select The Modelview Matrix
			Gl.glPopMatrix();										// Restore The Old Projection Matrix
			Gl.glEnable(Gl.GL_DEPTH_TEST);							// Enables Depth Testing
		}

		private void Events_Quit(object sender, QuitEventArgs e)
		{
			KillFont();
		}
		
		public void KillFont()
		{
			Gl.glDeleteLists(this.baseList, 256);				// Delete All 256 Display Lists
		}
	}
}