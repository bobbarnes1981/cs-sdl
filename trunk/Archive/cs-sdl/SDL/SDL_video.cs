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
using System;
using System.Runtime.InteropServices;

namespace CsGL.SDL
{
	[StructLayout(LayoutKind.Sequential)]
	public struct SDL_Rect {
		public short x;
		public short y;
		public ushort w;
		public ushort h;
	}
	
	[StructLayout(LayoutKind.Sequential)]
	public struct SDL_Color {
		public byte r;
		public byte g;
		public byte b;
		public byte unused;
	}

	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct SDL_Palette {
		public int ncolors;
		public SDL_Color * colors;
	}

	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct SDL_PixelFormat {
		public SDL_Palette * palette;
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
		public uint colorkey; // RGB colorkey info
		public byte alpha; // per-surface alpha
	}
		
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct SDL_Surface {
		public uint flags;				// Read-only
		public SDL_PixelFormat * format; 	// Read-only
		public uint w, h;				// Read-only
		public ushort pitch;				// Read-only
		public void * pixels;
		private int offset;				// Private

		/* Hardware-specific surface info */
		private void * hwdata;

		/* clipping information */
		public SDL_Rect clip_rect;			// Read-only
		private uint unused1;				// for binary compatibility

		/* Allow recursive locks */
		private uint locked;				// Private

		/* info for fast blit mapping to other surfaces */
		private void * blit_map;			// Private

		/* format version, bumped at every change to invalidate blit maps */
		private uint format_version;			// Private

		/* Reference count -- used when freeing surface */
		public int refcount;				// Read-mostly
	}
	
	// #define SDL_MUSTLOCK(surface)	\
	
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct SDL_VideoInfo {
		public uint hw_available {			// Flag: Can you create hardware surfaces?
			get { return info & 0x1; }
		}
		public uint wm_available {			// Flag: Can you talk to a window manager?
			get { return (info & 0x2) >> 1; }
		}
		//public uint UnusedBits1  :6;
		//public uint UnusedBits2  :1;
		public uint blit_hw {				// Flag: Accelerated blits HW --> HW
			get { return (info & (0x1<<9)) >> 9; }
		}
		public uint blit_hw_CC {			// Flag: Accelerated blits with Colorkey
			get { return (info & (0x1<<10)) >> 10; }
		}
		public uint blit_hw_A {				// Flag: Accelerated blits with Alpha
			get { return (info & (0x1<<11)) >> 11; }
		}
		public uint blit_sw {				// Flag: Accelerated blits SW --> HW
			get { return (info & (0x1<<12)) >> 12; }
		}
		public uint blit_sw_CC {			// Flag: Accelerated blits with Colorkey
			get { return (info & (0x1<<13)) >> 13; }
		}
		public uint blit_sw_A {				// Flag: Accelerated blits with Alpha
			get { return (info & (0x1<<14)) >> 14; }
		}
		public uint blit_fill {				// Flag: Accelerated color fill
			get { return (info & (0x1<<15)) >> 15; }
		}
		internal uint info;
		public uint video_mem;			// The total amount of video memory (in K)
		public SDL_PixelFormat * vfmt;	// Value: The format of the video surface
	}
	
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct SDL_Overlay {
		public uint format;			/* Read-only */
		public int w, h;			/* Read-only */
		public int planes;			/* Read-only */
		public ushort *pitches;		/* Read-only */
		public byte **pixels;		/* Read-write */

		/* Hardware-specific surface info */
		private void *hwfuncs;
		private void *hwdata;

		/* Special flags */
		private uint flags;
		public uint hw_overlay {		// Flag: This overlay hardware accelerated?
			get { return flags & 0x1; }
			set { flags |= value & 0x1; }
		}
	}

	public enum SDL_GLattr {
		SDL_GL_RED_SIZE,
		SDL_GL_GREEN_SIZE,
		SDL_GL_BLUE_SIZE,
		SDL_GL_ALPHA_SIZE,
		SDL_GL_BUFFER_SIZE,
		SDL_GL_DOUBLEBUFFER,
		SDL_GL_DEPTH_SIZE,
		SDL_GL_STENCIL_SIZE,
		SDL_GL_ACCUM_RED_SIZE,
		SDL_GL_ACCUM_GREEN_SIZE,
		SDL_GL_ACCUM_BLUE_SIZE,
		SDL_GL_ACCUM_ALPHA_SIZE
	}
	
	/*
	 * Grabbing means that the mouse is confined to the application window,
	 * and nearly all keyboard input is passed directly to the application,
	 * and not interpreted by a window manager, if any.
	 */
	public enum SDL_GrabMode{
		SDL_GRAB_QUERY = -1,
		SDL_GRAB_OFF = 0,
		SDL_GRAB_ON = 1,
		SDL_GRAB_FULLSCREEN	/* Used internally */
	}

	public abstract unsafe class SDL_video : SDL
	{
		/* Transparency definitions: These define alpha as the opacity of a surface */
		public const byte SDL_ALPHA_OPAQUE = 255;
		public const byte SDL_ALPHA_TRANSPARENT = 0;
		
