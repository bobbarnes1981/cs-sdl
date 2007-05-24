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
    public class ReadyRoomScreen : UIScreen
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="mpq"></param>
        /// <param name="scenarioPrefix"></param>
        /// <param name="startElementIndex"></param>
        /// <param name="cancelElementIndex"></param>
        /// <param name="skipTutorialElementIndex"></param>
        /// <param name="replayElementIndex"></param>
        /// <param name="transmissionElementIndex"></param>
        /// <param name="objectivesElementIndex"></param>
        /// <param name="firstPortraitElementIndex"></param>
        public ReadyRoomScreen(Mpq mpq,
        string scenarioPrefix,
        int startElementIndex,
        int cancelElementIndex,
        int skipTutorialElementIndex,
        int replayElementIndex,
        int transmissionElementIndex,
        int objectivesElementIndex,
        int firstPortraitElementIndex)
            : base(mpq,
        String.Format("glue\\Ready{0}", Utilities.RaceChar[(int)Game.Instance.Race]),
        String.Format(BuiltIns.GluRdyBin, Utilities.RaceCharLower[(int)Game.Instance.Race]))
        {
            if (mpq == null)
            {
                throw new ArgumentNullException("mpq");
            }
            BackgroundPath = String.Format("glue\\PalR{0}\\Backgnd.pcx", Utilities.RaceCharLower[(int)Game.Instance.Race]);
            FontPalettePath = String.Format("glue\\PalR{0}\\tFont.pcx", Utilities.RaceCharLower[(int)Game.Instance.Race]);
            EffectPalettePath = String.Format("glue\\PalR{0}\\tEffect.pcx", Utilities.RaceCharLower[(int)Game.Instance.Race]);
            ArrowGrpPath = String.Format("glue\\PalR{0}\\arrow.grp", Utilities.RaceCharLower[(int)Game.Instance.Race]);

            this.startElementIndex = startElementIndex;
            this.cancelElementIndex = cancelElementIndex;
            this.skipTutorialElementIndex = skipTutorialElementIndex;
            this.replayElementIndex = replayElementIndex;
            this.transmissionElementIndex = transmissionElementIndex;
            this.objectivesElementIndex = objectivesElementIndex;
            this.firstPortraitElementIndex = firstPortraitElementIndex;

            this.scenario = (Chk)mpq.GetResource(scenarioPrefix + "\\staredit\\scenario.chk");
            this.scenario_prefix = scenarioPrefix;
        }

        BriefingRunner runner;
        Chk scenario;
        string scenario_prefix;
        int startElementIndex;
        int cancelElementIndex;
        int skipTutorialElementIndex;
        int replayElementIndex;
        int transmissionElementIndex;
        int objectivesElementIndex;
        int firstPortraitElementIndex;

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

            if (scenario_prefix.EndsWith("tutorial"))
            {
                Elements[skipTutorialElementIndex].Visible = true;
                /* XXX Activate */
            }

            Elements[cancelElementIndex].Activate +=
            delegate(object sender, SCEventArgs args)
            {
                StopBriefing();
                Game.Instance.SwitchToScreen(UIScreenType.LogOn);
            };

            Elements[replayElementIndex].Activate +=
            delegate(object sender, SCEventArgs args)
            {
                StopBriefing();
                PlayBriefing();
            };

            Elements[startElementIndex].Activate +=
            delegate(object sender, SCEventArgs args)
            {
                StopBriefing();
                Game.Instance.SwitchToScreen(new GameScreen(this.Mpq, scenario_prefix, scenario));
            };

            runner = new BriefingRunner(this, scenario, scenario_prefix);
        }

        void StopBriefing()
        {
            Events.Tick -= runner.Tick;
            runner.Stop();

            Elements[transmissionElementIndex].Visible = false;
            Elements[transmissionElementIndex].Text = "";

            Elements[objectivesElementIndex].Visible = false;
            Elements[objectivesElementIndex].Text = "";

            for (int i = 0; i < 4; i++)
            {
                Elements[firstPortraitElementIndex + i].Background = null;
                Elements[firstPortraitElementIndex + i].Visible = false;
            }
        }

        void PlayBriefing()
        {
            runner.Play();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="surf"></param>
        /// <param name="now"></param>
        protected override void FirstPaint(Surface surf, DateTime now)
        {
            base.FirstPaint(surf, now);

            Events.Tick += runner.Tick;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="objectivesText"></param>
        public void SetObjectives(string objectivesText)
        {
            Elements[objectivesElementIndex].Visible = true;
            Elements[objectivesElementIndex].Text = objectivesText;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="transmissionText"></param>
        public void SetTransmissionText(string transmissionText)
        {
            Elements[transmissionElementIndex].Visible = true;
            Elements[transmissionElementIndex].Text = transmissionText;
        }

        int highlightedPortrait = -1;

        /// <summary>
        ///
        /// </summary>
        /// <param name="slot"></param>
        public void HighlightPortrait(int slot)
        {
            if (highlightedPortrait != -1)
            {
                EndHighlightPortrait(highlightedPortrait);
            }

            Elements[firstPortraitElementIndex + slot].Background = String.Format("glue\\Ready{0}\\{0}FrameH{1}.pcx",
            Utilities.RaceChar[(int)Game.Instance.Race],
            slot + 1);
            highlightedPortrait = slot;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="slot"></param>
        public void EndHighlightPortrait(int slot)
        {
            if (Elements[firstPortraitElementIndex + slot].Visible)
            {
                Elements[firstPortraitElementIndex + slot].Background = String.Format("glue\\Ready{0}\\{0}Frame{1}.pcx",
                Utilities.RaceChar[(int)Game.Instance.Race],
                slot + 1);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="slot"></param>
        public void ShowPortrait(int slot)
        {
            Elements[firstPortraitElementIndex + slot].Visible = true;
            Elements[firstPortraitElementIndex + slot].Background = String.Format("glue\\Ready{0}\\{0}Frame{1}.pcx",
            Utilities.RaceChar[(int)Game.Instance.Race],
            slot + 1);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="slot"></param>
        public void HidePortrait(int slot)
        {
            Elements[firstPortraitElementIndex + slot].Visible = false;
            Elements[firstPortraitElementIndex + slot].Background = null;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="mpq"></param>
        /// <param name="scenarioPrefix"></param>
        /// <returns></returns>
        public static ReadyRoomScreen Create(Mpq mpq,
        string scenarioPrefix)
        {
            switch (Game.Instance.Race)
            {
                case Race.Terran:
                    return new TerranReadyRoomScreen(mpq, scenarioPrefix);
                case Race.Protoss:
                    return new ProtossReadyRoomScreen(mpq, scenarioPrefix);
                case Race.Zerg:
                    return new ZergReadyRoomScreen(mpq, scenarioPrefix);
                default:
                    return null;
            }
        }
    }
}
