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
		private const int width = 640;
		private const int height = 480;
		private const int bpp = 16;
		private bool quit;
		private Surface screen;
		#endregion

		/// <summary>
		/// 
		/// </summary>
		public NeHe001()
		{
			Initialize();
		}
    
		/// <summary>
		/// 
		/// </summary>
		public void Initialize()
		{
			Events.KeyboardDown += new KeyboardEventHandler(this.KeyDown);
			Events.Quit += new QuitEventHandler(this.Quit);
			screen = Video.SetVideoModeWindowOpenGL(width, height, true);
			this.WindowAttributes();
			Reshape();
		}

		/// <summary>
		/// 
		/// </summary>
		protected void WindowAttributes()
		{
			Video.WindowIcon();
			Video.WindowCaption = 
				"SDL.NET - NeHe Lesson " + this.GetType().ToString().Substring(this.GetType().ToString().Length-3);
		}
    
		/// <summary>
		/// 
		/// </summary>
		public virtual void Reshape()
		{
			this.Reshape(100.0F);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="distance"></param>
		public virtual void Reshape(float distance)
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
		/// 
		/// </summary>
		public virtual void InitGL()
		{
			Gl.glShadeModel(Gl.GL_SMOOTH);
			Gl.glClearColor(0.0F, 0.0F, 0.0F, 0.5F);
			Gl.glClearDepth(1.0F);
			Gl.glEnable(Gl.GL_DEPTH_TEST);
			Gl.glDepthFunc(Gl.GL_LEQUAL);
			Gl.glHint(Gl.GL_PERSPECTIVE_CORRECTION_HINT, Gl.GL_NICEST);
		}
    
		/// <summary>
		/// 
		/// </summary>
		public virtual void DrawGLScene()
		{
			Gl.glClear((Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT));
			Gl.glLoadIdentity();
		}	
    
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
    
		/// <summary>
		/// 
		/// </summary>
		public void Run()
		{
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

		/// <summary>
		/// 
		/// </summary>
		protected bool QuitFlag
		{
			get
			{
				return quit;
			}
			set
			{
				quit = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		protected Surface Screen
		{
			get
			{
				return screen;
			}
			set
			{
				screen = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		protected int Width
		{
			get
			{
				return width;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		protected int Height
		{
			get
			{
				return height;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		protected int Bpp
		{
			get
			{
				return bpp;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public static string Title
		{
			get
			{
				return "Lesson 1: Setting Up An OpenGL Window";
			}
		}
	}
}