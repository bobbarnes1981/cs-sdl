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
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;

using SdlDotNet.Input;
using SCSharp;
using SCSharp.MpqLib;

namespace SCSharp.UI
{
    /// <summary>
    /// 
    /// </summary>
    public class PlayCustomScreen : UIScreen
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mpq"></param>
        public PlayCustomScreen(Mpq mpq)
            : base(mpq, "glue\\PalNl", BuiltIns.GluCustomBin)
        {
        }

        const int MAPSIZE_FORMAT_INDEX = 32;
        const int MAPDIM_FORMAT_INDEX = 31; // XXX we don't use this one yet..
        const int TILESET_FORMAT_INDEX = 33;
        const int NUMPLAYERS_FORMAT_INDEX = 30;
        const int HUMANSLOT_FORMAT_INDEX = 37;
        const int COMPUTERSLOT_FORMAT_INDEX = 35;

        const int FILELISTBOX_ELEMENT_INDEX = 7;
        const int CURRENTDIR_ELEMENT_INDEX = 8;
        const int MAPTITLE_ELEMENT_INDEX = 9;
        const int MAPDESCRIPTION_ELEMENT_INDEX = 10;
        const int MAPSIZE_ELEMENT_INDEX = 11;
        const int MAPTILESET_ELEMENT_INDEX = 12;
        const int MAPPLAYERS1_ELEMENT_INDEX = 14;
        const int MAPPLAYERS2_ELEMENT_INDEX = 15;

        const int OK_ELEMENT_INDEX = 16;
        const int CANCEL_ELEMENT_INDEX = 17;

        const int GAMETYPECOMBO_ELEMENT_INDEX = 20;

        const int GAMESUBTYPE_LABEL_ELEMENT_INDEX = 19;
        const int GAMESUBTYPE_COMBO_ELEMENT_INDEX = 21;

        const int PLAYER1_COMBOBOX_PLAYER = 22;
        const int PLAYER1_COMBOBOX_RACE = 30;

        const int max_players = 8;

        string mapdir;
        string curdir;

        Mpq selectedScenario;
        Chk selectedChk;
        Got selectedGot;

        ListBoxElement fileListbox;
        ComboBoxElement gametypeCombo;

        static void InitializeRaceCombo(ComboBoxElement combo)
        {
            combo.AddItem("Zerg"); /* XXX these should all come from some string constant table someplace */
            combo.AddItem("Terran");
            combo.AddItem("Protoss");
            combo.AddItem("Random", true);
        }

        static void InitializePlayerCombo(ComboBoxElement combo)
        {
            combo.AddItem(GlobalResources.GluAllTbl.Strings[130]); /* Closed */
            combo.AddItem(GlobalResources.GluAllTbl.Strings[128], true); /* Computer */
        }

        string[] files;
        string[] directories;
        Got[] templates;

        void PopulateFileList()
        {
            fileListbox.Clear();

            string[] dir = Directory.GetDirectories(curdir);
            List<string> dirs = new List<string>();
            if (curdir != mapdir)
            {
                dirs.Add("Up One Level");
            }
            foreach (string d in dir)
            {
                string dl = Path.GetFileName(d).ToLower(CultureInfo.CurrentCulture);

                if (curdir == mapdir)
                {
                    if (!Game.Instance.IsBroodWar
                        && dl == "broodwar")
                    {
                        continue;
                    }

                    if (dl == "replays")
                    {
                        continue;
                    }
                }

                dirs.Add(d);
            }

            directories = dirs.ToArray();

            files = Directory.GetFiles(curdir, "*.sc*");

            Elements[CURRENTDIR_ELEMENT_INDEX].Text = Path.GetFileName(curdir);

            for (int i = 0; i < directories.Length; i++)
            {
                fileListbox.AddItem(String.Format(CultureInfo.CurrentCulture, "[{0}]", Path.GetFileName(directories[i])));
            }

            for (int i = 0; i < files.Length; i++)
            {
                string lower = files[i].ToLower(CultureInfo.CurrentCulture);
                if (lower.EndsWith(".scm") || lower.EndsWith(".scx"))
                {
                    fileListbox.AddItem(Path.GetFileName(files[i]));
                }
            }

            fileListbox.SelectedIndex = directories.Length;
            FileListSelectionChanged(this, new BoxSelectionChangedEventArgs(directories.Length));
        }

