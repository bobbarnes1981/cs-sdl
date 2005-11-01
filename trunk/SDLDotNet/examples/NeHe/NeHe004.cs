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
	public class NeHe004 : NeHe001
	{
		/// <summary>
		/// 
		/// </summary>
		public new static string Title
		{
			get
			{
				return "Lesson 4: Rotation";
			}
		}
		// Angle For The Triangle ( NEW )
		private float rtri;                                              
		private float rquad;
    
		/// <summary>
		/// 
		/// </summary>
		public override void DrawGLScene()
		{
			Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);        
			// Clear Screen And Depth Buffer
			Gl.glLoadIdentity();                                                
			// Reset The Current Modelview Matrix
			Gl.glTranslatef(-1.5f, 0, -6);                                      
			// Move Left 1.5 Units And Into The Screen 6.0
			Gl.glRotatef(rtri, 0, 1, 0);                                        
			// Rotate The Triangle On The Y axis ( NEW )
			Gl.glBegin(Gl.GL_TRIANGLES);                                        
			// Drawing Using Triangles
			Gl.glColor3f(1, 0, 0);                                          
			// Set The Color To Red
			Gl.glVertex3f(0, 1, 0);                                         
			// Top
			Gl.glColor3f(0, 1, 0);                                          
			// Set The Color To Green
			Gl.glVertex3f(-1, -1, 0);                                       
			// Bottom Left
			Gl.glColor3f(0, 0, 1);                                          
			// Set The Color To Blue
			Gl.glVertex3f(1, -1, 0);                                        
			// Bottom Right
			Gl.glEnd();                                                         
			// Finished Drawing The Triangle
			Gl.glLoadIdentity();                                                
			// Reset The Current Modelview Matrix
			Gl.glTranslatef(1.5f, 0, -6);                                       
			// Move Right 1.5 Units And Into The Screen 6.0
			Gl.glRotatef(rquad, 1, 0, 0);                                       
			// Rotate The Quad On The X axis ( NEW )
			Gl.glColor3f(0.5f, 0.5f, 1);                                        
			// Set The Color To Blue One Time Only
			Gl.glBegin(Gl.GL_QUADS);                                            
			// Draw A Quad
			Gl.glVertex3f(-1, 1, 0);                                        
			// Top Left
			Gl.glVertex3f(1, 1, 0);                                         
			// Top Right
			Gl.glVertex3f(1, -1, 0);                                        
			// Bottom Right
			Gl.glVertex3f(-1, -1, 0);                                       
			// Bottom Left
			Gl.glEnd();                                                         
			// Done Drawing The Quad
			rtri += 0.2f;                                                       
			// Increase The Rotation Variable For The Triangle ( NEW )
			rquad -= 0.15f;                                                     
			// Decrease The Rotation Variable For The Quad ( NEW )
		}
	}
}