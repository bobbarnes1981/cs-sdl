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

// created on 10/26/2004 at 06:37
using System;
using System.Threading;

namespace SdlDotNetExamples.LargeDemos
{
    // The NES's primary processor
    public class ProcessorNes6502
    {
        private byte aRegister;

        public byte ARegister
        {
            get { return aRegister; }
            set { aRegister = value; }
        }
        private byte xIndexRegister;

        public byte XIndexRegister
        {
            get { return xIndexRegister; }
            set { xIndexRegister = value; }
        }
        private byte yIndexRegister;

        public byte YIndexRegister
        {
            get { return yIndexRegister; }
            set { yIndexRegister = value; }
        }
        private byte spRegister;

        public byte SPRegister
        {
            get { return spRegister; }
            set { spRegister = value; }
        }

        private ushort pcRegister;

        [CLSCompliant(false)]
        public ushort PCRegister
        {
            get { return pcRegister; }
            set { pcRegister = value; }
        }

        //As much as the flags should be bools, they're easier to use in math as bytes 
        private byte carryFlag;

        public byte CarryFlag
        {
            get { return carryFlag; }
            set { carryFlag = value; }
        }
        private byte zeroFlag;

        public byte ZeroFlag
        {
            get { return zeroFlag; }
            set { zeroFlag = value; }
        }
        private byte interruptFlag;

        public byte InterruptFlag
        {
            get { return interruptFlag; }
            set { interruptFlag = value; }
        }
        private byte decimalFlag;

        public byte DecimalFlag
        {
            get { return decimalFlag; }
            set { decimalFlag = value; }
        }
        private byte brkFlag;

        public byte BrkFlag
        {
            get { return brkFlag; }
            set { brkFlag = value; }
        }
        private byte overflowFlag;

        public byte OverflowFlag
        {
            get { return overflowFlag; }
            set { overflowFlag = value; }
        }
        private byte signFlag;

        public byte SignFlag
        {
            get { return signFlag; }
            set { signFlag = value; }
        }

        NesEngine myEngine;

        //Delegates removed temporarily for testing
        //public delegate void Opcode ();
        //Opcode[] opcodes;

        uint tickCount;

        private uint totalTickCount;  //because of timers, this is not strictly complete

        [CLSCompliant(false)]
        public uint TotalTickCount
        {
            get { return totalTickCount; }
            set { totalTickCount = value; }
        }

        private uint timerFinishTickCount;

        [CLSCompliant(false)]
        public uint TimerFinishTickCount
        {
            get { return timerFinishTickCount; }
            set { timerFinishTickCount = value; }
        }

        //Helper variables
        byte currentOpcode;
        ushort previousPC;

        //Helper funtions

        //While this might be slightly misnamed, it's an easy function
        //to get two bytes into a correct address
        [CLSCompliant(false)]
        public static ushort MakeAddress(byte address1, byte address2)
        {
            uint newAddress = (uint)address2;
            newAddress = newAddress << 8;
            newAddress += (uint)address1;
            return (ushort)newAddress;
        }
        // The default helpers are all read functions
        [CLSCompliant(false)]
        public byte ZeroPage(ushort address)
        {
            return myEngine.ReadMemory8(address);
        }

        [CLSCompliant(false)]
        public byte ZeroPageX(ushort address)
        {
            return myEngine.ReadMemory8((ushort)(0xff & (address + xIndexRegister)));
        }

        [CLSCompliant(false)]
        public byte ZeroPageY(ushort address)
        {
            return myEngine.ReadMemory8((ushort)(0xff & (address + yIndexRegister)));
        }
        public byte Absolute(byte address1, byte address2)
        {
            return myEngine.ReadMemory8(MakeAddress(address1, address2));
        }
        public byte AbsoluteX(byte address1, byte address2, bool checkPage)
        {
            if (checkPage)
            {
                if ((MakeAddress(address1, address2) & 0xFF00) !=
                    ((MakeAddress(address1, address2) + xIndexRegister) & 0xFF00))
                {
                    tickCount += 1;
                };
            }
            return myEngine.ReadMemory8((ushort)(MakeAddress(address1, address2) + xIndexRegister));
        }
        public byte AbsoluteY(byte address1, byte address2, bool checkPage)
        {
            if (checkPage)
            {
                if ((MakeAddress(address1, address2) & 0xFF00) !=
                    ((MakeAddress(address1, address2) + yIndexRegister) & 0xFF00))
                {
                    tickCount += 1;
                };
            }
            return myEngine.ReadMemory8((ushort)(MakeAddress(address1, address2) + yIndexRegister));
        }
        public byte IndirectX(byte address)
        {
            return myEngine.ReadMemory8((ushort)myEngine.ReadMemory16((ushort)(0xff & (address + (ushort)xIndexRegister))));
        }
        public byte IndirectY(byte address, bool checkPage)
        {
            if (checkPage)
            {
                if ((myEngine.ReadMemory16(address) & 0xFF00) !=
                    ((myEngine.ReadMemory16(address) + yIndexRegister) & 0xFF00))
                {
                    tickCount += 1;
                };
            }
            return myEngine.ReadMemory8((ushort)(myEngine.ReadMemory16(address) + (ushort)yIndexRegister));
        }

        //but there are other cases where we need to write instead
        //FIXME: I seriously doubt all these are needed
        [CLSCompliant(false)]
        public byte ZeroPageWrite(ushort address, byte data)
        {
            return myEngine.WriteMemory8(address, data);
        }
        [CLSCompliant(false)]
        public byte ZeroPageXWrite(ushort address, byte data)
        {
            return myEngine.WriteMemory8((ushort)(0xff & (address + xIndexRegister)), data);
        }
        [CLSCompliant(false)]
        public byte ZeroPageYWrite(ushort address, byte data)
        {
            return myEngine.WriteMemory8((ushort)(0xff & (address + yIndexRegister)), data);
        }
        public byte AbsoluteWrite(byte address1, byte address2, byte data)
        {
            return myEngine.WriteMemory8(MakeAddress(address1, address2), data);
        }
        public byte AbsoluteXWrite(byte address1, byte address2, byte data)
        {
            return myEngine.WriteMemory8((ushort)(MakeAddress(address1, address2) + xIndexRegister), data);
        }
        public byte AbsoluteYWrite(byte address1, byte address2, byte data)
        {
            return myEngine.WriteMemory8((ushort)(MakeAddress(address1, address2) + yIndexRegister), data);
        }
        public byte IndirectXWrite(byte address, byte data)
        {
            return myEngine.WriteMemory8((ushort)myEngine.ReadMemory16((ushort)(0xff & (address + (short)xIndexRegister))), data);
        }
        public byte IndirectYWrite(byte address, byte data)
        {
            return myEngine.WriteMemory8((ushort)(myEngine.ReadMemory16(address) + (ushort)yIndexRegister), data);
        }

        //PUSH/PULL/POP/ETC
        public void Push8(byte data)
        {
            myEngine.WriteMemory8((ushort)(0x100 + spRegister), (byte)(data & 0xff));
            spRegister = (byte)(spRegister - 1);
        }

        [CLSCompliant(false)]
        public void Push16(ushort data)
        {
            Push8((byte)(data >> 8));
            Push8((byte)(data & 0xff));
        }
        public void PushStatus()
        {
            byte statusdata = 0;

            if (signFlag == 1)
                statusdata = (byte)(statusdata + 0x80);

            if (overflowFlag == 1)
                statusdata = (byte)(statusdata + 0x40);

            //statusdata = (byte)(statusdata + 0x20);

            if (brkFlag == 1)
                statusdata = (byte)(statusdata + 0x10);

            if (decimalFlag == 1)
                statusdata = (byte)(statusdata + 0x8);

            if (interruptFlag == 1)
                statusdata = (byte)(statusdata + 0x4);

            if (zeroFlag == 1)
                statusdata = (byte)(statusdata + 0x2);

            if (carryFlag == 1)
                statusdata = (byte)(statusdata + 0x1);

            Push8(statusdata);
        }
        public byte Pull8()
        {
            spRegister = (byte)(spRegister + 1);
            return myEngine.ReadMemory8((ushort)(0x100 + spRegister));
        }
        [CLSCompliant(false)]
        public ushort Pull16()
        {
            byte data1, data2;
            ushort fulldata;

            data1 = Pull8();
            data2 = Pull8();

            //We use MakeAddress because it's easier
            fulldata = MakeAddress(data1, data2);

            return fulldata;
        }
        public void PullStatus()
        {
            byte statusdata = Pull8();

            if ((statusdata & 0x80) == 0x80)
                signFlag = 1;
            else
                signFlag = 0;

            if ((statusdata & 0x40) == 0x40)
                overflowFlag = 1;
            else
                overflowFlag = 0;

            if ((statusdata & 0x10) == 0x10)
                brkFlag = 1;
            else
                brkFlag = 0;

            if ((statusdata & 0x8) == 0x8)
                decimalFlag = 1;
            else
                decimalFlag = 0;

            if ((statusdata & 0x4) == 0x4)
                interruptFlag = 1;
            else
                interruptFlag = 0;

            if ((statusdata & 0x2) == 0x2)
                zeroFlag = 1;
            else
                zeroFlag = 0;

            if ((statusdata & 0x1) == 0x1)
                carryFlag = 1;
            else
                carryFlag = 0;

        }
        //---------------------------
        //START: Main opcode section
        //---------------------------

