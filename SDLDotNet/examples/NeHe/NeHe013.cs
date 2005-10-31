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
using System.Drawing;

using Tao.OpenGl;
using Tao.Platform.Windows;

namespace SdlDotNet.Examples 
{
	class NeHe013 : NeHe001 
	{
		private static string title = "Lesson 13: Bitmap Fonts";
		public new static string Title
		{
			get
			{
				return title;
			}
		}
		private IntPtr hDC;
		private static int fontbase;                                            // Base Display List For The Font Set
		private static float cnt1;                                              // 1st Counter Used To Move Text & For Coloring
		private static float cnt2;                                              // 2nd Counter Used To Move Text & For Coloring

		public NeHe013() 
		{
			Events.Quit += new QuitEventHandler(Events_Quit);
		}

		private void BuildFont() 
		{
			fontbase = Gl.glGenLists(96);

			System.Drawing.Font font = new System.Drawing.Font(
				"Courier New", 
				17,
				FontStyle.Bold);
			
			IntPtr oldfont = Gdi.SelectObject(hDC, font.ToHfont());             // Selects The Font We Want
			Wgl.wglUseFontBitmaps(hDC, 32, 96, fontbase);                       // Builds 96 Characters Starting At Character 32
			Gdi.SelectObject(hDC, oldfont);                                     // Selects The Font We Want
		}

		public override void DrawGLScene() 
		{
			Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);        // Clear Screen And Depth Buffer
			Gl.glLoadIdentity();                                                // Reset The Current Modelview Matrix
			Gl.glTranslatef(0, 0, -1);                                          // Move One Unit Into The Screen
			// Pulsing Colors Based On Text Position
			Gl.glColor3f(1.0f * ((float) (Math.Cos(cnt1))), 1.0f * ((float) (Math.Sin(cnt2))), 1.0f - 0.5f* ((float) (Math.Cos(cnt1 + cnt2))));
			// Position The Text On The Screen
			Gl.glRasterPos2f(-0.45f + 0.05f * ((float) (Math.Cos(cnt1))), 0.32f * ((float) (Math.Sin(cnt2))));
			// Print GL Text To The Screen
			glPrint(string.Format("Active OpenGL Text With NeHe - {0:0.00}", cnt1));
			cnt1 += 0.051f;                                                     // Increase The First Counter
			cnt2 += 0.005f;                                                     // Increase The First Counter
			Video.GLSwapBuffers();
		}

		private void glPrint(string text) 
		{
			if(text == null || text.Length == 0) 
			{                              // If There's No Text
				return;                                                         // Do Nothing
			}
			Gl.glPushAttrib(Gl.GL_LIST_BIT);                                    // Pushes The Display List Bits
			Gl.glListBase(fontbase - 32);                                   // Sets The Base Character to 32
			// .NET -- we can't just pass text, we need to convert
			byte [] textbytes = new byte[text.Length];
			for (int i = 0; i < text.Length; i++)
				textbytes[i] = (byte) text[i];
			Gl.glCallLists(text.Length, Gl.GL_UNSIGNED_BYTE, textbytes);        // Draws The Display List Text
			Gl.glPopAttrib();                                                   // Pops The Display List Bits
		}

		public override void InitGL()
		{
			Gl.glShadeModel(Gl.GL_SMOOTH);                                      // Enable Smooth Shading
			Gl.glClearColor(0, 0, 0, 0.5f);                                     // Black Background
			Gl.glClearDepth(1);                                                 // Depth Buffer Setup
			Gl.glEnable(Gl.GL_DEPTH_TEST);                                      // Enables Depth Testing
			Gl.glDepthFunc(Gl.GL_LEQUAL);                                       // The Type Of Depth Testing To Do
			Gl.glHint(Gl.GL_PERSPECTIVE_CORRECTION_HINT, Gl.GL_NICEST);         // Really Nice Perspective Calculations
			hDC = User.GetDC(Video.WindowHandle);
			BuildFont();     
		}

		private void KillFont() 
		{
			Gl.glDeleteLists(fontbase, 96);                                     // Delete All 96 Characters
		}

		private void Events_Quit(object sender, QuitEventArgs e)
		{
			KillFont();
		}
	}
}
