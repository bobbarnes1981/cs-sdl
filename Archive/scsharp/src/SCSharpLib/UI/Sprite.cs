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

/* opcode descriptions and info here
http://www.campaigncreations.org/starcraft/bible/chap4_ice_opcodes.shtml
http://www.stormcoast-fortress.net/cntt/tutorials/camsys/tilesetdependent/?PHPSESSID=7365d884cf33fc614c0b96d966872177

*/

using System;
using System.Diagnostics;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Collections.Generic;

using SCSharp;
using SCSharp.MpqLib;
using SdlDotNet.Graphics;
using SdlDotNet.Graphics.Primitives;

namespace SCSharp.UI
{
    /// <summary>
    ///
    /// </summary>
    public class Sprite
    {
        static Random rng = new Random(Environment.TickCount);
        //static bool showSpriteBorders = ShowSpriteBorders();
        ushort imagesEntry;

        string grpPath;
        Grp grp;
        byte[] palette;

        byte[] buf;
        ushort scriptStart; /* the pc of the script that started us off */
        ushort pc;
        ushort iscriptEntry;
        ushort scriptEntryOffset;

        int currentFrame = -1;
        int facing;

        bool trace;

        static bool showSpriteBorders;

        Mpq mpq;

        //int x;
        //int y;

        Sprite parentSprite;

        static Sprite()
        {
            ShowSpriteBorders();
        }

