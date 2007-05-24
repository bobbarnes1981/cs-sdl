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
    public static class GlobalResources
    {
        static Mpq starDataMpq;

        /// <summary>
        /// 
        /// </summary>
        public static Mpq StarDataMpq
        {
            get { return GlobalResources.starDataMpq; }
            set { GlobalResources.starDataMpq = value; }
        }
        static Mpq broodDataMpq;

        /// <summary>
        /// 
        /// </summary>
        public static Mpq BroodDataMpq
        {
            get { return GlobalResources.broodDataMpq; }
            set { GlobalResources.broodDataMpq = value; }
        }

        static SCResources starcraftResources;
        static SCResources broodwarResources;

        //static GlobalResources instance;

        ///// <summary>
        ///// 
        ///// </summary>
        //public static GlobalResources Instance
        //{
        //    get { return instance; }
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="starDataMpq"></param>
        /// <param name="broodDataMpq"></param>
        public static void LoadMpq(Mpq starDataMpq, Mpq broodDataMpq)
        {
            //if (instance != null)
            //{
            //    throw new SCException("There can only be one GlobalResources");
            //}

            GlobalResources.starDataMpq = starDataMpq;
            GlobalResources.broodDataMpq = broodDataMpq;

            starcraftResources = new SCResources();
            broodwarResources = new SCResources();

            //instance = this;
        }

        /// <summary>
        /// 
        /// </summary>
        public static void Load()
        {
            ThreadPool.QueueUserWorkItem(ResourceLoader);
        }

        /// <summary>
        /// 
        /// </summary>
        public static void LoadSingleThreaded()
        {
            ResourceLoader(null);
        }

        /// <summary>
        /// 
        /// </summary>
        static SCResources Resources
        {
            get { return Game.Instance.PlayingBroodWar ? broodwarResources : starcraftResources; }
        }

        /// <summary>
        /// 
        /// </summary>
        public static Tbl ImagesTbl
        {
            get { return Resources.ImagesTbl; }
        }

        /// <summary>
        /// 
        /// </summary>
        public static Tbl SfxDataTbl
        {
            get { return Resources.SfxDataTbl; }
        }

        /// <summary>
        /// 
        /// </summary>
        public static Tbl SpritesTbl
        {
            get { return Resources.SpritesTbl; }
        }

        /// <summary>
        /// 
        /// </summary>
        public static Tbl GluAllTbl
        {
            get { return Resources.GluAllTbl; }
        }

        /// <summary>
        /// 
        /// </summary>
        public static ImagesDat ImagesDat
        {
            get { return Resources.ImagesDat; }
        }

        /// <summary>
        /// 
        /// </summary>
        public static SpritesDat SpritesDat
        {
            get { return Resources.SpritesDat; }
        }

        /// <summary>
        /// 
        /// </summary>
        public static SfxDataDat SfxDataDat
        {
            get { return Resources.SfxDataDat; }
        }

        /// <summary>
        /// 
        /// </summary>
        public static ScriptBin IScriptBin
        {
            get { return Resources.ScriptBin; }
        }

        /// <summary>
        /// 
        /// </summary>
        public static UnitsDat UnitsDat
        {
            get { return Resources.UnitsDat; }
        }

        /// <summary>
        /// 
        /// </summary>
        public static FlingyDat FlingyDat
        {
            get { return Resources.FlingyDat; }
        }

        /// <summary>
        /// 
        /// </summary>
        public static MapDataDat MapDataDat
        {
            get { return Resources.MapDataDat; }
        }

        /// <summary>
        /// 
        /// </summary>
        public static Tbl MapDataTbl
        {
            get { return Resources.MapDataTbl; }
        }

        /// <summary>
        /// 
        /// </summary>
        public static SCResources StarDat
        {
            get { return starcraftResources; }
        }

        /// <summary>
        /// 
        /// </summary>
        public static SCResources BroodDat
        {
            get { return broodwarResources; }
        }

        static void ResourceLoader(object state)
        {
            try
            {
                starcraftResources.ImagesTbl = (Tbl)starDataMpq.GetResource(BuiltIns.ImagesTbl);
                starcraftResources.SfxDataTbl = (Tbl)starDataMpq.GetResource(BuiltIns.SfxDataTbl);
                starcraftResources.SpritesTbl = (Tbl)starDataMpq.GetResource(BuiltIns.SpritesTbl);
                starcraftResources.GluAllTbl = (Tbl)starDataMpq.GetResource(BuiltIns.GluAllTbl);
                starcraftResources.MapDataTbl = (Tbl)starDataMpq.GetResource(BuiltIns.MapDataTbl);
                starcraftResources.ImagesDat = (ImagesDat)starDataMpq.GetResource(BuiltIns.ImagesDat);
                starcraftResources.SfxDataDat = (SfxDataDat)starDataMpq.GetResource(BuiltIns.SfxDataDat);
                starcraftResources.SpritesDat = (SpritesDat)starDataMpq.GetResource(BuiltIns.SpritesDat);
                starcraftResources.ScriptBin = (ScriptBin)starDataMpq.GetResource(BuiltIns.IScriptBin);
                starcraftResources.UnitsDat = (UnitsDat)starDataMpq.GetResource(BuiltIns.UnitsDat);
                starcraftResources.FlingyDat = (FlingyDat)starDataMpq.GetResource(BuiltIns.FlingyDat);
                starcraftResources.MapDataDat = (MapDataDat)starDataMpq.GetResource(BuiltIns.MapDataDat);

                if (broodDataMpq != null)
                {
                    broodwarResources.ImagesTbl = (Tbl)broodDataMpq.GetResource(BuiltIns.ImagesTbl);
                    broodwarResources.SfxDataTbl = (Tbl)broodDataMpq.GetResource(BuiltIns.SfxDataTbl);
                    broodwarResources.SpritesTbl = (Tbl)broodDataMpq.GetResource(BuiltIns.SpritesTbl);
                    broodwarResources.GluAllTbl = (Tbl)broodDataMpq.GetResource(BuiltIns.GluAllTbl);
                    broodwarResources.MapDataTbl = (Tbl)broodDataMpq.GetResource(BuiltIns.MapDataTbl);
                    broodwarResources.ImagesDat = (ImagesDat)broodDataMpq.GetResource(BuiltIns.ImagesDat);
                    broodwarResources.SfxDataDat = (SfxDataDat)broodDataMpq.GetResource(BuiltIns.SfxDataDat);
                    broodwarResources.SpritesDat = (SpritesDat)broodDataMpq.GetResource(BuiltIns.SpritesDat);
                    broodwarResources.ScriptBin = (ScriptBin)broodDataMpq.GetResource(BuiltIns.IScriptBin);
                    broodwarResources.UnitsDat = (UnitsDat)broodDataMpq.GetResource(BuiltIns.UnitsDat);
                    broodwarResources.FlingyDat = (FlingyDat)broodDataMpq.GetResource(BuiltIns.FlingyDat);
                    broodwarResources.MapDataDat = (MapDataDat)broodDataMpq.GetResource(BuiltIns.MapDataDat);
                }

                // notify we're ready to roll
                Events.PushUserEvent(new UserEventArgs(new EventHandler<SCEventArgs>(FinishedLoading)));
            }
            catch (SdlException e)
            {
                Console.WriteLine("Global Resource loader failed: {0}", e);
                Events.PushUserEvent(new UserEventArgs(new EventHandler<SCEventArgs>(Game.Quit)));
            }
        }

        static void FinishedLoading(object sender, EventArgs e)
        {
            if (Ready != null)
            {
                Ready(new SCEventArgs(), new SCEventArgs());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static event EventHandler<SCEventArgs> Ready;
    }
}
