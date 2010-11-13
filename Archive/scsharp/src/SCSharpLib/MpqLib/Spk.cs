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
using System.Collections.Generic;

using SCSharp.UI;

namespace SCSharp.MpqLib
{
    /// <summary>
    /// 
    /// </summary>
    public class Spk : IMpqResource
    {
        ParallaxLayer[] layers;

        /// <summary>
        /// 
        /// </summary>
        public Spk()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        public void ReadFromStream(Stream stream)
        {
            layers = new ParallaxLayer[Utilities.ReadWord(stream)];

            for (int i = 0; i < layers.Length; i++)
            {
                int numobjects = Utilities.ReadWord(stream);
                layers[i] = new ParallaxLayer(numobjects);
                Console.WriteLine("Layer {0} has {1} objects", i, numobjects);
            }

            for (int i = 0; i < layers.Length; i++)
            {
                Console.WriteLine("Reading layer {0}", i);
                layers[i].ReadFromStream(stream);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ParallaxLayer[] Layers
        {
            get { return layers; }
        }
    }
}
