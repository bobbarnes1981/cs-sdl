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

// created on 11/27/2004 at 9:32 AM
using System;
using System.IO;
using SdlDotNet.Input;

namespace SdlDotNetExamples.LargeDemos
{
    public class Joypad
    {
        enum Button
        {
            ButtonA = 1,
            ButtonB = 2,
            ButtonSelect = 4,
            ButtonStart = 8,
            ButtonUp = 16,
            ButtonDown = 32,
            ButtonLeft = 64,
            ButtonRight = 128
        };

        byte joypad1_lastwrite;
        //byte joypad2_lastwrite;
        int joypad1_readpointer;
        //int joypad2_readpointer;

        byte joypad1_state;

        private void InternalGetJoyState()
        {
            //int numberOfKeys;
            //FIXME: This is SDL-centric for the time being

            KeyboardState state = new KeyboardState(true);

            joypad1_state = 0;

            if (state.IsKeyPressed(Key.Z))
            {
                joypad1_state |= (byte)Button.ButtonA;
            }
            if (state.IsKeyPressed(Key.X))
            {
                joypad1_state |= (byte)Button.ButtonB;
            }
            if (state.IsKeyPressed(Key.A))
            {
                joypad1_state |= (byte)Button.ButtonSelect;
            }
            if (state.IsKeyPressed(Key.S))
            {
                joypad1_state |= (byte)Button.ButtonStart;
            }
            if (state.IsKeyPressed(Key.UpArrow))
            {
                joypad1_state |= (byte)Button.ButtonUp;
            }
            else if (state.IsKeyPressed(Key.DownArrow))
            {
                joypad1_state |= (byte)Button.ButtonDown;
            }
            if (state.IsKeyPressed(Key.LeftArrow))
            {
                joypad1_state |= (byte)Button.ButtonLeft;
            }
            else if (state.IsKeyPressed(Key.RightArrow))
            {
                joypad1_state |= (byte)Button.ButtonRight;
            }
        }
        public byte Joypad1Read()
        {
            byte returnedValue = 0;

            switch (joypad1_readpointer)
            {
                case (1): if ((joypad1_state & (byte)Button.ButtonA) == (byte)Button.ButtonA) { returnedValue = 1; }; break;
                case (2): if ((joypad1_state & (byte)Button.ButtonB) == (byte)Button.ButtonB) { returnedValue = 1; }; break;
                case (3): if ((joypad1_state & (byte)Button.ButtonSelect) == (byte)Button.ButtonSelect) { returnedValue = 1; }; break;
                case (4): if ((joypad1_state & (byte)Button.ButtonStart) == (byte)Button.ButtonStart) { returnedValue = 1; }; break;
                case (5): if ((joypad1_state & (byte)Button.ButtonUp) == (byte)Button.ButtonUp) { returnedValue = 1; }; break;
                case (6): if ((joypad1_state & (byte)Button.ButtonDown) == (byte)Button.ButtonDown) { returnedValue = 1; }; break;
                case (7): if ((joypad1_state & (byte)Button.ButtonLeft) == (byte)Button.ButtonLeft) { returnedValue = 1; }; break;
                case (8): if ((joypad1_state & (byte)Button.ButtonRight) == (byte)Button.ButtonRight) { returnedValue = 1; }; break;
            }
            joypad1_readpointer++;
            return returnedValue;
        }
        public static byte Joypad2Read()
        {
            return 0;
        }
        public void Joypad1Write(byte data)
        {
            if ((data == 0) && (joypad1_lastwrite == 1))
            {
                InternalGetJoyState();
                joypad1_readpointer = 1;
            }
            joypad1_lastwrite = data;
        }
        public static void Joypad2Write(/*byte data*/)
        {
        }
    }
}
