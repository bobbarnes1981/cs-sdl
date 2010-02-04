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
    public class EstablishingShot : MarkupScreen
    {
        string markupResource;
        string scenarioPrefix;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="markupResource"></param>
        /// <param name="scenarioPrefix"></param>
        /// <param name="mpq"></param>
        public EstablishingShot(string markupResource, string scenarioPrefix, Mpq mpq)
            : base(mpq)
        {
            this.markupResource = markupResource;
            this.scenarioPrefix = scenarioPrefix;
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void LoadMarkup()
        {
            AddMarkup((Stream)this.Mpq.GetResource(markupResource));
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void MarkupFinished()
        {
            Game.Instance.SwitchToScreen(ReadyRoomScreen.Create(this.Mpq, scenarioPrefix));
        }
    }
}