        public void OpcodeAdc()
        {
            // We may not use both, but it's easier to grab them now
            byte arg1 = myEngine.ReadMemory8((ushort)(pcRegister + 1));
            byte arg2 = myEngine.ReadMemory8((ushort)(pcRegister + 2));
            byte valueholder = 0xff;

            //Decode
            switch (currentOpcode)
            {
                case (0x69): valueholder = arg1; break;
                case (0x65): valueholder = ZeroPage(arg1); break;
                case (0x75): valueholder = ZeroPageX(arg1); break;
                case (0x6D): valueholder = Absolute(arg1, arg2); break;
                case (0x7D): valueholder = AbsoluteX(arg1, arg2, true); break;
                case (0x79): valueholder = AbsoluteY(arg1, arg2, true); break;
                case (0x61): valueholder = IndirectX(arg1); break;
                case (0x71): valueholder = IndirectY(arg1, true); break;
                default: myEngine.IsQuitting = true; Console.WriteLine("Broken ADC"); break;
            }

            //Execute
            uint valueholder32;
            valueholder32 = (uint)(aRegister + valueholder + carryFlag);
            //valueholder32 = (uint)(a_register + valueholder);
            if (valueholder32 > 255)
            {
                carryFlag = 1;
                overflowFlag = 1;
            }
            else
            {
                carryFlag = 0;
                overflowFlag = 0;
            }
            if ((valueholder32 & 0xff) == 0)
                zeroFlag = 1;
            else
                zeroFlag = 0;

            if ((valueholder32 & 0x80) == 0x80)
                signFlag = 1;
            else
                signFlag = 0;

            aRegister = (byte)(valueholder32 & 0xff);

            //Advance PC and tick count
            //FIXME: X and Y index overflow tick
            switch (currentOpcode)
            {
                case (0x69): tickCount += 2; pcRegister += 2; break;
                case (0x65): tickCount += 3; pcRegister += 2; break;
                case (0x75): tickCount += 4; pcRegister += 2; break;
                case (0x6D): tickCount += 4; pcRegister += 3; break;
                case (0x7D): tickCount += 4; pcRegister += 3; break;
                case (0x79): tickCount += 4; pcRegister += 3; break;
                case (0x61): tickCount += 6; pcRegister += 2; break;
                case (0x71): tickCount += 5; pcRegister += 2; break;
                default: myEngine.IsQuitting = true; Console.WriteLine("Broken ADC"); break;
            }
        }
        public void OpcodeAnd()
        {
            // We may not use both, but it's easier to grab them now
            byte arg1 = myEngine.ReadMemory8((ushort)(pcRegister + 1));
            byte arg2 = myEngine.ReadMemory8((ushort)(pcRegister + 2));
            byte valueholder = 0xff;

            switch (currentOpcode)
            {
                case (0x29): valueholder = arg1; break;
                case (0x25): valueholder = ZeroPage(arg1); break;
                case (0x35): valueholder = ZeroPageX(arg1); break;
                case (0x2D): valueholder = Absolute(arg1, arg2); break;
                case (0x3D): valueholder = AbsoluteX(arg1, arg2, true); break;
                case (0x39): valueholder = AbsoluteY(arg1, arg2, true); break;
                case (0x21): valueholder = IndirectX(arg1); break;
                case (0x31): valueholder = IndirectY(arg1, false); break;
                default: myEngine.IsQuitting = true; Console.WriteLine("Broken AND"); break;
            }

            aRegister = (byte)(aRegister & valueholder);
            if ((aRegister & 0xff) == 0)
                zeroFlag = 1;
            else
                zeroFlag = 0;

            if ((aRegister & 0x80) == 0x80)
                signFlag = 1;
            else
                signFlag = 0;

            switch (currentOpcode)
            {
                case (0x29): tickCount += 2; pcRegister += 2; break;
                case (0x25): tickCount += 3; pcRegister += 2; break;
                case (0x35): tickCount += 4; pcRegister += 2; break;
                case (0x2D): tickCount += 3; pcRegister += 3; break;
                case (0x3D): tickCount += 4; pcRegister += 3; break;
                case (0x39): tickCount += 4; pcRegister += 3; break;
                case (0x21): tickCount += 6; pcRegister += 2; break;
                case (0x31): tickCount += 5; pcRegister += 2; break;
                default: myEngine.IsQuitting = true; Console.WriteLine("Broken AND"); break;
            }
        }

        public void OpcodeAsl()
        {
            // We may not use both, but it's easier to grab them now
            byte arg1 = myEngine.ReadMemory8((ushort)(pcRegister + 1));
            byte arg2 = myEngine.ReadMemory8((ushort)(pcRegister + 2));
            byte valueholder = 0xff;

            switch (currentOpcode)
            {
                case (0x0a): valueholder = aRegister; break;
                case (0x06): valueholder = ZeroPage(arg1); break;
                case (0x16): valueholder = ZeroPageX(arg1); break;
                case (0x0E): valueholder = Absolute(arg1, arg2); break;
                case (0x1E): valueholder = AbsoluteX(arg1, arg2, false); break;
                default: myEngine.IsQuitting = true; Console.WriteLine("Broken ASL"); break;
            }
            if ((valueholder & 0x80) == 0x80)
                carryFlag = 1;
            else
                carryFlag = 0;

            valueholder = (byte)(valueholder << 1);

            if ((valueholder & 0xff) == 0x0)
                zeroFlag = 1;
            else
                zeroFlag = 0;

            if ((valueholder & 0x80) == 0x80)
                signFlag = 1;
            else
                signFlag = 0;

            //This one is a little different because we actually need
            //to do more than incrementing in the last step		
            switch (currentOpcode)
            {
                case (0x0a): aRegister = valueholder;
                    tickCount += 2; pcRegister += 1; break;
                case (0x06): ZeroPageWrite(arg1, valueholder);
                    tickCount += 5; pcRegister += 2; break;
                case (0x16): ZeroPageXWrite(arg1, valueholder);
                    tickCount += 6; pcRegister += 2; break;
                case (0x0E): AbsoluteWrite(arg1, arg2, valueholder);
                    tickCount += 6; pcRegister += 3; break;
                case (0x1E): AbsoluteXWrite(arg1, arg2, valueholder);
                    tickCount += 7; pcRegister += 3; break;
                default: myEngine.IsQuitting = true; Console.WriteLine("Broken ASL"); break;
            }
        }

        public void OpcodeBcc()
        {
            byte arg1 = myEngine.ReadMemory8((ushort)(pcRegister + 1));

            //FIX ME: Branching to a new page takes a 1 tick penalty
            if (carryFlag == 0)
            {
                pcRegister += 2;
                if ((pcRegister & 0xFF00) != ((pcRegister + (sbyte)arg1 + 2) & 0xFF00))
                {
                    tickCount += 1;
                }
                pcRegister = (ushort)(pcRegister + (sbyte)arg1);
                tickCount += 1;
            }
            else
            {
                pcRegister += 2;
            }
            tickCount += 2;
        }

        public void OpcodeBcs()
        {
            byte arg1 = myEngine.ReadMemory8((ushort)(pcRegister + 1));

            //FIX ME: Branching to a new page takes a 1 tick penalty
            if (carryFlag == 1)
            {
                pcRegister += 2;
                if ((pcRegister & 0xFF00) != ((pcRegister + (sbyte)arg1 + 2) & 0xFF00))
                {
                    tickCount += 1;
                }
                pcRegister = (ushort)(pcRegister + (sbyte)arg1);
                tickCount += 1;
            }
            else
            {
                pcRegister += 2;
            }
            tickCount += 2;
        }

        public void OpcodeBeq()
        {
            byte arg1 = myEngine.ReadMemory8((ushort)(pcRegister + 1));

            //FIX ME: Branching to a new page takes a 1 tick penalty
            if (zeroFlag == 1)
            {
                pcRegister += 2;
                if ((pcRegister & 0xFF00) != ((pcRegister + (sbyte)arg1 + 2) & 0xFF00))
                {
                    tickCount += 1;
                }
                pcRegister = (ushort)(pcRegister + (sbyte)arg1);
                tickCount += 1;
            }
            else
            {
                pcRegister += 2;
            }
            tickCount += 2;
        }

