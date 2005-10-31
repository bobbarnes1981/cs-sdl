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
using System.Collections;

using SdlDotNet;
using Tao.OpenGl;

namespace SdlDotNet.Examples
{
	class NeHe019 : NeHeBase
	{
		private static string title = "Lesson 19: Particle Engine Using Triangle Strips";
		public static string Title
		{
			get
			{
				return title;
			}
		}
		readonly static int MaxParticles = 1000;	// Number of particles to create
		bool rainbow = true;			// Rainbow Mode?

		float slowdown = 2.0f;			// Slow Down Particles
		float xspeed = 0.0f;			// Base X Speed (To Allow Keyboard Direction Of Tail)
		float yspeed = 0.0f;			// Base Y Speed (To Allow Keyboard Direction Of Tail)
		float zoom = -40f;			// Used To Zoom Out

		uint col = 0;					// Current Color Selection
		uint delay = 0;					// Rainbow Effect Delay
		Random rand = new Random();		// Random number generator
		
		uint[] texture = new uint[3];	// Texture array

		public class Particle					// Create A Structure For Particle
		{
			public bool	active;					// Active (Yes/No)
			public float life;					// Particle Life
			public float fade;					// Fade Speed
			public float r, g, b;				// Color
			public float x, y, z;				// Position
			public float xi, yi, zi;			// Direction
			public float xg, yg, zg;			// Gravity
		}

		Particle[] particle = new Particle[MaxParticles];

		float[][] colors = new float[12][] {	// Rainbow Of Colors
			new float[3] {1.0f,0.5f,0.5f}, new float[3] {1.0f,0.75f,0.5f}, 
			new float[3] {1.0f,1.0f,0.5f}, new float[3] {0.75f,1.0f,0.5f},
			new float[3] {0.5f,1.0f,0.5f}, new float[3] {0.5f,1.0f,0.75f}, 
			new float[3] {0.5f,1.0f,1.0f}, new float[3] {0.5f,0.75f,1.0f},
			new float[3] {0.5f,0.5f,1.0f}, new float[3] {0.75f,0.5f,1.0f}, 
			new float[3] {1.0f,0.5f,1.0f}, new float[3] {1.0f,0.5f,0.75f} };

		public NeHe019()
		{
			Events.KeyboardDown+=new KeyboardEventHandler(Events_KeyboardDown);
			Keyboard.EnableKeyRepeat(50,10);
		}

		public override void InitGL()
		{
			LoadTextures();

			Gl.glEnable(Gl.GL_TEXTURE_2D);									// Enable Texture Mapping
			Gl.glShadeModel(Gl.GL_SMOOTH);									// Enable Smooth Shading
			Gl.glClearColor(0.0f, 0.0f, 0.0f, 0.5f);						// Black Background
			Gl.glClearDepth(1.0f);											// Depth Buffer Setup
			Gl.glDisable(Gl.GL_DEPTH_TEST);									// Enables Depth Testing
			Gl.glEnable(Gl.GL_BLEND);										// Enable Blending
			Gl.glBlendFunc(Gl.GL_SRC_ALPHA, Gl.GL_ONE);						// Type Of Blending To Perform
			Gl.glHint(Gl.GL_PERSPECTIVE_CORRECTION_HINT, Gl.GL_NICEST);		// Really Nice Perspective Calculations

			Gl.glHint(Gl.GL_POINT_SMOOTH_HINT, Gl.GL_NICEST);				// Really Nice Point Smoothing

			Gl.glBindTexture(Gl.GL_TEXTURE_2D, this.texture[0]);			// Select Our Texture

			for (int loop=0; loop < particle.Length; loop++)		// Initials All The Textures
			{
				particle[loop] = new Particle();
				particle[loop].active = true;								// Make All The Particles Active
				particle[loop].life = 1.0f;								// Give All The this.particles Full Life
				particle[loop].fade = (float)(this.rand.Next(100))/1000.0f+0.003f;	// Random Fade Speed
				particle[loop].r = colors[loop*(12/MaxParticles)][0];	// Select Red Rainbow Color
				particle[loop].g = colors[loop*(12/MaxParticles)][1];	// Select Red Rainbow Color
				particle[loop].b = colors[loop*(12/MaxParticles)][2];	// Select Red Rainbow Color
				particle[loop].xi = (float)((this.rand.Next(50))-26.0f)*10.0f;	// Random Speed On X Axis
				particle[loop].yi = (float)((this.rand.Next(50))-25.0f)*10.0f;	// Random Speed On Y Axis
				particle[loop].zi = (float)((this.rand.Next(50))-25.0f)*10.0f;	// Random Speed On Z Axis
				particle[loop].xg = 0.0f;									// Set Horizontal Pull To Zero
				particle[loop].yg = -0.8f;									// Set Vertical Pull Downward
				particle[loop].zg = 0.0f;									// Set Pull On Z Axis To Zero
			}
		}

