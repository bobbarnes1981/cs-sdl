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

namespace CsGL.SDL
{
	public unsafe class VideoInfo : SDL_video
	{
		internal SDL_VideoInfo * pointer;
		
		public VideoInfo(SDL_VideoInfo * vi) { pointer = vi; }
		public SDL_VideoInfo * Pointer { get { return pointer; } }
	
		public PixelFormat PixelFormat { 
			get { return new PixelFormat(pointer->vfmt); } 
		}

		public bool hw_available { get { return 1 == pointer->hw_available; }}
		//public uint wm_available why 0-3 ?
		public bool blit_hw { get { return 1 == pointer->blit_hw; }}
		public bool blit_hw_CC { get { return 1 == pointer->blit_hw_CC; }}	
		public bool blit_hw_A { get { return 1 == pointer->blit_hw_A; }}		
		public bool blit_sw { get { return 1 == pointer->blit_sw; }}		
		public bool blit_sw_CC { get { return 1 == pointer->blit_sw_CC; }}	
		public bool blit_sw_A { get { return 1 == pointer->blit_sw_A; }}		
		public bool blit_fill { get { return 1 == pointer->blit_fill; }}		
		public uint video_mem { get { return pointer->video_mem; } }

		public unsafe override string ToString() {
			StringBuilder sb = new StringBuilder();
			sb.Append("SDL_VideoInfo {");
			sb.Append("\n\tHardware acceleration: "); 
			sb.Append(pointer->hw_available != 0);
			sb.Append("\n\tWindow manager: "); 
			sb.Append(pointer->wm_available != 0);
			sb.Append("\n\tVideo memory: "); 
			sb.Append(pointer->video_mem);
			sb.Append(" k");
			sb.Append("\n\tCopy blit accelerated between HS: ");
			sb.Append(pointer->blit_hw != 0);
			sb.Append("\n\tColorkey blit accelerated between HS: ");
			sb.Append(pointer->blit_hw_CC != 0);
			sb.Append("\n\tAlpha blit accelerated between HS: ");
			sb.Append(pointer->blit_hw_A != 0);
			sb.Append("\n\tCopy blit accelerated from SS to HS: ");
			sb.Append(pointer->blit_sw != 0);
			sb.Append("\n\tColorkey blit accelerated from SS to HS: ");
			sb.Append(pointer->blit_sw_CC != 0);
			sb.Append("\n\tAlpha blit accelerated from SS to HS: ");
			sb.Append(pointer->blit_sw_A != 0);
			sb.Append("\n\tColor fill accelerated: ");
			sb.Append(pointer->blit_fill != 0);
			if(pointer->vfmt != null) {
				sb.Append("\n  ");
				sb.Append(* pointer->vfmt);
			}
			sb.Append("\n}");
			return sb.ToString();
		}
	}
}