        public void OpcodeBit()
        {
            // We may not use both, but it's easier to grab them now
            byte arg1 = myEngine.ReadMemory8((ushort)(pcRegister + 1));
            byte arg2 = myEngine.ReadMemory8((ushort)(pcRegister + 2));
            byte valueholder = 0xff;

            switch (currentOpcode)
            {
                case (0x24): valueholder = ZeroPage(arg1); break;
                case (0x2c): valueholder = Absolute(arg1, arg2); break;
                default: myEngine.IsQuitting = true; Console.WriteLine("Broken BIT"); break;
            }

            if ((aRegister & valueholder) == 0x0)
                zeroFlag = 1;
            else
                zeroFlag = 0;

            if ((valueholder & 0x80) == 0x80)
                signFlag = 1;
            else
                signFlag = 0;

            if ((valueholder & 0x40) == 0x40)
                overflowFlag = 1;
            else
                overflowFlag = 0;

            switch (currentOpcode)
            {
                case (0x24): tickCount += 3; pcRegister += 2; break;
                case (0x2c): tickCount += 4; pcRegister += 3; break;
                default: myEngine.IsQuitting = true; Console.WriteLine("Broken BIT"); break;
            }
        }

        public void OpcodeBmi()
        {
            byte arg1 = myEngine.ReadMemory8((ushort)(pcRegister + 1));

            //FIX ME: Branching to a new page takes a 1 tick penalty
            if (signFlag == 1)
            {
                pcRegister += 2;
                if ((pcRegister & 0xFF00) != ((pcRegister + (sbyte)arg1 + 2) & 0xFF00))
                {
                    tickCount += 1;
                }
                pcRegister = (ushort)(pcRegister + (sbyte)arg1);
                tickCount += 1;
            }
            else
            {
                pcRegister += 2;
            }
            tickCount += 2;
        }

        public void OpcodeBne()
        {
            byte arg1;

            //FIX ME: All these are set "wrong" to match the old emulator
            //FIXME: They should probably all be corrected when debugging is finished 
            if (zeroFlag == 0)
            {
                arg1 = myEngine.ReadMemory8((ushort)(pcRegister + 1));
                pcRegister += 2;
                if ((pcRegister & 0xFF00) != ((pcRegister + (sbyte)arg1 + 2) & 0xFF00))
                {
                    tickCount += 1;
                }
                pcRegister = (ushort)(pcRegister + (sbyte)arg1);
                tickCount += 1;
            }
            else
            {
                pcRegister += 2;
            }
            tickCount += 2;
        }

        public void OpcodeBpl()
        {
            byte arg1 = myEngine.ReadMemory8((ushort)(pcRegister + 1));

            //FIX ME: Branching to a new page takes a 1 tick penalty
            if (signFlag == 0)
            {
                pcRegister += 2;
                if ((pcRegister & 0xFF00) != ((pcRegister + (sbyte)arg1 + 2) & 0xFF00))
                {
                    tickCount += 1;
                }
                pcRegister = (ushort)(pcRegister + (sbyte)arg1);
                tickCount += 1;
            }
            else
            {
                pcRegister += 2;
            }
            tickCount += 2;
        }

        public void OpcodeBrk()
        {
            pcRegister = (ushort)(pcRegister + 2);
            Push16(pcRegister);
            brkFlag = 1;
            PushStatus();
            interruptFlag = 1;
            pcRegister = myEngine.ReadMemory16((ushort)0xfffe);
            tickCount += 7;
        }

        public void OpcodeBvc()
        {
            byte arg1 = myEngine.ReadMemory8((ushort)(pcRegister + 1));

            //FIX ME: Branching to a new page takes a 1 tick penalty
            if (overflowFlag == 0)
            {
                pcRegister += 2;
                if ((pcRegister & 0xFF00) != ((pcRegister + (sbyte)arg1 + 2) & 0xFF00))
                {
                    tickCount += 1;
                }
                pcRegister = (ushort)(pcRegister + (sbyte)arg1);
                tickCount += 1;
            }
            else
            {
                pcRegister += 2;
            }
            tickCount += 2;
        }

        public void OpcodeBvs()
        {
            byte arg1 = myEngine.ReadMemory8((ushort)(pcRegister + 1));

            //FIX ME: Branching to a new page takes a 1 tick penalty
            if (overflowFlag == 1)
            {
                pcRegister += 2;
                if ((pcRegister & 0xFF00) != ((pcRegister + (sbyte)arg1 + 2) & 0xFF00))
                {
                    tickCount += 1;
                }
                pcRegister = (ushort)(pcRegister + (sbyte)arg1);
                tickCount += 1;
            }
            else
            {
                pcRegister += 2;
            }
            tickCount += 2;
        }

        public void OpcodeClc()
        {
            carryFlag = 0;
            pcRegister += 1;
            tickCount += 2;
        }

        public void OpcodeCld()
        {
            decimalFlag = 0;
            pcRegister += 1;
            tickCount += 2;
        }

        public void OpcodeCli()
        {
            interruptFlag = 0;
            pcRegister += 1;
            tickCount += 2;
        }

        public void OpcodeClv()
        {
            overflowFlag = 0;
            pcRegister += 1;
            tickCount += 2;
        }

        public void OpcodeCmp()
        {
            // We may not use both, but it's easier to grab them now
            byte arg1 = myEngine.ReadMemory8((ushort)(pcRegister + 1));
            byte arg2 = myEngine.ReadMemory8((ushort)(pcRegister + 2));
            byte valueholder = 0xff;

            switch (currentOpcode)
            {
                case (0xC9): valueholder = arg1; break;
                case (0xC5): valueholder = ZeroPage(arg1); break;
                case (0xD5): valueholder = ZeroPageX(arg1); break;
                case (0xCD): valueholder = Absolute(arg1, arg2); break;
                case (0xDD): valueholder = AbsoluteX(arg1, arg2, true); break;
                case (0xD9): valueholder = AbsoluteY(arg1, arg2, true); break;
                case (0xC1): valueholder = IndirectX(arg1); break;
                case (0xD1): valueholder = IndirectY(arg1, true); break;
                default: myEngine.IsQuitting = true; Console.WriteLine("Broken CMP"); break;
            }
            if (aRegister >= valueholder)
                carryFlag = 1;
            else
                carryFlag = 0;

            valueholder = (byte)(aRegister - valueholder);

            if (valueholder == 0)
                zeroFlag = 1;
            else
                zeroFlag = 0;

            if ((valueholder & 0x80) == 0x80)
                signFlag = 1;
            else
                signFlag = 0;

            //FIXME: X and Y index overflow tick
            switch (currentOpcode)
            {
                case (0xC9): tickCount += 2; pcRegister += 2; break;
                case (0xC5): tickCount += 3; pcRegister += 2; break;
                case (0xD5): tickCount += 4; pcRegister += 2; break;
                case (0xCD): tickCount += 4; pcRegister += 3; break;
                case (0xDD): tickCount += 4; pcRegister += 3; break;
                case (0xD9): tickCount += 4; pcRegister += 3; break;
                case (0xC1): tickCount += 6; pcRegister += 2; break;
                case (0xD1): tickCount += 5; pcRegister += 2; break;
                default: myEngine.IsQuitting = true; Console.WriteLine("Broken CMP"); break;
            }
        }
        public void OpcodeCpx()
        {
            // We may not use both, but it's easier to grab them now
            byte arg1 = myEngine.ReadMemory8((ushort)(pcRegister + 1));
            byte arg2 = myEngine.ReadMemory8((ushort)(pcRegister + 2));
            byte valueholder = 0xff;

            switch (currentOpcode)
            {
                case (0xE0): valueholder = arg1; break;
                case (0xE4): valueholder = ZeroPage(arg1); break;
                case (0xEC): valueholder = Absolute(arg1, arg2); break;
                default: myEngine.IsQuitting = true; Console.WriteLine("Broken CPX"); break;
            }

            if (xIndexRegister >= valueholder)
                carryFlag = 1;
            else
                carryFlag = 0;

            valueholder = (byte)(xIndexRegister - valueholder);

            if (valueholder == 0)
                zeroFlag = 1;
            else
                zeroFlag = 0;

            if ((valueholder & 0x80) == 0x80)
                signFlag = 1;
            else
                signFlag = 0;

            switch (currentOpcode)
            {
                case (0xE0): tickCount += 2; pcRegister += 2; break;
                case (0xE4): tickCount += 3; pcRegister += 2; break;
                case (0xEC): tickCount += 4; pcRegister += 3; break;
                default: myEngine.IsQuitting = true; Console.WriteLine("Broken CPX"); break;
            }
        }
        public void OpcodeCpy()
        {
            // We may not use both, but it's easier to grab them now
            byte arg1 = myEngine.ReadMemory8((ushort)(pcRegister + 1));
            byte arg2 = myEngine.ReadMemory8((ushort)(pcRegister + 2));
            byte valueholder = 0xff;

            switch (currentOpcode)
            {
                case (0xC0): valueholder = arg1; break;
                case (0xC4): valueholder = ZeroPage(arg1); break;
                case (0xCC): valueholder = Absolute(arg1, arg2); break;
                default: myEngine.IsQuitting = true; Console.WriteLine("Broken CPY"); break;
            }

            if (yIndexRegister >= valueholder)
                carryFlag = 1;
            else
                carryFlag = 0;

            valueholder = (byte)(yIndexRegister - valueholder);

            if (valueholder == 0)
                zeroFlag = 1;
            else
                zeroFlag = 0;

            if ((valueholder & 0x80) == 0x80)
                signFlag = 1;
            else
                signFlag = 0;

            switch (currentOpcode)
            {
                case (0xC0): tickCount += 2; pcRegister += 2; break;
                case (0xC4): tickCount += 3; pcRegister += 2; break;
                case (0xCC): tickCount += 4; pcRegister += 3; break;
                default: myEngine.IsQuitting = true; Console.WriteLine("Broken CPY"); break;
            }
        }

