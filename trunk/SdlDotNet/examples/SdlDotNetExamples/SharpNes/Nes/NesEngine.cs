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

// created on 10/24/2004 at 11:36
using System;
using System.IO;
using System.Threading;
using SdlDotNet.Core;
using SdlDotNet.Input;
using SdlDotNet.Graphics;

namespace SdlDotNetExamples.LargeDemos
{
    public class NesEngine : EngineBase
    {
        //Universal constants
        //FIXME: this assumes NTSC
        private static uint ticksPerScanline = 113;

        [CLSCompliant(false)]
        public static uint TicksPerScanline
        {
            get { return NesEngine.ticksPerScanline; }
            set { NesEngine.ticksPerScanline = value; }
        }

        //The connections to tie the system together
        private NesCartridge myCartridge;

        public NesCartridge MyCartridge
        {
            get { return myCartridge; }
            set { myCartridge = value; }
        }
        private ProcessorNes6502 my6502;

        public ProcessorNes6502 My6502
        {
            get { return my6502; }
            set { my6502 = value; }
        }
        private Mapper myMapper;

        public Mapper MyMapper
        {
            get { return myMapper; }
            set { myMapper = value; }
        }
        private Ppu myPpu;

        public Ppu MyPpu
        {
            get { return myPpu; }
            set { myPpu = value; }
        }
        Joypad myJoypad;
        private bool isQuitting;

        public bool IsQuitting
        {
            get { return isQuitting; }
            set { isQuitting = value; }
        }
        private bool hasQuit;

        public bool HasQuit
        {
            get { return hasQuit; }
            set { hasQuit = value; }
        }
        private bool isDebugging;

        public bool IsDebugging
        {
            get { return isDebugging; }
            set { isDebugging = value; }
        }
        private bool isSaveRamReadOnly;

        public bool IsSaveRamReadOnly
        {
            get { return isSaveRamReadOnly; }
            set { isSaveRamReadOnly = value; }
        }
        private bool isPaused;

        public bool IsPaused
        {
            get { return isPaused; }
            set { isPaused = value; }
        }
        private bool fixBackgroundChange;

        public bool FixBackgroundChange
        {
            get { return fixBackgroundChange; }
            set { fixBackgroundChange = value; }
        }
        private bool fixSpriteHit;

        public bool FixSpriteHit
        {
            get { return fixSpriteHit; }
            set { fixSpriteHit = value; }
        }
        private bool fixScrollOffset1;

        public bool FixScrollOffset1
        {
            get { return fixScrollOffset1; }
            set { fixScrollOffset1 = value; }
        }
        private bool fixScrollOffset2;

        public bool FixScrollOffset2
        {
            get { return fixScrollOffset2; }
            set { fixScrollOffset2 = value; }
        }
        private bool fixScrollOffset3;

        public bool FixScrollOffset3
        {
            get { return fixScrollOffset3; }
            set { fixScrollOffset3 = value; }
        }

        //byte counter;
        byte[][] scratchRam;
        byte[] saveRam;

        string saveFilename;


        private uint numOfInstructions;

        [CLSCompliant(false)]
        public uint NumOfInstructions
        {
            get { return numOfInstructions; }
            set { numOfInstructions = value; }
        }

