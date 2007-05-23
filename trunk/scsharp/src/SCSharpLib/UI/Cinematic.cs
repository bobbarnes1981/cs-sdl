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
using SdlDotNet.Graphics;
using SCSharp;
using System.Drawing;
using SCSharp.MpqLib;

namespace SCSharp.UI
{
    /// <summary>
    /// 
    /// </summary>
    public class Cinematic : UIScreen
    {
        SmackerPlayer player;
        string resourcePath;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mpq"></param>
        /// <param name="resourcePath"></param>
        public Cinematic(Mpq mpq, string resourcePath)
            : base(mpq, null, null)
        {
            this.resourcePath = resourcePath;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="painter"></param>
        public override void AddToPainter(Painter painter)
        {
            if (painter == null)
            {
                throw new ArgumentNullException("painter");
            }
            /* XXX this should be abstracted into Game with the other mouse settings */
            Mouse.ShowCursor = false;

            painter.Add(Layer.Background, VideoPainter);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="painter"></param>
        public override void RemoveFromPainter(Painter painter)
        {
            if (painter == null)
            {
                throw new ArgumentNullException("painter");
            }
            painter.Remove(Layer.Background, VideoPainter);
        }

        void VideoPainter(Surface surf, DateTime dt)
        {
            if (player == null)
            {
                player = new SmackerPlayer(resourcePath,
                                (Stream)this.Mpq.GetResource(resourcePath),
                                Painter.ScreenResX, Painter.ScreenResY);

                player.Finished += PlayerFinished;
                player.Play();
            }

            player.BlitSurface(surf);
        }

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<SCEventArgs> Finished;

        void PlayerFinished(object sender, EventArgs e)
        {
            player = null;
            if (Finished != null)
            {
                Finished(this, new SCEventArgs());
            }
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
            if (args.Key == Key.Escape
                || args.Key == Key.Return
                || args.Key == Key.Space)
            {
                player.Stop();
                PlayerFinished(this, new SCEventArgs());
            }
        }
    }
}
