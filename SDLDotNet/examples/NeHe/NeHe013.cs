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
	/// <summary>
	/// 
	/// </summary>
	public class NeHe013 : NeHe012 
	{
		/// <summary>
		/// 
		/// </summary>
		public new static string Title
		{
			get
			{
				return "Lesson 13: Bitmap Fonts";
			}
		}
		private IntPtr hDC;
		/// <summary>
		/// 
		/// </summary>
		public IntPtr Hdc
		{
			get
			{
				return hDC;
			}
			set
			{
				hDC = value;
			}
	}
		private int fontBase;
		// Base Display List For The Font Set
		/// <summary>
		/// 
		/// </summary>
		public int FontBase
		{
			get
			{
				return fontBase;
			}
			set
			{
				fontBase = value;
			}
		}
		private float cnt1;
		// 1st Counter Used To Move Text & For Coloring
		private float cnt2;
		// 2nd Counter Used To Move Text & For Coloring

		/// <summary>
		/// 
		/// </summary>
		public float Cnt1
		{
			get
			{
				return cnt1;
			}
			set
			{
				cnt1 = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public float Cnt2
		{
			get
			{
				return cnt2;
			}
			set
			{
				cnt2 = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public NeHe013() 
		{
			Events.Quit += new QuitEventHandler(this.Quit);
		}

		/// <summary>
		/// 
		/// </summary>
		protected virtual void BuildFont() 
		{
			fontBase = Gl.glGenLists(96);

			System.Drawing.Font font = new System.Drawing.Font(
				"Courier New", 
				17,
				FontStyle.Bold);
			
			IntPtr oldfont = Gdi.SelectObject(hDC, font.ToHfont());
			// Selects The Font We Want
			Wgl.wglUseFontBitmaps(hDC, 32, 96, fontBase);
			// Builds 96 Characters Starting At Character 32
			Gdi.SelectObject(hDC, oldfont);
			// Selects The Font We Want
		}

		/// <summary>
		/// 
		/// </summary>
		protected override void DrawGLScene() 
		{
			Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
			// Clear Screen And Depth Buffer
			Gl.glLoadIdentity();
			// Reset The Current Modelview Matrix
			Gl.glTranslatef(0, 0, -1);
			// Move One Unit Into The Screen
			// Pulsing Colors Based On Text Position
			Gl.glColor3f(1.0f * ((float) (Math.Cos(Cnt1))), 1.0f * ((float) (Math.Sin(cnt2))), 1.0f - 0.5f* ((float) (Math.Cos(Cnt1 + cnt2))));

			// Position The Text On The Screen
			Gl.glRasterPos2f(-0.45f + 0.05f * ((float) (Math.Cos(Cnt1))), 0.32f * ((float) (Math.Sin(cnt2))));
			// Print GL Text To The Screen
			GlPrint(string.Format("Active OpenGL Text With NeHe - {0:0.00}", Cnt1));
			Cnt1 += 0.051f;
			// Increase The First Counter
			cnt2 += 0.005f;
			// Increase The First Counter
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="text"></param>
		protected virtual void GlPrint(string text) 
		{
			if(text == null || text.Length == 0) 
			{
				// If There's No Text
				return;
				// Do Nothing
			}
			Gl.glPushAttrib(Gl.GL_LIST_BIT);
			// Pushes The Display List Bits
			Gl.glListBase(fontBase - 32);
			// Sets The Base Character to 32
			// .NET -- we can't just pass text, we need to convert
			byte [] textbytes = new byte[text.Length];

			for (int i = 0; i < text.Length; i++)
				textbytes[i] = (byte) text[i];

			Gl.glCallLists(text.Length, Gl.GL_UNSIGNED_BYTE, textbytes);
			// Draws The Display List Text
			Gl.glPopAttrib();
			// Pops The Display List Bits
		}

		/// <summary>
		/// 
		/// </summary>
		protected override void InitGL()
		{
			base.InitGL();
			hDC = User.GetDC(Video.WindowHandle);
			BuildFont();     
		}

		/// <summary>
		/// 
		/// </summary>
		protected virtual void KillFont() 
		{
			Gl.glDeleteLists(fontBase, 96);
			// Delete All 96 Characters
		}

		private void Quit(object sender, QuitEventArgs e)
		{
			KillFont();
		}
	}
}
