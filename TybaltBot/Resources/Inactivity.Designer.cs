﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TybaltBot.Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Inactivity {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Inactivity() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("TybaltBot.Resources.Inactivity", typeof(Inactivity).Assembly);
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
        ///   Looks up a localized string similar to Account Name.
        /// </summary>
        internal static string AccountName {
            get {
                return ResourceManager.GetString("AccountName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0} ist nicht mehr inaktiv!.
        /// </summary>
        internal static string Active_Inform {
            get {
                return ResourceManager.GetString("Active_Inform", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Du bist nicht mehr Inaktiv!.
        /// </summary>
        internal static string Active_Success {
            get {
                return ResourceManager.GetString("Active_Success", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Dauer.
        /// </summary>
        internal static string Duration {
            get {
                return ResourceManager.GetString("Duration", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Inaktivitäts Rolle wurde bereits hinzugefügt..
        /// </summary>
        internal static string Embed_Footer {
            get {
                return ResourceManager.GetString("Embed_Footer", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Der Sinn und Zweck des Bots ist einfach: Er soll es euch ermöglichen euch als Inaktiv zu melden, wenn ihr für längere Zeit (4 oder mehr Wochen) nicht an der Community teilhaben könnt oder wollt.
        ///
        ///Wenn ihr euch Inaktiv melden möchtet, fragt der Bot euren GW2-Accountnamen ab, den Zeitraum (bitte gebt hier wirklich einen realistischen Zeitraum an, und kein &quot;bis irgendwann&quot;) und den Grund warum ihr euch Inaktiv meldet (längerer Urlaub, Auslandssemester, Bachelorarbeit, PC kaputt o.ä.).
        ///Anschließend wird die  [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string EmbedDescription {
            get {
                return ResourceManager.GetString("EmbedDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Vielen Dank für das Beantworten der Fragen. Dir wurde nun automatisch die Inaktivitäts-Rolle hinzugefügt und die Raidleitung wurde darüber Informiert..
        /// </summary>
        internal static string Inactive_Success {
            get {
                return ResourceManager.GetString("Inactive_Success", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Dein GW2-Accountname.
        /// </summary>
        internal static string Modal_AccountName {
            get {
                return ResourceManager.GetString("Modal_AccountName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Wie lange möchtest du pausieren?.
        /// </summary>
        internal static string Modal_Duration {
            get {
                return ResourceManager.GetString("Modal_Duration", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Bitte geb den Grund für die pause an.
        /// </summary>
        internal static string Modal_Reason {
            get {
                return ResourceManager.GetString("Modal_Reason", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Grund.
        /// </summary>
        internal static string Reason {
            get {
                return ResourceManager.GetString("Reason", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Inaktivität.
        /// </summary>
        internal static string Title {
            get {
                return ResourceManager.GetString("Title", resourceCulture);
            }
        }
    }
}
