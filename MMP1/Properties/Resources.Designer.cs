﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MMP1.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
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
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("MMP1.Properties.Resources", typeof(Resources).Assembly);
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
        ///   Looks up a localized string similar to &lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot; ?&gt;
        ///&lt;questions&gt;
        ///  &lt;q correct =&quot;1&quot; title=&quot;Who wrote Julius Caesar, Macbeth and Hamlet?&quot;&gt;
        ///    &lt;a&gt;Charles Dickens&lt;/a&gt;
        ///    &lt;a&gt;William Shakespeare&lt;/a&gt;
        ///    &lt;a&gt;Mark Twain&lt;/a&gt;
        ///    &lt;a&gt;Oscar Wilde&lt;/a&gt;
        ///  &lt;/q&gt;
        ///  &lt;q correct=&quot;0&quot; title=&quot;Where is the smallest bone in the body?&quot;&gt;
        ///    &lt;a&gt;Ear&lt;/a&gt;
        ///    &lt;a&gt;Nose&lt;/a&gt;
        ///    &lt;a&gt;Foot&lt;/a&gt;
        ///    &lt;a&gt;Back&lt;/a&gt;
        ///  &lt;/q&gt;
        ///  &lt;q correct =&quot;3&quot; title=&quot;Which is the only mammal that can’t jump?&quot;&gt;
        ///    &lt;a&gt;Turtle&lt;/a&gt;
        ///    &lt;a&gt;Giraffe&lt;/a&gt;
        ///    &lt;a&gt;Hippo&lt;/a&gt;
        ///  [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string questions {
            get {
                return ResourceManager.GetString("questions", resourceCulture);
            }
        }
    }
}
