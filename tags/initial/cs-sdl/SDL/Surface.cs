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
using System.Drawing;
using System.Runtime.InteropServices;

namespace CsGL.SDL
{
	public unsafe class Surface : SDL_video, IDisposable
	{
		public Surface(SDL_Surface * s)
		{
			pointer = s;
			colorkey = null;
		}
		
		public void Dispose()
		{
			if (!Disposable) return;
			checkPointer();
			SDL_FreeSurface(pointer);
			pointer = null;
		}
		
		public SDL_Surface * ToSDL_Surface
		{
			get
			{
				return pointer;
			}
		}
			
		public void * Pixels
		{
			get
			{
				checkPointer();
				return pointer->pixels;
			}
		}
		
		public uint Flags
		{
			get
			{
				checkPointer();
				return pointer->flags;
			}
		}
		
		public uint Width
		{
			get
			{
				checkPointer();
				return pointer->w;
			}
		}
		
		public uint Height
		{
			get
			{
				checkPointer();
				return pointer->h;
			}
		}
		
		public ushort Pitch
		{
			get
			{
				checkPointer();
				return pointer->pitch;
			}
		}
		
		public Rect ClippingRect
		{
			get
			{
				checkPointer();
				return new Rect(pointer->clip_rect);
			}
		}
		
		public PixelFormat Format
		{
			get
			{
				checkPointer();
				return new PixelFormat(pointer->format);
			}
		}
		
		public Rect BoundingRect
		{
			get
			{
				return unchecked(new Rect((ushort)Width, (ushort)Height));
			}
		}
		
		public Size Extends
		{
			get
			{
				return new Size((int)Width, (int)Height);
			}
		}
		
		public void Blit(Surface src, short x, short y)
		{
			Rect dstR = new Rect((short)x, (short)y, (ushort)src.Width, (ushort)src.Height);
			
			if (SDL_BlitSurface(src.pointer, null, pointer, dstR) != 0)
				throw new SDLException();
		}
		
		public void Blit(Surface src, Rect srcR, short x, short y)
		{
			Rect dstR = new Rect(x, y, srcR.Width, srcR.Height);
			
			if (SDL_BlitSurface(src.pointer, srcR, pointer, dstR) != 0)
				throw new SDLException();
		}
		
		public void Blit(Surface src, Rect srcR, Rect dstR)
		{
			if (SDL_BlitSurface(src.pointer, srcR, pointer, dstR) != 0)
				throw new SDLException();
		}
	
		public virtual bool SetColors(SDL_Color[] colors)
		{
			return SetColors(colors, 0, colors.Length);
		}
	
		public virtual bool SetColors(SDL_Color[] colors, int firstcolor, int ncolors)
		{
			fixed(SDL_Color * c = (SDL_Color *)&colors[0])
				if(1 == SDL_SetColors(pointer, c, firstcolor, ncolors))
					return true;
			return false;
		}
		
		public RGB ColorKey
		{
			get
			{
				return colorkey;
			}
			set
			{
				if (value == null)
				{
				}
				else
				{
					SDL_SetColorKey(pointer, SDL_SRCCOLORKEY, Format.MapRGB(value));
				}
			}
		}
		
		/// <summary>this would swap buffer if needed (i.e. if surface is double
		/// buffered) otherwise simply update all the surface (Update).</summary>
		public virtual void Flip()
		{
			SDL_Flip(pointer);
		}
		public virtual void Update()
		{
			Update((Rect)null);
		}
		
		public virtual void Update(Rect r)
		{
			if (r == null)
				SDL_UpdateRect(pointer, 0, 0, 0, 0);
			else
				SDL_UpdateRect(pointer, r.X, r.Y, r.Width, r.Height);
		}
		
		public virtual void Update(Rect[] rects)
		{
			SDL_UpdateRects(pointer, rects.Length, rects);
		}
		
		public virtual void Lock()
		{
			if (SDL_LockSurface(pointer) != 0)
				throw new SDLException();
		}
		
		public virtual void Unlock()
		{
			SDL_UnlockSurface(pointer);
		}
		
		/// return a new surface 
		/// This function takes a surface and copies it to a new surface of 
		// the pixel format and colors of the video
		/// framebuffer, suitable for fast blitting onto
		/// the display surface. It calls SDL_ConvertSurface.
		public virtual Surface Copy()
		{
			SDL_Surface * s = SDL_DisplayFormat(pointer);
			
			if (s == null)
				throw new SDLException();
			return new Surface(s);
		}
		
		protected virtual bool Disposable
		{
			get
			{
				return true;
			}
		}
		
		protected void checkPointer()
		{
			if (pointer == (SDL_Surface *)null)
				throw new ObjectDisposedException("Pointer on SDL_Surface is null.");
		}
		
		protected Surface() {}
		
		protected SDL_Surface * pointer;
			
		private RGB colorkey;
		
	}
}
