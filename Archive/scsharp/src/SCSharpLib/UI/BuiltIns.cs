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

namespace SCSharp.UI
{
    /// <summary>
    /// 
    /// </summary>
    public static class BuiltIns
    {
        /* title screen */
        /// <summary>
        /// 
        /// </summary>
        public const string TitleDialogBin = "rez\\titledlg.bin";
#if RELEASE
		public const string TitlePcx = "glue\\title\\title.pcx";
#else
        /// <summary>
        /// 
        /// </summary>
        public const string TitlePcx = "glue\\title\\title-beta.pcx";
#endif

        /* UI strings */
        /// <summary>
        /// 
        /// </summary>
        public const string GluAllTbl = "rez\\gluAll.tbl";

        /* Main menu */
        /// <summary>
        /// 
        /// </summary>
        public const string GluMainBin = "rez\\gluMain.bin";

        /// <summary>
        /// 
        /// </summary>
        public const string GluGameModeBin = "rez\\gluGameMode.bin";

        /* Campaign screen */
        /// <summary>
        /// 
        /// </summary>
        public const string GluCampaignBin = "rez\\glucmpgn.bin"; // original
        /// <summary>
        /// 
        /// </summary>
        public const string GluExpCampaignBin = "rez\\gluexpcmpgn.bin"; // broodwar

        /* Play custom screen */
        /// <summary>
        /// 
        /// </summary>
        public const string GluCustomBin = "rez\\gluCustm.bin";

        /// <summary>
        /// 
        /// </summary>
        public const string GluCreateBin = "rez\\gluCreat.bin";

        /* load saved screen */
        /// <summary>
        /// 
        /// </summary>
        public const string GluLoadBin = "rez\\gluLoad.bin";

        /* Login screen */
        /// <summary>
        /// 
        /// </summary>
        public const string GluLogOnBin = "rez\\gluLogin.bin";
        /// <summary>
        /// 
        /// </summary>
        public const string GluPEditBin = "rez\\gluPEdit.bin";
        /// <summary>
        /// 
        /// </summary>
        public const string GluPOkBin = "rez\\gluPOk.bin";
        /// <summary>
        /// 
        /// </summary>
        public const string GluPOkCancelBin = "rez\\gluPOkCancel.bin";

        /* Ready room */
        /// <summary>
        /// 
        /// </summary>
        public const string GluReadyBin = "rez\\glurdy{0}.bin";

        /* Connection screen */
        /// <summary>
        /// 
        /// </summary>
        public const string GluConnectionBin = "rez\\gluConn.bin";

        /* Score screen */
        /// <summary>
        /// 
        /// </summary>
        public const string GluScoreBin = "rez\\gluScore.bin";

        /// <summary>
        /// 
        /// </summary>
        public const string ScoreVPMainPcx = "glue\\score{0}v\\pMain.pcx";
        /// <summary>
        /// 
        /// </summary>
        public const string ScoreDPMainPcx = "glue\\score{0}d\\pMain.pcx";

        /// <summary>
        /// 
        /// </summary>
        public const string GameConsolePcx = "game\\{0}console.pcx";

        /* scripts */
        /// <summary>
        /// 
        /// </summary>
        public const string IScriptBin = "scripts\\iscript.bin";
        /// <summary>
        /// 
        /// </summary>
        public const string AIScriptBin = "scripts\\aiscript.bin";

