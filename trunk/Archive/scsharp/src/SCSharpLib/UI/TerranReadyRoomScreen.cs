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
using System.Threading;
using System.Drawing;

using SdlDotNet.Graphics;
using SdlDotNet.Input;
using SdlDotNet.Core;
using SCSharp;
using SCSharp.MpqLib;


namespace SCSharp.UI
{
    /// <summary>
    /// 
    /// </summary>
    public class TerranReadyRoomScreen : ReadyRoomScreen
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mpq"></param>
        /// <param name="scenario_prefix"></param>
        public TerranReadyRoomScreen(Mpq mpq,
                          string scenario_prefix)
            : base(mpq,
                scenario_prefix,
                START_ELEMENT_INDEX,
                CANCEL_ELEMENT_INDEX,
                SKIPTUTORIAL_ELEMENT_INDEX,
                REPLAY_ELEMENT_INDEX,
                TRANSMISSION_ELEMENT_INDEX,
                OBJECTIVES_ELEMENT_INDEX,
                FIRST_PORTRAIT_ELEMENT_INDEX)
        {
        }

        const int START_ELEMENT_INDEX = 1;
        const int CANCEL_ELEMENT_INDEX = 9;
        const int SKIPTUTORIAL_ELEMENT_INDEX = 11;
        const int REPLAY_ELEMENT_INDEX = 12;
        const int FIRST_PORTRAIT_ELEMENT_INDEX = 13;
        const int TRANSMISSION_ELEMENT_INDEX = 17;
        const int OBJECTIVES_ELEMENT_INDEX = 18;
    }
}
