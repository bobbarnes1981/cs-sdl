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
    public class TriggerAction
    {
        uint location;
        /// <summary>
        /// 
        /// </summary>
        [CLSCompliant(false)]
        public uint Location
        {
            get { return location; }
            set { location = value; }
        }

        uint textIndex;
        /// <summary>
        /// 
        /// </summary>
        [CLSCompliant(false)]
        public uint TextIndex
        {
            get { return textIndex; }
            set { textIndex = value; }
        }

        uint wavIndex;
        /// <summary>
        /// 
        /// </summary>
        [CLSCompliant(false)]
        public uint WavIndex
        {
            get { return wavIndex; }
            set { wavIndex = value; }
        }

        uint delay;
        /// <summary>
        /// 
        /// </summary>
        [CLSCompliant(false)]
        public uint Delay
        {
            get { return delay; }
            set { delay = value; }
        }

        uint group1;
        /// <summary>
        /// 
        /// </summary>
        [CLSCompliant(false)]
        public uint Group1
        {
            get { return group1; }
            set { group1 = value; }
        }

        uint group2;
        /// <summary>
        /// 
        /// </summary>
        [CLSCompliant(false)]
        public uint Group2
        {
            get { return group2; }
            set { group2 = value; }
        }

        ushort unitType;
        /// <summary>
        /// 
        /// </summary>
        [CLSCompliant(false)]
        public ushort UnitType
        {
            get { return unitType; }
            set { unitType = value; }
        }

        byte action;
        /// <summary>
        /// 
        /// </summary>
        public byte Action
        {
            get { return action; }
            set { action = value; }
        }

        byte _switch;
        /// <summary>
        /// 
        /// </summary>
        public byte Switch
        {
            get { return _switch; }
            set { _switch = value; }
        }

        byte flags;
        /// <summary>
        /// 
        /// </summary>
        public byte Flags
        {
            get { return flags; }
            set { flags = value; }
        }
    }
}
