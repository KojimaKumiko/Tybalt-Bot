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
        ///   Looks up a localized string similar to {0} Der Bot konnte der Person keine DM schicken. Es ist möglich, dass die DM&apos;s der Person geschlossen sind..
        /// </summary>
        internal static string CannotSendMessage {
            get {
                return ResourceManager.GetString("CannotSendMessage", resourceCulture);
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
        ///   Looks up a localized string similar to Bei dieser Bewerbung kam es zu einer Exception! Es ist möglich das die Person keine Rolle erhalten hat und es versucht wird die Bewerbung erneut abzuschicken..
        /// </summary>
        internal static string EmbedFail {
            get {
                return ResourceManager.GetString("EmbedFail", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Hier habt ihr die Möglichkeit verschiedene Rollen hinzuzufügen bzw. diese zu entfernen..
        /// </summary>
        internal static string EmbedRole {
            get {
                return ResourceManager.GetString("EmbedRole", resourceCulture);
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
        ///   Looks up a localized string similar to Es gab ein Problem beim verschicken deiner Bewerbung. Bitte versuche es später erneut.
        ///Falls das Problem weiterhin auftreten sollte, melde dich entweder im &lt;#{0}&gt; Channel oder bei {1} direkt..
        /// </summary>
        internal static string ModalFail {
            get {
                return ResourceManager.GetString("ModalFail", resourceCulture);
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
        ///   Looks up a localized string similar to Hallo! Deine Bewerbung wurde erfolgreich verschickt, jedoch konnte der Bot dir keine DM schicken, weshalb du diese Nachricht hier siehst. Stelle doch bitte sicher, dass deine DMs offen sind, damit wir dich bei Bedarf direkt anschreiben können bezüglich deiner Bewerbung. Du kannst das überprüfen, in dem du einen Rechtsklick auf den Server machst und dann auf den Punkt `Privatsphäreeinstellungen` klickst. Die erste Option `Direktnachrichten` sollte hier aktiviert sein..
        /// </summary>
        internal static string ModalSuccessNoDM {
            get {
                return ResourceManager.GetString("ModalSuccessNoDM", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Du interessierst dich für andere Spiele neben Guild Wars 2 und möchtest benachrichtigt werden, wenn weitere Mitspieler gesucht werden? Dann musst du lediglich auf den unten stehenden Knopf drücken und dir wird automatisch die Rolle &lt;@&amp;1049401228452438057&gt; hinzugefügt.
        ///Sollte dir die Benachrichtigungen zu viel werden oder das Interesse schwinden, kannst du dir die Rolle entfernen lassen, wenn du wieder auf den Knopf drückst..
        /// </summary>
        internal static string OtherGames {
            get {
                return ResourceManager.GetString("OtherGames", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Dir wurde die Rolle erfolgreich hinzugefügt..
        /// </summary>
        internal static string OtherGamesAdded {
            get {
                return ResourceManager.GetString("OtherGamesAdded", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Dir wurde die Rolle erfolgreich entfernt..
        /// </summary>
        internal static string OtherGamesRemoved {
            get {
                return ResourceManager.GetString("OtherGamesRemoved", resourceCulture);
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
        ///   Looks up a localized string similar to Dir wurde die Rolle {0} erfolgreich hinzugefügt..
        /// </summary>
        internal static string RoleAdded {
            get {
                return ResourceManager.GetString("RoleAdded", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Dir wurde die Rolle {0} erfolgreich entfernt..
        /// </summary>
        internal static string RoleRemoved {
            get {
                return ResourceManager.GetString("RoleRemoved", resourceCulture);
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
