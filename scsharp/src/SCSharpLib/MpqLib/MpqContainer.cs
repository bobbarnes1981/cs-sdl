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
using System.Text;
using System.Collections.Generic;

namespace SCSharp.MpqLib
{
    /// <summary>
    /// 
    /// </summary>
    public class MpqContainer : Mpq
    {
        List<Mpq> mpqs;

        /// <summary>
        /// 
        /// </summary>
        public MpqContainer()
        {
            mpqs = new List<Mpq>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mpq"></param>
        public void Add(Mpq mpq)
        {
            if (mpq == null)
            {
                return;
            }
            mpqs.Add(mpq);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mpq"></param>
        public void Remove(Mpq mpq)
        {
            if (mpq == null)
            {
                return;
            }
            mpqs.Remove(mpq);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Clear()
        {
            mpqs.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public override Stream GetStreamForResource(string path)
        {
            foreach (Mpq mpq in mpqs)
            {
                Stream s = mpq.GetStreamForResource(path);
                if (s != null)
                {
                    return s;
                }
            }

            Console.WriteLine("returning null stream for resource: {0}", path);
            return null;
        }
    }
}
