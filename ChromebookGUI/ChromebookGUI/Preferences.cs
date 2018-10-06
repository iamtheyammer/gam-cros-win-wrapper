using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

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
        /// This Init function needs to be called once in the code.
        /// It is called in App.xaml.cs, so you shouldn't need to call it again.
        /// </summary>
        public static void Init()
        {
            string prefsFile = null;

            try
            {
                prefsFile = File.ReadAllText("preferences.json");
            } catch
            {
                // set default prefs here
                Preferences.SerialNumberAssetIdPriority = false;
                Preferences.ShowWarningWhenImportingFromCSVFile = true;
                return;
                // no settings file found
            }

            Dictionary<string, string> prefs = JsonConvert.DeserializeObject<Dictionary<string, string>>(prefsFile);

            // more settings would go here
            SerialNumberAssetIdPriority = prefs["SerialNumberAssetIdPriority"] == "True" ? true : false;
            ShowWarningWhenImportingFromCSVFile = prefs["ShowWarningWhenImportingFromCSVFile"] == "True" ? true : false;
        }

        /// <summary>
        /// Saves preferences to a file.
        /// </summary>
        public static void Save()
        {
            Dictionary<string, string> prefs = new Dictionary<string, string>();
            // more settings would go here
            prefs["SerialNumberAssetIdPriority"] = SerialNumberAssetIdPriority.ToString();
            prefs["ShowWarningWhenImportingFromCSVFile"] = ShowWarningWhenImportingFromCSVFile.ToString();

            File.WriteAllText("preferences.json", JsonConvert.SerializeObject(prefs));
        }

        public static void OpenPreferencesWindow()
        {
            PreferencesWindow window = new PreferencesWindow();

            // must set settings here
            window.SearchForAssetIdsBeforeSerialNumbersCheckBox.IsChecked = SerialNumberAssetIdPriority;
            window.ShowWarningWhenImportingFromCSVFile.IsChecked = ShowWarningWhenImportingFromCSVFile;
            window.Title = "ChromebookGUI Preferences";
            window.ShowDialog();
        }
    }
}