        [CLSCompliant(false)]
        public byte ReadMemory8(ushort address)
        {
            byte returnvalue = 0;

            if (address < 0x2000)
            {

                if (address < 0x800)
                {
                    returnvalue = scratchRam[0][address];
                }
                else if (address < 0x1000)
                {
                    returnvalue = scratchRam[1][address - 0x800];
                    //Console.WriteLine("I need ram Mirroring {0:x}", address);
                }

                else if (address < 0x1800)
                {
                    returnvalue = scratchRam[2][address - 0x1000];
                    //Console.WriteLine("I need ram Mirroring {0:x}", address);
                }
                //else if (address < 0x2000) {
                else
                {
                    returnvalue = scratchRam[3][address - 0x1800];
                    //Console.WriteLine("I need ram Mirroring {0:x}", address);
                }

                //returnvalue = scratchRam[address >> 11][address & 0x7ff];
            }
            else if (address < 0x6000)
            {
                switch (address)
                {
                    case (0x2002): returnvalue = myPpu.StatusRegisterRead(); break;
                    case (0x2004): returnvalue = myPpu.SpriteRamIORegisterRead(); break;
                    case (0x2007): returnvalue = myPpu.VramIORegisterRead(); break;
                    //case (0x4015): returnvalue = myPPU.Sound_Signal_Register_Read(); break;
                    case (0x4016): returnvalue = myJoypad.Joypad1Read(); break;
                    case (0x4017): returnvalue = Joypad.Joypad2Read(); break;
                    //default: Console.WriteLine("UNKOWN READ: {0:x}", address); break;
                }
            }
            else if (address < 0x8000)
            {
                returnvalue = saveRam[address - 0x6000];
                if (myCartridge.Mapper == 5)
                    returnvalue = 1;
            }
            else
            {
                returnvalue = myMapper.ReadPrgRom(address);
            }
            return returnvalue;
        }

        //This is optimized for the places the PC can be
        //Except, it doesn't seem to be faster
        [CLSCompliant(false)]
        public byte ReadMemory8PC(ushort address)
        {
            byte returnvalue = 0;
            /*
            if ((address >= 0x2000) && (address < 0x6000))
            {
                Console.WriteLine("ERROR: PC = {0:x}", address);
                isQuitting = true;
            }
            */
            if (address < 0x800)
            {
                returnvalue = scratchRam[0][address];
            }
            else if (address < 0x1000)
            {
                returnvalue = scratchRam[1][address - 0x800];
            }

            else if (address < 0x1800)
            {
                returnvalue = scratchRam[2][address - 0x1000];
            }
            else if (address < 0x2000)
            {
                returnvalue = scratchRam[3][address - 0x1800];
            }

            else if (/*(address >= 0x6000)&&*/(address < 0x8000))
            {
                returnvalue = saveRam[address - 0x6000];
            }
            //else if (address >= 0x8000){
            else
            {
                returnvalue = myMapper.ReadPrgRom((ushort)address);
            }
            return returnvalue;
        }

        //NOTE: I'm replacing ReadMemory16, if there are changes
        //to ReadMemory please be sure to FIXME also
        /*
        public ushort ReadMemory16(ushort address)
        {
            byte data_1 = ReadMemory8(address);
            byte data_2 = ReadMemory8((ushort)(address+1));
            //Because of initialization order, we copy the code from Nes6502 to here
            ushort data = (ushort)data_2;
            data = (ushort)(data << 8);
            data += (ushort)data_1;
		
            return data;
        }
        */

        [CLSCompliant(false)]
        public ushort ReadMemory16(ushort address)
        {
            //byte returnvalue = 0;
            byte data_1;
            byte data_2;

            //FIXME: We assume it is not 0x2000-0x6000 because that
            //doesn't make any sense

            //FIXME: We also assume no boundaries are crossed
            //This may not be true, and may cause segfaults

            if (address < 0x2000)
            {
                if (address < 0x800)
                {
                    data_1 = scratchRam[0][address];
                    data_2 = scratchRam[0][address + 1];
                }
                else if (address < 0x1000)
                {
                    data_1 = scratchRam[1][address - 0x800];
                    data_2 = scratchRam[1][address - 0x800 + 1];
                }

                else if (address < 0x1800)
                {
                    data_1 = scratchRam[2][address - 0x1000];
                    data_2 = scratchRam[2][address - 0x1000 + 1];
                }
                else
                {
                    data_1 = scratchRam[3][address - 0x1800];
                    data_2 = scratchRam[3][address - 0x1800 + 1];
                }
            }
            else if (address < 0x8000)
            {
                data_1 = saveRam[address - 0x6000];
                data_2 = saveRam[address - 0x6000 + 1];
                //FIXME: At some point I need to fix mapper 5

                //if (myCartridge.mapper == 5)
                //	returnvalue = 1;

            }
            else
            {
                data_1 = myMapper.ReadPrgRom(address);
                data_2 = myMapper.ReadPrgRom((ushort)(address + 1));
            }

            ushort data = (ushort)((data_2 << 8) + data_1);

            return data;
        }