		/* These are the currently supported flags for the SDL_surface */
		/* Available for SDL_CreateRGBSurface() or SDL_SetVideoMode() */
		public const uint SDL_SWSURFACE =	0x00000000;	/* Surface is in system memory */
		public const uint SDL_HWSURFACE =	0x00000001;	/* Surface is in video memory */
		public const uint SDL_ASYNCBLIT	=	0x00000004;	/* Use asynchronous blits if possible */
		/* Available for SDL_SetVideoMode() */
		public const uint SDL_ANYFORMAT =	0x10000000;	/* Allow any video depth/pixel-format */
		public const uint SDL_HWPALETTE =	0x20000000;	/* Surface has exclusive palette */
		public const uint SDL_DOUBLEBUF =	0x40000000;	/* Set up double-buffered video mode */
		public const uint SDL_FULLSCREEN =	0x80000000;	/* Surface is a full screen display */
		public const uint SDL_OPENGL =		0x00000002;	/* Create an OpenGL rendering context */
		public const uint SDL_OPENGLBLIT =	0x0000000A;	/* Create an OpenGL rendering context and use it for blitting */
		public const uint SDL_RESIZABLE =	0x00000010;	/* This video mode may be resized */
		public const uint SDL_NOFRAME =	0x00000020;		/* No window caption or edge frame */
		/* Used internally (read-only) */
		public const uint SDL_HWACCEL =	0x00000100;		/* Blit uses hardware acceleration */
		public const uint SDL_SRCCOLORKEY =	0x00001000;	/* Blit uses a source color key */
		public const uint SDL_RLEACCELOK =	0x00002000;	/* Private flag */
		public const uint SDL_RLEACCEL =	0x00004000;	/* Surface is RLE encoded */
		public const uint SDL_SRCALPHA =	0x00010000;	/* Blit uses source alpha blending */
		public const uint SDL_PREALLOC =	0x01000000;	/* Surface uses preallocated memory */
		
		/* The most common video overlay formats.
		   For an explanation of these pixel formats, see:
			http://www.webartz.com/fourcc/indexyuv.htm

		   For information on the relationship between color spaces, see:
		   http://www.neuro.sfc.keio.ac.jp/~aly/polygon/info/color-space-faq.html
		 */
		public const uint SDL_YV12_OVERLAY = 0x32315659;	/* Planar mode: Y + V + U  (3 planes) */
		public const uint SDL_IYUV_OVERLAY = 0x56555949;	/* Planar mode: Y + U + V  (3 planes) */
		public const uint SDL_YUY2_OVERLAY = 0x32595559;	/* Packed mode: Y0+U0+Y1+V0 (1 plane) */
		public const uint SDL_UYVY_OVERLAY = 0x59565955;	/* Packed mode: U0+Y0+V0+Y1 (1 plane) */
		public const uint SDL_YVYU_OVERLAY = 0x55595659;	/* Packed mode: Y0+V0+Y1+U0 (1 plane) */
		
		/* flags for SDL_SetPalette() */
		public const uint SDL_LOGPAL = 0x01;
		public const uint SDL_PHYSPAL = 0x02;
		
		/* Function prototypes */

		/* These functions are used internally, and should not be used unless you
		 * have a specific need to specify the video driver you want to use.
		 * You should normally use SDL_Init() or SDL_InitSubSystem().
		 *
		 * SDL_VideoInit() initializes the video subsystem -- sets up a connection
		 * to the window manager, etc, and determines the current video mode and
		 * pixel format, but does not initialize a window or graphics mode.
		 * Note that event handling is activated by this routine.
		 *
		 * If you use both sound and video in your application, you need to call
		 * SDL_Init() before opening the sound device, otherwise under Win32 DirectX,
		 * you won't be able to set full-screen display modes.
		 */
		[DllImport("SDL.dll")]
		public static extern int SDL_VideoInit(string driver_name, uint flags);
		[DllImport("SDL.dll")]
		public static extern void SDL_VideoQuit();
		
		/* This function fills the given character buffer with the name of the
		 * video driver, and returns a pointer to it if the video driver has
		 * been initialized.  It returns NULL if no driver has been initialized.
		 */
		[DllImport("csgl-sdl-native.dll", EntryPoint="DMVideoDriver")]
		public static extern void SDL_VideoDriverName(ref string name);
			
		/*
		 * This function returns a pointer to the current display surface.
		 * If SDL is doing format conversion on the display surface, this
		 * function returns the publicly visible surface, not the real video
		 * surface.
		 */
		[DllImport("SDL.dll")]
		public static extern SDL_Surface * SDL_GetVideoSurface();
		
		/*
		 * This function returns a read-only pointer to information about the
		 * video hardware.  If this is called before SDL_SetVideoMode(), the 'vfmt'
		 * member of the returned structure will contain the pixel format of the
		 * "best" video mode.
		 */
		[DllImport("SDL.dll")]
		public static extern SDL_VideoInfo * SDL_GetVideoInfo();

