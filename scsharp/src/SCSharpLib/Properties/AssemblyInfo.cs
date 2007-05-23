using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Diagnostics.CodeAnalysis;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("SCSharp")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("SCSharp")]
[assembly: AssemblyCopyright("Copyright © ")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]
[assembly: CLSCompliant(true)]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("087ef5f7-4f47-47de-bb33-e1787052c986")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]
[module: SuppressMessage("Microsoft.Naming", "CA1703:ResourceStringsShouldBeSpelledCorrectly", Scope = "resource", Target = "SCSharp.UI.Credits.txt", MessageId = "Toshok")]
[module: SuppressMessage("Microsoft.Naming", "CA1703:ResourceStringsShouldBeSpelledCorrectly", Scope = "resource", Target = "SCSharp.UI.Credits.txt", MessageId = "Ladislav")]
[module: SuppressMessage("Microsoft.Naming", "CA1703:ResourceStringsShouldBeSpelledCorrectly", Scope = "resource", Target = "SCSharp.UI.Credits.txt", MessageId = "Zezula")]
[module: SuppressMessage("Microsoft.Naming", "CA1703:ResourceStringsShouldBeSpelledCorrectly", Scope = "resource", Target = "SCSharp.UI.Credits.txt", MessageId = "Starcraft")]
[module: SuppressMessage("Microsoft.Naming", "CA1703:ResourceStringsShouldBeSpelledCorrectly", Scope = "resource", Target = "SCSharp.UI.Credits.txt", MessageId = "Modding")]
[module: SuppressMessage("Microsoft.Naming", "CA1703:ResourceStringsShouldBeSpelledCorrectly", Scope = "resource", Target = "SCSharp.UI.Credits.txt", MessageId = "Olbrantz")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "namespace", Target = "SCSharp.MpqLib", MessageId = "Mpq")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "type", Target = "SCSharp.MpqLib.MpqContainer", MessageId = "Mpq")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SCSharp.MpqLib.MpqContainer.Add(SCSharp.MpqLib.Mpq):System.Void", MessageId = "0#mpq")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SCSharp.MpqLib.MpqContainer.Remove(SCSharp.MpqLib.Mpq):System.Void", MessageId = "0#mpq")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "type", Target = "SCSharp.MpqLib.MpqHuffman", MessageId = "Mpq")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "type", Target = "SCSharp.MpqLib.MpqStream", MessageId = "Mpq")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "type", Target = "SCSharp.MpqLib.Mpq", MessageId = "Mpq")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "type", Target = "SCSharp.MpqLib.MpqArchiveContainer", MessageId = "Mpq")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "type", Target = "SCSharp.MpqLib.IMpqResource", MessageId = "Mpq")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SCSharp.UI.EntryDialog..ctor(SCSharp.UI.UIScreen,SCSharp.MpqLib.Mpq,System.String)", MessageId = "1#mpq")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SCSharp.UI.GameMenuDialog..ctor(SCSharp.UI.UIScreen,SCSharp.MpqLib.Mpq)", MessageId = "1#mpq")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SCSharp.UI.UIDialog..ctor(SCSharp.UI.UIScreen,SCSharp.MpqLib.Mpq,System.String,System.String)", MessageId = "1#mpq")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SCSharp.UI.GameModeDialog..ctor(SCSharp.UI.UIScreen,SCSharp.MpqLib.Mpq)", MessageId = "1#mpq")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SCSharp.UI.MainMenu..ctor(SCSharp.MpqLib.Mpq)", MessageId = "0#mpq")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SCSharp.UI.Game.PlayingMpq", MessageId = "Mpq")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SCSharp.UI.Game.InstalledMpq", MessageId = "Mpq")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SCSharp.UI.OkDialog..ctor(SCSharp.UI.UIScreen,SCSharp.MpqLib.Mpq,System.String)", MessageId = "1#mpq")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SCSharp.UI.SoundDialog..ctor(SCSharp.UI.UIScreen,SCSharp.MpqLib.Mpq)", MessageId = "1#mpq")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SCSharp.UI.ZergReadyRoomScreen..ctor(SCSharp.MpqLib.Mpq,System.String)", MessageId = "0#mpq")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SCSharp.UI.RaceSelectionScreen..ctor(SCSharp.MpqLib.Mpq)", MessageId = "0#mpq")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SCSharp.UI.LoadSavedScreen..ctor(SCSharp.MpqLib.Mpq)", MessageId = "0#mpq")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SCSharp.UI.HelpDialog..ctor(SCSharp.UI.UIScreen,SCSharp.MpqLib.Mpq)", MessageId = "1#mpq")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SCSharp.UI.LogOnScreen..ctor(SCSharp.MpqLib.Mpq)", MessageId = "0#mpq")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SCSharp.UI.EstablishingShot..ctor(System.String,System.String,SCSharp.MpqLib.Mpq)", MessageId = "2#mpq")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SCSharp.UI.KeystrokeDialog..ctor(SCSharp.UI.UIScreen,SCSharp.MpqLib.Mpq)", MessageId = "1#mpq")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SCSharp.UI.OkCancelDialog..ctor(SCSharp.UI.UIScreen,SCSharp.MpqLib.Mpq,System.String)", MessageId = "1#mpq")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SCSharp.UI.GameScreen..ctor(SCSharp.MpqLib.Mpq,SCSharp.MpqLib.Mpq,SCSharp.MpqLib.Chk,SCSharp.MpqLib.Got)", MessageId = "0#mpq")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SCSharp.UI.GameScreen..ctor(SCSharp.MpqLib.Mpq,SCSharp.MpqLib.Mpq,SCSharp.MpqLib.Chk,SCSharp.MpqLib.Got)", MessageId = "1#mpq")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SCSharp.UI.GameScreen..ctor(SCSharp.MpqLib.Mpq,System.String,SCSharp.MpqLib.Chk)", MessageId = "0#mpq")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SCSharp.UI.CreditsScreen..ctor(SCSharp.MpqLib.Mpq)", MessageId = "0#mpq")]
[module: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "SCSharp.UI.EndMissionDialog..ctor(SCSharp.UI.UIScreen,SCSharp.MpqLib.Mpq)", MessageId = "1#mpq")]
