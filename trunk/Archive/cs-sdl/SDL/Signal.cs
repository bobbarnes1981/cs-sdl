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
using System.Runtime.InteropServices;

namespace CsGL.SDL
{
	public abstract class Signal
	{
		[DllImport("csgl-sdl-native.dll", EntryPoint="DMexit")]
		public static extern void exit(int error);

		/// <summary>
		/// Function which could be executed at program exit.
		/// </summary>
		public delegate void ExitHandler();
		
		[DllImport("csgl-sdl-native.dll", EntryPoint="DMatexit")]
		public static extern void atexit(ExitHandler cleaner);
		
		[DllImport("csgl-sdl-native.dll", EntryPoint="DMsignal")]
		public static extern void signal(Value s, Handler sd);
		[DllImport("csgl-sdl-native.dll", EntryPoint="DMsignal")]
		public static extern void signal(Value s, Handler2 sd);
		
		public static void IgnoreSignal(Value sig, bool defaultHandler)
		{
			signal(sig, defaultHandler ? SIG_DFL : SIG_IGN);
		}

		/// <summary> list of signal that could most commonly be raised </summary>
		public enum Value {
			SIGINT   = 2,	/* Interactive attention */
			SIGILL   = 4,	/* Illegal instruction */
			SIGFPE   = 8,	/* Floating point error */
			SIGSEGV  = 11,	/* Segmentation violation */
			SIGTERM  = 15,	/* Termination request */
			SIGBREAK = 21,	/* Control-break */
			SIGABRT  = 22	/* Abnormal termination (abort) */
		}

		/// <summary> Signal handler, use exit() to stop or 
		/// return to continue </summary>
		public delegate void Handler(Value sig);
		/// <summary> Signal handler, use exit() to stop or 
		/// return to continue </summary>
		public delegate void Handler2(int sig);
		
		static readonly int SIG_DFL = 0;
		static readonly int SIG_IGN = 1;
		[DllImport("csgl-sdl-native.dll", EntryPoint="DMsignal")]
		static extern void signal(Value s, int todo);
	}
}