		/* 
		 * Check to see if a particular video mode is supported.
		 * It returns 0 if the requested mode is not supported under any bit depth,
		 * or returns the bits-per-pixel of the closest available mode with the
		 * given width and height.  If this bits-per-pixel is different from the
		 * one used when setting the video mode, SDL_SetVideoMode() will succeed,
		 * but will emulate the requested bits-per-pixel with a shadow surface.
		 *
		 * The arguments to SDL_VideoModeOK() are the same ones you would pass to
		 * SDL_SetVideoMode()
		 */
		[DllImport("SDL.dll")]
		public static extern int SDL_VideoModeOK
				(int width, int height, int bpp, uint flags);		
		
		/*
		 * Return a pointer to an array of available screen dimensions for the
		 * given format and video flags, sorted largest to smallest.  Returns 
		 * NULL if there are no dimensions available for a particular format, 
		 * or (SDL_Rect **)-1 if any dimension is okay for the given format.
		 *
		 * If 'format' is NULL, the mode list will be for the format given 
		 * by SDL_GetVideoInfo()->vfmt
		 */
		[DllImport("SDL.dll")]
		public static extern  SDL_Rect ** SDL_ListModes(SDL_PixelFormat *format, uint flags);
		
		/*
		 * Set up a video mode with the specified width, height and bits-per-pixel.
		 *
		 * If 'bpp' is 0, it is treated as the current display bits per pixel.
		 *
		 * If SDL_ANYFORMAT is set in 'flags', the SDL library will try to set the
		 * requested bits-per-pixel, but will return whatever video pixel format is
		 * available.  The default is to emulate the requested pixel format if it
		 * is not natively available.
		 *
		 * If SDL_HWSURFACE is set in 'flags', the video surface will be placed in
		 * video memory, if possible, and you may have to call SDL_LockSurface()
		 * in order to access the raw framebuffer.  Otherwise, the video surface
		 * will be created in system memory.
		 *
		 * If SDL_ASYNCBLIT is set in 'flags', SDL will try to perform rectangle
		 * updates asynchronously, but you must always lock before accessing pixels.
		 * SDL will wait for updates to complete before returning from the lock.
		 *
		 * If SDL_HWPALETTE is set in 'flags', the SDL library will guarantee
		 * that the colors set by SDL_SetColors() will be the colors you get.
		 * Otherwise, in 8-bit mode, SDL_SetColors() may not be able to set all
		 * of the colors exactly the way they are requested, and you should look
		 * at the video surface structure to determine the actual palette.
		 * If SDL cannot guarantee that the colors you request can be set, 
		 * i.e. if the colormap is shared, then the video surface may be created
		 * under emulation in system memory, overriding the SDL_HWSURFACE flag.
		 *
		 * If SDL_FULLSCREEN is set in 'flags', the SDL library will try to set
		 * a fullscreen video mode.  The default is to create a windowed mode
		 * if the current graphics system has a window manager.
		 * If the SDL library is able to set a fullscreen video mode, this flag 
		 * will be set in the surface that is returned.
		 *
		 * If SDL_DOUBLEBUF is set in 'flags', the SDL library will try to set up
		 * two surfaces in video memory and swap between them when you call 
		 * SDL_Flip().  This is usually slower than the normal single-buffering
		 * scheme, but prevents "tearing" artifacts caused by modifying video 
		 * memory while the monitor is refreshing.  It should only be used by 
		 * applications that redraw the entire screen on every update.
		 *
		 * If SDL_RESIZABLE is set in 'flags', the SDL library will allow the
		 * window manager, if any, to resize the window at runtime.  When this
		 * occurs, SDL will send a SDL_VIDEORESIZE event to you application,
		 * and you must respond to the event by re-calling SDL_SetVideoMode()
		 * with the requested size (or another size that suits the application).
		 *
		 * If SDL_NOFRAME is set in 'flags', the SDL library will create a window
		 * without any title bar or frame decoration.  Fullscreen video modes have
		 * this flag set automatically.
		 *
		 * This function returns the video framebuffer surface, or NULL if it fails.
		 *
		 * If you rely on functionality provided by certain video flags, check the
		 * flags of the returned surface to make sure that functionality is available.
		 * SDL will fall back to reduced functionality if the exact flags you wanted
		 * are not available.
		 */
		[DllImport("SDL.dll")]
		public static extern  SDL_Surface * SDL_SetVideoMode
					(int width, int height, int bpp, uint flags);
		
		// here some of my additions
		// -----------------------------
		[DllImport("csgl-sdl-native.dll", EntryPoint="DMLoadBmp")]
		public static extern SDL_Surface * SDL_LoadBMP(string file);
		
		/*
		 * Makes sure the given list of rectangles is updated on the given screen.
		 * If 'x', 'y', 'w' and 'h' are all 0, SDL_UpdateRect will update the entire
		 * screen.
		 * These functions should not be called while 'screen' is locked.
		 */
		[DllImport("SDL.dll")]
		public static extern void SDL_UpdateRects
				(SDL_Surface *screen, int numrects, SDL_Rect *rects);
		//
		// Improved csgl version
		//
		[DllImport("SDL.dll")]
		public static extern void SDL_UpdateRects
				(SDL_Surface *screen, int numrects, Rect[] rects);
				