        [Conditional("DEBUG")]
        private static void ShowSpriteBorders()
        {
            string sb = ConfigurationManager.AppSettings["ShowSpriteBorders"];
            if (sb != null)
            {
                showSpriteBorders = Boolean.Parse(sb);
            }
            //return true;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="mpq"></param>
        /// <param name="spriteEntry"></param>
        /// <param name="palette"></param>
        /// <param name="positionX"></param>
        /// <param name="positionY"></param>
        public Sprite(Mpq mpq, int spriteEntry, byte[] palette, int positionX, int positionY)
        {
            if (mpq == null)
            {
                throw new ArgumentNullException("mpq");
            }
            this.mpq = mpq;
            this.palette = palette;

            imagesEntry = GlobalResources.SpritesDat.GetImagesDatEntry(spriteEntry);
            // Console.WriteLine ("image_dat_entry == {0}", images_entry);

            ushort grp_index = GlobalResources.ImagesDat.GetGrpIndex(imagesEntry);
            // Console.WriteLine ("grp_index = {0}", grp_index);
            grpPath = GlobalResources.ImagesTbl[grp_index - 1];
            // Console.WriteLine ("grp_path = {0}", grp_path);

            grp = (Grp)mpq.GetResource("unit\\" + grpPath);

            Console.WriteLine("new sprite: unit\\{0} @ {1}x{2} (image {3})", grpPath, positionX, positionY, imagesEntry);

            this.buf = GlobalResources.IScriptBin.Contents;
            iscriptEntry = GlobalResources.ImagesDat.GetIScriptIndex(imagesEntry);

            scriptEntryOffset = GlobalResources.IScriptBin.GetScriptEntryOffset(iscriptEntry);
            /* make sure the offset points to "SCEP" */
            if (Utilities.ReadDWord(buf, scriptEntryOffset) != 0x45504353)
            {
                Console.WriteLine("invalid script_entry_offset");
            }

            Position = new Point(positionX, positionY);
            //SetPosition(x, y);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="parentSprite"></param>
        /// <param name="imagesEntry"></param>
        /// <param name="palette"></param>
        [CLSCompliant(false)]
        public Sprite(Sprite parentSprite, ushort imagesEntry, byte[] palette)
        {
            if (parentSprite == null)
            {
                throw new ArgumentNullException("parentSprite");
            }
            this.parentSprite = parentSprite;
            this.mpq = parentSprite.mpq;
            this.palette = palette;
            this.imagesEntry = imagesEntry;

            ushort grpIndex = GlobalResources.ImagesDat.GetGrpIndex(imagesEntry);

            grpPath = GlobalResources.ImagesTbl[grpIndex - 1];

            grp = (Grp)mpq.GetResource("unit\\" + grpPath);

            this.buf = GlobalResources.IScriptBin.Contents;
            iscriptEntry = GlobalResources.ImagesDat.GetIScriptIndex(imagesEntry);

            scriptEntryOffset = GlobalResources.IScriptBin.GetScriptEntryOffset(iscriptEntry);
            /* make sure the offset points to "SCEP" */
            if (Utilities.ReadDWord(buf, scriptEntryOffset) != 0x45504353)
            {
                Console.WriteLine("invalid script_entry_offset");
            }

            //int x, y;

            //parentSprite.GetPosition(out x, out y);
            this.Position = parentSprite.Position;
            //SetPosition(x, y);
        }

        /* IScript opcodes */

        const byte PlayFrame = 0x00;
        const byte PlayTilesetFrame = 0x01;
        /* 0x02 = unknown */
        const byte ShiftGraphicVert = 0x03;
        /* 0x04 = unknown */
        const byte Wait = 0x05;
        const byte Wait2Rand = 0x06;
        const byte Goto = 0x07;
        const byte PlaceActiveOverlay = 0x08;
        const byte PlaceActiveUnderlay = 0x09;
        /* 0x0a = unknown */
        const byte SwitchUnderlay = 0x0b;
        /* 0x0c = unknown */
        const byte PlaceOverlay = 0x0d;
        /* 0x0e = unknown */
        const byte PlaceIndependentOverlay = 0x0f;
        const byte PlaceIndependentOverlayOnTop = 0x10;
        const byte PlaceIndependentUnderlay = 0x11;
        /* 0x12 = unknown */
        const byte DisplayOverlayWithLO = 0x13;
        /* 0x14 = unknown */
        const byte DisplayIndependentOverlayWithLO = 0x15;
        const byte EndAnimation = 0x16;
        /* 0x17 = unknown */
        const byte PlaySound = 0x18;
        const byte PlayRandomSound = 0x19;
        const byte PlayRandomSoundRange = 0x1a;
        const byte DoDamage = 0x1b;
        const byte AttackWithWeaponAndPlaySound = 0x1c;
        const byte FollowFrameChange = 0x1d;
        const byte RandomizerValueGoto = 0x1e;
        const byte TurnCCW = 0x1f;
        const byte TurnCW = 0x20;
        const byte Turn1CW = 0x21;
        const byte TurnRandom = 0x22;
        /* 0x23 = unknown */
        const byte Attack = 0x25;
        const byte AttackWithAppropriateWeapon = 0x26;
        const byte CastSpell = 0x27;
        const byte UseWeapon = 0x28;
        const byte MoveForward = 0x29;
        const byte AttackLoopMarker = 0x2a;
        /* 0x2b = unknown */
        /* 0x2c = unknown */
        /* 0x2d = unknown */
        const byte BeginPlayerLockout = 0x2e;
        const byte EndPlayerLockout = 0x2f;
        const byte IgnoreOtherOpcodes = 0x30;
        const byte AttackWithDirectionalProjectile = 0x31;
        const byte Hide = 0x32;
        const byte Unhide = 0x33;
        const byte PlaySpecificFrame = 0x34;
        /* 0x35 = unknown */
        /* 0x36 = unknown */
        /* 0x37 = unknown */
        const byte Unknown38 = 0x38;
        const byte IfPickedUp = 0x39;
        const byte IfTargetInRangeGoto = 0x3a;
        const byte IfTargetInArcGoto = 0x3b;
        const byte Unknown3c = 0x3c;
        const byte Unknown3d = 0x3d;
        /* 0x3e = unknown */
        const byte Unknown3f = 0x3f;
        const byte Unknown40 = 0x40;
        const byte Unknown41 = 0x41;
        const byte Unknown42 = 0x42; /* ICE manual says this is something dealing with sprites */

        void Trace(string fmt, params object[] args)
        {
            if (trace)
            {
                Console.Write(fmt, args);
            }
        }

        void TraceLine(string fmt, params object[] args)
        {
            if (trace)
            {
                Console.WriteLine(fmt, args);
            }
        }

        /// <summary>
        ///
        /// </summary>
        public bool Debug
        {
            get { return trace; }
            set { trace = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public Mpq Mpq
        {
            get { return mpq; }
        }

        /// <summary>
        ///
        /// </summary>
        public int CurrentFrame
        {
            get { return currentFrame; }
        }

        Point position = new Point();

        /// <summary>
        /// 
        /// </summary>
        public Point Position
        {
            get { return position; }
            set { position = value; }
        }

        ///// <summary>
        /////
        ///// </summary>
        ///// <param name="x"></param>
        ///// <param name="y"></param>
        //public void SetPosition(int x, int y)
        //{
        //    this.x = x;
        //    this.y = y;
        //}

        ///// <summary>
        /////
        ///// </summary>
        ///// <param name="xo"></param>
        ///// <param name="yo"></param>
        //public void GetPosition(out int xo, out int yo)
        //{
        //    xo = this.x;
        //    yo = this.y;
        //}

        ///// <summary>
        /////
        ///// </summary>
        ///// <param name="xo"></param>
        ///// <param name="yo"></param>
        //public void GetTopLeftPosition(out int xo, out int yo)
        //{
        //    xo = this.x;
        //    yo = this.y;

        //    if (spriteSurface != null)
        //    {
        //        xo -= spriteSurface.Width / 2;
        //        yo -= spriteSurface.Height / 2;
        //    }
        //}

        /// <summary>
        /// 
        /// </summary>
        public Point TopLeftPosition
        {
            get
            {
                Point point = new Point(position.X, position.Y);
                if (spriteSurface != null)
                {
                    point =  new Point(point.X - (spriteSurface.Width / 2), point.Y - (spriteSurface.Height / 2));
                }
                return point;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="scriptStart"></param>
        [CLSCompliant(false)]
        public void RunScript(ushort scriptStart)
        {
            this.scriptStart = scriptStart;
            pc = scriptStart;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="animationType"></param>
        public void RunScript(AnimationType animationType)
        {
            TraceLine("running script type = {0}", animationType);

            int offset_to_script_type = (4 /* "SCEP" */ + 1 /* the script entry "type" */ + 3 /* the spacers */ +
            (int)animationType * 2);

            scriptStart = Utilities.ReadWord(buf, scriptEntryOffset + offset_to_script_type);
            pc = scriptStart;
            TraceLine("pc = {0}", pc);
        }

        ushort ReadWord(ref ushort pc)
        {
            ushort retval = Utilities.ReadWord(buf, pc);
            pc += 2;
            return retval;
        }

        byte ReadByte(ref ushort pc)
        {
            byte retval = buf[pc];
            pc++;
            return retval;
        }

        //Painter painter;
        Surface spriteSurface;

        void PaintSprite(Surface surf, DateTime now)
        {
            if (spriteSurface != null)
            {
                if (this.position.X > SpriteManager.PositionX - spriteSurface.Width && this.position.X <= SpriteManager.PositionX + Painter.ScreenResX
                && this.position.Y > SpriteManager.PositionY - spriteSurface.Height && this.position.Y <= SpriteManager.PositionY + Painter.ScreenResY)
                {
                    surf.Blit(spriteSurface, new Point(this.position.X - SpriteManager.PositionX - spriteSurface.Width / 2,
                    this.position.Y - SpriteManager.PositionY - spriteSurface.Height / 2));

                    if (showSpriteBorders)
                    {
                        surf.Draw(new Box(new Point(this.position.X - SpriteManager.PositionX - spriteSurface.Width / 2,
                        this.position.Y - SpriteManager.PositionY - spriteSurface.Height / 2),
                        new Size(spriteSurface.Width - 1,
                        spriteSurface.Height - 1)),
                        Color.Green);
                    }
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="painter"></param>
        public void AddToPainter(Painter painter)
        {
            if (painter == null)
            {
                throw new ArgumentNullException("painter");
            }
            //this.painter = painter;
            painter.Add(Layer.Unit, PaintSprite);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="painter"></param>
        public void RemoveFromPainter(Painter painter)
        {
            if (painter == null)
            {
                throw new ArgumentNullException("painter");
            }
            painter.Add(Layer.Unit, PaintSprite);
            //this.painter = null;
        }

        void DoPlayFrame(Surface painter_surface, int frame_num)
        {
            if (currentFrame != frame_num)
            {
                currentFrame = frame_num;

                if (spriteSurface != null)
                {
                    spriteSurface.Dispose();
                }

                // XXX
                spriteSurface = GuiUtility.CreateSurfaceFromBitmap(grp.GetFrame(frame_num),
                grp.Width, grp.Height,
                palette,
                true);
            }
        }

        int waiting;

        /// <summary>
        ///
        /// </summary>
        /// <param name="surf"></param>
        /// <param name="now"></param>
        /// <returns></returns>
        public bool Tick(Surface surf, DateTime now)
        {
            ushort warg1;
            ushort warg2;
            // ushort warg3;
            byte barg1;
            byte barg2;
            // byte barg3;

            if (pc == 0)
            {
                return true;
            }

            if (waiting > 0)
            {
                waiting--;
                return true;
            }

            Trace("{0}: ", pc);
            switch (buf[pc++])
            {
                case PlayFrame:
                    warg1 = ReadWord(ref pc);
                    TraceLine("PlayFrame: {0}", warg1);
                    DoPlayFrame(surf, warg1 + facing % 16);
                    break;
                case PlayTilesetFrame:
                    warg1 = ReadWord(ref pc);
                    TraceLine("PlayTilesetFrame: {0}", warg1);
                    break;
                case ShiftGraphicVert:
                    barg1 = ReadByte(ref pc);
                    TraceLine("ShiftGraphicVert: {0}", barg1);
                    break;
                case Wait:
                    barg1 = ReadByte(ref pc);
                    TraceLine("Wait: {0}", barg1);
                    waiting = barg1;
                    break;
                case Wait2Rand:
                    barg1 = ReadByte(ref pc);
                    barg2 = ReadByte(ref pc);
                    TraceLine("Wait2: {0} {1}", barg1, barg2);
                    waiting = rng.Next(255) > 127 ? barg1 : barg2;
                    break;
                case Goto:
                    warg1 = ReadWord(ref pc);
                    TraceLine("Goto: {0}", warg1);
                    pc = warg1;
                    break;
                case PlaceActiveOverlay:
                    warg1 = ReadWord(ref pc);
                    warg2 = ReadWord(ref pc);
                    TraceLine("PlaceActiveOverlay: {0} {1}", warg1, warg2);
                    break;
                case PlaceActiveUnderlay:
                    warg1 = ReadWord(ref pc);
                    warg2 = ReadWord(ref pc);
                    TraceLine("PlaceActiveUnderlay: {0} {1}", warg1, warg2);
                    Sprite dependentSprite = SpriteManager.CreateSprite(this, warg1, palette);
                    dependentSprite.RunScript(AnimationType.Init);
                    break;
                case MoveForward:
                    barg1 = ReadByte(ref pc);
                    TraceLine("Move forward %1 units: {0}", barg1);
                    break;
                case RandomizerValueGoto:
                    barg1 = ReadByte(ref pc);
                    warg1 = ReadWord(ref pc);
                    TraceLine("Randomized (with test value) goto: {0} {1}", barg1, warg1);
                    int rand = rng.Next(255);
                    if (rand > barg1)
                    {
                        TraceLine("+ choosing goto branch");
                        pc = warg1;
                    }
                    break;
                case TurnRandom:
                    TraceLine("Turn graphic number of frames in random direction (CCW or CW)");
                    if (rng.Next(255) > 127)
                    {
                        goto case TurnCCW;
                    }
                    else
                    {
                        goto case TurnCW;
                    }
                case TurnCCW:
                    barg1 = ReadByte(ref pc);
                    TraceLine("Turn graphic number of frames CCW: {0}", barg1);
                    if (facing - barg1 < 0)
                    {
                        facing = 15 - barg1;
                    }
                    else
                    {
                        facing -= barg1;
                    }
                    break;
                case TurnCW:
                    barg1 = ReadByte(ref pc);
                    TraceLine("Turn graphic number of frames CW: {0}", barg1);
                    if (facing + barg1 > 15)
                    {
                        facing = facing + barg1 - 15;
                    }
                    else
                    {
                        facing += barg1;
                    }
                    break;
                case Turn1CW:
                    TraceLine("Turn graphic 1 frame clockwise");
                    break;
                case PlaySound:
                    warg1 = ReadWord(ref pc);
                    TraceLine("Play sound: {0} ({1})", warg1 - 1, GlobalResources.SfxDataTbl[GlobalResources.SfxDataDat.GetFileIndex((uint)(warg1 - 1))]);
                    break;
                case PlayRandomSoundRange:
                    warg1 = ReadWord(ref pc);
                    warg2 = ReadWord(ref pc);
                    TraceLine("Play random sound in range: {0}-{1}", warg1, warg2);
                    break;
                case PlaySpecificFrame:
                    barg1 = ReadByte(ref pc);
                    TraceLine("PlaySpecificFrame: {0}", barg1);
                    DoPlayFrame(surf, barg1);
                    break;
                case PlaceIndependentUnderlay:
                    warg1 = ReadWord(ref pc);
                    barg1 = ReadByte(ref pc);
                    barg2 = ReadByte(ref pc);
                    TraceLine("PlaceIndependentUnderlay: {0} ({1},{2})", warg1, barg1, barg2);
                    Sprite s = SpriteManager.CreateSprite(warg1, palette, this.position.X, this.position.Y);
                    s.RunScript(AnimationType.Init);
                    break;
                case EndAnimation:
                    return false;
                case Unknown38:
                    warg1 = ReadWord(ref pc);
                    TraceLine("Unknown 0x38 iscript opcode, arg {0}", warg1);
                    break;
                case Unknown3c:
                    warg1 = ReadWord(ref pc);
                    TraceLine("Unknown 0x3c iscript opcode, arg {0}", warg1);
                    break;
                case Unknown3d:
                    warg1 = ReadWord(ref pc);
                    TraceLine("Unknown 0x3d iscript opcode, arg {0}", warg1);
                    break;
                case Unknown3f:
                    warg1 = ReadWord(ref pc);
                    TraceLine("Unknown 0x3f iscript opcode, arg {0}", warg1);
                    break;
                case Unknown40:
                    warg1 = ReadWord(ref pc);
                    TraceLine("Unknown 0x40 iscript opcode, arg {0}", warg1);
                    break;
                case Unknown41:
                    warg1 = ReadWord(ref pc);
                    TraceLine("Unknown 0x41 iscript opcode, arg {0}", warg1);
                    break;
                case Unknown42:
                    warg1 = ReadWord(ref pc);
                    TraceLine("Unknown 0x42 iscript opcode, arg {0}", warg1);
                    break;
                case FollowFrameChange:
                    if (parentSprite != null)
                        DoPlayFrame(surf, parentSprite.CurrentFrame);
                    break;
                case SwitchUnderlay:
                case PlaceOverlay:
                case PlaceIndependentOverlay:
                case PlaceIndependentOverlayOnTop:
                case DisplayOverlayWithLO:
                case DisplayIndependentOverlayWithLO:
                case PlayRandomSound:
                case DoDamage:
                case AttackWithWeaponAndPlaySound:
                case Attack:
                case AttackWithAppropriateWeapon:
                case CastSpell:
                case UseWeapon:
                case AttackLoopMarker:
                case BeginPlayerLockout:
                case EndPlayerLockout:
                case IgnoreOtherOpcodes:
                case AttackWithDirectionalProjectile:
                case Hide:
                case Unhide:
                case IfPickedUp:
                case IfTargetInRangeGoto:
                case IfTargetInArcGoto:
                    Console.WriteLine("Unhandled iscript opcode: 0x{0:x}", buf[pc - 1]);
                    break;
                default:
                    Console.WriteLine("Unknown iscript opcode: 0x{0:x}", buf[pc - 1]);
                    break;
            }

            return true;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public enum AnimationType
    {
        /// <summary>
        ///
        /// </summary>
        Init,
        /// <summary>
        ///
        /// </summary>
        Death,
        /// <summary>
        ///
        /// </summary>
        GroundAttackInit,
        /// <summary>
        ///
        /// </summary>
        AirAttackInit,
        /// <summary>
        ///
        /// </summary>
        SpAbility1,
        /// <summary>
        ///
        /// </summary>
        GroundAttackRpt,
        /// <summary>
        ///
        /// </summary>
        AirAttackRpt,
        /// <summary>
        ///
        /// </summary>
        SpAbility2,
        /// <summary>
        ///
        /// </summary>
        GroundAttackToIdle,
        /// <summary>
        ///
        /// </summary>
        AirAttackToIdle,
        /// <summary>
        ///
        /// </summary>
        SpAbility3,
        /// <summary>
        ///
        /// </summary>
        Walking,
        /// <summary>
        ///
        /// </summary>
        Other,
        /// <summary>
        ///
        /// </summary>
        BurrowInit,
        /// <summary>
        ///
        /// </summary>
        ConstructHarvest,
        /// <summary>
        ///
        /// </summary>
        IsWorking,
        /// <summary>
        ///
        /// </summary>
        Landing,
        /// <summary>
        ///
        /// </summary>
        Liftoff,
        /// <summary>
        ///
        /// </summary>
        Unknown18,
        /// <summary>
        ///
        /// </summary>
        Unknown19,
        /// <summary>
        ///
        /// </summary>
        Unknown20,
        /// <summary>
        ///
        /// </summary>
        Unknown21,
        /// <summary>
        ///
        /// </summary>
        Unknown22,
        /// <summary>
        ///
        /// </summary>
        Unknown23,
        /// <summary>
        ///
        /// </summary>
        Unknown24,
        /// <summary>
        ///
        /// </summary>
        Burrow,
        /// <summary>
        ///
        /// </summary>
        UnBurrow,
        /// <summary>
        ///
        /// </summary>
        Unknown27
    }
}
