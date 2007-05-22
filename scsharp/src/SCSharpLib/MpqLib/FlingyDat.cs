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

/*
  sprite id      at 0x0000. WORD.
  speed          at 0x0170. DWORD.
  turn style     at 0x0450. WORD
  acceleration   at 0x05c0. DWORD.
  turning radius at 0x08a0. BYTE.
  move controls  at ?.. ?.
*/

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
    public class FlingyDat : IMpqResource
    {
        /// <summary>
        /// 
        /// </summary>
        public FlingyDat()
        {
        }

        byte[] buf;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        public void ReadFromStream(Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }

            buf = new byte[(int)stream.Length];

            stream.Read(buf, 0, buf.Length);
        }

        /* offsets from the stardat.mpq version */

        const int spriteOffset = 0x0000;
        const int speedOffset = 0x0170;
        const int turnstyleOffset = 0x0450;
        const int accelOffset = 0x05c0;
        const int turnradiusOffset = 0x08a0;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public int GetSpriteId(int index)
        {
            return Utilities.ReadWord(buf, spriteOffset + index * 2);
        }
    }
}
