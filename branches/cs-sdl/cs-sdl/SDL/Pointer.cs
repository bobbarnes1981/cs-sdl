// created on 27/08/2001 at 21:51
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
	[StructLayout(LayoutKind.Sequential)]
	/// <summary> a class to provide easy memory manipulation. </summary>
	public unsafe struct Pointer
	{
		public void * ptr;
		
		public Pointer(void * p)
		{
			ptr = p;
		}
		
		public static implicit operator void*(Pointer p)
		{
			return p.ptr;
		}
		
		// i use private version of standart function as they are in 
		// user32 on windows and libc on unix but in csogl everywhere !
        
        /// <summary>  
        /// copy dst pointer to source...
        /// </summary>
		[DllImport("csgl-sdl-native.dll", EntryPoint="DMmemcpy")]
		public static extern void memcpy(void * dst, void * src, uint size);
		
        /// <summary>  
        /// copy dst pointer to source...
        /// </summary>
		public static void memcpy(void * dst, void * src, int size)
		{
			memcpy(dst, src, (uint) size);
		}
		
        /// <summary>  
        /// set an area pointed to by a pointer to a given value...
        /// </summary>
		[DllImport("csgl-sdl-native.dll", EntryPoint="DMmemset")]
		public static extern void memset(void * dst, byte c, uint len);
	}
}