        /* arr files */
        /// <summary>
        /// 
        /// </summary>
        public const string FlingyDat = "arr\\flingy.dat";
        /// <summary>
        /// 
        /// </summary>
        public const string FlingyTbl = "arr\\flingy.tbl";
        /// <summary>
        /// 
        /// </summary>
        public const string ImagesDat = "arr\\images.dat";
        /// <summary>
        /// 
        /// </summary>
        public const string ImagesTbl = "arr\\images.tbl";
        //public const string MapdataDat = "arr\\mapdata.dat";
        //public const string MapdataTbl = "arr\\mapdata.tbl";
        /// <summary>
        /// 
        /// </summary>
        public const string OrdersDat = "arr\\orders.dat";
        /// <summary>
        /// 
        /// </summary>
        public const string OrdersTbl = "arr\\orders.tbl";
        /// <summary>
        /// 
        /// </summary>
        public const string MapDataDat = "arr\\mapdata.dat";
        /// <summary>
        /// 
        /// </summary>
        public const string MapDataTbl = "arr\\mapdata.tbl";
        /// <summary>
        /// 
        /// </summary>
        public const string PortDataDat = "arr\\portdata.dat";
        /// <summary>
        /// 
        /// </summary>
        public const string PortDataTbl = "arr\\portdata.tbl";
        /// <summary>
        /// 
        /// </summary>
        public const string SfxDataDat = "arr\\sfxdata.dat";
        /// <summary>
        /// 
        /// </summary>
        public const string SfxDataTbl = "arr\\sfxdata.tbl";
        /// <summary>
        /// 
        /// </summary>
        public const string SpritesDat = "arr\\sprites.dat";
        /// <summary>
        /// 
        /// </summary>
        public const string SpritesTbl = "arr\\sprites.tbl";
        /// <summary>
        /// 
        /// </summary>
        public const string TechDataDat = "arr\\techdata.dat";
        /// <summary>
        /// 
        /// </summary>
        public const string TechDataTbl = "arr\\techdata.tbl";
        /// <summary>
        /// 
        /// </summary>
        public const string UnitsDat = "arr\\units.dat";
        /// <summary>
        /// 
        /// </summary>
        public const string UnitsTbl = "arr\\units.tbl";
        /// <summary>
        /// 
        /// </summary>
        public const string UpgradesDat = "arr\\upgrades.dat";
        /// <summary>
        /// 
        /// </summary>
        public const string UpgradesTbl = "arr\\upgrades.tbl";
        /// <summary>
        /// 
        /// </summary>
        public const string WeaponsDat = "arr\\weapons.dat";
        /// <summary>
        /// 
        /// </summary>
        public const string WeaponsTbl = "arr\\weapons.tbl";

        /* sounds */
        /// <summary>
        /// 
        /// </summary>
        public const string MouseOverWav = "sound\\glue\\mouseover.wav";
        /// <summary>
        /// 
        /// </summary>
        public const string MouseDown2Wav = "sound\\glue\\mousedown2.wav";
        /// <summary>
        /// 
        /// </summary>
        public const string SwishInWav = "sound\\glue\\swishin.wav";
        /// <summary>
        /// 
        /// </summary>
        public const string SwishOutWav = "sound\\glue\\swishout.wav";

        /* credits */
        /// <summary>
        /// 
        /// </summary>
        public const string RezCreditExpTxt = "rez\\crdt_exp.txt";
        /// <summary>
        /// 
        /// </summary>
        public const string RezCreditListTxt = "rez\\crdt_lst.txt";

        /* music */
        /// <summary>
        /// 
        /// </summary>
        public const string MusicTitleWav = "music\\title.wav";

        /* game menus */
        /// <summary>
        /// 
        /// </summary>
        public const string GameMenuBin = "rez\\gamemenu.bin";
        /// <summary>
        /// 
        /// </summary>
        public const string OptionsBin = "rez\\options.bin";
        /// <summary>
        /// 
        /// </summary>
        public const string SoundDialogBin = "rez\\snd_dlg.bin";
        /// <summary>
        /// 
        /// </summary>
        public const string SpeedDialogBin = "rez\\spd_dlg.bin";
        /// <summary>
        /// 
        /// </summary>
        public const string VideoBin = "rez\\video.bin";
        /// <summary>
        /// 
        /// </summary>
        public const string NetDialogBin = "rez\\netdlg.bin";
        /// <summary>
        /// 
        /// </summary>
        public const string ObjectDialogBin = "rez\\objctdlg.bin";
        /// <summary>
        /// 
        /// </summary>
        public const string AbortMenuBin = "rez\\abrtmenu.bin";
        /// <summary>
        /// 
        /// </summary>
        public const string RestartBin = "rez\\restart.bin";
        /// <summary>
        /// 
        /// </summary>
        public const string QuitBin = "rez\\quit.bin";
        /// <summary>
        /// 
        /// </summary>
        public const string Quit2MenuBin = "rez\\quit2mnu.bin";
        /// <summary>
        /// 
        /// </summary>
        public const string HelpMenuBin = "rez\\helpmenu.bin";
        /// <summary>
        /// 
        /// </summary>
        public const string HelpBin = "rez\\help.bin";

        /// <summary>
        /// 
        /// </summary>
        public const string HelpTxtTbl = "rez\\help_txt.tbl";

        /// <summary>
        /// 
        /// </summary>
        public const string MiniMapBin = "rez\\minimap.bin";
    }
}
