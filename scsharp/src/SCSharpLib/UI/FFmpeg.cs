#region LICENSE
//
// Authors:
//	Chris Toshok (toshok@hungry.com)
//
// (C) 2006 The Hungry Programmers (http://www.hungry.com/)
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
#endregion LICENSE

using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using SdlDotNet.Graphics;
using SdlDotNet.Core;
using System.Drawing;

namespace SCSharp.UI
{
    /// <summary>
    /// 
    /// </summary>
    public class FFmpeg
    {
        static bool initSucceeded;

        static FFmpeg()
        {
            try
            {
                ffmpeg_init();
                initSucceeded = true;
            }
            catch (DllNotFoundException)
            {
                initSucceeded = false;
            }
        }

        GCHandle handle;
        string filename;
        byte[] buf;
        int width;
        int height;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="buf"></param>
        public FFmpeg(string filename, byte[] buf)
        {
            if (!initSucceeded)
            {
                throw new Exception("initialization of ffmpegglue library failed");
            }

            this.filename = filename;
            this.buf = buf;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Start()
        {
            handle = start_decoder(filename, buf, buf.Length);
            get_dimensions(handle, out width, out height);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Stop()
        {
            if (handle.Target != null)
            {
                stop_decoder(handle);
                handle.Target = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buf"></param>
        /// <returns></returns>
        public bool GetNextFrame(byte[] buf)
        {
            return get_next_frame(handle, buf);
        }

        /// <summary>
        /// 
        /// </summary>
        public int Width
        {
            get { return width; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int Height
        {
            get { return height; }
        }

        /// <summary>
        /// 
        /// </summary>
        [DllImport("ffmpegglue.dll")]
        extern static void ffmpeg_init();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="buf"></param>
        /// <param name="buf_size"></param>
        /// <returns></returns>
        [DllImport("ffmpegglue.dll")]
        public extern static GCHandle start_decoder(string filename, byte[] buf, int buf_size);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        [DllImport("ffmpegglue.dll")]
        public extern static void get_dimensions(GCHandle handle, out int width, out int height);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="buf"></param>
        /// <returns></returns>
        [DllImport("ffmpegglue.dll")]
        public extern static bool get_next_frame(GCHandle handle, byte[] buf);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="handle"></param>
        [DllImport("ffmpegglue.dll")]
        public extern static void stop_decoder(GCHandle handle);
    }
}
