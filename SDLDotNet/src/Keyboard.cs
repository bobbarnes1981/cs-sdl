using System;

namespace SdlDotNet
{
	/// <summary>
	/// Summary description for Keyboard.
	/// </summary>
	public class Keyboard
	{
		/// <summary>
		/// Returns the global instance of this class.
		/// </summary>
		static readonly Keyboard instance = new Keyboard();

		Keyboard() 
		{
		}

		/// <summary>
		/// 
		/// </summary>
		public static Keyboard Instance
		{
			get
			{
				return instance;
			}
		}
		/// <summary>
		/// Enable keyboard autorepeat
		/// </summary>
		/// <param name="delay">
		/// Delay in system ticks before repeat starts. 
		/// Set to 0 to disable key repeat.
		/// </param>
		/// <param name="rate">
		/// Rate in system ticks at which key repeats.
		/// </param>
		public static void EnableKeyRepeat(int delay, int rate) 
		{
			if (Sdl.SDL_EnableKeyRepeat(delay, rate) == -1)
			{
				throw SdlException.Generate();
			}
		}
	}
}
