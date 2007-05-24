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
using System.Collections.ObjectModel;

using SCSharp.UI;

namespace SCSharp.MpqLib
{
    /// <summary>
    /// 
    /// </summary>
    public enum TileSet
    {
        /// <summary>
        /// 
        /// </summary>
        Badlands = 0,
        /// <summary>
        /// 
        /// </summary>
        Platform = 1,
        /// <summary>
        /// 
        /// </summary>
        Installation = 2,
        /// <summary>
        /// 
        /// </summary>
        Ashworld = 3,
        /// <summary>
        /// 
        /// </summary>
        Jungle = 4,
        /// <summary>
        /// 
        /// </summary>
        Desert = 5,
        /// <summary>
        /// 
        /// </summary>
        Ice = 6,
        /// <summary>
        /// 
        /// </summary>
        Twilight = 7
    }

    /// <summary>
    /// 
    /// </summary>
    public class Chk : IMpqResource
    {
        /// <summary>
        /// 
        /// </summary>
        public Chk()
        {
        }

        const string DIM = "DIM ";
        const string MTXM = "MTXM";

        class SectionData
        {
            private long dataPosition;

            /// <summary>
            /// 
            /// </summary>
            public long DataPosition
            {
                get { return dataPosition; }
                set { dataPosition = value; }
            }
            private uint dataLength;

            /// <summary>
            /// 
            /// </summary>
            public uint DataLength
            {
                get { return dataLength; }
                set { dataLength = value; }
            }
            private byte[] buffer;

            /// <summary>
            /// 
            /// </summary>
            public byte[] Buffer
            {
                get { return buffer; }
                set { buffer = value; }
            }
        }

        Stream stream;
        Dictionary<string, SectionData> sections;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        public void ReadFromStream(Stream stream)
        {
            this.stream = stream;

            //long stream_length = stream.Length;
            byte[] sectionNameBuf = new byte[4];
            string sectionName;

            sections = new Dictionary<string, SectionData>();

            while (true)
            {
                stream.Read(sectionNameBuf, 0, 4);

                SectionData sec_data = new SectionData();

                sec_data.DataLength = Utilities.ReadDWord(stream);
                sec_data.DataPosition = stream.Position;

                sectionName = Encoding.ASCII.GetString(sectionNameBuf, 0, 4);

                sections.Add(sectionName, sec_data);

                if (stream.Position + sec_data.DataLength >= stream.Length)
                {
                    break;
                }

                stream.Position += sec_data.DataLength;
            }

            /* parse only the sections we really care
             * about up front.  the rest are done as
             * needed.  this speeds up the Play Custom
             * screen immensely. */

            /* find out what version we're dealing with */
            ParseSection("VER ");
            if (sections.ContainsKey("IVER"))
            {
                ParseSection("IVER");
            }
            if (sections.ContainsKey("IVE2"))
            {
                ParseSection("IVE2");
            }

            /* strings */
            ParseSection("STR ");

            /* load in the map info */

            /* map name/description */
            ParseSection("SPRP");

            /* dimension */
            ParseSection("DIM ");

            /* tileset info */
            ParseSection("ERA ");

            /* player information */
            ParseSection("OWNR");
            ParseSection("SIDE");
        }

