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
    public class Tbl : IMpqResource
    {
        Stream stream;
        int numStrings;
        string[] strings;

        /// <summary>
        /// 
        /// </summary>
        public Tbl()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        public void ReadFromStream(Stream stream)
        {
            this.stream = stream;
            ReadStrings();
        }

        void ReadStrings()
        {
            int i;

            numStrings = Utilities.ReadWord(stream);

            int[] offsets = new int[numStrings];

            for (i = 0; i < numStrings; i++)
            {
                offsets[i] = Utilities.ReadWord(stream);
            }

            StreamReader tr = new StreamReader(stream);
            strings = new string[numStrings];

            for (i = 0; i < numStrings; i++)
            {
                if (tr.BaseStream.Position != offsets[i])
                {
                    tr.BaseStream.Seek(offsets[i], SeekOrigin.Begin);
                    tr.DiscardBufferedData();
                }

                strings[i] = Utilities.ReadUntilNull(tr);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public string this[int index]
        {
            get { return strings[index]; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string[] Strings
        {
            get { return strings; }
        }
    }
}