        [CLSCompliant(false)]
        public byte WriteMemory8(ushort address, byte data)
        {
            if (address < 0x800)
            {
                scratchRam[0][address] = data;
            }
            else if (address < 0x1000)
            {
                scratchRam[1][address - 0x800] = data;
            }
            else if (address < 0x1800)
            {
                scratchRam[2][address - 0x1000] = data;
            }
            else if (address < 0x2000)
            {
                scratchRam[3][address - 0x1800] = data;
            }
            else if (address < 0x4000)
            {
                //address = (ushort)((address - 0x2000) % 8);
                switch (address)
                {
                    case (0x2000): myPpu.ControlRegister1Write(data); break;
                    case (0x2001): myPpu.ControlRegister2Write(data); break;
                    case (0x2003): myPpu.SpriteRamAddressRegisterWrite(data); break;
                    case (0x2004): myPpu.SpriteRamIORegisterWrite(data); break;
                    case (0x2005): myPpu.VramAddressRegister1Write(data); break;
                    case (0x2006): myPpu.VramAddressRegister2Write(data); break;
                    case (0x2007): myPpu.VramIORegisterWrite(data); break;
                    //default: Console.WriteLine("UNKOWN CONTROL WRITE: {0}", address); break;
                }
            }
            else if (address < 0x6000)
            {
                switch (address)
                {
                    case (0x4014): myPpu.SpriteRamDmaBegin(data); break;
                    case (0x4016): myJoypad.Joypad1Write(data); break;
                    case (0x4017): Joypad.Joypad2Write(/*data)*/); break;
                    //default: Console.WriteLine("UNKOWN WRITE: {0:x}", address); break;
                }
                if (myCartridge.Mapper == 5)
                    myMapper.WritePrgRom(address, data);
            }
            else if (address < 0x8000)
            {
                if (!isSaveRamReadOnly)
                    saveRam[address - 0x6000] = data;

                if (myCartridge.Mapper == 34)
                    myMapper.WritePrgRom(address, data);

            }
            else
            {
                myMapper.WritePrgRom(address, data);
            }
            return 1;
        }

        [CLSCompliant(false)]
        public static byte WriteMemory16(/*ushort address, ushort data*/)
        {
            Console.WriteLine("** ERROR: WriteMemory16 was used **");

            return 255;
        }

        public void TogglePause()
        {
            if (isPaused)
            {
                isPaused = false;
                Video.WindowCaption = "SDL.NET - NES Window";
            }
            else
            {
                isPaused = true;
                Video.WindowCaption = "[paused]";
            }
        }

        private void Quit(object sender, QuitEventArgs e)
        {
            QuitEngine();
        }

        private void KeyboardDown(object sender, KeyboardEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                if (isPaused)
                {
                    QuitEngine();
                }
            }
            if (e.Key == Key.Space)
            {
                TogglePause();
            }
        }

        public static void CheckForEvents()
        {
            while (Events.Poll())
            { }
        }

        public void InitializeEngine()
        {
            myCartridge = new NesCartridge();
            my6502 = new ProcessorNes6502(this);
            myMapper = new Mapper(this, myCartridge);
            myPpu = new Ppu(this);
            myJoypad = new Joypad();

            scratchRam = new byte[4][];
            scratchRam[0] = new byte[0x800];
            scratchRam[1] = new byte[0x800];
            scratchRam[2] = new byte[0x800];
            scratchRam[3] = new byte[0x800];
            saveRam = new byte[0x2000];

            isSaveRamReadOnly = false;
            isDebugging = false;
            isQuitting = false;
            isPaused = false;
            hasQuit = false;
            fixBackgroundChange = false;
            fixSpriteHit = false;
            fixScrollOffset1 = false;
            fixScrollOffset2 = false;
            fixScrollOffset3 = false;
        }

