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

			float red;
			float green; 
			float blue;
			
			/// <summary>
			/// 
			/// </summary>
			public float Blue
			{
				get 
				{
					return blue;
				}
				set 
				{
					blue = value;
				}
			}

			/// <summary>
			/// 
			/// </summary>
			public float Green
			{
				get 
				{
					return green;
				}
				set 
				{
					green = value;
				}
			}

			/// <summary>
			/// 
			/// </summary>
			public float Red 
			{
				get 
				{
					return red;
				}
				set 
				{
					red = value;
				}
			}
			// Color

			float positionX;
			float positionY;
			float positionZ;
			// 
			/// <summary>
			/// Position
			/// </summary>
			public float PositionX 
			{
				get 
				{
					return positionX;
				}
				set 
				{
					positionX = value;
				}
			}
			/// <summary>
			/// Position
			/// </summary>
			public float PositionY 
			{
				get 
				{
					return positionY;
				}
				set 
				{
					positionY = value;
				}
			}
			/// <summary>
			/// Position
			/// </summary>
			public float PositionZ 
			{
				get 
				{
					return positionZ;
				}
				set 
				{
					positionZ = value;
				}
			}
			
			float directionX;
			float directionY;
			float directionZ;
			
			/// <summary>
			/// 
			/// </summary>
			public float DirectionX 
			{
				get 
				{
					return directionX;
				}
				set 
				{
					directionX = value;
				}
			}

			/// <summary>
			/// 
			/// </summary>
			public float DirectionY 
			{
				get 
				{
					return directionY;
				}
				set 
				{
					directionY = value;
				}
			}

			/// <summary>
			/// 
			/// </summary>
			public float DirectionZ 
			{
				get 
				{
					return directionZ;
				}
				set 
				{
					directionZ = value;
				}
			}
			
			// Direction
			/// <summary>
			/// 
			/// </summary>
			float gravityX;
			float gravityY;
			float gravityZ;

			/// <summary>
			/// Gravity
			/// </summary>
			public float GravityX 
			{
				get 
				{
					return gravityX;
				}
				set 
				{
					gravityX = value;
				}
			}

			/// <summary>
			/// Gravity
			/// </summary>
			public float GravityY 
			{
				get 
				{
					return gravityY;
				}
				set 
				{
					gravityY = value;
				}
			}

			/// <summary>
			/// Gravity
			/// </summary>
			public float GravityZ 
			{
				get 
				{
					return gravityZ;
				}
				set 
				{
					gravityZ = value;
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
				particle[loop].Red = colors[loop*(12/MaxParticles)][0];	
				// Select Red Rainbow Color
				particle[loop].Green = colors[loop*(12/MaxParticles)][1];	
				// Select Red Rainbow Color
				particle[loop].Blue = colors[loop*(12/MaxParticles)][2];	
				// Select Red Rainbow Color
				particle[loop].DirectionX = (float)((this.rand.Next(50))-26.0f)*10.0f;	
				// Random Speed On X Axis
				particle[loop].DirectionY = (float)((this.rand.Next(50))-25.0f)*10.0f;	
				// Random Speed On Y Axis
				particle[loop].DirectionZ = (float)((this.rand.Next(50))-25.0f)*10.0f;	
				// Random Speed On Z Axis
				particle[loop].GravityX = 0.0f;									
				// Set Horizontal Pull To Zero
				particle[loop].GravityY = -0.8f;									
				// Set Vertical Pull Downward
				particle[loop].GravityZ = 0.0f;									
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
					float x = this.particle[loop].PositionX;					
					// Grab Our Particle X Position
					float y = this.particle[loop].PositionY;					
					// Grab Our Particle Y Position
					float z = this.particle[loop].PositionZ+zoom;				
					// Particle Z Pos + Zoom

					// Draw The Particle Using Our RGB Values, Fade The Particle Based On It's Life
					Gl.glColor4f(this.particle[loop].Red, this.particle[loop].Green, this.particle[loop].Blue, this.particle[loop].Life);

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

					this.particle[loop].PositionX += this.particle[loop].DirectionX/(slowdown*1000);
					// Move On The X Axis By X Speed
					this.particle[loop].PositionY += this.particle[loop].DirectionY/(slowdown*1000);
					// Move On The Y Axis By Y Speed
					this.particle[loop].PositionZ += this.particle[loop].DirectionZ/(slowdown*1000);
					// Move On The Z Axis By Z Speed

					this.particle[loop].DirectionX += this.particle[loop].GravityX;			
					// Take Pull On X Axis Into Account
					this.particle[loop].DirectionY += this.particle[loop].GravityY;			
					// Take Pull On Y Axis Into Account
					this.particle[loop].DirectionZ += this.particle[loop].GravityZ;			
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
						this.particle[loop].PositionX = 0.0f;						
						// Center On X Axis
						this.particle[loop].PositionY = 0.0f;						
						// Center On Y Axis
						this.particle[loop].PositionZ = 0.0f;						
						// Center On Z Axis
						this.particle[loop].DirectionX = this.XSpeed+(float)(this.rand.Next(60)-32.0f);	
						// X Axis Speed And Direction
						this.particle[loop].DirectionY = this.YSpeed+(float)(this.rand.Next(60)-30.0f);	
						// Y Axis Speed And Direction
						this.particle[loop].DirectionZ = (float)(this.rand.Next(60)-30.0f);	
						// Z Axis Speed And Direction
						this.particle[loop].Red = colors[this.col][0];			
						
						// Select Red From Color Table
						this.particle[loop].Green = colors[this.col][1];			
						// Select Green From Color Table
						this.particle[loop].Blue = colors[this.col][2];			
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
						if(p.GravityY < 1.5f)
						{
							p.GravityY += 0.01f;
						}
					}
					break;
				case Key.S:
					foreach(Particle p in particle)
					{
						if(p.GravityY > -1.5f)
						{
							p.GravityY -= 0.01f;
						}
					}
					break;
				case Key.D:
					foreach(Particle p in particle)
					{
						if(p.GravityX < 1.5f)
						{
							p.GravityX += 0.01f;
						}
					}
					break;
				case Key.A:
					foreach(Particle p in particle)
					{
						if(p.GravityX > -1.5f)
						{
							p.GravityX -= 0.01f;
						}
					}
					break;
				case Key.Tab:
					foreach(Particle p in particle)
					{
						p.PositionX = 0.0f;
						// Center On X Axis
						p.PositionY = 0.0f;
						// Center On Y Axis
						p.PositionZ = 0.0f;
						// Center On Z Axis
						p.DirectionX = (float)(this.rand.Next(50)-26.0f)*10.0f;	
						// Random Speed On X Axis
						p.DirectionY = (float)(this.rand.Next(50)-25.0f)*10.0f;	
						// Random Speed On Y Axis
						p.DirectionZ = (float)(this.rand.Next(50)-25.0f)*10.0f;	
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
