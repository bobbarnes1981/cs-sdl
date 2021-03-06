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
using System.Threading;

using SdlDotNet.Input;
using SCSharp;
using SCSharp.MpqLib;
using System.Drawing;

namespace SCSharp.UI
{
    /// <summary>
    /// 
    /// </summary>
    public class EntryDialog : UIDialog
    {
        string title;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="mpq"></param>
        /// <param name="title"></param>
        public EntryDialog(UIScreen parent, Mpq mpq, string title)
            : base(parent, mpq, "glue\\PalNl", BuiltIns.GluPEditBin)
        {
            BackgroundPath = "glue\\PalNl\\pEPopup.pcx";
            this.title = title;
        }

        const int OK_ELEMENT_INDEX = 1;
        const int TITLE_ELEMENT_INDEX = 2;
        const int CANCEL_ELEMENT_INDEX = 3;
        const int ENTRY_ELEMENT_INDEX = 4;

        TextBoxElement entry;

        /// <summary>
        /// 
        /// </summary>
        protected override void ResourceLoader()
        {
            base.ResourceLoader();

            Console.WriteLine("entry element is {0}", Elements[ENTRY_ELEMENT_INDEX].Type);

            Elements[TITLE_ELEMENT_INDEX].Text = title;

            Elements[OK_ELEMENT_INDEX].Activate +=
                delegate(object sender, SCEventArgs args) 
                {
                    if (Ok != null)
                    {
                        Ok(this, new SCEventArgs());
                    }
                };

            Elements[CANCEL_ELEMENT_INDEX].Activate +=
                delegate(object sender, SCEventArgs args) 
                {
                    if (Cancel != null)
                    {
                        Cancel(this, new SCEventArgs());
                    }
                };

            entry = (TextBoxElement)Elements[ENTRY_ELEMENT_INDEX];

            Elements[OK_ELEMENT_INDEX].Sensitive = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        public override void KeyboardDown(KeyboardEventArgs args)
        {
            if ((args.Mod & (ModifierKeys.LeftAlt | ModifierKeys.RightAlt)) != 0
                || args.Key == Key.Return
                || args.Key == Key.Escape)
            {

                base.KeyboardDown(args);
                return;
            }

            entry.KeyboardDown(args);

            Elements[OK_ELEMENT_INDEX].Sensitive = (entry.Value.Length > 0);
        }

        /// <summary>
        /// 
        /// </summary>
        public string Value
        {
            get { return entry.Value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<SCEventArgs> Cancel;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<SCEventArgs> Ok;
    }
}
