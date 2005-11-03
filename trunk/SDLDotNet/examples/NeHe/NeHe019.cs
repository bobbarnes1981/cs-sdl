#region License
/*
MIT License
Copyright 2003-2005 Tao Framework Team
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
	/// <summary>
	/// 
	/// </summary>
	public class NeHe019 : NeHe018
	{
		/// <summary>
		/// 
		/// </summary>
		public new static string Title
		{
			get
			{
				return "Lesson 19: Particle Engine Using Triangle Strips";
			}
		}
		// Number of particles to create
		readonly static int MaxParticles = 1000;
	
		// Rainbow Mode?
		bool rainbow = true;			
		
		// Slow Down Particles
		float slowdown = 2.0f;
		
		// Used To Zoom Out
		float zoom = -40f;
			
		// Current Color Selection
		int col = 0;					
		// Rainbow Effect Delay
		int delay = 0;					
		// Random number generator
		Random rand = new Random();		
		
		/// <summary>
		/// Create A Structure For Particle
		/// </summary>
		public class Particle					
		{
			bool active;
			
			/// <summary>
			/// Active (Yes/No)
			/// </summary>
			public bool Active
			{
				get
				{
					return active;
				}
				set
				{
					active = value;
				}
			}

			float life;	
			/// <summary>
			/// Particle Life
			/// </summary>
			public float Life
			{
				get
				{
					return life;
				}
				set
				{
					life = value;
				}
			}

			float fade;	
			/// <summary>
			/// Fade Speed
			/// </summary>
			public float Fade
			{
				get
				{
					return fade;
				}
				set
				{
					fade = value;
				}
			}

			float r;
			float g; 
			float b;
			
			/// <summary>
			/// 
			/// </summary>
			public float B 
			{
				get 
				{
					return b;
				}
				set 
				{
					b = value;
				}
			}

			/// <summary>
			/// 
			/// </summary>
			public float G 
			{
				get 
				{
					return g;
				}
				set 
				{
					g = value;
				}
			}

			/// <summary>
			/// 
			/// </summary>
			public float R 
			{
				get 
				{
					return r;
				}
				set 
				{
					r = value;
				}
			}
			// Color

			float x;
			float y;
			float z;
			// 
			/// <summary>
			/// Position
			/// </summary>
			public float X 
			{
				get 
				{
					return x;
				}
				set 
				{
					x = value;
				}
			}
			/// <summary>
			/// Position
			/// </summary>
			public float Y 
			{
				get 
				{
					return y;
				}
				set 
				{
					y = value;
				}
			}
			/// <summary>
			/// Position
			/// </summary>
			public float Z 
			{
				get 
				{
					return z;
				}
				set 
				{
					z = value;
				}
			}
			
			float xi;
			float yi;
			float zi;
			
			/// <summary>
			/// 
			/// </summary>
			public float Xi 
			{
				get 
				{
					return xi;
				}
				set 
				{
					xi = value;
				}
			}

			/// <summary>
			/// 
			/// </summary>
			public float Yi 
			{
				get 
				{
					return yi;
				}
				set 
				{
					yi = value;
				}
			}

			/// <summary>
			/// 
			/// </summary>
			public float Zi 
			{
				get 
				{
					return zi;
				}
				set 
				{
					zi = value;
				}
			}
			
			// Direction
			/// <summary>
			/// 
			/// </summary>
			float xg;
			float yg;
			float zg;

			/// <summary>
			/// Gravity
			/// </summary>
			public float Xg 
			{
				get 
				{
					return xg;
				}
				set 
				{
					xg = value;
				}
			}

			/// <summary>
			/// Gravity
			/// </summary>
			public float Yg 
			{
				get 
				{
					return yg;
				}
				set 
				{
					yg = value;
				}
			}

			/// <summary>
			/// Gravity
			/// </summary>
			public float Zg 
			{
				get 
				{
					return zg;
				}
				set 
				{
					zg = value;
				}
			}
		}

		Particle[] particle = new Particle[MaxParticles];

		float[][] colors = new float[12][] 
{
	// Rainbow Of Colors
	new float[3] {1.0f,0.5f,0.5f}, new float[3] {1.0f,0.75f,0.5f}, 
	new float[3] {1.0f,1.0f,0.5f}, new float[3] {0.75f,1.0f,0.5f},
	new float[3] {0.5f,1.0f,0.5f}, new float[3] {0.5f,1.0f,0.75f}, 
	new float[3] {0.5f,1.0f,1.0f}, new float[3] {0.5f,0.75f,1.0f},
	new float[3] {0.5f,0.5f,1.0f}, new float[3] {0.75f,0.5f,1.0f}, 
	new float[3] {1.0f,0.5f,1.0f}, new float[3] {1.0f,0.5f,0.75f} 
};

		/// <summary>
		/// 
		/// </summary>
		public NeHe019()
		{
			this.TextureName = "NeHe019.bmp";
			this.Texture = new int[3];
		}

		/// <summary>
		/// 
		/// </summary>
		public override void InitGL()
		{
			Events.KeyboardDown += new KeyboardEventHandler(this.KeyDown);
			Keyboard.EnableKeyRepeat(50,10);
			LoadGLTextures();

			Gl.glEnable(Gl.GL_TEXTURE_2D);									
			// Enable Texture Mapping
			Gl.glShadeModel(Gl.GL_SMOOTH);									
			// Enable Smooth Shading
			Gl.glClearColor(0.0f, 0.0f, 0.0f, 0.5f);						
			// Black Background
			Gl.glClearDepth(1.0f);											
			// Depth Buffer Setup
			Gl.glDisable(Gl.GL_DEPTH_TEST);									
			// Enables Depth Testing
			Gl.glEnable(Gl.GL_BLEND);										
			// Enable Blending
			Gl.glBlendFunc(Gl.GL_SRC_ALPHA, Gl.GL_ONE);						
			// Type Of Blending To Perform
			Gl.glHint(Gl.GL_PERSPECTIVE_CORRECTION_HINT, Gl.GL_NICEST);		
			// Really Nice Perspective Calculations

			Gl.glHint(Gl.GL_POINT_SMOOTH_HINT, Gl.GL_NICEST);				
			// Really Nice Point Smoothing

			Gl.glBindTexture(Gl.GL_TEXTURE_2D, this.Texture[0]);			
			// Select Our Texture

			for (int loop=0; loop < particle.Length; loop++)		
				// Initials All The Textures
			{
				particle[loop] = new Particle();
				particle[loop].Active = true;								
				// Make All The Particles Active
				particle[loop].Life = 1.0f;								
				// Give All The this.particles Full Life
				particle[loop].Fade = (float)(this.rand.Next(100))/1000.0f+0.003f;	
				// Random Fade Speed
				particle[loop].R = colors[loop*(12/MaxParticles)][0];	
				// Select Red Rainbow Color
				particle[loop].G = colors[loop*(12/MaxParticles)][1];	
				// Select Red Rainbow Color
				particle[loop].B = colors[loop*(12/MaxParticles)][2];	
				// Select Red Rainbow Color
				particle[loop].Xi = (float)((this.rand.Next(50))-26.0f)*10.0f;	
				// Random Speed On X Axis
				particle[loop].Yi = (float)((this.rand.Next(50))-25.0f)*10.0f;	
				// Random Speed On Y Axis
				particle[loop].Zi = (float)((this.rand.Next(50))-25.0f)*10.0f;	
				// Random Speed On Z Axis
				particle[loop].Xg = 0.0f;									
				// Set Horizontal Pull To Zero
				particle[loop].Yg = -0.8f;									
				// Set Vertical Pull Downward
				particle[loop].Zg = 0.0f;									
				// Set Pull On Z Axis To Zero
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public override void DrawGLScene()
		{
			Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
			Gl.glLoadIdentity();
			
			for (int loop=0; loop < particle.Length; loop++)	
				// Loop Through All The Particles
			{
				if (this.particle[loop].Active)							
					// If The Particle Is Active
				{
					float x = this.particle[loop].X;					
					// Grab Our Particle X Position
					float y = this.particle[loop].Y;					
					// Grab Our Particle Y Position
					float z = this.particle[loop].Z+zoom;				
					// Particle Z Pos + Zoom

					// Draw The Particle Using Our RGB Values, Fade The Particle Based On It's Life
					Gl.glColor4f(this.particle[loop].R, this.particle[loop].G, this.particle[loop].B, this.particle[loop].Life);

					Gl.glBegin(Gl.GL_TRIANGLE_STRIP);						
					// Build Quad From A TrianGL.gle Strip
					Gl.glTexCoord2d(1, 1); Gl.glVertex3f(x+0.5f, y+0.5f, z); 
					// Top Right
					Gl.glTexCoord2d(0, 1); Gl.glVertex3f(x-0.5f, y+0.5f, z); 
					// Top Left
					Gl.glTexCoord2d(1, 0); Gl.glVertex3f(x+0.5f, y-0.5f, z); 
					// Bottom Right
					Gl.glTexCoord2d(0, 0); Gl.glVertex3f(x-0.5f, y-0.5f, z); 
					// Bottom Left
					Gl.glEnd();										
					// Done Building TrianGL.gle Strip

					this.particle[loop].X += this.particle[loop].Xi/(slowdown*1000);
					// Move On The X Axis By X Speed
					this.particle[loop].Y += this.particle[loop].Yi/(slowdown*1000);
					// Move On The Y Axis By Y Speed
					this.particle[loop].Z += this.particle[loop].Zi/(slowdown*1000);
					// Move On The Z Axis By Z Speed

					this.particle[loop].Xi += this.particle[loop].Xg;			
					// Take Pull On X Axis Into Account
					this.particle[loop].Yi += this.particle[loop].Yg;			
					// Take Pull On Y Axis Into Account
					this.particle[loop].Zi += this.particle[loop].Zg;			
					// Take Pull On Z Axis Into Account
					this.particle[loop].Life -= this.particle[loop].Fade;		
					// Reduce Particles Life By 'Fade'

					if (this.particle[loop].Life < 0.0f)					
						// If Particle Is Burned Out
					{
						this.particle[loop].Life = 1.0f;					
						// Give It New Life
						this.particle[loop].Fade = (float)(this.rand.Next(100))/1000.0f+0.003f;	
						// Random Fade Value
						this.particle[loop].X = 0.0f;						
						// Center On X Axis
						this.particle[loop].Y = 0.0f;						
						// Center On Y Axis
						this.particle[loop].Z = 0.0f;						
						// Center On Z Axis
						this.particle[loop].Xi = this.XSpeed+(float)(this.rand.Next(60)-32.0f);	
						// X Axis Speed And Direction
						this.particle[loop].Yi = this.YSpeed+(float)(this.rand.Next(60)-30.0f);	
						// Y Axis Speed And Direction
						this.particle[loop].Zi = (float)(this.rand.Next(60)-30.0f);	
						// Z Axis Speed And Direction
						this.particle[loop].R = colors[this.col][0];			
						
						// Select Red From Color Table
						this.particle[loop].G = colors[this.col][1];			
						// Select Green From Color Table
						this.particle[loop].B = colors[this.col][2];			
						// Select Blue From Color Table
					}
				}
			}

			if (this.rainbow && (this.delay > 25))
			{
				this.delay = 0;
				// Reset The Rainbow Color Cycling Delay
				this.col = (this.col + 1) % 12;
				// Change The Particle Color
			}
			delay++;
		}

		private void KeyDown(object sender, KeyboardEventArgs e)
		{
			switch(e.Key)
			{
				case Key.Equals:
					if(this.slowdown > 1.0f)
					{
						this.slowdown -= 0.01f;
					}
					break;
				case Key.Plus:
					if(this.slowdown < 4.0f)
					{
						this.slowdown+=0.01f;
					}
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
					this.delay = 0;
					// Reset The Rainbow Color Cycling Delay
					this.col = (this.col + 1) % 12;
					// Change The Particle Color
					break;
				case Key.W:
					foreach(Particle p in particle)
					{
						if(p.Yg < 1.5f)
						{
							p.Yg += 0.01f;
						}
					}
					break;
				case Key.S:
					foreach(Particle p in particle)
					{
						if(p.Yg > -1.5f)
						{
							p.Yg -= 0.01f;
						}
					}
					break;
				case Key.D:
					foreach(Particle p in particle)
					{
						if(p.Xg < 1.5f)
						{
							p.Xg += 0.01f;
						}
					}
					break;
				case Key.A:
					foreach(Particle p in particle)
					{
						if(p.Xg > -1.5f)
						{
							p.Xg -= 0.01f;
						}
					}
					break;
				case Key.Tab:
					foreach(Particle p in particle)
					{
						p.X = 0.0f;
						// Center On X Axis
						p.Y = 0.0f;
						// Center On Y Axis
						p.Z = 0.0f;
						// Center On Z Axis
						p.Xi = (float)(this.rand.Next(50)-26.0f)*10.0f;	
						// Random Speed On X Axis
						p.Yi = (float)(this.rand.Next(50)-25.0f)*10.0f;	
						// Random Speed On Y Axis
						p.Zi = (float)(this.rand.Next(50)-25.0f)*10.0f;	
						// Random Speed On Z Axis
					}
					break;
				case Key.UpArrow:
					this.YSpeed += 1.0f;
					break;
				case Key.DownArrow:
					this.YSpeed -= 1.0f;
					break;
				case Key.RightArrow:
					this.XSpeed += 1.0f;
					break;
				case Key.LeftArrow:
					this.XSpeed -= 1.0f;
					break;
			}
		}
	}
}
