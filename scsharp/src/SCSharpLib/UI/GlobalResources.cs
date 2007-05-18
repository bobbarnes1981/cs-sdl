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
    public class GlobalResources
    {
        Mpq stardatMpq;
        Mpq broodatMpq;

        Resources starcraftResources;
        Resources broodwarResources;

        static GlobalResources instance;

        /// <summary>
        /// 
        /// </summary>
        public static GlobalResources Instance
        {
            get { return instance; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stardatMpq"></param>
        /// <param name="broodatMpq"></param>
        public GlobalResources(Mpq stardatMpq, Mpq broodatMpq)
        {
            if (instance != null)
            {
                throw new Exception("There can only be one GlobalResources");
            }

            this.stardatMpq = stardatMpq;
            this.broodatMpq = broodatMpq;

            starcraftResources = new Resources();
            broodwarResources = new Resources();

            instance = this;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Load()
        {
            ThreadPool.QueueUserWorkItem(ResourceLoader);
        }

        /// <summary>
        /// 
        /// </summary>
        public void LoadSingleThreaded()
        {
            ResourceLoader(null);
        }

        /// <summary>
        /// 
        /// </summary>
        Resources Resources
        {
            get { return Game.Instance.PlayingBroodWar ? broodwarResources : starcraftResources; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Tbl ImagesTbl
        {
            get { return Resources.ImagesTbl; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Tbl SfxDataTbl
        {
            get { return Resources.SfxDataTbl; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Tbl SpritesTbl
        {
            get { return Resources.SpritesTbl; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Tbl GluAllTbl
        {
            get { return Resources.GluAllTbl; }
        }

        /// <summary>
        /// 
        /// </summary>
        public ImagesDat ImagesDat
        {
            get { return Resources.ImagesDat; }
        }

        /// <summary>
        /// 
        /// </summary>
        public SpritesDat SpritesDat
        {
            get { return Resources.SpritesDat; }
        }

        /// <summary>
        /// 
        /// </summary>
        public SfxDataDat SfxDataDat
        {
            get { return Resources.SfxDataDat; }
        }

        /// <summary>
        /// 
        /// </summary>
        public ScriptBin IScriptBin
        {
            get { return Resources.ScriptBin; }
        }

        /// <summary>
        /// 
        /// </summary>
        public UnitsDat UnitsDat
        {
            get { return Resources.UnitsDat; }
        }

        /// <summary>
        /// 
        /// </summary>
        public FlingyDat FlingyDat
        {
            get { return Resources.FlingyDat; }
        }

        /// <summary>
        /// 
        /// </summary>
        public MapDataDat MapDataDat
        {
            get { return Resources.MapDataDat; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Tbl MapDataTbl
        {
            get { return Resources.MapDataTbl; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Resources StarDat
        {
            get { return starcraftResources; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Resources BrooDat
        {
            get { return broodwarResources; }
        }

        void ResourceLoader(object state)
        {
            try
            {
                starcraftResources.ImagesTbl = (Tbl)stardatMpq.GetResource(Builtins.ImagesTbl);
                starcraftResources.SfxDataTbl = (Tbl)stardatMpq.GetResource(Builtins.SfxDataTbl);
                starcraftResources.SpritesTbl = (Tbl)stardatMpq.GetResource(Builtins.SpritesTbl);
                starcraftResources.GluAllTbl = (Tbl)stardatMpq.GetResource(Builtins.GluAllTbl);
                starcraftResources.MapDataTbl = (Tbl)stardatMpq.GetResource(Builtins.MapDataTbl);
                starcraftResources.ImagesDat = (ImagesDat)stardatMpq.GetResource(Builtins.ImagesDat);
                starcraftResources.SfxDataDat = (SfxDataDat)stardatMpq.GetResource(Builtins.SfxDataDat);
                starcraftResources.SpritesDat = (SpritesDat)stardatMpq.GetResource(Builtins.SpritesDat);
                starcraftResources.ScriptBin = (ScriptBin)stardatMpq.GetResource(Builtins.IScriptBin);
                starcraftResources.UnitsDat = (UnitsDat)stardatMpq.GetResource(Builtins.UnitsDat);
                starcraftResources.FlingyDat = (FlingyDat)stardatMpq.GetResource(Builtins.FlingyDat);
                starcraftResources.MapDataDat = (MapDataDat)stardatMpq.GetResource(Builtins.MapDataDat);

                if (broodatMpq != null)
                {
                    broodwarResources.ImagesTbl = (Tbl)broodatMpq.GetResource(Builtins.ImagesTbl);
                    broodwarResources.SfxDataTbl = (Tbl)broodatMpq.GetResource(Builtins.SfxDataTbl);
                    broodwarResources.SpritesTbl = (Tbl)broodatMpq.GetResource(Builtins.SpritesTbl);
                    broodwarResources.GluAllTbl = (Tbl)broodatMpq.GetResource(Builtins.GluAllTbl);
                    broodwarResources.MapDataTbl = (Tbl)broodatMpq.GetResource(Builtins.MapDataTbl);
                    broodwarResources.ImagesDat = (ImagesDat)broodatMpq.GetResource(Builtins.ImagesDat);
                    broodwarResources.SfxDataDat = (SfxDataDat)broodatMpq.GetResource(Builtins.SfxDataDat);
                    broodwarResources.SpritesDat = (SpritesDat)broodatMpq.GetResource(Builtins.SpritesDat);
                    broodwarResources.ScriptBin = (ScriptBin)broodatMpq.GetResource(Builtins.IScriptBin);
                    broodwarResources.UnitsDat = (UnitsDat)broodatMpq.GetResource(Builtins.UnitsDat);
                    broodwarResources.FlingyDat = (FlingyDat)broodatMpq.GetResource(Builtins.FlingyDat);
                    broodwarResources.MapDataDat = (MapDataDat)broodatMpq.GetResource(Builtins.MapDataDat);
                }

                // notify we're ready to roll
                Events.PushUserEvent(new UserEventArgs(new ReadyDelegate(FinishedLoading)));
            }
            catch (SdlException e)
            {
                Console.WriteLine("Global Resource loader failed: {0}", e);
                Events.PushUserEvent(new UserEventArgs(new ReadyDelegate(Events.QuitApplication)));
            }
        }

        void FinishedLoading()
        {
            if (Ready != null)
            {
                Ready();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public event ReadyDelegate Ready;
    }
}
