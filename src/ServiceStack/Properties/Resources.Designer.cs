﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ServiceStack.ResourceDesigner {
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
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
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("ServiceStack.Properties.Resources", typeof(Resources).GetAssembly());
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
        ///   Looks up a localized string similar to Container service is built-in and read-only..
        /// </summary>
        internal static string Registration_CantRegisterContainer {
            get {
                return ResourceManager.GetString("Registration_CantRegisterContainer", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Service type {0} does not inherit or implement {1}..
        /// </summary>
        internal static string Registration_IncompatibleAsType {
            get {
                return ResourceManager.GetString("Registration_IncompatibleAsType", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Required dependency of type {0} named &apos;{1}&apos; could not be resolved..
        /// </summary>
        internal static string ResolutionException_MissingNamedType {
            get {
                return ResourceManager.GetString("ResolutionException_MissingNamedType", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Required dependency of type {0} could not be resolved..
        /// </summary>
        internal static string ResolutionException_MissingType {
            get {
                return ResourceManager.GetString("ResolutionException_MissingType", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unknown scope..
        /// </summary>
        internal static string ResolutionException_UnknownScope {
            get {
                return ResourceManager.GetString("ResolutionException_UnknownScope", resourceCulture);
            }
        }
    }
}
