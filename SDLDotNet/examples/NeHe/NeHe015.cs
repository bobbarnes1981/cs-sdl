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
using System.Drawing.Imaging;
using System.IO;

using Tao.OpenGl;
using Tao.Platform.Windows;

namespace SdlDotNet.Examples 
{
	/// <summary>
	/// 
	/// </summary>
	public class NeHe015 : NeHe014
	{
		/// <summary>
		/// Lesson Title
		/// </summary>
		public new static string Title
		{
			get
			{
				return "Lesson 15: Texture Mapped Outline Fonts";
			}
		}
		//		// Private GDI Device Context
		//		private IntPtr hDC;
		// Base Display List For The Font Set
		//		private int fontbase;
		// Used To Rotate The Text
		//		private float rot;
		// Storage For Information About Our Outline Font Characters
		private Gdi.GLYPHMETRICSFLOAT[] gmf = new Gdi.GLYPHMETRICSFLOAT[256];

		/// <summary>
		/// 
		/// </summary>
		public NeHe015() 
		{
			// One Texture Map
			this.Texture = new int[1];
			this.TextureName = new string[1];
			this.TextureName[0] = "NeHe015.bmp";
			Events.Quit += new QuitEventHandler(this.Quit);
		}

		/// <summary>
		/// 
		/// </summary>
		protected override void InitGL()
		{
			LoadGLTextures();
			base.InitGL ();
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
				Gdi.SYMBOL_CHARSET,   
				// Character Set Identifier
				Gdi.OUT_TT_PRECIS, 
				// Output Precision
				Gdi.CLIP_DEFAULT_PRECIS,
				// Clipping Precision
				Gdi.ANTIALIASED_QUALITY,
				// Output Quality
				Gdi.FF_DONTCARE | Gdi.DEFAULT_PITCH, 
				// Family And Pitch
				"Wingdings");  
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
		/// Renders the scene
		/// </summary>
		protected override void DrawGLScene() 
		{
			Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
			// Clear Screen And Depth Buffer
			Gl.glLoadIdentity();   
			// Reset The Current Modelview Matrix
			Gl.glTranslatef(1.1f * ((float) (Math.Cos(this.Rotation / 16.0f))), 0.8f * ((float) (Math.Sin(this.Rotation / 20.0f))), -3.0f);
			Gl.glRotatef(this.Rotation, 1, 0, 0);
			// Rotate On The X Axis
			Gl.glRotatef(this.Rotation * 1.2f, 0, 1, 0);   
			// Rotate On The Y Axis
			Gl.glRotatef(this.Rotation * 1.4f, 0, 0, 1);   
			// Rotate On The Z Axis
			Gl.glTranslatef(-0.35f, -0.35f, 0.1f);   
			// Center On X, Y, Z Axis
			GlPrint("N");  
			// Draw A Skull And Crossbones Symbol
			this.Rotation += 0.1f;   
			// Increase The Rotation Variable
		}

		/// <summary>
		/// 
		/// </summary>
		protected override void LoadGLTextures()
		{
			if (File.Exists(this.DataDirectory + this.TextureName))
			{																	
				this.FilePath = "";															
			} 
			// Status Indicator
			Bitmap[] textureImage = new Bitmap[this.TextureName.Length];   
			// Create Storage Space For The Texture

			textureImage[0] = new Bitmap(this.FilePath + this.DataDirectory + this.TextureName[0]);
			// Load The Bitmap
			// Check For Errors, If Bitmap's Not Found, Quit
			if(textureImage[0] != null) 
			{
				Gl.glGenTextures(this.Texture.Length, this.Texture);

				textureImage[0].RotateFlip(RotateFlipType.RotateNoneFlipY); 
				// Flip The Bitmap Along The Y-Axis
				// Rectangle For Locking The Bitmap In Memory
				Rectangle rectangle = new Rectangle(0, 0, textureImage[0].Width, textureImage[0].Height);
				// Get The Bitmap's Pixel Data From The Locked Bitmap
				BitmapData bitmapData = textureImage[0].LockBits(rectangle, ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);

				// Create Linear Filtered Texture
				Gl.glBindTexture(Gl.GL_TEXTURE_2D, this.Texture[0]);
				Glu.gluBuild2DMipmaps(Gl.GL_TEXTURE_2D, Gl.GL_RGB8, textureImage[0].Width, textureImage[0].Height, Gl.GL_RGB, Gl.GL_UNSIGNED_BYTE, bitmapData.Scan0);
				Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MAG_FILTER, Gl.GL_LINEAR);
				Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MIN_FILTER, Gl.GL_LINEAR_MIPMAP_NEAREST);
				Gl.glTexGeni(Gl.GL_S, Gl.GL_TEXTURE_GEN_MODE, Gl.GL_OBJECT_LINEAR);
				Gl.glTexGeni(Gl.GL_T, Gl.GL_TEXTURE_GEN_MODE, Gl.GL_OBJECT_LINEAR);
				Gl.glEnable(Gl.GL_TEXTURE_GEN_S);
				Gl.glEnable(Gl.GL_TEXTURE_GEN_T);
				
				if(textureImage[0] != null) 
				{
					// If Texture Exists
					textureImage[0].UnlockBits(bitmapData); 
					// Unlock The Pixel Data From Memory
					textureImage[0].Dispose();   
					// Dispose The Bitmap
				}
			}
		}

		#region GlPrint(string text)
		/// <summary>
		/// Custom GL "print" routine.
		/// </summary>
		/// <param name="text">
		/// The text to print.
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
			{ // Loop To Find Text Length
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

		private void Quit(object sender, QuitEventArgs e)
		{
			KillFont();
		}
	}
}
