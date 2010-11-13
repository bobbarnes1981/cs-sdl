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

using SCSharp.UI;
using SCSharp.MpqLib;
using NUnit.Framework;

namespace SCSharp.Tests
{
    /// <summary>
    /// 
    /// </summary>
    [TestFixture]
    public class DumpGrp
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        [Test]
        public void DumpGrpTest()
        {
            string filename = "C:\\data\\SCSharpData";
            string palettename = "C:\\data\\SCSharpData";

            ProcessGrp(filename, palettename);
        }

        private static void ProcessGrp(string filename, string palettename)
        {
            Console.WriteLine("grp file {0}", filename);
            Console.WriteLine("palette file {0}", palettename);

            FileStream fs = File.OpenRead(filename);

            Grp grp = new Grp();

            ((IMpqResource)grp).ReadFromStream(fs);
            Pcx pal = new Pcx();
            pal.ReadFromStream(File.OpenRead(palettename), -1, -1);

            for (int i = 0; i < grp.FrameCount; i++)
            {
                Bmp.WriteBmp(String.Format("output{0:0000}.bmp", i),
                          grp.GetFrame(i),
                          grp.Width, grp.Height,
                          pal.Palette);
            }
        }
    }
}