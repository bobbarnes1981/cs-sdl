using System;

namespace SDLDotNet {
	/// <summary>
	/// Contains methods for interacting with the window title frame and for grabbing input focus.
	/// These methods do not need a windowed display to be called.
	/// You can obtain an instance of this class by accessing the WindowManager property of the main SDL
	/// object.
	/// </summary>
	/// <type>class</type>
	unsafe public class WindowManager {
		internal WindowManager() {}

		/// <summary>
		/// gets or sets the text for the current window
		/// </summary>
		/// <proptype>System.String</proptype>
		public string Caption {
			get{
				string ret, dummy;
				Natives.SDL_WM_GetCaption(out ret, out dummy);
				return ret;
			}
			set{
				Natives.SDL_WM_SetCaption(value, "");
			}
		}
		/// <summary>
		/// sets the icon for the current window
		/// </summary>
		/// <param name="icon">the surface containing the image</param>
		public void SetIcon(Surface icon) {
			Natives.SDL_WM_SetIcon(icon.GetPtr(), null);
		}
		/// <summary>
		/// Iconifies (minimizes) the current window
		/// </summary>
		/// <returntype>System.Boolean</returntype>
		/// <returns>True if the action succeeded, otherwise False</returns>
		public bool IconifyWindow() {
			return (Natives.SDL_WM_IconifyWindow() != 0);
		}
		/// <summary>
		/// Forces keyboard focus and prevents the mouse from leaving the window
		/// </summary>
		public void GrabInput() {
			Natives.SDL_WM_GrabInput((int)Natives.GrabInput.On);
		}
		/// <summary>
		/// Releases keyboard and mouse focus from a previous call to GrabInput()
		/// </summary>
		public void ReleaseInput() {
			Natives.SDL_WM_GrabInput((int)Natives.GrabInput.Off);
		}
	}
}
