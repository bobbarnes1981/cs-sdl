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
    public class BriefingRunner
    {
        TriggerData triggerData;
        Chk scenario;
        ReadyRoomScreen screen;
        string prefix;

        int sleepUntil;
        int totalElapsed;
        int currentAction;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="screen"></param>
        /// <param name="scenario"></param>
        /// <param name="scenarioPrefix"></param>
        public BriefingRunner(ReadyRoomScreen screen, Chk scenario,
                       string scenarioPrefix)
        {
            this.screen = screen;
            this.scenario = scenario;
            this.prefix = scenarioPrefix;
            triggerData = scenario.BriefingData;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Play()
        {
            currentAction = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Stop()
        {
            sleepUntil = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Tick(object sender, TickEventArgs e)
        {
            TriggerAction[] actions = triggerData.Triggers[0].Actions;

            if (currentAction == actions.Length)
            {
                return;
            }

            totalElapsed += e.TicksElapsed;

            /* if we're presently waiting, make sure
               enough time has gone by.  otherwise
               return */
            if (totalElapsed < sleepUntil)
            {
                return;
            }

            totalElapsed = 0;

            while (currentAction < actions.Length)
            {
                TriggerAction action = actions[currentAction];

                currentAction++;

                switch (action.Action)
                {
                    case 0: /* no action */
                        break;
                    case 1:
                        sleepUntil = (int)action.Delay;
                        return;
                    case 2:
                        GuiUtil.PlaySound(screen.Mpq, prefix + "\\" + scenario.GetMapString((int)action.WavIndex));
                        sleepUntil = (int)action.Delay;
                        return;
                    case 3:
                        screen.SetTransmissionText(scenario.GetMapString((int)action.TextIndex));
                        break;
                    case 4:
                        screen.SetObjectives(scenario.GetMapString((int)action.TextIndex));
                        break;
                    case 5:
                        screen.ShowPortrait((int)action.Group1);
                        break;
                    case 6:
                        screen.HidePortrait((int)action.Group1);
                        break;
                    case 7:
                        Console.WriteLine("Display Speaking Portrait(Slot, Time)");
                        break;
                    case 8:
                        Console.WriteLine("Transmission(Text, Slot, Time, Modifier, Wave, WavTime)");
                        screen.SetTransmissionText(scenario.GetMapString((int)action.TextIndex));
                        screen.HighlightPortrait((int)action.Group1);
                        GuiUtil.PlaySound(screen.Mpq, prefix + "\\" + scenario.GetMapString((int)action.WavIndex));
                        sleepUntil = (int)action.Delay;
                        return;
                    default:
                        break;
                }
            }
        }
    }
}
