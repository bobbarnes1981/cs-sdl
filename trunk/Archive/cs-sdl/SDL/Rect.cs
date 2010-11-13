/*
 * BSD Licence:
 * Copyright (c) 2001, Jan Nockemann (jnockemann@gmx.net)
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
	[StructLayout(LayoutKind.Sequential)]
	public class Rect
	{
		public SDL_Rect rect;

		public Rect() {}
		
		public Rect(ushort w, ushort h) 
		{ 
			rect.w = w;
			rect.h = h;
		}
		public Rect(int w, int h) : this((ushort)w, (ushort) h)
		{ 
		}
		
		public Rect(short x, short y, ushort w, ushort h)
		{
			rect.x = x;
			rect.y = y;
			rect.w = w;
			rect.h = h;
		}
		public Rect(int x, int y, int w, int h)
			: this((short) x, (short) y, (ushort) w, (ushort) h)
		{
		}
		public Rect(int x, int y, uint w, uint h)
			: this((short) x, (short) y, (ushort) w, (ushort) h)
		{
		}
		
		public static explicit operator SDL_Rect(Rect r)
		{
			return r.rect;
		}
		
		public static explicit operator Rect(SDL_Rect r)
		{
			return new Rect(r);
		}
		
		public static explicit operator Rectangle(Rect r)
		{
			return new Rectangle((int)r.X, (int)r.Y, (int)r.Width, (int)r.Height);
		}
		
		public int X
		{
			get
			{
				return rect.x;
			}
			set
			{
				rect.x = (short) value;
			}
		}
		
		public int Y
		{
			get
			{
				return rect.y;
			}
			set
			{
				rect.y = (short) value;
			}
		}
		
		public uint Width
		{
			get
			{
				return rect.w;
			}
			set
			{
				rect.w = (ushort) value;
			}
		}
		
		public uint Height
		{
			get
			{
				return rect.h;
			}
			set
			{
				rect.h = (ushort) value;
			}
		}
		
		internal Rect(SDL_Rect r)
		{
			rect = r;
		}
		
		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendFormat("Rect ({0}, {1}) -> ({2}, {3})", X, Y, Width, Height);
			return sb.ToString();
		}
	}
}
