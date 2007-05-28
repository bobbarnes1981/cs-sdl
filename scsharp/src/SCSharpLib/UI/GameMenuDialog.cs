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
    public class GameMenuDialog : UIDialog
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="mpq"></param>
        public GameMenuDialog(UIScreen parent, Mpq mpq)
            : base(parent, mpq, "glue\\palmm", BuiltIns.GameMenuBin)
        {
            BackgroundPath = null;
        }

        const int RETURNTOGAME_ELEMENT_INDEX = 1;
        const int SAVEGAME_ELEMENT_INDEX = 2;
        const int LOADGAME_ELEMENT_INDEX = 3;
        const int PAUSEGAME_ELEMENT_INDEX = 4;
        const int RESUMEGAME_ELEMENT_INDEX = 5;
        const int OPTIONS_ELEMENT_INDEX = 6;
        const int HELP_ELEMENT_INDEX = 7;
        const int MISSIONOBJECTIVES_ELEMENT_INDEX = 8;
        const int ENDMISSION_ELEMENT_INDEX = 9;

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

            Elements[RETURNTOGAME_ELEMENT_INDEX].Activate +=
                delegate(object sender, SCEventArgs args) 
                {
                    if (ReturnToGame != null)
                    {
                        ReturnToGame(this, new SCEventArgs());
                    }
                };

            Elements[MISSIONOBJECTIVES_ELEMENT_INDEX].Activate +=
                delegate(object sender, SCEventArgs args) 
                {
                    ObjectivesDialog d = new ObjectivesDialog(this, this.Mpq);
                    d.Previous += delegate(object sender2, SCEventArgs args2) 
                    { 
                        DismissDialog(); 
                    };
                    ShowDialog(d);
                };

            Elements[ENDMISSION_ELEMENT_INDEX].Activate +=
                delegate(object sender, SCEventArgs args) 
                {
                    EndMissionDialog d = new EndMissionDialog(this, this.Mpq);
                    d.Previous += delegate(object sender2, SCEventArgs args2) 
                    { 
                        DismissDialog(); 
                    };
#if false
					d.Quit += delegate () { DismissDialog ();
								/* XXX hack just to get things looking right */
								parent.DismissDialog ();
								Game.Instance.SwitchToScreen (UIScreenType.MainMenu); };
#endif
                    ShowDialog(d);
                };

            Elements[OPTIONS_ELEMENT_INDEX].Activate +=
                delegate(object sender, SCEventArgs args) 
                {
                    OptionsDialog d = new OptionsDialog(this, this.Mpq);
                    d.Previous += delegate(object sender2, SCEventArgs args2)  
                    { 
                        DismissDialog(); 
                    };
                    ShowDialog(d);
                };

            Elements[HELP_ELEMENT_INDEX].Activate +=
                delegate(object sender, SCEventArgs args) 
                {
                    HelpDialog d = new HelpDialog(this, this.Mpq);
                    d.Previous += delegate(object sender2, SCEventArgs args2) 
                    { 
                        DismissDialog(); 
                    };
                    ShowDialog(d);
                };
        }

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<SCEventArgs> ReturnToGame;
    }
}
