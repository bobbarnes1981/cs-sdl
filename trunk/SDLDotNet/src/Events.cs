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
using Tao.Sdl;

namespace SdlDotNet {
	/// <summary>
	/// Represents the current state of all the mouse buttons
	/// </summary>
	public struct MouseButtonState 
	{
		private int _state;

		internal MouseButtonState(int state) 
		{
			_state = state;
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
			return (_state & (1 << ((int)button) - 1)) != 0;
		}
	}

	/// <summary>
	/// Indicates that the application has gained or lost input focus
	/// </summary>
	/// <param name="gained">
	/// True if the focus was gained, False if it was lost
	/// </param>
	public delegate void ActiveEventHandler(bool gained);

	/// <summary>
	/// Indicates that the keyboard state has changed
	/// </summary>
	/// <param name="device">The device index of the keyboard</param>
	/// <param name="down">True if the key is pressed, False if it was released</param>
	/// <param name="scancode">The scancode of the key</param>
	/// <param name="key">The Sdl virtual keycode</param>
	/// <param name="mod">Current modifier flags</param>
	public delegate void KeyboardEventHandler(int device, bool down, int scancode, Sdl.SDLKey key, Sdl.SDLMod mod);

	/// <summary>
	/// Indicates that the mouse has moved
	/// </summary>
	/// <param name="state">The current mouse button state</param>
	/// <param name="x">The current X coordinate</param>
	/// <param name="y">The current Y coordinate</param>
	/// <param name="relx">The difference between the last X coordinite and current</param>
	/// <param name="rely">The difference between the last Y coordinite and current</param>
	public delegate void MouseMotionEventHandler(MouseButtonState state, int x, int y, int relx, int rely);
	/// <summary>
	/// Indicates that a mouse button has been pressed or released
	/// </summary>
	/// <param name="button">The mouse button</param>
	/// <param name="down">True if the button is pressed, False if it is released</param>
	/// <param name="x">The current X coordinite</param>
	/// <param name="y">The current Y coordinite</param>
	public delegate void MouseButtonEventHandler(int button, bool down, int x, int y);
	/// <summary>
	/// Indicates that a joystick has moved on an axis
	/// </summary>
	/// <param name="device">The joystick index</param>
	/// <param name="axis">The axis index</param>
	/// <param name="val">The new axis value</param>
	public delegate void JoyAxisEventHandler(int device, int axis, int val);
	/// <summary>
	/// Indicates that a joystick button has been pressed or released
	/// </summary>
	/// <param name="device">The joystick index</param>
	/// <param name="button">The button index</param>
	/// <param name="down">True if the button was pressed, False if it was released</param>
	public delegate void JoyButtonEventHandler(int device, int button, bool down);
	/// <summary>
	/// Indicates a joystick hat has changed position
	/// </summary>
	/// <param name="device">The joystick index</param>
	/// <param name="hat">The hat index</param>
	/// <param name="val">The new hat position</param>
	public delegate void JoyHatEventHandler(int device, int hat, int val);
	/// <summary>
	/// Indicates a joystick trackball has changed position
	/// </summary>
	/// <param name="device">The joystick index</param>
	/// <param name="ball">The trackball index</param>
	/// <param name="xrel">The relative X position</param>
	/// <param name="yrel">The relative Y position</param>
	public delegate void JoyBallEventHandler(int device, int ball, int xrel, int yrel);
	/// <summary>
	/// Indicates the user has resized the window
	/// </summary>
	/// <param name="w">The new window width</param>
	/// <param name="h">The new window height</param>
	public delegate void ResizeEventHandler(int w, int h);
	/// <summary>
	/// Indicates that a portion of the window has been exposed
	/// </summary>
	public delegate void ExposeEventHandler();
	/// <summary>
	/// Indicates that the user has closed the main window
	/// </summary>
	public delegate void QuitEventHandler();
	/// <summary>
	/// Indicates a user event has fired
	/// </summary>
	/// <param name="userevent">The user event object</param>
	public delegate void UserEventHandler(object userevent);
	/// <summary>
	/// Indicates that a sound channel has stopped playing
	/// </summary>
	/// <param name="channel">The channel which has finished</param>
	public delegate void ChannelFinishedHandler(int channel);
	/// <summary>
	/// Indicates that a music sample has stopped playing
	/// </summary>
	public delegate void MusicFinishedHandler();

