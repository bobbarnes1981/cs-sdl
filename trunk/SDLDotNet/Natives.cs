using System;
using System.Runtime.InteropServices;
using System.Security;

namespace SDLDotNet {
	unsafe class Natives {
		// enums
		public enum Init {
			Timer = 0x00000001,
			Audio = 0x00000010,
			Video = 0x00000020,
			Cdrom = 0x00000100,
			Joystick = 0x00000200
		}
		public enum Video {
			SWSurface = 0x00000000,
			HWSurface = 0x00000001,
			AsyncBlit = 0x00000004,
			AnyFormat = 0x10000000,
			HWPallete = 0x20000000,
			DoubleBuf = 0x40000000,
			FullScreen = -0x7FFFFFFF,
			OpenGL = 0x00000002,
			OpenGLBlit = 0x0000000A,
			Resizable = 0x00000010,
			NoFrame = 0x00000020,
			RLEAccel = 0x00004000
		}
		public enum Event {
			NOEVENT = 0,
			ACTIVEEVENT,
			KEYDOWN,
			KEYUP,
			MOUSEMOTION,
			MOUSEBUTTONDOWN,
			MOUSEBUTTONUP,
			JOYAXISMOTION,
			JOYBALLMOTION,
			JOYHATMOTION,
			JOYBUTTONDOWN,
			JOYBUTTONUP,
			QUIT,
			SYSWMEVENT,
			EVENT_RESERVEDA,
			EVENT_RESERVEDB,
			VIDEORESIZE,
			VIDEOEXPOSE,
			EVENT_RESERVED2,
			EVENT_RESERVED3,
			EVENT_RESERVED4,
			EVENT_RESERVED5,
			EVENT_RESERVED6,
			EVENT_RESERVED7,
			USEREVENT = 24,
			NUMEVENTS = 32
		}
		public enum Enable {
			Query = -1,
			Ignore = 0,
			Disable = 0,
			Enable = 1
		}
		public enum ColorKey {
			SrcColorKey = 0x00001000,
			RLEAccelOK = 0x00002000
		}
		public enum KeyState {
			Pressed = 1,
			Released = 0
		}
		public enum Focus {
			MouseFocus = 1,
			InputFocus = 2,
			AppActive = 4
		}
		public enum GrabInput {
			Query = -1,
			Off = 0,
			On = 1
		}
		
		// delegates
		public delegate void ChannelFinishedDelegate(int channel);
		public delegate void MusicFinishedDelegate();

