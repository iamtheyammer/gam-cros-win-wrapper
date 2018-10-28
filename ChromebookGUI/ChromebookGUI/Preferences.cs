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
        /// If true, we'll check for updates when the app is opened.
        /// If there's an update, the user will see a window telling them to update.
        /// </summary>
        public static bool PromptWhenUpdatesAreAvailable { get; set; }

        /// <summary>
        /// This Init function needs to be called once in the code.
        /// It is called in App.xaml.cs, so you shouldn't need to call it again.
        /// </summary>
        public static void Init()
        {
            string prefsFile = null;

            try
            {
                prefsFile = File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\iamtheyammer\ChromebookGUI\preferences.json");
                
            } catch
            {
                SerialNumberAssetIdPriority = false;
                ShowWarningWhenImportingFromCSVFile = true;
                PromptWhenUpdatesAreAvailable = true;
                return;
            }

            Dictionary<string, string> prefs = JsonConvert.DeserializeObject<Dictionary<string, string>>(prefsFile);

            // more settings would go here. default prefs go in the catches.
            try
            {
                SerialNumberAssetIdPriority = prefs["SerialNumberAssetIdPriority"] == "True" ? true : false;
            } catch
            {
                SerialNumberAssetIdPriority = false;
            }

            try
            {
                ShowWarningWhenImportingFromCSVFile = prefs["ShowWarningWhenImportingFromCSVFile"] == "True" ? true : false;
            } catch
            {
                ShowWarningWhenImportingFromCSVFile = true;
            }

            try
            {
                PromptWhenUpdatesAreAvailable = prefs["PromptWhenUpdatesAreAvailable"] == "True" ? true : false;
            } catch
            {
                PromptWhenUpdatesAreAvailable = true;
            }
            
            
            
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
            prefs["PromptWhenUpdatesAreAvailable"] = PromptWhenUpdatesAreAvailable.ToString();

            File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\iamtheyammer\ChromebookGUI\preferences.json", JsonConvert.SerializeObject(prefs));
        }

        public static void OpenPreferencesWindow()
        {
            PreferencesWindow window = new PreferencesWindow();

            // must set settings here
            window.SearchForAssetIdsBeforeSerialNumbersCheckBox.IsChecked = SerialNumberAssetIdPriority;
            window.ShowWarningWhenImportingFromCSVFile.IsChecked = ShowWarningWhenImportingFromCSVFile;
            window.PromptWhenUpdatesAreAvailableCheckBox.IsChecked = PromptWhenUpdatesAreAvailable;
            window.Title = "ChromebookGUI Preferences";
            window.ShowDialog();
        }
    }
}
