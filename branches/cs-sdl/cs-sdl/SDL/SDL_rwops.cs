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
	// this class stand for any (SDL_RWops *) in C code. as it just
	// keep the pointer value

	/// <summary>
	/// SDL_RWops is the base IO object of SDL. (the SDL stream "class") 
	/// you don't really need to use it (prefer Stream) except for SDL_RWops
	/// using function..
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct SDL_RWops : IDisposable
	{
		// hold the underlying pointer to the struct..
		IntPtr handle;
		
#if NATIVE_SDL_RWops // no longer needed and buggy as not freed...
		[DllImport("SDL.dll")]
		static extern IntPtr SDL_RWFromFile(string file, string mode);
		
		public const string FILE_READ = "rb";
		public const string FILE_WRITE = "wb";
		public const string FILE_APPEND = "ab";
		public const string FILE_READ_WRITE = "rb+";
		public const string FILE_READ_APPEND = "ab+";
		
		/// <summary>create a READ SDL_RWops from a file
		/// caution: could be a bug or not: no function to free
		/// such SDL_RWops, don't use it (use SDL_RWops(Stream) constructor)
		/// or use autofreeing method with them.
		/// </summary>
		public SDL_RWops(string file)
		{
			handle = SDL_RWFromFile(file, FILE_READ);
			if(handle == IntPtr.Zero)
				throw new SDLException();
		}
		/// <summary>create SDL_RWops from a file
		/// caution: could be a bug or not: no function to free
		/// such SDL_RWops, don't use it (use SDL_RWops(Stream) constructor)
		/// or use autofreeing method with them.
		/// </summary>
		public SDL_RWops(string file, string mode)
		{
			handle = SDL_RWFromFile(file, mode);
			if(handle == IntPtr.Zero)
				throw new SDLException();
		}
#endif		
		// ------------------------------------------
		// C# SDL_RWops - Stream part....
		
		// a table holding all currently opened & used stream
		static Hashtable streams = new Hashtable();
		
		[DllImport("csgl-sdl-native.dll")]
		static extern void DMInitRWops(s_seek s, s_read r, s_write w, s_close c);
		[DllImport("csgl-sdl-native.dll")]
		static extern IntPtr DMRWopsFromStream();
		[DllImport("csgl-sdl-native.dll")]
		static extern void DMRWopsFree(IntPtr io);
		
		/// <summary>
		/// create a SDL_RWops from a Stream.
		/// beware! often sld's function often close used stream
		/// </summary>
		public SDL_RWops(Stream s)
		{
			handle = DMRWopsFromStream();
			if(handle == IntPtr.Zero)
				throw new SDLException();
			streams[handle] = s;
		}
		/// <summary>
		/// free any native resource but don't close the underlying stream,
		/// if any...
		/// </summary>
		public void Dispose()
		{
			Stream s = (Stream) streams[handle];
			if(s != null) {
				streams.Remove(handle);
				DMRWopsFree(handle);
			}
			handle = IntPtr.Zero;
		}
		
		const int SEEK_SET = 0;
		const int SEEK_CUR = 1;
		const int SEEK_END = 2;

		delegate int s_seek(IntPtr ctxt, int offset, int whence);
		static int seek(IntPtr ctxt, int offset, int whence)
		{
			Stream s = (Stream) streams[ctxt];
			if(s == null) {
				SDL_error.SDL_SetError("attempt to seek a closed stream");
				return -1;
			}
			switch(whence) {
				case SEEK_SET:
					s.Seek(offset, SeekOrigin.Begin);
					break;
				case SEEK_CUR:
					s.Seek(offset, SeekOrigin.Current);
					break;
				case SEEK_END:
					s.Seek(offset, SeekOrigin.End);
					break;
			}
			return (int) s.Position;
		}
		delegate int s_read(IntPtr ctxt, int size,
		                    [Out][MarshalAs(UnmanagedType.LPArray, SizeParamIndex=1)] byte[] ptr);
		static int read(IntPtr ctxt, int size, byte[] buf)
		{
			Stream s = (Stream) streams[ctxt];
			if(s == null) {
				SDL_error.SDL_SetError("attempt to read from a closed stream");
				return -1;
			}
			return s.Read(buf, 0, buf.Length);
		}
		delegate int s_write(IntPtr ctxt, int size,
		                     [Out][MarshalAs(UnmanagedType.LPArray, SizeParamIndex=1)] byte[] ptr);
		static int write(IntPtr ctxt, int size, byte[] buf)
		{
			Stream s = (Stream) streams[ctxt];
			if(s == null) {
				SDL_error.SDL_SetError("attempt to write to a closed stream");
				return -1;
			}
			try { s.Write(buf, 0, buf.Length); }
			catch (IOException ioe) {
				SDL_error.SDL_SetError(ioe.Message);
				return -1;
			}
			return buf.Length;
		}
		delegate int s_close(IntPtr ctxt);
		static int close(IntPtr ctxt)
		{
			Stream s = (Stream) streams[ctxt];
			if(s != null) {
				streams.Remove(ctxt);
				DMRWopsFree(ctxt);
				s.Close();
			}
			return 1;
		}
		
		static s_seek  seeker = new s_seek(seek);
		static s_read  reader = new s_read(read);
		static s_write writer = new s_write(write);
		static s_close closer = new s_close(close);
		static SDL_RWops() 
		{
			DMInitRWops(seeker, reader, writer, closer);
		}
	}
}
