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
    [Flags]
    public enum ElementFlags
    {
        /// <summary>
        /// 
        /// </summary>
        Unknown00000001 = 0x00000001,
        /// <summary>
        /// 
        /// </summary>
        Unknown00000002 = 0x00000002,
        /// <summary>
        /// 
        /// </summary>
        Unknown00000004 = 0x00000004,
        /// <summary>
        /// 
        /// </summary>
        Visible = 0x00000008,
        /// <summary>
        /// 
        /// </summary>
        RespondToMouse = 0x00000010,
        /// <summary>
        /// 
        /// </summary>
        Unknown00000020 = 0x00000020,
        /// <summary>
        /// 
        /// </summary>
        CancelButton = 0x00000040,
        /// <summary>
        /// 
        /// </summary>
        NoSoundOnMouseOvr = 0x00000080,
        /// <summary>
        /// 
        /// </summary>
        Unknown00000100 = 0x00000100,
        /// <summary>
        /// 
        /// </summary>
        HasHotkey = 0x00000200,
        /// <summary>
        /// 
        /// </summary>
        FontSmallest = 0x00000400,
        /// <summary>
        /// 
        /// </summary>
        FontSmaller = 0x00000800,
        /// <summary>
        /// 
        /// </summary>
        Unknown00001000 = 0x00001000,
        /// <summary>
        /// 
        /// </summary>
        Transparent = 0x00002000,
        /// <summary>
        /// 
        /// </summary>
        FontLargest = 0x00004000,
        /// <summary>
        /// 
        /// </summary>
        Unused00008000 = 0x00008000,
        /// <summary>
        /// 
        /// </summary>
        FontLarger = 0x00010000,
        /// <summary>
        /// 
        /// </summary>
        Unused00020000 = 0x00020000,
        /// <summary>
        /// 
        /// </summary>
        Translucent = 0x00040000,
        /// <summary>
        /// 
        /// </summary>
        DefaultButton = 0x00080000,
        /// <summary>
        /// 
        /// </summary>
        BringToFront = 0x00100000,
        /// <summary>
        /// 
        /// </summary>
        CenterTextHoriz = 0x00200000,
        /// <summary>
        /// 
        /// </summary>
        RightAlignText = 0x00400000,
        /// <summary>
        /// 
        /// </summary>
        CenterTextVert = 0x00800000,
        /// <summary>
        /// 
        /// </summary>
        Unused01000000 = 0x01000000,
        /// <summary>
        /// 
        /// </summary>
        Unused02000000 = 0x02000000,
        /// <summary>
        /// 
        /// </summary>
        NoClickSound = 0x04000000,
        /// <summary>
        /// 
        /// </summary>
        Unused08000000 = 0x08000000,
        /// <summary>
        /// 
        /// </summary>
        Unused10000000 = 0x10000000,
        /// <summary>
        /// 
        /// </summary>
        Unused20000000 = 0x20000000,
        /// <summary>
        /// 
        /// </summary>
        Unused40000000 = 0x40000000
    }

    /// <summary>
    /// 
    /// </summary>
    public enum ElementType
    {
        /// <summary>
        /// 
        /// </summary>
        DialogBox = 0,
        /// <summary>
        /// 
        /// </summary>
        DefaultButton = 1,
        /// <summary>
        /// 
        /// </summary>
        Button = 2,
        /// <summary>
        /// 
        /// </summary>
        OptionButton = 3,
        /// <summary>
        /// 
        /// </summary>
        CheckBox = 4,
        /// <summary>
        /// 
        /// </summary>
        Image = 5,
        /// <summary>
        /// 
        /// </summary>
        Slider = 6,
        /// <summary>
        /// 
        /// </summary>
        TextBox = 8,
        /// <summary>
        /// 
        /// </summary>
        LabelLeftAlign = 9,
        /// <summary>
        /// 
        /// </summary>
        LabelCenterAlign = 10,
        /// <summary>
        /// 
        /// </summary>
        LabelRightAlign = 11,
        /// <summary>
        /// 
        /// </summary>
        ListBox = 12,
        /// <summary>
        /// 
        /// </summary>
        ComboBox = 13,
        /// <summary>
        /// 
        /// </summary>
        ButtonWithoutBorder = 14
    }

    /// <summary>
    /// 
    /// </summary>
    public class BinElement
    {
        private ushort x1;

        /// <summary>
        /// 
        /// </summary>
        [CLSCompliant(false)]
        public ushort X1
        {
            get { return x1; }
            set { x1 = value; }
        }
        private ushort y1;

        /// <summary>
        /// 
        /// </summary>
        [CLSCompliant(false)]
        public ushort Y1
        {
            get { return y1; }
            set { y1 = value; }
        }
        private ushort x2;

        /// <summary>
        /// 
        /// </summary>
        [CLSCompliant(false)]
        public ushort X2
        {
            get { return x2; }
            set { x2 = value; }
        }
        private ushort y2;

        /// <summary>
        /// 
        /// </summary>
        [CLSCompliant(false)]
        public ushort Y2
        {
            get { return y2; }
            set { y2 = value; }
        }

        private ushort width;

        /// <summary>
        /// 
        /// </summary>
        [CLSCompliant(false)]
        public ushort Width
        {
            get { return width; }
            set { width = value; }
        }
        private ushort height;

        /// <summary>
        /// 
        /// </summary>
        [CLSCompliant(false)]
        public ushort Height
        {
            get { return height; }
            set { height = value; }
        }

        private byte hotkey;

        /// <summary>
        /// 
        /// </summary>
        public byte Hotkey
        {
            get { return hotkey; }
            set { hotkey = value; }
        }
        private string text;

        /// <summary>
        /// 
        /// </summary>
        public string Text1
        {
            get { return text; }
            set { text = value; }
        }
        private uint textOffset;

        /// <summary>
        /// 
        /// </summary>
        [CLSCompliant(false)]
        public uint TextOffset
        {
            get { return textOffset; }
            set { textOffset = value; }
        }

        private ElementFlags flags;

        /// <summary>
        /// 
        /// </summary>
        public ElementFlags Flags
        {
            get { return flags; }
            set { flags = value; }
        }
        private ElementType type;

        /// <summary>
        /// 
        /// </summary>
        public ElementType Type
        {
            get { return type; }
            set { type = value; }
        }

        private object resolvedData;

        /// <summary>
        /// 
        /// </summary>
        public object ResolvedData
        {
            get { return resolvedData; }
            set { resolvedData = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buf"></param>
        /// <param name="position"></param>
        /// <param name="streamLength"></param>
        [CLSCompliant(false)]
        public BinElement(byte[] buf, int position, uint streamLength)
        {
            x1 = Utilities.ReadWord(buf, position + 4);
            y1 = Utilities.ReadWord(buf, position + 6);
            x2 = Utilities.ReadWord(buf, position + 8);
            y2 = Utilities.ReadWord(buf, position + 10);
            width = Utilities.ReadWord(buf, position + 12);
            height = Utilities.ReadWord(buf, position + 14);
            textOffset = Utilities.ReadDWord(buf, position + 20);

            flags = (ElementFlags)Utilities.ReadDWord(buf, position + 24);
            type = (ElementType)buf[position + 34];

            if (textOffset < streamLength)
            {
                uint textLength = 0;
                while (buf[textOffset + textLength] != 0)
                {
                    textLength++;
                }

                text = Encoding.ASCII.GetString(buf, (int)textOffset, (int)textLength);

                if ((flags & ElementFlags.HasHotkey) == ElementFlags.HasHotkey)
                {
                    hotkey = Encoding.ASCII.GetBytes(new char[] { text[0] })[0];
                    text = text.Substring(1);
                }
            }
            else
            {
                text = "";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void DumpFlags()
        {
            Console.Write("Flags: ");
            foreach (ElementFlags f in Enum.GetValues(typeof(ElementFlags)))
            {
                if ((flags & f) == f)
                {
                    Console.Write(f);
                    Console.Write(" ");
                }
            }
            Console.WriteLine();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return String.Format("{0} ({1})", type, text);
        }

        /// <summary>
        /// 
        /// </summary>
        public string Text
        {
            get { return text; }
            set
            {
                text = value;
                resolvedData = null;
            }
        }
    }
}
