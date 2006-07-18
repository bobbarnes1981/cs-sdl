/*
 * $RCSfile$
 * Copyright (C) 2006 David Hudson (jendave@yahoo.com)
 *
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later version.
 * 
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 * 
 * You should have received a copy of the GNU Lesser General Public
 * License along with this library; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
 */

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using SdlDotNet;
using SdlDotNet.OpenGl;
using Tao.OpenGl;

namespace SdlDotNet.Examples.OpenGlFont
{
	/// <summary>
	/// 
	/// </summary>
	public class OpenGlFont : IDisposable
	{
		int width = 640;
		int height = 480;
		string dataDirectory = @"Data/";
		// Path to Data directory
		string filePath = @"../../";
		string fontName = "FreeSans.ttf";
		string phrase1 = "Hello world! ";
		string phrase2 = "This is a Truetype font ";
		string phrase3 = "On an OpenGl Surface ";
		Font font;
		// Angle For The Triangle ( NEW )
		float rtri;
		// Angle For The Quad ( NEW ) 
		float rquad;

		#region Run Loop
		/// <summary>
		/// Starts lesson
		/// </summary>
		public void Run()
		{
			Initialize();
			InitGL();
			Events.Run();
		}

		#endregion Run Loop

		/// <summary>
		/// 
		/// </summary>
		public void Initialize()
		{
			if (File.Exists(dataDirectory + "FreeSans.ttf"))
			{
				filePath = "";
			}
			Video.WindowIcon();
			Video.WindowCaption = "SDL.NET - OpenGlFont Example";
			Video.SetVideoModeWindowOpenGL(this.width, this.height);
			Events.Quit += new QuitEventHandler(this.Quit);
			Events.Tick += new TickEventHandler(this.Tick);
			font = new Font(filePath + dataDirectory + fontName, 18);
			Video.GLDoubleBufferEnabled = true;
		}

		[STAThread]
		static void Main()
		{
			OpenGlFont openGlFont = new OpenGlFont();
			openGlFont.Run();
		}

		/// <summary>
		/// Initializes the OpenGL system
		/// </summary>
		protected void InitGL()
		{
			// Reset The Current Viewport
			Gl.glViewport(0, 0, width, height);
			// Select The Projection Matrix
			Gl.glMatrixMode(Gl.GL_PROJECTION);
			// Reset The Projection Matrix
			Gl.glLoadIdentity();
			Gl.glOrtho( -2.0, 2.0, -2.0, 2.0, -20.0, 20.0 );
			// Calculate The Aspect Ratio Of The Window
			//Glu.gluPerspective(45.0F, (width / (float)height), 0.1F, 100.0F);
			// Select The Modelview Matrix
			Gl.glMatrixMode(Gl.GL_MODELVIEW);
			// Reset The Modelview Matrix
			Gl.glLoadIdentity();
//			// Enable Texture Mapping ( NEW )
//			Gl.glEnable(Gl.GL_TEXTURE_2D);
			// Enable Smooth Shading
			Gl.glShadeModel(Gl.GL_SMOOTH);
//			// Black Background
//			Gl.glClearColor(0.0F, 0.0F, 0.0F, 0.5F);
//			// Depth Buffer Setup
//			Gl.glClearDepth(1.0F);
			// Enables Depth Testing
			Gl.glEnable(Gl.GL_DEPTH_TEST);
//			// The Type Of Depth Testing To Do
			Gl.glDepthFunc(Gl.GL_LEQUAL);
			// Really Nice Perspective Calculations
			Gl.glHint(Gl.GL_PERSPECTIVE_CORRECTION_HINT, Gl.GL_NICEST);
		}

		int i;
		SurfaceGl surfaceGl = new SurfaceGl();

		#region void DrawGLScene

