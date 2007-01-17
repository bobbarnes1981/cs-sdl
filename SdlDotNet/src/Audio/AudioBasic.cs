#region LICENSE
/*
 * Copyright (C) 2006 Stuart Carnie (stuart.carnie@gmail.com)
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
using System.Globalization;
using System.Runtime.InteropServices;

using Tao.Sdl;
using SdlDotNet.Core;
using SdlDotNet.Graphics;

namespace SdlDotNet.Audio
{
    #region Public Delegates
    /// <summary>
    /// Used in the SDL_AudioSpec struct
    /// </summary>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void AudioCallback(IntPtr userData, IntPtr stream, int len);
    #endregion Public Delegates

    /// <summary>
    /// Represents an interface into the SDL Audio API, providing methods to open and close audio, and a callback facility to stream audio
    /// </summary>
    public static class AudioBasic
    {
        private enum Pause
        {
            UnPause,
            Pause
        }
        #region Private fields

        static bool audioOpen;
        static AudioInfo audioInfo;
        static bool audioLocked;
        static AudioStream stream;
        static bool isInitialized = Mixer.Initialize();

        #endregion Private fields

        #region Private methods

        static void CheckOpenStatus()
        {
            if (!audioOpen)
            {
                throw new AudioException(Events.StringManager.GetString("OpenAudioNotInit", CultureInfo.CurrentUICulture));
            }
        }

        #endregion Private methods

        #region Public methods

        /// <summary>
        /// 
        /// </summary>
        public static bool IsInitialized
        {
            get { return AudioBasic.isInitialized; }
        }

        /// <summary>
        /// Opens the audio device with the desired parameters.  
        /// Audio must be closed before calling this function.  You can check the <see cref="IsOpen"/> property for the current status.
        /// </summary>
        /// <param name="frequency">Audio frequency in samples per second</param>
        /// <param name="format">Audio data format. See <see cref="AudioFormat"/></param>
        /// <param name="channels">Number of channels.  See <see cref="SoundChannel"/></param>
        /// <param name="samples">Number of samples desired in the callback buffer.  This must be verified by checking the <see cref="Audio.AudioInfo"/> property.</param>
        /// <param name="callback">The delegate to handle populating the audio stream.</param>
        /// <param name="data">Any additional data to be passed to the callback function.</param>
        /// <exception cref="AudioException">Thrown under these circumstances:
        /// <list type="bullet">
        ///     <item>
        ///         <term>test</term>
        ///         <description>Attempting to initialize the audio subsystem</description>
        ///     </item>
        ///     <item>
        ///         <description>Attempting to open the audio device</description>
        ///     </item>
        /// </list>
        /// </exception>
        public static void OpenAudio(int frequency, AudioFormat format, SoundChannel channels, short samples, AudioCallback callback, object data)
        {
            Sdl.SDL_AudioSpec spec;
            // To keep compiler happy, we must 'initialize' these values
            spec.padding = 0;
            spec.size = 0;
            spec.silence = 0;
            // 

            spec.freq = frequency;
            spec.format = (short)format;
            spec.channels = (byte)channels;
            spec.callback = Marshal.GetFunctionPointerForDelegate(callback);
            spec.samples = samples;
            spec.userdata = data;

            IntPtr pSpec = Marshal.AllocHGlobal(Marshal.SizeOf(spec));
            try
            {
                Marshal.StructureToPtr(spec, pSpec, false);

                if (Sdl.SDL_OpenAudio(pSpec, IntPtr.Zero) < 0)
                {
                    throw new AudioException(Sdl.SDL_GetError());
                }

                spec = (Sdl.SDL_AudioSpec)Marshal.PtrToStructure(pSpec, typeof(Sdl.SDL_AudioSpec));
                byte bits = (byte)spec.format;
                int offset;

                if (((ushort)spec.format & 0x8000) == 0x8000)    // signed
                {
                    offset = 0;
                }
                else
                {
                    offset = 2 << (bits - 2);
                }

                audioInfo = new AudioInfo(spec, bits, offset);

            }
            finally
            {
                Marshal.FreeHGlobal(pSpec);
            }

            audioOpen = true;
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <returns></returns>
        //public static bool Initialize()
        //{
        //    Video.Initialize();
        //    if (!audioOpen)
        //    {
        //        if (Sdl.SDL_WasInit(Sdl.SDL_INIT_AUDIO) != Sdl.SDL_INIT_AUDIO)
        //        {
        //            if (Sdl.SDL_InitSubSystem(Sdl.SDL_INIT_AUDIO) == -1)
        //            {
        //                throw new AudioException();
        //            }
        //            //audioWasNotAlreadyInitialized = true;
        //        }
        //    }
        //    return true;
        //}

        static AudioCallback callback;

        /// <summary>
        /// Opens the audio device with the desired parameters.
        /// Audio must be closed before calling this function.  You can check the <see cref="IsOpen"/> property for the current status.
        /// Currently only <see cref="AudioFormat.Unsigned16Little"/> is supported and an exception will be thrown if any other value is specified
        /// </summary>
        /// <param name="frequency">Audio frequency in samples per second</param>
        /// <param name="format">Audio data format. See <see cref="AudioFormat"/></param>
        /// <param name="channels">Number of channels.  See <see cref="SoundChannel"/></param>
        /// <param name="samples">Number of samples desired in the callback buffer.  This must be verified by checking the <see cref="Audio.AudioInfo"/> property.</param>
        /// <returns>An <see cref="AudioStream"/> for queueing audio data to be played asynchronously.</returns>
        /// <exception cref="AudioException">Thrown under these circumstances:
        /// <list type="bullet">
        ///     <item>
        ///         <term>test</term>
        ///         <description>Attempting to initialize the audio subsystem</description>
        ///     </item>
        ///     <item>
        ///         <description>Attempting to open the audio device</description>
        ///     </item>
        /// </list>
        /// </exception>
        public static AudioStream OpenAudioStream(int frequency, AudioFormat format, SoundChannel channels, short samples)
        {
            if (audioOpen)
            {
                throw new AudioException(Events.StringManager.GetString(
                        "OpenAudioInit", CultureInfo.CurrentUICulture));
            }

            if (format != AudioFormat.Unsigned16Little)
            {
                throw new ArgumentException(Events.StringManager.GetString(
                        "AudioFormatSupported", CultureInfo.CurrentUICulture));
            }


            //if (Sdl.SDL_WasInit(Sdl.SDL_INIT_AUDIO) != Sdl.SDL_INIT_AUDIO)
            //{
            //    if (Sdl.SDL_InitSubSystem(Sdl.SDL_INIT_AUDIO) == -1)
            //    {
            //        throw new AudioException();
            //    }

            //    //audioWasNotAlreadyInitialized = true;
            //}

            Sdl.SDL_AudioSpec spec;
            // To keep compiler happy, we must 'initialize' these values
            spec.padding = 0;
            spec.size = 0;
            spec.silence = 0;

            // create new stream
            AudioStream audioStream = new AudioStream(frequency, samples);

            // get delegate
            callback = new AudioCallback(audioStream.Unsigned16LittleStream);

            spec.freq = frequency;
            spec.format = (short)format;
            spec.channels = (byte)channels;
            spec.callback = Marshal.GetFunctionPointerForDelegate(callback);
            spec.samples = audioStream.Samples;
            spec.userdata = null;

            IntPtr pSpec = Marshal.AllocHGlobal(Marshal.SizeOf(spec));
            try
            {
                Marshal.StructureToPtr(spec, pSpec, false);

                if (Sdl.SDL_OpenAudio(pSpec, IntPtr.Zero) < 0)
                {
                    throw new AudioException(Sdl.SDL_GetError());
                }

                spec = (Sdl.SDL_AudioSpec)Marshal.PtrToStructure(pSpec, typeof(Sdl.SDL_AudioSpec));
                byte bits = (byte)spec.format;
                int offset;

                if (((ushort)spec.format & 0x8000) == 0x8000)    // signed
                {
                    offset = 0;
                }
                else
                {
                    offset = 2 << (bits - 2);
                }

                audioInfo = new AudioInfo(spec, bits, offset);

            }
            finally
            {
                Marshal.FreeHGlobal(pSpec);
            }

            audioStream.Samples = audioInfo.Samples;

            stream = audioStream;
            audioOpen = true;

            return stream;
        }

        /// <summary>
        /// Call to close the audio subsystem
        /// </summary>
        public static void Close()
        {
            CheckOpenStatus();

            Sdl.SDL_CloseAudio();

            //audioCallback = null;
            audioOpen = false;
            audioInfo = null;
            audioLocked = false;

            if (stream != null)
            {
                stream.Close();
                stream = null;
            }
            Events.CloseMixer();
        }

        /// <summary>
        /// Gets or sets the locked status of the audio subsystem.  Necessary when data is 
        /// shared between the <see cref="Sdl.AudioSpecCallbackDelegate">callback</see> and the main thread.
        /// </summary>
        public static bool Locked
        {
            get
            {
                return audioLocked;
            }

            set
            {
                audioLocked = value;
                if (value)
                {
                    Sdl.SDL_LockAudio();
                }
                else
                {
                    Sdl.SDL_UnlockAudio();
                }
            }
        }

        /// <summary>
        /// Returns the current playback state of the audio subsystem.  See <see cref="AudioStatus"/>.
        /// </summary>
        public static AudioStatus AudioStatus
        {
            get
            {
                CheckOpenStatus();

                return (AudioStatus)Sdl.SDL_GetAudioStatus();
            }
        }

        /// <summary>
        /// Gets or sets the paused state of the audio subsystem.
        /// </summary>
        public static bool Paused
        {
            get
            {
                CheckOpenStatus();

                return AudioStatus != AudioStatus.Playing;
            }

            set
            {
                CheckOpenStatus();

                Sdl.SDL_PauseAudio(value ? (int)Pause.Pause : (int)Pause.UnPause);
            }
        }

        /// <summary>
        /// Gets the audio information of the currently open audio subsystem.  See <see cref="AudioInfo"/> for more information.
        /// </summary>
        public static AudioInfo AudioInfo
        {
            get
            {
                return audioInfo;
            }
        }

        /// <summary>
        /// Returns whether the audio subsystem is open or not.
        /// </summary>
        public static bool IsOpen
        {
            get
            {
                return audioOpen;
            }
        }

        #endregion Public methods
    }
}