        public void OpcodeDec()
        {
            // We may not use both, but it's easier to grab them now
            byte arg1 = myEngine.ReadMemory8((ushort)(pcRegister + 1));
            byte arg2 = myEngine.ReadMemory8((ushort)(pcRegister + 2));
            byte valueholder = 0xff;

            switch (currentOpcode)
            {
                //case (0xCA): valueholder = a_register; break;
                case (0xC6): valueholder = ZeroPage(arg1); break;
                case (0xD6): valueholder = ZeroPageX(arg1); break;
                case (0xCE): valueholder = Absolute(arg1, arg2); break;
                case (0xDE): valueholder = AbsoluteX(arg1, arg2, false); break;
                default: myEngine.IsQuitting = true; Console.WriteLine("Broken DEC"); break;
            }

            valueholder--;

            if ((valueholder & 0xff) == 0x0)
                zeroFlag = 1;
            else
                zeroFlag = 0;

            if ((valueholder & 0x80) == 0x80)
                signFlag = 1;
            else
                signFlag = 0;

            //This one is a little different because we actually need
            //to do more than incrementing in the last step		
            switch (currentOpcode)
            {
                //case (0xCA): a_register = valueholder; 
                //	tick_count += 2; pc_register += 1; break;
                case (0xC6): ZeroPageWrite(arg1, valueholder);
                    tickCount += 5; pcRegister += 2; break;
                case (0xD6): ZeroPageXWrite(arg1, valueholder);
                    tickCount += 6; pcRegister += 2; break;
                case (0xCE): AbsoluteWrite(arg1, arg2, valueholder);
                    tickCount += 6; pcRegister += 3; break;
                case (0xDE): AbsoluteXWrite(arg1, arg2, valueholder);
                    tickCount += 7; pcRegister += 3; break;
                default: myEngine.IsQuitting = true; Console.WriteLine("Broken DEC"); break;
            }
        }
        public void OpcodeDex()
        {
            xIndexRegister--;

            if ((xIndexRegister & 0xff) == 0x0)
                zeroFlag = 1;
            else
                zeroFlag = 0;

            if ((xIndexRegister & 0x80) == 0x80)
                signFlag = 1;
            else
                signFlag = 0;

            pcRegister++;
            tickCount += 2;
        }
        public void OpcodeDey()
        {
            yIndexRegister--;

            if ((yIndexRegister & 0xff) == 0x0)
                zeroFlag = 1;
            else
                zeroFlag = 0;

            if ((yIndexRegister & 0x80) == 0x80)
                signFlag = 1;
            else
                signFlag = 0;

            pcRegister++;
            tickCount += 2;
        }
        public void OpcodeEor()
        {
            // We may not use both, but it's easier to grab them now
            byte arg1 = myEngine.ReadMemory8((ushort)(pcRegister + 1));
            byte arg2 = myEngine.ReadMemory8((ushort)(pcRegister + 2));
            byte valueholder = 0xff;

            switch (currentOpcode)
            {
                case (0x49): valueholder = arg1; break;
                case (0x45): valueholder = ZeroPage(arg1); break;
                case (0x55): valueholder = ZeroPageX(arg1); break;
                case (0x4D): valueholder = Absolute(arg1, arg2); break;
                case (0x5D): valueholder = AbsoluteX(arg1, arg2, true); break;
                case (0x59): valueholder = AbsoluteY(arg1, arg2, true); break;
                case (0x41): valueholder = IndirectX(arg1); break;
                case (0x51): valueholder = IndirectY(arg1, true); break;
                default: myEngine.IsQuitting = true; Console.WriteLine("Broken EOR"); break;
            }

            aRegister = (byte)(aRegister ^ valueholder);
            if ((aRegister & 0xff) == 0)
                zeroFlag = 1;
            else
                zeroFlag = 0;

            if ((aRegister & 0x80) == 0x80)
                signFlag = 1;
            else
                signFlag = 0;

            switch (currentOpcode)
            {
                case (0x49): tickCount += 2; pcRegister += 2; break;
                case (0x45): tickCount += 3; pcRegister += 2; break;
                case (0x55): tickCount += 4; pcRegister += 2; break;
                case (0x4D): tickCount += 3; pcRegister += 3; break;
                case (0x5D): tickCount += 4; pcRegister += 3; break;
                case (0x59): tickCount += 4; pcRegister += 3; break;
                case (0x41): tickCount += 6; pcRegister += 2; break;
                case (0x51): tickCount += 5; pcRegister += 2; break;
                default: myEngine.IsQuitting = true; Console.WriteLine("Broken EOR"); break;
            }
        }

        public void OpcodeInc()
        {
            // We may not use both, but it's easier to grab them now
            byte arg1 = myEngine.ReadMemory8((ushort)(pcRegister + 1));
            byte arg2 = myEngine.ReadMemory8((ushort)(pcRegister + 2));
            byte valueholder = 0xff;

            switch (currentOpcode)
            {
                //case (0xCA): valueholder = a_register; break;
                case (0xE6): valueholder = ZeroPage(arg1); break;
                case (0xF6): valueholder = ZeroPageX(arg1); break;
                case (0xEE): valueholder = Absolute(arg1, arg2); break;
                case (0xFE): valueholder = AbsoluteX(arg1, arg2, false); break;
                default: myEngine.IsQuitting = true; Console.WriteLine("Broken INC"); break;
            }
            valueholder++;

            if ((valueholder & 0xff) == 0x0)
                zeroFlag = 1;
            else
                zeroFlag = 0;

            if ((valueholder & 0x80) == 0x80)
                signFlag = 1;
            else
                signFlag = 0;

            //This one is a little different because we actually need
            //to do more than incrementing in the last step		
            switch (currentOpcode)
            {
                //case (0xCA): a_register = valueholder; 
                //	tick_count += 2; pc_register += 1; break;
                case (0xE6): ZeroPageWrite(arg1, valueholder);
                    tickCount += 5; pcRegister += 2; break;
                case (0xF6): ZeroPageXWrite(arg1, valueholder);
                    tickCount += 6; pcRegister += 2; break;
                case (0xEE): AbsoluteWrite(arg1, arg2, valueholder);
                    tickCount += 6; pcRegister += 3; break;
                case (0xFE): AbsoluteXWrite(arg1, arg2, valueholder);
                    tickCount += 7; pcRegister += 3; break;
                default: myEngine.IsQuitting = true; Console.WriteLine("Broken INC"); break;
            }
        }
        public void OpcodeInx()
        {
            xIndexRegister++;

            if ((xIndexRegister & 0xff) == 0x0)
                zeroFlag = 1;
            else
                zeroFlag = 0;

            if ((xIndexRegister & 0x80) == 0x80)
                signFlag = 1;
            else
                signFlag = 0;

            pcRegister++;
            tickCount += 2;
        }
        public void OpcodeIny()
        {
            yIndexRegister++;

            if ((yIndexRegister & 0xff) == 0x0)
                zeroFlag = 1;
            else
                zeroFlag = 0;

            if ((yIndexRegister & 0x80) == 0x80)
                signFlag = 1;
            else
                signFlag = 0;

            pcRegister++;
            tickCount += 2;
        }
        public void OpcodeJmp()
        {
            //byte arg1 = myEngine.ReadMemory8((ushort)(pc_register + 1));
            //byte arg2 = myEngine.ReadMemory8((ushort)(pc_register + 2));
            ushort myAddress = myEngine.ReadMemory16((ushort)(pcRegister + 1));

            switch (currentOpcode)
            {
                case (0x4c): //pc_register = MakeAddress(arg1, arg2); 
                    pcRegister = myAddress;
                    //Console.WriteLine("Jumping to: {0:x}", pc_register);
                    tickCount += 3; break;
                case (0x6c): //pc_register = myEngine.ReadMemory16(MakeAddress(arg1, arg2));
                    pcRegister = myEngine.ReadMemory16(myAddress);
                    //Console.WriteLine("Jumping to: {0:x}", pc_register);
                    tickCount += 5; break;
                default: myEngine.IsQuitting = true; Console.WriteLine("Broken JMP"); break;
            }
        }
        public void OpcodeJsr()
        {
            byte arg1 = myEngine.ReadMemory8((ushort)(pcRegister + 1));
            byte arg2 = myEngine.ReadMemory8((ushort)(pcRegister + 2));
            Push16((ushort)(pcRegister + 2));
            pcRegister = MakeAddress(arg1, arg2);
            tickCount += 6;
        }
        public void OpcodeLda()
        {
            // We may not use both, but it's easier to grab them now
            byte arg1 = myEngine.ReadMemory8((ushort)(pcRegister + 1));
            byte arg2;
            //byte valueholder = 0xff;
            //FIXME: X and Y index overflow tick
            switch (currentOpcode)
            {
                case (0xA9): aRegister = arg1;
                    tickCount += 2; pcRegister += 2; break;
                case (0xA5): aRegister = ZeroPage(arg1);
                    tickCount += 3; pcRegister += 2; break;
                case (0xB5): aRegister = ZeroPageX(arg1);
                    tickCount += 4; pcRegister += 2; break;
                case (0xAD): arg2 = myEngine.ReadMemory8((ushort)(pcRegister + 2));
                    aRegister = Absolute(arg1, arg2);
                    tickCount += 4; pcRegister += 3; break;
                case (0xBD): arg2 = myEngine.ReadMemory8((ushort)(pcRegister + 2));
                    aRegister = AbsoluteX(arg1, arg2, true);     //CHECK FOR PAGE BOUNDARIES


                    tickCount += 4; pcRegister += 3; break;
                case (0xB9): arg2 = myEngine.ReadMemory8((ushort)(pcRegister + 2));
                    aRegister = AbsoluteY(arg1, arg2, true);     //CHECK FOR PAGE BOUNDARIES
                    tickCount += 4; pcRegister += 3; break;
                case (0xA1): aRegister = IndirectX(arg1);
                    tickCount += 6; pcRegister += 2; break;
                case (0xB1): aRegister = IndirectY(arg1, true);     //CHECK FOR PAGE BOUNDARIES
                    tickCount += 5; pcRegister += 2; break;
                default: myEngine.IsQuitting = true; Console.WriteLine("Broken LDA"); break;
            }

            if (aRegister == 0)
                zeroFlag = 1;
            else
                zeroFlag = 0;

            if ((aRegister & 0x80) == 0x80)
                signFlag = 1;
            else
                signFlag = 0;
        }

