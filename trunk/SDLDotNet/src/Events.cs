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
	public delegate void ResizeEventHandler(object sender, ResizeEventArgs e);
	/// <summary>
	/// Indicates that a portion of the window has been exposed
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	public delegate void ExposeEventHandler(object sender, ExposeEventArgs e);
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
		private static int UserEventId = 0;
		//Reduced joystick "jitter"
		private const int JOYSTICK_THRESHHOLD = 3200;
		private const int JOYSTICK_ADJUSTMENT = 32768;
		private const int JOYSTICK_SCALE = 65535;

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
		public static event ResizeEventHandler Resize;
		/// <summary>
		/// Fires when a portion of the window is uncovered
		/// </summary>
		public static event ExposeEventHandler Expose;
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
		/// Will only occur if you call Mixer.EnableMusicCallbacks().
		/// </summary>
		public static event ChannelFinishedEventHandler ChannelFinished;
		/// <summary>
		/// Fires when a music sample finishes playing.
		/// Will only occur if you call Mixer.EnableMusicCallbacks().
		/// </summary>
		public static event MusicFinishedEventHandler MusicFinished;

		static readonly Events instance = new Events();

		static Events()
		{
		}

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
		public static bool PollAndDelegate() 
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
			DelegateEvent(ref ev);
			return true;
		}

		/// <summary>
		/// Checks the event queue, and waits until an event is available
		/// </summary>
		public static void WaitAndDelegate() 
		{
			Sdl.SDL_Event ev;
			if (Sdl.SDL_WaitEvent(out ev) == (int) SdlFlag.Error2)
			{
				throw SdlException.Generate();
			}
			DelegateEvent(ref ev);
		}

		/// <summary>
		/// Pushes a user-defined event on the event queue
		/// </summary>
		/// <param name="userEvent">
		/// An opaque object representing a user-defined event.
		/// Will be passed back to the UserEvent handler delegate when this event is processed.</param>
		public static void PushUserEvent(object userEvent) 
		{
			Sdl.SDL_UserEvent sdlev = new Sdl.SDL_UserEvent();
			sdlev.type = (byte)Sdl.SDL_USEREVENT;
			lock (instance) 
			{
				UserEvents[UserEventId] = userEvent;
				sdlev.code = UserEventId;
				UserEventId++;
			}
			Sdl.SDL_Event evt = new Sdl.SDL_Event();
			evt.user = sdlev;
			if (Sdl.SDL_PushEvent(out evt) != (int) SdlFlag.Success)
			{
				throw SdlException.Generate();
			}
		}

		private static void DelegateEvent(ref Sdl.SDL_Event ev) 
		{
			switch (ev.type) 
			{
			case Sdl.SDL_ACTIVEEVENT:
				DelegateActiveEvent(instance, ref ev);
				break;
			case Sdl.SDL_JOYAXISMOTION:
				DelegateJoystickAxisMotion(instance, ref ev);
				break;
			case Sdl.SDL_JOYBALLMOTION:
				DelegateJoystickBallMotion(instance, ref ev);
				break;
			case Sdl.SDL_JOYBUTTONDOWN:
				DelegateJoystickButtonDown(instance, ref ev);
				break;
			case Sdl.SDL_JOYBUTTONUP:
				DelegateJoystickButtonUp(instance, ref ev);
				break;
			case Sdl.SDL_JOYHATMOTION:
				DelegateJoystickHatMotion(instance, ref ev);
				break;
			case Sdl.SDL_KEYDOWN:
				DelegateKeyDown(instance, ref ev);
				break;
			case Sdl.SDL_KEYUP:
				DelegateKeyUp(instance, ref ev);
				break;
			case Sdl.SDL_MOUSEBUTTONDOWN:
				DelegateMouseButtonDown(instance, ref ev);
				break;
			case Sdl.SDL_MOUSEBUTTONUP:
				DelegateMouseButtonUp(instance, ref ev);
				break;
			case Sdl.SDL_MOUSEMOTION:
				DelegateMouseMotion(instance, ref ev);
				break;
			case Sdl.SDL_QUIT:
				DelegateQuit(instance, ref ev);
				break;
			case Sdl.SDL_USEREVENT:
				DelegateUserEvent(instance, ref ev);
				break;
			case Sdl.SDL_VIDEOEXPOSE:
				DelegateVideoExpose(instance, ref ev);
				break;
			case Sdl.SDL_VIDEORESIZE:
				DelegateVideoResize(instance, ref ev);
				break;
			}
		}

		private static void DelegateActiveEvent(object sender,  ref Sdl.SDL_Event ev) 
		{
			if (AppActive != null || MouseFocus != null || InputFocus != null) 
			{
				bool activeGain = false;
				if (ev.active.gain != 0)
				{
					activeGain = true;
				}
				ActiveEventArgs e = new ActiveEventArgs(activeGain);

				if (((int)ev.active.state & 
					(int)Sdl.SDL_APPMOUSEFOCUS) != 0 && 
					MouseFocus != null)
				{
					MouseFocus(sender, e);
				}
				if (((int)ev.active.state & 
					(int)Sdl.SDL_APPINPUTFOCUS) != 0 && 
					InputFocus != null)
				{
					InputFocus(sender, e);
				}
				if (((int)ev.active.state & 
					(int)Sdl.SDL_APPACTIVE) != 0 && 
					AppActive != null)
				{
					AppActive(sender, e);
				}
			}
		}

		private static void DelegateJoystickAxisMotion(object sender, ref Sdl.SDL_Event ev) 
		{
			if (JoystickAxisMotion != null) 
			{
				if ((ev.jaxis.val < (-1)*JOYSTICK_THRESHHOLD) || (ev.jaxis.val > JOYSTICK_THRESHHOLD))
				{
					float jaxisValue = 
						(float)((int)ev.jaxis.val + JOYSTICK_ADJUSTMENT) / (float)JOYSTICK_SCALE;
					//Console.WriteLine("jaxisValue: " + jaxisValue.ToString());
					if (jaxisValue < 0)
					{
						jaxisValue = 0;
					}

					if (ev.jaxis.axis == 0)
					{

						JoystickAxisEventArgs e = 
							new JoystickAxisEventArgs(
							ev.jaxis.which, 
							ev.jaxis.axis, 
							jaxisValue);
						JoystickHorizontalAxisMotion(sender, e);
					}
					else if (ev.jaxis.axis == 1)
					{
						JoystickAxisEventArgs e = 
							new JoystickAxisEventArgs(
							ev.jaxis.which, 
							ev.jaxis.axis, 
							jaxisValue);
						JoystickVerticalAxisMotion(sender, e);
					}
					else
					{
						JoystickAxisEventArgs e = 
							new JoystickAxisEventArgs(
							ev.jaxis.which, 
							ev.jaxis.axis, 
							jaxisValue);
						JoystickAxisMotion(sender, e);
					}
				}
			}
		}

		private static void DelegateJoystickBallMotion(object sender, ref Sdl.SDL_Event ev) 
		{
			if (JoystickBallMotion != null) 
			{
				JoystickBallMotion(
					sender,
					new JoystickBallEventArgs(
					ev.jball.which, 
					ev.jball.ball,
					ev.jball.xrel, 
					ev.jball.yrel));
			}
		}

		private static void DelegateJoystickButtonDown(object sender, ref Sdl.SDL_Event ev) 
		{
			if (JoystickButton != null || JoystickButtonDown != null) 
			{
				JoystickButtonEventArgs e = 
					new JoystickButtonEventArgs(ev.jbutton.which, ev.jbutton.button, true);
				if (JoystickButton != null)
				{
					JoystickButton(sender, e);
				}
				if (JoystickButtonDown != null)
				{
					JoystickButtonDown(sender, e);
				}
			}
		}

		private static void DelegateJoystickButtonUp(object sender, ref Sdl.SDL_Event ev) 
		{
			if (JoystickButton != null || JoystickButtonUp != null) 
			{
				JoystickButtonEventArgs e = 
					new JoystickButtonEventArgs(ev.jbutton.which, ev.jbutton.button, false);
				if (JoystickButton != null)
				{
					JoystickButton(sender, e);
				}
				if (JoystickButtonUp != null)
				{
					JoystickButtonUp(sender, e);
				}
			}
		}

		private static void DelegateJoystickHatMotion(object sender, ref Sdl.SDL_Event ev) 
		{
			if (JoystickHatMotion != null) 
			{
				JoystickHatMotion(sender, new JoystickHatEventArgs(ev.jhat.which, ev.jhat.hat, (int)ev.jhat.val));
			}
		}

		private static void DelegateKeyDown(object sender, ref Sdl.SDL_Event ev) 
		{
			if (Keyboard != null || KeyboardDown != null) 
			{
				int device;
				int scancode;
				Key key;
				ModifierKeys mod;
				bool down;
				ParseKeyStruct(
					ref ev, out device, 
					out down, out scancode, 
					out key, out mod);
				KeyboardEventArgs e = new KeyboardEventArgs(device,  down, scancode, key, mod);
				
				if (Keyboard != null) 
				{
						Keyboard(sender, e);
				}
				if (KeyboardDown != null) 
				{
					KeyboardDown(sender, e);
				}
			}
		}

		private static void DelegateKeyUp(object sender, ref Sdl.SDL_Event ev) 
		{
			if (Keyboard != null || KeyboardUp != null) 
			{
				int device;
				int scancode;
				Key key;
				ModifierKeys mod;
				bool down;
				ParseKeyStruct(
					ref ev, out device, 
					out down, out scancode, 
					out key, out mod);
				KeyboardEventArgs e = new KeyboardEventArgs(device,  down, scancode, key, mod);
				if (Keyboard != null) 
				{
					Keyboard(sender, e);
				}
				if (KeyboardUp != null) 
				{
					KeyboardUp(sender, e);
				}
			}
		}

		private static void DelegateMouseButtonDown(object sender, ref Sdl.SDL_Event ev) 
		{
			if (MouseButton != null || MouseButtonDown != null) {
				int button;
				int x, y;
				ParseMouseStruct(ref ev, out button, out x, out y);

				MouseButtonEventArgs e = new MouseButtonEventArgs(button, true, x, y);

				if (MouseButton != null)
				{
					MouseButton(sender, e);
				}
				if (MouseButtonDown != null)
				{
					MouseButtonDown(sender, e);
				}
			}
		}

		private static void DelegateMouseButtonUp(object sender, ref Sdl.SDL_Event ev) 
		{
			if (MouseButton != null || MouseButtonUp != null) 
			{
				int button;
				int x, y;
				ParseMouseStruct(ref ev, out button, out x, out y);

				MouseButtonEventArgs e = new MouseButtonEventArgs(button, true, x, y);

				e.Button = button;
				e.Down = true;
				e.X = x;
				e.Y = y;

				if (MouseButton != null)
				{
					MouseButton(sender, e);
				}
				if (MouseButtonUp != null)
				{
					MouseButtonUp(sender, e);
				}
			}
		}

		private static void DelegateMouseMotion(object sender, ref Sdl.SDL_Event ev) 
		{
			if (MouseMotion != null) 
			{
				MouseMotion(
					sender, 
					new MouseMotionEventArgs(
					ev.motion.state, 
					ev.motion.x, 
					ev.motion.y, 
					ev.motion.xrel, 
					ev.motion.yrel));
			}
		}

		private static void DelegateQuit(object sender, ref Sdl.SDL_Event ev) {
			if (Quit != null) 
			{
				QuitEventArgs e = new QuitEventArgs();
				Quit(sender, e);
			}
		}

		private static void DelegateUserEvent(object sender, ref Sdl.SDL_Event ev) 
		{
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
					} else if (ret is MusicFinishedEventArgs) {
						if (MusicFinished != null)
						{
							MusicFinished(instance, (MusicFinishedEventArgs)ret);
						}
					} else
						UserEvent(sender, new UserEventArgs(ret));
				}
			}
		}

		private static void DelegateVideoExpose(object sender, ref Sdl.SDL_Event ev) 
		{
			if (Expose != null) 
			{
				Expose(sender, new ExposeEventArgs());
			}
		}

		private static void DelegateVideoResize(object sender, ref Sdl.SDL_Event ev) 
		{
			if (Resize != null) 
			{
				Resize(sender, new ResizeEventArgs(ev.resize.w, ev.resize.h));
			}
		}

		private static void ParseKeyStruct(
			ref Sdl.SDL_Event ev, 
			out int device, 
			out bool down, 
			out int scancode, 
			out Key key, 
			out ModifierKeys mod) 
		{
			device = ev.key.which;
			down = ((int)ev.key.state == (int)Sdl.SDL_PRESSED);
			Sdl.SDL_keysym ks = ev.key.keysym;
			scancode = ks.scancode;
			key = (Key)ks.sym;
			mod = (ModifierKeys)ks.mod;
		}

		private static void ParseMouseStruct(
			ref Sdl.SDL_Event ev, 
			out int button, 
			out int x, 
			out int y) 
		{
			button = (int)ev.button.button;
			x = ev.button.x;
			y = ev.button.y;
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