        public void RestartEngine()
        {
            isSaveRamReadOnly = false;
            isDebugging = false;
            isQuitting = false;
            isPaused = false;
            hasQuit = false;
            fixBackgroundChange = false;
            fixSpriteHit = false;
            fixScrollOffset1 = false;
            fixScrollOffset2 = false;
            fixScrollOffset3 = false;
            myPpu.RestartPpu();
            Video.WindowCaption = "SDL.NET - NES Window";
        }

        public void QuitEngine()
        {
            isQuitting = true;

        }
        public override void RenderNextScanline()
        {
            //This is a tie-in function to the PPU
            //If we decide later that the CPU should have direct visibility, this
            //won't be needed

            if (myPpu.RenderNextScanline() == true)
            {
                //We entered VBlank and executeNMIonVBlank is set
                //Console.WriteLine("Entering --VBLANK--");
                my6502.Push16(my6502.PCRegister);
                my6502.PushStatus();
                my6502.PCRegister = ReadMemory16(0xFFFA);
            }

            if (myCartridge.Mapper == 4)
            {
                myMapper.TickTimer();
            }

        }

        public bool LoadCart(string fileName)
        {
            if (fileName == null)
            {
                throw new ArgumentNullException("fileName");
            }
            byte[] nesHeader = new byte[16];
            int i;

            //DEBUG
            //counter = 1;

            try
            {
                using (FileStream reader = File.OpenRead(fileName))
                {
                    reader.Read(nesHeader, 0, 16);

                    //Allocate space, because of mappers we don't use the 16k default
                    int prg_roms = nesHeader[4] * 4;
                    myCartridge.PrgRomPages = nesHeader[4];
                    //Console.WriteLine("Number of PRG pages: {0}", myCartridge.prg_rom_pages);
                    myCartridge.PrgRom = new byte[prg_roms][];
                    for (i = 0; i < (prg_roms); i++)
                    {
                        //Console.WriteLine("Create PRG Page: {0}", i);
                        myCartridge.PrgRom[i] = new byte[4096];
                        reader.Read(myCartridge.PrgRom[i], 0, 4096);
                    }
                    //Console.WriteLine("CHR ROM: {0}", nesHeader[5]);

                    //Console.WriteLine("Zelda fix: {0}", fix_zelda);
                    //Allocate space, because of mappers we don't use the 16k default
                    int chr_roms = nesHeader[5] * 8;
                    myCartridge.ChrRomPages = nesHeader[5];
                    //Console.WriteLine("Number of CHR pages: {0}", myCartridge.chr_rom_pages);
                    if (myCartridge.ChrRomPages != 0)
                    {
                        myCartridge.ChrRom = new byte[chr_roms][];
                        for (i = 0; i < (chr_roms); i++)
                        {
                            myCartridge.ChrRom[i] = new byte[1024];
                            reader.Read(myCartridge.ChrRom[i], 0, 1024);
                        }
                        myCartridge.IsVram = false;
                    }
                    else
                    {
                        //If we have 0 CHR pages, we're dealing with VRAM instead of VROM
                        //So make enough space by providing the minimum for 0x0000 and 0x1000
                        //and set the toggle that allows us to to write to the video memory
                        myCartridge.ChrRom = new byte[8][];
                        for (i = 0; i < 8; i++)
                        {
                            myCartridge.ChrRom[i] = new byte[1024];
                        }
                        myCartridge.IsVram = true;
                    }

                    if ((nesHeader[6] & 0x1) == 0x0)
                    {
                        myCartridge.Mirroring = Mirroring.Horizontal;
                    }
                    else
                    {
                        myCartridge.Mirroring = Mirroring.Vertical;
                    }

                    if ((nesHeader[6] & 0x2) == 0x0)
                    {
                        //Console.WriteLine("No Save RAM");
                        myCartridge.SaveRamPresent = false;
                    }
                    else
                    {
                        //Console.WriteLine("Save RAM enabled");
                        myCartridge.SaveRamPresent = true;
                    }

                    if ((nesHeader[6] & 0x4) == 0x0)
                    {
                        myCartridge.TrainerPresent = false;
                    }
                    else
                    {
                        myCartridge.TrainerPresent = true;
                    }

                    if ((nesHeader[6] & 0x8) != 0x0)
                    {
                        myCartridge.Mirroring = Mirroring.FourScreen;
                    }

                    if (nesHeader[7] == 0x44)
                    {
                        //!DiskDude! garbage ignore
                        myCartridge.Mapper = (byte)(nesHeader[6] >> 4);
                    }
                    else
                    {
                        myCartridge.Mapper = (byte)((nesHeader[6] >> 4) + (nesHeader[7] & 0xf0));
                    }
                    if ((nesHeader[6] == 0x23) && (nesHeader[7] == 0x64))
                        myCartridge.Mapper = 2;

                    //Console.WriteLine("Mirroring: {0}", myCartridge.Mirroring);
                    //Console.WriteLine("Mapper: {0}", myCartridge.mapper);

                    //ID the rom, and enable fixes when necessary

                    if ((myCartridge.PrgRom[prg_roms - 1][0xfeb] == 'Z') &&
                        (myCartridge.PrgRom[prg_roms - 1][0xfec] == 'E') &&
                        (myCartridge.PrgRom[prg_roms - 1][0xfed] == 'L') &&
                        (myCartridge.PrgRom[prg_roms - 1][0xfee] == 'D') &&
                        (myCartridge.PrgRom[prg_roms - 1][0xfef] == 'A'))
                    {
                        fixBackgroundChange = true;
                        //Console.WriteLine("Workaround: BG Change");
                    }

                    if ((myCartridge.PrgRom[prg_roms - 1][0xfe0] == 'B') &&
                        (myCartridge.PrgRom[prg_roms - 1][0xfe1] == 'B') &&
                        (myCartridge.PrgRom[prg_roms - 1][0xfe2] == '4') &&
                        (myCartridge.PrgRom[prg_roms - 1][0xfe3] == '7') &&
                        (myCartridge.PrgRom[prg_roms - 1][0xfe4] == '9') &&
                        (myCartridge.PrgRom[prg_roms - 1][0xfe5] == '5') &&
                        (myCartridge.PrgRom[prg_roms - 1][0xfe6] == '6') &&
                        (myCartridge.PrgRom[prg_roms - 1][0xfe7] == '-') &&
                        (myCartridge.PrgRom[prg_roms - 1][0xfe8] == '1') &&
                        (myCartridge.PrgRom[prg_roms - 1][0xfe9] == '5') &&
                        (myCartridge.PrgRom[prg_roms - 1][0xfea] == '4') &&
                        (myCartridge.PrgRom[prg_roms - 1][0xfeb] == '4') &&
                        (myCartridge.PrgRom[prg_roms - 1][0xfec] == '0'))
                    {
                        fixScrollOffset1 = true;
                        //Console.WriteLine("Workaround: Scroll Offset #1");
                    }
                    if ((myCartridge.PrgRom[0][0x9] == 0xfc) &&
                        (myCartridge.PrgRom[0][0xa] == 0xfc) &&
                        (myCartridge.PrgRom[0][0xb] == 0xfc) &&
                        (myCartridge.PrgRom[0][0xc] == 0x40) &&
                        (myCartridge.PrgRom[0][0xd] == 0x40) &&
                        (myCartridge.PrgRom[0][0xe] == 0x40) &&
                        (myCartridge.PrgRom[0][0xf] == 0x40))
                    {
                        fixScrollOffset2 = true;
                        //Console.WriteLine("Workaround: Scroll Offset #2");
                    }
                    if ((myCartridge.PrgRom[0][0x75] == 0x11) &&
                        (myCartridge.PrgRom[0][0x76] == 0x12) &&
                        (myCartridge.PrgRom[0][0x77] == 0x13) &&
                        (myCartridge.PrgRom[0][0x78] == 0x14) &&
                        (myCartridge.PrgRom[0][0x79] == 0x07) &&
                        (myCartridge.PrgRom[0][0x7a] == 0x03) &&
                        (myCartridge.PrgRom[0][0x7b] == 0x03) &&
                        (myCartridge.PrgRom[0][0x7c] == 0x03) &&
                        (myCartridge.PrgRom[0][0x7d] == 0x03)
                        )
                    {
                        fixScrollOffset3 = true;
                        //Console.WriteLine("Workaround: Scroll Offset #3");
                    }


                    //This should help with Dragon Strike
                    //if (myCartridge.mapper == 4)
                    //	fix_spritehit = true;

                    myMapper.SetupMapperDefaults();

                    //my6502.RunProcessor();

                    //myPPU.DumpVRAM();
                }
            }
            catch (FileNotFoundException e)
            {
                e.ToString();
                //Console.Error.WriteLine("File \"{0}\" does not exist.", filename);
                return false;
            }
            catch (NullReferenceException e)
            {
                e.ToString();
                //Console.Error.WriteLine(e);
                return false;
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.StackTrace);
            }

