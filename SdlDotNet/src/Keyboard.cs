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
using System.Text;
using System.Text.RegularExpressions;
using Tao.Sdl;

namespace SdlDotNet
{
	/// <summary>
	/// Keyboard class
	/// </summary>
	public sealed class Keyboard
	{
		Keyboard()
		{}

		static Keyboard()
		{
			Video.Initialize();
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
		/// <remarks>This method will initialize the Video subsystem as well.</remarks>
		public static void EnableKeyRepeat(int delay, int rate) 
		{
			if (Sdl.SDL_EnableKeyRepeat(delay, rate) == (int) SdlFlag.Error)
			{
				throw SdlException.Generate();
			}
		}

		/// <summary>
		/// Returns true if the application has keyboard focus.
		/// </summary>
		public static bool HasFocus
		{
			get
			{
				return (Sdl.SDL_GetAppState() & Sdl.SDL_APPINPUTFOCUS) !=0;
			}
		}

		/// <summary>
		/// Returns the actual keyboard character that was pressed.
		/// </summary>
		/// <param name="key">Key to translate into the actual keyboard character.</param>
		/// <returns>Actual keyvboard character that was pressed.</returns>
		public static string KeyboardCharacter(Key key)
		{
			return Sdl.SDL_GetKeyName((int)key);
		}

		/// <summary>
		/// Returns which modifier keys are pressed
		/// </summary>
		public static ModifierKeys ModifierKeyState
		{
			get
			{
				return (ModifierKeys) Sdl.SDL_GetModState();
			}
			set
			{
				Sdl.SDL_SetModState((int)value);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public static string ShortToUnicode(short input)
		{
			Encoding unicode;
			unicode = Encoding.Unicode;
			string hexString = Convert.ToString(input, 16);
			string finalString = "";
			if (hexString.Length >= 2)
			{
				finalString =  Regex.Unescape(@"\x" + hexString);
			}
			
			byte[] codes;
			char[] chars = new char[0];
			codes = unicode.GetBytes(finalString);
			chars = unicode.GetChars(codes);
			if (chars.Length > 0 )
			{
				return chars[0].ToString();
			}
			else
			{
				return "";
			}
		}

		/// <summary>
		/// Checks key state
		/// </summary>
		/// <param name="key">Key to check</param>
		/// <returns>True if key is pressed</returns>
		public static bool IsKeyPressed(Key key)
		{
			int numberOfKeys;
			byte[] keys;
			keys = Sdl.SDL_GetKeyState(out numberOfKeys);
			if (keys[(int)key] == 1)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// Checks if Unicode is enabled
		/// </summary>
		public static bool UnicodeEnabled
		{
			get
			{
				return (Sdl.SDL_EnableUNICODE(-1) == 1);
			}
			set
			{
				if (value == true)
				{
					Sdl.SDL_EnableUNICODE(1);
				}
				else
				{
					Sdl.SDL_EnableUNICODE(0);
				}
			}
		}
	}
}