        void PopulateGameTypes()
        {
            /* load the templates we're interested in displaying */
            StreamReader sr = new StreamReader((Stream)this.Mpq.GetResource("templates\\templates.lst"));
            List<Got> templateList = new List<Got>();
            string l;

            while ((l = sr.ReadLine()) != null)
            {
                string t = l.Replace("\"", "");

                Got got = (Got)this.Mpq.GetResource("templates\\" + t);

                if (got.ComputerPlayersAllowed && got.NumberOfTeams == 0)
                {
                    Console.WriteLine("adding template {0}:{1}", got.UIGameTypeName, got.UISubtypeLabel);
                    templateList.Add(got);
                }
            }

            templates = new Got[templateList.Count];
            templateList.CopyTo(templates, 0);

            Array.Sort(templates, delegate(Got g1, Got g2) { return g1.ListPosition - g2.ListPosition; });

            /* fill in the game type menu.
               we only show the templates that allow computer players, have 0 teams */
            foreach (Got got in templates)
            {
                gametypeCombo.AddItem(got.UIGameTypeName);
            }
            gametypeCombo.SelectedIndex = 0;

            GameTypeSelectionChanged(this, new BoxSelectionChangedEventArgs(gametypeCombo.SelectedIndex));
        }

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

            /* these don't ever show up in the UI, that i know of... */
            Elements[GAMESUBTYPE_LABEL_ELEMENT_INDEX].Visible = false;
            Elements[GAMESUBTYPE_COMBO_ELEMENT_INDEX].Visible = false;

            /* initialize all the race combo boxes */
            for (int i = 0; i < max_players; i++)
            {
                InitializePlayerCombo((ComboBoxElement)Elements[PLAYER1_COMBOBOX_PLAYER + i]);
                InitializeRaceCombo((ComboBoxElement)Elements[PLAYER1_COMBOBOX_RACE + i]);
            }

            fileListbox = (ListBoxElement)Elements[FILELISTBOX_ELEMENT_INDEX];
            gametypeCombo = (ComboBoxElement)Elements[GAMETYPECOMBO_ELEMENT_INDEX];

            /* initially populate the map list by scanning the maps/ directory in the starcraftdir */
            mapdir = Path.Combine(Game.Instance.RootDirectory, "maps");
            curdir = mapdir;

            PopulateGameTypes();
            PopulateFileList();

            fileListbox.SelectionChanged += FileListSelectionChanged;
            gametypeCombo.SelectionChanged += GameTypeSelectionChanged;

            Elements[OK_ELEMENT_INDEX].Activate +=
                delegate(object sender, SCEventArgs args)
                {
                    if (selectedScenario == null)
                    {
                        // the selected entry is a directory, switch to it
                        if (curdir != mapdir)
                        {
                            if (fileListbox.SelectedIndex == 0)
                            {
                                curdir = Directory.GetParent(curdir).FullName;
                            }
                            else
                            {
                                curdir = directories[fileListbox.SelectedIndex - 1];
                            }
                        }
                        else
                        {
                            curdir = directories[fileListbox.SelectedIndex];
                        }

                        PopulateFileList();
                    }
                    else
                    {
                        Game.Instance.SwitchToScreen(new GameScreen(this.Mpq,
                                                  selectedScenario,
                                                  selectedChk,
                                                  selectedGot));
                    }
                };

            Elements[CANCEL_ELEMENT_INDEX].Activate +=
                delegate(object sender, SCEventArgs args)
                {
                    Game.Instance.SwitchToScreen(new RaceSelectionScreen(this.Mpq));
                };


