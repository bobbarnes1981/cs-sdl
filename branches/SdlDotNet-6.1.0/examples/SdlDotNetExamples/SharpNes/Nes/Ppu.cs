#region License
/*
Copyright (c) 2005, Jonathan Turner
All rights reserved.

Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:

    * Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
    * Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
    * Neither the name of Sharpnes nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/
#endregion License

// created on 11/24/2004 at 7:49 AM

using System;

namespace SdlDotNetExamples.LargeDemos
{
    public class Ppu
    {
        bool executeNMIonVBlank;
        byte ppuMaster; // 0 = slave, 1 = master, 0xff = unset (master) 
        int spriteSize;  // instead of being 'boolean', this will be 8 or 16
        private int backgroundAddress; // 0000 or 1000

        public int BackgroundAddress
        {
            get { return backgroundAddress; }
            set { backgroundAddress = value; }
        }
        private int spriteAddress; // 0000 or 1000

        public int SpriteAddress
        {
            get { return spriteAddress; }
            set { spriteAddress = value; }
        }
        int ppuAddressIncrement;
        private int nameTableAddress; // 2000, 2400, 2800, 2c00

        public int NameTableAddress
        {
            get { return nameTableAddress; }
            set { nameTableAddress = value; }
        }

        //bool monochromeDisplay; // false = color
        bool noBackgroundClipping; // false = clip left 8 bg pixels
        //bool noSpriteClipping; // false = clip left 8 sprite pixels
        private bool backgroundVisible; // false = invisible

        public bool BackgroundVisible
        {
            get { return backgroundVisible; }
            set { backgroundVisible = value; }
        }
        private bool spritesVisible; // false = sprites invisible

        public bool SpritesVisible
        {
            get { return spritesVisible; }
            set { spritesVisible = value; }
        }
        //int ppuColor; // r/b/g or intensity level

        byte sprite0Hit;
        int[] sprite0Buffer;

        int vramReadWriteAddress;
        int prev_vramReadWriteAddress;
        byte vramHiLoToggle;
        byte vramReadBuffer;
        private byte scrollV;

        public byte ScrollV
        {
            get { return scrollV; }
            set { scrollV = value; }
        }
        private byte scrollH;

        public byte ScrollH
        {
            get { return scrollH; }
            set { scrollH = value; }
        }

        private int currentScanline;

        public int CurrentScanline
        {
            get { return currentScanline; }
            set { currentScanline = value; }
        }
        private byte[] nameTables; //Not sure how smart this is to put here, but it is

        public byte[] NameTables
        {
            get { return nameTables; }
            set { nameTables = value; }
        }
        //the name table part of VRAM

        byte[] spriteRam;
        uint spriteRamAddress;
        int spritesCrossed;

        int frameCounter;

        NesEngine myEngine;
        //public int [] offscreenBuffer;
        private short[] offScreenBuffer;

        public short[] OffScreenBuffer
        {
            get { return offScreenBuffer; }
            set { offScreenBuffer = value; }
        }

        //FIXME: should this be here?
        private VideoNes myVideo;

        public VideoNes MyVideo
        {
            get { return myVideo; }
            set { myVideo = value; }
        }
        /*
        public uint [] Nes_Palette =
        {
        0x808080, 0x0000BB, 0x3700BF, 0x8400A6, 0xBB006A, 0xB7001E, 0xB30000, 0x912600,
        0x7B2B00, 0x003E00, 0x00480D, 0x003C22, 0x002F66, 0x000000, 0x050505, 0x050505, 
        0xC8C8C8, 0x0059FF, 0x443CFF, 0xB733CC, 0xFF33AA, 0xFF375E, 0xFF371A, 0xD54B00,
        0xC46200, 0x3C7B00, 0x1E8415, 0x009566, 0x0084C4, 0x111111, 0x090909, 0x090909, 
        0xFFFFFF, 0x0095FF, 0x6F84FF, 0xD56FFF, 0xFF77CC, 0xFF6F99, 0xFF7B59, 0xFF915F, 
        0xFFA233, 0xA6BF00, 0x51D96A, 0x4DD5AE, 0x00D9FF, 0x666666, 0x0D0D0D, 0x0D0D0D,
        0xFFFFFF, 0x84BFFF, 0xBBBBFF, 0xD0BBFF, 0xFFBFEA, 0xFFBFCC, 0xFFC4B7, 0xFFCCAE, 
        0xFFD9A2, 0xCCE199, 0xAEEEB7, 0xAAF7EE, 0xB3EEFF, 0xDDDDDD, 0x111111, 0x111111
        };
        */

        private ushort[] nesPalette =
	{
	    0x8410, 0x17, 0x3017, 0x8014, 0xb80d, 0xb003, 0xb000, 0x9120,
	    0x7940, 0x1e0, 0x241, 0x1e4, 0x16c, 0x0, 0x20, 0x20,
	    0xce59, 0x2df, 0x41ff, 0xb199, 0xf995, 0xf9ab, 0xf9a3, 0xd240,
	    0xc300, 0x3bc0, 0x1c22, 0x4ac, 0x438, 0x1082, 0x841, 0x841,
	    0xffff, 0x4bf, 0x6c3f, 0xd37f, 0xfbb9, 0xfb73, 0xfbcb, 0xfc8b,
	    0xfd06, 0xa5e0, 0x56cd, 0x4eb5, 0x6df, 0x632c, 0x861, 0x861,
	    0xffff, 0x85ff, 0xbddf, 0xd5df, 0xfdfd, 0xfdf9, 0xfe36, 0xfe75,
	    0xfed4, 0xcf13, 0xaf76, 0xafbd, 0xb77f, 0xdefb, 0x1082, 0x1082
	};

        [CLSCompliant(false)]
        public ushort[] NesPalette
        {
            get { return nesPalette; }
            set { nesPalette = value; }
        }
        public void ControlRegister1Write(byte data)
        {
            //go bit by bit, and flag our values
            if ((data & 0x80) == 0x80)
                executeNMIonVBlank = true;
            else
                executeNMIonVBlank = false;

            if ((data & 0x20) == 0x20)
                spriteSize = 16;
            else
                spriteSize = 8;

            if ((data & 0x10) == 0x10)
                backgroundAddress = 0x1000;
            else
                backgroundAddress = 0x0000;

            if ((data & 0x8) == 0x8)
                spriteAddress = 0x1000;
            else
                spriteAddress = 0x0000;

            if ((data & 0x4) == 0x4)
                ppuAddressIncrement = 32;
            else
                ppuAddressIncrement = 1;

            //FIXME: This is a hack for SMB, but I'm not sure this is true for all games
            if ((backgroundVisible == true) || (ppuMaster == 0xff) || (ppuMaster == 1))
            {
                switch (data & 0x3)
                {
                    case (0x0): nameTableAddress = 0x2000; break;
                    case (0x1): nameTableAddress = 0x2400; break;
                    case (0x2): nameTableAddress = 0x2800; break;
                    case (0x3): nameTableAddress = 0x2C00; break;
                }
            }
            //scrollH = (byte)(scrollH - currentScanline);

            //Console.WriteLine("Name Table now: 0x{0:x}  Scroll H: {1}  Scroll V: {2}  Scanline: {3}  RW: {4:x}", 
            //	nameTableAddress, scrollH, scrollV, currentScanline, vramReadWriteAddress);

            //Zelda fix
            if (myEngine.FixBackgroundChange == true)
            {
                if (currentScanline == 241)
                    nameTableAddress = 0x2000;
            }

            if (ppuMaster == 0xff)
            {
                if ((data & 0x40) == 0x40)
                    ppuMaster = 0;
                else
                    ppuMaster = 1;
            }
            //Console.WriteLine("New Name Table: {0:x}", nameTableAddress);
            //Zelda hack
            //if (currentScanline > 239 )
            //	nameTableAddress = 0x2000;
        }

        public void ControlRegister2Write(byte data)
        {
            //Since some of the settings require us to know other settings first
            //we'll go ahead and do this one in the opposite order

            //if ((data & 0x1) == 0x1)
            //monochromeDisplay = true;
            //else
            //monochromeDisplay = false;

            if ((data & 0x2) == 0x2)
                noBackgroundClipping = true;
            else
                noBackgroundClipping = false;

            //if ((data & 0x4) == 0x4)
            //    noSpriteClipping = true;
            //else
            //    noSpriteClipping = false;

            if ((data & 0x8) == 0x8)
                backgroundVisible = true;
            else
                backgroundVisible = false;

            if ((data & 0x10) == 0x10)
                spritesVisible = true;
            else
                spritesVisible = false;

            //ppuColor = (data >> 5);

        }

        public byte StatusRegisterRead()
        {
            byte returnedValue = 0;

            // VBlank
            if (currentScanline >= 240)
                returnedValue = (byte)(returnedValue | 0x80);

            // Sprite 0 hit
            SpriteZeroHit();

            if (sprite0Hit == 1)
            {
                //Console.WriteLine("Sprite Hit on Line: {0}", currentScanline);
                returnedValue = (byte)(returnedValue | 0x40);
                //sprite0Hit = 0;
            }
            // Sprites on current scanline
            if (spritesCrossed > 8)
                returnedValue = (byte)(returnedValue | 0x20);

            // VRAM Write flag
            // FIXME: Implement this
            vramHiLoToggle = 1;

            return returnedValue;
        }

        public void VramAddressRegister1Write(byte data)
        {
            // Pan and Scroll register write
            if (vramHiLoToggle == 1)
            {
                scrollV = data;
                vramHiLoToggle = 0;
            }
            else
            {
                scrollH = data;
                if (scrollH > 239)
                {
                    //Console.WriteLine("Negative Scroll: {0}", scrollH);
                    scrollH = 0;
                }
                //FIXME: Not sure what to do with this.  It will fix
                //Legacy of the Wizard
                if (myEngine.FixScrollOffset2)
                {
                    if (currentScanline < 240)
                    {
                        scrollH = (byte)(scrollH - currentScanline + 8);
                    }
                }
                //FIXME: Not sure what to do with this.  It will 
                //fix Battle of Olympus
                if (myEngine.FixScrollOffset1)
                {
                    if (currentScanline < 240)
                    {
                        scrollH = (byte)(scrollH - currentScanline);
                    }
                }

                // FIXME: This is another workaround, this time for smb3
                if (myEngine.FixScrollOffset3)
                {
                    if (currentScanline < 240)
                        scrollH = 238;
                }

                //if (currentScanline < 240)
                //	scrollH = 0;

                //FIXME: This will fix Kirby's main menu
                /*
                if (currentScanline < 240)
                {
                    scrollH = (byte)(scrollH - currentScanline - 15 );
                }
                */
                //Console.WriteLine("SCROLL: scrollH: {0}, scrollV: {1}, scanline: {2}"
                //	, scrollH, scrollV, currentScanline);
                vramHiLoToggle = 1;
            }
            //Console.Write("{0} ", data);

            //Console.WriteLine("{0} -- PC: {1:x}", data, myEngine.my6502.pc_register);
        }

        public void VramAddressRegister2Write(byte data)
        {

            if (vramHiLoToggle == 1)
            {
                //if we're high, take the data and move it to the high byte
                //Console.WriteLine("vramReadWriteAddress(before): {0:x}", vramReadWriteAddress);
                prev_vramReadWriteAddress = vramReadWriteAddress;
                vramReadWriteAddress = (int)data << 8;
                vramHiLoToggle = 0;
            }
            else
            {
                vramReadWriteAddress = vramReadWriteAddress + (int)data;
                //Console.WriteLine("Vram RW: {0:x}", vramReadWriteAddress);
                if ((prev_vramReadWriteAddress == 0) && (currentScanline < 240))
                {
                    //We may have a scrolling trick
                    //Console.WriteLine("vramReadWriteAddress(diff): {0:x}", vramReadWriteAddress);
                    if ((vramReadWriteAddress >= 0x2000) && (vramReadWriteAddress <= 0x2400))
                        scrollH = (byte)(((vramReadWriteAddress - 0x2000) / 0x20) * 8 - currentScanline);
                }
                vramHiLoToggle = 1;
            }
        }

        public void VramIORegisterWrite(byte data)
        {
            //Console.WriteLine("VRAM -- Writing 0x{0:x} to 0x{1:x}", data, vramReadWriteAddress);
            if (vramReadWriteAddress < 0x2000)
            {
                myEngine.MyMapper.WriteChrRom((ushort)vramReadWriteAddress, data);
            }
            else if ((vramReadWriteAddress >= 0x2000) && (vramReadWriteAddress < 0x3f00))
            {
                if (myEngine.MyCartridge.Mirroring == Mirroring.Horizontal)
                {
                    //FIXME: trying out the newer Mirroring scheme
                    //nameTables[vramReadWriteAddress - 0x2000] = data;
                    /*
                    switch (vramReadWriteAddress & 0x2C00)
                    {
                        case (0x2000): nameTables[vramReadWriteAddress - 0x2000] = data;
                            break;
                        case (0x2400): nameTables[(vramReadWriteAddress - 0x400) - 0x2000] = data; 
                            break;
                        case (0x2800): nameTables[vramReadWriteAddress - 0x2000] = data;
                            break;
                        case (0x2C00): nameTables[(vramReadWriteAddress - 0x400) - 0x2000] = data; 
                            break;
                    }
                    */
                    /*
                    switch (vramReadWriteAddress & 0x2C00)
                    {
                        case (0x2000): nameTables[vramReadWriteAddress - 0x2000] = data;
                            nameTables[(vramReadWriteAddress + 0x400) - 0x2000] = data; break;
                        case (0x2400): nameTables[vramReadWriteAddress - 0x2000] = data;
                            nameTables[(vramReadWriteAddress - 0x400) - 0x2000] = data; break;
                        case (0x2800): nameTables[vramReadWriteAddress - 0x2000] = data;
                            nameTables[(vramReadWriteAddress + 0x400) - 0x2000] = data; break;
                        case (0x2C00): nameTables[vramReadWriteAddress - 0x2000] = data;
                            nameTables[(vramReadWriteAddress - 0x400) - 0x2000] = data; break;
                    }
                    */
                    //Next Try: Forcing two page only: 0x2000 and 0x2400

                    switch (vramReadWriteAddress & 0x2C00)
                    {
                        case (0x2000): nameTables[vramReadWriteAddress - 0x2000] = data;
                            break;
                        case (0x2400): nameTables[(vramReadWriteAddress - 0x400) - 0x2000] = data;
                            break;
                        case (0x2800): nameTables[vramReadWriteAddress - 0x400 - 0x2000] = data;
                            break;
                        case (0x2C00): nameTables[(vramReadWriteAddress - 0x800) - 0x2000] = data;
                            break;
                    }


                }
                else if (myEngine.MyCartridge.Mirroring == Mirroring.Vertical)
                {
                    //FIXME: trying out the newer Mirroring scheme
                    //nameTables[vramReadWriteAddress - 0x2000] = data;
                    /*
                    switch (vramReadWriteAddress & 0x2C00)
                    {
                        case (0x2000): nameTables[vramReadWriteAddress - 0x2000] = data;
                            break;
                        case (0x2400): nameTables[vramReadWriteAddress - 0x2000] = data;
                            break;
                        case (0x2800): nameTables[(vramReadWriteAddress - 0x800) - 0x2000] = data; 
                            break;
                        case (0x2C00): nameTables[(vramReadWriteAddress - 0x800) - 0x2000] = data; 
                            break;
                    }
                    */
                    /*
                    switch (vramReadWriteAddress & 0x2C00)
                    {
                        case (0x2000): nameTables[vramReadWriteAddress - 0x2000] = data;
                            nameTables[(vramReadWriteAddress + 0x800) - 0x2000] = data; break;
                        case (0x2400): nameTables[vramReadWriteAddress - 0x2000] = data;
                            nameTables[(vramReadWriteAddress + 0x800) - 0x2000] = data; break;
                        case (0x2800): nameTables[vramReadWriteAddress - 0x2000] = data;
                            nameTables[(vramReadWriteAddress - 0x800) - 0x2000] = data; break;
                        case (0x2C00): nameTables[vramReadWriteAddress - 0x2000] = data;
                            nameTables[(vramReadWriteAddress - 0x800) - 0x2000] = data; break;
                    }
                    */
                    //Next Try: Forcing two page only: 0x2000 and 0x2400

                    switch (vramReadWriteAddress & 0x2C00)
                    {
                        case (0x2000): nameTables[vramReadWriteAddress - 0x2000] = data;
                            break;
                        case (0x2400): nameTables[vramReadWriteAddress - 0x2000] = data;
                            break;
                        case (0x2800): nameTables[vramReadWriteAddress - 0x800 - 0x2000] = data;
                            break;
                        case (0x2C00): nameTables[(vramReadWriteAddress - 0x800) - 0x2000] = data;
                            break;
                    }

                }

                else if (myEngine.MyCartridge.Mirroring == Mirroring.OneScreen)
                {
                    if (myEngine.MyCartridge.MirroringBase == 0x2000)
                    {
                        switch (vramReadWriteAddress & 0x2C00)
                        {
                            case (0x2000): nameTables[vramReadWriteAddress - 0x2000] = data;
                                break;
                            case (0x2400): nameTables[vramReadWriteAddress - 0x400 - 0x2000] = data;
                                break;
                            case (0x2800): nameTables[vramReadWriteAddress - 0x800 - 0x2000] = data;
                                break;
                            case (0x2C00): nameTables[vramReadWriteAddress - 0xC00 - 0x2000] = data;
                                break;
                        }
                    }
                    else if (myEngine.MyCartridge.MirroringBase == 0x2400)
                    {
                        switch (vramReadWriteAddress & 0x2C00)
                        {
                            case (0x2000): nameTables[vramReadWriteAddress + 0x400 - 0x2000] = data;
                                break;
                            case (0x2400): nameTables[vramReadWriteAddress - 0x2000] = data;
                                break;
                            case (0x2800): nameTables[vramReadWriteAddress - 0x400 - 0x2000] = data;
                                break;
                            case (0x2C00): nameTables[vramReadWriteAddress - 0x800 - 0x2000] = data;
                                break;
                        }
                    }
                }

                else
                {
                    nameTables[vramReadWriteAddress - 0x2000] = data;
                }
            }
            else if ((vramReadWriteAddress >= 0x3f00) && (vramReadWriteAddress < 0x3f20))
            {
                //Console.WriteLine("Palette: 0x{0:x} = {1:x}", vramReadWriteAddress, data);
                nameTables[vramReadWriteAddress - 0x2000] = data;
                if ((vramReadWriteAddress & 0x7) == 0)
                {
                    nameTables[(vramReadWriteAddress - 0x2000) ^ 0x10] = data;
                }
            }
            //vramHiLoToggle = 1;
            vramReadWriteAddress = vramReadWriteAddress + ppuAddressIncrement;
        }

        public byte VramIORegisterRead()
        {
            byte returnedValue = 0;

            if (vramReadWriteAddress < 0x3f00)
            {
                returnedValue = vramReadBuffer;
                if (vramReadWriteAddress >= 0x2000)
                {
                    vramReadBuffer = nameTables[vramReadWriteAddress - 0x2000];
                }
                else //if (vramReadWriteAddress < 0x2000)
                {
                    vramReadBuffer = myEngine.MyMapper.ReadChrRom((ushort)(vramReadWriteAddress));
                }
            }
            else if (vramReadWriteAddress >= 0x4000)
            {
                Console.WriteLine("I need vram Mirroring {0:x}", vramReadWriteAddress);

                myEngine.IsQuitting = true;
            }
            else
            {
                returnedValue = nameTables[vramReadWriteAddress - 0x2000];
            }

            //vramHiLoToggle = 1;
            //FIXME: This is not entirely accurate, the 'buffered' read
            //should not increment the address the first time
            vramReadWriteAddress = vramReadWriteAddress + ppuAddressIncrement;

            return returnedValue;
        }

        public void SpriteRamAddressRegisterWrite(byte data)
        {
            spriteRamAddress = (uint)data;
        }

        public void SpriteRamIORegisterWrite(byte data)
        {
            spriteRam[spriteRamAddress] = data;
            //FIXME: I assume this wraps around, and does it increment?
            spriteRamAddress++;
        }

        //FIXME: Does this function exist?
        public byte SpriteRamIORegisterRead()
        {
            return spriteRam[spriteRamAddress];
        }

        public void SpriteRamDmaBegin(byte data)
        {
            int i;
            if (data > (ushort.MaxValue - 255) / 256)
            {
                throw new ArgumentOutOfRangeException("data");
            }
            for (i = 0; i < 0x100; i++)
            {
                spriteRam[i] = myEngine.ReadMemory8((ushort)(((uint)data * 0x100) + i));
            }
        }
        public void SpriteZeroHit()
        {
            //"WORKING" SPRITE 0 HIT DETECTION


            //byte sprite_x;
            byte sprite_y;
            //byte sprite_id;
            //byte sprite_attributes;

            if (myEngine.FixSpriteHit)
            {
                //Grab Sprite 0

                //FIXME: Sprite Hit hack

                sprite_y = spriteRam[0];
                //sprite_id = spriteRam[1];
                //sprite_attributes = spriteRam[2];
                //sprite_x = spriteRam[3];

                if (myEngine.MyCartridge.Mapper == 4)
                {
                    if (currentScanline >= (sprite_y + spriteSize - 4))
                        sprite0Hit = 1;
                }
                else
                {
                    if (currentScanline >= (sprite_y + spriteSize + 1))
                        sprite0Hit = 1;
                }
            }
        }

        public void RenderBackground()
        {
            int currentTileColumn;
            int tileNumber;
            //int scanInsideTile;
            int tileDataOffset;
            byte tiledata1, tiledata2;
            byte paletteHighBits;
            int pixelColor;
            int virtualScanline;
            int nameTableBase;
            int i; // genero loop, I should probably name this something better
            int startColumn, endColumn;
            int vScrollSide;
            int startTilePixel, endTilePixel;

            for (vScrollSide = 0; vScrollSide < 2; vScrollSide++)
            {
                virtualScanline = currentScanline + scrollH;
                nameTableBase = nameTableAddress;
                if (vScrollSide == 0)
                {
                    if (virtualScanline >= 240)
                    {
                        switch (nameTableAddress)
                        {
                            case (0x2000): nameTableBase = 0x2800; break;
                            case (0x2400): nameTableBase = 0x2C00; break;
                            case (0x2800): nameTableBase = 0x2000; break;
                            case (0x2C00): nameTableBase = 0x2400; break;

                        }
                        virtualScanline = virtualScanline - 240;
                    }

                    startColumn = scrollV / 8;
                    endColumn = 32;
                }
                else
                {
                    if (virtualScanline >= 240)
                    {
                        switch (nameTableAddress)
                        {
                            case (0x2000): nameTableBase = 0x2C00; break;
                            case (0x2400): nameTableBase = 0x2800; break;
                            case (0x2800): nameTableBase = 0x2400; break;
                            case (0x2C00): nameTableBase = 0x2000; break;

                        }
                        virtualScanline = virtualScanline - 240;
                    }
                    else
                    {
                        switch (nameTableAddress)
                        {
                            case (0x2000): nameTableBase = 0x2400; break;
                            case (0x2400): nameTableBase = 0x2000; break;
                            case (0x2800): nameTableBase = 0x2C00; break;
                            case (0x2C00): nameTableBase = 0x2800; break;

                        }
                    }
                    startColumn = 0;
                    endColumn = (scrollV / 8) + 1;
                }

                //Mirroring step, doing it here allows for dynamic Mirroring
                //like that seen in mappers
                /*
                if (myEngine.myCartridge.Mirroring == Mirroring.HORIZONTAL)
                {
                    switch (nameTableBase)
                    {
                        case (0x2400): nameTableBase = 0x2000; break;
                        case (0x2C00): nameTableBase = 0x2800; break;
                    }
                }
                else if (myEngine.myCartridge.Mirroring == Mirroring.VERTICAL)
                {
                    switch (nameTableBase)
                    {
                        case (0x2800): nameTableBase = 0x2000; break;
                        case (0x2C00): nameTableBase = 0x2400; break;
                    }
                }
                */
                //Next Try: Forcing two page only: 0x2000 and 0x2400				
                if (myEngine.MyCartridge.Mirroring == Mirroring.Horizontal)
                {
                    switch (nameTableBase)
                    {
                        case (0x2400): nameTableBase = 0x2000; break;
                        case (0x2800): nameTableBase = 0x2400; break;
                        case (0x2C00): nameTableBase = 0x2400; break;
                    }
                }
                else if (myEngine.MyCartridge.Mirroring == Mirroring.Vertical)
                {
                    switch (nameTableBase)
                    {
                        case (0x2800): nameTableBase = 0x2000; break;
                        case (0x2C00): nameTableBase = 0x2400; break;
                    }
                }
                else if (myEngine.MyCartridge.Mirroring == Mirroring.OneScreen)
                {
                    nameTableBase = (int)myEngine.MyCartridge.MirroringBase;
                }

                for (currentTileColumn = startColumn; currentTileColumn < endColumn;
                    currentTileColumn++)
                {
                    //Starting tile row is currentScanline / 8
                    //The offset in the tile is currentScanline % 8

                    //Step #1, get the tile number
                    tileNumber = nameTables[nameTableBase - 0x2000 + ((virtualScanline / 8) * 32) + currentTileColumn];

                    //Step #2, get the offset for the tile in the tile data
                    tileDataOffset = backgroundAddress + (tileNumber * 16);

                    //Step #3, get the tile data from chr rom
                    tiledata1 = myEngine.MyMapper.ReadChrRom((ushort)(tileDataOffset + (virtualScanline % 8)));
                    tiledata2 = myEngine.MyMapper.ReadChrRom((ushort)(tileDataOffset + (virtualScanline % 8) + 8));

                    //Step #4, get the attribute byte for the block of tiles we're in
                    //this will put us in the correct section in the palette table
                    paletteHighBits = nameTables[((nameTableBase - 0x2000 +
                        0x3c0 + (((virtualScanline / 8) / 4) * 8) + (currentTileColumn / 4)))];
                    paletteHighBits = (byte)(paletteHighBits >> ((4 * (((virtualScanline / 8) % 4) / 2)) +
                        (2 * ((currentTileColumn % 4) / 2))));
                    paletteHighBits = (byte)((paletteHighBits & 0x3) << 2);

                    //Step #5, render the line inside the tile to the offscreen buffer
                    if (vScrollSide == 0)
                    {
                        if (currentTileColumn == startColumn)
                        {
                            startTilePixel = scrollV % 8;
                            endTilePixel = 8;
                        }
                        else
                        {
                            startTilePixel = 0;
                            endTilePixel = 8;
                        }
                    }
                    else
                    {
                        if (currentTileColumn == endColumn)
                        {
                            startTilePixel = 0;
                            endTilePixel = scrollV % 8;
                        }
                        else
                        {
                            startTilePixel = 0;
                            endTilePixel = 8;
                        }
                    }

                    for (i = startTilePixel; i < endTilePixel; i++)
                    {
                        pixelColor = paletteHighBits + (((tiledata2 & (1 << (7 - i))) >> (7 - i)) << 1) +
                            ((tiledata1 & (1 << (7 - i))) >> (7 - i));

                        if ((pixelColor % 4) != 0)
                        {
                            if (vScrollSide == 0)
                            {
                                offScreenBuffer[(currentScanline * 256) + (8 * currentTileColumn) - scrollV + i] =
                                    (short)NesPalette[(0x3f & nameTables[0x1f00 + pixelColor])];

                                if (sprite0Hit == 0)
                                    sprite0Buffer[(8 * currentTileColumn) - scrollV + i] += 4;
                            }
                            else
                            {
                                if (((8 * currentTileColumn) + (256 - scrollV) + i) < 256)
                                {
                                    offScreenBuffer[(currentScanline * 256) + (8 * currentTileColumn) + (256 - scrollV) + i] =
                                        (short)NesPalette[(0x3f & nameTables[0x1f00 + pixelColor])];

                                    //Console.WriteLine("Greater than: {0}", ((8 * currentTileColumn) + (256-scrollV) + i));
                                    if (sprite0Hit == 0)
                                        sprite0Buffer[(8 * currentTileColumn) + (256 - scrollV) + i] += 4;
                                }
                            }
                        }
                    }
                }
            }
        }

        private void RenderSprites(int behind)
        {
            int i, j;
            int spriteLineToDraw;
            byte tiledata1, tiledata2;
            int offsetToSprite;
            byte paletteHighBits;
            int pixelColor;
            byte actualY;

            byte spriteId;

            //Step #1 loop through each sprite in sprite RAM
            //Back to front, early numbered sprites get drawing priority

            for (i = 252; i >= 0; i = i - 4)
            {
                actualY = (byte)(spriteRam[i] + 1);
                //Step #2: if the sprite falls on the current scanline, draw it
                if (((spriteRam[i + 2] & 0x20) == behind) && (actualY <= currentScanline) && ((actualY + spriteSize) > currentScanline))
                {
                    spritesCrossed++;
                    //Step #3: Draw the sprites differently if they are 8x8 or 8x16
                    if (spriteSize == 8)
                    {
                        //Step #4: calculate which line of the sprite is currently being drawn
                        //Line to draw is: currentScanline - Y coord + 1

                        if ((spriteRam[i + 2] & 0x80) != 0x80)
                            spriteLineToDraw = currentScanline - actualY;
                        else
                            spriteLineToDraw = actualY + 7 - currentScanline;

                        //Step #5: calculate the offset to the sprite's data in
                        //our chr rom data 
                        offsetToSprite = spriteAddress + spriteRam[i + 1] * 16;

                        //Step #6: extract our tile data
                        tiledata1 = myEngine.MyMapper.ReadChrRom((ushort)(offsetToSprite + spriteLineToDraw));
                        tiledata2 = myEngine.MyMapper.ReadChrRom((ushort)(offsetToSprite + spriteLineToDraw + 8));

                        //Step #7: get the palette attribute data
                        paletteHighBits = (byte)((spriteRam[i + 2] & 0x3) << 2);

                        //Step #8, render the line inside the tile to the offscreen buffer
                        for (j = 0; j < 8; j++)
                        {
                            if ((spriteRam[i + 2] & 0x40) == 0x40)
                            {
                                pixelColor = paletteHighBits + (((tiledata2 & (1 << (j))) >> (j)) << 1) +
                                    ((tiledata1 & (1 << (j))) >> (j));
                            }
                            else
                            {
                                pixelColor = paletteHighBits + (((tiledata2 & (1 << (7 - j))) >> (7 - j)) << 1) +
                                    ((tiledata1 & (1 << (7 - j))) >> (7 - j));
                            }
                            if ((pixelColor % 4) != 0)
                            {

                                if ((spriteRam[i + 3] + j) < 256)
                                {
                                    offScreenBuffer[(currentScanline * 256) + (spriteRam[i + 3]) + j] =
                                            (short)NesPalette[(0x3f & nameTables[0x1f10 + pixelColor])];

                                    if (i == 0)
                                    {
                                        sprite0Buffer[(spriteRam[i + 3]) + j] += 1;
                                    }
                                }

                            }
                        }
                    }

                    else
                    {
                        //The sprites are 8x16, to do so we draw two tiles with slightly
                        //different rules than we had before

                        //Step #4: Get the sprite ID and the offset in that 8x16 sprite
                        //Note, for vertical flip'd sprites, we start at 15, instead of
                        //8 like above to force the tiles in opposite order
                        spriteId = spriteRam[i + 1];
                        if ((spriteRam[i + 2] & 0x80) != 0x80)
                        {
                            spriteLineToDraw = currentScanline - actualY;
                        }
                        else
                        {
                            spriteLineToDraw = actualY + 15 - currentScanline;
                        }
                        //Step #5: We draw the sprite like two halves, so getting past the 
                        //first 8 puts us into the next tile
                        //If the ID is even, the tile is in 0x0000, odd 0x1000
                        if (spriteLineToDraw < 8)
                        {
                            //Draw the top tile
                            {
                                if ((spriteId % 2) == 0)
                                    offsetToSprite = 0x0000 + (spriteId) * 16;
                                else
                                    offsetToSprite = 0x1000 + (spriteId - 1) * 16;

                            }
                        }
                        else
                        {
                            //Draw the bottom tile
                            spriteLineToDraw = spriteLineToDraw - 8;

                            if ((spriteId % 2) == 0)
                                offsetToSprite = 0x0000 + (spriteId + 1) * 16;
                            else
                                offsetToSprite = 0x1000 + (spriteId) * 16;
                        }

                        //Step #6: extract our tile data
                        tiledata1 = myEngine.MyMapper.ReadChrRom((ushort)(offsetToSprite + spriteLineToDraw));
                        tiledata2 = myEngine.MyMapper.ReadChrRom((ushort)(offsetToSprite + spriteLineToDraw + 8));

                        //Step #7: get the palette attribute data
                        paletteHighBits = (byte)((spriteRam[i + 2] & 0x3) << 2);

                        //Step #8, render the line inside the tile to the offscreen buffer
                        for (j = 0; j < 8; j++)
                        {
                            if ((spriteRam[i + 2] & 0x40) == 0x40)
                            {
                                pixelColor = paletteHighBits + (((tiledata2 & (1 << (j))) >> (j)) << 1) +
                                    ((tiledata1 & (1 << (j))) >> (j));
                            }
                            else
                            {
                                pixelColor = paletteHighBits + (((tiledata2 & (1 << (7 - j))) >> (7 - j)) << 1) +
                                    ((tiledata1 & (1 << (7 - j))) >> (7 - j));
                            }
                            if ((pixelColor % 4) != 0)
                            {
                                if ((spriteRam[i + 3] + j) < 256)
                                {
                                    offScreenBuffer[(currentScanline * 256) + (spriteRam[i + 3]) + j] =
                                        (short)NesPalette[(0x3f & nameTables[0x1f10 + pixelColor])];

                                    if (i == 0)
                                    {
                                        sprite0Buffer[(spriteRam[i + 3]) + j] += 1;
                                    }
                                }
                            }
                        }

                    }
                }
            }
        }

        public bool RenderNextScanline()
        {
            int i;
            //Console.WriteLine("Rendering line: {0}", currentScanline);

            if (currentScanline < 234)
            {
                //Clean up the line from before
                if ((uint)nameTables[0x1f00] > 63)
                {
                    for (i = 0; i < 256; i++)
                    {
                        //offscreenBuffer[(currentScanline * 256) + i] = (int)Nes_Palette[(int)nameTables[0x1f00]];
                        offScreenBuffer[(currentScanline * 256) + i] = 0;
                        sprite0Buffer[i] = 0;
                    }
                }
                else
                {
                    for (i = 0; i < 256; i++)
                    {
                        offScreenBuffer[(currentScanline * 256) + i] = (short)NesPalette[(uint)nameTables[0x1f00]];
                        sprite0Buffer[i] = 0;
                        //offscreenBuffer[(currentScanline * 256) + i] = 0;
                    }
                }

                spritesCrossed = 0;
                //We are in visible territory, so render to our offscreen buffer
                if (spritesVisible)
                    RenderSprites(0x20);

                if (backgroundVisible)
                    RenderBackground();

                if (spritesVisible)
                    RenderSprites(0);


                //Check to see if we hit sprite 0 against the background
                //Sprite pixels = 1, BG = 4, so if we're greater than 4, we hit
                if (sprite0Hit == 0)
                {
                    for (i = 0; i < 256; i++)
                    {
                        //Console.Write("{0} ", sprite0Buffer[i]);
                        if (sprite0Buffer[i] > 4)
                            sprite0Hit = 1;
                        //offscreenBuffer[(currentScanline * 256) + i] = 0;
                    }
                }
                //Console.WriteLine("Scanline: {0}  Hit: {1}", currentScanline, sprite0Hit);
                if (!noBackgroundClipping)
                {
                    for (i = 0; i < 8; i++)
                        offScreenBuffer[(currentScanline * 256) + i] = 0;
                }
            }

            if (currentScanline == 240)
            {
                myVideo.BlitScreen();
                NesEngine.CheckForEvents();

            }

            currentScanline++;
            if (myEngine.FixScrollOffset1)
            {
                if (currentScanline > 244)
                {
                    //FIXME: This helps fix Battle of Olympus, does it 
                    //break anything?
                    //244 and greater is vblank, so maybe this makes sense
                    //--OR--
                    //Is this cleared on a read?
                    sprite0Hit = 0;
                }
            }
            if (currentScanline > 262)
            {
                //Reset our screen-by-screen variables
                currentScanline = 0;
                sprite0Hit = 0;
                //scrollH = 0;
                //scrollV = 0;
                frameCounter++;
                /*
                if (frameCounter == 60)
                {
                    dtafter = DateTime.Now;
                    Console.WriteLine("FPS: " + (60.0 / ((dtafter-dtbefore).Ticks / 10000000.0)));
                    dtbefore = dtafter;
                    frameCounter = 0;
                }
                */

            }

            //Are we about to NMI on vblank?
            if ((currentScanline == 240) && (executeNMIonVBlank == true))
                return true;
            else
                return false;
        }

        //DEBUG
        public void DumpVram()
        {
            int i;

            Console.WriteLine("\n---Video RAM---");
            for (i = 0; i < 0x2000; i++)
            {
                if (((i) % 32) == 0)
                    Console.Write("\n{0:x}: ", i + 0x2000);
                Console.Write("{0:x} ", nameTables[i]);
            }

            //Console.WriteLine("----------END VRAM----------");
            //Console.WriteLine("");
            //Console.WriteLine("\n---Video RAM---");
            for (i = 0; i < 0x100; i++)
            {
                if (((i) % 0x10) == 0)
                    Console.Write("\n{0:x}: ", i);
                Console.Write("{0:x} ", spriteRam[i]);
            }
            //Console.WriteLine("----------END SPR RAM----------");

            /*
            Console.WriteLine("----------VIDEO RAM----------");
            for (i = 0; i < (256*240); i++)
            {
                Console.Write("{0:x}", offscreenBuffer[i]);
                if (((i+1) % 256) == 0)
                    Console.WriteLine("");
				
            }
            Console.WriteLine("----------END VIDEO RAM----------");
            */
            Console.WriteLine("");
            //Console.WriteLine("Name Table Address: {0:x}", nameTableAddress);
            Console.WriteLine("VRAM Read/Write Address: {0:x}", vramReadWriteAddress);
            Console.WriteLine("Toggle: {0}", vramHiLoToggle);
        }
        public void RestartPpu()
        {
            executeNMIonVBlank = false;
            ppuMaster = 0xff;
            spriteSize = 8;
            backgroundAddress = 0x0000;
            spriteAddress = 0x0000;
            ppuAddressIncrement = 1;
            nameTableAddress = 0x2000;
            currentScanline = 0;
            vramHiLoToggle = 1;
            vramReadBuffer = 0;
            spriteRamAddress = 0x0;
            scrollV = 0;
            scrollH = 0;
            sprite0Hit = 0;
            frameCounter = 0;
        }

        public Ppu(NesEngine theEngine)
        {
            myEngine = theEngine;

            nameTables = new byte[0x2000];
            spriteRam = new byte[0x100];
            //offscreenBuffer = new int[256 * 240];
            offScreenBuffer = new short[256 * 240];
            sprite0Buffer = new int[256];
            myVideo = new VideoNes(this);
            RestartPpu();
        }
    }
}