        public void OpcodeLdx()
        {
            // We may not use both, but it's easier to grab them now
            byte arg1 = myEngine.ReadMemory8((ushort)(pcRegister + 1));
            byte arg2 = myEngine.ReadMemory8((ushort)(pcRegister + 2));
            //byte valueholder = 0xff;

            //FIXME: X and Y index overflow tick
            switch (currentOpcode)
            {
                case (0xA2): xIndexRegister = arg1;
                    tickCount += 2; pcRegister += 2; break;
                case (0xA6): xIndexRegister = ZeroPage(arg1);
                    tickCount += 3; pcRegister += 2; break;
                case (0xB6): xIndexRegister = ZeroPageY(arg1);
                    tickCount += 4; pcRegister += 2; break;
                case (0xAE): xIndexRegister = Absolute(arg1, arg2);
                    tickCount += 4; pcRegister += 3; break;
                case (0xBE): xIndexRegister = AbsoluteY(arg1, arg2, true);
                    tickCount += 4; pcRegister += 3; break;
                default: myEngine.IsQuitting = true; Console.WriteLine("Broken LDX"); break;
            }

            if (xIndexRegister == 0)
                zeroFlag = 1;
            else
                zeroFlag = 0;

            if ((xIndexRegister & 0x80) == 0x80)
                signFlag = 1;
            else
                signFlag = 0;
        }

        public void OpcodeLdy()
        {
            // We may not use both, but it's easier to grab them now
            byte arg1 = myEngine.ReadMemory8((ushort)(pcRegister + 1));
            byte arg2 = myEngine.ReadMemory8((ushort)(pcRegister + 2));
            //byte valueholder = 0xff;

            //FIXME: X and Y index overflow tick
            switch (currentOpcode)
            {
                case (0xA0): yIndexRegister = arg1;
                    tickCount += 2; pcRegister += 2; break;
                case (0xA4): yIndexRegister = ZeroPage(arg1);
                    tickCount += 3; pcRegister += 2; break;
                case (0xB4): yIndexRegister = ZeroPageX(arg1);
                    tickCount += 4; pcRegister += 2; break;
                case (0xAC): yIndexRegister = Absolute(arg1, arg2);
                    tickCount += 4; pcRegister += 3; break;
                case (0xBC): yIndexRegister = AbsoluteX(arg1, arg2, true);
                    tickCount += 4; pcRegister += 3; break;
                default: myEngine.IsQuitting = true; Console.WriteLine("Broken LDY"); break;
            }

            if (yIndexRegister == 0)
                zeroFlag = 1;
            else
                zeroFlag = 0;

            if ((yIndexRegister & 0x80) == 0x80)
                signFlag = 1;
            else
                signFlag = 0;
        }

        public void OpcodeLsr()
        {
            // We may not use both, but it's easier to grab them now
            byte arg1 = myEngine.ReadMemory8((ushort)(pcRegister + 1));
            byte arg2 = myEngine.ReadMemory8((ushort)(pcRegister + 2));
            byte valueholder = 0xff;

            switch (currentOpcode)
            {
                case (0x4a): valueholder = aRegister; break;
                case (0x46): valueholder = ZeroPage(arg1); break;
                case (0x56): valueholder = ZeroPageX(arg1); break;
                case (0x4E): valueholder = Absolute(arg1, arg2); break;
                case (0x5E): valueholder = AbsoluteX(arg1, arg2, false); break;
                default: myEngine.IsQuitting = true; Console.WriteLine("Broken LSR"); break;
            }
            if ((valueholder & 0x1) == 0x1)
                carryFlag = 1;
            else
                carryFlag = 0;

            valueholder = (byte)(valueholder >> 1);

            if ((valueholder & 0xff) == 0x0)
                zeroFlag = 1;
            else
                zeroFlag = 0;

            if ((valueholder & 0x80) == 0x80)
                signFlag = 1;
            else
                signFlag = 0;

            //This one is a little different because we actually need
            //to do more than incrementing in the last step		
            switch (currentOpcode)
            {
                case (0x4a): aRegister = valueholder;
                    tickCount += 2; pcRegister += 1; break;
                case (0x46): ZeroPageWrite(arg1, valueholder);
                    tickCount += 5; pcRegister += 2; break;
                case (0x56): ZeroPageXWrite(arg1, valueholder);
                    tickCount += 6; pcRegister += 2; break;
                case (0x4E): AbsoluteWrite(arg1, arg2, valueholder);
                    tickCount += 6; pcRegister += 3; break;
                case (0x5E): AbsoluteXWrite(arg1, arg2, valueholder);
                    tickCount += 7; pcRegister += 3; break;
                default: myEngine.IsQuitting = true; Console.WriteLine("Broken LSR"); break;
            }
        }

        public void OpcodeNop()
        {
            if (currentOpcode != 0xEA)
            {
                //Console.WriteLine("Illegal Instruction");
                //myEngine.isQuitting = true;
            }
            pcRegister += 1;
            tickCount += 2;
        }

        public void OpcodeOra()
        {
            // We may not use both, but it's easier to grab them now
            byte arg1 = myEngine.ReadMemory8((ushort)(pcRegister + 1));
            byte arg2 = myEngine.ReadMemory8((ushort)(pcRegister + 2));
            byte valueholder = 0xff;

            switch (currentOpcode)
            {
                case (0x09): valueholder = arg1; break;
                case (0x05): valueholder = ZeroPage(arg1); break;
                case (0x15): valueholder = ZeroPageX(arg1); break;
                case (0x0D): valueholder = Absolute(arg1, arg2); break;
                case (0x1D): valueholder = AbsoluteX(arg1, arg2, true); break;
                case (0x19): valueholder = AbsoluteY(arg1, arg2, true); break;
                case (0x01): valueholder = IndirectX(arg1); break;
                case (0x11): valueholder = IndirectY(arg1, false); break;
                default: myEngine.IsQuitting = true; Console.WriteLine("Broken ORA"); break;
            }

            aRegister = (byte)(aRegister | valueholder);
            if ((aRegister & 0xff) == 0)
                zeroFlag = 1;
            else
                zeroFlag = 0;

            if ((aRegister & 0x80) == 0x80)
                signFlag = 1;
            else
                signFlag = 0;

            //FIXME: X and Y index overflow tick
            switch (currentOpcode)
            {
                case (0x09): tickCount += 2; pcRegister += 2; break;
                case (0x05): tickCount += 3; pcRegister += 2; break;
                case (0x15): tickCount += 4; pcRegister += 2; break;
                case (0x0D): tickCount += 4; pcRegister += 3; break;
                case (0x1D): tickCount += 4; pcRegister += 3; break;
                case (0x19): tickCount += 4; pcRegister += 3; break;
                case (0x01): tickCount += 6; pcRegister += 2; break;
                case (0x11): tickCount += 5; pcRegister += 2; break;
                default: myEngine.IsQuitting = true; Console.WriteLine("Broken ORA"); break;
            }
        }

