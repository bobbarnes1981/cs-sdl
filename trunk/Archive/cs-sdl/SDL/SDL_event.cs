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
	/* Application visibility event structure */
	[StructLayout(LayoutKind.Sequential)]
	public struct SDL_ActiveEvent {
		public byte type;	/* SDL_ACTIVEEVENT */
		public byte gain;	/* Whether given states were gained or lost (1/0) */
		public byte state;	/* A mask of the focus states */
	}
	
	/* Keyboard event structure */
	[StructLayout(LayoutKind.Sequential)]
	public struct SDL_KeyboardEvent {
		public byte type;	/* SDL_KEYDOWN or SDL_KEYUP */
		public byte which;	/* The keyboard device index */
		public byte state;	/* SDL_PRESSED or SDL_RELEASED */
		public SDL_keysym keysym;
	} 
	
	/* Mouse motion event structure */
	[StructLayout(LayoutKind.Sequential)]
	public struct SDL_MouseMotionEvent {
		public byte type;	/* SDL_MOUSEMOTION */
		public byte which;	/* The mouse device index */
		public byte state;	/* The current button state */
		public ushort x, y;	/* The X/Y coordinates of the mouse */
		public short xrel;	/* The relative motion in the X direction */
		public short yrel;	/* The relative motion in the Y direction */
	}
	
	/* Mouse button event structure */
	[StructLayout(LayoutKind.Sequential)]
	public struct SDL_MouseButtonEvent {
		public byte type;	/* SDL_MOUSEBUTTONDOWN or SDL_MOUSEBUTTONUP */
		public byte which;	/* The mouse device index */
		public byte button;	/* The mouse button index */
		public byte state;	/* SDL_PRESSED or SDL_RELEASED */
		public ushort x, y;	/* The X/Y coordinates of the mouse at press time */
	} 
	
	/* Joystick axis motion event structure */
	[StructLayout(LayoutKind.Sequential)]
	public struct SDL_JoyAxisEvent {
		public byte type;	/* SDL_JOYAXISMOTION */
		public byte which;	/* The joystick device index */
		public byte axis;	/* The joystick axis index */
		public short value;	/* The axis value (range: -32768 to 32767) */
	} 
	
	/* Joystick trackball motion event structure */
	[StructLayout(LayoutKind.Sequential)]
	public struct SDL_JoyBallEvent {
		public byte type;	/* SDL_JOYBALLMOTION */
		public byte which;	/* The joystick device index */
		public byte ball;	/* The joystick trackball index */
		public short xrel;	/* The relative motion in the X direction */
		public short yrel;	/* The relative motion in the Y direction */
	} 
	
	/* Joystick hat position change event structure */
	[StructLayout(LayoutKind.Sequential)]
	public struct SDL_JoyHatEvent {
		public byte type;	/* SDL_JOYHATMOTION */
		public byte which;	/* The joystick device index */
		public byte hat;	/* The joystick hat index */
		public byte value;	/* The hat position value:
						8   1   2
						7   0   3
						6   5   4
					   Note that zero means the POV is centered.
					*/
	} 
	
	/* Joystick button event structure */
	[StructLayout(LayoutKind.Sequential)]
	public struct SDL_JoyButtonEvent {
		public byte type;	/* SDL_JOYBUTTONDOWN or SDL_JOYBUTTONUP */
		public byte which;	/* The joystick device index */
		public byte button;	/* The joystick button index */
		public byte state;	/* SDL_PRESSED or SDL_RELEASED */
	} 
	
	/* The "window resized" event
	   When you get this event, you are responsible for setting a new video
	   mode with the new width and height.
	 */
	[StructLayout(LayoutKind.Sequential)]
	public struct SDL_ResizeEvent {
		public byte type;	/* SDL_VIDEORESIZE */
		public int w;		/* New width */
		public int h;		/* New height */
	} 
	
	/* The "screen redraw" event */
	[StructLayout(LayoutKind.Sequential)]
	public struct SDL_ExposeEvent {
		public byte type;	/* SDL_VIDEOEXPOSE */
	} 
	
	/* The "quit requested" event */
	[StructLayout(LayoutKind.Sequential)]
	public struct SDL_QuitEvent {
		public byte type;	/* SDL_QUIT */
	} 
	
	/* A user-defined event type */
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct SDL_UserEvent {
		public byte type;	/* SDL_USEREVENT through SDL_NUMEVENTS-1 */
		public int code;	/* User defined event code */
		public void *data1;	/* User defined data pointer */
		public void *data2;	/* User defined data pointer */
	}
	
	/* If you want to use this event, you should include SDL_syswm.h */
	/*[StructLayout(LayoutKind.Sequential)]
	public struct SDL_SysWMEvent {
		uint type;
		SDL_SysWMmsg *msg;
	}*/
	
	public enum SDL_eventaction : int {
		SDL_ADDEVENT = 0,
		SDL_PEEKEVENT,
		SDL_GETEVENT
	}
	
	/* General event structure */
	[StructLayout(LayoutKind.Explicit)]
	public struct SDL_Event {
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
		//[FieldOffset(0)] public SDL_SysWMEvent syswm;
	}
	
	public unsafe class SDL_event : SDL
	{
		public const byte SDL_NOEVENT = 0;		/* Unused (do not remove) */
		public const byte SDL_ACTIVEEVENT = 1;		/* Application loses/gains visibility */
		public const byte SDL_KEYDOWN = 2;		/* Keys pressed */
		public const byte SDL_KEYUP = 3;		/* Keys released */
		public const byte SDL_MOUSEMOTION = 4;		/* Mouse moved */
		public const byte SDL_MOUSEBUTTONDOWN = 5;	/* Mouse button pressed */
		public const byte SDL_MOUSEBUTTONUP = 6;	/* Mouse button released */
		public const byte SDL_JOYAXISMOTION = 7;	/* Joystick axis motion */
		public const byte SDL_JOYBALLMOTION = 8;	/* Joystick trackball motion */
		public const byte SDL_JOYHATMOTION = 9;		/* Joystick hat position change */
		public const byte SDL_JOYBUTTONDOWN = 10;	/* Joystick button pressed */
		public const byte SDL_JOYBUTTONUP = 11;		/* Joystick button released */
		public const byte SDL_QUIT = 12;		/* User-requested quit */
		public const byte SDL_SYSWMEVENT = 13;		/* System specific event */
		public const byte SDL_EVENT_RESERVEDA = 14;	/* Reserved for future use.. */
		public const byte SDL_EVENT_RESERVEDB = 15;	/* Reserved for future use.. */
		public const byte SDL_VIDEORESIZE = 16;		/* User resized video mode */
		public const byte SDL_VIDEOEXPOSE = 17;		/* Screen needs to be redrawn */
		public const byte SDL_EVENT_RESERVED2 = 18;	/* Reserved for future use.. */
		public const byte SDL_EVENT_RESERVED3 = 19;	/* Reserved for future use.. */
		public const byte SDL_EVENT_RESERVED4 = 20;	/* Reserved for future use.. */
		public const byte SDL_EVENT_RESERVED5 = 21;	/* Reserved for future use.. */
		public const byte SDL_EVENT_RESERVED6 = 22;	/* Reserved for future use.. */
		public const byte SDL_EVENT_RESERVED7 = 23;	/* Reserved for future use.. */
		/* Events SDL_USEREVENT through SDL_MAXEVENTS-1 are for your use */
		public const byte SDL_USEREVENT = 24;
		/* This last event is only for bounding internal arrays
		It is the number of bits in the event mask datatype -- Uint32
		 */
		public const byte SDL_NUMEVENTS = 32;

		/* Predefined event masks */
		public const int SDL_ACTIVEEVENTMASK		= (1 << SDL_ACTIVEEVENT);
		public const int SDL_KEYDOWNMASK		= (1 << SDL_KEYDOWN);
		public const int SDL_KEYUPMASK			= (1 << SDL_KEYUP);
		public const int SDL_MOUSEMOTIONMASK		= (1 << SDL_MOUSEMOTION);
		public const int SDL_MOUSEBUTTONDOWNMASK	= (1 << SDL_MOUSEBUTTONDOWN);
		public const int SDL_MOUSEBUTTONUPMASK		= (1 << SDL_MOUSEBUTTONUP);
		public const int SDL_MOUSEEVENTMASK		= (1 << SDL_MOUSEMOTION) |
								  (1 << SDL_MOUSEBUTTONDOWN) |
								  (1 << SDL_MOUSEBUTTONUP);
		public const int SDL_JOYAXISMOTIONMASK		= (1 << SDL_JOYAXISMOTION);
		public const int SDL_JOYBALLMOTIONMASK		= (1 << SDL_JOYBALLMOTION);
		public const int SDL_JOYHATMOTIONMASK		= (1 << SDL_JOYHATMOTION);
		public const int SDL_JOYBUTTONDOWNMASK		= (1 << SDL_JOYBUTTONDOWN);
		public const int SDL_JOYBUTTONUPMASK		= (1 << SDL_JOYBUTTONUP);
		public const int SDL_JOYEVENTMASK		= (1 << SDL_JOYAXISMOTION) |
								  (1 << SDL_JOYBALLMOTION)|
								  (1 << SDL_JOYHATMOTION)|
								  (1 << SDL_JOYBUTTONDOWN)|
								  (1 << SDL_JOYBUTTONUP);
		public const int SDL_VIDEORESIZEMASK		= (1 << SDL_VIDEORESIZE);
		public const int SDL_VIDEOEXPOSEMASK		= (1 << SDL_VIDEOEXPOSE);
		public const int SDL_QUITMASK			= (1 << SDL_QUIT);
		public const int SDL_SYSWMEVENTMASK		= (1 << SDL_SYSWMEVENT);
		public const int SDL_ALLEVENTS			= unchecked((int)0xFFFFFFFF);
	
		/* Function prototypes */

		/* Pumps the event loop, gathering events from the input devices.
		   This function updates the event queue and internal input device state.
		   This should only be run in the thread that sets the video mode.
		*/
		[DllImport("SDL.dll")]
		public static extern void SDL_PumpEvents();
			
		/* Checks the event queue for messages and optionally returns them.
		   If 'action' is SDL_ADDEVENT, up to 'numevents' events will be added to
		   the back of the event queue.
		   If 'action' is SDL_PEEKEVENT, up to 'numevents' events at the front
		   of the event queue, matching 'mask', will be returned and will not
		   be removed from the queue.
		   If 'action' is SDL_GETEVENT, up to 'numevents' events at the front 
		   of the event queue, matching 'mask', will be returned and will be
		   removed from the queue.
		   This function returns the number of events actually stored, or -1
		   if there was an error.  This function is thread-safe.
		*/
		[DllImport("SDL.dll")]
		public static extern int SDL_PeepEvents(SDL_Event *events, int numevents,
				SDL_eventaction action, uint mask);
		//
		// Improved csgl version
		//
		[DllImport("SDL.dll")]
		public static extern int SDL_PeepEvents(SDL_Event[] events, int numevents,
				SDL_eventaction action, uint mask);
		
		/* Polls for currently pending events, and returns 1 if there are any pending
		   events, or 0 if there are none available.  If 'event' is not NULL, the next
		   event is removed from the queue and stored in that area.
		 */
		[DllImport("SDL.dll")]
		public static extern int SDL_PollEvent(SDL_Event *ev);
		//
		// Improved csgl version
		//
		[DllImport("SDL.dll")]
		public static extern int SDL_PollEvent(Event ev);
				
		/* Waits indefinitely for the next available event, returning 1, or 0 if there
		   was an error while waiting for events.  If 'event' is not NULL, the next
		   event is removed from the queue and stored in that area.
		 */
		[DllImport("SDL.dll")]
		public static extern int SDL_WaitEvent(SDL_Event *ev);
		//
		// Improved csgl version
		//
		[DllImport("SDL.dll")]
		public static extern int SDL_WaitEvent(Event ev);
		
		/* Add an event to the event queue.
		   This function returns 0, or -1 if the event couldn't be added to
		   the event queue.  If the event queue is full, this function fails.
		 */
		[DllImport("SDL.dll")]
		public static extern int SDL_PushEvent(SDL_Event *ev);
		//
		// Improved csgl version
		//
		[DllImport("SDL.dll")]
		public static extern int SDL_PushEvent(Event ev);
				
		/*
		  This function sets up a filter to process all events before they
		  change internal state and are posted to the internal event queue.
	
		  The filter is protypted as:
		*/
		public delegate int SDL_EventFilter(SDL_Event *ev);
		/*
		  If the filter returns 1, then the event will be added to the internal queue.
		  If it returns 0, then the event will be dropped from the queue, but the 
		  internal state will still be updated.  This allows selective filtering of
		  dynamically arriving events.
		
		  WARNING:  Be very careful of what you do in the event filter function, as 
			    it may run in a different thread!
		
		  There is one caveat when dealing with the SDL_QUITEVENT event type.  The
		  event filter is only called when the window manager desires to close the
		  application window.  If the event filter returns 1, then the window will
		  be closed, otherwise the window will remain open if possible.
		  If the quit event is generated by an interrupt signal, it will bypass the
		  internal queue and be delivered to the application at the next event poll.
		*/
		[DllImport("SDL.dll")]
		public static extern int SDL_SetEventFilter(SDL_EventFilter filter);
		
		/*
		  Return the current event filter - can be used to "chain" filters.
		  If there is no event filter set, this function returns NULL.
		*/
		[DllImport("SDL.dll")]
		public static extern SDL_EventFilter GetEventFilter();

		/*
		  This function allows you to set the state of processing certain events.
		  If 'state' is set to SDL_IGNORE, that event will be automatically dropped
		  from the event queue and will not event be filtered.
		  If 'state' is set to SDL_ENABLE, that event will be processed normally.
		  If 'state' is set to SDL_QUERY, SDL_EventState() will return the 
		  current processing state of the specified event.
		*/
		public const int SDL_QUERY	= -1;
		public const int SDL_IGNORE	= 0;
		public const int SDL_DISABLE	= 0;
		public const int SDL_ENABLE	= 1;
		[DllImport("SDL.dll")]
		public static extern byte SDL_EventState(uint type, int state);
	}
}
