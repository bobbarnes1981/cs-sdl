#region LICENSE
/*
 * Copyright (C) 2004 - 2007 David Hudson (jendave@yahoo.com)
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
#endregion LICENSE

using System;
using System.IO;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Globalization;
using System.Reflection;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;

using Tao.Sdl;
using SdlDotNet.Core;
using SdlDotNet.Graphics.Sprites;

namespace SdlDotNet.Graphics
{
    /// <summary>
    /// Represents an Sdl drawing surface.
    /// You can create instances of this class with the methods in the Video 
    /// object.
    /// </summary>
    public class Overlay : BaseSdlResource
    {
            /// <summary>
            /// 
            /// </summary>
            public short[] PitchesArray
            {
                get
                {
                    short[] pitchesArray = new short[this.OverlayStruct.planes];
                    for (int i = 0; i < this.OverlayStruct.planes; i++)
                    {
                        pitchesArray[i] = Marshal.ReadInt16(this.OverlayStruct.pitches, i * sizeof(Int16));
                    }
                    return pitchesArray;
                }
            }
            /// <summary>
            /// 
            /// </summary>
            public byte[][] PixelsArray
            {
                get
                {
                    byte[][] pixelsArray = new byte[this.OverlayStruct.planes][];
                    for (int i = 0; i < this.OverlayStruct.planes; i++)
                    {
                        pixelsArray[i] = new byte[PitchesArray[i]];
                        for (int j = 0; j < PitchesArray[i]; j++)
                        {
                            pixelsArray[i][j] = Marshal.ReadByte(this.OverlayStruct.pixels, i * sizeof(byte) + j * sizeof(byte));
                        }
                    }
                    return pixelsArray;
                }
            }

        internal Sdl.SDL_Overlay OverlayStruct
        {
            get
            {
                if (this.disposed)
                {
                    throw (new ObjectDisposedException(this.ToString()));
                }
                GC.KeepAlive(this);
                return (Sdl.SDL_Overlay)Marshal.PtrToStructure(this.Handle,
                    typeof(Sdl.SDL_Overlay));
            }
        }
        #region Private Fields

        private bool disposed;
        Surface surface;

        #endregion Private Fields

        #region Constructors and Destructors

        /// <summary>
        /// Creates a surface with the designated rectangle size.
        /// </summary>
        /// <param name="rectangle">Rectangle size of surface</param>
        /// <param name="surface"></param>
        public Overlay(Rectangle rectangle, Surface surface)
            : this(rectangle.Width, rectangle.Height, surface)
        {
        }

        /// <summary>
        /// Create surface of a given width and height
        /// </summary>
        /// <param name="width">Width of surface</param>
        /// <param name="height">Height of surface</param>
        /// <param name="surface"></param>
        public Overlay(int width, int height, Surface surface)
            : this(width, height, OverlayFormats.YV12, surface)
        {
        }

        /// <summary>
        /// Create surface of a given width and height
        /// </summary>
        /// <param name="width">Width of surface</param>
        /// <param name="height">Height of surface</param>
        /// <param name="display"></param>
        /// <param name="format"></param>
        public Overlay(int width, int height, OverlayFormats format, Surface display)
        {
            this.surface = display;
            this.Handle =
                Sdl.SDL_CreateYUVOverlay(width, height, (int)format, display.Handle);
            if (this.Handle == IntPtr.Zero)
            {
                throw SdlException.Generate();
            }
        }

        #endregion Constructors and Destructors

        #region Private Methods

        private static Sdl.SDL_Rect ConvertRecttoSDLRect(
            System.Drawing.Rectangle rect)
        {
            return new Sdl.SDL_Rect(
                (short)rect.X,
                (short)rect.Y,
                (short)rect.Width,
                (short)rect.Height);
        }

        #endregion Private Methods

        #region Internal Methods

        internal static Surface FromScreenPtr(IntPtr surfacePtr)
        {
            return new Surface(surfacePtr);
        }

        internal Sdl.SDL_Surface SurfaceStruct
        {
            get
            {
                if (this.disposed)
                {
                    throw (new ObjectDisposedException(this.ToString()));
                }
                GC.KeepAlive(this);
                return (Sdl.SDL_Surface)Marshal.PtrToStructure(this.Handle,
                    typeof(Sdl.SDL_Surface));
            }
        }

        internal Sdl.SDL_PixelFormat PixelFormat
        {
            get
            {
                if (this.disposed)
                {
                    throw (new ObjectDisposedException(this.ToString()));
                }
                GC.KeepAlive(this);
                return (Sdl.SDL_PixelFormat)Marshal.PtrToStructure(this.SurfaceStruct.format,
                    typeof(Sdl.SDL_PixelFormat));
            }
        }

        #endregion Internal Methods

        #region Protected Methods

        /// <summary>
        /// Destroys the surface object and frees its memory
        /// </summary>
        /// <param name="disposing">If true, dispose unmanaged objects</param>
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (!this.disposed)
                {
                    if (disposing)
                    {
                    }
                    this.disposed = true;
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        /// <summary>
        /// Closes Surface handle
        /// </summary>
        protected override void CloseHandle()
        {
            try
            {
                if (this.Handle != IntPtr.Zero)
                {
                    Sdl.SDL_FreeYUVOverlay(this.Handle);
                    this.Handle = IntPtr.Zero;
                }
            }
            catch (NullReferenceException e)
            {
                e.ToString();
            }
            catch (AccessViolationException e)
            {
                Console.WriteLine(e.StackTrace);
                e.ToString();
            }
            finally
            {
                this.Handle = IntPtr.Zero;
            }
        }

        #endregion Protected Methods

        #region Public Methods

        #endregion Public Methods
    }
}