        public void OpcodePha()
        {
            Push8(aRegister);
            pcRegister += 1;
            tickCount += 3;
        }

        public void OpcodePhp()
        {
            PushStatus();
            pcRegister += 1;
            tickCount += 3;
        }

        public void OpcodePla()
        {
            aRegister = Pull8();
            if ((aRegister & 0xff) == 0)
                zeroFlag = 1;
            else
                zeroFlag = 0;

            if ((aRegister & 0x80) == 0x80)
                signFlag = 1;
            else
                signFlag = 0;
            pcRegister += 1;
            tickCount += 4;
        }

        public void OpcodePlp()
        {
            PullStatus();
            pcRegister += 1;
            tickCount += 4;
        }

        public void OpcodeRol()
        {
            // We may not use both, but it's easier to grab them now
            byte arg1 = myEngine.ReadMemory8((ushort)(pcRegister + 1));
            byte arg2 = myEngine.ReadMemory8((ushort)(pcRegister + 2));
            byte valueholder = 0xff;
            byte bitholder = 0;

            switch (currentOpcode)
            {
                case (0x2a): valueholder = aRegister; break;
                case (0x26): valueholder = ZeroPage(arg1); break;
                case (0x36): valueholder = ZeroPageX(arg1); break;
                case (0x2E): valueholder = Absolute(arg1, arg2); break;
                case (0x3E): valueholder = AbsoluteX(arg1, arg2, false); break;
                default: myEngine.IsQuitting = true; Console.WriteLine("Broken ROL"); break;
            }
            if ((valueholder & 0x80) == 0x80)
                bitholder = 1;
            else
                bitholder = 0;

            valueholder = (byte)(valueholder << 1);
            valueholder = (byte)(valueholder | carryFlag);

            carryFlag = bitholder;

            if ((valueholder & 0xff) == 0x0)
                zeroFlag = 1;
            else
                zeroFlag = 0;

            if ((valueholder & 0x80) == 0x80)
                signFlag = 1;
            else
                signFlag = 0;

            //This one is a little different because we actually need
            //to do more than incrementing in the last step		
            switch (currentOpcode)
            {
                case (0x2a): aRegister = valueholder;
                    tickCount += 2; pcRegister += 1; break;
                case (0x26): ZeroPageWrite(arg1, valueholder);
                    tickCount += 5; pcRegister += 2; break;
                case (0x36): ZeroPageXWrite(arg1, valueholder);
                    tickCount += 6; pcRegister += 2; break;
                case (0x2E): AbsoluteWrite(arg1, arg2, valueholder);
                    tickCount += 6; pcRegister += 3; break;
                case (0x3E): AbsoluteXWrite(arg1, arg2, valueholder);
                    tickCount += 7; pcRegister += 3; break;
                default: myEngine.IsQuitting = true; Console.WriteLine("Broken ROL"); break;
            }
        }

        public void OpcodeRor()
        {
            // We may not use both, but it's easier to grab them now
            byte arg1 = myEngine.ReadMemory8((ushort)(pcRegister + 1));
            byte arg2 = myEngine.ReadMemory8((ushort)(pcRegister + 2));
            byte valueholder = 0xff;
            byte bitholder = 0;

            switch (currentOpcode)
            {
                case (0x6a): valueholder = aRegister; break;
                case (0x66): valueholder = ZeroPage(arg1); break;
                case (0x76): valueholder = ZeroPageX(arg1); break;
                case (0x6E): valueholder = Absolute(arg1, arg2); break;
                case (0x7E): valueholder = AbsoluteX(arg1, arg2, false); break;
                default: myEngine.IsQuitting = true; Console.WriteLine("Broken ROR"); break;
            }

            if ((valueholder & 0x1) == 0x1)
                bitholder = 1;
            else
                bitholder = 0;

            valueholder = (byte)(valueholder >> 1);

            if (carryFlag == 1)
                valueholder = (byte)(valueholder | 0x80);

            carryFlag = bitholder;

            if ((valueholder & 0xff) == 0x0)
                zeroFlag = 1;
            else
                zeroFlag = 0;

            if ((valueholder & 0x80) == 0x80)
                signFlag = 1;
            else
                signFlag = 0;

            //This one is a little different because we actually need
            //to do more than incrementing in the last step		
            switch (currentOpcode)
            {
                case (0x6a): aRegister = valueholder;
                    tickCount += 2; pcRegister += 1; break;
                case (0x66): ZeroPageWrite(arg1, valueholder);
                    tickCount += 5; pcRegister += 2; break;
                case (0x76): ZeroPageXWrite(arg1, valueholder);
                    tickCount += 6; pcRegister += 2; break;
                case (0x6E): AbsoluteWrite(arg1, arg2, valueholder);
                    tickCount += 6; pcRegister += 3; break;
                case (0x7E): AbsoluteXWrite(arg1, arg2, valueholder);
                    tickCount += 7; pcRegister += 3; break;
                default: myEngine.IsQuitting = true; Console.WriteLine("Broken ROR"); break;
            }
        }

        public void OpcodeRti()
        {
            PullStatus();
            pcRegister = Pull16();
            tickCount += 6;
        }

        public void OpcodeRts()
        {
            pcRegister = Pull16();
            tickCount += 6;
            pcRegister += 1;
        }

        public void OpcodeSbc()
        {
            // We may not use both, but it's easier to grab them now
            byte arg1 = myEngine.ReadMemory8((ushort)(pcRegister + 1));
            byte arg2 = myEngine.ReadMemory8((ushort)(pcRegister + 2));
            byte valueholder = 0xff;

            //Decode
            switch (currentOpcode)
            {
                case (0xE9): valueholder = arg1; break;
                case (0xE5): valueholder = ZeroPage(arg1); break;
                case (0xF5): valueholder = ZeroPageX(arg1); break;
                case (0xED): valueholder = Absolute(arg1, arg2); break;
                case (0xFD): valueholder = AbsoluteX(arg1, arg2, true); break;
                case (0xF9): valueholder = AbsoluteY(arg1, arg2, true); break;
                case (0xE1): valueholder = IndirectX(arg1); break;
                case (0xF1): valueholder = IndirectY(arg1, false); break;
                default: myEngine.IsQuitting = true; Console.WriteLine("Broken SBC"); break;
            }

            //Execute
            uint valueholder32;
            valueholder32 = (uint)(aRegister - valueholder);
            if (carryFlag == 0)
                valueholder32 = valueholder32 - 1;

            if (valueholder32 > 255)
            {
                carryFlag = 0;
                overflowFlag = 1;
            }
            else
            {
                carryFlag = 1;
                overflowFlag = 0;
            }
            if ((valueholder32 & 0xff) == 0)
                zeroFlag = 1;
            else
                zeroFlag = 0;

            if ((valueholder32 & 0x80) == 0x80)
                signFlag = 1;
            else
                signFlag = 0;

            aRegister = (byte)(valueholder32 & 0xff);

            //Advance PC and tick count
            //FIXME: X and Y index overflow tick
            switch (currentOpcode)
            {
                case (0xE9): tickCount += 2; pcRegister += 2; break;
                case (0xE5): tickCount += 3; pcRegister += 2; break;
                case (0xF5): tickCount += 4; pcRegister += 2; break;
                case (0xED): tickCount += 4; pcRegister += 3; break;
                case (0xFD): tickCount += 4; pcRegister += 3; break;
                case (0xF9): tickCount += 4; pcRegister += 3; break;
                case (0xE1): tickCount += 6; pcRegister += 2; break;
                case (0xF1): tickCount += 5; pcRegister += 2; break;
                default: myEngine.IsQuitting = true; Console.WriteLine("Broken SBC"); break;
            }
        }

        public void OpcodeSec()
        {
            carryFlag = 1;
            tickCount += 2;
            pcRegister += 1;
        }

        public void OpcodeSed()
        {
            decimalFlag = 1;
            tickCount += 2;
            pcRegister += 1;
        }

