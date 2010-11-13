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
	/* Keysym structure
	   - The scancode is hardware dependent, and should not be used by general
	     applications.  If no hardware scancode is available, it will be 0.
	
	   - The 'unicode' translated character is only available when character
	     translation is enabled by the SDL_EnableUNICODE() API.  If non-zero,
	     this is a UNICODE character corresponding to the keypress.  If the
	     high 9 bits of the character are 0, then this maps to the equivalent
	     ASCII character:
		char ch;
		if ( (keysym.unicode & 0xFF80) == 0 ) {
			ch = keysym.unicode & 0x7F;
		} else {
			An international character..
		}
	 */
	public struct SDL_keysym {
		public byte scancode;			/* hardware specific scancode */
		public SDLKey sym;			/* SDL virtual keysym */
		public SDLMod mod;			/* current key modifiers */
		public ushort unicode;			/* translated character */
	}
	
	public unsafe class SDL_keyboard
	{
		/* This is the mask which refers to all hotkey bindings */
		public const int SDL_ALL_HOTKEYS = 0xFFFFFFF;
		
		/* Function prototypes */
		/*
		 * Enable/Disable UNICODE translation of keyboard input.
		 * This translation has some overhead, so translation defaults off.
		 * If 'enable' is 1, translation is enabled.
		 * If 'enable' is 0, translation is disabled.
		 * If 'enable' is -1, the translation state is not changed.
		 * It returns the previous state of keyboard translation.
		 */
		[DllImport("SDL.dll")]
		public static extern int SDL_EnableUNICODE(int enable);
	
		/*
		 * Enable/Disable keyboard repeat.  Keyboard repeat defaults to off.
		 * 'delay' is the initial delay in ms between the time when a key is
		 * pressed, and keyboard repeat begins.
		 * 'interval' is the time in ms between keyboard repeat events.
		 */
		public const int SDL_DEFAULT_REPEAT_DELAY = 500;
		public const int SDL_DEFAULT_REPEAT_INTERVAL = 30;
		/*
		 * If 'delay' is set to 0, keyboard repeat is disabled.
		 */
		[DllImport("SDL.dll")]
		public static extern int SDL_EnableKeyRepeat(int delay, int interval);
		
		/*
		 * Get a snapshot of the current state of the keyboard.
		 * Returns an array of keystates, indexed by the SDLK_* syms.
		 * Used:
		 * 	Uint8 *keystate = SDL_GetKeyState(NULL);
		 *	if ( keystate[SDLK_RETURN] ) ... <RETURN> is pressed.
		 */
		[DllImport("SDL.dll")]
		public static extern byte * SDL_GetKeyState(int *numkeys);
		
		/*
		 * Get the current key modifier state
		 */
		[DllImport("SDL.dll")]
		public static extern SDLMod SDL_GetModState();
		
		/*
		 * Set the current key modifier state
		 * This does not change the keyboard state, only the key modifier flags.
		 */
		[DllImport("SDL.dll")]
		public static extern void SDL_SetModState(SDLMod modstate);
		
		/*
		 * Get the name of an SDL virtual keysym
		 */
		[DllImport("SDL.dll")]
		public static extern string SDL_GetKeyName(SDLKey key);
	}
}