		[DllImport("SDL.dll")]
		public static extern void SDL_UpdateRect
				(SDL_Surface *screen, int x, int y, uint w, uint h);

		/*
		 * On hardware that supports double-buffering, this function sets up a flip
		 * and returns.  The hardware will wait for vertical retrace, and then swap
		 * video buffers before the next video surface blit or lock will return.
		 * On hardware that doesn not support double-buffering, this is equivalent
		 * to calling SDL_UpdateRect(screen, 0, 0, 0, 0);
		 * The SDL_DOUBLEBUF flag must have been passed to SDL_SetVideoMode() when
		 * setting the video mode for this function to perform hardware flipping.
		 * This function returns 0 if successful, or -1 if there was an error.
		 */
		[DllImport("SDL.dll")]
		public static extern int SDL_Flip(SDL_Surface *screen);
		
		/*
		 * Set the gamma correction for each of the color channels.
		 * The gamma values range (approximately) between 0.1 and 10.0
		 * 
		 * If this function isn't supported directly by the hardware, it will
		 * be emulated using gamma ramps, if available.  If successful, this
		 * function returns 0, otherwise it returns -1.
		 */
		[DllImport("SDL.dll")]
		public static extern int SDL_SetGamma(float red, float green, float blue);
		
		/*
		 * Set the gamma translation table for the red, green, and blue channels
		 * of the video hardware.  Each table is an array of 256 16-bit quantities,
		 * representing a mapping between the input and output for that channel.
		 * The input is the index into the array, and the output is the 16-bit
		 * gamma value at that index, scaled to the output color precision.
		 * 
		 * You may pass NULL for any of the channels to leave it unchanged.
		 * If the call succeeds, it will return 0.  If the display driver or
		 * hardware does not support gamma translation, or otherwise fails,
		 * this function will return -1.
		 */
		[DllImport("SDL.dll")]
		public static extern int SDL_SetGammaRamp(ushort *red, ushort *green, ushort *blue);
		
		/*
		 * Retrieve the current values of the gamma translation tables.
		 * 
		 * You must pass in valid pointers to arrays of 256 16-bit quantities.
		 * Any of the pointers may be NULL to ignore that channel.
		 * If the call succeeds, it will return 0.  If the display driver or
		 * hardware does not support gamma translation, or otherwise fails,
		 * this function will return -1.
		 */
		[DllImport("SDL.dll")]
		public static extern int SDL_GetGammaRamp(ushort *red, ushort *green, ushort *blue);
		
		/*
		 * Sets a portion of the colormap for the given 8-bit surface.  If 'surface'
		 * is not a palettized surface, this function does nothing, returning 0.
		 * If all of the colors were set as passed to SDL_SetColors(), it will
		 * return 1.  If not all the color entries were set exactly as given,
		 * it will return 0, and you should look at the surface palette to
		 * determine the actual color palette.
		 *
		 * When 'surface' is the surface associated with the current display, the
		 * display colormap will be updated with the requested colors.  If 
		 * SDL_HWPALETTE was set in SDL_SetVideoMode() flags, SDL_SetColors()
		 * will always return 1, and the palette is guaranteed to be set the way
		 * you desire, even if the window colormap has to be warped or run under
		 * emulation.
		 */
		[DllImport("SDL.dll")]
		public static extern int SDL_SetColors(SDL_Surface *surface, 
					SDL_Color *colors, int firstcolor, int ncolors);
		
		/*
		 * Sets a portion of the colormap for a given 8-bit surface.
		 * 'flags' is one or both of:
		 * SDL_LOGPAL  -- set logical palette, which controls how blits are mapped
		 *                to/from the surface,
		 * SDL_PHYSPAL -- set physical palette, which controls how pixels look on
		 *                the screen
		 * Only screens have physical palettes. Separate change of physical/logical
		 * palettes is only possible if the screen has SDL_HWPALETTE set.
		 *
		 * The return value is 1 if all colours could be set as requested, and 0
		 * otherwise.
		 *
		 * SDL_SetColors() is equivalent to calling this function with
		 *     flags = (SDL_LOGPAL|SDL_PHYSPAL).
		 */
		[DllImport("SDL.dll")]
		public static extern int SDL_SetPalette(SDL_Surface *surface, int flags,
						SDL_Color *colors, int firstcolor,
						int ncolors);
		
		/*
		 * Maps an RGB triple to an opaque pixel value for a given pixel format
		 */
		[DllImport("SDL.dll")]
		public static extern uint SDL_MapRGB
					(SDL_PixelFormat *format, byte r, byte g, byte b);
		
		/*
		 * Maps an RGBA quadruple to a pixel value for a given pixel format
		 */
		[DllImport("SDL.dll")]
		public static extern uint SDL_MapRGBA(SDL_PixelFormat *format,
						byte r, byte g, byte b, byte a);
		
		/*
		 * Maps a pixel value into the RGB components for a given pixel format
		 */
		[DllImport("SDL.dll")]
		public static extern void SDL_GetRGB(uint pixel, SDL_PixelFormat *fmt,
						byte *r, byte *g, byte *b);
		