        void ParseSection(string sectionName)
        {
            SectionData sec = sections[sectionName];
            if (sec == null)
            {
                throw new SCException(String.Format("map file is missing section {0}, cannot load", sectionName));
            }

            if (sec.Buffer == null)
            {
                stream.Position = sec.DataPosition;
                sec.Buffer = new byte[sec.DataLength];
                stream.Read(sec.Buffer, 0, (int)sec.DataLength);
            }

            byte[] sectionData = sec.Buffer;

            if (sectionName == "TYPE")
            {
                scenarioType = Utilities.ReadWord(sectionData, 0);
            }
            else if (sectionName == "ERA ")
            {
                tileSet = (TileSet)Utilities.ReadWord(sectionData, 0);
            }
            else if (sectionName == "DIM ")
            {
                width = Utilities.ReadWord(sectionData, 0);
                height = Utilities.ReadWord(sectionData, 2);
            }
            else if (sectionName == "MTXM")
            {
                mapTiles = new ushort[width, height];
                int y, x;
                for (y = 0; y < height; y++)
                {
                    for (x = 0; x < width; x++)
                    {
                        mapTiles[x, y] = Utilities.ReadWord(sectionData, (y * width + x) * 2);
                    }
                }
            }
            else if (sectionName == "MASK")
            {
                mapMask = new byte[width, height];
                int y, x;
                int i = 0;

                for (y = 0; y < height; y++)
                {
                    for (x = 0; x < width; x++)
                    {
                        mapMask[x, y] = sectionData[i++];
                    }
                }
            }
            else if (sectionName == "SPRP")
            {
                int nameStringIndex = Utilities.ReadWord(sectionData, 0);
                int descriptionStringIndex = Utilities.ReadWord(sectionData, 2);

                Console.WriteLine("mapName = {0}", nameStringIndex);
                Console.WriteLine("mapDescription = {0}", descriptionStringIndex);
                mapName = GetMapString(nameStringIndex);
                mapDescription = GetMapString(descriptionStringIndex);
            }
            else if (sectionName == "STR ")
            {
                ReadStrings(sectionData);
            }
            else if (sectionName == "OWNR")
            {
                numberOfPlayers = 0;
                for (int i = 0; i < 12; i++)
                {
                    /* 
                       00 - Unused
                       03 - Rescuable
                       05 - Computer
                       06 - Human
                       07 - Neutral
                    */
                    if (sectionData[i] == 0x05)
                    {
                        numberOfComputerSlots++;
                    }
                    else if (sectionData[i] == 0x06)
                    {
                        numberOfHumanSlots++;
                    }
                }
            }
            else if (sectionName == "SIDE")
            {
                /*
                  00 - Zerg
                  01 - Terran
                  02 - Protoss
                  03 - Independent
                  04 - Neutral
                  05 - User Select
                  07 - Inactive
                  10 - Human
                */
                numberOfPlayers = 0;
                for (int i = 0; i < 12; i++)
                {
                    if (sectionData[i] == 0x05) /* user select */
                    {
                        numberOfPlayers++;
                    }
                }
            }
            else if (sectionName == "UNIT")
            {
                ReadUnits(sectionData);
            }
            else if (sectionName == "MBRF")
            {
                briefingData = new TriggerData();
                briefingData.Parse(sectionData);
            }
            //else
            //Console.WriteLine ("Unhandled Chk section type {0}, length {1}", section_name, section_data.Length);
        }

        Collection<UnitInfo> units;
        void ReadUnits(byte[] data)
        {
            units = new Collection<UnitInfo>();

            MemoryStream stream = new MemoryStream(data);
            Console.WriteLine("unit section data = {0} bytes long", data.Length);

            int i = 0;
            while (i <= data.Length / 36)
            {
                /*uint serial =*/
                Utilities.ReadDWord(stream);
                ushort x = Utilities.ReadWord(stream);
                ushort y = Utilities.ReadWord(stream);
                ushort type = Utilities.ReadWord(stream);
                Utilities.ReadWord(stream);
                Utilities.ReadWord(stream);
                Utilities.ReadWord(stream);
                byte player = Utilities.ReadByte(stream);
                Utilities.ReadByte(stream);
                Utilities.ReadByte(stream);
                Utilities.ReadByte(stream);
                Utilities.ReadDWord(stream);
                Utilities.ReadWord(stream);
                Utilities.ReadWord(stream);

                Utilities.ReadByte(stream);
                Utilities.ReadByte(stream);
                Utilities.ReadByte(stream);
                Utilities.ReadByte(stream);
                Utilities.ReadByte(stream);
                Utilities.ReadByte(stream);
                Utilities.ReadByte(stream);
                Utilities.ReadByte(stream);
                i++;

                UnitInfo info = new UnitInfo();
                info.UnitId = type;
                info.PositionX = x;
                info.PositionY = y;
                info.Player = player;

                units.Add(info);
            }
        }

