/*
 * $RCSfile$
 * Copyright (C) 2004, 2005 David Hudson (jendave@yahoo.com)
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
using System.Threading;
using System.Collections;
using System.Runtime.InteropServices;

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
	/// Indicates that a joystick has moved on an axis
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	/// 
	public delegate void JoystickAxisEventHandler(object sender, JoystickAxisEventArgs e);
	/// <summary>
	/// Indicates a joystick trackball has changed position
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	public delegate void JoystickBallEventHandler(object sender, JoystickBallEventArgs e);
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
	/// Indicates that the keyboard state has changed
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	public delegate void KeyboardEventHandler(object sender, KeyboardEventArgs e);
	/// <summary>
	/// Indicates that a mouse button has been pressed or released
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	public delegate void MouseButtonEventHandler(object sender, MouseButtonEventArgs e);
	/// <summary>
	/// Indicates that the mouse has moved
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param> 
	public delegate void MouseMotionEventHandler(object sender, MouseMotionEventArgs e);
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
	/// Indicates that a portion of the window has been exposed
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	public delegate void VideoExposeEventHandler(object sender, VideoExposeEventArgs e);
	/// <summary>
	/// Indicates the user has resized the window
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	public delegate void VideoResizeEventHandler(object sender, VideoResizeEventArgs e);
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
	/// Handles the ticks as they are processed by the system.
	/// </summary>
	public delegate void TickEventHandler(object sender, TickEventArgs e);

	/// <summary>
	/// Contains events which can be attached to to read user input and other miscellaneous occurances.
	/// You can obtain an instance of this class by accessing the Events property of the main Sdl object.
	/// You must call the PollAndDelegate() member in order for any events to fire.
	/// </summary>
	public sealed class Events 
	{
		private static Hashtable UserEvents = new Hashtable();
		private static int UserEventId = 0;		
		private const int QUERY_EVENTS_MAX = 254;

		/// <summary>
		/// Fires when the application has become active or inactive
		/// </summary>
		public static event ActiveEventHandler AppActive;
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
		/// <summary>
		/// Fires every frame.
		/// </summary>
		public static event TickEventHandler TickEvent;

		static readonly Events instance = new Events();

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
				ProcessEvent(ev);
				return true;
			}
		}

		/// <summary>
		/// Polls and processes a given number of events before returning false
		/// </summary>
		/// <remarks>If the are no more events, the method will return false</remarks>
		/// <param name="numberOfEvents">Nunmber of events to process at a time at most</param>
		/// <returns>Returns false when done processing events</returns>
		public static bool Poll(int numberOfEvents) 
		{
			Sdl.SDL_Event ev;
			for (int i=0;i<=numberOfEvents;i++)
			{
				int ret = Sdl.SDL_PollEvent(out ev);
				if (ret == (int) SdlFlag.Error)
				{
					throw SdlException.Generate();
				}
				if (ret == (int) SdlFlag.None)
				{
					break;
				}
				else
				{
					ProcessEvent(ev);
				}
			}
			return false;
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
			ProcessEvent(ev);
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
		/// Remove all events from the queue
		/// </summary>
		/// <returns></returns>
		public static void Remove()
		{
			Remove(EventMask.AllEvents, QUERY_EVENTS_MAX);
		}

		/// <summary>
		/// Remove the last number of events from the queue
		/// </summary>
		/// <param name="numberOfEvents">number of events to remove</param>
		public static void Remove(int numberOfEvents)
		{
			Remove(EventMask.AllEvents, numberOfEvents);
		}

		/// <summary>
		/// Remove all events of a certain type
		/// </summary>
		/// <param name="eventMask">Events to remove</param>
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
		/// View number of events in the queue
		/// </summary>
		/// <param name="numberOfEvents">number of events to retrieve</param>
		/// <returns></returns>
		public static SdlEventArgs[] Peek(int numberOfEvents)
		{
			return Peek(EventMask.AllEvents, numberOfEvents);
		}

		/// <summary>
		/// View all events in the queue
		/// </summary>
		/// <returns></returns>
		public static SdlEventArgs[] Peek()
		{
			return Peek(EventMask.AllEvents, QUERY_EVENTS_MAX);
		}

		/// <summary>
		/// View all events of a certain type in the queue
		/// </summary>
		/// <param name="eventMask">Type of events to view</param>
		/// <returns></returns>
		public static SdlEventArgs[] Peek(EventMask eventMask)
		{
			return Peek(eventMask, QUERY_EVENTS_MAX);
		}

		/// <summary>
		/// check to se if a certain kind of event is queued
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
		/// Disables event from appearing in the queue
		/// </summary>
		/// <param name="eventType"></param>
		public static void IgnoreEvent(EventTypes eventType)
		{
			Sdl.SDL_EventState((byte)eventType, Sdl.SDL_IGNORE);
		}

		/// <summary>
		/// Event type will be processed by queue.
		/// </summary>
		/// <remarks>all events are enabled by default</remarks>
		/// <param name="eventType"></param>
		public static void EnableEvent(EventTypes eventType)
		{
			Sdl.SDL_EventState((byte)eventType, Sdl.SDL_ENABLE);
		}

		/// <summary>
		/// Check to see if this type of event is enabled
		/// </summary>
		/// <param name="eventType"></param>
		/// <returns></returns>
		public static bool IsEventEnabled(EventTypes eventType)
		{
			return (Sdl.SDL_EventState((byte)eventType, Sdl.SDL_QUERY) == Sdl.SDL_ENABLE);
		}

		private static void ProcessEvent(Sdl.SDL_Event ev) 
		{
			switch ((EventTypes)ev.type)
			{
				case EventTypes.ActiveEvent:
					OnActiveEvent(new ActiveEventArgs(ev));
					break;

				case EventTypes.JoystickAxisMotion:
					OnJoystickAxisMotion(new JoystickAxisEventArgs(ev));
					break;

				case EventTypes.JoystickBallMotion:
					OnJoystickBallMotion(new JoystickBallEventArgs(ev));
					break;

				case EventTypes.JoystickButtonDown:
					OnJoystickButtonDown(new JoystickButtonEventArgs(ev));
					break;

				case EventTypes.JoystickButtonUp:
					OnJoystickButtonUp(new JoystickButtonEventArgs(ev));
					break;

				case EventTypes.JoystickHatMotion:
					OnJoystickHatMotion(new JoystickHatEventArgs(ev));
					break;

				case EventTypes.KeyDown:
					OnKeyboardDown(new KeyboardEventArgs(ev));
					break;

				case EventTypes.KeyUp:
					OnKeyboardUp(new KeyboardEventArgs(ev));
					break;

				case EventTypes.MouseButtonDown:
					OnMouseButtonDown(new MouseButtonEventArgs(ev));
					break;

				case EventTypes.MouseButtonUp:
					OnMouseButtonUp(new MouseButtonEventArgs(ev));
					break;

				case EventTypes.MouseMotion:
					OnMouseMotion(new MouseMotionEventArgs(ev));
					break;

				case EventTypes.Quit:
					OnQuitEvent(new QuitEventArgs(ev));
					break;

				case EventTypes.UserEvent:
					OnUserEvent(new UserEventArgs(ev));
					break;

				case EventTypes.VideoExpose:
					OnVideoExpose(new VideoExposeEventArgs(ev));
					break;

				case EventTypes.VideoResize:
					OnVideoResize(new VideoResizeEventArgs(ev));
					break;
			}
		}

		static void OnActiveEvent(ActiveEventArgs e)
		{
			if (AppActive != null)
			{
				AppActive(instance, e);
			}
		}

		static void OnJoystickAxisMotion(JoystickAxisEventArgs e)
		{
			if (JoystickAxisMotion != null) 
			{						
				if (((e.RawAxisValue < (-1)*JoystickAxisEventArgs.JoystickThreshold) ||
					(e.RawAxisValue > JoystickAxisEventArgs.JoystickThreshold)) && (e.AxisIndex == 0 || e.AxisIndex == 1) )
				{	
					JoystickAxisMotion(instance, e);
				}
			}
		}

		static void OnJoystickBallMotion(JoystickBallEventArgs e)
		{
			if (JoystickBallMotion != null) 
			{
				JoystickBallMotion(instance, e);
			}
		}

		static void OnJoystickButtonDown(JoystickButtonEventArgs e)
		{
			if (JoystickButtonDown != null) 
			{
				JoystickButtonDown(instance, e);
			}
		}

		static void OnJoystickButtonUp(JoystickButtonEventArgs e)
		{
			if (JoystickButtonUp != null) 
			{
				JoystickButtonUp(instance, e);
			}
		}

		static void OnJoystickHatMotion(JoystickHatEventArgs e)
		{
			if (JoystickHatMotion != null) 
			{
				JoystickHatMotion(instance, e);
			}
		}

		static void OnKeyboardDown(KeyboardEventArgs e)
		{
			if (KeyboardDown != null) 
			{
				KeyboardDown(instance, e);
			}
		}
		static void OnKeyboardUp(KeyboardEventArgs e)
		{
			if (KeyboardUp != null) 
			{
				KeyboardUp(instance, e);
			}
		}

		static void OnMouseButtonDown(MouseButtonEventArgs e)
		{
			if (MouseButtonDown != null)
			{
				MouseButtonDown(instance, e);
			}
		}

		static void OnMouseButtonUp(MouseButtonEventArgs e)
		{
			if (MouseButtonUp != null)
			{
				MouseButtonUp(instance, e);
			}
		}

		static void OnMouseMotion(MouseMotionEventArgs e)
		{
			if (MouseMotion != null) 
			{
				MouseMotion(instance, e);
			}
		}

		static void OnQuitEvent(QuitEventArgs e)
		{
			if (Quit != null) 
			{
				Quit(instance, e);
			}
		}

		static void OnUserEvent(UserEventArgs e)
		{
			if (UserEvent != null || ChannelFinished != null || MusicFinished != null) 
			{
				object ret;
				lock (instance) 
				{
					ret = (UserEventArgs)UserEvents[e.UserCode];
				}
				if (ret != null) 
				{
					if (ret.GetType().Name == "ChannelFinishedEventArgs") 
					{
						if (ChannelFinished != null)
						{
							ChannelFinished(instance, (ChannelFinishedEventArgs)ret);
						}
					} 
					else if (ret.GetType().Name == "MusicFinishedEventArgs") 
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
		}

		static void OnVideoExpose(VideoExposeEventArgs e)
		{
			if (VideoExpose != null) 
			{
				VideoExpose(instance, e);
			}
		}

		static void OnVideoResize(VideoResizeEventArgs e)
		{
			if (VideoResize != null) 
			{
				VideoResize(instance, e);
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

		#region Thread Management
		private static Thread serverThread = null;
		private static Thread tickerThread = null;
		private static bool stopThread = true;
		private static int skippedTicks = 0;
		private static int processSkipped = 0;
		private static bool processing = false;
		private static long lastTick = DateTime.Now.Ticks;

		/// <summary>
		/// Processes the tick server thread. This keeps track of the
		/// number of skipped ticks, to give a more accurate count or
		/// status of each tick.
		/// </summary>
		private static void Run()
		{
			// Loops until the system indicates a stop
			while (!stopThread)
			{
				// Sleep for a little bit
				Thread.Sleep(tickSpan);

				// Lock to see if we are processing
				lock (Events.instance) 
				{
					// If we are processing, just increment it
					if (processing)
					{
						skippedTicks++;
						continue;
					}

					// Process the counter
					processSkipped = skippedTicks;
					skippedTicks = 0;
					processing = true;
				}

				// Perform the actual threaded tick
				tickerThread = new Thread(new ThreadStart(RunTicker));
				tickerThread.IsBackground = true;
				tickerThread.Priority = ThreadPriority.Normal; // ThreadPriority.Lowest; Almost disregards thread
				tickerThread.Name = "Server Thread Tick";
				tickerThread.Start();
			}
		}

		/// <summary>
		/// Executes a single tick statement in a second thread.
		/// </summary>
		//private static void RunTicker(object stateInfo)
		private static void RunTicker()
		{
			// Execute the tick
			if (Events.TickEvent != null)
			{
				// Create the arguments
				long now = DateTime.Now.Ticks;
				TickEventArgs args = new TickEventArgs(processSkipped, now - lastTick);
				lastTick = now;
			
				// Trigger the ticker
				Events.TickEvent(Events.instance, args);
			}
      
			// Clear the flag. This is in a locked block because the testing
			// of it is also in the same lock.
			lock (Events.instance)
			{
				processing = false;
			}
		}

		/// <summary>
		/// Start ticker thread
		/// </summary>
		public static void StartTicker()
		{
			// Prepare for the server thread
			stopThread = false;
			serverThread = new Thread(new ThreadStart(Run));
			serverThread.IsBackground = true;
			serverThread.Priority = ThreadPriority.Normal; // ThreadPriority.Lowest; - Almost disregards tick events
			serverThread.Name = "Server Thread";
			serverThread.Start();
		}

		/// <summary>
		/// Stop ticker thread
		/// </summary>
		public static void StopTicker()
		{
			// Mark everything to stop
			stopThread = true;

			// Join the ticker
			if (tickerThread != null)
			{
				try
				{
					tickerThread.Join();
				}
				catch
				{
					throw new SdlException("StopTicker Problem");
				}
			}

			// Join the server
			try
			{
				serverThread.Join();
				serverThread = null;
			}
			catch
			{
				throw new SdlException("Thread Join exception");
			}

			// Make noise
			while (processing)
			{
				Thread.Sleep(tickSpan);
			}
		}
		#endregion

		#region Tick Duration
		private static int tickSpan = 50;

		/// <summary>
		/// Ticks per second
		/// </summary>
		public static int TicksPerSecond
		{
			get 
			{ 
				return 1000 / tickSpan; 
			}
			set 
			{ 
				if (value > 1 && value <= 1000)
				{
					int temp = 1000 / value;
					tickSpan = temp;
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public static int TickSpan
		{
			get 
			{ 
				return tickSpan; 
			}
			set
			{
				if (value < 1)
				{
					throw new SdlException("Cannot set a negative or zero sleep time.");
				}

				tickSpan = value;
			}
		}
		#endregion
	}
}
