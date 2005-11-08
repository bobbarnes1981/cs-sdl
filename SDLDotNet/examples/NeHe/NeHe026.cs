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
	/// <summary>
	/// 
	/// </summary>
	public class NeHe026 : NeHe025
	{
		#region Fields

		/// <summary>
		/// Lesson Title
		/// </summary>
		public new static string Title
		{
			get
			{
				return "Lesson 26: Clipping & Reflections Using The Stencil Buffer";
			}
		}
		
		private Glu.GLUquadric q;
		// Quadratic For Drawing A Sphere
		private float ballHeight;

		private float xrotspeed;  
		// X Rotation Speed
		private float yrotspeed;  
		// Y Rotation Speed
		private static float zoom = -7;

		// Maximum Number Of Vertices
		private float[] LightAmb = {0.7f, 0.7f, 0.7f, 1};
		// Ambient Light
		private float[] LightDif = {1, 1, 1, 1};
		// Diffuse Light
		private float[] LightPos = {4, 4, 6, 1};
		// Light Position
		
		#endregion Fields

		#region Constructor

		/// <summary>
		/// Basic constructor
		/// </summary>
		public NeHe026()
		{
			this.ballHeight = 2;
			this.Texture = new int[3];
			this.TextureName = new string[3];
			this.TextureName[0] = "NeHe026.EnvWall.bmp";
			this.TextureName[1] = "NeHe026.Ball.bmp";
			this.TextureName[2] = "NeHe026.EnvRoll.bmp";
		}
		
		#endregion Constructor

		#region DrawFloor()
		/// <summary>
		/// Draws the floor.
		/// </summary>
		private void DrawFloor() 
		{
			Gl.glBindTexture(Gl.GL_TEXTURE_2D, this.Texture[0]);
			// Select Texture 1 (0)
			Gl.glBegin(Gl.GL_QUADS);
			// Begin Drawing A Quad
			Gl.glNormal3f(0, 1, 0);
			// Normal Pointing Up
			Gl.glTexCoord2f(0, 1);
			// Bottom Left Of Texture
			Gl.glVertex3f(-2, 0, 2);
			// Bottom Left Corner Of Floor
			Gl.glTexCoord2f(0, 0);
			// Top Left Of Texture
			Gl.glVertex3f(-2, 0, -2);
			// Top Left Corner Of Floor
			Gl.glTexCoord2f(1, 0);
			// Top Right Of Texture
			Gl.glVertex3f(2, 0,-2);
			// Top Right Corner Of Floor
			Gl.glTexCoord2f(1, 1);
			// Bottom Right Of Texture
			Gl.glVertex3f(2, 0, 2);
			// Bottom Right Corner Of Floor
			Gl.glEnd();
			// Done Drawing The Quad
		}
		#endregion DrawFloor()

		#region DrawObject()
		/// <summary>
		/// Draws our ball.
		/// </summary>
		private void DrawObject() 
		{
			Gl.glColor3f(1, 1, 1); // Set Color To White
			Gl.glBindTexture(Gl.GL_TEXTURE_2D, this.Texture[1]); // Select Texture 2 (1)
			Glu.gluSphere(q, 0.35f, 32, 16); // Draw First Sphere
			Gl.glBindTexture(Gl.GL_TEXTURE_2D, this.Texture[2]); // Select Texture 3 (2)
			Gl.glColor4f(1, 1, 1, 0.4f);// Set Color To White With 40% Alpha
			Gl.glEnable(Gl.GL_BLEND);  // Enable Blending
			Gl.glBlendFunc(Gl.GL_SRC_ALPHA, Gl.GL_ONE); // Set Blending Mode To Mix Based On SRC Alpha
			Gl.glEnable(Gl.GL_TEXTURE_GEN_S);// Enable Sphere Mapping
			Gl.glEnable(Gl.GL_TEXTURE_GEN_T);// Enable Sphere Mapping
			Glu.gluSphere(q, 0.35f, 32, 16); // Draw Another Sphere Using New Texture
			// Textures Will Mix Creating A MultiTexture Effect (Reflection)
			Gl.glDisable(Gl.GL_TEXTURE_GEN_S);   // Disable Sphere Mapping
			Gl.glDisable(Gl.GL_TEXTURE_GEN_T);   // Disable Sphere Mapping
			Gl.glDisable(Gl.GL_BLEND); // Disable Blending
		}
		#endregion DrawObject()

		#region Lesson Setup

		#region void InitGL()
		/// <summary>
		/// All setup for OpenGL goes here.
		/// </summary>
		/// <returns>
		/// Returns <c>true</c> on success, otherwise <c>false</c>.
		/// </returns>
		protected override void InitGL() 
		{
			Events.KeyboardDown += new KeyboardEventHandler(this.KeyDown);
			Keyboard.EnableKeyRepeat(150,50);

			// All Setup For OpenGL Goes Here
			LoadGLTextures();
			
			Gl.glShadeModel(Gl.GL_SMOOTH);   // Enable Smooth Shading
			Gl.glClearColor(0.2f, 0.5f, 1, 1);   // Background
			Gl.glClearDepth(1);// Depth Buffer Setup
			Gl.glClearStencil(0);  // Clear The Stencil Buffer To 0
			Gl.glEnable(Gl.GL_DEPTH_TEST);   // Enables Depth Testing
			Gl.glDepthFunc(Gl.GL_LEQUAL);// The Type Of Depth Testing To Do
			Gl.glHint(Gl.GL_PERSPECTIVE_CORRECTION_HINT, Gl.GL_NICEST); // Really Nice Perspective Calculations
			Gl.glEnable(Gl.GL_TEXTURE_2D);   // Enable 2D Texture Mapping
			Gl.glLightfv(Gl.GL_LIGHT0, Gl.GL_AMBIENT, LightAmb);// Set The Ambient Lighting For Light0
			Gl.glLightfv(Gl.GL_LIGHT0, Gl.GL_DIFFUSE, LightDif);// Set The Diffuse Lighting For Light0
			Gl.glLightfv(Gl.GL_LIGHT0, Gl.GL_POSITION, LightPos);   // Set The Position For Light0
			Gl.glEnable(Gl.GL_LIGHT0); // Enable Light 0
			Gl.glEnable(Gl.GL_LIGHTING);// Enable Lighting
			q = Glu.gluNewQuadric();   // Create A New Quadratic
			Glu.gluQuadricNormals(q, Gl.GL_SMOOTH);  // Generate Smooth Normals For The Quad
			Glu.gluQuadricTexture(q, Gl.GL_TRUE);// Enable Texture Coords For The Quad
			Gl.glTexGeni(Gl.GL_S, Gl.GL_TEXTURE_GEN_MODE, Gl.GL_SPHERE_MAP);// Set Up Sphere Mapping
			Gl.glTexGeni(Gl.GL_T, Gl.GL_TEXTURE_GEN_MODE, Gl.GL_SPHERE_MAP);// Set Up Sphere Mapping

		}
		#endregion void InitGL()

		#endregion Lesson Setup

		#region Render

		#region void DrawGLScene()
		/// <summary>
		/// Renders the scene
		/// </summary>
		protected override void DrawGLScene() 
		{
			// Clear Screen, Depth Buffer & Stencil Buffer
			Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT | Gl.GL_STENCIL_BUFFER_BIT);

			// Clip Plane Equations
			double[] eqr = {0,-1, 0, 0};
			// Plane Equation To Use For The Reflected Objects

			Gl.glLoadIdentity();
			// Reset The Modelview Matrix
			Gl.glTranslatef(0, -0.6f, zoom);
			// Zoom And Raise Camera Above The Floor (Up 0.6 Units)
			Gl.glColorMask(0, 0, 0, 0);
			// Set Color Mask
			Gl.glEnable(Gl.GL_STENCIL_TEST);
			// Enable Stencil Buffer For "marking" The Floor
			Gl.glStencilFunc(Gl.GL_ALWAYS, 1, 1);
			// Always Passes, 1 Bit Plane, 1 As Mask
			Gl.glStencilOp(Gl.GL_KEEP, Gl.GL_KEEP, Gl.GL_REPLACE);
			// We Set The Stencil Buffer To 1 Where We Draw Any Polygon
			// Keep If Test Fails, Keep If Test Passes But Buffer Test Fails
			// Replace If Test Passes
			Gl.glDisable(Gl.GL_DEPTH_TEST);
			// Disable Depth Testing
			DrawFloor();
			// Draw The Floor (Draws To The Stencil Buffer)
			// We Only Want To Mark It In The Stencil Buffer
			Gl.glEnable(Gl.GL_DEPTH_TEST);
			// Enable Depth Testing
			Gl.glColorMask(1, 1, 1, 1);
			// Set Color Mask to TRUE, TRUE, TRUE, TRUE
			Gl.glStencilFunc(Gl.GL_EQUAL, 1, 1);
			// We Draw Only Where The Stencil Is 1
			// (I.E. Where The Floor Was Drawn)
			Gl.glStencilOp(Gl.GL_KEEP, Gl.GL_KEEP, Gl.GL_KEEP);
			// Don't Change The Stencil Buffer
			Gl.glEnable(Gl.GL_CLIP_PLANE0);
			// Enable Clip Plane For Removing Artifacts
			// (When The Object Crosses The Floor)
			Gl.glClipPlane(Gl.GL_CLIP_PLANE0, eqr);
			// Equation For Reflected Objects
			Gl.glPushMatrix();
			// Push The Matrix Onto The Stack
			Gl.glScalef(1, -1, 1);
			// Mirror Y Axis
			Gl.glLightfv(Gl.GL_LIGHT0, Gl.GL_POSITION, LightPos);
			// Set Up Light0
			Gl.glTranslatef(0, this.Height, 0);
			// Position The Object
			Gl.glRotatef(this.RotationX, 1, 0, 0);
			// Rotate Local Coordinate System On X Axis
			Gl.glRotatef(this.RotationY, 0, 1, 0);
			// Rotate Local Coordinate System On Y Axis
			DrawObject();
			// Draw The Sphere (Reflection)
			Gl.glPopMatrix();
			// Pop The Matrix Off The Stack
			Gl.glDisable(Gl.GL_CLIP_PLANE0);
			// Disable Clip Plane For Drawing The Floor
			Gl.glDisable(Gl.GL_STENCIL_TEST);
			// We Don't Need The Stencil Buffer Any More (Disable)
			Gl.glLightfv(Gl.GL_LIGHT0, Gl.GL_POSITION, LightPos);
			// Set Up Light0 Position
			Gl.glEnable(Gl.GL_BLEND);
			// Enable Blending (Otherwise The Reflected Object Wont Show)
			Gl.glDisable(Gl.GL_LIGHTING);
			// Since We Use Blending, We Disable Lighting
			Gl.glColor4f(1, 1, 1, 0.8f);
			// Set Color To White With 80% Alpha
			Gl.glBlendFunc(Gl.GL_SRC_ALPHA, Gl.GL_ONE_MINUS_SRC_ALPHA);
			// Blending Based On Source Alpha And 1 Minus Dest Alpha
			DrawFloor();
			// Draw The Floor To The Screen
			Gl.glEnable(Gl.GL_LIGHTING);
			// Enable Lighting
			Gl.glDisable(Gl.GL_BLEND);
			// Disable Blending
			Gl.glTranslatef(0, this.ballHeight, 0); 
			// Position The Ball At Proper Height
			Gl.glRotatef(this.RotationX, 1, 0, 0);
			// Rotate On The X Axis
			Gl.glRotatef(this.RotationY, 0, 1, 0);
			// Rotate On The Y Axis
			DrawObject();
			// Draw The Ball
			this.RotationX += xrotspeed; 
			// Update X Rotation Angle By xrotspeed
			this.RotationY += yrotspeed;
			// Update Y Rotation Angle By yrotspeed
			Gl.glFlush();
			// Flush The GL Pipeline
		}
		#endregion void DrawGLScene()

		#endregion Render

		#region Event Handlers

		private void KeyDown(object sender, KeyboardEventArgs e)
		{
			switch (e.Key) 
			{
				case Key.PageUp:
					this.ballHeight += 0.03f;
					break;
				case Key.PageDown:
					this.ballHeight -= 0.03f;
					break;
				case Key.UpArrow: 
					this.xrotspeed -= 0.08f;
					break;
				case Key.DownArrow:
					this.xrotspeed += 0.08f;
					break;
				case Key.RightArrow:
					this.yrotspeed += 0.08f;
					break;
				case Key.LeftArrow:
					this.yrotspeed -= 0.08f;
					break;
				case Key.A:
					zoom += 0.05f;
					break;
				case Key.Z:
					zoom -= 0.05f;
					break;				
			}
		}

		#endregion Event Handlers
	}
}