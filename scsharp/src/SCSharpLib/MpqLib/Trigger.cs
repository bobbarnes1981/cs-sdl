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
    public class Trigger
    {
        TriggerCondition[] conditions = new TriggerCondition[16];
        TriggerAction[] actions = new TriggerAction[64];

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="offset"></param>
        public void Parse(byte[] data, ref int offset)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            int i;
            for (i = 0; i < conditions.Length; i++)
            {
                TriggerCondition c = new TriggerCondition();
                c.Number = Utilities.ReadDWord(data, offset); offset += 4;
                c.Group = Utilities.ReadDWord(data, offset); offset += 4;
                c.Amount = Utilities.ReadDWord(data, offset); offset += 4;
                c.UnitType = Utilities.ReadWord(data, offset); offset += 2;
                c.ComparisonSwitch = data[offset++];
                c.Condition = data[offset++];
                c.Resource = data[offset++];
                c.Flags = data[offset++];

                // padding
                offset += 2;

                conditions[i] = c;
            }

            for (i = 0; i < actions.Length; i++)
            {
                TriggerAction a = new TriggerAction();

                a.Location = Utilities.ReadDWord(data, offset); offset += 4;
                a.TextIndex = Utilities.ReadDWord(data, offset); offset += 4;
                a.WavIndex = Utilities.ReadWord(data, offset); offset += 4;
                a.Delay = Utilities.ReadDWord(data, offset); offset += 4;
                a.Group1 = Utilities.ReadDWord(data, offset); offset += 4;
                a.Group2 = Utilities.ReadDWord(data, offset); offset += 4;
                a.UnitType = Utilities.ReadWord(data, offset); offset += 2;
                a.Action = data[offset++];
                a.Switch = data[offset++];
                a.Flags = data[offset++];

                // padding
                offset += 3;

                actions[i] = a;
            }

            // more padding?
            offset += 4;


            // this is 1 byte for each player in the groups list:
            //
            // 00 - trigger is not executed for player
            // 01 - trigger is executed for player
            offset += 28;
        }

        /// <summary>
        /// 
        /// </summary>
        public TriggerCondition[] Conditions
        {
            get { return conditions; }
        }

        /// <summary>
        /// 
        /// </summary>
        public TriggerAction[] Actions
        {
            get { return actions; }
        }
    }
}
