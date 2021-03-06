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
using System.Collections.ObjectModel;

namespace SCSharp.MpqLib
{
    /// <summary>
    /// 
    /// </summary>
    public class TriggerData
    {
        /// <summary>
        /// 
        /// </summary>
        public TriggerData()
        {
            triggers = new Collection<Trigger>();
        }

        //public void Parse(byte[] data, bool briefing)

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        public void Parse(byte[] data)
        {
            int offset = 0;

            while (offset < data.Length)
            {
                Trigger t = new Trigger();
                t.Parse(data, ref offset);
                triggers.Add(t);
            }
        }

        Collection<Trigger> triggers;

        /// <summary>
        /// 
        /// </summary>
        public Collection<Trigger> Triggers
        {
            get { return triggers; }
        }
    }
}
