/*
 * $RCSfile$
 * Copyright (C) 2004 David Hudson (jendave@yahoo.com)
 * Copyright (C) 2003 Will Weisser (ogl@9mm.com)
 *
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later version.
 * 
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 * 
 * You should have received a copy of the GNU Lesser General Public
 * License along with this library; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
 */

using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Globalization;

using Tao.Sdl;

namespace SdlDotNet 
{
	/// <summary>
	/// Indicates that the application has gained or lost input focus
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	public delegate void ActiveEventHandler(object sender, ActiveEventArgs e);

	/// <summary>
	/// Indicates that the keyboard state has changed
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	public delegate void KeyboardEventHandler(object sender, KeyboardEventArgs e);

	/// <summary>
	/// Indicates that the mouse has moved
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param> 
	public delegate void MouseMotionEventHandler(object sender, MouseMotionEventArgs e);
	/// <summary>
	/// Indicates that a mouse button has been pressed or released
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	public delegate void MouseButtonEventHandler(object sender, MouseButtonEventArgs e);
	/// <summary>
	/// Indicates that a joystick has moved on an axis
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	/// 
	public delegate void JoystickAxisEventHandler(object sender, JoystickAxisEventArgs e);
	/// <summary>
	/// Indicates that a joystick button has been pressed or released
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	public delegate void JoystickButtonEventHandler(object sender, JoystickButtonEventArgs e);
	/// <summary>
	/// Indicates a joystick hat has changed position
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	public delegate void JoystickHatEventHandler(object sender, JoystickHatEventArgs e);
	/// <summary>
	/// Indicates a joystick trackball has changed position
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	public delegate void JoystickBallEventHandler(object sender, JoystickBallEventArgs e);
	/// <summary>
	/// Indicates the user has resized the window
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	public delegate void VideoResizeEventHandler(object sender, VideoResizeEventArgs e);
	/// <summary>
	/// Indicates that a portion of the window has been exposed
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	public delegate void VideoExposeEventHandler(object sender, VideoExposeEventArgs e);
	/// <summary>
	/// Indicates that the user has closed the main window
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	public delegate void QuitEventHandler(object sender, QuitEventArgs e);
	/// <summary>
	/// Indicates a user event has fired
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	public delegate void UserEventHandler(object sender, UserEventArgs e);
	/// <summary>
	/// Indicates that a sound channel has stopped playing
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	public delegate void ChannelFinishedEventHandler(object sender, ChannelFinishedEventArgs e);
	/// <summary>
	/// Indicates that a music sample has stopped playing
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	public delegate void MusicFinishedEventHandler(object sender, MusicFinishedEventArgs e);

	/// <summary>
	/// Contains events which can be attached to to read user input and other miscellaneous occurances.
	/// You can obtain an instance of this class by accessing the Events property of the main Sdl object.
	/// You must call the PollAndDelegate() member in order for any events to fire.
	/// </summary>
	public sealed class Events 
	{
		private static Hashtable UserEvents = new Hashtable();
		private static int UserEventId = 1;
		
		//Reduced joystick "jitter"
		
		private const int QUERY_EVENTS_MAX = 254;

