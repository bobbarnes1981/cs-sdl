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
using System.Drawing;
using System.Threading;
using System.Globalization;

using SdlDotNet;
using SCSharp;

using SCSharp.MpqLib;

namespace SCSharp.UI
{
    /// <summary>
    ///
    /// </summary>
    public class RaceSelectionScreen : UIScreen
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="mpq"></param>
        public RaceSelectionScreen(Mpq mpq)
            : base(mpq, "glue\\PalNl",
        Game.Instance.PlayingBroodWar ? BuiltIns.GluExpCampaignBin : BuiltIns.GluCampaignBin)
        {
            BackgroundPath = null;
        }

        int[] BroodwarCampaignsMapDataStart = new int[] {
31,
40,
49
};

        Race[] BroodWarRaces = new Race[] {
Race.Protoss,
Race.Terran,
Race.Zerg
};

        int[] StarcraftCampaignsMapDataStart = new int[] {
0,
11,
21
};

        Race[] StarcraftRaces = new Race[] {
Race.Terran,
Race.Zerg,
Race.Protoss
};

        const int LOADSAVED_ELEMENT_INDEX = 3;
        const int THIRD_CAMPAIGN_ELEMENT_INDEX = 4;
        const int FIRST_CAMPAIGN_ELEMENT_INDEX = 5;
        const int SECOND_CAMPAIGN_ELEMENT_INDEX = 6;
        const int CANCEL_ELEMENT_INDEX = 7;
        const int PLAYCUSTOM_ELEMENT_INDEX = 8;
        const int SECOND_BUT_FIRST_INCOMPLETE_INDEX = 9;
        const int THIRD_BUT_FIRST_INCOMPLETE_INDEX = 10;
        const int THIRD_BUT_SECOND_INCOMPLETE_INDEX = 11;

        /// <summary>
        ///
        /// </summary>
        protected override void ResourceLoader()
        {
            base.ResourceLoader();

            for (int i = 0; i < Elements.Count; i++)
                Console.WriteLine("{0}: {1} '{2}'", i, Elements[i].Type, Elements[i].Text);

            Elements[THIRD_CAMPAIGN_ELEMENT_INDEX].MouseEnterEvent +=
            delegate(object sender, SCEventArgs args)
            {
                Console.WriteLine("over third campaign element");
                if (true /* XXX this should come from the player's file */)
                {
                    Elements[THIRD_BUT_FIRST_INCOMPLETE_INDEX].Visible = true;
                }
            };

            Elements[THIRD_CAMPAIGN_ELEMENT_INDEX].MouseLeaveEvent +=
            delegate(object sender, SCEventArgs args)
            {
                if (true /* XXX this should come from the player's file */)
                {
                    Elements[THIRD_BUT_FIRST_INCOMPLETE_INDEX].Visible = false;
                }
            };

            Elements[SECOND_CAMPAIGN_ELEMENT_INDEX].MouseEnterEvent +=
            delegate(object sender, SCEventArgs args)
            {
                Console.WriteLine("over second campaign element");
                if (true /* XXX this should come from the player's file */)
                {
                    Elements[SECOND_BUT_FIRST_INCOMPLETE_INDEX].Visible = true;
                }
            };

            Elements[SECOND_CAMPAIGN_ELEMENT_INDEX].MouseLeaveEvent +=
            delegate(object sender, SCEventArgs args)
            {
                if (true /* XXX this should come from the player's file */)
                {
                    Elements[SECOND_BUT_FIRST_INCOMPLETE_INDEX].Visible = false;
                }
            };

            Elements[FIRST_CAMPAIGN_ELEMENT_INDEX].Activate +=
            delegate(object sender, SCEventArgs args)
            {
                SelectCampaign(0);
            };

            Elements[SECOND_CAMPAIGN_ELEMENT_INDEX].Activate +=
            delegate(object sender, SCEventArgs args)
            {
                SelectCampaign(1);
            };

            Elements[THIRD_CAMPAIGN_ELEMENT_INDEX].Activate +=
            delegate(object sender, SCEventArgs args)
            {
                SelectCampaign(2);
            };


            Elements[CANCEL_ELEMENT_INDEX].Activate +=
            delegate(object sender, SCEventArgs args)
            {
                Game.Instance.SwitchToScreen(UIScreenType.LogOn);
            };

            Elements[LOADSAVED_ELEMENT_INDEX].Activate +=
            delegate(object sender, SCEventArgs args)
            {
                Game.Instance.SwitchToScreen(new LoadSavedScreen(this.Mpq));
            };

            Elements[PLAYCUSTOM_ELEMENT_INDEX].Activate +=
            delegate(object sender, SCEventArgs args)
            {
                Game.Instance.SwitchToScreen(new PlayCustomScreen(this.Mpq));
            };
        }

        void SelectCampaign(int campaign)
        {
            uint mapdataIndex;
            string prefix;
            string markup;

            Game.Instance.Race = (Game.Instance.PlayingBroodWar ? BroodWarRaces : StarcraftRaces)[campaign];

            mapdataIndex = GlobalResources.MapDataDat.GetFileIndex((uint)(Game.Instance.PlayingBroodWar ? BroodwarCampaignsMapDataStart : StarcraftCampaignsMapDataStart)[campaign]);

            prefix = GlobalResources.MapDataTbl[(int)mapdataIndex];
            markup = String.Format(CultureInfo.CurrentCulture, "rez\\Est{0}{1}{2}.txt",
            Utilities.RaceChar[(int)Game.Instance.Race],
            prefix.EndsWith("tutorial") ? "0t" : prefix.Substring(prefix.Length - 2),
            Game.Instance.PlayingBroodWar ? "x" : "");

            Game.Instance.SwitchToScreen(new EstablishingShot(markup, prefix, this.Mpq));
        }
    }
}
