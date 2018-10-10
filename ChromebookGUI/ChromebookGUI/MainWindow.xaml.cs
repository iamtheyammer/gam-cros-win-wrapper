using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;

namespace ChromebookGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        /// <summary>
        /// Essentially, run GAM.GetDeviceId() on whatever is entered into the text field.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SubmitDeviceId_Click(object sender, RoutedEventArgs e)
        {
            ToggleMainWindowButtons(false);
            outputField.Text = "Loading...";
            if (deviceInputField.Text.Length < 1 || deviceInputField.Text.ToLower() == "enter a device id, serial number or email...")
            {
                outputField.Text = "You must enter something into the field at the top.";
                return;
            }

            //outputField.Text = GAM.GetDeviceId(deviceInputField.Text);
            BasicDeviceInfo deviceInfo = GAM.GetDeviceId(deviceInputField.Text);
            Globals.ClearGlobals(); // clear the globals before adding new ones
            Globals.SetGlobalsFromBasicDeviceInfo(deviceInfo);
            if (deviceInfo.Error)
            {
                outputField.Text = deviceInfo.ErrorText;
                return;
            }
            
            outputField.Text = "Found device. ID: " + deviceInfo.DeviceId + ".";
            if (!String.IsNullOrEmpty(deviceInfo.SerialNumber)) outputField.Text += "\nSerial Number: " + deviceInfo.SerialNumber;
            if (!String.IsNullOrEmpty(deviceInfo.Notes)) outputField.Text += "\nNotes: " + deviceInfo.Notes;
            if (!String.IsNullOrEmpty(deviceInfo.LastSync)) outputField.Text += "\nLast Sync: " + deviceInfo.LastSync;
            if (!String.IsNullOrEmpty(deviceInfo.AssetId)) outputField.Text += "\nAsset ID: " + deviceInfo.AssetId;
            if (!String.IsNullOrEmpty(deviceInfo.Location)) outputField.Text += "\nLocation: " + deviceInfo.Location;
            if (!String.IsNullOrEmpty(deviceInfo.Status)) outputField.Text += "\nStatus: " + deviceInfo.Status;
            ToggleMainWindowButtons(true);
            //deviceInputField.Text = deviceId;
        }

        private void outputField_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        /// <summary>
        /// Get info about the device. Does NOT support CSVs.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void getInfoButton_Click(object sender, RoutedEventArgs e)
        {
            if(Globals.DeviceIdExists() == false)
            {
                outputField.Text = "No device ID currently in memory. Press " + submitDeviceId.Content + " then try again.";
                return;
            } else if (Globals.DeviceId == "csv")
            {
                GetInput.ShowInfoDialog("Not Supported", "This action is not supported with a CSV", "We don't support using CSVs for info because they put out unreadable output.");
                return;
            }
            string deviceId = Globals.DeviceId;
            //if (deviceId == null || deviceId.Length < 1) ;
            outputField.Text = GAM.RunGAMFormatted("info cros " + Globals.DeviceId + " allfields");
        }

        /// <summary>
        /// Set the location of the device.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void setLocationButton_Click(object sender, RoutedEventArgs e)
        {
            if (Globals.DeviceIdExists() == false && Globals.DeviceId != "csv")
            {
                outputField.Text = "No device ID currently in memory. Press " + submitDeviceId.Content + " then try again.";
                return;
            }
            string location = !String.IsNullOrEmpty(Globals.Location) ? Globals.Location : "Enter a location...";
            string newLocation = GetInput.getInput("What would you like to set the location to?", location , !String.IsNullOrEmpty(Globals.SerialNumber) ? "Add/Change Device Location: " + Globals.SerialNumber : "Add/Change Device Location: " + Globals.DeviceId, new Button { IsEnabled = true, Text = "Clear Location" });
            if (newLocation == null | newLocation == location)
            {
                outputField.Text = "You didn't enter anything or pressed cancel.";
                return;
            }
            else if (newLocation == "ExtraButtonClicked") newLocation = "";

            string gamResult = null;
            if(Globals.DeviceId == "csv")
            {
                gamResult = GAM.RunGAMFormatted(GAM.GetGAMCSVCommand(Globals.CsvLocation, "update cros", "location \"" + newLocation + "\""));
            } else
            {
                gamResult = GAM.RunGAMFormatted("update cros " + Globals.DeviceId + " location \"" + newLocation + "\"");
            }
            Globals.Location = newLocation;
            outputField.Text = gamResult + "\nAs long as you don't see an error, the location has been updated.";
        }
        
        /// <summary>
        /// Set the asset id of the device. If an asset id is in the Globals, it will prefill that in the text box.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void setAssetIdButton_Click(object sender, RoutedEventArgs e)
        {
            if (Globals.DeviceIdExists() == false && Globals.DeviceId != "csv")
            {
                outputField.Text = "No device ID currently in memory. Press " + submitDeviceId.Content + " then try again.";
                return;
            }
            string assetId = !String.IsNullOrEmpty(Globals.AssetId) ? Globals.AssetId : "Enter an Asset ID...";
            string newAssetId = GetInput.getInput("What would you like to set the asset ID to?", assetId, !String.IsNullOrEmpty(Globals.SerialNumber) ? "Enter/Change Device Asset ID: " + Globals.SerialNumber : "Enter/Change Device Asset ID: " + Globals.DeviceId, new Button { IsEnabled = true, Text = "Clear Asset ID" });
            if (newAssetId == null)
            {
                outputField.Text = "You didn't enter anything or you pressed cancel, silly goose!";
                return;
            }
            else if (newAssetId == "ExtraButtonClicked") newAssetId = "";
            string gamResult = null;
            if(Globals.DeviceId == "csv")
            {
                gamResult = GAM.RunGAMFormatted(GAM.GetGAMCSVCommand(Globals.CsvLocation, "update cros ", "assetid \"" + newAssetId + "\""));
            } else
            {
                gamResult = GAM.RunGAMFormatted("update cros " + Globals.DeviceId + " assetid \"" + newAssetId + "\"");
            }
            Globals.AssetId = newAssetId;
            outputField.Text = gamResult + "\nAs long as you don't see an error, this query completed successfully.";
        }

        private void setUserButton_Click(object sender, RoutedEventArgs e)
        {
            if (Globals.DeviceIdExists() == false && Globals.DeviceId != "csv")
            {
                outputField.Text = "No device ID currently in memory. Press " + submitDeviceId.Content + " then try again.";
                return;
            }
            string user = !String.IsNullOrEmpty(Globals.User) ? Globals.User : "Set the user...";
            string newUser = GetInput.getInput("What would you like to set the user to?", user, !String.IsNullOrEmpty(Globals.SerialNumber) ? "Modify Device User: " + Globals.SerialNumber : "Modify Device User: " + Globals.DeviceId);
            if (newUser == null)
            {
                outputField.Text = "You didn't enter anything or you pressed cancel, silly goose!";
                return;
            }
            string gamResult = null;
            if(Globals.DeviceId == "csv")
            {
                gamResult = GAM.RunGAMFormatted(GAM.GetGAMCSVCommand(Globals.CsvLocation, "update cros", "user " + newUser));
            } else
            {
                gamResult = GAM.RunGAMFormatted("update cros " + Globals.DeviceId + " user " + newUser);
            }
            Globals.User = newUser;
            outputField.Text = gamResult + "\nAs long as you don't see an error, this query completed successfully.";
        }

        private void disableButton_Click(object sender, RoutedEventArgs e)
        {
            if (Globals.DeviceIdExists() == false && Globals.DeviceId != "csv")
            {
                outputField.Text = "No device ID currently in memory. Press " + submitDeviceId.Content + " then try again.";
                return;
            }

            string gamResult = null;
            if(Globals.DeviceId == "csv")
            {
                gamResult = GAM.RunGAMFormatted(GAM.GetGAMCSVCommand(Globals.CsvLocation, "update cros", "action disable"));
            } else
            {
                gamResult = GAM.RunGAMFormatted("update cros " + Globals.DeviceId + " action disable");
            }
            outputField.Text = gamResult + "\nAs long as you don't see an error, this query completed successfully.";
        }

        private void enableButton_Click(object sender, RoutedEventArgs e)
        {
            if (Globals.DeviceIdExists() == false)
            {
                outputField.Text = "No device ID currently in memory. Press " + submitDeviceId.Content + " then try again.";
                return;
            }
            string gamResult = null;
            if (Globals.DeviceId == "csv")
            {
                gamResult = GAM.RunGAMFormatted(GAM.GetGAMCSVCommand(Globals.CsvLocation, "update cros", "action reenable"));
            }
            else
            {
                gamResult = GAM.RunGAMFormatted("update cros " + Globals.DeviceId + " action reenable");
            }
            outputField.Text = gamResult += "\nAs long as you don't see an error, this query completed successfully.";
        }

        private void changeOuButton_Click(object sender, RoutedEventArgs e)
        {
            if (Globals.DeviceIdExists() == false)
            {
                outputField.Text = "No device ID currently in memory. Press " + submitDeviceId.Content + " then try again.";
                return;
            }
            outputField.Text = "You should see the org selector in a second...";
            //return;

            List<string> allOrgs = GAM.RunGAM("print orgs allfields");
            List<List<string>> fixedOrgs = FixCSVCommas.FixCommas(allOrgs);

            List<OrgUnit> orgUnits = new List<OrgUnit>();
            foreach(List<string> org in fixedOrgs)
            {
                if (org[0] == "orgUnitPath") continue;

                orgUnits.Add(new OrgUnit()
                {
                    OrgUnitPath = !String.IsNullOrEmpty(org[0]) ? org[0] : null,
                    OrgUnitName = !String.IsNullOrEmpty(org[2]) ? (org[2].StartsWith("id:") ? "(no description provided)" : org[2]) : null,
                    OrgUnitDescription = !String.IsNullOrEmpty(org[3]) ? (org[3].StartsWith("id:") ? "(no description provided)" : org[3]) : null
                });
            }
            if(orgUnits.Count < 2)
            {
                outputField.Text = "There was an error getting your org units. You don't seem to have any.";
                return;
            }
            List<string> orgSelection = GetInput.GetDataGridSelection("Pick an org!", "Click on an row to select it, or paste the full path here and press submit...", "Organizational Unit Selector", orgUnits);
            string orgPath = null;
            foreach(string item in orgSelection)
            {
                if (item.Contains("/")) orgPath = item;
            }
            if (orgPath == null | orgSelection.Contains("Click on an row to select it, or paste the full path here and press submit..."))
            {
                outputField.Text = "Either you didn't enter anything or there was an error. Nothing has been changed.";
                return;
            }

            string gamResult = null;
            if (Globals.DeviceId == "csv")
            {
                gamResult = GAM.RunGAMFormatted(GAM.GetGAMCSVCommand(Globals.CsvLocation, "update cros", "ou \"" + orgPath + "\""));
            }
            else
            {
                gamResult = GAM.RunGAMFormatted("update cros " + Globals.DeviceId + " ou \"" + orgPath + "\"");
            }
            outputField.Text = "Done! Your OU has been changed.";
        }

        private void deprovisionButton_Click(object sender, RoutedEventArgs e)
        {
            if (Globals.DeviceIdExists() == false)
            {
                outputField.Text = "No device ID currently in memory. Press " + submitDeviceId.Content + " then try again.";
                return;
            }
            int depChoice = GetInput.GetDeprovisionReason();
            string depAction = null;
            switch (depChoice) {
                case 0:
                    outputField.Text = "Either you cancelled or selected nothing.";
                    return;
                case 1:
                    // same model replacement
                    depAction = "deprovision_same_model_replace";
                    break;
                case 2:
                    // different model replacement
                    depAction = "deprovision_different_model_replace";
                    break;
                case 3:
                    // retiring device
                    depAction = "deprovision_retiring_device";
                    break;
                default:
                    depAction = "thisbetterfailbecausesomethingiswrong";
                    return;
            }

            string gamResult = null;
            if (Globals.DeviceId == "csv")
            {
                gamResult = GAM.RunGAMFormatted(GAM.GetGAMCSVCommand(Globals.CsvLocation, "update cros", "action " + depAction + " acknowledge_device_touch_requirement"));
            }
            else
            {
                gamResult = GAM.RunGAMFormatted("update cros " + Globals.DeviceId + " action " + depAction + " acknowledge_device_touch_requirement");
            }
            outputField.Text = gamResult += "\nAs long as you don't see an error, this query completed successfully.";
        }

        private void noteButton_Click(object sender, RoutedEventArgs e)
        {
            if (Globals.DeviceIdExists() == false)
            {
                outputField.Text = "No device ID currently in memory. Press " + submitDeviceId.Content + " then try again.";
            }
            string note = null;
            if(String.IsNullOrEmpty(Globals.Note) && Globals.DeviceId != "csv")
            {
                List<string> gamResult = GAM.RunGAM("info cros " + Globals.DeviceId + " fields notes");
                if (gamResult.Count < 2)
                {
                    note = "No note found. Enter a new one here...";
                }
                else
                {
                    note = gamResult[1].Substring(9);
                }
            } else if (!String.IsNullOrEmpty(Globals.Note))
            {
                note = Globals.Note;
            } else if (Globals.DeviceId == "csv")
            {
                note = "Set a note for all devices from this CSV...";
            }

            string newNote = GetInput.getInput("Edit/modify note:", note, !String.IsNullOrEmpty(Globals.SerialNumber) ? "Add/Change Device Note: " + Globals.SerialNumber : "Add/Change Device Note: " + Globals.DeviceId);

            if (newNote == null | newNote == note)
            {
                outputField.Text = "You didn't change the note so I'm leaving it as it is.";
                return;
            }

            string finalGamResult = null;
            if (Globals.DeviceId == "csv")
            {
                finalGamResult = GAM.RunGAMFormatted(GAM.GetGAMCSVCommand(Globals.CsvLocation, "update cros", "notes \"" + newNote + "\""));
            }
            else
            {
                finalGamResult = GAM.RunGAMFormatted("update cros " + Globals.DeviceId + " notes \"" + newNote + "\"");
            }

            outputField.Text = "As long as there's no error, the note was updated.";
        }

        private void copyToClipboard_Click (object sender, RoutedEventArgs e)
        {
            if(outputField.Text.Length < 1)
            {
                outputField.Text = "Nothing was here to copy.";
            }
            Clipboard.SetText(outputField.Text, TextDataFormat.UnicodeText);
            outputField.Text += "\n\nCopied to clipboard.";
        }

        private void copyId_Click (object sender, RoutedEventArgs e)
        {
            if(Globals.DeviceId != null && Globals.DeviceId != "csv")
            {
                Clipboard.SetText(Globals.DeviceId, TextDataFormat.UnicodeText);
                outputField.Text += "\n\nCopied to clipboard.";
            }
            else if (Globals.DeviceId == "csv")
            {
                GetInput.ShowInfoDialog("Not Supported", "This action is not supported from a CSV", "We don't support copying all IDs from a CSV at this time.");
                return;
            } else
            {
                outputField.Text += "\n\nNo device ID currently in memory.";
            }
        }

        private void deviceInputField_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void selectAllTextInBox(object sender, object e)
        {
            if (deviceInputField.Text == "Enter a Device ID, Asset ID, Serial Number or Email...")
            {
                deviceInputField.Text = String.Empty;
            } else
            {
                deviceInputField.SelectAll();
            }
        }

        private void FontSizeUpButton_Click(object sender, RoutedEventArgs e)
        {
            outputField.FontSize += 2;
        }

        private void FontSizeDownButton_Click(object sender, RoutedEventArgs e)
        {
            outputField.FontSize -= 2;
        }

        public void ToggleMainWindowButtons(bool value)
        {
            getInfoButton.IsEnabled = value;
            setLocationButton.IsEnabled = value;
            setAssetIdButton.IsEnabled = value;
            setUserButton.IsEnabled = value;
            disableButton.IsEnabled = value;
            enableButton.IsEnabled = value;
            changeOuButton.IsEnabled = value;
            deprovisionButton.IsEnabled = value;
            noteButton.IsEnabled = value;
            copyIdButton.IsEnabled = value;

        }

        private void FilePreferences_Click(object sender, RoutedEventArgs e)
        {
            Preferences.OpenPreferencesWindow();
        }

        private void FileCloseChromebookGUI_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void FileAboutChromebookGUI_Click(object sender, RoutedEventArgs e)
        {
            GetInput.ShowInfoDialog("About ChromebookGUI", "Open source, by iamtheyammer", "This project was made by iamtheyammer on GitHub. It lives at https://git.io/fxYBf, and was made because of my disdain for the speed of Google's Admin Console.\n\nI hope you enjoy it! If you'd like to contribute (this project is written in C# with .NET) please submit a pull request!\n\nThank you for using my software.");
        }

        private void ImportFromCSV_Click(object sender, RoutedEventArgs e)
        {
            if(Preferences.ShowWarningWhenImportingFromCSVFile)
            {
                GetInput.ShowInfoDialog("CSV Import Warning", "About importing from CSV",
                    "When you use this tool to import data from a CSV, you are making many modifications quickly, " +
                    "which can be hard to undo. \n" +
                    "To import a CSV, make sure that, in the CSV you want to import, the column with Device IDs is named " + 
                    "\"deviceId\", without the quotes. That will allow ChromebookGUI to run the mass operation.\n" + 
                    "If you want to silence this warning in the future, go to File -> Preferences and untick the appropriate " + 
                    "box."
                    );
            }
            string filePath = GetInput.GetFileSelection("csv");
            if (filePath == null)
            {
                outputField.Text = "You didn't select a file or pressed cancel. Either way no changes have been made.";
                return;
            }
            Globals.CsvLocation = filePath;
            Globals.DeviceId = "csv";
            ToggleMainWindowButtons(true);

        }

        private void ImportFromGAMCommand_Click(object sender, RoutedEventArgs e)
        {
            // not currently supported, commented out in XAML
        }

        private void Window_ResetWindowSize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Normal;
            Height = 500;
            Width = 800;
        }

        private void HelpGoToGithubProject_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("https://github.com/iamtheyammer/gam-cros-win-wrapper");
        }

        private void HelpCheckForUpdates_Click(object sender, RoutedEventArgs e)
        {
            string updates = Updates.CheckForUpdates();
            switch(updates)
            {
                case "false":
                    GetInput.ShowInfoDialog("ChromebookGUI Updater", "No updates available", "This product, ChromebookGUI by iamtheyammer, is up to date. You are running " + Software.Type + " " + Software.Version + ".");
                    return;
                case "true":
                    return;
                //case "error":
                //    GetInput.ShowInfoDialog("ChromebookGUI Updater", "Error checking for updates.", "There was an error checking for updates."); // this should never happen!
                //    return;
            }
        }

        
    }

}