		// structs
		[StructLayout(LayoutKind.Sequential)]
		public struct SDL_Rect {
			public SDL_Rect(System.Drawing.Rectangle r) {
				x = (short)r.X;
				y = (short)r.Y;
				w = (ushort)r.Width;
				h = (ushort)r.Height;
			}
			public short x, y;
			public ushort w, h;
		}
		[StructLayout(LayoutKind.Sequential)]
		public struct SDL_PixelFormat {
			public IntPtr palette;
			public byte BitsPerPixel;
			public byte BytesPerPixel;
			public byte Rloss;
			public byte Gloss;
			public byte Bloss;
			public byte Aloss;
			public byte Rshift;
			public byte Gshift;
			public byte Bshift;
			public byte Ashift;
			public uint Rmask;
			public uint Gmask;
			public uint Bmask;
			public uint Amask;
			public uint colorkey;
			public byte alpha;
		}
		[StructLayout(LayoutKind.Sequential)]
		public struct SDL_Surface {
			public uint flags;
			public SDL_PixelFormat *format;
			public int w, h;
			public ushort pitch;
			public IntPtr pixels;
			public int offset;
			public IntPtr hwdata;
			public SDL_Rect clip_rect;
			public uint unused1;
			public uint locked;
			public IntPtr map;
			public uint format_version;
			public int refcount;
		}
		[StructLayout(LayoutKind.Sequential)]
		public struct SDL_keysym {
			public byte scancode;
			public int sym;
			public int mod;
			public ushort unicode;
		}
		[StructLayout(LayoutKind.Sequential)]
		public struct SDL_Event {
			public byte type;
			public IntPtr d1;
			public IntPtr d2;
			public IntPtr d3;
			public IntPtr d4;
			public IntPtr d5;
			public IntPtr d6;
		}
		[StructLayout(LayoutKind.Sequential)]
		public struct SDL_ActiveEvent {
			public byte type;
			public byte gain;
			public byte state;
		}
		[StructLayout(LayoutKind.Sequential)]
		public struct SDL_KeyboardEvent {
			public byte type;
			public byte which;
			public byte state;
			public SDL_keysym keysym;
		}
		[StructLayout(LayoutKind.Sequential)]
		public struct SDL_MouseMotionEvent {
			public byte type;
			public byte which;
			public byte state;
			public ushort x, y;
			public short xrel;
			public short yrel;
		}
		[StructLayout(LayoutKind.Sequential)]
		public struct SDL_MouseButtonEvent {
			public byte type;
			public byte which;
			public byte button;
			public byte state;
			public ushort x, y;
		}
		[StructLayout(LayoutKind.Sequential)]
		public struct SDL_JoyAxisEvent {
			public byte type;
			public byte which;
			public byte axis;
			public short val;
		}
		[StructLayout(LayoutKind.Sequential)]
		public struct SDL_JoyBallEvent {
			public byte type;
			public byte which;
			public byte ball;
			public short xrel;
			public short yrel;
		}
		[StructLayout(LayoutKind.Sequential)]
		public struct SDL_JoyHatEvent {
			public byte type;
			public byte which;
			public byte hat;
			public byte val;
		}
		[StructLayout(LayoutKind.Sequential)]
		public struct SDL_JoyButtonEvent {
			public byte type;
			public byte which;
			public byte button;
			public byte state;
		}
		[StructLayout(LayoutKind.Sequential)]
		public struct SDL_ResizeEvent {
			public byte type;
			public int w;
			public int h;
		}
		[StructLayout(LayoutKind.Sequential)]
		public struct SDL_ExposeEvent {
			public byte type;
		}
		[StructLayout(LayoutKind.Sequential)]
		public struct SDL_QuitEvent {
			public byte type;
		}
		[StructLayout(LayoutKind.Sequential)]
		public struct SDL_UserEvent {
			public byte type;
			public int code;
			public void *data1;
			public void *data2;
		}
		[StructLayout(LayoutKind.Sequential)]
		public struct SDL_CDtrack {
			public byte id;
			public byte type;
			public ushort unused;
			public uint length;
			public uint offset;
		}
		[StructLayout(LayoutKind.Sequential)]
		public struct SDL_CD {
			public int id;
			public int status;
			public int numtracks;
			public int cur_track;
			public int cur_frame;
			[MarshalAs(UnmanagedType.ByValArray, SizeConst=1200)]
			public byte[] track;
		};

#if __MONO__
		const string SDL_DLL = "SDL";
		const string MIX_DLL = "SDL_mixer";
#else
		const string SDL_DLL = "SDL.dll";
		const string MIX_DLL = "SDL_mixer.dll";
#endif

		// General
		[DllImport(SDL_DLL), SuppressUnmanagedCodeSecurity]
		public static extern int SDL_Init(int flags);
		[DllImport(SDL_DLL), SuppressUnmanagedCodeSecurity]
		public static extern int SDL_InitSubSystem(int flags);
		[DllImport(SDL_DLL), SuppressUnmanagedCodeSecurity]
		public static extern int SDL_Quit();
		[DllImport(SDL_DLL), SuppressUnmanagedCodeSecurity]
		public static extern string SDL_GetError();
		[DllImport(SDL_DLL), SuppressUnmanagedCodeSecurity]
		public static extern uint SDL_GetTicks();

