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

using SdlDotNet;
using SCSharp;
using SCSharp.MpqLib;
using System.Drawing;

namespace SCSharp.UI
{
    /// <summary>
    /// 
    /// </summary>
    public class GameModeDialog : UIDialog
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="mpq"></param>
        public GameModeDialog(UIScreen parent, Mpq mpq)
            : base(parent, mpq, "glue\\Palmm", Builtins.GluGameModeBin)
        {
            BackgroundPath = "glue\\Palmm\\retail_ex.pcx";
            BackgroundTranslucent = 42;
            BackgroundTransparent = 0;
        }

        const int ORIGINAL_ELEMENT_INDEX = 1;
        const int TITLE_ELEMENT_INDEX = 2;
        const int EXPANSION_ELEMENT_INDEX = 3;
        const int CANCEL_ELEMENT_INDEX = 4;

        /// <summary>
        /// 
        /// </summary>
        protected override void ResourceLoader()
        {
            base.ResourceLoader();

            for (int i = 0; i < Elements.Count; i++)
            {
                Console.WriteLine("{0}: {1}", i, Elements[i].Text);
            }

            Elements[TITLE_ELEMENT_INDEX].Text = GlobalResources.BrooDat.GluAllTbl.Strings[172];

            Elements[ORIGINAL_ELEMENT_INDEX].Activate +=
                delegate(object sender, EventArgs args)
                {
                    if (Activate != null)
                    {
                        Activate(this, new GameModeActivateEventArgs(false));
                    }
                };

            Elements[EXPANSION_ELEMENT_INDEX].Activate +=
                delegate(object sender, EventArgs args)
                {
                    if (Activate != null)
                    {
                        Activate(this, new GameModeActivateEventArgs(true));
                    }
                };

            Elements[CANCEL_ELEMENT_INDEX].Activate +=
                delegate(object sender, EventArgs args)
                {
                    if (Cancel != null)
                    {
                        Cancel(this, new EventArgs());
                    }
                };
        }

        /// <summary>
        /// 
        /// </summary>
        public event DialogEventHandler Cancel;
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<GameModeActivateEventArgs> Activate;
    }

    ///// <summary>
    ///// 
    ///// </summary>
    ///// <param name="e"></param>
    ///// <param name="sender"></param>
    //public delegate void GameModeActivateEventHandler(object sender, GameModeActivateEventArgs e);
}
