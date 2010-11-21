#region LICENSE
/*
 * Copyright (C) 2005 David Hudson (jendave@yahoo.com)
 *
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later version.
 * 
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 * 
 * You should have received a copy of the GNU Lesser General Public
 * License along with this library; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
 */
#endregion LICENSE

using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Resources;
using System.Diagnostics.CodeAnalysis;

//
// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
//
[assembly: AssemblyTitle("SDL.NET Examples")]
[assembly: AssemblyDescription("Example of SDL.NET")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("The SDL.NET Project")]
[assembly: AssemblyProduct("")]
[assembly: AssemblyCopyright("")]
[assembly: AssemblyTrademark("http://cs-sdl.sourceforge.net")]
[assembly: AssemblyCulture("")]		
[assembly: ComVisible(false)]
[assembly: CLSCompliant(true)]
[assembly: NeutralResourcesLanguageAttribute("en-US")]

//
// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Revision and Build Numbers 
// by using the '*' as shown below:

[assembly: AssemblyVersion("1.0.*")]

//
// In order to sign your assembly you must specify a key to use. Refer to the 
// Microsoft .NET Framework documentation for more information on assembly signing.
//
// Use the attributes below to control which key is used for signing. 
//
// Notes: 
//   (*) If no key is specified, the assembly is not signed.
//   (*) KeyName refers to a key that has been installed in the Crypto Service
//       Provider (CSP) on your machine. KeyFile refers to a file which contains
//       a key.
//   (*) If the KeyFile and the KeyName values are both specified, the 
//       following processing occurs:
//       (1) If the KeyName can be found in the CSP, that key is used.
//       (2) If the KeyName does not exist and the KeyFile does exist, the key 
//           in the KeyFile is installed into the CSP and used.
//   (*) In order to create a KeyFile, you can use the sn.exe (Strong Name) utility.
//       When specifying the KeyFile, the location of the KeyFile should be
//       relative to the project output directory which is
//       %Project Directory%\obj\<configuration>. For example, if your KeyFile is
//       located in the project directory, you would specify the AssemblyKeyFile 
//       attribute as [assembly: AssemblyKeyFile("..\\..\\mykey.snk")]
//   (*) Delay Signing is an advanced option - see the Microsoft .NET Framework
//       documentation for more information on this.
//
[assembly: AssemblyDelaySign(false)]
[assembly: AssemblyKeyName("")]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, Flags = SecurityPermissionFlag.Execution)]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, Flags = SecurityPermissionFlag.SkipVerification)]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, Flags = SecurityPermissionFlag.UnmanagedCode)]
[module: SuppressMessage("Microsoft.Naming", "CA1706:ShortAcronymsShouldBeUppercase", Scope = "namespace", Target = "SdlDotNetExamples.NeHe")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "type", Target = "SdlDotNetExamples.RedBook.RedBookTessWind", MessageId = "Tess")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "type", Target = "SdlDotNetExamples.RedBook.RedBookTorus", MessageId = "Torus")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.SpriteDemos.MultipleMode.AdjustBoundedViewport(SdlDotNet.Graphics.Sprites.Sprite,SdlDotNet.Graphics.Surface):System.Drawing.Point", MessageId = "Viewport")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "type", Target = "SdlDotNetExamples.RedBook.RedBookTess", MessageId = "Tess")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "type", Target = "SdlDotNetExamples.SpriteDemos.ViewportMode", MessageId = "Viewport")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.SpriteDemos.ViewportMode.AdjustViewport():System.Drawing.Point", MessageId = "Viewport")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.SpriteDemos.ViewportMode.AdjustBoundedViewport():System.Drawing.Point", MessageId = "Viewport")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.SpriteDemos.MultipleMode.AdjustViewport(SdlDotNet.Graphics.Sprites.Sprite,SdlDotNet.Graphics.Surface):System.Drawing.Point", MessageId = "Viewport")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "type", Target = "SdlDotNetExamples.SmallDemos.GtkSprite", MessageId = "Gtk")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "type", Target = "SdlDotNetExamples.SmallDemos.GtkSpriteExample", MessageId = "Gtk")]
[module: SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member", Target = "SdlDotNetExamples.SmallDemos.GtkSpriteExample.spriteCollection")]
[module: SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member", Target = "SdlDotNetExamples.SmallDemos.GtkSpriteExample.GraphWindow")]
[module: SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member", Target = "SdlDotNetExamples.SmallDemos.GtkSpriteExample.vbox1")]
[module: SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member", Target = "SdlDotNetExamples.SmallDemos.GtkSpriteExample.graphView")]
[module: SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member", Target = "SdlDotNetExamples.SmallDemos.GtkSpriteExample.tickTimer")]
[module: SuppressMessage("Microsoft.Mobility", "CA1601:DoNotUseTimersThatPreventPowerStateChanges", Scope = "member", Target = "SdlDotNetExamples.SmallDemos.GtkSpriteExample..ctor()")]
[module: SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Scope = "member", Target = "SdlDotNetExamples.SmallDemos.GtkSpriteExample.initGraphView():System.Void", MessageId = "Member")]
[module: SuppressMessage("Microsoft.Security", "CA2109:ReviewVisibleEventHandlers", Scope = "member", Target = "SdlDotNetExamples.SmallDemos.GtkSpriteExample.OnDeleteWindow(System.Object,Gtk.DeleteEventArgs):System.Void")]
[module: SuppressMessage("Microsoft.Security", "CA2109:ReviewVisibleEventHandlers", Scope = "member", Target = "SdlDotNetExamples.SmallDemos.GtkSpriteExample.OnRedrawTick(System.Object,System.Timers.ElapsedEventArgs):System.Void")]
[module: SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Scope = "member", Target = "SdlDotNetExamples.SmallDemos.GtkSpriteExample.Go():System.Void")]
[module: SuppressMessage("Microsoft.Security", "CA2109:ReviewVisibleEventHandlers", Scope = "member", Target = "SdlDotNetExamples.SmallDemos.GtkSpriteExample.OnKeyPress(System.Object,Gtk.KeyPressEventArgs):System.Void")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "type", Target = "SdlDotNetExamples.LargeDemos.VideoNes", MessageId = "Nes")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.LargeDemos.VideoNes.BlitScreen():System.Void", MessageId = "Blit")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.LargeDemos.EngineBase.RenderNextScanline():System.Void", MessageId = "Scanline")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.LargeDemos.EngineBase.LoadCart(System.String,System.String):System.Byte", MessageId = "1#num")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "type", Target = "SdlDotNetExamples.LargeDemos.Ppu", MessageId = "Ppu")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.LargeDemos.Ppu.RestartPpu():System.Void", MessageId = "Ppu")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.LargeDemos.Ppu.RenderNextScanline():System.Boolean", MessageId = "Scanline")]
[module: SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays", Scope = "member", Target = "SdlDotNetExamples.LargeDemos.Ppu.OffScreenBuffer")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.LargeDemos.Ppu.VramAddressRegister1Write(System.Byte):System.Void", MessageId = "Vram")]
[module: SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays", Scope = "member", Target = "SdlDotNetExamples.LargeDemos.Ppu.NameTables")]
[module: SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays", Scope = "member", Target = "SdlDotNetExamples.LargeDemos.Ppu.NesPalette")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.LargeDemos.Ppu.NesPalette", MessageId = "Nes")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.LargeDemos.Ppu.VramIORegisterWrite(System.Byte):System.Void", MessageId = "Vram")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.LargeDemos.Ppu.CurrentScanline", MessageId = "Scanline")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.LargeDemos.Ppu.VramAddressRegister2Write(System.Byte):System.Void", MessageId = "Vram")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.LargeDemos.Ppu.VramIORegisterRead():System.Byte", MessageId = "Vram")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.LargeDemos.Ppu.DumpVram():System.Void", MessageId = "Vram")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "type", Target = "SdlDotNetExamples.LargeDemos.Joypad", MessageId = "Joypad")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "type", Target = "SdlDotNetExamples.LargeDemos.NesEngine", MessageId = "Nes")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.LargeDemos.NesEngine.NumOfInstructions", MessageId = "Num")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.LargeDemos.NesEngine.TicksPerScanline", MessageId = "Scanline")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.LargeDemos.NesEngine.MyPpu", MessageId = "Ppu")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "type", Target = "SdlDotNetExamples.LargeDemos.NesCartridge", MessageId = "Nes")]
[module: SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays", Scope = "member", Target = "SdlDotNetExamples.LargeDemos.NesCartridge.ChrRom")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.LargeDemos.NesCartridge.ChrRom", MessageId = "Chr")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.LargeDemos.NesCartridge.PrgRomPages", MessageId = "Prg")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.LargeDemos.NesCartridge.ChrRomPages", MessageId = "Chr")]
[module: SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays", Scope = "member", Target = "SdlDotNetExamples.LargeDemos.NesCartridge.PrgRom")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.LargeDemos.NesCartridge.PrgRom", MessageId = "Prg")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.LargeDemos.NesCartridge.IsVram", MessageId = "Vram")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.LargeDemos.ProcessorNes6502.OpcodeBcs():System.Void", MessageId = "Bcs")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.LargeDemos.ProcessorNes6502.OpcodeRol():System.Void", MessageId = "Rol")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.LargeDemos.ProcessorNes6502.OpcodePhp():System.Void", MessageId = "Php")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.LargeDemos.ProcessorNes6502.OpcodePlp():System.Void", MessageId = "Plp")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.LargeDemos.ProcessorNes6502.OpcodeLda():System.Void", MessageId = "Lda")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.LargeDemos.ProcessorNes6502.OpcodeJsr():System.Void", MessageId = "Jsr")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.LargeDemos.ProcessorNes6502.BrkFlag", MessageId = "Brk")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.LargeDemos.ProcessorNes6502.OpcodeBrk():System.Void", MessageId = "Brk")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.LargeDemos.ProcessorNes6502.OpcodeEor():System.Void", MessageId = "Eor")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.LargeDemos.ProcessorNes6502.OpcodeStx():System.Void", MessageId = "Stx")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.LargeDemos.ProcessorNes6502.OpcodeSed():System.Void", MessageId = "Sed")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.LargeDemos.ProcessorNes6502.OpcodeJmp():System.Void", MessageId = "Jmp")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.LargeDemos.ProcessorNes6502.OpcodePha():System.Void", MessageId = "Pha")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.LargeDemos.ProcessorNes6502.OpcodeBvs():System.Void", MessageId = "Bvs")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.LargeDemos.ProcessorNes6502.OpcodeLdx():System.Void", MessageId = "Ldx")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.LargeDemos.ProcessorNes6502.OpcodeIny():System.Void", MessageId = "Iny")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.LargeDemos.ProcessorNes6502.OpcodeTay():System.Void", MessageId = "Tay")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.LargeDemos.ProcessorNes6502.OpcodeLdy():System.Void", MessageId = "Ldy")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.LargeDemos.ProcessorNes6502.OpcodeBpl():System.Void", MessageId = "Bpl")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.LargeDemos.ProcessorNes6502.OpcodeRts():System.Void", MessageId = "Rts")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.LargeDemos.ProcessorNes6502.OpcodeRor():System.Void", MessageId = "Ror")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.LargeDemos.ProcessorNes6502.OpcodeLsr():System.Void", MessageId = "Lsr")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.LargeDemos.ProcessorNes6502.OpcodeDey():System.Void", MessageId = "Dey")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.LargeDemos.ProcessorNes6502.OpcodeTxs():System.Void", MessageId = "Txs")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.LargeDemos.ProcessorNes6502.OpcodePla():System.Void", MessageId = "Pla")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.LargeDemos.ProcessorNes6502.OpcodeSei():System.Void", MessageId = "Sei")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.LargeDemos.ProcessorNes6502.OpcodeCpy():System.Void", MessageId = "Cpy")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.LargeDemos.ProcessorNes6502.OpcodeTya():System.Void", MessageId = "Tya")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.LargeDemos.ProcessorNes6502.OpcodeCld():System.Void", MessageId = "Cld")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.LargeDemos.ProcessorNes6502.OpcodeCmp():System.Void", MessageId = "Cmp")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.LargeDemos.ProcessorNes6502.OpcodeOra():System.Void", MessageId = "Ora")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.LargeDemos.ProcessorNes6502.OpcodeBeq():System.Void", MessageId = "Beq")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.LargeDemos.ProcessorNes6502.OpcodeBvc():System.Void", MessageId = "Bvc")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.LargeDemos.ProcessorNes6502.OpcodeBne():System.Void", MessageId = "Bne")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.LargeDemos.ProcessorNes6502.OpcodeAsl():System.Void", MessageId = "Asl")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.LargeDemos.ProcessorNes6502.OpcodeTsx():System.Void", MessageId = "Tsx")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.LargeDemos.ProcessorNes6502.OpcodeClv():System.Void", MessageId = "Clv")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.LargeDemos.ProcessorNes6502.OpcodeTxa():System.Void", MessageId = "Txa")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.LargeDemos.ProcessorNes6502.OpcodeClc():System.Void", MessageId = "Clc")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.LargeDemos.ProcessorNes6502.OpcodeDex():System.Void", MessageId = "Dex")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.LargeDemos.ProcessorNes6502.OpcodeRti():System.Void", MessageId = "Rti")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.LargeDemos.ProcessorNes6502.OpcodeCpx():System.Void", MessageId = "Cpx")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.LargeDemos.ProcessorNes6502.OpcodeCli():System.Void", MessageId = "Cli")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.LargeDemos.Mapper.ReadPrgRom(System.UInt16):System.Byte", MessageId = "Prg")]
[module: SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays", Scope = "member", Target = "SdlDotNetExamples.LargeDemos.Mapper.CurrentPrgRomPage")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.LargeDemos.Mapper.CurrentPrgRomPage", MessageId = "Prg")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.LargeDemos.Mapper.TimerIrqCount", MessageId = "Irq")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.LargeDemos.Mapper.WriteChrRom(System.UInt16,System.Byte):System.Void", MessageId = "Chr")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.LargeDemos.Mapper.WritePrgRom(System.UInt16,System.Byte):System.Void", MessageId = "Prg")]
[module: SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays", Scope = "member", Target = "SdlDotNetExamples.LargeDemos.Mapper.CurrentChrRomPage")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.LargeDemos.Mapper.CurrentChrRomPage", MessageId = "Chr")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.LargeDemos.Mapper.TimerIrqReload", MessageId = "Irq")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.LargeDemos.Mapper.ReadChrRom(System.UInt16):System.Byte", MessageId = "Chr")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SdlDotNetExamples.LargeDemos.Mapper.TimerIrqEnabled", MessageId = "Irq")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "type", Target = "SdlDotNetExamples.LargeDemos.SharpNesMain", MessageId = "Nes")]