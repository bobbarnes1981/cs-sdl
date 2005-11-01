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
	/// <summary>
	/// 
	/// </summary>
	public class NeHe018 : NeHe016
	{
		/// <summary>
		/// 
		/// </summary>
		public new static string Title
		{
			get
			{
				return "Lesson 18: Quadrics";
			}
		}
		bool light = true;				
		// Lighting ON/OFF

		int part1 = 0;					
		// Start Of Disc ( NEW )
		int part2 = 0;					
		// End Of Disc ( NEW )
		int p1 = 0;						
		// Increase 1 ( NEW )
		int p2 = 1;						
		// Increase 2 ( NEW )

		Glu.GLUquadric quadratic;
		// Storage For Our Quadratic Objects 

		// Lighting components for the cube
		float[] LightAmbient =  {0.5f, 0.5f, 0.5f, 1.0f};
		float[] LightDiffuse =  {1.0f, 1.0f, 1.0f, 1.0f};
		float[] LightPosition = {0.0f, 0.0f, 2.0f, 1.0f};

		int obj = 0;
		// Which Object To Draw

		/// <summary>
		/// 
		/// </summary>
		public NeHe018()
		{
			Events.KeyboardDown += new KeyboardEventHandler(Events_KeyboardDown);
			Keyboard.EnableKeyRepeat(60,60);
			Events.Quit += new QuitEventHandler(Events_Quit);
			this.Texture = new int[3];
			this.TextureName = "NeHe018.bmp";
			this.Z = -5.0f;
		}

		/// <summary>
		/// 
		/// </summary>
		public override void InitGL()
		{
			LoadGLTextures();

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
			Gl.glHint(Gl.GL_PERSPECTIVE_CORRECTION_HINT, Gl.GL_NICEST);
			// Really Nice Perspective Calculations

			Gl.glLightfv(Gl.GL_LIGHT1, Gl.GL_AMBIENT,  this.LightAmbient);
			// Setup The Ambient Light
			Gl.glLightfv(Gl.GL_LIGHT1, Gl.GL_DIFFUSE,  this.LightDiffuse);
			// Setup The Diffuse Light
			Gl.glLightfv(Gl.GL_LIGHT1, Gl.GL_POSITION, this.LightPosition);
			// Position The Light
			Gl.glEnable(Gl.GL_LIGHT1);
			// Enable Light One

			this.quadratic = Glu.gluNewQuadric();
			// Create A Pointer To The Quadric Object (Return 0 If No Memory) (NEW)
			Glu.gluQuadricNormals(this.quadratic, Glu.GLU_SMOOTH);
			// Create Smooth Normals (NEW)
			Glu.gluQuadricTexture(this.quadratic, (byte)Gl.GL_TRUE);
			// Create Texture Coords (NEW)

			if (this.light)	
			{
				// If lighting, enable it to start
				Gl.glEnable(Gl.GL_LIGHTING);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public override void DrawGLScene()
		{
			Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
			Gl.glLoadIdentity();
			Gl.glTranslatef(0.0f, 0.0f, this.Z);

			Gl.glRotatef(this.XRot, 1.0f, 0.0f, 0.0f);
			Gl.glRotatef(this.YRot, 0.0f, 1.0f, 0.0f);

			Gl.glBindTexture(Gl.GL_TEXTURE_2D, this.Texture[this.Filter]);

			switch (this.obj)
			{
				case 0:
					GlDrawCube();
					break;
				case 1:
					Gl.glTranslatef(0.0f, 0.0f, -1.5f);
					// Center The Cylinder
					Glu.gluCylinder(this.quadratic, 1.0f, 1.0f, 3.0f, 32, 32);
					// A Cylinder With A Radius Of 0.5 And A Height Of 2
					break;
				case 2:
					Glu.gluDisk(this.quadratic, 0.5f, 1.5f, 32, 32);
					// Draw A Disc (CD Shape) With An Inner Radius Of 0.5, 
					// And An Outer Radius Of 2.  Plus A Lot Of Segments ;)
					break;
				case 3:
					Glu.gluSphere(this.quadratic, 1.3f, 32, 32);
					// Draw A Sphere With A Radius Of 1 And 16 Longitude And 16 Latitude Segments
					break;
				case 4:
					Gl.glTranslatef(0.0f, 0.0f, -1.5f);
					// Center The Cone
					Glu.gluCylinder(this.quadratic, 1.0f, 0.0f, 3.0f, 32, 32);
					// A Cone With A Bottom Radius Of .5 And A Height Of 2
					break;
				case 5:
					this.part1 += this.p1;
					this.part2 += this.p2;

					if (this.part1 > 359)
						// 360 Degrees
					{
						this.p1 = 0;
						this.p2 = 1;
						this.part1 = this.part2 = 0;
					}
					if (this.part2 > 359)
						// 360 Degrees
					{
						this.p1 = 1;
						this.p2 = 0;
					}
					Glu.gluPartialDisk(this.quadratic, 0.5f, 1.5f, 32, 32, this.part1, this.part2 - this.part1);
					// A Disk Like The One Before
					break;
			};

			this.XRot += this.XSpeed;
			this.YRot += this.YSpeed;

		}

		/// <summary>
		/// 
		/// </summary>
		public void GlDrawCube()
		{
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
			// Right Face
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
		}

		private void Events_KeyboardDown(object sender, KeyboardEventArgs e)
		{
			switch(e.Key)
			{
				case Key.L:
					this.light = !this.light;
					if (this.light)
					{
						Gl.glEnable(Gl.GL_LIGHTING);
					}
					else
					{
						Gl.glDisable(Gl.GL_LIGHTING);
					}
					break;
				case Key.F:
					this.Filter = (this.Filter + 1) % 3;
					break;
				case Key.Space:
					this.obj = (this.obj + 1) % 6;
					break;
				case Key.PageUp:
					this.Z -= 0.02f;
					break;
				case Key.PageDown:
					this.Z += 0.02f;
					break;
				case Key.UpArrow:
					this.XSpeed -= 0.1f;
					break;
				case Key.DownArrow:
					this.XSpeed += 0.1f;
					break;
				case Key.LeftArrow:
					this.YSpeed -= 0.1f;
					break;
				case Key.RightArrow:
					this.YSpeed += 0.1f;
					break;
			}
		}

		private void Events_Quit(object sender, QuitEventArgs e)
		{
			Glu.gluDeleteQuadric(this.quadratic);
			// Delete The Quadratic To Free System Resources
		}
	}
}