		public override void DrawGLScene()
		{
			Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
			Gl.glLoadIdentity();
			
			for (int loop=0; loop < particle.Length; loop++)	// Loop Through All The Particles
			{
				if (this.particle[loop].active)							// If The Particle Is Active
				{
					float x = this.particle[loop].x;					// Grab Our Particle X Position
					float y = this.particle[loop].y;					// Grab Our Particle Y Position
					float z = this.particle[loop].z+zoom;				// Particle Z Pos + Zoom

					// Draw The Particle Using Our RGB Values, Fade The Particle Based On It's Life
					Gl.glColor4f(this.particle[loop].r, this.particle[loop].g, this.particle[loop].b, this.particle[loop].life);

					Gl.glBegin(Gl.GL_TRIANGLE_STRIP);						// Build Quad From A TrianGL.gle Strip
					Gl.glTexCoord2d(1, 1); Gl.glVertex3f(x+0.5f, y+0.5f, z); // Top Right
					Gl.glTexCoord2d(0, 1); Gl.glVertex3f(x-0.5f, y+0.5f, z); // Top Left
					Gl.glTexCoord2d(1, 0); Gl.glVertex3f(x+0.5f, y-0.5f, z); // Bottom Right
					Gl.glTexCoord2d(0, 0); Gl.glVertex3f(x-0.5f, y-0.5f, z); // Bottom Left
					Gl.glEnd();										// Done Building TrianGL.gle Strip

					this.particle[loop].x += this.particle[loop].xi/(slowdown*1000);// Move On The X Axis By X Speed
					this.particle[loop].y += this.particle[loop].yi/(slowdown*1000);// Move On The Y Axis By Y Speed
					this.particle[loop].z += this.particle[loop].zi/(slowdown*1000);// Move On The Z Axis By Z Speed

					this.particle[loop].xi += this.particle[loop].xg;			// Take Pull On X Axis Into Account
					this.particle[loop].yi += this.particle[loop].yg;			// Take Pull On Y Axis Into Account
					this.particle[loop].zi += this.particle[loop].zg;			// Take Pull On Z Axis Into Account
					this.particle[loop].life -= this.particle[loop].fade;		// Reduce Particles Life By 'Fade'

					if (this.particle[loop].life < 0.0f)					// If Particle Is Burned Out
					{
						this.particle[loop].life = 1.0f;					// Give It New Life
						this.particle[loop].fade = (float)(this.rand.Next(100))/1000.0f+0.003f;	// Random Fade Value
						this.particle[loop].x = 0.0f;						// Center On X Axis
						this.particle[loop].y = 0.0f;						// Center On Y Axis
						this.particle[loop].z = 0.0f;						// Center On Z Axis
						this.particle[loop].xi = this.xspeed+(float)(this.rand.Next(60)-32.0f);	// X Axis Speed And Direction
						this.particle[loop].yi = this.yspeed+(float)(this.rand.Next(60)-30.0f);	// Y Axis Speed And Direction
						this.particle[loop].zi = (float)(this.rand.Next(60)-30.0f);	// Z Axis Speed And Direction
						this.particle[loop].r = colors[this.col][0];			// Select Red From Color Table
						this.particle[loop].g = colors[this.col][1];			// Select Green From Color Table
						this.particle[loop].b = colors[this.col][2];			// Select Blue From Color Table
					}
				}
			}

			Video.GLSwapBuffers();

			if (this.rainbow && (this.delay > 25))
			{
				this.delay = 0;						// Reset The Rainbow Color Cycling Delay
				this.col = (this.col + 1) % 12;		// Change The Particle Color
			}

			delay++;
	
		}


		

		public void LoadTextures()
		{
			string file1 = "NeHe019.bmp";
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

		private void Events_KeyboardDown(object sender, KeyboardEventArgs e)
		{
			switch(e.Key)
			{
				case Key.Equals:
					if(this.slowdown > 1.0f)
						this.slowdown -= 0.01f;
					break;
				case Key.Plus:
					if(this.slowdown < 4.0f)
						this.slowdown+=0.01f;
					break;

				case Key.PageUp:
					this.zoom -= 0.1f;
					break;
				case Key.PageDown:
					this.zoom += 0.1f;
					break;

				case Key.Return:
					this.rainbow = !this.rainbow;
					break;
				case Key.Space:
					this.delay = 0;						// Reset The Rainbow Color Cycling Delay
					this.col = (this.col + 1) % 12;		// Change The Particle Color
					break;

				case Key.W:
					foreach(Particle p in particle)
						if(p.yg < 1.5f)
							p.yg += 0.01f;
					break;
				case Key.S:
					foreach(Particle p in particle)
						if(p.yg > -1.5f)
							p.yg -= 0.01f;
					break;
				case Key.D:
					foreach(Particle p in particle)
						if(p.xg < 1.5f)
							p.xg += 0.01f;
					break;
				case Key.A:
					foreach(Particle p in particle)
						if(p.xg > -1.5f)
							p.xg -= 0.01f;
					break;
				case Key.Tab:
					foreach(Particle p in particle)
					{
						p.x = 0.0f;								// Center On X Axis
						p.y = 0.0f;								// Center On Y Axis
						p.z = 0.0f;								// Center On Z Axis
						p.xi = (float)(this.rand.Next(50)-26.0f)*10.0f;	// Random Speed On X Axis
						p.yi = (float)(this.rand.Next(50)-25.0f)*10.0f;	// Random Speed On Y Axis
						p.zi = (float)(this.rand.Next(50)-25.0f)*10.0f;	// Random Speed On Z Axis
					}
					break;

				case Key.UpArrow:
					this.yspeed += 1.0f;
					break;
				case Key.DownArrow:
					this.yspeed -= 1.0f;
					break;
				case Key.RightArrow:
					this.xspeed += 1.0f;
					break;
				case Key.LeftArrow:
					this.xspeed -= 1.0f;
					break;

			}
		}
	}
}