	/// <summary>
	/// Contains events which can be attached to to read user input and other miscellaneous occurances.
	/// You can obtain an instance of this class by accessing the Events property of the main Sdl object.
	/// You must call the PollAndDelegate() member in order for any events to fire.
	/// </summary>
	public class Events {
		private Hashtable _userevents = new Hashtable();
		private int _usereventid = 0;

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
		public event JoyAxisEventHandler JoyAxisMotion;
		/// <summary>
		/// Fires when a joystick button is pressed or released
		/// </summary>
		public event JoyButtonEventHandler JoyButton;
		/// <summary>
		/// Fires when a joystick button is pressed
		/// </summary>
		public event JoyButtonEventHandler JoyButtonDown;
		/// <summary>
		/// Fires when a joystick button is released
		/// </summary>
		public event JoyButtonEventHandler JoyButtonUp;
		/// <summary>
		/// Fires when a joystick hat changes
		/// </summary>
		public event JoyHatEventHandler JoyHatMotion;
		/// <summary>
		/// Fires when a joystick trackball changes
		/// </summary>
		public event JoyBallEventHandler JoyBallMotion;
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
		public event ChannelFinishedHandler ChannelFinished;
		/// <summary>
		/// Fires when a music sample finishes playing.
		/// Will only occur if you call Mixer.EnableMusicCallbacks().
		/// </summary>
		public event MusicFinishedHandler MusicFinished;

		static readonly Events instance = new Events();

		Events()
		{
		}

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
		/// <param name="ev">An opaque object representing a user-defined event.
		/// Will be passed back to the UserEvent handler delegate when this event is processed.</param>
		public void PushUserEvent(object ev) {
			Sdl.SDL_UserEvent sdlev;
			sdlev.type = (byte)Sdl.SDL_USEREVENT;
			lock (this) {
				_userevents[_usereventid] = ev;
				_usereventid++;
				sdlev.code = _usereventid;
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
				DelegateActiveEvent(ref ev);
				break;
			case Sdl.SDL_JOYAXISMOTION:
				DelegateJoyAxisMotion(ref ev);
				break;
			case Sdl.SDL_JOYBALLMOTION:
				DelegateJoyBallMotion(ref ev);
				break;
			case Sdl.SDL_JOYBUTTONDOWN:
				DelegateJoyButtonDown(ref ev);
				break;
			case Sdl.SDL_JOYBUTTONUP:
				DelegateJoyButtonUp(ref ev);
				break;
			case Sdl.SDL_JOYHATMOTION:
				DelegateJoyHatMotion(ref ev);
				break;
			case Sdl.SDL_KEYDOWN:
				DelegateKeyDown(ref ev);
				break;
			case Sdl.SDL_KEYUP:
				DelegateKeyUp(ref ev);
				break;
			case Sdl.SDL_MOUSEBUTTONDOWN:
				DelegateMouseButtonDown(ref ev);
				break;
			case Sdl.SDL_MOUSEBUTTONUP:
				DelegateMouseButtonUp(ref ev);
				break;
			case Sdl.SDL_MOUSEMOTION:
				DelegateMouseMotion(ref ev);
				break;
			case Sdl.SDL_QUIT:
				DelegateQuit(ref ev);
				break;
			case Sdl.SDL_USEREVENT:
				DelegateUserEvent(ref ev);
				break;
			case Sdl.SDL_VIDEOEXPOSE:
				DelegateVideoExpose(ref ev);
				break;
			case Sdl.SDL_VIDEORESIZE:
				DelegateVideoResize(ref ev);
				break;
			}
		}

		private void DelegateActiveEvent(ref Sdl.SDL_Event ev) 
		{
			if (AppActive != null || MouseFocus != null || InputFocus != null) 
			{
				if (((int)ev.active.state & 
					(int)Sdl.SDL_APPMOUSEFOCUS) != 0 && 
					MouseFocus != null)
				{
					MouseFocus(ev.active.gain != 0);
				}
				if (((int)ev.active.state & 
					(int)Sdl.SDL_APPINPUTFOCUS) != 0 && 
					InputFocus != null)
				{
					InputFocus(ev.active.gain != 0);
				}
				if (((int)ev.active.state & 
					(int)Sdl.SDL_APPACTIVE) != 0 && 
					AppActive != null)
				{
					AppActive(ev.active.gain != 0);
				}
			}
		}

		private void DelegateJoyAxisMotion(ref Sdl.SDL_Event ev) 
		{
			if (JoyAxisMotion != null) 
			{
				JoyAxisMotion(ev.jaxis.which, ev.jaxis.axis, ev.jaxis.val);
			}
		}

		private void DelegateJoyBallMotion(ref Sdl.SDL_Event ev) 
		{
			if (JoyBallMotion != null) 
			{
				JoyBallMotion(
					ev.jball.which, 
					ev.jball.ball,
					ev.jball.xrel, 
					ev.jball.yrel);
			}
		}

		private void DelegateJoyButtonDown(ref Sdl.SDL_Event ev) 
		{
			if (JoyButton != null || JoyButtonDown != null) 
			{
				if (JoyButton != null)
				{
					JoyButton(ev.jbutton.which, ev.jbutton.button, true);
				}
				if (JoyButtonDown != null)
				{
					JoyButtonDown(ev.jbutton.which, ev.jbutton.button, true);
				}
			}
		}

		private void DelegateJoyButtonUp(ref Sdl.SDL_Event ev) 
		{
			if (JoyButton != null || JoyButtonUp != null) 
			{
				if (JoyButton != null)
				{
					JoyButton(ev.jbutton.which, ev.jbutton.button, true);
				}
				if (JoyButtonUp != null)
				{
					JoyButtonUp(ev.jbutton.which, ev.jbutton.button, true);
				}
			}
		}