		/// <summary>
		/// Fires when the application has become active or inactive
		/// </summary>
		public static event ActiveEventHandler AppActive;
		/// <summary>
		/// Fires when the application gains or loses mouse focus
		/// </summary>
		public static event ActiveEventHandler MouseFocus;
		/// <summary>
		/// Fires when the application gains or loses input focus
		/// </summary>
		public static event ActiveEventHandler InputFocus;
		/// <summary>
		/// Fires when a key is pressed or released
		/// </summary>
		public static event KeyboardEventHandler Keyboard;
		/// <summary>
		/// Fires when a key is pressed
		/// </summary>
		public static event KeyboardEventHandler KeyboardDown;
		/// <summary>
		/// Fires when a key is released
		/// </summary>
		public static event KeyboardEventHandler KeyboardUp;
		/// <summary>
		/// Fires when the mouse moves
		/// </summary>
		public static event MouseMotionEventHandler MouseMotion;
		/// <summary>
		/// Fires when a mouse button is pressed or released
		/// </summary>
		public static event MouseButtonEventHandler MouseButton;
		/// <summary>
		/// Fires when a mouse button is pressed
		/// </summary>
		public static event MouseButtonEventHandler MouseButtonDown;
		/// <summary>
		/// Fires when a mouse button is released
		/// </summary>
		public static event MouseButtonEventHandler MouseButtonUp;
		/// <summary>
		/// Fires when a joystick axis changes
		/// </summary>
		public static event JoystickAxisEventHandler JoystickAxisMotion;
		/// <summary>
		/// Fires when a joystick vertical axis changes
		/// </summary>
		public static event JoystickAxisEventHandler JoystickVerticalAxisMotion;
		/// <summary>
		/// Fires when a joystick horizontal axis changes
		/// </summary>
		public static event JoystickAxisEventHandler JoystickHorizontalAxisMotion;
		/// <summary>
		/// Fires when a joystick button is pressed or released
		/// </summary>
		public static event JoystickButtonEventHandler JoystickButton;
		/// <summary>
		/// Fires when a joystick button is pressed
		/// </summary>
		public static event JoystickButtonEventHandler JoystickButtonDown;
		/// <summary>
		/// Fires when a joystick button is released
		/// </summary>
		public static event JoystickButtonEventHandler JoystickButtonUp;
		/// <summary>
		/// Fires when a joystick hat changes
		/// </summary>
		public static event JoystickHatEventHandler JoystickHatMotion;
		/// <summary>
		/// Fires when a joystick trackball changes
		/// </summary>
		public static event JoystickBallEventHandler JoystickBallMotion;
		/// <summary>
		/// Fires when the user resizes the window
		/// </summary>
		public static event VideoResizeEventHandler VideoResize;
		/// <summary>
		/// Fires when a portion of the window is uncovered
		/// </summary>
		public static event VideoExposeEventHandler VideoExpose;
		/// <summary>
		/// Fires when a user closes the window
		/// </summary>
		public static event QuitEventHandler Quit;
		/// <summary>
		/// Fires when a user event is consumed
		/// </summary>
		public static event UserEventHandler UserEvent;
		/// <summary>
		/// Fires when a sound channel finishes playing.
		/// Will only occur if you call Channel.EnableChannelCallbacks().
		/// </summary>
		public static event ChannelFinishedEventHandler ChannelFinished;
		/// <summary>
		/// Fires when a music sample finishes playing.
		/// Will only occur if you call Mixer.EnableMusicCallbacks().
		/// </summary>
		public static event MusicFinishedEventHandler MusicFinished;

		static readonly Events instance = new Events();

//		static Events()
//		{
//		}

		Events()
		{
		}

		/// <summary>
		/// Checks the event queue, and processes 
		/// any events it finds by invoking the event properties
		/// </summary>
		/// <returns>
		/// True if any events were in the queue, otherwise False
		/// </returns>
		public static bool Poll() 
		{
			Sdl.SDL_Event ev;
			int ret = Sdl.SDL_PollEvent(out ev);
			if (ret == (int) SdlFlag.Error)
			{
				throw SdlException.Generate();
			}
			if (ret == (int) SdlFlag.None)
			{
				return false;
			}
			else
			{
				ProcessEvent(ref ev);
				return true;
			}
		}

		/// <summary>
		/// Checks the event queue, and waits until an event is available
		/// </summary>
		public static void Wait() 
		{
			Sdl.SDL_Event ev;
			if (Sdl.SDL_WaitEvent(out ev) == (int) SdlFlag.Error2)
			{
				throw SdlException.Generate();
			}
			ProcessEvent(ref ev);
		}