		/*
		 * Maps a pixel value into the RGBA components for a given pixel format
		 */
		[DllImport("SDL.dll")]
		public static extern void SDL_GetRGBA(uint pixel, SDL_PixelFormat *fmt,
						byte *r, byte *g, byte *b, byte *a);
		
		/*
		 * Allocate and free an RGB surface (must be called after SDL_SetVideoMode)
		 * If the depth is 4 or 8 bits, an empty palette is allocated for the surface.
		 * If the depth is greater than 8 bits, the pixel format is set using the
		 * flags '[RGB]mask'.
		 * If the function runs out of memory, it will return NULL.
		 *
		 * The 'flags' tell what kind of surface to create.
		 * SDL_SWSURFACE means that the surface should be created in system memory.
		 * SDL_HWSURFACE means that the surface should be created in video memory,
		 * with the same format as the display surface.  This is useful for surfaces
		 * that will not change much, to take advantage of hardware acceleration
		 * when being blitted to the display surface.
		 * SDL_ASYNCBLIT means that SDL will try to perform asynchronous blits with
		 * this surface, but you must always lock it before accessing the pixels.
		 * SDL will wait for current blits to finish before returning from the lock.
		 * SDL_SRCCOLORKEY indicates that the surface will be used for colorkey blits.
		 * If the hardware supports acceleration of colorkey blits between
		 * two surfaces in video memory, SDL will try to place the surface in
		 * video memory. If this isn't possible or if there is no hardware
		 * acceleration available, the surface will be placed in system memory.
		 * SDL_SRCALPHA means that the surface will be used for alpha blits and 
		 * if the hardware supports hardware acceleration of alpha blits between
		 * two surfaces in video memory, to place the surface in video memory
		 * if possible, otherwise it will be placed in system memory.
		 * If the surface is created in video memory, blits will be _much_ faster,
		 * but the surface format must be identical to the video surface format,
		 * and the only way to access the pixels member of the surface is to use
		 * the SDL_LockSurface() and SDL_UnlockSurface() calls.
		 * If the requested surface actually resides in video memory, SDL_HWSURFACE
		 * will be set in the flags member of the returned surface.  If for some
		 * reason the surface could not be placed in video memory, it will not have
		 * the SDL_HWSURFACE flag set, and will be created in system memory instead.
		 */
		// #define SDL_AllocSurface    SDL_CreateRGBSurface
		[DllImport("SDL.dll")]
		public static extern SDL_Surface *SDL_CreateRGBSurface
					(uint flags, int width, int height, int depth, 
					uint Rmask, uint Gmask, uint Bmask, uint Amask);
		[DllImport("SDL.dll")]
		public static extern SDL_Surface *SDL_CreateRGBSurfaceFrom(void *pixels,
					int width, int height, int depth, int pitch,
					uint Rmask, uint Gmask, uint Bmask, uint Amask);
		[DllImport("SDL.dll")]
		public static extern void SDL_FreeSurface(SDL_Surface *surface);
		
		/*
		 * SDL_LockSurface() sets up a surface for directly accessing the pixels.
		 * Between calls to SDL_LockSurface()/SDL_UnlockSurface(), you can write
		 * to and read from 'surface->pixels', using the pixel format stored in 
		 * 'surface->format'.  Once you are done accessing the surface, you should 
		 * use SDL_UnlockSurface() to release it.
		 *
		 * Not all surfaces require locking.  If SDL_MUSTLOCK(surface) evaluates
		 * to 0, then you can read and write to the surface at any time, and the
		 * pixel format of the surface will not change.  In particular, if the
		 * SDL_HWSURFACE flag is not given when calling SDL_SetVideoMode(), you
		 * will not need to lock the display surface before accessing it.
		 * 
		 * No operating system or library calls should be made between lock/unlock
		 * pairs, as critical system locks may be held during this time.
		 *
		 * SDL_LockSurface() returns 0, or -1 if the surface couldn't be locked.
		 */
		[DllImport("SDL.dll")]
		public static extern int SDL_LockSurface(SDL_Surface *surface);
		[DllImport("SDL.dll")]
		public static extern void SDL_UnlockSurface(SDL_Surface *surface);
		
		/*
		 * Load a surface from a seekable SDL data source (memory or file.)
		 * If 'freesrc' is non-zero, the source will be closed after being read.
		 * Returns the new surface, or NULL if there was an error.
		 * The new surface should be freed with SDL_FreeSurface().
		 */
		// extern DECLSPEC SDL_Surface * SDL_LoadBMP_RW(SDL_RWops *src, int freesrc);

		/* Convenience macro -- load a surface from a file */
		// #define SDL_LoadBMP(file)	SDL_LoadBMP_RW(SDL_RWFromFile(file, "rb"), 1)

		/*
		 * Save a surface to a seekable SDL data source (memory or file.)
		 * If 'freedst' is non-zero, the source will be closed after being written.
		 * Returns 0 if successful or -1 if there was an error.
		 */
		//extern DECLSPEC int SDL_SaveBMP_RW
		//		(SDL_Surface *surface, SDL_RWops *dst, int freedst);

