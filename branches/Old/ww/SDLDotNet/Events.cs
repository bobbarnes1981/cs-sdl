using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace SDLDotNet {
	/// <summary>
	/// Represents the current state of all the mouse buttons
	/// </summary>
	/// <type>struct</type>
	public struct MouseButtonState {
		private int _state;
		internal MouseButtonState(int state) {
			_state = state;
		}
		/// <summary>
		/// Gets the pressed or released state of a mouse button
		/// </summary>
		/// <param name="button">The mouse button to check</param>
		/// <returntype>System.Boolean</returntype>
		/// <returns>If the button is pressed, returns True, otherwise returns False</returns>
		public bool IsButtonPressed(MouseButton button) {
			return (_state & (1 << ((int)button) - 1)) != 0;
		}
	}

	/// <summary>
	/// Indicates that the application has gained or lost input focus
	/// </summary>
	/// <proto>System.Void (System.Boolean)</proto>
	/// <param name="gained">True if the focus was gained, False if it was lost</param>
	/// <type>delegate</type>
	public delegate void ActiveEventHandler(bool gained);
	/// <summary>
	/// Indicates that the keyboard state has changed
	/// </summary>
	/// <proto>System.Void (System.Int32,System.Boolean,System.Int32,SDLDotNet.Key,SDLDotNet.Mod)</proto>
	/// <param name="device">The device index of the keyboard</param>
	/// <param name="down">True if the key is pressed, False if it was released</param>
	/// <param name="scancode">The scancode of the key</param>
	/// <param name="key">The SDL virtual keycode</param>
	/// <param name="mod">Current modifier flags</param>
	/// <type>delegate</type>
	public delegate void KeyboardEventHandler(int device, bool down, int scancode, Key key, Mod mod);
	/// <summary>
	/// Indicates that the mouse has moved
	/// </summary>
	/// <proto>System.Void (SDLDotNet.MouseButtonState,System.Int32,System.Int32,System.Int32,System.Int32)</proto>
	/// <param name="state">The current mouse button state</param>
	/// <param name="x">The current X coordinite</param>
	/// <param name="y">The current Y coordinite</param>
	/// <param name="relx">The difference between the last X coordinite and current</param>
	/// <param name="rely">The difference between the last Y coordinite and current</param>
	/// <type>delegate</type>
	public delegate void MouseMotionEventHandler(MouseButtonState state, int x, int y, int relx, int rely);
	/// <summary>
	/// Indicates that a mouse button has been pressed or released
	/// </summary>
	/// <proto>System.Void (SDLDotNet.MouseButton,System.Boolean,System.Int32,System.Int32)</proto>
	/// <param name="button">The mouse button</param>
	/// <param name="down">True if the button is pressed, False if it is released</param>
	/// <param name="x">The current X coordinite</param>
	/// <param name="y">The current Y coordinite</param>
	/// <type>delegate</type>
	public delegate void MouseButtonEventHandler(MouseButton button, bool down, int x, int y);
	/// <summary>
	/// Indicates that a joystick has moved on an axis
	/// </summary>
	/// <proto>System.Void (System.Int32,System.Int32,System.Int32)</proto>
	/// <param name="device">The joystick index</param>
	/// <param name="axis">The axis index</param>
	/// <param name="val">The new axis value</param>
	/// <type>delegate</type>
	public delegate void JoyAxisEventHandler(int device, int axis, int val);
	/// <summary>
	/// Indicates that a joystick button has been pressed or released
	/// </summary>
	/// <proto>System.Void (System.Int32,System.Int32,System.Boolean)</proto>
	/// <param name="device">The joystick index</param>
	/// <param name="button">The button index</param>
	/// <param name="down">True if the button was pressed, False if it was released</param>
	/// <type>delegate</type>
	public delegate void JoyButtonEventHandler(int device, int button, bool down);
	/// <summary>
	/// Indicates a joystick hat has changed position
	/// </summary>
	/// <proto>System.Void (System.Int32,System.Int32,SDLDotNet.HatPos)</proto>
	/// <param name="device">The joystick index</param>
	/// <param name="hat">The hat index</param>
	/// <param name="val">The new hat position</param>
	/// <type>delegate</type>
	public delegate void JoyHatEventHandler(int device, int hat, HatPos val);
	/// <summary>
	/// Indicates a joystick trackball has changed position
	/// </summary>
	/// <proto>System.Void (System.Int32,System.Int32,System.Int32,System.Int32)</proto>
	/// <param name="device">The joystick index</param>
	/// <param name="ball">The trackball index</param>
	/// <param name="xrel">The relative X position</param>
	/// <param name="yrel">The relative Y position</param>
	/// <type>delegate</type>
	public delegate void JoyBallEventHandler(int device, int ball, int xrel, int yrel);
	/// <summary>
	/// Indicates the user has resized the window
	/// </summary>
	/// <proto>System.Void (System.Int32,System.Int32)</proto>
	/// <param name="w">The new window width</param>
	/// <param name="h">The new window height</param>
	/// <type>delegate</type>
	public delegate void ResizeEventHandler(int w, int h);
	/// <summary>
	/// Indicates that a portion of the window has been exposed
	/// </summary>
	/// <proto>System.Void ()</proto>
	/// <type>delegate</type>
	public delegate void ExposeEventHandler();
	/// <summary>
	/// Indicates that the user has closed the main window
	/// </summary>
	/// <proto>System.Void ()</proto>
	/// <type>delegate</type>
	public delegate void QuitEventHandler();
	/// <summary>
	/// Indicates a user event has fired
	/// </summary>
	/// <proto>System.Void (System.Object)</proto>
	/// <param name="userevent">The user event object</param>
	/// <type>delegate</type>
	public delegate void UserEventHandler(object userevent);
	/// <summary>
	/// Indicates that a sound channel has stopped playing
	/// </summary>
	/// <proto>System.Void (System.Int32)</proto>
	/// <param name="channel">The channel which has finished</param>
	/// <type>delegate</type>
	public delegate void ChannelFinishedHandler(int channel);
	/// <summary>
	/// Indicates that a music sample has stopped playing
	/// </summary>
	/// <proto>System.Void ()</proto>
	/// <type>delegate</type>
	public delegate void MusicFinishedHandler();

	/// <summary>
	/// Contains events which can be attached to to read user input and other miscellaneous occurances.
	/// You can obtain an instance of this class by accessing the Events property of the main SDL object.
	/// You must call the PollAndDelegate() member in order for any events to fire.
	/// </summary>
	/// <type>class</type>
	unsafe public class Events {
		private Hashtable _userevents;
		private int _usereventid;

		/// <summary>
		/// Fires when the application has become active or inactive
		/// </summary>
		/// <eventtype>SDLDotNet.ActiveEventHandler</eventtype>
		public event ActiveEventHandler AppActive;
		/// <summary>
		/// Fires when the application gains or loses mouse focus
		/// </summary>
		/// <eventtype>SDLDotNet.ActiveEventHandler</eventtype>
		public event ActiveEventHandler MouseFocus;
		/// <summary>
		/// Fires when the application gains or loses input focus
		/// </summary>
		/// <eventtype>SDLDotNet.ActiveEventHandler</eventtype>
		public event ActiveEventHandler InputFocus;
		/// <summary>
		/// Fires when a key is pressed or released
		/// </summary>
		/// <eventtype>SDLDotNet.KeyboardEventHandler</eventtype>
		public event KeyboardEventHandler Keyboard;
		/// <summary>
		/// Fires when a key is pressed
		/// </summary>
		/// <eventtype>SDLDotNet.KeyboardEventHandler</eventtype>
		public event KeyboardEventHandler KeyboardDown;
		/// <summary>
		/// Fires when a key is released
		/// </summary>
		/// <eventtype>SDLDotNet.KeyboardEventHandler</eventtype>
		public event KeyboardEventHandler KeyboardUp;
		/// <summary>
		/// Fires when the mouse moves
		/// </summary>
		/// <eventtype>SDLDotNet.MouseMotionEventHandler</eventtype>
		public event MouseMotionEventHandler MouseMotion;
		/// <summary>
		/// Fires when a mouse button is pressed or released
		/// </summary>
		/// <eventtype>SDLDotNet.MouseButtonEventHandler</eventtype>
		public event MouseButtonEventHandler MouseButton;
		/// <summary>
		/// Fires when a mouse button is pressed
		/// </summary>
		/// <eventtype>SDLDotNet.MouseButtonEventHandler</eventtype>
		public event MouseButtonEventHandler MouseButtonDown;
		/// <summary>
		/// Fires when a mouse button is released
		/// </summary>
		/// <eventtype>SDLDotNet.MouseButtonEventHandler</eventtype>
		public event MouseButtonEventHandler MouseButtonUp;
		/// <summary>
		/// Fires when a joystick axis changes
		/// </summary>
		/// <eventtype>SDLDotNet.JoyAxisEventHandler</eventtype>
		public event JoyAxisEventHandler JoyAxisMotion;
		/// <summary>
		/// Fires when a joystick button is pressed or released
		/// </summary>
		/// <eventtype>SDLDotNet.JoyButtonEventHandler</eventtype>
		public event JoyButtonEventHandler JoyButton;
		/// <summary>
		/// Fires when a joystick button is pressed
		/// </summary>
		/// <eventtype>SDLDotNet.JoyButtonEventHandler</eventtype>
		public event JoyButtonEventHandler JoyButtonDown;
		/// <summary>
		/// Fires when a joystick button is released
		/// </summary>
		/// <eventtype>SDLDotNet.JoyButtonEventHandler</eventtype>
		public event JoyButtonEventHandler JoyButtonUp;
		/// <summary>
		/// Fires when a joystick hat changes
		/// </summary>
		/// <eventtype>SDLDotNet.JoyHatEventHandler</eventtype>
		public event JoyHatEventHandler JoyHatMotion;
		/// <summary>
		/// Fires when a joystick trackball changes
		/// </summary>
		/// <eventtype>SDLDotNet.JoyBallEventHandler</eventtype>
		public event JoyBallEventHandler JoyBallMotion;
		/// <summary>
		/// Fires when the user resizes the window
		/// </summary>
		/// <eventtype>SDLDotNet.ResizeEventHandler</eventtype>
		public event ResizeEventHandler Resize;
		/// <summary>
		/// Fires when a portion of the window is uncovered
		/// </summary>
		/// <eventtype>SDLDotNet.ExposeEventHandler</eventtype>
		public event ExposeEventHandler Expose;
		/// <summary>
		/// Fires when a user closes the window
		/// </summary>
		/// <eventtype>SDLDotNet.QuitEventHandler</eventtype>
		public event QuitEventHandler Quit;
		/// <summary>
		/// Fires when a user event is consumed
		/// </summary>
		/// <eventtype>SDLDotNet.UserEventHandler</eventtype>
		public event UserEventHandler UserEvent;
		/// <summary>
		/// Fires when a sound channel finishes playing.
		/// Will only occur if you call Mixer.EnableMusicCallbacks().
		/// </summary>
		/// <eventtype>SDLDotNet.ChannelFinishedHandler</eventtype>
		public event ChannelFinishedHandler ChannelFinished;
		/// <summary>
		/// Fires when a music sample finishes playing.
		/// Will only occur if you call Mixer.EnableMusicCallbacks().
		/// </summary>
		/// <eventtype>SDLDotNet.MusicFinishedHandler</eventtype>
		public event MusicFinishedHandler MusicFinished;

		internal Events() {
			_userevents = new Hashtable();
			_usereventid = 0;
		}

		/// <summary>
		/// Checks the event queue, and processes any events it finds by invoking the event properties
		/// </summary>
		/// <returntype>System.Boolean</returntype>
		/// <returns>True if any events were in the queue, otherwise False</returns>
		public bool PollAndDelegate() {
			Natives.SDL_Event ev;
			if (Natives.SDL_PollEvent(&ev) == 0)
				return false;
			DelegateEvent(&ev);
			return true;
		}

		/// <summary>
		/// Pushes a user-defined event on the event queue
		/// </summary>
		/// <param name="ev">An opaque object representing a user-defined event.
		/// Will be passed back to the UserEvent handler delegate when this event is processed.</param>
		public void PushUserEvent(object ev) {
			Natives.SDL_UserEvent sdlev;
			sdlev.type = (byte)Natives.Event.USEREVENT;
			lock (this) {
				_userevents[_usereventid] = ev;
				_usereventid++;
				sdlev.code = _usereventid;
			}
			if (Natives.SDL_PushEvent((Natives.SDL_Event *)&sdlev) != 0)
				throw SDLException.Generate();
		}

		private void DelegateEvent(Natives.SDL_Event *ev) {
			switch ((Natives.Event)ev->type) {
			case Natives.Event.ACTIVEEVENT:
				DelegateActiveEvent(ev);
				break;
			case Natives.Event.JOYAXISMOTION:
				DelegateJoyAxisMotion(ev);
				break;
			case Natives.Event.JOYBALLMOTION:
				DelegateJoyBallMotion(ev);
				break;
			case Natives.Event.JOYBUTTONDOWN:
				DelegateJoyButtonDown(ev);
				break;
			case Natives.Event.JOYBUTTONUP:
				DelegateJoyButtonUp(ev);
				break;
			case Natives.Event.JOYHATMOTION:
				DelegateJoyHatMotion(ev);
				break;
			case Natives.Event.KEYDOWN:
				DelegateKeyDown(ev);
				break;
			case Natives.Event.KEYUP:
				DelegateKeyUp(ev);
				break;
			case Natives.Event.MOUSEBUTTONDOWN:
				DelegateMouseButtonDown(ev);
				break;
			case Natives.Event.MOUSEBUTTONUP:
				DelegateMouseButtonUp(ev);
				break;
			case Natives.Event.MOUSEMOTION:
				DelegateMouseMotion(ev);
				break;
			case Natives.Event.QUIT:
				DelegateQuit(ev);
				break;
			case Natives.Event.USEREVENT:
				DelegateUserEvent(ev);
				break;
			case Natives.Event.VIDEOEXPOSE:
				DelegateVideoExpose(ev);
				break;
			case Natives.Event.VIDEORESIZE:
				DelegateVideoResize(ev);
				break;
			}
		}

		private void DelegateActiveEvent(Natives.SDL_Event *ev) {
			if (AppActive != null || MouseFocus != null || InputFocus != null) {
				Natives.SDL_ActiveEvent *aev = (Natives.SDL_ActiveEvent *)ev;
				if (((int)aev->state & (int)Natives.Focus.MouseFocus) != 0 && MouseFocus != null)
					MouseFocus(aev->gain != 0);
				if (((int)aev->state & (int)Natives.Focus.InputFocus) != 0 && InputFocus != null)
					InputFocus(aev->gain != 0);
				if (((int)aev->state & (int)Natives.Focus.AppActive) != 0 && AppActive != null)
					AppActive(aev->gain != 0);
			}
		}
		private void DelegateJoyAxisMotion(Natives.SDL_Event *ev) {
			if (JoyAxisMotion != null) {
				Natives.SDL_JoyAxisEvent *jev = (Natives.SDL_JoyAxisEvent *)ev;
				JoyAxisMotion(jev->which, jev->axis, jev->val);
			}
		}
		private void DelegateJoyBallMotion(Natives.SDL_Event *ev) {
			if (JoyBallMotion != null) {
				Natives.SDL_JoyBallEvent *jev = (Natives.SDL_JoyBallEvent *)ev;
				JoyBallMotion(jev->which, jev->ball, jev->xrel, jev->yrel);
			}
		}
		private void DelegateJoyButtonDown(Natives.SDL_Event *ev) {
			if (JoyButton != null || JoyButtonDown != null) {
				Natives.SDL_JoyButtonEvent *jev = (Natives.SDL_JoyButtonEvent *)ev;
				if (JoyButton != null)
					JoyButton(jev->which, jev->button, true);
				if (JoyButtonDown != null)
					JoyButtonDown(jev->which, jev->button, true);
			}
		}
		private void DelegateJoyButtonUp(Natives.SDL_Event *ev) {
			if (JoyButton != null || JoyButtonUp != null) {
				Natives.SDL_JoyButtonEvent *jev = (Natives.SDL_JoyButtonEvent *)ev;
				if (JoyButton != null)
					JoyButton(jev->which, jev->button, true);
				if (JoyButtonUp != null)
					JoyButtonUp(jev->which, jev->button, true);
			}
		}
		private void DelegateJoyHatMotion(Natives.SDL_Event *ev) {
			if (JoyHatMotion != null) {
				Natives.SDL_JoyHatEvent *jev = (Natives.SDL_JoyHatEvent *)ev;
				JoyHatMotion(jev->which, jev->hat, (HatPos)jev->val);
			}
		}
		private void DelegateKeyDown(Natives.SDL_Event *ev) {
			if (Keyboard != null || KeyboardDown != null) {
				int device, scancode;
				Key key;
				Mod mod;
				bool down;
				ParseKeyStruct(ev, out device, out down, out scancode, out key, out mod);
				if (Keyboard != null) Keyboard(device, down, scancode, key, mod);
				if (KeyboardDown != null) KeyboardDown(device, down, scancode, key, mod);
			}
		}
		private void DelegateKeyUp(Natives.SDL_Event *ev) {
			if (Keyboard != null || KeyboardUp != null) {
				int device, scancode;
				Key key;
				Mod mod;
				bool down;
				ParseKeyStruct(ev, out device, out down, out scancode, out key, out mod);
				if (Keyboard != null) Keyboard(device, down, scancode, key, mod);
				if (KeyboardUp != null) KeyboardUp(device, down, scancode, key, mod);
			}
		}
		private void DelegateMouseButtonDown(Natives.SDL_Event *ev) {
			if (MouseButton != null || MouseButtonDown != null) {
				MouseButton button;
				int x, y;
				ParseMouseStruct(ev, out button, out x, out y);
				if (MouseButton != null)
					MouseButton(button, true, x, y);
				if (MouseButtonDown != null)
					MouseButtonDown(button, true, x, y);
			}
		}
		private void DelegateMouseButtonUp(Natives.SDL_Event *ev) {
			if (MouseButton != null || MouseButtonUp != null) {
				MouseButton button;
				int x, y;
				ParseMouseStruct(ev, out button, out x, out y);
				if (MouseButton != null)
					MouseButton(button, true, x, y);
				if (MouseButtonUp != null)
					MouseButtonUp(button, true, x, y);
			}
		}
		private void DelegateMouseMotion(Natives.SDL_Event *ev) {
			if (MouseMotion != null) {
				Natives.SDL_MouseMotionEvent *mev = (Natives.SDL_MouseMotionEvent *)ev;
				MouseMotion(new MouseButtonState(mev->state), mev->x, mev->y, mev->xrel, mev->yrel);
			}
		}
		private void DelegateQuit(Natives.SDL_Event *ev) {
			if (Quit != null) Quit();
		}
		private void DelegateUserEvent(Natives.SDL_Event *ev) {
			if (UserEvent != null) {
				Natives.SDL_UserEvent *uev = (Natives.SDL_UserEvent *)ev;
				object ret;
				lock (this) {
					ret = _userevents[uev->code];
				}
				if (ret != null) {
					if (ret is ChannelFinishedEvent) {
						if (ChannelFinished != null)
							ChannelFinished(((ChannelFinishedEvent)ret).Channel);
					} else if (ret is MusicFinishedEvent) {
						if (MusicFinished != null)
							MusicFinished();
					} else
						UserEvent(ret);
				}
			}
		}
		private void DelegateVideoExpose(Natives.SDL_Event *ev) {
			if (Expose != null) Expose();
		}
		private void DelegateVideoResize(Natives.SDL_Event *ev) {
			if (Resize != null) {
				Natives.SDL_ResizeEvent *rev = (Natives.SDL_ResizeEvent *)ev;
				Resize(rev->w, rev->h);
			}
		}

		private void ParseKeyStruct(Natives.SDL_Event *ev, out int device, out bool down, out int scancode, out Key key, out Mod mod) {
			Natives.SDL_KeyboardEvent *kev = (Natives.SDL_KeyboardEvent *)ev;
			device = kev->which;
			down = ((int)kev->state == (int)Natives.KeyState.Pressed);
			Natives.SDL_keysym ks = kev->keysym;
			scancode = ks.scancode;
			key = (Key)ks.sym;
			mod = (Mod)ks.mod;
		}

		private void ParseMouseStruct(Natives.SDL_Event *ev, out MouseButton button, out int x, out int y) {
			Natives.SDL_MouseButtonEvent *mev = (Natives.SDL_MouseButtonEvent *)ev;
			button = (MouseButton)mev->button;
			x = mev->x;
			y = mev->y;
		}

		private class ChannelFinishedEvent {
			public int Channel;
			public ChannelFinishedEvent(int channel) { Channel = channel; }
		}
		private class MusicFinishedEvent {}

		internal void NotifyChannelFinished(int channel) {
			PushUserEvent(new ChannelFinishedEvent(channel));
		}
		internal void NotifyMusicFinished() {
			PushUserEvent(new MusicFinishedEvent());
		}
	}
}
