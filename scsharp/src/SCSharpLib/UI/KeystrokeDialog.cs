#region LICENSE
//
// Authors:
// Chris Toshok (toshok@hungry.com)
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

using SdlDotNet;
using SCSharp;
using SCSharp.MpqLib;
using System.Drawing;

namespace SCSharp.UI
{
    /// <summary>
    ///
    /// </summary>
    public class KeystrokeDialog : UIDialog
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="mpq"></param>
        public KeystrokeDialog(UIScreen parent, Mpq mpq)
            : base(parent, mpq, "glue\\Palmm", BuiltIns.HelpBin)
        {
            BackgroundPath = null;
        }

        const int OK_ELEMENT_INDEX = 1;
        const int HELPLIST_ELEMENT_INDEX = 2;

        /// <summary>
        ///
        /// </summary>
        protected override void ResourceLoader()
        {
            base.ResourceLoader();

            for (int i = 0; i < Elements.Count; i++)
            {
                Console.WriteLine("{0}: {1} '{2}'", i, Elements[i].Type, Elements[i].Text);
            }

            Elements[OK_ELEMENT_INDEX].Activate +=
            delegate(object sender, SCEventArgs args) 
            {
                if (Ok != null)
                {
                    Ok(this, new SCEventArgs());
                }
            };

            ListBoxElement list = (ListBoxElement)Elements[HELPLIST_ELEMENT_INDEX];
            Tbl helpText = (Tbl)this.Mpq.GetResource(BuiltIns.HelpTxtTbl);

            for (int i = 0; i < helpText.Strings.Length; i++)
            {
                list.AddItem(helpText.Strings[i]);
            }
        }

        /// <summary>
        ///
        /// </summary>
        public event EventHandler<SCEventArgs> Ok;
    }
}
