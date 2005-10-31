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
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Reflection;

using SdlDotNet;
using Tao.OpenGl;

namespace SdlDotNet.Examples
{
	class NeHe010 : NeHe008
	{
		private static string title = "Lesson 10: Loading and Moving through a 3D World";
		public new static string Title
		{
			get
			{
				return title;
			}
		}
		private float xpos;
		private float zpos;
		private float heading;
		private float walkbias = 0;
		private float walkbiasangle = 0;
		private float lookupdown = 0;
		private const float piover180 = 0.0174532925f;

		private struct Vertex 
		{
			public float x, y, z;
			public float u, v;
		}
		private struct Triangle 
		{
			public Vertex[] vertex;
		}
		private struct Sector 
		{
			public int numtriangles;
			public Triangle[] triangle;
		};
		private Sector sector;

		public NeHe010()
		{
			this.TextureName = "NeHe010.bmp";
			this.Texture = new int[3];
			Events.KeyboardDown += new KeyboardEventHandler(this.KeyDown);
			Keyboard.EnableKeyRepeat(150,50);
			this.Z = 0;
		}

		public override void InitGL()
		{
			this.LoadGLTextures();
			Gl.glEnable(Gl.GL_TEXTURE_2D);                                      
			// Enable Texture Mapping
			Gl.glBlendFunc(Gl.GL_SRC_ALPHA, Gl.GL_ONE);                         
			// Set The Blending Function For Translucency
			Gl.glClearColor(0, 0, 0, 0);                                        
			// This Will Clear The Background Color To Black
			Gl.glClearDepth(1);                                                 
			// Enables Clearing Of The Depth Buffer
			Gl.glDepthFunc(Gl.GL_LESS);                                         
			// The Type Of Depth Test To Do
			Gl.glEnable(Gl.GL_DEPTH_TEST);                                      
			// Enables Depth Testing
			Gl.glShadeModel(Gl.GL_SMOOTH);                                      
			// Enables Smooth Color Shading
			Gl.glHint(Gl.GL_PERSPECTIVE_CORRECTION_HINT, Gl.GL_NICEST);         
			// Really Nice Perspective Calculations
			this.SetupWorld();
		}

