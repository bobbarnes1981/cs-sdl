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

using SdlDotNet.Core;
using SCSharp;
using SCSharp.MpqLib;

namespace SCSharp.UI
{
    /// <summary>
    /// 
    /// </summary>
    public class SCResources
    {
        private ScriptBin scriptBin;

        /// <summary>
        /// 
        /// </summary>
        public ScriptBin ScriptBin
        {
            get { return scriptBin; }
            set { scriptBin = value; }
        }
        private ImagesDat imagesDat;

        /// <summary>
        /// 
        /// </summary>
        public ImagesDat ImagesDat
        {
            get { return imagesDat; }
            set { imagesDat = value; }
        }
        private SpritesDat spritesDat;

        /// <summary>
        /// 
        /// </summary>
        public SpritesDat SpritesDat
        {
            get { return spritesDat; }
            set { spritesDat = value; }
        }
        private SfxDataDat sfxDataDat;

        /// <summary>
        /// 
        /// </summary>
        public SfxDataDat SfxDataDat
        {
            get { return sfxDataDat; }
            set { sfxDataDat = value; }
        }
        private UnitsDat unitsDat;

        /// <summary>
        /// 
        /// </summary>
        public UnitsDat UnitsDat
        {
            get { return unitsDat; }
            set { unitsDat = value; }
        }
        private FlingyDat flingyDat;

        /// <summary>
        /// 
        /// </summary>
        public FlingyDat FlingyDat
        {
            get { return flingyDat; }
            set { flingyDat = value; }
        }
        private MapDataDat mapDataDat;

        /// <summary>
        /// 
        /// </summary>
        public MapDataDat MapDataDat
        {
            get { return mapDataDat; }
            set { mapDataDat = value; }
        }

        private Tbl imagesTbl;

        /// <summary>
        /// 
        /// </summary>
        public Tbl ImagesTbl
        {
            get { return imagesTbl; }
            set { imagesTbl = value; }
        }
        private Tbl sfxDataTbl;

        /// <summary>
        /// 
        /// </summary>
        public Tbl SfxDataTbl
        {
            get { return sfxDataTbl; }
            set { sfxDataTbl = value; }
        }
        private Tbl spritesTbl;

        /// <summary>
        /// 
        /// </summary>
        public Tbl SpritesTbl
        {
            get { return spritesTbl; }
            set { spritesTbl = value; }
        }
        private Tbl gluAllTbl;

        /// <summary>
        /// 
        /// </summary>
        public Tbl GluAllTbl
        {
            get { return gluAllTbl; }
            set { gluAllTbl = value; }
        }
        private Tbl mapDataTbl;

        /// <summary>
        /// 
        /// </summary>
        public Tbl MapDataTbl
        {
            get { return mapDataTbl; }
            set { mapDataTbl = value; }
        }
    }
}
