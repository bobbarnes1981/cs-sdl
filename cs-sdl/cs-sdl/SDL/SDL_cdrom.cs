/*
 * BSD Licence:
 * Copyright (c) 2001, Jan Nockemann (jnockemann@gmx.net)
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
using System;
using System.Runtime.InteropServices;

namespace CsGL.SDL
{
	/* The possible states which a CD-ROM drive can be in. */
	public enum CDstatus {
		CD_TRAYEMPTY,
		CD_STOPPED,
		CD_PLAYING,
		CD_PAUSED,
		CD_ERROR = -1
	}
	
	[StructLayout(LayoutKind.Sequential)]
	public struct SDL_CDtrack {
		public byte id;		/* Track number */
		public byte type;		/* Data or audio track */
		public ushort unused;
		public uint length;		/* Length, in frames, of this track */
		public uint offset;		/* Offset, in frames, from start of disk */
	}
	
	/* This structure is only current as of the last call to SDL_CDStatus() */
	/// because of interop consideration this c-struct couldn't be a C#-struct...
	[StructLayout(LayoutKind.Sequential)]
	public unsafe class SDL_CD {
		public int id;			/* Private drive identifier */
		public CDstatus status;	/* Current drive status */
	
		/* The rest of this structure is only valid if there's a CD in drive */
		public int numtracks;		/* Number of tracks on disk */
		public int cur_track;		/* Current track position */
		public int cur_frame;		/* Current frame offset within current track */
		[MarshalAs(UnmanagedType.ByValArray, SizeConst=SDL_MAX_TRACKS+1)] 
		public SDL_CDtrack[] track;

		/* Audio format flags (defaults to LSB byte order) */
		public const ushort AUDIO_U8	= 0x0008;	/* Unsigned 8-bit samples */
		public const ushort AUDIO_S8	= 0x8008;	/* Signed 8-bit samples */
		public const ushort AUDIO_U16LSB	= 0x0010;	/* Unsigned 16-bit samples */
		public const ushort AUDIO_S16LSB	= 0x8010;	/* Signed 16-bit samples */
		public const ushort AUDIO_U16MSB	= 0x1010;	/* As above, but big-endian byte order */
		public const ushort AUDIO_S16MSB	= 0x9010;	/* As above, but big-endian byte order */

		public static readonly ushort AUDIO_U16SYS = (SDL_byteorder.SysLittleEndian ? AUDIO_U16LSB : AUDIO_U16MSB);
		public static readonly ushort AUDIO_S16SYS = (SDL_byteorder.SysLittleEndian ? AUDIO_S16LSB : AUDIO_S16MSB);

		/* The maximum number of CD-ROM tracks on a disk */
		public const byte SDL_MAX_TRACKS	= 99;

		/* The types of CD-ROM track possible */
		public const byte SDL_AUDIO_TRACK	= 0x00;
		public const byte SDL_DATA_TRACK	= 0x04;
			
		public const byte CD_FPS	= 75;
		
		public static bool CD_INDRIVE(CDstatus status)
		{
			return ((int)status > 0);
		}
		
		
		/* Conversion functions from frames to Minute/Second/Frames and vice versa */
		// Lloyd: make a new function with out's ?
		/*#define FRAMES_TO_MSF(f, M,S,F)	{					\
				int value = f;							\
				*(F) = value%CD_FPS;						\
				value /= CD_FPS;						\
				*(S) = value%60;						\
				value /= 60;							\
				*(M) = value;							\
			}
			*/
		public static void FRAMES_TO_MSF(int f, out int M, out int S, out int F) {
				int val = f;
				F = val%CD_FPS;
				val /= CD_FPS;
				S = val%60;
				val /= 60;
				M = val;
		}
		public static void FRAMES_TO_MSF(int f, int* M, int* S, int* F) {
				int val = f;
				*F = val%CD_FPS;
				val /= CD_FPS;
				*S = val%60;
				val /= 60;
				*M = val;
		}
		/*#define MSF_TO_FRAMES(M, S, F)	((M)*60*CD_FPS+(S)*CD_FPS+(F))*/
		public static int MSF_TO_FRAMES(int M, int S, int F) {
			return M*60*CD_FPS+S*CD_FPS+F;
		}
		
		/* CD-audio API functions: */

		/* Returns the number of CD-ROM drives on the system, or -1 if
		   SDL_Init() has not been called with the SDL_INIT_CDROM flag.
		 */
		[DllImport("SDL.dll")]
		public static extern int SDL_CDNumDrives();
		
		/* Returns a human-readable, system-dependent identifier for the CD-ROM.
		   Example:
			"/dev/cdrom"
			"E:"
			"/dev/disk/ide/1/master"
		*/
		[DllImport("SDL.dll")]
		public static extern string SDL_CDName(int drive);

		/* Opens a CD-ROM drive for access.  It returns a drive handle on success,
		   or NULL if the drive was invalid or busy.  This newly opened CD-ROM
		   becomes the default CD used when other CD functions are passed a NULL
		   CD-ROM handle.
		   Drives are numbered starting with 0.  Drive 0 is the system default CD-ROM.
		*/
		[DllImport("SDL.dll")]
		public static unsafe extern SDL_CD SDL_CDOpen(int drive);
			
		/* This function returns the current status of the given drive.
		   If the drive has a CD in it, the table of contents of the CD and current
		   play position of the CD will be stored in the SDL_CD structure.
		*/
		[DllImport("SDL.dll")]
		public static unsafe extern CDstatus SDL_CDStatus(SDL_CD cdrom);
			
		/* Play the given CD starting at 'start_track' and 'start_frame' for 'ntracks'
		   tracks and 'nframes' frames.  If both 'ntrack' and 'nframe' are 0, play 
		   until the end of the CD.  This function will skip data tracks.
		   This function should only be called after calling SDL_CDStatus() to 
		   get track information about the CD.
		   For example:
			// Play entire CD:
			if ( CD_INDRIVE(SDL_CDStatus(cdrom)) )
				SDL_CDPlayTracks(cdrom, 0, 0, 0, 0);
			// Play last track:
			if ( CD_INDRIVE(SDL_CDStatus(cdrom)) ) {
				SDL_CDPlayTracks(cdrom, cdrom->numtracks-1, 0, 0, 0);
			}
			// Play first and second track and 10 seconds of third track:
			if ( CD_INDRIVE(SDL_CDStatus(cdrom)) )
				SDL_CDPlayTracks(cdrom, 0, 0, 2, 10);
		
		   This function returns 0, or -1 if there was an error.
		*/
		[DllImport("SDL.dll")]
		public static unsafe extern int SDL_CDPlayTracks(SDL_CD cdrom,
				int start_track, int start_frame, int ntracks, int nframes);
		
		/* Play the given CD starting at 'start' frame for 'length' frames.
		   It returns 0, or -1 if there was an error.
		*/
		[DllImport("SDL.dll")]
		public static unsafe extern int SDL_CDPlay(SDL_CD cdrom, int start, int length);
		
		/* Pause play -- returns 0, or -1 on error */
		[DllImport("SDL.dll")]
		public static unsafe extern int SDL_CDPause(SDL_CD cdrom);
		
		/* Resume play -- returns 0, or -1 on error */
		[DllImport("SDL.dll")]
		public static unsafe extern int SDL_CDResume(SDL_CD cdrom);
		
		/* Stop play -- returns 0, or -1 on error */
		[DllImport("SDL.dll")]
		public static unsafe extern int SDL_CDStop(SDL_CD cdrom);
		
		/* Eject CD-ROM -- returns 0, or -1 on error */
		[DllImport("SDL.dll")]
		public static unsafe extern int SDL_CDEject(SDL_CD cdrom);
		
		/* Closes the handle for the CD-ROM drive */
		[DllImport("SDL.dll")]
		public static unsafe extern void SDL_CDClose(SDL_CD cdrom);
	}
}
