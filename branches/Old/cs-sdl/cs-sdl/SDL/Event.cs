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
	[StructLayout(LayoutKind.Explicit)]
	public class Event
	{
		[FieldOffset(0)] internal SDL_Event myEvent;
		
		[FieldOffset(0)] public byte type;
		[FieldOffset(0)] public SDL_ActiveEvent active;
		[FieldOffset(0)] public SDL_KeyboardEvent key;
		[FieldOffset(0)] public SDL_MouseMotionEvent motion;
		[FieldOffset(0)] public SDL_MouseButtonEvent button;
		[FieldOffset(0)] public SDL_JoyAxisEvent jaxis;
		[FieldOffset(0)] public SDL_JoyBallEvent jball;
		[FieldOffset(0)] public SDL_JoyHatEvent jhat;
		[FieldOffset(0)] public SDL_JoyButtonEvent jbutton;
		[FieldOffset(0)] public SDL_ResizeEvent resize;
		[FieldOffset(0)] public SDL_ExposeEvent expose;
		[FieldOffset(0)] public SDL_QuitEvent quit;
		[FieldOffset(0)] public SDL_UserEvent user;

		public Event() {}
		
		public static bool PollEvent(Event ev)
		{
			return (SDL_event.SDL_PollEvent(ev) == 1);
		}
		
		public static bool WaitEvent(Event ev)
		{
			return (SDL_event.SDL_WaitEvent(ev) == 1);
		}
		
		public static bool PushEvent(Event ev)
		{
			return (SDL_event.SDL_PushEvent(ev) == 0);
		}
		
		public unsafe static int PeepEvents(Event[] evs, SDL_eventaction action, uint mask)
		{
			SDL_Event[] sevs = new SDL_Event[evs.Length];
			int ret = SDL_event.SDL_PeepEvents(sevs, sevs.Length, action, mask);
			for(int i=0; i<ret; i++)
				evs[i] = new Event(sevs[i]);
			return ret;
		}
		
		private Event(SDL_Event ev)
		{
			myEvent = ev;
		}
	}
}