		/// <summary>
		/// Renders the scene
		/// </summary>
		protected void DrawGLScene()
		{
			// Clear Screen And Depth Buffer
			//Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
			// Reset The Current Modelview Matrix
			//Gl.glLoadIdentity();   
//			// Move Left 1.5 Units And Into The Screen 6.0
//			Gl.glTranslatef(-1.5f, 0, -6);   
//			// Rotate The Triangle On The Y axis ( NEW )
//			Gl.glRotatef(rtri, 0, 1, 0);
//			// Drawing Using Triangles
//			Gl.glBegin(Gl.GL_TRIANGLES);
//			// Red
//			Gl.glColor3f(1, 0, 0); 
//			// Top Of Triangle (Front)
//			Gl.glVertex3f(0, 1, 0);
//			// Green
//			Gl.glColor3f(0, 1, 0); 
//			// Left Of Triangle (Front)
//			Gl.glVertex3f(-1, -1, 1);
//			// Blue
//			Gl.glColor3f(0, 0, 1); 
//			// Right Of Triangle (Front)
//			Gl.glVertex3f(1, -1, 1);
//			// Red
//			Gl.glColor3f(1, 0, 0); 
//			// Top Of Triangle (Right)
//			Gl.glVertex3f(0, 1, 0);
//			// Blue
//			Gl.glColor3f(0, 0, 1); 
//			// Left Of Triangle (Right)
//			Gl.glVertex3f(1, -1, 1);
//			// Green
//			Gl.glColor3f(0, 1, 0); 
//			// Right Of Triangle (Right)
//			Gl.glVertex3f(1, -1, -1);
//			// Red
//			Gl.glColor3f(1, 0, 0); 
//			// Top Of Triangle (Back)
//			Gl.glVertex3f(0, 1, 0);
//			// Green
//			Gl.glColor3f(0, 1, 0); 
//			// Left Of Triangle (Back)
//			Gl.glVertex3f(1, -1, -1);
//			// Blue
//			Gl.glColor3f(0, 0, 1); 
//			// Right Of Triangle (Back)
//			Gl.glVertex3f(-1, -1, -1);   
//			// Red
//			Gl.glColor3f(1, 0, 0); 
//			// Top Of Triangle (Left)
//			Gl.glVertex3f(0, 1, 0);
//			// Blue
//			Gl.glColor3f(0, 0, 1); 
//			// Left Of Triangle (Left)
//			Gl.glVertex3f(-1, -1, -1);   
//			// Green
//			Gl.glColor3f(0, 1, 0); 
//			// Right Of Triangle (Left)
//			Gl.glVertex3f(-1, -1, 1);
//			// Finished Drawing The Triangle
//			Gl.glEnd();
			// Reset The Current Modelview Matrix
			//Gl.glLoadIdentity();   
			// Move Right 1.5 Units And Into The Screen 7.0
			//Gl.glTranslatef(1.5f, 0, -7);
			// Rotate The Quad On The X, Y, and Z Axis ( NEW )
			Gl.glRotatef(rquad, 1, 1, 1);
			// Set The Color To Blue One Time Only
			Gl.glColor3f(0.5f, 0.5f, 1);
			// Draw A Quad
			Gl.glBegin(Gl.GL_QUADS);   
			// Set The Color To Green
			Gl.glColor3f(0, 1, 0); 
			// Top Right Of The Quad (Top)
			Gl.glVertex3f(1, 1, -1);
			// Top Left Of The Quad (Top)
			Gl.glVertex3f(-1, 1, -1);
			// Bottom Left Of The Quad (Top)
			Gl.glVertex3f(-1, 1, 1);
			// Bottom Right Of The Quad (Top)
			Gl.glVertex3f(1, 1, 1);
			// Set The Color To Orange
			Gl.glColor3f(1, 0.5f, 0);
			// Top Right Of The Quad (Bottom)
			Gl.glVertex3f(1, -1, 1);
			// Top Left Of The Quad (Bottom)
			Gl.glVertex3f(-1, -1, 1);
			// Bottom Left Of The Quad (Bottom)
			Gl.glVertex3f(-1, -1, -1);   
			// Bottom Right Of The Quad (Bottom)
			Gl.glVertex3f(1, -1, -1);
			// Set The Color To Red
			Gl.glColor3f(1, 0, 0); 
			// Top Right Of The Quad (Front)
			Gl.glVertex3f(1, 1, 1);
			// Top Left Of The Quad (Front)
			Gl.glVertex3f(-1, 1, 1);
			// Bottom Left Of The Quad (Front)
			Gl.glVertex3f(-1, -1, 1);
			// Bottom Right Of The Quad (Front)
			Gl.glVertex3f(1, -1, 1);
			// Set The Color To Yellow
			Gl.glColor3f(1, 1, 0); 
			// Top Right Of The Quad (Back)
			Gl.glVertex3f(1, -1, -1);
			// Top Left Of The Quad (Back)
			Gl.glVertex3f(-1, -1, -1);   
			// Bottom Left Of The Quad (Back)
			Gl.glVertex3f(-1, 1, -1);
			// Bottom Right Of The Quad (Back)
			Gl.glVertex3f(1, 1, -1);
			// Set The Color To Blue
			Gl.glColor3f(0, 0, 1); 
			// Top Right Of The Quad (Left)
			Gl.glVertex3f(-1, 1, 1);
			// Top Left Of The Quad (Left)
			Gl.glVertex3f(-1, 1, -1);
			// Bottom Left Of The Quad (Left)
			Gl.glVertex3f(-1, -1, -1);   
			// Bottom Right Of The Quad (Left)
			Gl.glVertex3f(-1, -1, 1);
			
			// Set The Color To Violet
			Gl.glColor3f(1, 0, 1); 
			// Top Right Of The Quad (Right)
			Gl.glVertex3f(1, 1, -1);
			// Top Left Of The Quad (Right)
			Gl.glVertex3f(1, 1, 1);
			// Bottom Left Of The Quad (Right)
			Gl.glVertex3f(1, -1, 1);
			// Bottom Right Of The Quad (Right)
			Gl.glVertex3f(1, -1, -1);
			// Done Drawing The Quad
			Gl.glEnd();
			Gl.glMatrixMode(Gl.GL_MODELVIEW);
			
			// Increase The Rotation Variable For The Triangle ( NEW )
			rtri += 0.2f;
			// Decrease The Rotation Variable For The Quad ( NEW )
			rquad -= 0.15f;
		}

