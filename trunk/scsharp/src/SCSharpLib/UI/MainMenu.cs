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
    public class MainMenu : UIScreen
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="mpq"></param>
        public MainMenu(Mpq mpq)
            : base(mpq, "glue\\Palmm", BuiltIns.GluMainBin)
        {
        }

        const int EXIT_ELEMENT_INDEX = 2;
        const int SINGLEPLAYER_ELEMENT_INDEX = 3;
        const int MULTIPLAYER_ELEMENT_INDEX = 4;
        const int CAMPAIGNEDITOR_ELEMENT_INDEX = 5;
        const int INTRO_ELEMENT_INDEX = 8;
        const int CREDITS_ELEMENT_INDEX = 9;
        const int VERSION_ELEMENT_INDEX = 10;

        void ShowGameModeDialog(UIScreenType nextScreen)
        {
            GameModeDialog d = new GameModeDialog(this, this.Mpq);
            d.Cancel += delegate(object sender, SCEventArgs args)
            {
                DismissDialog();
            };
            d.Activate += delegate(object sender, GameModeActivateEventArgs args)
            {
                DismissDialog();
                try
                {
                    Game.Instance.PlayingBroodWar = args.Expansion;
                    GuiUtility.PlaySound(this.Mpq, BuiltIns.Mousedown2Wav);
                    Game.Instance.SwitchToScreen(nextScreen);
                }
                catch (Exception e)
                {
                    ShowDialog(new OkDialog(this, this.Mpq, e.Message));
                }
            };
            ShowDialog(d);
        }

        /// <summary>
        ///
        /// </summary>
        protected override void ResourceLoader()
        {
            base.ResourceLoader();

            Elements[VERSION_ELEMENT_INDEX].Text = "v0.0000004";

            Elements[SINGLEPLAYER_ELEMENT_INDEX].Activate +=
            delegate(object sender, SCEventArgs args)
            {
                if (Game.Instance.IsBroodWar)
                {
                    ShowGameModeDialog(UIScreenType.LogOn);
                }
                else
                {
                    GuiUtility.PlaySound(this.Mpq, BuiltIns.Mousedown2Wav);
                    Game.Instance.SwitchToScreen(UIScreenType.LogOn);
                }
            };

            Elements[MULTIPLAYER_ELEMENT_INDEX].Activate +=
            delegate(object sender, SCEventArgs args)
            {
                if (Game.Instance.IsBroodWar)
                {
                    ShowGameModeDialog(UIScreenType.Connection);
                }
                else
                {
                    GuiUtility.PlaySound(this.Mpq, BuiltIns.Mousedown2Wav);
                    Game.Instance.SwitchToScreen(UIScreenType.Connection);
                }
            };

            Elements[CAMPAIGNEDITOR_ELEMENT_INDEX].Activate +=
            delegate(object sender, SCEventArgs args)
            {
                OkDialog d = new OkDialog(this, this.Mpq,
                "The campaign editor functionality is not available in SCSharp");
                ShowDialog(d);
            };

            Elements[INTRO_ELEMENT_INDEX].Activate +=
            delegate(object sender, SCEventArgs args)
            {
                Cinematic introScreen = new Cinematic(this.Mpq,
                Game.Instance.IsBroodWar
                ? "smk\\starXIntr.smk"
                : "smk\\starintr.smk");
                introScreen.Finished +=
                    delegate(object sender2, SCEventArgs e2)
                    {
                        Game.Instance.SwitchToScreen(this);
                    };
                Game.Instance.SwitchToScreen(introScreen);
            };

            Elements[CREDITS_ELEMENT_INDEX].Activate +=
            delegate(object sender, SCEventArgs args)
            {
                Game.Instance.SwitchToScreen(new CreditsScreen(this.Mpq));
            };

            Elements[EXIT_ELEMENT_INDEX].Activate +=
            delegate(object sender, SCEventArgs args)
            {
                Game.Quit(this, new SCEventArgs());
            };
        }
    }
}