        public void OpcodeSei()
        {
            interruptFlag = 1;
            tickCount += 2;
            pcRegister += 1;
        }

        public void OpcodeSta()
        {
            // We may not use both, but it's easier to grab them now
            byte arg1 = myEngine.ReadMemory8((ushort)(pcRegister + 1));
            byte arg2 = myEngine.ReadMemory8((ushort)(pcRegister + 2));
            //byte valueholder = 0xff;

            //Decode
            switch (currentOpcode)
            {
                case (0x85): ZeroPageWrite(arg1, aRegister);
                    tickCount += 3; pcRegister += 2; break;
                case (0x95): ZeroPageXWrite(arg1, aRegister);
                    tickCount += 4; pcRegister += 2; break;
                case (0x8D): AbsoluteWrite(arg1, arg2, aRegister);
                    tickCount += 4; pcRegister += 3; break;
                case (0x9D): AbsoluteXWrite(arg1, arg2, aRegister);
                    tickCount += 5; pcRegister += 3; break;
                case (0x99): AbsoluteYWrite(arg1, arg2, aRegister);
                    tickCount += 5; pcRegister += 3; break;
                case (0x81): IndirectXWrite(arg1, aRegister);
                    tickCount += 6; pcRegister += 2; break;
                case (0x91): IndirectYWrite(arg1, aRegister);
                    tickCount += 6; pcRegister += 2; break;
                default: myEngine.IsQuitting = true; Console.WriteLine("Broken STA"); break;
            }
        }

        public void OpcodeStx()
        {
            // We may not use both, but it's easier to grab them now
            byte arg1 = myEngine.ReadMemory8((ushort)(pcRegister + 1));
            byte arg2 = myEngine.ReadMemory8((ushort)(pcRegister + 2));
            //byte valueholder = 0xff;

            //Decode
            switch (currentOpcode)
            {
                case (0x86): ZeroPageWrite(arg1, xIndexRegister);
                    tickCount += 3; pcRegister += 2; break;
                case (0x96): ZeroPageYWrite(arg1, xIndexRegister);
                    tickCount += 4; pcRegister += 2; break;
                case (0x8E): AbsoluteWrite(arg1, arg2, xIndexRegister);
                    tickCount += 4; pcRegister += 3; break;
                default: myEngine.IsQuitting = true; Console.WriteLine("Broken STX"); break;
            }
        }

        public void OpcodeSty()
        {
            // We may not use both, but it's easier to grab them now
            byte arg1 = myEngine.ReadMemory8((ushort)(pcRegister + 1));
            byte arg2 = myEngine.ReadMemory8((ushort)(pcRegister + 2));
            //byte valueholder = 0xff;

            //Decode
            switch (currentOpcode)
            {
                case (0x84): ZeroPageWrite(arg1, yIndexRegister);
                    tickCount += 3; pcRegister += 2; break;
                case (0x94): ZeroPageXWrite(arg1, yIndexRegister);
                    tickCount += 4; pcRegister += 2; break;
                case (0x8C): AbsoluteWrite(arg1, arg2, yIndexRegister);
                    tickCount += 4; pcRegister += 3; break;
                default: myEngine.IsQuitting = true; Console.WriteLine("Broken STY"); break;
            }
        }

        public void OpcodeTax()
        {
            xIndexRegister = aRegister;

            if (xIndexRegister == 0)
                zeroFlag = 1;
            else
                zeroFlag = 0;

            if ((xIndexRegister & 0x80) == 0x80)
                signFlag = 1;
            else
                signFlag = 0;

            pcRegister += 1;
            tickCount += 2;
        }

        public void OpcodeTay()
        {
            yIndexRegister = aRegister;

            if (yIndexRegister == 0)
                zeroFlag = 1;
            else
                zeroFlag = 0;

            if ((yIndexRegister & 0x80) == 0x80)
                signFlag = 1;
            else
                signFlag = 0;

            pcRegister += 1;
            tickCount += 2;
        }

        public void OpcodeTsx()
        {
            xIndexRegister = spRegister;

            if (xIndexRegister == 0)
                zeroFlag = 1;
            else
                zeroFlag = 0;

            if ((xIndexRegister & 0x80) == 0x80)
                signFlag = 1;
            else
                signFlag = 0;

            pcRegister += 1;
            tickCount += 2;
        }

        public void OpcodeTxa()
        {
            aRegister = xIndexRegister;

            if (aRegister == 0)
                zeroFlag = 1;
            else
                zeroFlag = 0;

            if ((aRegister & 0x80) == 0x80)
                signFlag = 1;
            else
                signFlag = 0;

            pcRegister += 1;
            tickCount += 2;
        }

        public void OpcodeTxs()
        {
            spRegister = xIndexRegister;

            pcRegister += 1;
            tickCount += 2;
        }

        public void OpcodeTya()
        {
            aRegister = yIndexRegister;

            if (aRegister == 0)
                zeroFlag = 1;
            else
                zeroFlag = 0;

            if ((aRegister & 0x80) == 0x80)
                signFlag = 1;
            else
                signFlag = 0;

            pcRegister += 1;
            tickCount += 2;
        }
        //DEBUG function
        public void DumpProcessor()
        {
            //int i;
            byte statusdata = 0;

            if (signFlag == 1)
            {
                statusdata = (byte)(statusdata + 0x80);
            }

            if (overflowFlag == 1)
            {
                statusdata = (byte)(statusdata + 0x40);
            }

            //statusdata = (byte)(statusdata + 0x20);

            if (brkFlag == 1)
            {
                statusdata = (byte)(statusdata + 0x10);
            }

            if (decimalFlag == 1)
            {
                statusdata = (byte)(statusdata + 0x8);
            }

            if (interruptFlag == 1)
            {
                statusdata = (byte)(statusdata + 0x4);
            }

            if (zeroFlag == 1)
            {
                statusdata = (byte)(statusdata + 0x2);
            }

            if (carryFlag == 1)
            {
                statusdata = (byte)(statusdata + 0x1);
            }

            Console.Write("A: 0x{0:x}  ", aRegister);
            Console.Write("X: 0x{0:x}  ", xIndexRegister);
            Console.Write("Y: 0x{0:x}  ", yIndexRegister);
            Console.Write("SP: 0x{0:x}  ", 0x100 + spRegister);
            Console.Write("PC: 0x{0:x}  ", pcRegister);

            Console.Write("PC: 0x{0:x} -- {1:x} {2:x} {3:x} -- ", pcRegister,
                (int)myEngine.ReadMemory8((ushort)pcRegister), (int)myEngine.ReadMemory8((ushort)(pcRegister + 1)),
                (int)myEngine.ReadMemory8((ushort)(pcRegister + 2)));

            Console.Write("PrevPC: 0x{0:x}  ", previousPC);

            Console.Write("Status: 0x{0:x}  ", statusdata);
            Console.Write("Ticks: {0}  ", totalTickCount);
            Console.Write("Scanline: {0}  ", myEngine.MyPpu.CurrentScanline);
            /*
            if (sign_flag == 1)
                Console.Write("S");
            else
                Console.Write("-");
		
            if (overflow_flag == 1)
                Console.Write("V");
            else
                Console.Write("-");
		
            //Console.Write("1");
            Console.Write("0");
		
            if (brk_flag == 1)
                Console.Write("B");
            else
                Console.Write("-");
		
            if (decimal_flag == 1)
                Console.Write("D");
            else
                Console.Write("-");
		
            if (interrupt_flag == 1)
                Console.Write("I");
            else
                Console.Write("-");

            if (zero_flag == 1)
                Console.Write("Z");
            else
                Console.Write("-");
		
            if (carry_flag == 1)
                Console.Write("C");
            else
                Console.Write("-");
            */
            Console.WriteLine("");
            /*
            for (i = 0; i < 0x800; i++)
            {
                if ( (i % 0x20) == 0)
                    Console.Write("\n{0:x}: ", i);
                Console.Write("{0:x} ", myEngine.ReadMemory8((ushort)i));
            }
            */
        }
        //End DEBUG function

