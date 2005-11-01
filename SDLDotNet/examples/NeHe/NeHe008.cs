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
using System.Drawing.Imaging;
using System.Reflection;

using SdlDotNet;
using Tao.OpenGl;

namespace SdlDotNet.Examples
{
	class NeHe008 : NeHe007
	{  
		public new static string Title
		{
			get
			{
				return "Lesson 8: Blending";
			}
		}

        private bool blend; 
        private bool bp;                           
		// Which Filter To Use                     

		public NeHe008()
		{
			this.TextureName = "NeHe008.bmp";
			this.Texture = new int[3];
			Events.KeyboardDown += new KeyboardEventHandler(this.KeyDown);
			Keyboard.EnableKeyRepeat(150,50);
		}

		public override void InitGL()
		{
			base.InitGL ();
			Gl.glColor4f(1, 1, 1, 0.5f);                                        
			// Full Brightness.  50% Alpha
			Gl.glBlendFunc(Gl.GL_SRC_ALPHA, Gl.GL_ONE);                         
			// Set The Blending Function For Translucency
		}

		private void KeyDown(object sender, KeyboardEventArgs e)
		{
			switch (e.Key) 
			{
				case Key.L: 
					if (!this.Lp)
					{
						this.Lp = true;
						this.Light = !this.Light;
						if(!this.Light) 
						{
							Gl.glDisable(Gl.GL_LIGHTING);
						}
						else 
						{
							Gl.glEnable(Gl.GL_LIGHTING);
						}
					}
					else
					{ 
						this.Lp = false;
					}
					break;	
				case Key.F:
					if (!this.Fp)
					{
						this.Fp = true;
						this.Filter += 1;
						if(this.Filter > 2) 
						{
							this.Filter = 0;
						}
					}
					else
					{
						this.Fp = false;
					}
					break;
				case Key.PageUp:
					this.Z -= 0.02f;
					break;
				case Key.PageDown:
					this.Z += 0.02f;
					break;
				case Key.UpArrow: 
					this.XSpeed -= 0.01f;
					break;
				case Key.DownArrow:
					this.XSpeed += 0.01f;
					break;
				case Key.RightArrow:
					this.YSpeed += 0.01f;
					break;
				case Key.LeftArrow:
					this.YSpeed -= 0.01f;
					break;
				case Key.B:
					// Blending Code Starts Here
					if(!bp) 
					{
						bp = true;
						blend = !blend;
						if(blend) 
						{
							Gl.glEnable(Gl.GL_BLEND);                           
							// Turn Blending On
							Gl.glDisable(Gl.GL_DEPTH_TEST);                     
							// Turn Depth Testing Off
						}
						else 
						{
							Gl.glDisable(Gl.GL_BLEND);                          
							// Turn Blending Off
							Gl.glEnable(Gl.GL_DEPTH_TEST);                      
							// Turn Depth Testing On
						}
					}
					else
					{
						bp = false;
					}
					// Blending Code Ends Here
					break;
			}
		}
		protected bool Blend
		{
			get
			{
				return blend;
			}
			set
			{
				blend = value;
			}
		}
		protected bool Bp
		{
			get
			{
				return bp;
			}
			set
			{
				bp = value;
			}
		}

	}
}