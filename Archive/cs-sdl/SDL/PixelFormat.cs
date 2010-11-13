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
using System.Runtime.InteropServices;

namespace CsGL.SDL
{
	public unsafe class PixelFormat : SDL_video
	{
		public PixelFormat(SDL_PixelFormat *pf)
		{
			pointer = pf;
		}
	
		/// return the inner palette, loawer case for unsafe struct
		public SDL_Palette * palette { get { return pointer->palette; }}
		
		public byte BitsPerPixel
		{
			get
			{
				return pointer->BitsPerPixel;
			}
			set
			{
				pointer->BitsPerPixel = value;
			}
		}
		
		public byte BytesPerPixel
		{
			get
			{
				return pointer->BytesPerPixel;
			}
			set
			{
				pointer->BytesPerPixel = value;
			}
		}
		
		public byte RedLoss
		{
			get
			{
				return pointer->Rloss;
			}
		}
		
		public byte GreenLoss
		{
			get
			{
				return pointer->Gloss;
			}
		}
		
		public byte BlueLoss
		{
			get
			{
				return pointer->Bloss;
			}
		}
		
		public byte AlphaLoss
		{
			get
			{
				return pointer->Aloss;
			}
		}
		
		public byte RedShift
		{
			get
			{
				return pointer->Rshift;
			}
		}
		
		public byte GreenShift
		{
			get
			{
				return pointer->Gshift;
			}
		}
		
		public byte BlueShift
		{
			get
			{
				return pointer->Bshift;
			}
		}
		
		public byte AlphaShift
		{
			get
			{
				return pointer->Aloss;
			}
		}
		
		public uint RedMask
		{
			get
			{
				return pointer->Rmask;
			}
		}
		
		public uint GreenMask
		{
			get
			{
				return pointer->Gmask;
			}
		}
		
		public uint BlueMask
		{
			get
			{
				return pointer->Bmask;
			}
		}
		
		public uint AlphaMask
		{
			get
			{
				return pointer->Amask;
			}
		}
		
		public uint ColorKey
		{
			get
			{
				return pointer->colorkey;
			}
		}
		
		public RGB ColorKeyAsRGB()
		{
			return GetRGB(pointer->colorkey);
		}
		
		public byte Alpha
		{
			get
			{
				return pointer->alpha;
			}
		}
		
		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("SDL_PixelFormat {");
			sb.Append("\n\tBits per pixel : ");
			sb.Append(BitsPerPixel);
			sb.Append("\n\tBytes per pixel : ");
			sb.Append(BytesPerPixel);
			sb.Append("\n\tRGBA loss : ");
			sb.AppendFormat("{0}  {1}  {2}  {3}", 
				StringUtil.ToHexa(RedLoss), StringUtil.ToHexa(GreenLoss),
				StringUtil.ToHexa(BlueLoss), StringUtil.ToHexa(AlphaLoss));
			sb.Append("\n\tRGBA shift : ");
			sb.AppendFormat("{0}  {1}  {2}  {3}", 
				StringUtil.ToHexa(RedShift), StringUtil.ToHexa(GreenShift),
				StringUtil.ToHexa(BlueShift), StringUtil.ToHexa(AlphaShift));
			sb.Append("\n\tRGBA mask : ");
			sb.AppendFormat("{0}  {1}  {2}  {3}", 
				StringUtil.ToHexa(RedMask), StringUtil.ToHexa(GreenMask),
				StringUtil.ToHexa(BlueMask), StringUtil.ToHexa(AlphaMask));
			sb.Append("\n\tColor key : ");
			sb.Append(ColorKey);
			sb.Append("\n\talpha : ");
			sb.Append(Alpha);
		
			if (pointer->palette == null) sb.Append("\n\tno palette");
			else
			{
				sb.Append("\n\t");
				sb.Append(pointer->palette->ncolors);
				sb.Append(" colors palette");
			}
			
			sb.Append("\n}");
		
			return sb.ToString();
		}
		
		public uint MapRGB(RGB rgb)
		{
			return SDL_MapRGB(pointer, rgb.r, rgb.g, rgb.b);
		}
		public uint MapRGB(byte r, byte g, byte b)
		{
			return SDL_MapRGB(pointer, r, g, b);
		}
		
		public uint MapRGBA(RGBA rgba)
		{
			return SDL_MapRGBA(pointer, rgba.r, rgba.g, rgba.b, rgba.a);
		}
		public uint MapRGBA(byte r, byte g, byte b, byte a)
		{
			return SDL_MapRGBA(pointer, r, g, b, a);
		}
		
		public RGB GetRGB(uint pixel)
		{
			byte r, g, b;
			
			SDL_GetRGB(pixel, pointer, &r, &g, &b);
			
			return new RGB(r, g, b);
		}
		
		public RGBA GetRGBA(uint pixel)
		{
			byte r, g, b, a;
			
			SDL_GetRGBA(pixel, pointer, &r, &g, &b, &a);
			
			return new RGBA(r, g, b, a);
		}
		
		public SDL_PixelFormat * Pointer
		{
			get
			{
				return pointer;
			}
		}
		
		private SDL_PixelFormat * pointer;
	}
}
