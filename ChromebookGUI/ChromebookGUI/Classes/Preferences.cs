using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Windows;

namespace ChromebookGUI
{
    class Preferences
    {
        /// <summary>
        /// If true, we will search for Asset ID matches first, before searching for Serial Number matches.
        /// </summary>
        public static bool SerialNumberAssetIdPriority { get; set; }
        /// <summary>
        /// Whether to show the default warning when importing from CSV file.
        /// </summary>
        public static bool ShowWarningWhenImportingFromCSVFile { get; set; }

        /// <summary>
        /// If true, we'll check for updates when the app is opened.
        /// If there's an update, the user will see a window telling them to update.
        /// </summary>
        public static bool PromptWhenUpdatesAreAvailable { get; set; }

        /// <summary>
        /// Automatically opens the text box layout instead of the button layout.
        /// </summary>
        public static bool UseTextBoxLayoutInsteadOfButtonLayout { get; set; }

        /// <summary>
        /// Allows us to send rich error data back to Sentry (includes PII)
        /// </summary>
        public static bool AllowEnhancedTelemetry { get; set; }

        /// <summary>
        /// Indicates whether we need a telemetry consent from the user.
        /// </summary>
        public static bool NeedsTelemetryConsent { get; set; }

        /// <summary>
        /// Sets the UI Mode: "light" or "dark"
        /// </summary>
        public static string UiMode { get; set; }

        /// <summary>
        /// Path to the preferences file
        /// </summary>
        private static readonly string prefsFilePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\iamtheyammer\ChromebookGUI\preferences.json";

        /// <summary>
        /// This Init function needs to be called once in the code.
        /// It is called in App.xaml.cs, so you shouldn't need to call it again.
        /// </summary>
        public static void Init()
        {
            string prefsFile = null;

            try
            {
                prefsFile = File.ReadAllText(prefsFilePath);

            } catch
            {
                SerialNumberAssetIdPriority = false;
                ShowWarningWhenImportingFromCSVFile = true;
                PromptWhenUpdatesAreAvailable = true;
                UseTextBoxLayoutInsteadOfButtonLayout = true;
                AllowEnhancedTelemetry = false;
                NeedsTelemetryConsent = true;
                UiMode = "light";
                return;
            }

            Dictionary<string, string> prefs = JsonConvert.DeserializeObject<Dictionary<string, string>>(prefsFile);

            // more settings would go here. default prefs go in the catches.
            if (prefs.ContainsKey("SerialNumberAssetIdPriority"))
            {
                SerialNumberAssetIdPriority = prefs["SerialNumberAssetIdPriority"] == "True";
            } else
            {
                SerialNumberAssetIdPriority = false;
            }

            if(prefs.ContainsKey("ShowWarningWhenImportingFromCSVFile"))
            {
                ShowWarningWhenImportingFromCSVFile = prefs["ShowWarningWhenImportingFromCSVFile"] == "True";
            } else
            {
                ShowWarningWhenImportingFromCSVFile = true;
            }

            if(prefs.ContainsKey("PromptWhenUpdatesAreAvailable"))
            {
                PromptWhenUpdatesAreAvailable = prefs["PromptWhenUpdatesAreAvailable"] == "True";
            } else
            {
                PromptWhenUpdatesAreAvailable = true;
            }

            if (prefs.ContainsKey("UseTextBoxLayoutInsteadOfButtonLayout"))
            {
                UseTextBoxLayoutInsteadOfButtonLayout = prefs["UseTextBoxLayoutInsteadOfButtonLayout"] == "True";
            }
            else
            {
                UseTextBoxLayoutInsteadOfButtonLayout = true;
            }

            if(prefs.ContainsKey("AllowEnhancedTelemetry"))
            {
                AllowEnhancedTelemetry = prefs["AllowEnhancedTelemetry"] == "True";
                NeedsTelemetryConsent = false;
            } else
            {
                NeedsTelemetryConsent = true;
            }

            if (prefs.ContainsKey("UiMode"))
            {
                UiMode = prefs["UiMode"] == "light" ? "light" : "dark";
            }
            else
            {
                UiMode = "dark";
            }
        }

        /// <summary>
        /// Saves preferences to a file.
        /// </summary>
        public static void Save()
        {
            Dictionary<string, string> prefs = new Dictionary<string, string>
            {
                // more settings would go here
                ["SerialNumberAssetIdPriority"] = SerialNumberAssetIdPriority.ToString(),
                ["ShowWarningWhenImportingFromCSVFile"] = ShowWarningWhenImportingFromCSVFile.ToString(),
                ["PromptWhenUpdatesAreAvailable"] = PromptWhenUpdatesAreAvailable.ToString(),
                ["UseTextBoxLayoutInsteadOfButtonLayout"] = UseTextBoxLayoutInsteadOfButtonLayout.ToString(),
                ["UiMode"] = UiMode
            };
            if(!NeedsTelemetryConsent) prefs.Add("AllowEnhancedTelemetry", AllowEnhancedTelemetry.ToString());
            File.WriteAllText(prefsFilePath, JsonConvert.SerializeObject(prefs));
        }

        public static void OpenPreferencesWindow()
        {
            PreferencesWindow window = new PreferencesWindow();

            // must set settings here
            window.SearchForAssetIdsBeforeSerialNumbersCheckBox.IsChecked = SerialNumberAssetIdPriority;
            window.ShowWarningWhenImportingFromCSVFile.IsChecked = ShowWarningWhenImportingFromCSVFile;
            window.PromptWhenUpdatesAreAvailableCheckBox.IsChecked = PromptWhenUpdatesAreAvailable;
            Console.WriteLine(UseTextBoxLayoutInsteadOfButtonLayout);
            window.UseTextBoxLayoutInsteadOfButtonLayoutCheckBox.IsChecked = UseTextBoxLayoutInsteadOfButtonLayout;
            window.AllowEnhancedTelemetryCheckbox.IsChecked = AllowEnhancedTelemetry;
            window.Title = "ChromebookGUI Preferences";
            window.ShowDialog();
        }
    }
}
