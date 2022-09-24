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
    internal class Application {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Application() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("TybaltBot.Resources.Application", typeof(Application).Assembly);
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
        ///   Looks up a localized string similar to Dein Guild Wars 2 Accountname ist unvollständig. Gib bitte auch die 4 Ziffern deines Account Namens an..
        /// </summary>
        internal static string AccountName_Wrong {
            get {
                return ResourceManager.GetString("AccountName_Wrong", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Willkommen Fremder!
        ///Wir freuen uns, dass du den Weg auf unseren Server gefunden hast. Alle Infos zu unserer Raid-Community findest du hier im Discord oder auf unserer Website [Rising Light](https://rising-light.de/).
        ///
        ///Wenn du Mitglied werden möchtest, bewirb dich einfach indem du auf den Bewerben-Knopf unter dieser Nachricht klickst.
        ///Es öffnet sich dann ein Pop-Up Fenster, welches ausgefüllt werden muss..
        /// </summary>
        internal static string EmbedDescription {
            get {
                return ResourceManager.GetString("EmbedDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Vielen Dank für deine Bewerbung! Wir werden sie innerhalb der nächsten Tage bearbeiten und dir Rechte geben.
        ///
        ///In der Zwischenzeit kannst du dir schon einmal deinen Account bei [RaidOrga+](https://orga.rising-light.de/#/) anlegen. Bitte achte auch hier darauf, deinen Accountnamen richtig anzugeben..
        /// </summary>
        internal static string EmbedSuccess {
            get {
                return ResourceManager.GetString("EmbedSuccess", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Wie bist du auf Rising Light gestoßen?.
        /// </summary>
        internal static string Found {
            get {
                return ResourceManager.GetString("Found", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Dein GW2-Accountname.
        /// </summary>
        internal static string ModalAcountName {
            get {
                return ResourceManager.GetString("ModalAcountName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Die Bewerbung wurde erfolgreich verschickt!.
        /// </summary>
        internal static string ModalSuccess {
            get {
                return ResourceManager.GetString("ModalSuccess", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Warum möchtest du Mitglied werden?.
        /// </summary>
        internal static string Reason {
            get {
                return ResourceManager.GetString("Reason", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Wie würdest du deine Raid-Erfahrung bewerten?.
        /// </summary>
        internal static string Skill {
            get {
                return ResourceManager.GetString("Skill", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Bewerbung.
        /// </summary>
        internal static string Title {
            get {
                return ResourceManager.GetString("Title", resourceCulture);
            }
        }
    }
}
