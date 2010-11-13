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

using SCSharp.MpqLib;
using SCSharp.UI;
using SdlDotNet.Graphics;
using SdlDotNet.Core;
using SdlDotNet.Input;
using NUnit.Framework;

namespace SCSharp.Tests
{
    /// <summary>
    ///
    /// </summary>
    [TestFixture]
    public class FontFoo
    {
#if false
static string str1 = "abcdefghijklmnopqrstuvwxyz";
static string str2 = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
static string str3 = "1234567890!@#$%^&*()`~-_=+[]{}\\|;:'\",.<>/?";
#endif

        static string str4 = "Kyll";

        /// <summary>
        ///
        /// </summary>
        /// <param name="args"></param>
        [Test]
        public void FontFooTest()
        {
            MpqContainer mpq = new MpqContainer();
            //TODO: FIX
            mpq.Add(new MpqArchive("C:\\data\\SCSharpData\\SC\\install.exe"));
            mpq.Add(new MpqArchive("C:\\data\\SCSharpData\\StarDat.mpq"));

            SCFont fnt = (SCFont)mpq.GetResource("files\\font\\font16.fnt");
            Console.WriteLine("loading font palette");
            Stream palStream = (Stream)mpq.GetResource("glue\\palmm\\tfont.pcx");
            Pcx pcx1 = new Pcx();
            pcx1.ReadFromStream(palStream, -1, -1);

            Painter painter = new Painter(Video.SetVideoMode(600, 100), 300);

#if false
Surface textSurf1 = GuiUtil.ComposeText (str1, fnt, pcx1.Palette);
Surface textSurf2 = GuiUtil.ComposeText (str2, fnt, pcx1.Palette);
Surface textSurf3 = GuiUtil.ComposeText (str3, fnt, pcx1.Palette);
#endif
            Surface textSurf4 = GuiUtility.ComposeText(str4, fnt, pcx1.Palette);

            painter.Add(Layer.UI,
            delegate(Surface surf, DateTime now)
            {
#if false
surf.Blit (textSurf1, new Point (0,0));
surf.Blit (textSurf2, new Point (0, textSurf1.Size.Height));
surf.Blit (textSurf3, new Point (0, textSurf1.Size.Height + textSurf2.Size.Height));
#endif
                surf.Blit(textSurf4, new Point(0, 0));
            });

            painter.Add(Layer.Background,
            delegate(Surface surf, DateTime now)
            {
                surf.Fill(new Rectangle(new Point(0, 0), surf.Size), Color.Red);
            });

            Events.KeyboardUp += delegate(object sender, KeyboardEventArgs arg)
            {
                if (arg.Key == Key.Escape)
                {
                    Events.QuitApplication();
                }
            };

            Events.Run();
        }
    }
}