		// Video
		[DllImport(SDL_DLL), SuppressUnmanagedCodeSecurity]
		public static extern SDL_Surface *SDL_SetVideoMode(int width, int height, int bpp, int flags);
		[DllImport(SDL_DLL), SuppressUnmanagedCodeSecurity]
		public static extern void SDL_FreeSurface(SDL_Surface *surface);
		[DllImport(SDL_DLL), SuppressUnmanagedCodeSecurity]
		public static extern int SDL_Flip(SDL_Surface *screen);
		[DllImport(SDL_DLL), SuppressUnmanagedCodeSecurity]
		public static extern int SDL_FillRect(SDL_Surface *surface, SDL_Rect *rect, uint color);
		[DllImport(SDL_DLL), SuppressUnmanagedCodeSecurity]
		public static extern uint SDL_MapRGBA(SDL_PixelFormat *fmt, byte r, byte g, byte b, byte a);
		[DllImport(SDL_DLL), SuppressUnmanagedCodeSecurity]
		public static extern void SDL_GetRGBA(uint pixel, SDL_PixelFormat *fmt, out byte r, out byte g, out byte b, out byte a);
		[DllImport(SDL_DLL), SuppressUnmanagedCodeSecurity]
		public static extern int SDL_ShowCursor(int toggle);
		[DllImport(SDL_DLL), SuppressUnmanagedCodeSecurity]
		public static extern void SDL_WarpMouse(ushort x, ushort y);
		[DllImport(SDL_DLL), SuppressUnmanagedCodeSecurity]
		public static extern SDL_Surface *SDL_CreateRGBSurface(int flags, int width, int height, int depth, uint Rmask, uint Gmask, uint Bmask, uint Amask);
		[DllImport(SDL_DLL, EntryPoint="SDL_UpperBlit"), SuppressUnmanagedCodeSecurity]
		public static extern int SDL_BlitSurface(SDL_Surface *src, SDL_Rect *srcrect, SDL_Surface *dst, SDL_Rect *dstrect);
		[DllImport(SDL_DLL), SuppressUnmanagedCodeSecurity]
		public static extern SDL_Surface *SDL_GetVideoSurface();
		[DllImport(SDL_DLL), SuppressUnmanagedCodeSecurity]
		public static extern int SDL_LockSurface(SDL_Surface *surface);
		[DllImport(SDL_DLL), SuppressUnmanagedCodeSecurity]
		public static extern int SDL_UnlockSurface(SDL_Surface *surface);
		[DllImport(SDL_DLL), SuppressUnmanagedCodeSecurity]
		public static extern SDL_Surface *SDL_LoadBMP_RW(IntPtr ops, int freesrc);
		[DllImport(SDL_DLL), SuppressUnmanagedCodeSecurity]
		public static extern int SDL_SaveBMP_RW(SDL_Surface *surface, IntPtr dst, int freedst);
		[DllImport(SDL_DLL), SuppressUnmanagedCodeSecurity]
		public static extern int SDL_SetColorKey(SDL_Surface *surface, int flag, uint key);
		[DllImport(SDL_DLL), SuppressUnmanagedCodeSecurity]
		public static extern void SDL_SetClipRect(SDL_Surface *surface, SDL_Rect *rect);
		[DllImport(SDL_DLL), SuppressUnmanagedCodeSecurity]
		public static extern void SDL_GetClipRect(SDL_Surface *surface, SDL_Rect *rect);
		[DllImport(SDL_DLL), SuppressUnmanagedCodeSecurity]
		public static extern SDL_Surface *SDL_DisplayFormat(SDL_Surface *surface);
		[DllImport(SDL_DLL), SuppressUnmanagedCodeSecurity]
		public static extern SDL_Surface *SDL_ConvertSurface(SDL_Surface *src, SDL_PixelFormat *fmt, int flags);

		// RWops
		[DllImport(SDL_DLL), SuppressUnmanagedCodeSecurity]
		public static extern IntPtr SDL_RWFromFile(string file, string mode);
		[DllImport(SDL_DLL), SuppressUnmanagedCodeSecurity]
		public static extern IntPtr SDL_RWFromMem(byte[] mem, int size);

		// Events
		[DllImport(SDL_DLL), SuppressUnmanagedCodeSecurity]
		public static extern int SDL_PollEvent(SDL_Event *evt);
		[DllImport(SDL_DLL), SuppressUnmanagedCodeSecurity]
		public static extern int SDL_WaitEvent(SDL_Event *evt);
		[DllImport(SDL_DLL), SuppressUnmanagedCodeSecurity]
		public static extern int SDL_PushEvent(SDL_Event *evt);

		// WM
		[DllImport(SDL_DLL), SuppressUnmanagedCodeSecurity]
		public static extern void SDL_WM_SetCaption(string title, string icon);
		[DllImport(SDL_DLL), SuppressUnmanagedCodeSecurity]
		public static extern void SDL_WM_GetCaption(out string title, out string icon);
		[DllImport(SDL_DLL), SuppressUnmanagedCodeSecurity]
		public static extern void SDL_WM_SetIcon(SDL_Surface *icon, byte *mask);
		[DllImport(SDL_DLL), SuppressUnmanagedCodeSecurity]
		public static extern int SDL_WM_IconifyWindow();
		[DllImport(SDL_DLL), SuppressUnmanagedCodeSecurity]
		public static extern int SDL_WM_GrabInput(int grab);