            /* make sure the PLAYER1 player combo reads
             * the player's name and is desensitized */
            ((ComboBoxElement)Elements[PLAYER1_COMBOBOX_PLAYER]).AddItem(/*XXX player name*/"toshok");
            Elements[PLAYER1_COMBOBOX_PLAYER].Sensitive = false;
        }

        void UpdatePlayersDisplay()
        {
            if (selectedGot.UseMapSettings)
            {
                string slotString;

                slotString = GlobalResources.GluAllTbl.Strings[HUMANSLOT_FORMAT_INDEX];
                slotString = slotString.Replace("%c", " "); /* should probably be a tab.. */
                slotString = slotString.Replace("%s",
                                 (selectedChk == null
                                  ? ""
                                  : String.Format(CultureInfo.CurrentCulture, "{0}",
                                           selectedChk.NumberOfHumanSlots)));

                Elements[MAPPLAYERS1_ELEMENT_INDEX].Text = slotString;
                Elements[MAPPLAYERS1_ELEMENT_INDEX].Visible = true;

                slotString = GlobalResources.GluAllTbl.Strings[COMPUTERSLOT_FORMAT_INDEX];
                slotString = slotString.Replace("%c", " "); /* should probably be a tab.. */
                slotString = slotString.Replace("%s",
                                 (selectedChk == null
                                  ? ""
                                  : String.Format(CultureInfo.CurrentCulture, "{0}",
                                           selectedChk.NumberOfComputerSlots)));

                Elements[MAPPLAYERS2_ELEMENT_INDEX].Text = slotString;
                Elements[MAPPLAYERS2_ELEMENT_INDEX].Visible = true;
            }
            else
            {
                string numPlayersString = GlobalResources.GluAllTbl.Strings[NUMPLAYERS_FORMAT_INDEX];

                numPlayersString = numPlayersString.Replace("%c", " "); /* should probably be a tab.. */
                numPlayersString = numPlayersString.Replace("%s",
                                         (selectedChk == null
                                          ? ""
                                          : String.Format(CultureInfo.CurrentCulture, "{0}",
                                                   selectedChk.NumberOfPlayers)));

                Elements[MAPPLAYERS1_ELEMENT_INDEX].Text = numPlayersString;
                Elements[MAPPLAYERS1_ELEMENT_INDEX].Visible = true;
                Elements[MAPPLAYERS2_ELEMENT_INDEX].Visible = false;
            }

            int i = 0;
            if (selectedChk != null)
            {
                for (i = 0; i < max_players; i++)
                {
                    if (selectedGot.UseMapSettings)
                    {
                        if (i >= selectedChk.NumberOfComputerSlots + 1) break;
                    }
                    else
                    {
                        if (i >= selectedChk.NumberOfPlayers) break;
                    }

                    if (i > 0)
                        ((ComboBoxElement)Elements[PLAYER1_COMBOBOX_PLAYER + i]).SelectedIndex = 1;
                    ((ComboBoxElement)Elements[PLAYER1_COMBOBOX_RACE + i]).SelectedIndex = 3;
                    Elements[PLAYER1_COMBOBOX_PLAYER + i].Visible = true;
                    Elements[PLAYER1_COMBOBOX_RACE + i].Visible = true;
                }
            }
            for (int j = i; j < max_players; j++)
            {
                Elements[PLAYER1_COMBOBOX_PLAYER + j].Visible = false;
                Elements[PLAYER1_COMBOBOX_RACE + j].Visible = false;
            }
        }

        void GameTypeSelectionChanged(object sender, BoxSelectionChangedEventArgs e)//int selectedIndex)
        {
            /* the display of the number of players
             * changes depending upon the template */
            selectedGot = templates[e.SelectedIndex];

            UpdatePlayersDisplay();
        }

        void FileListSelectionChanged(object sender, BoxSelectionChangedEventArgs e)
        {
            string mapPath = Path.Combine(curdir, fileListbox.SelectedItem);

            if (e.SelectedIndex < directories.Length)
            {
                selectedScenario = null;
                selectedChk = null;
            }
            else
            {
                selectedScenario = new MpqArchiveContainer(mapPath);

                selectedChk = (Chk)selectedScenario.GetResource("staredit\\scenario.chk");
            }

            Elements[MAPTITLE_ELEMENT_INDEX].Text = selectedChk == null ? "" : selectedChk.Name;
            Elements[MAPDESCRIPTION_ELEMENT_INDEX].Text = selectedChk == null ? "" : selectedChk.Description;

            string mapSizeString = GlobalResources.GluAllTbl.Strings[MAPSIZE_FORMAT_INDEX];
            //			string mapDimString = GlobalResources.Instance.GluAllTbl.Strings[MAPDIM_FORMAT_INDEX];
            string tileSetString = GlobalResources.GluAllTbl.Strings[TILESET_FORMAT_INDEX];

            mapSizeString = mapSizeString.Replace("%c", " "); /* should probably be a tab.. */
            mapSizeString = mapSizeString.Replace("%s",
                                   (selectedChk == null
                                ? ""
                                : String.Format(CultureInfo.CurrentCulture, "{0}x{1}",
                                         selectedChk.Width,
                                         selectedChk.Height)));

            tileSetString = tileSetString.Replace("%c", " "); /* should probably be a tab.. */
            tileSetString = tileSetString.Replace("%s",
                                   (selectedChk == null
                                ? ""
                                : String.Format(CultureInfo.CurrentCulture, "{0}",
                                         selectedChk.TileSet)));

            Elements[MAPSIZE_ELEMENT_INDEX].Text = mapSizeString;
            Elements[MAPTILESET_ELEMENT_INDEX].Text = tileSetString;

            UpdatePlayersDisplay();
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
            if (args.Key == Key.DownArrow
                || args.Key == Key.UpArrow)
            {
                fileListbox.KeyboardDown(args);
            }
            else
            {
                base.KeyboardDown(args);
            }
        }
    }
}
