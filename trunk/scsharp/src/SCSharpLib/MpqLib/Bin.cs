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

namespace SCSharp.MpqLib
{
    /// <summary>
    /// 
    /// </summary>
    public class Bin : IMpqResource
    {
        Stream stream;
        List<BinElement> elements;

        /// <summary>
        /// 
        /// </summary>
        public Bin()
        {
            elements = new List<BinElement>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        public void ReadFromStream(Stream stream)
        {
            this.stream = stream;
            ReadElements();
        }

        void ReadElements()
        {
            int position;

            byte[] buf = new byte[stream.Length];

            stream.Read(buf, 0, (int)stream.Length);

            position = 0;
            do
            {
                BinElement element = new BinElement(buf, position, (uint)stream.Length);
                elements.Add(element);
                position += 86;
            } while (position < ((BinElement)elements[0]).TextOffset);
        }

        BinElement[] arr;
        /// <summary>
        /// 
        /// </summary>
        public BinElement[] Elements
        {
            get
            {
                if (arr == null)
                {
                    arr = elements.ToArray();
                }
                return arr;
            }
        }
    }
}
