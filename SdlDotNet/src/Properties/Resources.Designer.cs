//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.42
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SdlDotNet.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "2.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("SdlDotNet.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Only AudioFormat.Unsigned16Little currently supported..
        /// </summary>
        internal static string AudioFormatSupported {
            get {
                return ResourceManager.GetString("AudioFormatSupported", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Video.GLGetAttribute returned an improper result.
        /// </summary>
        internal static string GLGetAttributeImproperResult {
            get {
                return ResourceManager.GetString("GLGetAttributeImproperResult", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to point.Y must be less then the Height and greater or equal to zero.
        /// </summary>
        internal static string HeightOutOfRange {
            get {
                return ResourceManager.GetString("HeightOutOfRange", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Index out of range.
        /// </summary>
        internal static string IndexOutOfRange {
            get {
                return ResourceManager.GetString("IndexOutOfRange", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The method or operation is not implemented..
        /// </summary>
        internal static string NotImplemented {
            get {
                return ResourceManager.GetString("NotImplemented", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cannot assign a null font to value.
        /// </summary>
        internal static string NullFont {
            get {
                return ResourceManager.GetString("NullFont", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to OpenAudio already called and initialized.  Call CloseAudio first before calling OpenAudio again..
        /// </summary>
        internal static string OpenAudioInit {
            get {
                return ResourceManager.GetString("OpenAudioInit", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Call OpenAudio before using function..
        /// </summary>
        internal static string OpenAudioNotInit {
            get {
                return ResourceManager.GetString("OpenAudioNotInit", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Select Drive.
        /// </summary>
        internal static string SelectDrive {
            get {
                return ResourceManager.GetString("SelectDrive", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to object is not a Sprite.
        /// </summary>
        internal static string SpriteCompareException {
            get {
                return ResourceManager.GetString("SpriteCompareException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Only AudioFormat.Unsigned16Little is currently supported.
        /// </summary>
        internal static string SupportedAudioFormats {
            get {
                return ResourceManager.GetString("SupportedAudioFormats", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unknown Bytes Per Pixel.
        /// </summary>
        internal static string UnknownBytesPerPixel {
            get {
                return ResourceManager.GetString("UnknownBytesPerPixel", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Video query failed: .
        /// </summary>
        internal static string VideoQueryFailed {
            get {
                return ResourceManager.GetString("VideoQueryFailed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to point.X must be less then the Width and greater or equal to zero.
        /// </summary>
        internal static string WidthOutOfRange {
            get {
                return ResourceManager.GetString("WidthOutOfRange", resourceCulture);
            }
        }
    }
}
