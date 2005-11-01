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
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;
using System.Reflection;

using SdlDotNet;
using Tao.OpenGl;

namespace SdlDotNet.Examples
{
	class NeHe025 : NeHe010
	{
		public new static string Title
		{
			get
			{
				return "Lesson 25: Morphing and Loading Objects from a File";
			}
		}

		private float zspeed;                                            
		private float ypos;
		public float YPos
		{
			get
			{
				return ypos;
			}
			set
			{
				ypos = value;
			}
		}
		private Random rand = new Random();

		private int key = 1;                                             
		// Make Sure Same Morph Key Is Not Pressed
		private int step = 0;                                            
		// Step Counter
		private int steps = 200;                                         
		// Maximum Number Of Steps
		private bool morph;                                              
		// Morphing?

		private int maxver;                                              
		// Maximum Number Of Vertices
		private Thing morph1, morph2, morph3, morph4;                   
		// Our 4 Morphable Objects
		private Thing helper, source, destination;

		private struct Vertex 
		{                                     
			// Structure For 3d Points
			public float X;                                                     
			// X Coordinate
			public float Y;                                                     
			// Y Coordinate
			public float Z;                                                     
			// Z Coordinate
		}
		private struct Thing 
		{                                                  
			// Structure For An Object
			public int Verts;                                                   
			// Number Of Vertices For The Object
			public Vertex[] Points;                                             
			// Vertices
		}

		public NeHe025()
		{
			this.ZPos = -15;
			Events.KeyboardDown += new KeyboardEventHandler(this.KeyDown);
			Keyboard.EnableKeyRepeat(150,50);
		}

		public override void InitGL()
		{
			// All Setup For OpenGL Goes Here
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

			maxver = 0;                                                         
			// Sets Max Vertices To 0 By Default

			LoadThing("NeHe025.Sphere.txt", ref morph1);                  
			// Load The First Object Into morph1 From File sphere.txt
			LoadThing("NeHe025.Torus.txt", ref morph2);                   
			// Load The Second Object Into morph2 From File torus.txt
			LoadThing("NeHe025.Tube.txt", ref morph3);                    
			// Load The Third Object Into morph3 From File tube.txt

			AllocateThing(ref morph4, 486);                                     
			// Manually Reserver Ram For A 4th 468 Vertice Object (morph4)
			for(int i = 0; i < 486; i++) 
			{                                     
				// Loop Through All 468 Vertices
				morph4.Points[i].X = ((float) (rand.Next() % 14000) / 1000) - 7;
				// morph4 X Point Becomes A Random Float Value From -7 to 7
				morph4.Points[i].Y = ((float) (rand.Next() % 14000) / 1000) - 7;
				// morph4 Y Point Becomes A Random Float Value From -7 to 7
				morph4.Points[i].Z = ((float) (rand.Next() % 14000) / 1000) - 7;
				// morph4 Z Point Becomes A Random Float Value From -7 to 7
			}

			LoadThing("NeHe025.Sphere.txt", ref helper);
			// Load sphere.txt Object Into Helper (Used As Starting Point)
			source = destination = morph1;
			// Source & Destination Are Set To Equal First Object (morph1)

		}

