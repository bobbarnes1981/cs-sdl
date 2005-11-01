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
	public class NeHe014 : NeHe013 
	{
		/// <summary>
		/// 
		/// </summary>
		public new static string Title
		{
			get
			{
				return "Lesson 14: Outline Fonts";
			}
		}
//		// Private GDI Device Context
//		private IntPtr hDC;
		// Base Display List For The Font Set
//		private int fontbase;
		// Used To Rotate The Text
		private float rot;
		/// <summary>
		/// 
		/// </summary>
		public float Rotation
		{
			get
			{
				return rot;
			}
			set
			{
				rot = value;
			}
		}

		// Storage For Information About Our Outline Font Characters
		private Gdi.GLYPHMETRICSFLOAT[] gmf = new Gdi.GLYPHMETRICSFLOAT[256];

		/// <summary>
		/// 
		/// </summary>
		public NeHe014() 
		{
			Events.Quit += new QuitEventHandler(this.Quit);
		}

		/// <summary>
		/// 
		/// </summary>
		protected override void BuildFont() 
		{
			IntPtr font;                                                        
			// Windows Font ID
			this.FontBase = Gl.glGenLists(256);                                      
			// Storage For 256 Characters

			font = Gdi.CreateFont(                                              
				// Create The Font
				-12,                                                            
				// Height Of Font
				0,                                                              
				// Width Of Font
				0,                                                              
				// Angle Of Escapement
				0,                                                              
				// Orientation Angle
				Gdi.FW_BOLD,                                                    
				// Font Weight
				false,                                                          
				// Italic
				false,                                                          
				// Underline
				false,                                                          
				// Strikeout
				Gdi.ANSI_CHARSET,                                               
				// Character Set Identifier
				Gdi.OUT_TT_PRECIS,                                              
				// Output Precision
				Gdi.CLIP_DEFAULT_PRECIS,                                        
				// Clipping Precision
				Gdi.ANTIALIASED_QUALITY,                                        
				// Output Quality
				Gdi.FF_DONTCARE | Gdi.DEFAULT_PITCH,                            
				// Family And Pitch
				"Comic Sans MS");                                               
			// Font Name

			Gdi.SelectObject(this.Hdc, font);                                        
			// Selects The Font We Created
			Wgl.wglUseFontOutlines(
				this.Hdc,                                                            
				// Select The Current DC
				0,                                                              
				// Starting Character
				255,                                                            
				// Number Of Display Lists To Build
				this.FontBase,                                                       
				// Starting Display Lists
				0,                                                              
				// Deviation From The True Outlines
				0.2f,                                                           
				// Font Thickness In The Z Direction
				Wgl.WGL_FONT_POLYGONS,                                          
				// Use Polygons, Not Lines
				gmf);                                                           
			// Address Of Buffer To Recieve Data
		}

		/// <summary>
		/// 
		/// </summary>
		public override void DrawGLScene() 
		{
			Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);        
			// Clear Screen And Depth Buffer
			Gl.glLoadIdentity();                                                
			// Reset The Current Modelview Matrix
			Gl.glTranslatef(0, 0, -10);                                         
			// Move One Unit Into The Screen
			Gl.glRotatef(rot, 1, 0, 0);                                         
			// Rotate On The X Axis
			Gl.glRotatef(rot * 1.5f, 0, 1, 0);                                  
			// Rotate On The Y Axis
			Gl.glRotatef(rot * 1.4f, 0, 0, 1);                                  
			// Rotate On The Z Axis
			// Pulsing Colors Based On The Rotation
			Gl.glColor3f(1.0f * ((float) (Math.Cos(rot / 20.0f))), 1.0f * ((float) (Math.Sin(rot / 25.0f))), 1.0f - 0.5f * ((float) (Math.Cos(rot / 17.0f))));
			GlPrint(string.Format("NeHe - {0:0.00}", rot / 50));                
			// Print GL Text To The Screen
			rot += 0.5f;                                                        
			// Increase The Rotation Variable
		}

		#region GlPrint(string text)
		/// <summary>
		///     Custom GL "print" routine.
		/// </summary>
		/// <param name="text">
		///     The text to print.
		/// </param>
		protected override void GlPrint(string text) 
		{
			if(text == null || text.Length == 0) 
			{                              
				// If There's No Text
				return;                                                         
				// Do Nothing
			}
			float length = 0;                                                   
			// Used To Find The Length Of The Text
			char[] chars = text.ToCharArray();                                  
			// Holds Our String

			for(int loop = 0; loop < text.Length; loop++) 
			{                     // Loop To Find Text Length
				length += gmf[chars[loop]].gmfCellIncX;                         
				// Increase Length By Each Characters Width
			}

			Gl.glTranslatef(-length / 2, 0, 0);                                 
			// Center Our Text On The Screen
			Gl.glPushAttrib(Gl.GL_LIST_BIT);                                    
			// Pushes The Display List Bits
			Gl.glListBase(this.FontBase);                                        
			// Sets The Base Character to 0
			// .NET - can't call text, it's a string!
			byte [] textbytes = new byte[text.Length];
			for (int i = 0; i < text.Length; i++) textbytes[i] = (byte) text[i];
			Gl.glCallLists(text.Length, Gl.GL_UNSIGNED_BYTE, textbytes);         
			// Draws The Display List Text
			Gl.glPopAttrib();                                                   
			// Pops The Display List Bits
		}
		#endregion GlPrint(string text)

//		/// <summary>
//		/// 
//		/// </summary>
//		public override void InitGL()
//		{
//			Gl.glShadeModel(Gl.GL_SMOOTH);                                      
//			// Enable Smooth Shading
//			Gl.glClearColor(0, 0, 0, 0.5f);                                     
//			// Black Background
//			Gl.glClearDepth(1);                                                 
//			// Depth Buffer Setup
//			Gl.glEnable(Gl.GL_DEPTH_TEST);                                      
//			// Enables Depth Testing
//			Gl.glDepthFunc(Gl.GL_LEQUAL);                                       
//			// The Type Of Depth Testing To Do
//			Gl.glHint(Gl.GL_PERSPECTIVE_CORRECTION_HINT, Gl.GL_NICEST);         
//			// Really Nice Perspective Calculations
//			Gl.glEnable(Gl.GL_LIGHT0);                                          
//			// Enable Default Light (Quick And Dirty)
//			Gl.glEnable(Gl.GL_LIGHTING);                                        
//			// Enable Lighting
//			Gl.glEnable(Gl.GL_COLOR_MATERIAL);                                  
//			// Enable Coloring Of Material
//			BuildFont();                                                        
//			// Build The Font  
//		}

		private void Quit(object sender, QuitEventArgs e)
		{
			KillFont();
		}
	}
}