            if (myCartridge.SaveRamPresent)
            {
                //If we have save RAM, try to load it
                saveFilename = fileName.Remove(fileName.Length - 3, 3);
                saveFilename = saveFilename.Insert(saveFilename.Length, "sav");
                //Console.WriteLine("SaveRAM enabled: {0}", saveFilename);

                try
                {
                    using (FileStream reader = File.OpenRead(saveFilename))
                    {
                        reader.Read(saveRam, 0, 0x2000);
                    }
                }
                catch (NullReferenceException e)
                {
                    e.ToString();
                    //Console.WriteLine("No SaveRAM found.");
                    //Ignore it, we'll make our own.
                }
                catch (FileNotFoundException e)
                {
                    e.ToString();
                    //Console.WriteLine("No SaveRAM found.");
                    //Ignore it, we'll make our own.
                }
                catch (ArgumentException e)
                {
                    Console.WriteLine(e.StackTrace);
                }
            }

            return true;
        }
        public override void RunCart()
        {
            Events.Quit += new EventHandler<QuitEventArgs>(this.Quit);
            Events.KeyboardDown += new EventHandler<SdlDotNet.Input.KeyboardEventArgs>(this.KeyboardDown);
            my6502.PCRegister = ReadMemory16(0xFFFC);
            //myPPU.myVideo.StartVideo();
            try
            {
                my6502.RunProcessor();
            }
            catch (ThreadAbortException tae)
            {
                tae.ToString();
                if (myCartridge.SaveRamPresent)
                {
                    //If we have save RAM, try to save it
                    try
                    {
                        using (FileStream writer = File.OpenWrite(Path.Combine(Path.Combine(SharpNesMain.FilePath, SharpNesMain.FileDirectory), saveFilename)))
                        {
                            writer.Write(saveRam, 0, 0x2000);
                        }
                    }
                    catch (NullReferenceException e)
                    {
                        e.ToString();
                        //Console.WriteLine("SaveRAM could not be saved.");
                    }
                    catch (FileNotFoundException e)
                    {
                        e.ToString();
                        //Console.WriteLine("No SaveRAM found.");
                        //Ignore it, we'll make our own.
                    }
                    catch (ArgumentException e)
                    {
                        Console.WriteLine(e.StackTrace);
                    }
                }
            }

            if (myCartridge.SaveRamPresent)
            {
                //If we have save RAM, try to save it
                try
                {
                    using (FileStream writer = File.OpenWrite(saveFilename))
                    {
                        writer.Write(saveRam, 0, 0x2000);
                    }
                }
                catch (NullReferenceException e)
                {
                    e.ToString();
                    //Console.WriteLine("SaveRAM could not be saved.");
                }
                catch (FileNotFoundException e)
                {
                    e.ToString();
                    //Console.WriteLine("No SaveRAM found.");
                    //Ignore it, we'll make our own.
                }
                catch (ArgumentException e)
                {
                    Console.WriteLine(e.StackTrace);
                }
            }
            hasQuit = true;
            //myPPU.myVideo.CloseVideo();
        }

        public NesEngine()
        {
            InitializeEngine();

            myPpu.MyVideo.StartVideo();
        }
    }
}