		#region DrawGLScene()
		/// <summary>
		///     Draws everything.
		/// </summary>
		/// <returns>
		///     Returns <c>true</c> on success, otherwise <c>false</c>.
		/// </returns>
		public override void DrawGLScene() 
		{                                    
			// Here's Where We Do All The Drawing
			Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
			// Clear The Screen And The Depth Buffer
			Gl.glLoadIdentity();
			// Reset The View
			Gl.glTranslatef(this.XPos, this.YPos, this.ZPos);  
			// Translate The The Current Position To Start Drawing
			Gl.glRotatef(this.XRot, 1, 0, 0);
			// Rotate On The X Axis By xrot
			Gl.glRotatef(this.YRot, 0, 1, 0);
			// Rotate On The Y Axis By yrot
			Gl.glRotatef(this.ZRot, 0, 0, 1);
			// Rotate On The Z Axis By zrot

			this.XRot += this.XSpeed;
			this.YRot += this.YSpeed;
			this.ZRot += zspeed;
			// Increase xrot,yrot & zrot by xspeed, yspeed & zspeed

			float tx, ty, tz;
			// Temp X, Y & Z Variables
			Vertex q;
			// Holds Returned Calculated Values For One Vertex

			Gl.glBegin(Gl.GL_POINTS); 
			// Begin Drawing Points
			for(int i = 0; i < morph1.Verts; i++) 
			{
				// Loop Through All The Verts Of morph1 (All Objects Have
				if(morph) 
				{
					// The Same Amount Of Verts For Simplicity, Could Use maxver Also)
					q = Calculate(i);
				}
				else 
				{
					q.X = q.Y = q.Z = 0;
					// If morph Is True Calculate Movement Otherwise Movement=0
				}
				helper.Points[i].X -= q.X; 
				// Subtract q.x Units From helper.points[i].x (Move On X Axis)
				helper.Points[i].Y -= q.Y;
				// Subtract q.y Units From helper.points[i].y (Move On Y Axis)
				helper.Points[i].Z -= q.Z;
				// Subtract q.z Units From helper.points[i].z (Move On Z Axis)
				tx = helper.Points[i].X;
				// Make Temp X Variable Equal To Helper's X Variable
				ty = helper.Points[i].Y;
				// Make Temp Y Variable Equal To Helper's Y Variable
				tz = helper.Points[i].Z;
				// Make Temp Z Variable Equal To Helper's Z Variable

				Gl.glColor3f(0, 1, 1); 
				// Set Color To A Bright Shade Of Off Blue
				Gl.glVertex3f(tx, ty, tz); 
				// Draw A Point At The Current Temp Values (Vertex)
				Gl.glColor3f(0, 0.5f, 1);
				// Darken Color A Bit
				tx -= 2 * q.X;
				ty -= 2 * q.Y;
				ty -= 2 * q.Y;
				// Calculate Two Positions Ahead
				Gl.glVertex3f(tx, ty, tz); 
				// Draw A Second Point At The Newly Calculate Position
				Gl.glColor3f(0, 0, 1);
				// Set Color To A Very Dark Blue
				tx -= 2 * q.X;
				ty -= 2 * q.Y;
				ty -= 2 * q.Y;
				// Calculate Two More Positions Ahead
				Gl.glVertex3f(tx, ty, tz);
				// Draw A Third Point At The Second New Position
			}
			// This Creates A Ghostly Tail As Points Move
			Gl.glEnd();
			// Done Drawing Points

			// If We're Morphing And We Haven't 
			// Gone Through All 200 Steps Increase Our Step Counter
			// Otherwise Set Morphing To False, 
			// Make Source=Destination And Set The Step Counter Back To Zero.
			if(morph && step <= steps) 
			{
				step++;
			}
			else 
			{
				morph = false;
				source = destination;
				step = 0;
			}
			Video.GLSwapBuffers();
		}
		#endregion DrawGLScene()