		/* Convenience macro -- save a surface to a file */
		//#define SDL_SaveBMP(surface, file) \
		//		SDL_SaveBMP_RW(surface, SDL_RWFromFile(file, "wb"), 1)
		
		/*
		 * Sets the color key (transparent pixel) in a blittable surface.
		 * If 'flag' is SDL_SRCCOLORKEY (optionally OR'd with SDL_RLEACCEL), 
		 * 'key' will be the transparent pixel in the source image of a blit.
		 * SDL_RLEACCEL requests RLE acceleration for the surface if present,
		 * and removes RLE acceleration if absent.
		 * If 'flag' is 0, this function clears any current color key.
		 * This function returns 0, or -1 if there was an error.
		 */
		[DllImport("SDL.dll")]
		public static extern int SDL_SetColorKey
					(SDL_Surface *surface, uint flag, uint key);
		
		/*
		 * This function sets the alpha value for the entire surface, as opposed to
		 * using the alpha component of each pixel. This value measures the range
		 * of transparency of the surface, 0 being completely transparent to 255
		 * being completely opaque. An 'alpha' value of 255 causes blits to be
		 * opaque, the source pixels copied to the destination (the default). Note
		 * that per-surface alpha can be combined with colorkey transparency.
		 *
		 * If 'flag' is 0, alpha blending is disabled for the surface.
		 * If 'flag' is SDL_SRCALPHA, alpha blending is enabled for the surface.
		 * OR:ing the flag with SDL_RLEACCEL requests RLE acceleration for the
		 * surface; if SDL_RLEACCEL is not specified, the RLE accel will be removed.
		 */
		[DllImport("SDL.dll")]
		public static extern int SDL_SetAlpha(SDL_Surface *surface, uint flag, byte alpha);
		
		/*
		 * Sets the clipping rectangle for the destination surface in a blit.
		 *
		 * If the clip rectangle is NULL, clipping will be disabled.
		 * If the clip rectangle doesn't intersect the surface, the function will
		 * return SDL_FALSE and blits will be completely clipped.  Otherwise the
		 * function returns SDL_TRUE and blits to the surface will be clipped to
		 * the intersection of the surface area and the clipping rectangle.
		 *
		 * Note that blits are automatically clipped to the edges of the source
		 * and destination surfaces.
		 */
		[DllImport("SDL.dll")]
		public static extern SDL_bool SDL_SetClipRect(SDL_Surface *surface, SDL_Rect *rect);
		
		/*
		 * Gets the clipping rectangle for the destination surface in a blit.
		 * 'rect' must be a pointer to a valid rectangle which will be filled
		 * with the correct values.
		 */
		[DllImport("SDL.dll")]
		public static extern void SDL_GetClipRect(SDL_Surface *surface, SDL_Rect *rect);

		/*
		 * Creates a new surface of the specified format, and then copies and maps 
		 * the given surface to it so the blit of the converted surface will be as 
		 * fast as possible.  If this function fails, it returns NULL.
		 *
		 * The 'flags' parameter is passed to SDL_CreateRGBSurface() and has those 
		 * semantics.  You can also pass SDL_RLEACCEL in the flags parameter and
		 * SDL will try to RLE accelerate colorkey and alpha blits in the resulting
		 * surface.
		 *
		 * This function is used internally by SDL_DisplayFormat().
		 */
		[DllImport("SDL.dll")]
		public static extern SDL_Surface *SDL_ConvertSurface
					(SDL_Surface *src, SDL_PixelFormat *fmt, uint flags);
		
