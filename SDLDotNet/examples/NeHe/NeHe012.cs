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
	/// <summary>
	/// 
	/// </summary>
	public class NeHe012 : NeHe006
	{
		/// <summary>
		/// 
		/// </summary>
		public new static string Title
		{
			get
			{
				return "Lesson 12: Display Lists";
			}
		}
		int box = 0;			// Storage For The Box Display List
		int top = 0;			// Storage For The Top Display List

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

		KeyboardState keyData = new KeyboardState(false);

		/// <summary>
		/// 
		/// </summary>
		public NeHe012()
		{
			this.Texture = new int[1];
			this.TextureName = "NeHe012.bmp";
			Events.KeyboardDown += new KeyboardEventHandler(this.KeyDown);
			Keyboard.EnableKeyRepeat(50,25);
		}

		/// <summary>
		/// 
		/// </summary>
		public override void InitGL()
		{
			LoadGLTextures();
			BuildLists();

			Gl.glEnable(Gl.GL_TEXTURE_2D);
			// Enable Texture Mapping
			Gl.glShadeModel(Gl.GL_SMOOTH);
			// Enable Smooth Shading
			Gl.glClearColor(0.0f, 0.0f, 0.0f, 0.5f);
			// Black Background
			Gl.glClearDepth(1.0f);
			// Depth Buffer Setup
			Gl.glEnable(Gl.GL_DEPTH_TEST);
			// Enables Depth Testing
			Gl.glDepthFunc(Gl.GL_LEQUAL);
			// The Type Of Depth Testing To Do
			Gl.glEnable(Gl.GL_LIGHT0);
			// Quick and dirty lighting
			Gl.glEnable(Gl.GL_LIGHTING);
			// Enable lighting
			Gl.glEnable(Gl.GL_COLOR_MATERIAL);
			// Enable material coloring
			Gl.glHint(Gl.GL_PERSPECTIVE_CORRECTION_HINT, Gl.GL_NICEST);
			// Really Nice Perspective Calculations
		}

		/// <summary>
		/// 
		/// </summary>
		public void BuildLists()
		{
			this.box = Gl.glGenLists(2);
			// Generate 2 Different Lists
			Gl.glNewList(this.box, Gl.GL_COMPILE);
			// Start With The Box List
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
			this.top = this.box + 1;
			// Storage For "Top" Is "Box" Plus One
			Gl.glNewList(this.top, Gl.GL_COMPILE);
			// Now The "Top" Display List
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

		/// <summary>
		/// 
		/// </summary>
		public override void DrawGLScene()
		{
			// Draw the scene
			Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
			Gl.glBindTexture(Gl.GL_TEXTURE_2D, this.Texture[0]);
			for (int yloop=1; yloop < 6; yloop++)
			{
				for (int xloop=0; xloop < yloop; xloop++)
				{
					Gl.glLoadIdentity();
					// Reset The View
					Gl.glTranslatef(1.4f + ((float)xloop * 2.8f) - ((float)yloop * 1.4f), ((6.0f - (float)yloop) * 2.4f) - 7.0f, -20.0f);
					Gl.glRotatef(45.0f - (2.0f * yloop) + this.XRot, 1.0f, 0.0f, 0.0f);
					Gl.glRotatef(45.0f + this.YRot, 0.0f, 1.0f, 0.0f);
					Gl.glColor3fv(boxcol[yloop-1]);
					Gl.glCallList(this.box);
					Gl.glColor3fv(topcol[yloop-1]);
					Gl.glCallList(this.top);
				}
			}
		}

		private void KeyDown(object sender, KeyboardEventArgs e)
		{
			switch (e.Key) 
			{
				case Key.UpArrow: 
					this.XRot -= 0.2f;
					break;
				case Key.DownArrow:
					this.XRot += 0.2f;
					break;
				case Key.RightArrow:
					this.YRot += 0.2f;
					break;
				case Key.LeftArrow:
					this.YRot -= 0.2f;
					break;
			}
		}
	}
}