		/// <summary>
		/// Pushes a user-defined event on the event queue
		/// </summary>
		/// <param name="userEventArgs">
		/// An opaque object representing a user-defined event.
		/// Will be passed back to the UserEvent handler delegate when this event is processed.</param>
		public static void PushUserEvent(UserEventArgs userEventArgs) 
		{
			//Sdl.SDL_UserEvent sdlev = new Sdl.SDL_UserEvent();
			//sdlev.type = (byte)Sdl.SDL_USEREVENT;
			lock (instance) 
			{
				UserEvents[UserEventId] = userEventArgs;
				userEventArgs.UserCode = UserEventId;
				UserEventId++;
			}

			Sdl.SDL_Event evt = userEventArgs.EventStruct;
			if (Sdl.SDL_PushEvent(out evt) != (int) SdlFlag.Success)
			{
				throw SdlException.Generate();
			}
		}

		/// <summary>
		/// Adds an event to the Event queue.
		/// </summary>
		/// <param name="sdlEvent"></param>
		/// <returns></returns>
		public static void Add(SdlEventArgs sdlEvent)
		{
			SdlEventArgs[] events = new SdlEventArgs[1];
			events[0] = sdlEvent;
			Add(events);
		}

		/// <summary>
		/// Adds an array of events to the event queue.
		/// </summary>
		/// <param name="sdlEvents"></param>
		/// <returns></returns>
		public static void Add(SdlEventArgs[] sdlEvents)
		{
			Sdl.SDL_Event[] events = new Sdl.SDL_Event[sdlEvents.Length];
			for (int i = 0; i < sdlEvents.Length; i++)
			{
				events[ i ] = sdlEvents[ i ].EventStruct;
			}
			int result = 
				Sdl.SDL_PeepEvents(
				events, 
				events.Length, 
				Sdl.SDL_ADDEVENT, 
				(int)EventMask.AllEvents);
			if (result == (int)SdlFlag.Error)
			{
				throw SdlException.Generate();
			}
		}
		/// <summary>
		/// Returns an array of events in the event queue.
		/// </summary>
		/// <param name="eventMask"></param>
		/// <param name="numberOfEvents"></param>
		/// <returns></returns>
		public static void Remove(EventMask eventMask, int numberOfEvents)
		{
			Sdl.SDL_Event[] events = new Sdl.SDL_Event[numberOfEvents];
			int result = 
				Sdl.SDL_PeepEvents(
				events, 
				events.Length,
				Sdl.SDL_GETEVENT, 
				(int)eventMask);
			if (result == (int)SdlFlag.Error)
			{
				throw SdlException.Generate();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public static void Remove()
		{
			Remove(EventMask.AllEvents, QUERY_EVENTS_MAX);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="numberOfEvents"></param>
		/// <returns></returns>
		public static void Remove(int numberOfEvents)
		{
			Remove(EventMask.AllEvents, numberOfEvents);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="eventMask"></param>
		/// <returns></returns>
		public static void Remove(EventMask eventMask)
		{
			Remove(eventMask, QUERY_EVENTS_MAX);
		}

		/// <summary>
		/// Returns an array of events in the event queue.
		/// </summary>
		/// <param name="eventMask"></param>
		/// <param name="numberOfEvents"></param>
		/// <returns></returns>
		public static SdlEventArgs[] Peek(EventMask eventMask, int numberOfEvents)
		{
			Sdl.SDL_Event[] events = new Sdl.SDL_Event[numberOfEvents];
			int result = 
				Sdl.SDL_PeepEvents(
				events, 
				events.Length,
				Sdl.SDL_PEEKEVENT, 
				(int)eventMask);
			if (result == (int)SdlFlag.Error)
			{
				throw SdlException.Generate();
			}
			SdlEventArgs[] eventsArray = new SdlEventArgs[result];
			for (int i = 0; i < eventsArray.Length; i++)
			{
				eventsArray[ i ] = SdlEventArgs.CreateEventArgs(events[ i ]);
			}
			return eventsArray;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="numberOfEvents"></param>
		/// <returns></returns>
		public static SdlEventArgs[] Peek(int numberOfEvents)
		{
			return Peek(EventMask.AllEvents, numberOfEvents);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public static SdlEventArgs[] Peek()
		{
			return Peek(EventMask.AllEvents, QUERY_EVENTS_MAX);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="eventMask"></param>
		/// <returns></returns>
		public static SdlEventArgs[] Peek(EventMask eventMask)
		{
			return Peek(eventMask, QUERY_EVENTS_MAX);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="eventMask"></param>
		/// <returns></returns>
		public static bool IsEventQueued(EventMask eventMask)
		{
			SdlEventArgs[] eventArray = Peek(eventMask, QUERY_EVENTS_MAX);
			if (eventArray.Length > 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// /pump
		/// </summary>
		/// <param name="eventType"></param>
		public static void IgnoreEvent(EventTypes eventType)
		{
			Sdl.SDL_EventState((byte)eventType, Sdl.SDL_IGNORE);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="eventType"></param>
		public static void EnableEvent(EventTypes eventType)
		{
			Sdl.SDL_EventState((byte)eventType, Sdl.SDL_ENABLE);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="eventType"></param>
		/// <returns></returns>
		public static bool IsEventEnabled(EventTypes eventType)
		{
			return (Sdl.SDL_EventState((byte)eventType, Sdl.SDL_QUERY) == Sdl.SDL_ENABLE);
		}

		private static void ProcessEvent(ref Sdl.SDL_Event ev) 
		{
			switch ((EventTypes)ev.type)
			{
				case EventTypes.ActiveEvent:
					if (AppActive != null || MouseFocus != null || InputFocus != null) 
					{
						ActiveEventArgs e = new ActiveEventArgs(ev);

						if ((ev.active.state & (byte)Focus.Mouse) != 0 && 
							MouseFocus != null)
						{
							MouseFocus(instance, e);
						}
						if ((ev.active.state & (byte)Focus.Keyboard) != 0 && 
							InputFocus != null)
						{
							InputFocus(instance, e);
						}
						if ((ev.active.state & (byte)Focus.Application) != 0 && 
							AppActive != null)
						{
							AppActive(instance, e);
						}
					}
					break;

				case EventTypes.JoystickAxisMotion:
					if (JoystickAxisMotion != null || 
						JoystickHorizontalAxisMotion != null || 
						JoystickVerticalAxisMotion != null) 
					{
						Console.WriteLine("axisVal: " + ev.jaxis.val);
						Console.WriteLine("which: " + ev.jaxis.which);
						Console.WriteLine("axis: " + ev.jaxis.axis);
						Console.WriteLine("type: " + ev.type);
						
						if ((ev.jaxis.val < (-1)*JoystickAxisEventArgs.JoystickThreshold) || (ev.jaxis.val > JoystickAxisEventArgs.JoystickThreshold))
						{	
							JoystickAxisEventArgs e = new JoystickAxisEventArgs(ev);
							if (ev.jaxis.axis == 0)
							{
								if (JoystickAxisMotion != null) 
								{
									JoystickAxisMotion(instance, e);
								}
								if (JoystickHorizontalAxisMotion != null) 
								{
									JoystickHorizontalAxisMotion(instance, e);
								}
							}
							else if (ev.jaxis.axis == 1)
							{
								if (JoystickAxisMotion != null) 
								{
									JoystickAxisMotion(instance, e);
								}
								if (JoystickVerticalAxisMotion != null) 
								{
									JoystickVerticalAxisMotion(instance, e);
								}
							}
							else
							{
								if (JoystickAxisMotion != null) 
								{
									JoystickAxisMotion(instance, e);
								}
							}
						}
					}
					break;

				case EventTypes.JoystickBallMotion:
					if (JoystickBallMotion != null) 
					{
						JoystickBallMotion(
							instance,
							new JoystickBallEventArgs(ev));
					}
					break;

				case EventTypes.JoystickButtonDown:
					if (JoystickButton != null || JoystickButtonDown != null) 
					{
						JoystickButtonEventArgs e = 
							new JoystickButtonEventArgs(ev);
						if (JoystickButton != null)
						{
							JoystickButton(instance, e);
						}
						if (JoystickButtonDown != null)
						{
							JoystickButtonDown(instance, e);
						}
					}
					break;

				case EventTypes.JoystickButtonUp:
					if (JoystickButton != null || JoystickButtonUp != null) 
					{
						JoystickButtonEventArgs e = 
							new JoystickButtonEventArgs(ev);
						if (JoystickButton != null)
						{
							JoystickButton(instance, e);
						}
						if (JoystickButtonUp != null)
						{
							JoystickButtonUp(instance, e);
						}
					}
					break;

				case EventTypes.JoystickHatMotion:
					if (JoystickHatMotion != null) 
					{
						JoystickHatMotion(instance, new JoystickHatEventArgs(ev));
					}
					break;

				case EventTypes.KeyDown:
					if (Keyboard != null || KeyboardDown != null) 
					{
						KeyboardEventArgs e = new KeyboardEventArgs(ev);
						if (KeyboardDown != null) 
						{
							KeyboardDown(instance, e);
						}
						if (Keyboard != null) 
						{
							Keyboard(instance, e);
						}
					}
					break;

				case EventTypes.KeyUp:
					if (Keyboard != null || KeyboardUp != null) 
					{
						KeyboardEventArgs e = new KeyboardEventArgs(ev);
						if (KeyboardUp != null) 
						{
							KeyboardUp(instance, e);
						}
						if (Keyboard != null) 
						{
							Keyboard(instance, e);
						}
					}
					break;

				case EventTypes.MouseButtonDown:
					if (MouseButton != null)
					{
						MouseButton(instance, new MouseButtonEventArgs(ev));
					}
					if (MouseButtonDown != null)
					{
						MouseButtonDown(instance, new MouseButtonEventArgs(ev));
					}
					break;

				case EventTypes.MouseButtonUp:
					if (MouseButton != null)
					{
						MouseButton(instance, new MouseButtonEventArgs(ev));
					}
					if (MouseButtonUp != null)
					{
						MouseButtonUp(instance, new MouseButtonEventArgs(ev));
					}
					break;

				case EventTypes.MouseMotion:
					if (MouseMotion != null) 
					{
						MouseMotion(instance, new MouseMotionEventArgs(ev));
					}
					break;

				case EventTypes.Quit:
					if (Quit != null) 
					{
						Quit(instance, new QuitEventArgs(ev));
					}
					break;

				case EventTypes.UserEvent:
					if (UserEvent != null || ChannelFinished != null || MusicFinished != null) 
					{
						object ret;
						lock (instance) 
						{
							ret = UserEvents[ev.user.code];
						}
						if (ret != null) 
						{
							if (ret is ChannelFinishedEventArgs) 
							{
								if (ChannelFinished != null)
								{
									ChannelFinished(instance, (ChannelFinishedEventArgs)ret);
								}
							} 
							else if (ret is MusicFinishedEventArgs) 
							{
								if (MusicFinished != null)
								{
									MusicFinished(instance, (MusicFinishedEventArgs)ret);
								}
							} 
							else
								UserEvent(instance, (UserEventArgs)ret);
						}
					}
					break;

				case EventTypes.VideoExpose:
					if (VideoExpose != null) 
					{
						VideoExpose(instance, new VideoExposeEventArgs(ev));
					}
					break;

				case EventTypes.VideoResize:
					if (VideoResize != null) 
					{
						VideoResize(instance, new VideoResizeEventArgs(ev));
					}
					break;
			}
		}

		internal static void NotifyChannelFinished(int channel) 
		{
			PushUserEvent(new ChannelFinishedEventArgs(channel));
		}
		internal static void NotifyMusicFinished() 
		{
			PushUserEvent(new MusicFinishedEventArgs());
		}
	}
}