		#region SetupWorld()
		/// <summary>
		///     Loads and parses the world.
		/// </summary>
		/// <returns>
		///     <c>true</c> on successful load, otherwise <c>false</c>.
		/// </returns>
		private void SetupWorld() 
		{
			// This Method Is Pretty Ugly.  The .Net Framework Doesn't 
			// Have An Analogous Implementation
			// Of C/C++'s sscanf().  As Such You Have To Manually 
			// Parse A File, You Can Either Do So
			// Procedurally Like I'm Doing Here, Or Use Some RegEx's.  
			// To Make It A Bit Easier I Modified
			// The World.txt File To Remove Comments, Empty Lines And 
			// Excess Spaces.  Sorry For The
			// Ugliness, I'm Too Lazy To Clean It Up.
			float x, y, z, u, v;                                                
			// Local Vertex Information
			int numtriangles;                                                   
			// Local Number Of Triangles
			string oneline = "";                                                
			// The Line We've Read
			string[] splitter;                                                  
			// Array For Split Values
			StreamReader reader = null;                                         
			// Our StreamReader
			ASCIIEncoding encoding = new ASCIIEncoding();                       
			// ASCII Encoding
			string fileName = @"NeHe010.World.txt";                       
			// The File To Load
			string fileName1 = string.Format("Data{0}{1}",                      
				// Look For Data\Filename
				Path.DirectorySeparatorChar, fileName);
			string fileName2 = string.Format("{0}{1}{0}{1}Data{1}{2}",          
				// Look For ..\..\Data\Filename
				"..", Path.DirectorySeparatorChar, fileName);

			// Make Sure The File Exists In One Of The Usual Directories
			if(!File.Exists(fileName) && !File.Exists(fileName1) && !File.Exists(fileName2)) 
			{
				throw new FileNotFoundException();                                              
			}

			if(File.Exists(fileName)) 
			{                                         
				// Does The File Exist Here?
				fileName = fileName;                                            
				// Do Nothing
			}
			else if(File.Exists(fileName1)) 
			{                                   
				// Does The File Exist Here?
				fileName = fileName1;                                           
				// Swap Filename
			}
			else if(File.Exists(fileName2)) 
			{                                   
				// Does The File Exist Here?
				fileName = fileName2;                                           
				// Swap Filename
			}

			reader = new StreamReader(fileName, encoding);                      
			// Open The File As ASCII Text

			oneline = reader.ReadLine();                                        
			// Read The First Line
			splitter = oneline.Split();                                         
			// Split The Line On Spaces

			// The First Item In The Array Will Contain The String "NUMPOLLIES",
			// Which We Will Ignore
			numtriangles = Convert.ToInt32(splitter[1]);                        
			// Save The Number Of Triangles As An int

			sector.triangle = new Triangle[numtriangles];                       
			// Initialize The Triangles And Save To sector
			sector.numtriangles = numtriangles;                                 
			// Save The Number Of Triangles In sector

			for(int triloop = 0; triloop < numtriangles; triloop++) 
			{           
				// For Every Triangle
				sector.triangle[triloop].vertex = new Vertex[3];                
				// Initialize The Vertices In sector
				for(int vertloop = 0; vertloop < 3; vertloop++) 
				{               // For Every Vertex
					oneline = reader.ReadLine();                                
					// Read A Line
					if(oneline != null) 
					{                                      
						// If The Line Isn't null
						splitter = oneline.Split();                             
						// Split The Line On Spaces
						x = Single.Parse(splitter[0]);                          
						// Save x As A float
						y = Single.Parse(splitter[1]);                          
						// Save y As A float
						z = Single.Parse(splitter[2]);                          
						// Save z As A float
						u = Single.Parse(splitter[3]);                          
						// Save u As A float
						v = Single.Parse(splitter[4]);                          
						// Save v As A float
						sector.triangle[triloop].vertex[vertloop].x = x;        
						// Save x To sector's Current triangle's vertex x
						sector.triangle[triloop].vertex[vertloop].y = y;        
						// Save y To sector's Current triangle's vertex y
						sector.triangle[triloop].vertex[vertloop].z = z;        
						// Save z To sector's Current triangle's vertex z
						sector.triangle[triloop].vertex[vertloop].u = u;        
						// Save u To sector's Current triangle's vertex u
						sector.triangle[triloop].vertex[vertloop].v = v;        
						// Save v To sector's Current triangle's vertex v
					}
				}
			}
			if(reader != null) 
			{
				reader.Close();                                                 
				// Close The StreamReader
			}
		}
		#endregion SetupWorld()

		#region void DrawGLScene()
		/// <summary>
		///     Here's where we do all the drawing.
		/// </summary>
		/// <returns>
		///     <c>true</c> on successful drawing, otherwise <c>false</c>.
		/// </returns>
		public override void DrawGLScene() 
		{
			Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);        
			// Clear The Screen And The Depth Buffer
			Gl.glLoadIdentity();                                                
			// Reset The View

			float x_m, y_m, z_m, u_m, v_m;
			float xtrans = -xpos;
			float ztrans = -zpos;
			float ytrans = -walkbias - 0.25f;
			float sceneroty = 360 - this.YRot;
			int numtriangles;

