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

using System;

// created on 10/30/2004 at 09:56

namespace SdlDotNetExamples.LargeDemos
{
    public enum Mirroring
    {
        Horizontal,
        Vertical,
        FourScreen,
        OneScreen
    };

    public class NesCartridge
    {
        private byte[][] prgRom;

        public byte[][] PrgRom
        {
            get { return prgRom; }
            set { prgRom = value; }
        }
        private byte[][] chrRom;

        public byte[][] ChrRom
        {
            get { return chrRom; }
            set { chrRom = value; }
        }
        private Mirroring mirroring;

        public Mirroring Mirroring
        {
            get { return mirroring; }
            set { mirroring = value; }
        }
        private bool trainerPresent;

        public bool TrainerPresent
        {
            get { return trainerPresent; }
            set { trainerPresent = value; }
        }
        private bool saveRamPresent;

        public bool SaveRamPresent
        {
            get { return saveRamPresent; }
            set { saveRamPresent = value; }
        }
        private bool isVram;

        public bool IsVram
        {
            get { return isVram; }
            set { isVram = value; }
        }
        private byte mapper;

        public byte Mapper
        {
            get { return mapper; }
            set { mapper = value; }
        }

        private byte prgRomPages;

        public byte PrgRomPages
        {
            get { return prgRomPages; }
            set { prgRomPages = value; }
        }
        private byte chrRomPages;

        public byte ChrRomPages
        {
            get { return chrRomPages; }
            set { chrRomPages = value; }
        }

        private uint mirroringBase; //For one screen Mirroring

        [CLSCompliant(false)]
        public uint MirroringBase
        {
            get { return mirroringBase; }
            set { mirroringBase = value; }
        }
    }
}
