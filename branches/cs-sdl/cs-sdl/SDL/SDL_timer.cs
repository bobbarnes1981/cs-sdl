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
	public unsafe class SDL_timer : SDL
	{
		static SDL_timer() { 
			SDL_Init(SDL_INIT_TIMER);
		}
		
		/* This is the OS scheduler timeslice, in milliseconds */
		const uint SDL_TIMESLICE = 10;
		
		/* This is the maximum resolution of the SDL timer on all platforms */
		const uint TIMER_RESOLUTION	= 10;	/* Experimentally determined */
		
		/* Get the number of milliseconds since the SDL library initialization.
		 * Note that this value wraps if the program runs for more than ~49 days.
		 */ 
		[DllImport("SDL.dll")]
		public static extern uint SDL_GetTicks();
		
		/* Wait a specified number of milliseconds before returning */
		[DllImport("SDL.dll")]
		public static extern void SDL_Delay(uint ms);
		
		/* Function prototype for the timer callback function */
		public delegate uint SDL_TimerCallback(uint interval);
		
		/* Set a callback to run after the specified number of milliseconds has
		 * elapsed. The callback function is passed the current timer interval
		 * and returns the next timer interval.  If the returned value is the 
		 * same as the one passed in, the periodic alarm continues, otherwise a
		 * new alarm is scheduled.  If the callback returns 0, the periodic alarm
		 * is cancelled.
		 *
		 * To cancel a currently running timer, call SDL_SetTimer(0, NULL);
		 *
		 * The timer callback function may run in a different thread than your
		 * main code, and so shouldn't call any functions from within itself.
		 *
		 * The maximum resolution of this timer is 10 ms, which means that if
		 * you request a 16 ms timer, your callback will run approximately 20 ms
		 * later on an unloaded system.  If you wanted to set a flag signaling
		 * a frame update at 30 frames per second (every 33 ms), you might set a 
		 * timer for 30 ms:
		 *   SDL_SetTimer((33/10)*10, flag_update);
		 *
		 * If you use this function, you need to pass SDL_INIT_TIMER to SDL_Init().
		 *
		 * Under UNIX, you should not use raise or use SIGALRM and this function
		 * in the same program, as it is implemented using setitimer().  You also
		 * should not use this function in multi-threaded applications as signals
		 * to multi-threaded apps have undefined behavior in some implementations.
		 */
		[DllImport("SDL.dll")]
		public static extern int SDL_SetTimer(uint interval, SDL_TimerCallback callback);
		
		/* New timer API, supports multiple timers
		 * Written by Stephane Peter <megastep@lokigames.com>
		 */
		
		/* Function prototype for the new timer callback function.
		 * The callback function is passed the current timer interval and returns
		 * the next timer interval.  If the returned value is the same as the one
		 * passed in, the periodic alarm continues, otherwise a new alarm is
		 * scheduled.  If the callback returns 0, the periodic alarm is cancelled.
		 */
		public delegate uint SDL_NewTimerCallback(uint interval, object param);
		
		/* Definition of the timer ID type */
		//typedef struct _SDL_TimerID *SDL_TimerID;
		
		/* Add a new timer to the pool of timers already running.
		   Returns a timer ID, or NULL when an error occurs.
		 */
		[DllImport("SDL.dll")]
		public static extern IntPtr SDL_AddTimer(uint interval, SDL_NewTimerCallback callback, object param);
		
		/* Remove one of the multiple timers knowing its ID.
		 * Returns a boolean value indicating success.
		 */
		[DllImport("SDL.dll")]
		public static extern bool SDL_RemoveTimer(IntPtr t);
	}
}
