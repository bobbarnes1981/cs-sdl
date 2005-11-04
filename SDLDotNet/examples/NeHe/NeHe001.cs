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
using System.Reflection;

using SdlDotNet;
using Tao.OpenGl;

namespace SdlDotNet.Examples
{
	/// <summary>
	/// 
	/// </summary>
	public class NeHe001
	{
		#region Variables

		//Width of screen
		const int width = 640;
		//Height of screen
		const int height = 480;
		// Bits per pixel of screen
		const int bpp = 16;
		// quit flag
		bool quit;
		// Surface to render on
		Surface screen;

		/// <summary>
		/// Width of window
		/// </summary>
		protected int Width
		{
			get
			{
				return width;
			}
		}

		/// <summary>
		/// Height of window
		/// </summary>
		protected int Height
		{
			get
			{
				return height;
			}
		}

		/// <summary>
		/// Bits per pixel of surface
		/// </summary>
		protected int BitsPerPixel
		{
			get
			{
				return bpp;
			}
		}

		/// <summary>
		/// Lesson title
		/// </summary>
		public static string Title
		{
			get
			{
				return "Lesson 01: Setting Up An OpenGL Window";
			}
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Basic constructor
		/// </summary>
		public NeHe001()
		{
			Initialize();
		}

		#endregion Constructors
    
		#region Lesson Setup
		/// <summary>
		/// Initializes methods common to all NeHe lessons
		/// </summary>
		protected void Initialize()
		{
			Events.KeyboardDown += new KeyboardEventHandler(this.KeyDown);
			Events.Quit += new QuitEventHandler(this.Quit);
			screen = Video.SetVideoModeWindowOpenGL(width, height, true);
			this.WindowAttributes();
		}

		/// <summary>
		/// Sets Window icon and caption
		/// </summary>
		protected void WindowAttributes()
		{
			Video.WindowIcon();
			Video.WindowCaption = 
				"SDL.NET - NeHe Lesson " + this.GetType().ToString().Substring(this.GetType().ToString().Length-3);
		}
    
		/// <summary>
		/// Resizes window
		/// </summary>
		protected virtual void Reshape()
		{
			this.Reshape(100.0F);
		}

		/// <summary>
		/// Resizes window
		/// </summary>
		/// <param name="distance"></param>
		protected virtual void Reshape(float distance)
		{
			Gl.glViewport(0, 0, width, height);
			Gl.glMatrixMode(Gl.GL_PROJECTION);
			Gl.glLoadIdentity();
			//  Calculate The Aspect Ratio Of The Window
			Glu.gluPerspective(45.0F, (width / (double)height), 0.1F, distance);
			Gl.glMatrixMode(Gl.GL_MODELVIEW);
			Gl.glLoadIdentity();
		}
    
		/// <summary>
		/// Initializes the OpenGL system
		/// </summary>
		protected virtual void InitGL()
		{
			InitGLBase();
		}

		/// <summary>
		/// Initializes methods common to all NeHe lessons
		/// </summary>
		protected virtual void InitGLBase()
		{
			// Enable Smooth Shading
			Gl.glShadeModel(Gl.GL_SMOOTH);
			// Black Background
			Gl.glClearColor(0.0F, 0.0F, 0.0F, 0.5F);
			// Depth Buffer Setup
			Gl.glClearDepth(1.0F);
			// Enables Depth Testing
			Gl.glEnable(Gl.GL_DEPTH_TEST);
			// The Type Of Depth Testing To Do
			Gl.glDepthFunc(Gl.GL_LEQUAL);
			// Really Nice Perspective Calculations
			Gl.glHint(Gl.GL_PERSPECTIVE_CORRECTION_HINT, Gl.GL_NICEST);
		}

		#endregion Lesson Setup
    
		#region Rendering
		/// <summary>
		/// Renders the scene
		/// </summary>
		protected virtual void DrawGLScene()
		{
			Gl.glClear((Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT));
			Gl.glLoadIdentity();
		}
		#endregion Rendering

		#region Event Handlers
    
		private void KeyDown(object sender, KeyboardEventArgs e)
		{
			switch (e.Key) 
			{
				case Key.Escape:
					quit = true;
					break;
				case Key.F1:
					if ((screen.FullScreen)) 
					{
						screen = Video.SetVideoModeWindowOpenGL(width, height, true);
						this.WindowAttributes();
					}
					else 
					{
						screen = Video.SetVideoModeOpenGL(width, height, bpp);
					}
					Reshape();
					break;
			}
		}
    
		private void Quit(object sender, QuitEventArgs e)
		{
			quit = true;
		}

		#endregion Event Handlers
    
		#region Run Loop
		/// <summary>
		/// Starts lesson
		/// </summary>
		public void Run()
		{
			Reshape();
			InitGL();
			while ((!quit)) 
			{
				while (Events.Poll()) 
				{
				}
				DrawGLScene();
				Video.GLSwapBuffers();
			}
			Video.Dispose(true);
		}
		#endregion Run Loop
	}
}