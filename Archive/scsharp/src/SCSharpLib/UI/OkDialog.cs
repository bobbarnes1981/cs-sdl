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

using SdlDotNet;
using SCSharp;
using SCSharp.MpqLib;
using System.Drawing;

namespace SCSharp.UI
{
    /// <summary>
    /// 
    /// </summary>
    public class OkDialog : UIDialog
    {
        string message;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="mpq"></param>
        /// <param name="message"></param>
        public OkDialog(UIScreen parent, Mpq mpq, string message)
            : base(parent, mpq, "glue\\PalNl", BuiltIns.GluPOkBin)
        {
            BackgroundPath = "glue\\PalNl\\pOPopup.pcx";
            this.message = message;
        }

        const int OK_ELEMENT_INDEX = 1;
        const int MESSAGE_ELEMENT_INDEX = 2;

        /// <summary>
        /// 
        /// </summary>
        protected override void ResourceLoader()
        {
            base.ResourceLoader();

            Elements[MESSAGE_ELEMENT_INDEX].Text = message;

            Elements[OK_ELEMENT_INDEX].Activate +=
                delegate(object sender, SCEventArgs args)
                {
                    if (Ok == null)
                    {
                        Parent.DismissDialog();
                    }
                    else
                    {
                        Ok(this, new SCEventArgs());
                    }
                };
        }

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<SCEventArgs> Ok;
    }
}