		private void KeyDown(object sender, KeyboardEventArgs e)
		{
			switch (e.Key) 
			{
				case Key.PageUp:
					this.zspeed -= 0.01f;
					break;
				case Key.PageDown:
					this.zspeed += 0.01f;
					break;
				case Key.UpArrow: 
					this.XSpeed -= 0.01f;
					break;
				case Key.DownArrow:
					this.XSpeed += 0.01f;
					break;
				case Key.RightArrow:
					this.YSpeed += 0.01f;
					break;
				case Key.LeftArrow:
					this.YSpeed -= 0.01f;
					break;
				case Key.Q:
					this.ZPos -= 0.01f;
					break;
				case Key.Z:
					this.ZPos += 0.01f;
					break;				
				case Key.W:
					this.ypos += 0.01f;
					break;
				case Key.S:
					this.ypos -= 0.01f;
					break;
				case Key.D:
					this.XPos += 0.01f;
					break;
				case Key.A:
					this.XPos -= 0.01f;
					break;
				case Key.One:
					if (key != 1 && !morph)
					{
						key = 1;
						morph = true;
						destination = morph1;
					}
					break;
				case Key.Two:
					if (key != 2 && !morph)
					{
						key = 2;
						morph = true;
						destination = morph2;
					}
					break;
				case Key.Three:
					if (key != 3 && !morph)
					{
						key = 3;
						morph = true;
						destination = morph3;
					}
					break;
				case Key.Four:
					if (key != 4 && !morph)
					{
						key = 4;
						morph = true;
						destination = morph4;
					}
					break;
			}
		}
		#region LoadThing(string filename, ref Thing k)
		/// <summary>
		///     Loads Object from a file.
		/// </summary>
		/// <param name="filename">
		///     The file to load.
		/// </param>
		/// <param name="k">
		///     The Object to save to.
		/// </param>
		private void LoadThing(string filename, ref Thing k) 
		{
			int ver;  
			// Will Hold Vertice Count
			float rx, ry, rz;
			// Hold Vertex X, Y & Z Position
			string oneline = "";
			// The Line We've Read
			string[] splitter;
			// Array For Split Values
			StreamReader reader = null;
			// Our StreamReader
			ASCIIEncoding encoding = new ASCIIEncoding();
			// ASCII Encoding

			try 
			{
				if(filename == null || filename == string.Empty) 
				{                      
					// Make Sure A Filename Was Given
					return; 
					// If Not Return
				}

				string fileName1 = string.Format("Data{0}{1}", 
					// Look For Data\Filename
					Path.DirectorySeparatorChar, filename);
				string fileName2 = string.Format("{0}{1}{0}{1}Data{1}{2}",
					// Look For ..\..\Data\Filename
					"..", Path.DirectorySeparatorChar, filename);

				// Make Sure The File Exists In One Of The Usual Directories
				if(!File.Exists(filename) && !File.Exists(fileName1) && !File.Exists(fileName2)) 
				{
					return;
					// If Not Return Null
				}

				if(File.Exists(filename)) 
				{                                             
					// Does The File Exist Here?
					reader = new StreamReader(filename, encoding); 
					// Open The File As ASCII Text
				}
				else if(File.Exists(fileName1)) 
				{                                       
					// Does The File Exist Here?
					reader = new StreamReader(fileName1, encoding);
					// Open The File As ASCII Text
				}
				else if(File.Exists(fileName2)) 
				{                                       
					// Does The File Exist Here?
					reader = new StreamReader(fileName2, encoding);
					// Open The File As ASCII Text
				}

				oneline = reader.ReadLine();
				// Read The First Line
				splitter = oneline.Split();
				// Split The Line On Spaces

				// The First Item In The Array 
				// Will Contain The String "Vertices:", Which We Will Ignore
				ver = Convert.ToInt32(splitter[1]);
				// Save The Number Of Triangles To ver As An int
				k.Verts = ver;
				// Sets PointObjects (k) verts Variable To Equal The Value Of ver
				AllocateThing(ref k, ver); 
				// Jumps To Code That Allocates Ram To Hold The Object

				for(int vertloop = 0; vertloop < ver; vertloop++) 
				{                     
					// Loop Through The Vertices
					oneline = reader.ReadLine();
					// Reads In The Next Line Of Text
					if(oneline != null) 
					{
						// If The Line's Not null
						splitter = oneline.Split();
						// Split The Line On Spaces
						rx = float.Parse(splitter[0]);
						// Save The X Value As A Float

						ry = float.Parse(splitter[1]);
						// Save The Y Value As A Float
						rz = float.Parse(splitter[2]);
						// Save The Z Value As A Float
						k.Points[vertloop].X = rx;
						// Sets PointObjects (k) points.x Value To rx
						k.Points[vertloop].Y = ry;
						// Sets PointObjects (k) points.y Value To ry
						k.Points[vertloop].Z = rz;
						// Sets PointObjects (k) points.z Value To rz
					}
				}

				if(ver > maxver) 
				{                                                      
					// If ver Is Greater Than maxver
					// maxver Keeps Track Of The Highest Number Of 
					// Vertices Used In Any Of The Objects
					maxver = ver;                                                       // Set maxver Equal To ver
				}
			}
			catch(Exception e) 
			{
				// Handle Any Exceptions While Loading Object Data, Exit App
				string errorMsg = "An Error Occurred While Loading And Parsing Object Data:\n\t" + filename + "\n" + "\n\nStack Trace:\n\t" + e.StackTrace + "\n";
				MessageBox.Show(errorMsg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
			}
			finally 
			{
				if(reader != null) 
				{
					reader.Close();                                                     // Close The StreamReader
				}
			}
		}
		#endregion LoadThing(string filename, ref Thing k)

		#region AllocateThing(ref Thing thing, int number)
		/// <summary>
		///     Allocate memory for each object.
		/// </summary>
		/// <param name="thing">
		///     The object.
		/// </param>
		/// <param name="number">
		///     The number of points to allocate.
		/// </param>
		private void AllocateThing(ref Thing thing, int number) 
		{
			thing.Points = new Vertex[number];
		}
		#endregion AllocateThing(ref Thing thing, int number)
		#region Vertex Calculate(int i)
		/// <summary>
		///     Calculates movement of points during morphing.
		/// </summary>
		/// <param name="i">
		///     The number of the point to calculate.
		/// </param>
		/// <returns>
		///     A Vertex.
		/// </returns>
		private Vertex Calculate(int i) 
		{
			// This Makes Points Move At A Speed So They All Get To 
			// Their Destination At The Same Time
			Vertex a;
			// Temporary Vertex Called a
			a.X = (source.Points[i].X - destination.Points[i].X) / steps;
			// a.X Value Equals Source X - Destination X Divided By Steps
			a.Y = (source.Points[i].Y - destination.Points[i].Y) / steps;
			// a.Y Value Equals Source Y - Destination Y Divided By Steps
			a.Z = (source.Points[i].Z - destination.Points[i].Z) / steps;
			// a.Z Value Equals Source Z - Destination Z Divided By Steps
			return a; 
			// Return The Results
		}
		#endregion Vertex Calculate(int i)
	}
}