		// Joystick
		[DllImport(SDL_DLL), SuppressUnmanagedCodeSecurity]
		public static extern int SDL_NumJoysticks();
		[DllImport(SDL_DLL), SuppressUnmanagedCodeSecurity]
		public static extern string SDL_JoystickName(int index);
		[DllImport(SDL_DLL), SuppressUnmanagedCodeSecurity]
		public static extern IntPtr SDL_JoystickOpen(int index);
		[DllImport(SDL_DLL), SuppressUnmanagedCodeSecurity]
		public static extern int SDL_JoystickIndex(IntPtr joystick);
		[DllImport(SDL_DLL), SuppressUnmanagedCodeSecurity]
		public static extern int SDL_JoystickNumAxes(IntPtr joystick);
		[DllImport(SDL_DLL), SuppressUnmanagedCodeSecurity]
		public static extern int SDL_JoystickNumBalls(IntPtr joystick);
		[DllImport(SDL_DLL), SuppressUnmanagedCodeSecurity]
		public static extern int SDL_JoystickNumHats(IntPtr joystick);
		[DllImport(SDL_DLL), SuppressUnmanagedCodeSecurity]
		public static extern int SDL_JoystickNumButtons(IntPtr joystick);
		[DllImport(SDL_DLL), SuppressUnmanagedCodeSecurity]
		public static extern void SDL_JoystickClose(IntPtr joystick);

		// OpenGL
		[DllImport(SDL_DLL), SuppressUnmanagedCodeSecurity]
		public static extern void SDL_GL_SwapBuffers();
		[DllImport(SDL_DLL), SuppressUnmanagedCodeSecurity]
		public static extern int SDL_GL_SetAttribute(int attrib, int val);
		[DllImport(SDL_DLL), SuppressUnmanagedCodeSecurity]
		public static extern int SDL_GL_GetAttribute(int attrib, out int val);

		// CD-Rom
		[DllImport(SDL_DLL), SuppressUnmanagedCodeSecurity]
		public static extern int SDL_CDNumDrives();
		[DllImport(SDL_DLL), SuppressUnmanagedCodeSecurity]
		public static extern string SDL_CDName(int drive);
		[DllImport(SDL_DLL), SuppressUnmanagedCodeSecurity]
		public static extern IntPtr SDL_CDOpen(int drive);
		[DllImport(SDL_DLL), SuppressUnmanagedCodeSecurity]
		public static extern int SDL_CDStatus(IntPtr cdrom);
		[DllImport(SDL_DLL), SuppressUnmanagedCodeSecurity]
		public static extern int SDL_CDPlay(IntPtr cdrom, int start, int length);
		[DllImport(SDL_DLL), SuppressUnmanagedCodeSecurity]
		public static extern int SDL_CDPlayTracks(IntPtr cdrom, int start_track, int start_frame, int ntracks, int nframes);
		[DllImport(SDL_DLL), SuppressUnmanagedCodeSecurity]
		public static extern int SDL_CDPause(IntPtr cdrom);
		[DllImport(SDL_DLL), SuppressUnmanagedCodeSecurity]
		public static extern int SDL_CDResume(IntPtr cdrom);
		[DllImport(SDL_DLL), SuppressUnmanagedCodeSecurity]
		public static extern int SDL_CDStop(IntPtr cdrom);
		[DllImport(SDL_DLL), SuppressUnmanagedCodeSecurity]
		public static extern int SDL_CDEject(IntPtr cdrom);
		[DllImport(SDL_DLL), SuppressUnmanagedCodeSecurity]
		public static extern void SDL_CDClose(IntPtr cdrom);