		/*
		 * This performs a fast blit from the source surface to the destination
		 * surface.  It assumes that the source and destination rectangles are
		 * the same size.  If either 'srcrect' or 'dstrect' are NULL, the entire
		 * surface (src or dst) is copied.  The final blit rectangles are saved
		 * in 'srcrect' and 'dstrect' after all clipping is performed.
		 * If the blit is successful, it returns 0, otherwise it returns -1.
		 *
		 * The blit function should not be called on a locked surface.
		 *
		 * The blit semantics for surfaces with and without alpha and colorkey
		 * are defined as follows:
		 *
		 * RGBA->RGB:
		 *     SDL_SRCALPHA set:
		 * 	alpha-blend (using alpha-channel).
		 * 	SDL_SRCCOLORKEY ignored.
		 *     SDL_SRCALPHA not set:
		 * 	copy RGB.
		 * 	if SDL_SRCCOLORKEY set, only copy the pixels matching the
		 * 	RGB values of the source colour key, ignoring alpha in the
		 * 	comparison.
		 * 
		 * RGB->RGBA:
		 *     SDL_SRCALPHA set:
		 * 	alpha-blend (using the source per-surface alpha value);
		 * 	set destination alpha to opaque.
		 *     SDL_SRCALPHA not set:
		 * 	copy RGB, set destination alpha to opaque.
		 *     both:
		 * 	if SDL_SRCCOLORKEY set, only copy the pixels matching the
		 * 	source colour key.
		 * 
		 * RGBA->RGBA:
		 *     SDL_SRCALPHA set:
		 * 	alpha-blend (using the source alpha channel) the RGB values;
		 * 	leave destination alpha untouched. [Note: is this correct?]
		 * 	SDL_SRCCOLORKEY ignored.
		 *     SDL_SRCALPHA not set:
		 * 	copy all of RGBA to the destination.
		 * 	if SDL_SRCCOLORKEY set, only copy the pixels matching the
		 * 	RGB values of the source colour key, ignoring alpha in the
		 * 	comparison.
		 * 
		 * RGB->RGB: 
		 *     SDL_SRCALPHA set:
		 * 	alpha-blend (using the source per-surface alpha value).
		 *     SDL_SRCALPHA not set:
		 * 	copy RGB.
		 *     both:
		 * 	if SDL_SRCCOLORKEY set, only copy the pixels matching the
		 * 	source colour key.
		 *
		 * If either of the surfaces were in video memory, and the blit returns -2,
		 * the video memory was lost, so it should be reloaded with artwork and 
		 * re-blitted:
			while ( SDL_BlitSurface(image, imgrect, screen, dstrect) == -2 ) {
				while ( SDL_LockSurface(image) < 0 )
					Sleep(10);
				-- Write image pixels to image->pixels --
				SDL_UnlockSurface(image);
			}
		 * This happens under DirectX 5.0 when the system switches away from your
		 * fullscreen application.  The lock will also fail until you have access
		 * to the video memory again.
		 */
		/* You should call SDL_BlitSurface() unless you know exactly how SDL
		   blitting works internally and how to use the other blit functions.
		*/
		[DllImport("SDL", EntryPoint="SDL_UpperBlit")]
		public static extern int SDL_BlitSurface
					(SDL_Surface *src, Rect srcrect,
					 SDL_Surface *dst, Rect dstrect);
		
		/* This is the public blit function, SDL_BlitSurface(), and it performs
		   rectangle validation and clipping before passing it to SDL_LowerBlit()
		*/
		[DllImport("SDL.dll")]
		public static extern int SDL_UpperBlit
					(SDL_Surface *src, SDL_Rect *srcrect,
					 SDL_Surface *dst, SDL_Rect *dstrect);
		
		/* This is a semi-private blit function and it performs low-level surface
		   blitting only.
		*/
		[DllImport("SDL.dll")]
		public static extern int SDL_LowerBlit
					(SDL_Surface *src, SDL_Rect *srcrect,
					 SDL_Surface *dst, SDL_Rect *dstrect);
	
		/*
		 * This function performs a fast fill of the given rectangle with 'color'
		 * The given rectangle is clipped to the destination surface clip area
		 * and the final fill rectangle is saved in the passed in pointer.
		 * If 'dstrect' is NULL, the whole surface will be filled with 'color'
		 * The color should be a pixel of the format used by the surface, and 
		 * can be generated by the SDL_MapRGB() function.
		 * This function returns 0 on success, or -1 on error.
		 */
		[DllImport("SDL.dll")]
		public static extern int SDL_FillRect(SDL_Surface *dst, SDL_Rect *dstrect, uint color);
		
		/* 
		 * This function takes a surface and copies it to a new surface of the
		 * pixel format and colors of the video framebuffer, suitable for fast
		 * blitting onto the display surface.  It calls SDL_ConvertSurface()
		 *
		 * If you want to take advantage of hardware colorkey or alpha blit
		 * acceleration, you should set the colorkey and alpha value before
		 * calling this function.
		 *
		 * If the conversion fails or runs out of memory, it returns NULL
		 */
		[DllImport("SDL.dll")]
		public static extern SDL_Surface * SDL_DisplayFormat(SDL_Surface *surface);
		
		/* 
		 * This function takes a surface and copies it to a new surface of the
		 * pixel format and colors of the video framebuffer (if possible),
		 * suitable for fast alpha blitting onto the display surface.
		 * The new surface will always have an alpha channel.
		 *
		 * If you want to take advantage of hardware colorkey or alpha blit
		 * acceleration, you should set the colorkey and alpha value before
		 * calling this function.
		 *
		 * If the conversion fails or runs out of memory, it returns NULL
		 */
		[DllImport("SDL.dll")]
		public static extern SDL_Surface * SDL_DisplayFormatAlpha(SDL_Surface *surface);
		
		
		/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
		/* YUV video surface overlay functions                                       */
		/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
		
		/* This function creates a video output overlay
		   Calling the returned surface an overlay is something of a misnomer because
		   the contents of the display surface underneath the area where the overlay
		   is shown is undefined - it may be overwritten with the converted YUV data.
		*/
		[DllImport("SDL.dll")]
		public static extern SDL_Overlay *SDL_CreateYUVOverlay(int width, int height,
						uint format, SDL_Surface *display);
		
		/* Lock an overlay for direct access, and unlock it when you are done */
		[DllImport("SDL.dll")]
		public static extern int SDL_LockYUVOverlay(SDL_Overlay *overlay);
		[DllImport("SDL.dll")]
		public static extern void SDL_UnlockYUVOverlay(SDL_Overlay *overlay);
		
