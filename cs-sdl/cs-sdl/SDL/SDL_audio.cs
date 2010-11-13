/*
    SDL - Simple DirectMedia Layer
    Copyright (C) 1997, 1998, 1999, 2000, 2001  Sam Lantinga

    This library is free software; you can redistribute it and/or
    modify it under the terms of the GNU Library General Public
    License as published by the Free Software Foundation; either
    version 2 of the License, or (at your option) any later version.

    This library is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
    Library General Public License for more details.

    You should have received a copy of the GNU Library General Public
    License along with this library; if not, write to the Free
    Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA

    Sam Lantinga
    slouken@devolution.com
*/
using System;
using System.Collections;
using System.IO;
using System.Runtime.InteropServices;

namespace CsGL.SDL
{
	// look into class SDL_mixer
	// audio is not implemented due to bug with interop & callback...
	// but SDL_mixer do not need callback.
	public class SDL_audio
	{
		/* Audio format flags (defaults to LSB byte order) */
		public const ushort AUDIO_U8 = 0x0008;	/* Unsigned 8-bit samples */
		public const ushort AUDIO_S8 = 0x8008;	/* Signed 8-bit samples */
		public const ushort AUDIO_U16LSB = 0x0010;	/* Unsigned 16-bit samples */
		public const ushort AUDIO_S16LSB = 0x8010;	/* Signed 16-bit samples */
		public const ushort AUDIO_U16MSB = 0x1010;	/* As above, but big-endian byte order */
		public const ushort AUDIO_S16MSB = 0x9010;	/* As above, but big-endian byte order */
		public const ushort AUDIO_U16	= AUDIO_U16LSB;
		public const ushort AUDIO_S16	= AUDIO_S16LSB;
		
		/* Native audio byte ordering */
		public static readonly ushort AUDIO_U16SYS;
		public static readonly ushort AUDIO_S16SYS;
		
		static SDL_audio()
		{
			if(SDL_byteorder.SysLittleEndian) {
				AUDIO_U16SYS = AUDIO_U16LSB;
				AUDIO_S16SYS = AUDIO_S16LSB;
			}
			else {
				AUDIO_U16SYS = AUDIO_U16MSB;
				AUDIO_S16SYS = AUDIO_S16MSB;
			}
		}
	}
}
