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
using System.Runtime.InteropServices;

namespace CsGL.SDL
{
	public class SDL : CSSDL
	{
		/* As of version 0.5, SDL is loaded dynamically into the application */

		/* These are the flags which may be passed to SDL_Init() -- you should
		   specify the subsystems which you will be using in your application.
		*/
		public const uint NO_TYPE_INIT = 0;
		public const uint SDL_INIT_TIMER = 0x00000001;
		public const uint SDL_INIT_AUDIO = 0x00000010;
		public const uint SDL_INIT_VIDEO = 0x00000020;
		public const uint SDL_INIT_CDROM = 0x00000100;
		public const uint SDL_INIT_JOYSTICK = 0x00000200;
		public const uint SDL_INIT_NOPARACHUTE = 0x00100000;	/* Don't catch fatal signals */
		public const uint SDL_INIT_EVENTTHREAD = 0x01000000;	/* Not supported on all OS's */
		public const uint SDL_INIT_EVERYTHING = 0x0000FFFF;
		
		/* This function loads the SDL dynamically linked library and initializes 
		 * the subsystems specified by 'flags' (and those satisfying dependencies)
		 * Unless the SDL_INIT_NOPARACHUTE flag is set, it will install cleanup
		 * signal handlers for some commonly ignored fatal signals (like SIGSEGV)
		 */
		[DllImport("SDL.dll")]
		public static extern int SDL_Init(uint flags);
		
		/* This function initializes specific SDL subsystems */
		[DllImport("SDL.dll")]
		public static extern int SDL_InitSubSystem(uint flags);

		/* This function cleans up specific SDL subsystems */
		[DllImport("SDL.dll")]
		public static extern void SDL_QuitSubSystem(uint flags);
		
		/* This function returns mask of the specified subsystems which have
		   been initialized.
		   If 'flags' is 0, it returns a mask of all initialized subsystems.
		*/
		[DllImport("SDL.dll")]
		public static extern uint SDL_WasInit(uint flags);
		
		/* This function cleans up all initialized subsystems and unloads the
		 * dynamically linked library.  You should call it upon all exit conditions.
		 */
		[DllImport("SDL.dll")]
		public static extern void SDL_Quit();
	}
}