		/* Blit a video overlay to the display surface.
		   The contents of the video surface underneath the blit destination are
		   not defined.  
		   The width and height of the destination rectangle may be different from
		   that of the overlay, but currently only 2x scaling is supported.
		*/
		[DllImport("SDL.dll")]
		public static extern int SDL_DisplayYUVOverlay(SDL_Overlay *overlay, SDL_Rect *dstrect);
		
		/* Free a video overlay */
		[DllImport("SDL.dll")]
		public static extern void SDL_FreeYUVOverlay(SDL_Overlay *overlay);
		
		/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
		/* OpenGL support functions.                                                 */
		/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
	
		/*
		 * Dynamically load a GL driver, if SDL is built with dynamic GL.
		 *
		 * SDL links normally with the OpenGL library on your system by default,
		 * but you can compile it to dynamically load the GL driver at runtime.
		 * If you do this, you need to retrieve all of the GL functions used in
		 * your program from the dynamic library using SDL_GL_GetProcAddress().
		 *
		 * This is disabled in default builds of SDL.
		 */
		[DllImport("SDL.dll")]
		public static extern int SDL_GL_LoadLibrary(string path);
		
		/*
		 * Get the address of a GL function (for extension functions)
		 */
		[DllImport("SDL.dll")]
		public static extern void *SDL_GL_GetProcAddress(string proc);
		
		/*
		 * Set an attribute of the OpenGL subsystem before intialization.
		 */
		[DllImport("SDL.dll")]
		public static extern int SDL_GL_SetAttribute(SDL_GLattr attr, int value);
		
		/*
		 * Get an attribute of the OpenGL subsystem from the windowing
		 * interface, such as glX. This is of course different from getting
		 * the values from SDL's internal OpenGL subsystem, which only
		 * stores the values you request before initialization.
		 *
		 * Developers should track the values they pass into SDL_GL_SetAttribute
		 * themselves if they want to retrieve these values.
		 */
		[DllImport("SDL.dll")]
		public static extern int SDL_GL_GetAttribute(SDL_GLattr attr, int* value);
		
		/*
		 * Swap the OpenGL buffers, if double-buffering is supported.
		 */
		[DllImport("SDL.dll")]
		public static extern void SDL_GL_SwapBuffers();
		
		/*
		 * Internal functions that should not be called unless you have read
		 * and understood the source code for these functions.
		 */
		[DllImport("SDL.dll")]
		public static extern void SDL_GL_UpdateRects(int numrects, SDL_Rect* rects);
		[DllImport("SDL.dll")]
		public static extern void SDL_GL_Lock();
		[DllImport("SDL.dll")]
		public static extern void SDL_GL_Unlock();
		
		/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
		/* These functions allow interaction with the window manager, if any.        */
		/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
		
		/*
		 * Sets/Gets the title and icon text of the display window
		 */
		[DllImport("SDL.dll")]
		public static extern  void SDL_WM_SetCaption(string title, string icon);
		[DllImport("SDL.dll")]
		public static extern  void SDL_WM_GetCaption(out string title, out string icon);
		
		/*
		 * Sets the icon for the display window.
		 * This function must be called before the first call to SDL_SetVideoMode().
		 * It takes an icon surface, and a mask in MSB format.
		 * If 'mask' is NULL, the entire icon surface will be used as the icon.
		 */
		[DllImport("SDL.dll")]
		public static extern  void SDL_WM_SetIcon(SDL_Surface *icon, byte *mask);
		
		/*
		 * This function iconifies the window, and returns 1 if it succeeded.
		 * If the function succeeds, it generates an SDL_APPACTIVE loss event.
		 * This function is a noop and returns 0 in non-windowed environments.
		 */
		[DllImport("SDL.dll")]
		public static extern  int SDL_WM_IconifyWindow();
		
		/*
		 * Toggle fullscreen mode without changing the contents of the screen.
		 * If the display surface does not require locking before accessing
		 * the pixel information, then the memory pointers will not change.
		 *
		 * If this function was able to toggle fullscreen mode (change from 
		 * running in a window to fullscreen, or vice-versa), it will return 1.
		 * If it is not implemented, or fails, it returns 0.
		 *
		 * The next call to SDL_SetVideoMode() will set the mode fullscreen
		 * attribute based on the flags parameter - if SDL_FULLSCREEN is not
		 * set, then the display will be windowed by default where supported.
		 *
		 * This is currently only implemented in the X11 video driver.
		 */
		[DllImport("SDL.dll")]
		public static extern  int SDL_WM_ToggleFullScreen(SDL_Surface *surface);

		/*
		 * This function allows you to set and query the input grab state of
		 * the application.  It returns the new input grab state.
		 */
		[DllImport("SDL.dll")]
		public static extern SDL_GrabMode SDL_WM_GrabInput(SDL_GrabMode mode);
		
		/* Not in public API at the moment - do not use! */
		[DllImport("SDL.dll")]
		public static extern int SDL_SoftStretch(SDL_Surface *src, SDL_Rect *srcrect,
                                    SDL_Surface *dst, SDL_Rect *dstrect);
	}
}