		private void DelegateJoyHatMotion(ref Sdl.SDL_Event ev) 
		{
			if (JoyHatMotion != null) 
			{
				JoyHatMotion(ev.jhat.which, ev.jhat.hat, (int)ev.jhat.val);
			}
		}

		private void DelegateKeyDown(ref Sdl.SDL_Event ev) 
		{
			if (Keyboard != null || KeyboardDown != null) 
			{
				int device;
				int scancode;
				Sdl.SDLKey key;
				Sdl.SDLMod mod;
				bool down;
				ParseKeyStruct(
					ref ev, out device, 
					out down, out scancode, 
					out key, out mod);
				if (Keyboard != null) 
				{
						Keyboard(device, down, scancode, key, mod);
				}
				if (KeyboardDown != null) 
				{
					KeyboardDown(device, down, scancode, key, mod);
				}
			}
		}

		private void DelegateKeyUp(ref Sdl.SDL_Event ev) 
		{
			if (Keyboard != null || KeyboardUp != null) 
			{
				int device;
				int scancode;
				Sdl.SDLKey key;
				Sdl.SDLMod mod;
				bool down;
				ParseKeyStruct(
					ref ev, out device, 
					out down, out scancode, 
					out key, out mod);
				if (Keyboard != null) 
				{
					Keyboard(device, down, scancode, key, mod);
				}
				if (KeyboardUp != null) 
				{
					KeyboardUp(device, down, scancode, key, mod);
				}
			}
		}

		private void DelegateMouseButtonDown(ref Sdl.SDL_Event ev) 
		{
			if (MouseButton != null || MouseButtonDown != null) {
				int button;
				int x, y;
				ParseMouseStruct(ref ev, out button, out x, out y);
				if (MouseButton != null)
				{
					MouseButton(button, true, x, y);
				}
				if (MouseButtonDown != null)
				{
					MouseButtonDown(button, true, x, y);
				}
			}
		}
		private void DelegateMouseButtonUp(ref Sdl.SDL_Event ev) 
		{
			if (MouseButton != null || MouseButtonUp != null) 
			{
				int button;
				int x, y;
				ParseMouseStruct(ref ev, out button, out x, out y);
				if (MouseButton != null)
				{
					MouseButton(button, true, x, y);
				}
				if (MouseButtonUp != null)
				{
					MouseButtonUp(button, true, x, y);
				}
			}
		}

		private void DelegateMouseMotion(ref Sdl.SDL_Event ev) 
		{
			if (MouseMotion != null) 
			{
				MouseMotion(
					new MouseButtonState(ev.motion.state), 
					ev.motion.x, 
					ev.motion.y, 
					ev.motion.xrel, 
					ev.motion.yrel);
			}
		}

		private void DelegateQuit(ref Sdl.SDL_Event ev) {
			if (Quit != null) 
			{
					Quit();
			}
		}
		private void DelegateUserEvent(ref Sdl.SDL_Event ev) {
			if (UserEvent != null) {
				object ret;
				lock (this) {
					ret = _userevents[ev.user.code];
				}
				if (ret != null) {
					if (ret is ChannelFinishedEvent) {
						if (ChannelFinished != null)
						{
							ChannelFinished(((ChannelFinishedEvent)ret).Channel);
						}
					} else if (ret is MusicFinishedEvent) {
						if (MusicFinished != null)
						{
							MusicFinished();
						}
					} else
						UserEvent(ret);
				}
			}
		}

		private void DelegateVideoExpose(ref Sdl.SDL_Event ev) 
		{
			if (Expose != null) 
			{
				Expose();
			}
		}

		private void DelegateVideoResize(ref Sdl.SDL_Event ev) 
		{
			if (Resize != null) 
			{
				Resize(ev.resize.w, ev.resize.h);
			}
		}

		private void ParseKeyStruct(
			ref Sdl.SDL_Event ev, 
			out int device, 
			out bool down, 
			out int scancode, 
			out Sdl.SDLKey key, 
			out Sdl.SDLMod mod) 
		{
			device = ev.key.which;
			down = ((int)ev.key.state == (int)Sdl.SDL_PRESSED);
			Sdl.SDL_keysym ks = ev.key.keysym;
			scancode = ks.scancode;
			key = (Sdl.SDLKey)ks.sym;
			mod = (Sdl.SDLMod)ks.mod;
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

		private class ChannelFinishedEvent 
		{
			public int Channel;
			public ChannelFinishedEvent(int channel) 
			{ 
				Channel = channel; 
			}
		}
		private class MusicFinishedEvent {}

//		internal void NotifyChannelFinished(int channel) {
//			PushUserEvent(new ChannelFinishedEvent(channel));
//		}
//		internal void NotifyMusicFinished() {
//			PushUserEvent(new MusicFinishedEvent());
//		}
	}
}