		#endregion void DrawGLScene

		private void Tick(object sender, TickEventArgs e)
		{
			Gl.glClearColor(0.0F, 0.0F, 0.0F, 1.0F);
			Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
			DrawGLScene();
			//Gl.glPushMatrix();
			surfaceGl.Surface = font.Render(phrase1 + i++, Color.White);
			surfaceGl.Draw(new Point(0, 0));
			//Gl.glPopMatrix();
			surfaceGl.Surface = font.Render(phrase2 + i++, Color.White);
			surfaceGl.Draw(new Point(100,100));
			surfaceGl.Surface = font.Render(phrase3 + i++, Color.White);
			surfaceGl.Draw(new Point(200, 200));
			
			Video.GLSwapBuffers();
		}

		private void Quit(object sender, QuitEventArgs e)
		{
			Events.QuitApplication();
		}

		#region IDisposable Members

		private bool disposed;

		/// <summary>
		/// Destroy object
		/// </summary>
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Destroy object
		/// </summary>
		public void Close() 
		{
			Dispose();
		}

		/// <summary>
		/// Destroy object
		/// </summary>
		~OpenGlFont() 
		{
			Dispose(false);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="disposing"></param>
		protected virtual void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				if (disposing)
				{
					if (this.font != null)
					{
						this.font.Dispose();
						this.font = null;
					}
				}
				this.disposed = true;
			}
		}

		#endregion
	}
}
