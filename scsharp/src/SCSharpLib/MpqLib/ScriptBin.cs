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
using System.Text;
using System.Collections.Generic;

using SCSharp.UI;

namespace SCSharp.MpqLib
{
    /// <summary>
    /// 
    /// </summary>
    public class ScriptBin : IMpqResource
    {
        byte[] buf;
        Dictionary<ushort, ushort> entries;

        const int entry_table_offset = 0x0082e0;

        /// <summary>
        /// 
        /// </summary>
        public ScriptBin()
        {
            entries = new Dictionary<ushort, ushort>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        public void ReadFromStream(Stream stream)
        {
            buf = new byte[stream.Length];
            stream.Read(buf, 0, buf.Length);

            int p = entry_table_offset;

            Console.WriteLine("iscript entry offsets {0:x}", p);
            Console.WriteLine("iscript.bin contains {0} entries",
                       (buf.Length - p) / 4);

            while (p < buf.Length - 4)
            {
                ushort images_id = Utilities.ReadWord(buf, p);
                ushort offset = Utilities.ReadWord(buf, p + 2);
                entries[images_id] = offset;
                p += 4;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public byte[] Contents
        {
            get { return buf; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="imagesId"></param>
        /// <returns></returns>
        [CLSCompliant(false)]
        public ushort GetScriptEntryOffset(ushort imagesId)
        {
            if (!entries.ContainsKey(imagesId))
            {
                return 0;
            }
            return entries[imagesId];
        }
    }

}
