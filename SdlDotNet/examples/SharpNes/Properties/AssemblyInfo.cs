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
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Resources;
using System.Diagnostics.CodeAnalysis;

// Information about this assembly is defined by the following
// attributes.
//
// change them to the information which is associated with the assembly
// you compile.

[assembly: AssemblyTitle("SharpNES")]
[assembly: AssemblyDescription("An 8-bit NES emulator")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("The SDL.NET Project")]
[assembly: AssemblyProduct("")]
[assembly: AssemblyCopyright("")]
[assembly: AssemblyTrademark("http://cs-sdl.sourceforge.net")]
[assembly: AssemblyCulture("")]
[assembly: ComVisible(false)]
[assembly: CLSCompliant(true)]
[assembly: NeutralResourcesLanguageAttribute("en-US")]

// The assembly version has following format :
//
// Major.Minor.Build.Revision
//
// You can specify all values by your own or you can build default build and revision
// numbers with the '*' character (the default):

[assembly: AssemblyVersion("0.3.*")]

// The following attributes specify the key for the sign of your assembly. See the
// .NET Framework documentation for more information about signing.
// This is not required, if you don't want signing let these attributes like they're.
[assembly: AssemblyDelaySign(false)]
[assembly: AssemblyKeyName("")]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, Flags = SecurityPermissionFlag.Execution)]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, Flags = SecurityPermissionFlag.SkipVerification)]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, Flags = SecurityPermissionFlag.UnmanagedCode)]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "namespace", Target = "SdlDotNetExamples.SharpNes", MessageId = "Nes")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "type", Target = "SdlDotNetExamples.SharpNes.NesCartridge", MessageId = "Nes")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "type", Target = "SdlDotNetExamples.SharpNes.Ppu", MessageId = "Ppu")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.SharpNes.Ppu.RestartPpu():System.Void", MessageId = "Ppu")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "type", Target = "SdlDotNetExamples.SharpNes.NesEngine", MessageId = "Nes")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.SharpNes.Ppu.NesPalette", MessageId = "Nes")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.SharpNes.NesCartridge.ChrRom", MessageId = "Chr")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.SharpNes.NesCartridge.PrgRomPages", MessageId = "Prg")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.SharpNes.NesCartridge.ChrRomPages", MessageId = "Chr")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.SharpNes.NesCartridge.PrgRom", MessageId = "Prg")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.SharpNes.NesCartridge.IsVram", MessageId = "Vram")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.SharpNes.Ppu.RenderNextScanline():System.Boolean", MessageId = "Scanline")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.SharpNes.Ppu.VramAddressRegister1Write(System.Byte):System.Void", MessageId = "Vram")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.SharpNes.Ppu.VramIORegisterWrite(System.Byte):System.Void", MessageId = "Vram")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.SharpNes.Ppu.CurrentScanline", MessageId = "Scanline")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.SharpNes.Ppu.VramAddressRegister2Write(System.Byte):System.Void", MessageId = "Vram")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.SharpNes.Ppu.VramIORegisterRead():System.Byte", MessageId = "Vram")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.SharpNes.Ppu.DumpVram():System.Void", MessageId = "Vram")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "type", Target = "SdlDotNetExamples.SharpNes.Joypad", MessageId = "Joypad")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.SharpNes.NesEngine.NumOfInstructions", MessageId = "Num")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.SharpNes.NesEngine.TicksPerScanline", MessageId = "Scanline")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.SharpNes.NesEngine.MyPpu", MessageId = "Ppu")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.SharpNes.EngineBase.RenderNextScanline():System.Void", MessageId = "Scanline")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.SharpNes.EngineBase.LoadCart(System.String,System.String):System.Byte", MessageId = "1#num")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.SharpNes.ProcessorNes6502.OpcodeBcs():System.Void", MessageId = "Bcs")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.SharpNes.ProcessorNes6502.OpcodeRol():System.Void", MessageId = "Rol")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.SharpNes.ProcessorNes6502.OpcodePhp():System.Void", MessageId = "Php")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.SharpNes.ProcessorNes6502.OpcodePlp():System.Void", MessageId = "Plp")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.SharpNes.ProcessorNes6502.OpcodeLda():System.Void", MessageId = "Lda")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.SharpNes.ProcessorNes6502.OpcodeJsr():System.Void", MessageId = "Jsr")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.SharpNes.ProcessorNes6502.BrkFlag", MessageId = "Brk")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.SharpNes.ProcessorNes6502.OpcodeBrk():System.Void", MessageId = "Brk")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.SharpNes.ProcessorNes6502.OpcodeEor():System.Void", MessageId = "Eor")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.SharpNes.ProcessorNes6502.OpcodeStx():System.Void", MessageId = "Stx")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.SharpNes.ProcessorNes6502.OpcodeSed():System.Void", MessageId = "Sed")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.SharpNes.ProcessorNes6502.OpcodeJmp():System.Void", MessageId = "Jmp")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.SharpNes.ProcessorNes6502.OpcodePha():System.Void", MessageId = "Pha")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.SharpNes.ProcessorNes6502.OpcodeBvs():System.Void", MessageId = "Bvs")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.SharpNes.ProcessorNes6502.OpcodeLdx():System.Void", MessageId = "Ldx")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.SharpNes.ProcessorNes6502.OpcodeIny():System.Void", MessageId = "Iny")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.SharpNes.ProcessorNes6502.OpcodeTay():System.Void", MessageId = "Tay")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.SharpNes.ProcessorNes6502.OpcodeLdy():System.Void", MessageId = "Ldy")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.SharpNes.ProcessorNes6502.OpcodeBpl():System.Void", MessageId = "Bpl")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.SharpNes.ProcessorNes6502.OpcodeRts():System.Void", MessageId = "Rts")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.SharpNes.ProcessorNes6502.OpcodeRor():System.Void", MessageId = "Ror")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.SharpNes.ProcessorNes6502.OpcodeLsr():System.Void", MessageId = "Lsr")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.SharpNes.ProcessorNes6502.OpcodeDey():System.Void", MessageId = "Dey")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.SharpNes.ProcessorNes6502.OpcodeTxs():System.Void", MessageId = "Txs")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.SharpNes.ProcessorNes6502.OpcodePla():System.Void", MessageId = "Pla")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.SharpNes.ProcessorNes6502.OpcodeSei():System.Void", MessageId = "Sei")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.SharpNes.ProcessorNes6502.OpcodeCpy():System.Void", MessageId = "Cpy")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.SharpNes.ProcessorNes6502.OpcodeTya():System.Void", MessageId = "Tya")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.SharpNes.ProcessorNes6502.OpcodeCld():System.Void", MessageId = "Cld")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.SharpNes.ProcessorNes6502.OpcodeCmp():System.Void", MessageId = "Cmp")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.SharpNes.ProcessorNes6502.OpcodeOra():System.Void", MessageId = "Ora")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.SharpNes.ProcessorNes6502.OpcodeBeq():System.Void", MessageId = "Beq")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.SharpNes.ProcessorNes6502.OpcodeBvc():System.Void", MessageId = "Bvc")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.SharpNes.ProcessorNes6502.OpcodeBne():System.Void", MessageId = "Bne")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.SharpNes.ProcessorNes6502.OpcodeAsl():System.Void", MessageId = "Asl")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.SharpNes.ProcessorNes6502.OpcodeTsx():System.Void", MessageId = "Tsx")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.SharpNes.ProcessorNes6502.OpcodeClv():System.Void", MessageId = "Clv")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.SharpNes.ProcessorNes6502.OpcodeTxa():System.Void", MessageId = "Txa")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.SharpNes.ProcessorNes6502.OpcodeClc():System.Void", MessageId = "Clc")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.SharpNes.ProcessorNes6502.OpcodeDex():System.Void", MessageId = "Dex")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.SharpNes.ProcessorNes6502.OpcodeRti():System.Void", MessageId = "Rti")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.SharpNes.ProcessorNes6502.OpcodeCpx():System.Void", MessageId = "Cpx")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.SharpNes.ProcessorNes6502.OpcodeCli():System.Void", MessageId = "Cli")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "type", Target = "SdlDotNetExamples.SharpNes.VideoNes", MessageId = "Nes")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.SharpNes.VideoNes.BlitScreen():System.Void", MessageId = "Blit")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.SharpNes.Mapper.ReadPrgRom(System.UInt16):System.Byte", MessageId = "Prg")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.SharpNes.Mapper.CurrentPrgRomPage", MessageId = "Prg")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.SharpNes.Mapper.TimerIrqCount", MessageId = "Irq")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.SharpNes.Mapper.WriteChrRom(System.UInt16,System.Byte):System.Void", MessageId = "Chr")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.SharpNes.Mapper.WritePrgRom(System.UInt16,System.Byte):System.Void", MessageId = "Prg")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.SharpNes.Mapper.CurrentChrRomPage", MessageId = "Chr")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.SharpNes.Mapper.TimerIrqReload", MessageId = "Irq")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.SharpNes.Mapper.ReadChrRom(System.UInt16):System.Byte", MessageId = "Chr")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.SharpNes.Mapper.TimerIrqEnabled", MessageId = "Irq")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "type", Target = "SdlDotNetExamples.SharpNes.SharpNesMain", MessageId = "Nes")]
[module: SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays", Scope = "member", Target = "SdlDotNetExamples.SharpNes.NesCartridge.ChrRom")]
[module: SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays", Scope = "member", Target = "SdlDotNetExamples.SharpNes.NesCartridge.PrgRom")]
[module: SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays", Scope = "member", Target = "SdlDotNetExamples.SharpNes.Ppu.OffScreenBuffer")]
[module: SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays", Scope = "member", Target = "SdlDotNetExamples.SharpNes.Ppu.NameTables")]
[module: SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays", Scope = "member", Target = "SdlDotNetExamples.SharpNes.Ppu.NesPalette")]
[module: SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays", Scope = "member", Target = "SdlDotNetExamples.SharpNes.Mapper.CurrentPrgRomPage")]
[module: SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays", Scope = "member", Target = "SdlDotNetExamples.SharpNes.Mapper.CurrentChrRomPage")]