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

using SdlDotNet.Input;
using SCSharp;
using SCSharp.MpqLib;
using System.Drawing;

namespace SCSharp.UI
{
    /// <summary>
    /// 
    /// </summary>
    public class ConnectionScreen : UIScreen
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mpq"></param>
        public ConnectionScreen(Mpq mpq)
            : base(mpq, "glue\\PalNl", Builtins.GluConnBin)
        {
        }

        const int LISTBOX_ELEMENT_INDEX = 6;
        const int TITLE_ELEMENT_INDEX = 7;
        const int DESCRIPTION_ELEMENT_INDEX = 9;
        const int OK_ELEMENT_INDEX = 10;
        const int CANCEL_ELEMENT_INDEX = 11;

#if INCLUDE_ALL_NETWORK_OPTIONS
		const int num_choices = 4;
#else
        const int numChoices = 1;
#endif
        const int titleStartIdx = 95;
        const int descriptionStartIdx = 99;

        string[] titles;
        string[] descriptions;

        ListBoxElement listbox;

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

            titles = new string[numChoices];
            descriptions = new string[numChoices];

#if INCLUDE_ALL_NETWORK_OPTIONS			
			for (int i = 0; i < num_choices; i ++) {
				titles[i] = GlobalResources.Instance.GluAllTbl[ title_startidx + i ];
				descriptions[i] = GlobalResources.Instance.GluAllTbl[ description_startidx + i ];
			}
#else
            titles[0] = GlobalResources.Instance.GluAllTbl[titleStartIdx + 3];
            descriptions[0] = GlobalResources.Instance.GluAllTbl[descriptionStartIdx + 3];
#endif
            listbox = (ListBoxElement)Elements[LISTBOX_ELEMENT_INDEX];

            foreach (string s in titles)
            {
                listbox.AddItem(s);
            }

            listbox.SelectedIndex = 0;
            HandleSelectionChanged(0);

            listbox.SelectionChanged += HandleSelectionChanged;

            Elements[OK_ELEMENT_INDEX].Activate +=
                delegate()
                {
                    ShowDialog(new OkDialog(this, this.Mpq,
                                  "insert battle.net code here"));
                };

            Elements[CANCEL_ELEMENT_INDEX].Activate +=
                delegate()
                {
                    Game.Instance.SwitchToScreen(UIScreenType.MainMenu);
                };
        }

        void HandleSelectionChanged(int selectedIndex)
        {
            Elements[TITLE_ELEMENT_INDEX].Text = titles[selectedIndex];
            Elements[DESCRIPTION_ELEMENT_INDEX].Text = descriptions[selectedIndex];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        public override void KeyboardDown(KeyboardEventArgs args)
        {
            if (args == null)
            {
                throw new ArgumentNullException("args");
            }
            if (args.Key == Key.DownArrow
                || args.Key == Key.UpArrow)
            {
                listbox.KeyboardDown(args);
            }
            else
            {
                base.KeyboardDown(args);
            }
        }
    }
}