        public void RunProcessor()
        {
            //uint before_tick_count;

            previousPC = pcRegister;
            //pc_register = myEngine.ReadMemory16(0xFFFC);
            while (!myEngine.IsQuitting)
            {
                currentOpcode = myEngine.ReadMemory8(pcRegister);

                if (myEngine.IsDebugging)
                    DumpProcessor();

                //previousPC = pc_register;
                //before_tick_count = tick_count;

                //Try #3: Optimized Switch, NOPs are default
                if (!myEngine.IsPaused)
                {
                    switch (currentOpcode)
                    {
                        case (0x00): OpcodeBrk(); break;
                        case (0x01): OpcodeOra(); break;
                        case (0x05): OpcodeOra(); break;  //0x05
                        case (0x06): OpcodeAsl(); break;
                        case (0x08): OpcodePhp(); break;
                        case (0x09): OpcodeOra(); break;
                        case (0x0a): OpcodeAsl(); break;
                        case (0x0d): OpcodeOra(); break;
                        case (0x0e): OpcodeAsl(); break;   //0x0E
                        case (0x10): OpcodeBpl(); break;
                        case (0x11): OpcodeOra(); break;
                        case (0x15): OpcodeOra(); break;
                        case (0x16): OpcodeAsl(); break;
                        case (0x18): OpcodeClc(); break;
                        case (0x19): OpcodeOra(); break;
                        case (0x1d): OpcodeOra(); break;
                        case (0x1e): OpcodeAsl(); break;
                        case (0x20): OpcodeJsr(); break;  //0x20
                        case (0x21): OpcodeAnd(); break;
                        case (0x24): OpcodeBit(); break;
                        case (0x25): OpcodeAnd(); break;
                        case (0x26): OpcodeRol(); break;
                        case (0x28): OpcodePlp(); break;
                        case (0x29): OpcodeAnd(); break;  //0x29
                        case (0x2a): OpcodeRol(); break;
                        case (0x2c): OpcodeBit(); break;
                        case (0x2d): OpcodeAnd(); break;
                        case (0x2e): OpcodeRol(); break;
                        case (0x30): OpcodeBmi(); break;
                        case (0x31): OpcodeAnd(); break;
                        case (0x32): OpcodeNop(); break;  //0x32
                        case (0x33): OpcodeNop(); break;
                        case (0x34): OpcodeNop(); break;
                        case (0x35): OpcodeAnd(); break;
                        case (0x36): OpcodeRol(); break;
                        case (0x38): OpcodeSec(); break;
                        case (0x39): OpcodeAnd(); break;
                        case (0x3d): OpcodeAnd(); break;
                        case (0x3e): OpcodeRol(); break;
                        case (0x40): OpcodeRti(); break;
                        case (0x41): OpcodeEor(); break;
                        case (0x45): OpcodeEor(); break;
                        case (0x46): OpcodeLsr(); break;
                        case (0x48): OpcodePha(); break;
                        case (0x49): OpcodeEor(); break;
                        case (0x4a): OpcodeLsr(); break;
                        case (0x4c): OpcodeJmp(); break;
                        case (0x4d): OpcodeEor(); break; //0x4D
                        case (0x4e): OpcodeLsr(); break;
                        case (0x50): OpcodeBvc(); break;
                        case (0x51): OpcodeEor(); break;
                        case (0x55): OpcodeEor(); break;
                        case (0x56): OpcodeLsr(); break; //0x56
                        case (0x58): OpcodeCli(); break;
                        case (0x59): OpcodeEor(); break;
                        case (0x5d): OpcodeEor(); break;
                        case (0x5e): OpcodeLsr(); break;
                        case (0x60): OpcodeRts(); break;
                        case (0x61): OpcodeAdc(); break;
                        case (0x65): OpcodeAdc(); break;
                        case (0x66): OpcodeRor(); break;
                        case (0x68): OpcodePla(); break; //0x68
                        case (0x69): OpcodeAdc(); break;
                        case (0x6a): OpcodeRor(); break;
                        case (0x6c): OpcodeJmp(); break;
                        case (0x6d): OpcodeAdc(); break;
                        case (0x6e): OpcodeRor(); break;
                        case (0x70): OpcodeBvs(); break;
                        case (0x71): OpcodeAdc(); break; //0x71
                        case (0x75): OpcodeAdc(); break;
                        case (0x76): OpcodeRor(); break;
                        case (0x78): OpcodeSei(); break;
                        case (0x79): OpcodeAdc(); break;
                        case (0x7d): OpcodeAdc(); break;
                        case (0x7e): OpcodeRor(); break;
                        case (0x81): OpcodeSta(); break;
                        case (0x84): OpcodeSty(); break;
                        case (0x85): OpcodeSta(); break;
                        case (0x86): OpcodeStx(); break;
                        case (0x88): OpcodeDey(); break;
                        case (0x8a): OpcodeTxa(); break;
                        case (0x8c): OpcodeSty(); break; //0x8C
                        case (0x8d): OpcodeSta(); break;
                        case (0x8e): OpcodeStx(); break;
                        case (0x90): OpcodeBcc(); break;
                        case (0x91): OpcodeSta(); break;
                        case (0x94): OpcodeSty(); break;
                        case (0x95): OpcodeSta(); break; //0x95
                        case (0x96): OpcodeStx(); break;
                        case (0x98): OpcodeTya(); break;
                        case (0x99): OpcodeSta(); break;
                        case (0x9a): OpcodeTxs(); break;
                        case (0x9d): OpcodeSta(); break;
                        case (0xa0): OpcodeLdy(); break;
                        case (0xa1): OpcodeLda(); break;
                        case (0xa2): OpcodeLdx(); break;
                        case (0xa4): OpcodeLdy(); break;
                        case (0xa5): OpcodeLda(); break;
                        case (0xa6): OpcodeLdx(); break;
                        case (0xa8): OpcodeTay(); break;
                        case (0xa9): OpcodeLda(); break;
                        case (0xaa): OpcodeTax(); break;
                        case (0xac): OpcodeLdy(); break;
                        case (0xad): OpcodeLda(); break;
                        case (0xae): OpcodeLdx(); break;
                        case (0xb0): OpcodeBcs(); break; //0xB0
                        case (0xb1): OpcodeLda(); break;
                        case (0xb4): OpcodeLdy(); break;
                        case (0xb5): OpcodeLda(); break;
                        case (0xb6): OpcodeLdx(); break;
                        case (0xb8): OpcodeClv(); break;
                        case (0xb9): OpcodeLda(); break; //0xB9
                        case (0xba): OpcodeTsx(); break;
                        case (0xbc): OpcodeLdy(); break;
                        case (0xbd): OpcodeLda(); break;
                        case (0xbe): OpcodeLdx(); break;
                        case (0xc0): OpcodeCpy(); break;
                        case (0xc1): OpcodeCmp(); break;
                        case (0xc4): OpcodeCpy(); break;
                        case (0xc5): OpcodeCmp(); break;
                        case (0xc6): OpcodeDec(); break;
                        case (0xc8): OpcodeIny(); break;
                        case (0xc9): OpcodeCmp(); break;
                        case (0xca): OpcodeDex(); break;
                        case (0xcc): OpcodeCpy(); break;
                        case (0xcd): OpcodeCmp(); break;
                        case (0xce): OpcodeDec(); break;
                        case (0xd0): OpcodeBne(); break;
                        case (0xd1): OpcodeCmp(); break;
                        case (0xd5): OpcodeCmp(); break;
                        case (0xd6): OpcodeDec(); break;
                        case (0xd8): OpcodeCld(); break;
                        case (0xd9): OpcodeCmp(); break;
                        case (0xdd): OpcodeCmp(); break; //0xDD
                        case (0xde): OpcodeDec(); break;
                        case (0xe0): OpcodeCpx(); break;
                        case (0xe1): OpcodeSbc(); break;
                        case (0xe4): OpcodeCpx(); break;
                        case (0xe5): OpcodeSbc(); break;
                        case (0xe6): OpcodeInc(); break; //0xE6
                        case (0xe8): OpcodeInx(); break;
                        case (0xe9): OpcodeSbc(); break;
                        case (0xec): OpcodeCpx(); break;
                        case (0xed): OpcodeSbc(); break;
                        case (0xee): OpcodeInc(); break;
                        case (0xf0): OpcodeBeq(); break;
                        case (0xf1): OpcodeSbc(); break;
                        case (0xf5): OpcodeSbc(); break;
                        case (0xf6): OpcodeInc(); break;
                        case (0xf8): OpcodeSed(); break; //0xF8
                        case (0xf9): OpcodeSbc(); break;
                        case (0xfd): OpcodeSbc(); break;
                        case (0xfe): OpcodeInc(); break;
                        default: OpcodeNop(); break; //0xFF
                    };
                }
                else
                {
                    Thread.Sleep(100);
                    NesEngine.CheckForEvents();
                }
                //total_tick_count += (tick_count - before_tick_count);

                if (tickCount > NesEngine.TicksPerScanline)
                {
                    myEngine.RenderNextScanline();
                    tickCount = tickCount - NesEngine.TicksPerScanline;
                }
                //Let's call the ReadMemory function optimized for grabbing instructions
                //currentOpcode = myEngine.ReadMemory8PC(pc_register);
            }
        }

        public ProcessorNes6502(NesEngine theEngine)
        {
            myEngine = theEngine;
            //aRegister;
            //xIndexRegister = 0;
            //yIndexRegister = 0;
            spRegister = 0xff;

            //FIXME: this is for debugging
            //totalTickCount = 0;
        }
    }
}
