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

using SdlDotNet;
using Tao.OpenGl;

namespace SdlDotNet.Examples
{
	/// <summary>
	/// 
	/// </summary>
	public class NeHe016 : NeHe010
	{
		/// <summary>
		/// 
		/// </summary>
		public new static string Title
		{
			get
			{
				return "Lesson 16: Cool Looking Fog";
			}
		}

		private float[] LightAmbient = {0.5f, 0.5f, 0.5f, 1.0f};
		private float[] LightDiffuse = {1.0f, 1.0f, 1.0f, 1.0f};
		private float[] LightPosition = {0.0f, 0.0f, 2.0f, 1.0f};

		private int[] fogMode = {Gl.GL_EXP, Gl.GL_EXP2, Gl.GL_LINEAR};	
		// Storage For Three Types Of Fog
		private int fogfilter = 0;										
		// Which Fog Mode To Use 
		private float[] fogColor = {0.5f, 0.5f, 0.5f, 1.0f};				
		// Fog Color

		/// <summary>
		/// 
		/// </summary>
		public NeHe016()
		{
			Events.KeyboardDown += new KeyboardEventHandler(Events_KeyboardDown);
			Keyboard.EnableKeyRepeat(30,30);
			this.Z = -5.0f;
			// Depth Into The Screen
			this.Texture = new int[3];
			this.TextureName = "NeHe016.bmp";
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
			Gl.glClearColor(0.5f, 0.5f, 0.5f, 1.0f);						
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

			Gl.glFogi(Gl.GL_FOG_MODE, (int)this.fogMode[this.fogfilter]);	
			// Fog Mode
			Gl.glFogfv(Gl.GL_FOG_COLOR, this.fogColor);						
			// Set Fog Color
			Gl.glFogf(Gl.GL_FOG_DENSITY, 0.35f);							
			// How Dense Will The Fog Be
			Gl.glHint(Gl.GL_FOG_HINT, Gl.GL_DONT_CARE);						
			// Fog Hint Value
			Gl.glFogf(Gl.GL_FOG_START, 1.0f);								
			// Fog Start Depth
			Gl.glFogf(Gl.GL_FOG_END, 5.0f);									
			// Fog End Depth
			Gl.glEnable(Gl.GL_FOG);											
			// Enables GL_FOG
			
			if (this.Light)	
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
			// Right face
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

			this.XRot += this.XSpeed;
			this.YRot += this.YSpeed;
		}

		private void Events_KeyboardDown(object sender, KeyboardEventArgs e)
		{
			switch(e.Key)
			{
				// L, F and G.
				case Key.L:
					this.Light = !this.Light;
					if (this.Light)
						Gl.glEnable(Gl.GL_LIGHTING);
					else
						Gl.glDisable(Gl.GL_LIGHTING);
					break;
				case Key.F:
					this.Filter = (this.Filter + 1) % 3;
					break;
				case Key.G:
					this.fogfilter = (this.fogfilter + 1) % 3;
					Gl.glFogi(Gl.GL_FOG_MODE, (int)this.fogMode[this.fogfilter]);	
					break;

				// Zoom in cube with Page Up/Down
				case Key.PageUp:
					this.Z -= 0.02f;
					break;
				case Key.PageDown:
					this.Z += 0.02f;
					break;

				// Rotate cube with arrows.
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
	}
}