        void ReadStrings(byte[] data)
        {
            MemoryStream stream = new MemoryStream(data);

            int i;

            int num_strings = Utilities.ReadWord(stream);

            int[] offsets = new int[num_strings];

            for (i = 0; i < num_strings; i++)
            {
                offsets[i] = Utilities.ReadWord(stream);
            }

            StreamReader tr = new StreamReader(stream);
            strings = new string[num_strings];

            for (i = 0; i < num_strings; i++)
            {
                if (tr.BaseStream.Position != offsets[i])
                {
                    tr.BaseStream.Seek(offsets[i], SeekOrigin.Begin);
                    tr.DiscardBufferedData();
                }

                strings[i] = Utilities.ReadUntilNull(tr);
            }
        }

        string[] strings;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public string GetMapString(int index)
        {
            if (index == 0)
            {
                return "";
            }
            return strings[index - 1];
        }

        string mapName;
        /// <summary>
        /// 
        /// </summary>
        public string Name
        {
            get { return mapName; }
        }

        string mapDescription;
        /// <summary>
        /// 
        /// </summary>
        public string Description
        {
            get { return mapDescription; }
        }

        ushort scenarioType;
        /// <summary>
        /// 
        /// </summary>
        [CLSCompliant(false)]
        public ushort ScenarioType
        {
            get { return scenarioType; }
        }

        TileSet tileSet;
        /// <summary>
        /// 
        /// </summary>
        public TileSet TileSet
        {
            get { return tileSet; }
        }

        ushort width;
        /// <summary>
        /// 
        /// </summary>
        [CLSCompliant(false)]
        public ushort Width
        {
            get { return width; }
        }

        ushort height;
        /// <summary>
        /// 
        /// </summary>
        [CLSCompliant(false)]
        public ushort Height
        {
            get { return height; }
        }

        ushort[,] mapTiles;
        /// <summary>
        /// 
        /// </summary>
        [CLSCompliant(false)]
        public ushort[,] MapTiles
        {
            get
            {
                if (mapTiles == null)
                {
                    ParseSection("MTXM");

                    /* XXX put these here for now.. */

                    /* tile info */
                    if (sections.ContainsKey("TILE"))
                    {
                        ParseSection("TILE");
                    }

                    /* isometric mapping */
                    if (sections.ContainsKey("ISOM"))
                    {
                        ParseSection("ISOM");
                    }
                }

                return mapTiles;
            }
        }

        byte[,] mapMask;
        /// <summary>
        /// 
        /// </summary>
        public byte[,] MapMask
        {
            get
            {
                if (mapMask == null)
                {
                    ParseSection("MASK");
                }

                return mapMask;
            }
        }

        int numberOfComputerSlots;
        /// <summary>
        /// 
        /// </summary>
        public int NumberOfComputerSlots
        {
            get { return numberOfComputerSlots; }
        }

        int numberOfHumanSlots;
        /// <summary>
        /// 
        /// </summary>
        public int NumberOfHumanSlots
        {
            get { return numberOfHumanSlots; }
        }

        int numberOfPlayers;
        /// <summary>
        /// 
        /// </summary>
        public int NumberOfPlayers
        {
            get { return numberOfPlayers; }
        }

        TriggerData briefingData;
        /// <summary>
        /// 
        /// </summary>
        public TriggerData BriefingData
        {
            get
            {
                if (briefingData == null)
                {
                    ParseSection("MBRF");
                }
                return briefingData;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Collection<UnitInfo> Units
        {
            get
            {
                if (units == null)
                {
                    /* initial units on the map */
                    ParseSection("UNIT");
                }

                return units;
            }
        }
    }
}
