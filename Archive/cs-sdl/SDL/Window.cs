/*
 * BSD Licence:
 * Copyright (c) 2001, Lloyd Dupont (lloyd@galador.net)
 * <ORGANIZATION> 
 * All rights reserved.
 * 
 *
 * Redistribution and use in source and binary forms, with or without 
 * modification, are permitted provided that the following conditions are met:
 *
 * 1. Redistributions of source code must retain the above copyright notice, 
 * this list of conditions and the following disclaimer.
 * 2. Redistributions in binary form must reproduce the above copyright 
 * notice, this list of conditions and the following disclaimer in the 
 * documentation and/or other materials provided with the distribution.
 * 3. Neither the name of the <ORGANIZATION> nor the names of its contributors
 * may be used to endorse or promote products derived from this software
 * without specific prior written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
 * AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
 * IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
 * ARE DISCLAIMED. IN NO EVENT SHALL THE REGENTS OR CONTRIBUTORS BE LIABLE FOR
 * ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
 * DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
 * SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER
 * CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT 
 * LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY
 * OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH
 * DAMAGE.
 */
using System;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace CsGL.SDL
{
	public unsafe class Window : Surface
	{
		public static Window Top
		{
			get
			{
				return windows;
			}
		}
		
		static Window()
		{
			SDL.SDL_Init(SDL.SDL_INIT_EVERYTHING);
		}
		
		/// <summary>
		/// Create a new SDL top level window (it could just have one in any SDL program, now)
		/// </summary>
		public static Window Create(int width, int height, int bpp, uint flags)
		{
			SDL_Surface * s = SDL_video.SDL_SetVideoMode(width, height, bpp, flags);
			
			if (s == null)
				throw new SDLException();
			windows = new Window(s);
			return windows;
		}

		/// <summary>this would swap buffer if needed (i.e. if surface is double
		/// buffered) otherwise simply update all the surface (Update).
		/// this method will swap OpenGL buffer if OpenGL is activated.
		/// </summary>
		public override void Flip()
		{
			if (isOpenGLSurface)
			{
				SDL_video.SDL_GL_SwapBuffers();
			}
			else if (SDL_Flip(pointer) != 0)
				throw new SDLException();
		}

		public string Title
		{
			set
			{
				SDL_video.SDL_WM_SetCaption(value, null);
			}
			get
			{
				string title, icon=null;
				SDL_video.SDL_WM_GetCaption(out title, out icon);
				return title;
			}
		}
		
		internal readonly bool isOpenGLSurface = false;
		
		protected Window(SDL_Surface * s) : base(s)
		{
			isOpenGLSurface = (s->flags & (uint) SDL_video.SDL_OPENGL) != 0;
		}
		
		protected override bool Disposable
		{
			get
			{
				return false;
			}
		}
		
		private static Window windows;

		/// <summary>
		/// OpenGL flags for new window.
		/// invoke it before Creating the window
		/// </summary>
		public static readonly GLAttrib GL = new GLAttrib();

		/// <summary>
		/// get extensive video information about current video mode
		/// </summary>
		public static VideoInfo VideoInfo
		{
			get { return new VideoInfo(SDL_GetVideoInfo()); }
		}

		/// <summary>
		/// get available video mode for a given surface type
		/// and actual pixel format. <BR/>
		/// return <B>null</B> if any format is acceptable
		/// </summary>
		public static SDL_Rect[] ListMode(uint flags)
		{
			return ListMode(flags, (SDL_PixelFormat *)null);
		}
		/// <summary>
		/// get available video mode for a given surface type
		/// and a given pixel format. <BR/>
		/// return <B>null</B> if any format is acceptable
		/// </summary>
		public static SDL_Rect[] ListMode(uint flags, PixelFormat pix)
		{
			return ListMode(flags, pix.Pointer);
		}
		internal static SDL_Rect[] ListMode(uint flags, SDL_PixelFormat * pix)
		{
			SDL_Rect ** modes = SDL_ListModes(pix, flags);
			if(modes == (SDL_Rect **) 0)
				throw new SDLException("no accessible video mode");
			if(modes == (SDL_Rect **)-1)
				return null;
			int i,n;
			for(i=0; true; i++)
				if(modes[i] == null)
					break;
			n = i;
			SDL_Rect[] ret = new SDL_Rect[n];
			for(i=0; i<n; i++)
				ret[i] = * modes[i];
			return ret;
		}
	}
	
	public unsafe class GLAttrib
	{
		internal GLAttrib() {}

		public int this[SDL_GLattr index] 
		{
			set { Window.SDL_GL_SetAttribute(index, value); }
			get {
				int a;
				Window.SDL_GL_GetAttribute(index, &a);
				return a;
			}
		}
	}
}