			Gl.glRotatef(lookupdown, 1, 0, 0);
			Gl.glRotatef(sceneroty, 0, 1, 0);
			Gl.glTranslatef(xtrans, ytrans, ztrans);
			Gl.glBindTexture(Gl.GL_TEXTURE_2D, this.Texture[this.Filter]);
			numtriangles = sector.numtriangles;
			// Process Each Triangle
			for(int loop_m = 0; loop_m < numtriangles; loop_m++) 
			{
				Gl.glBegin(Gl.GL_TRIANGLES);
				Gl.glNormal3f(0, 0, 1);
				x_m = sector.triangle[loop_m].vertex[0].x;
				y_m = sector.triangle[loop_m].vertex[0].y;
				z_m = sector.triangle[loop_m].vertex[0].z;
				u_m = sector.triangle[loop_m].vertex[0].u;
				v_m = sector.triangle[loop_m].vertex[0].v;
				Gl.glTexCoord2f(u_m, v_m); Gl.glVertex3f(x_m, y_m, z_m);

				x_m = sector.triangle[loop_m].vertex[1].x;
				y_m = sector.triangle[loop_m].vertex[1].y;
				z_m = sector.triangle[loop_m].vertex[1].z;
				u_m = sector.triangle[loop_m].vertex[1].u;
				v_m = sector.triangle[loop_m].vertex[1].v;
				Gl.glTexCoord2f(u_m, v_m); Gl.glVertex3f(x_m, y_m, z_m);

				x_m = sector.triangle[loop_m].vertex[2].x;
				y_m = sector.triangle[loop_m].vertex[2].y;
				z_m = sector.triangle[loop_m].vertex[2].z;
				u_m = sector.triangle[loop_m].vertex[2].u;
				v_m = sector.triangle[loop_m].vertex[2].v;
				Gl.glTexCoord2f(u_m, v_m); Gl.glVertex3f(x_m, y_m, z_m);
				Gl.glEnd();

				Video.GLSwapBuffers();
			}
		}
		#endregion bool DrawGLScene()

		private void KeyDown(object sender, KeyboardEventArgs e)
		{
			switch (e.Key) 
			{
				case Key.F:
					if (!this.Fp)
					{
						this.Fp = true;
						this.Filter += 1;
						if(this.Filter > 2) 
						{
							this.Filter = 0;
						}
					}
					else
					{
						this.Fp = false;
					}
					break;
				case Key.PageUp:
					this.Z -= 0.02f;
					break;
				case Key.PageDown:
					this.Z += 0.02f;
					break;
				case Key.UpArrow: 
					xpos -= (float) Math.Sin(heading * piover180) * 0.05f;
					zpos -= (float) Math.Cos(heading * piover180) * 0.05f;
					if(walkbiasangle >= 359) 
					{
						walkbiasangle = 0;
					}
					else 
					{
						walkbiasangle += 10;
					}
					walkbias = (float) Math.Sin(walkbiasangle * piover180) / 20;
					break;
				case Key.DownArrow:
					xpos += (float) Math.Sin(heading * piover180) * 0.05f;
					zpos += (float) Math.Cos(heading * piover180) * 0.05f;
					if(walkbiasangle <= 1) 
					{
						walkbiasangle = 359;
					}
					else 
					{
						walkbiasangle -= 10;
					}
					walkbias = (float) Math.Sin(walkbiasangle * piover180) / 20;
					break;
				case Key.RightArrow:
					heading -= 1;
					this.YRot = heading;
					break;
				case Key.LeftArrow:
					heading += 1;
					this.YRot = heading;
					break;
				case Key.B:
					// Blending Code Starts Here
					if(!this.Bp) 
					{
						this.Bp = true;
						this.Blend = !this.Blend;
						if(this.Blend) 
						{
							Gl.glEnable(Gl.GL_BLEND);                           
							// Turn Blending On
							Gl.glDisable(Gl.GL_DEPTH_TEST);                     
							// Turn Depth Testing Off
						}
						else 
						{
							Gl.glDisable(Gl.GL_BLEND);                          
							// Turn Blending Off
							Gl.glEnable(Gl.GL_DEPTH_TEST);                      
							// Turn Depth Testing On
						}
					}
					else
					{
						this.Bp = false;
					}
					// Blending Code Ends Here
					break;
			}
		}
		protected float XPos
		{
			get
			{
				return xpos;
			}
			set
			{
				xpos = value;
			}
		}
		protected float ZPos
		{
			get
			{
				return zpos;
			}
			set
			{
				zpos = value;
			}
		}
	}
}