		// Mixer
		[DllImport(MIX_DLL), SuppressUnmanagedCodeSecurity]
		public static extern int Mix_OpenAudio(int freq, ushort format, int channels, int chucksize);
		[DllImport(MIX_DLL), SuppressUnmanagedCodeSecurity]
		public static extern int Mix_CloseAudio();
		[DllImport(MIX_DLL), SuppressUnmanagedCodeSecurity]
		public static extern IntPtr Mix_LoadWAV_RW(IntPtr ops, int freesrc);
		[DllImport(MIX_DLL), SuppressUnmanagedCodeSecurity]
		public static extern int Mix_VolumeChunk(IntPtr chunk, int volume);
		[DllImport(MIX_DLL), SuppressUnmanagedCodeSecurity]
		public static extern void Mix_FreeChunk(IntPtr chunk);
		[DllImport(MIX_DLL), SuppressUnmanagedCodeSecurity]
		public static extern int Mix_AllocateChannels(int num);
		[DllImport(MIX_DLL), SuppressUnmanagedCodeSecurity]
		public static extern int Mix_Volume(int channel, int volume);
		[DllImport(MIX_DLL), SuppressUnmanagedCodeSecurity]
		public static extern int Mix_PlayChannelTimed(int channel, IntPtr chunk, int loops, int ticks);
		[DllImport(MIX_DLL), SuppressUnmanagedCodeSecurity]
		public static extern int Mix_FadeInChannelTimed(int channel, IntPtr chunk, int loops, int ms, int ticks);
		[DllImport(MIX_DLL), SuppressUnmanagedCodeSecurity]
		public static extern void Mix_Pause(int channel);
		[DllImport(MIX_DLL), SuppressUnmanagedCodeSecurity]
		public static extern void Mix_Resume(int channel);
		[DllImport(MIX_DLL), SuppressUnmanagedCodeSecurity]
		public static extern int Mix_HaltChannel(int channel);
		[DllImport(MIX_DLL), SuppressUnmanagedCodeSecurity]
		public static extern int Mix_ExpireChannel(int channel, int ticks);
		[DllImport(MIX_DLL), SuppressUnmanagedCodeSecurity]
		public static extern int Mix_FadeOutChannel(int which, int ms);
		[DllImport(MIX_DLL), SuppressUnmanagedCodeSecurity]
		public static extern int Mix_Playing(int channel);
		[DllImport(MIX_DLL), SuppressUnmanagedCodeSecurity]
		public static extern int Mix_Paused(int channel);
		[DllImport(MIX_DLL), SuppressUnmanagedCodeSecurity]
		public static extern int Mix_FadingChannel(int which);
		[DllImport(MIX_DLL), SuppressUnmanagedCodeSecurity]
		public static extern int Mix_SetPanning(int channel, byte left, byte right);
		[DllImport(MIX_DLL), SuppressUnmanagedCodeSecurity]
		public static extern int Mix_SetDistance(int channel, byte distance);
		[DllImport(MIX_DLL), SuppressUnmanagedCodeSecurity]
		public static extern int Mix_SetPosition(int channel, short angle, byte distance);
		[DllImport(MIX_DLL), SuppressUnmanagedCodeSecurity]
		public static extern int Mix_SetReverseStereo(int channel, int flip);
		[DllImport(MIX_DLL), SuppressUnmanagedCodeSecurity]
		public static extern IntPtr Mix_LoadMUS(string file);
		[DllImport(MIX_DLL), SuppressUnmanagedCodeSecurity]
		public static extern void Mix_FreeMusic(IntPtr mus);
		[DllImport(MIX_DLL), SuppressUnmanagedCodeSecurity]
		public static extern int Mix_PlayMusic(IntPtr mus, int loops);
		[DllImport(MIX_DLL), SuppressUnmanagedCodeSecurity]
		public static extern int Mix_FadeInMusic(IntPtr mus, int loops, int ms);
		[DllImport(MIX_DLL), SuppressUnmanagedCodeSecurity]
		public static extern int Mix_FadeInMusicPos(IntPtr mus, int loops, int ms, double pos);
		[DllImport(MIX_DLL), SuppressUnmanagedCodeSecurity]
		public static extern int Mix_VolumeMusic(int volume);
		[DllImport(MIX_DLL), SuppressUnmanagedCodeSecurity]
		public static extern void Mix_PauseMusic();
		[DllImport(MIX_DLL), SuppressUnmanagedCodeSecurity]
		public static extern void Mix_ResumeMusic();
		[DllImport(MIX_DLL), SuppressUnmanagedCodeSecurity]
		public static extern void Mix_RewindMusic();
		[DllImport(MIX_DLL), SuppressUnmanagedCodeSecurity]
		public static extern int Mix_SetMusicPosition(double position);
		[DllImport(MIX_DLL), SuppressUnmanagedCodeSecurity]
		public static extern int Mix_HaltMusic();
		[DllImport(MIX_DLL), SuppressUnmanagedCodeSecurity]
		public static extern int Mix_FadeOutMusic(int ms);
		[DllImport(MIX_DLL), SuppressUnmanagedCodeSecurity]
		public static extern int Mix_PlayingMusic();
		[DllImport(MIX_DLL), SuppressUnmanagedCodeSecurity]
		public static extern int Mix_PausedMusic();
		[DllImport(MIX_DLL), SuppressUnmanagedCodeSecurity]
		public static extern int Mix_FadingMusic();
		[DllImport(MIX_DLL), SuppressUnmanagedCodeSecurity]
		public static extern void Mix_ChannelFinished(ChannelFinishedDelegate cb);
		[DllImport(MIX_DLL), SuppressUnmanagedCodeSecurity]
		public static extern void Mix_HookMusicFinished(MusicFinishedDelegate cb);
	}
}
