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

namespace SdlDotNet {
	/// <summary>
	/// Represents the current state of all the mouse buttons
	/// </summary>
	public struct MouseButtonState 
	{
		private int state;

		internal MouseButtonState(int state) 
		{
			this.state = state;
		}

		/// <summary>
		/// Gets the pressed or released state of a mouse button
		/// </summary>
		/// <param name="button">The mouse button to check</param>
		/// <returns>
		/// If the button is pressed, returns True, otherwise returns False
		/// </returns>
		public bool IsButtonPressed(int button) 
		{
			return (state & (1 << ((int)button) - 1)) != 0;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return String.Format(CultureInfo.CurrentCulture, "({0})", state);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			if (obj.GetType() != typeof(MouseButtonState))
				return false;
                
			MouseButtonState mouseButtonState = (MouseButtonState)obj;   
			return (
				(this.state == mouseButtonState.state) 
				);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="mouseButtonState1"></param>
		/// <param name="mouseButtonState2"></param>
		/// <returns></returns>
		public static bool operator== (
			MouseButtonState mouseButtonState1, 
			MouseButtonState mouseButtonState2)
		{
			return (
				(mouseButtonState1.state == mouseButtonState2.state)
				);
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="mouseButtonState1"></param>
		/// <param name="mouseButtonState2"></param>
		/// <returns></returns>
		public static bool operator!= (
			MouseButtonState mouseButtonState1, 
			MouseButtonState mouseButtonState2)
		{
			return !(mouseButtonState1 == mouseButtonState2);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode()
		{
			return state;

		}
	}

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
	public class Events 
	{
		private Hashtable userEvents = new Hashtable();
		private int userEventId = 0;

		/// <summary>
		/// Fires when the application has become active or inactive
		/// </summary>
		public event ActiveEventHandler AppActive;
		/// <summary>
		/// Fires when the application gains or loses mouse focus
		/// </summary>
		public event ActiveEventHandler MouseFocus;
		/// <summary>
		/// Fires when the application gains or loses input focus
		/// </summary>
		public event ActiveEventHandler InputFocus;
		/// <summary>
		/// Fires when a key is pressed or released
		/// </summary>
		public event KeyboardEventHandler Keyboard;
		/// <summary>
		/// Fires when a key is pressed
		/// </summary>
		public event KeyboardEventHandler KeyboardDown;
		/// <summary>
		/// Fires when a key is released
		/// </summary>
		public event KeyboardEventHandler KeyboardUp;
		/// <summary>
		/// Fires when the mouse moves
		/// </summary>
		public event MouseMotionEventHandler MouseMotion;
		/// <summary>
		/// Fires when a mouse button is pressed or released
		/// </summary>
		public event MouseButtonEventHandler MouseButton;
		/// <summary>
		/// Fires when a mouse button is pressed
		/// </summary>
		public event MouseButtonEventHandler MouseButtonDown;
		/// <summary>
		/// Fires when a mouse button is released
		/// </summary>
		public event MouseButtonEventHandler MouseButtonUp;
		/// <summary>
		/// Fires when a joystick axis changes
		/// </summary>
		public event JoystickAxisEventHandler JoystickAxisMotion;
		/// <summary>
		/// Fires when a joystick button is pressed or released
		/// </summary>
		public event JoystickButtonEventHandler JoystickButton;
		/// <summary>
		/// Fires when a joystick button is pressed
		/// </summary>
		public event JoystickButtonEventHandler JoystickButtonDown;
		/// <summary>
		/// Fires when a joystick button is released
		/// </summary>
		public event JoystickButtonEventHandler JoystickButtonUp;
		/// <summary>
		/// Fires when a joystick hat changes
		/// </summary>
		public event JoystickHatEventHandler JoystickHatMotion;
		/// <summary>
		/// Fires when a joystick trackball changes
		/// </summary>
		public event JoystickBallEventHandler JoystickBallMotion;
		/// <summary>
		/// Fires when the user resizes the window
		/// </summary>
		public event ResizeEventHandler Resize;
		/// <summary>
		/// Fires when a portion of the window is uncovered
		/// </summary>
		public event ExposeEventHandler Expose;
		/// <summary>
		/// Fires when a user closes the window
		/// </summary>
		public event QuitEventHandler Quit;
		/// <summary>
		/// Fires when a user event is consumed
		/// </summary>
		public event UserEventHandler UserEvent;
		/// <summary>
		/// Fires when a sound channel finishes playing.
		/// Will only occur if you call Mixer.EnableMusicCallbacks().
		/// </summary>
		public event ChannelFinishedEventHandler ChannelFinished;
		/// <summary>
		/// Fires when a music sample finishes playing.
		/// Will only occur if you call Mixer.EnableMusicCallbacks().
		/// </summary>
		public event MusicFinishedEventHandler MusicFinished;

		static readonly Events instance = new Events();

		Events()
		{
		}

		/// <summary>
		/// 
		/// </summary>
		public static Events Instance
		{
			get 
			{
				return instance;
			}
		}

		/// <summary>
		/// Checks the event queue, and processes 
		/// any events it finds by invoking the event properties
		/// </summary>
		/// <returns>
		/// True if any events were in the queue, otherwise False
		/// </returns>
		public bool PollAndDelegate() 
		{
			Sdl.SDL_Event ev;
			int ret = Sdl.SDL_PollEvent(out ev);
			if (ret == -1)
			{
				throw SdlException.Generate();
			}
			if (ret == 0)
			{
				return false;
			}
			DelegateEvent(ref ev);
			return true;
		}

		/// <summary>
		/// Checks the event queue, and waits until an event is available
		/// </summary>
		public void WaitAndDelegate() 
		{
			Sdl.SDL_Event ev;
			if (Sdl.SDL_WaitEvent(out ev) == 0)
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
		public void PushUserEvent(object userEvent) {
			Sdl.SDL_UserEvent sdlev = new Sdl.SDL_UserEvent();
			sdlev.type = (byte)Sdl.SDL_USEREVENT;
			lock (this) {
				this.userEvents[userEventId] = userEvent;
				sdlev.code = userEventId;
				userEventId++;
			}
			Sdl.SDL_Event evt = new Sdl.SDL_Event();
			evt.user = sdlev;
			if (Sdl.SDL_PushEvent(out evt) != 0)
			{
				throw SdlException.Generate();
			}
		}

		private void DelegateEvent(ref Sdl.SDL_Event ev) 
		{
			switch (ev.type) 
			{
			case Sdl.SDL_ACTIVEEVENT:
				DelegateActiveEvent(this, ref ev);
				break;
			case Sdl.SDL_JOYAXISMOTION:
				DelegateJoystickAxisMotion(this, ref ev);
				break;
			case Sdl.SDL_JOYBALLMOTION:
				DelegateJoystickBallMotion(this, ref ev);
				break;
			case Sdl.SDL_JOYBUTTONDOWN:
				DelegateJoystickButtonDown(this, ref ev);
				break;
			case Sdl.SDL_JOYBUTTONUP:
				DelegateJoystickButtonUp(this, ref ev);
				break;
			case Sdl.SDL_JOYHATMOTION:
				DelegateJoystickHatMotion(this, ref ev);
				break;
			case Sdl.SDL_KEYDOWN:
				DelegateKeyDown(this, ref ev);
				break;
			case Sdl.SDL_KEYUP:
				DelegateKeyUp(this, ref ev);
				break;
			case Sdl.SDL_MOUSEBUTTONDOWN:
				DelegateMouseButtonDown(this, ref ev);
				break;
			case Sdl.SDL_MOUSEBUTTONUP:
				DelegateMouseButtonUp(this, ref ev);
				break;
			case Sdl.SDL_MOUSEMOTION:
				DelegateMouseMotion(this, ref ev);
				break;
			case Sdl.SDL_QUIT:
				DelegateQuit(this, ref ev);
				break;
			case Sdl.SDL_USEREVENT:
				DelegateUserEvent(this, ref ev);
				break;
			case Sdl.SDL_VIDEOEXPOSE:
				DelegateVideoExpose(this, ref ev);
				break;
			case Sdl.SDL_VIDEORESIZE:
				DelegateVideoResize(this, ref ev);
				break;
			}
		}

		private void DelegateActiveEvent(object sender,  ref Sdl.SDL_Event ev) 
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

		private void DelegateJoystickAxisMotion(object sender, ref Sdl.SDL_Event ev) 
		{
			if (JoystickAxisMotion != null) 
			{
				JoystickAxisEventArgs e = 
					new JoystickAxisEventArgs(ev.jaxis.which, ev.jaxis.axis, ev.jaxis.val);
				JoystickAxisMotion(sender, e);
			}
		}

		private void DelegateJoystickBallMotion(object sender, ref Sdl.SDL_Event ev) 
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

		private void DelegateJoystickButtonDown(object sender, ref Sdl.SDL_Event ev) 
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

		private void DelegateJoystickButtonUp(object sender, ref Sdl.SDL_Event ev) 
		{
			if (JoystickButton != null || JoystickButtonUp != null) 
			{
				JoystickButtonEventArgs e = 
					new JoystickButtonEventArgs(ev.jbutton.which, ev.jbutton.button, true);
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

		private void DelegateJoystickHatMotion(object sender, ref Sdl.SDL_Event ev) 
		{
			if (JoystickHatMotion != null) 
			{
				JoystickHatMotion(sender, new JoystickHatEventArgs(ev.jhat.which, ev.jhat.hat, (int)ev.jhat.val));
			}
		}

		private void DelegateKeyDown(object sender, ref Sdl.SDL_Event ev) 
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

		private void DelegateKeyUp(object sender, ref Sdl.SDL_Event ev) 
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

		private void DelegateMouseButtonDown(object sender, ref Sdl.SDL_Event ev) 
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

		private void DelegateMouseButtonUp(object sender, ref Sdl.SDL_Event ev) 
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

		private void DelegateMouseMotion(object sender, ref Sdl.SDL_Event ev) 
		{
			if (MouseMotion != null) 
			{
				MouseMotion(
					sender, 
					new MouseMotionEventArgs(
					new MouseButtonState(ev.motion.state), 
					ev.motion.x, 
					ev.motion.y, 
					ev.motion.xrel, 
					ev.motion.yrel));
			}
		}

		private void DelegateQuit(object sender, ref Sdl.SDL_Event ev) {
			if (Quit != null) 
			{
				QuitEventArgs e = new QuitEventArgs();
				Quit(sender, e);
			}
		}
		private void DelegateUserEvent(object sender, ref Sdl.SDL_Event ev) 
		{
			if (UserEvent != null || ChannelFinished != null || MusicFinished != null) 
			{
				object ret;
				lock (this) 
				{
					ret = this.userEvents[ev.user.code];
				}
				if (ret != null) 
				{
					if (ret is ChannelFinishedEventArgs) 
					{
						if (ChannelFinished != null)
						{
							ChannelFinished(this, (ChannelFinishedEventArgs)ret);
						}
					} else if (ret is MusicFinishedEventArgs) {
						if (MusicFinished != null)
						{
							MusicFinished(this, (MusicFinishedEventArgs)ret);
						}
					} else
						UserEvent(sender, new UserEventArgs(ret));
				}
			}
		}

		private void DelegateVideoExpose(object sender, ref Sdl.SDL_Event ev) 
		{
			if (Expose != null) 
			{
				Expose(sender, new ExposeEventArgs());
			}
		}

		private void DelegateVideoResize(object sender, ref Sdl.SDL_Event ev) 
		{
			if (Resize != null) 
			{
				Resize(sender, new ResizeEventArgs(ev.resize.w, ev.resize.h));
			}
		}

		private void ParseKeyStruct(
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

		private void ParseMouseStruct(
			ref Sdl.SDL_Event ev, 
			out int button, 
			out int x, 
			out int y) 
		{
			button = (int)ev.button.button;
			x = ev.button.x;
			y = ev.button.y;
		}

		internal void NotifyChannelFinished(int channel) 
		{
			PushUserEvent(new ChannelFinishedEventArgs(channel));
		}
		internal void NotifyMusicFinished() 
		{
			PushUserEvent(new MusicFinishedEventArgs());
		}
	}
}
