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
using System.Globalization;

namespace SCSharp.MpqLib
{
    /// <summary>
    /// 
    /// </summary>
    public class TriggerCondition
    {
        uint number;
        /// <summary>
        /// 
        /// </summary>
        [CLSCompliant(false)]
        public uint Number
        {
            get { return number; }
            set { number = value; }
        }

        uint group;
        /// <summary>
        /// 
        /// </summary>
        [CLSCompliant(false)]
        public uint Group
        {
            get { return group; }
            set { group = value; }
        }

        uint amount;
        /// <summary>
        /// 
        /// </summary>
        [CLSCompliant(false)]
        public uint Amount
        {
            get { return amount; }
            set { amount = value; }
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

        byte comparison;
        /// <summary>
        /// 
        /// </summary>
        public byte ComparisonSwitch
        {
            get { return comparison; }
            set { comparison = value; }
        }

        byte condition;
        /// <summary>
        /// 
        /// </summary>
        public byte Condition
        {
            get { return condition; }
            set { condition = value; }
        }

        byte resource;
        /// <summary>
        /// 
        /// </summary>
        public byte Resource
        {
            get { return resource; }
            set { resource = value; }
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return String.Format(CultureInfo.CurrentCulture, "Trigger{{ Condition={0} }}", Condition);
        }
    }
}
