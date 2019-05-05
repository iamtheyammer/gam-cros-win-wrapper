using ChromebookGUI.Windows;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ChromebookGUI
{
    /// <summary>
    /// Interaction logic for DefaultView.xaml
    /// </summary>
    public partial class TextBoxView : Page
    {
        public TextBoxView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Essentially, run GAM.GetDeviceId() on whatever is entered into the text field.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void SubmitDeviceId_Click(object sender, RoutedEventArgs e)
        {
            IsLoading = true;
            ProgressBarDialog progressBar = GetInput.ShowProgressBarDialog("Getting Device Info", 5, "Searching for devices...");
            if (deviceInputField.Text.Length < 1 || deviceInputField.Text.ToLower() == "enter a device id, asset id, serial number, query string or email...")
            {
                outputField.Text = "You must enter something into the field at the top.";
                progressBar.Close();
                return;
            }

            //outputField.Text = GAM.GetDeviceId(deviceInputField.Text);
            string input = deviceInputField.Text;
            List<BasicDeviceInfo> possibleDevices = await Task.Run(() => GAM.GetDeviceId(input));
            BasicDeviceInfo deviceInfo = BasicDeviceInfo.HandleGetDeviceId(possibleDevices);
            Globals.ClearGlobals(); // clear the globals before adding new ones
            Globals.SetGlobalsFromBasicDeviceInfo(deviceInfo);
            if (deviceInfo.Error)
            {
                outputField.Text = deviceInfo.ErrorText;
                progressBar.Close();
                return;
            }
            progressBar.UpdateBarAndText(30, "Getting more device info...");
            

            BasicDeviceInfo fullDeviceInfo = await Task.Run(() => GAM.GetAllDeviceInfo(deviceInfo.DeviceId));
            fullDeviceInfo.LastSync = !string.IsNullOrEmpty(deviceInfo.LastSync) ? deviceInfo.LastSync : null;
            fullDeviceInfo.DeviceId = deviceInfo.DeviceId;
            fullDeviceInfo.SerialNumber = !string.IsNullOrEmpty(deviceInfo.SerialNumber) ? deviceInfo.SerialNumber : null;
            fullDeviceInfo.Status = !string.IsNullOrEmpty(deviceInfo.Status) ? deviceInfo.Status : null;
            fullDeviceInfo.User = !string.IsNullOrEmpty(deviceInfo.User) ? deviceInfo.User : null;
            progressBar.UpdateBarAndText(40, "Saving variables...");

            Globals.SetGlobalsFromBasicDeviceInfo(fullDeviceInfo);
            progressBar.UpdateBarAndText(55, "Populating fields...");
            if (!String.IsNullOrEmpty(fullDeviceInfo.Notes)) NoteField.Text = fullDeviceInfo.Notes; else NoteField.Text = "";
            if (!String.IsNullOrEmpty(fullDeviceInfo.AssetId)) AssetIdField.Text = fullDeviceInfo.AssetId; else AssetIdField.Text = "";
            if (!String.IsNullOrEmpty(fullDeviceInfo.Location)) LocationField.Text = fullDeviceInfo.Location; else LocationField.Text = "";
            if (!String.IsNullOrEmpty(fullDeviceInfo.User)) UserField.Text = fullDeviceInfo.User; else UserField.Text = "";
            if (!String.IsNullOrEmpty(fullDeviceInfo.OrgUnitPath)) OrganizationalUnitField.Text = fullDeviceInfo.OrgUnitPath; else OrganizationalUnitField.Text = "";

            if (!String.IsNullOrEmpty(fullDeviceInfo.Status))
            {
                switch(fullDeviceInfo.Status)
                {
                    case "ACTIVE":
                        StatusActiveRadio.IsChecked = true;
                        StatusDisabledRadio.IsEnabled = true;
                        StatusActiveRadio.IsEnabled = true;
                        StatusDeprovisionedRadio.IsEnabled = true;
                        break;
                    case "DISABLED":
                        StatusDisabledRadio.IsChecked = true;
                        StatusDisabledRadio.IsEnabled = true;
                        StatusActiveRadio.IsEnabled = true;
                        StatusDeprovisionedRadio.IsEnabled = true;
                        break;
                    case "DEPROVISIONED":
                        StatusDeprovisionedRadio.IsChecked = true;
                        StatusDeprovisionedRadio.IsEnabled = false;
                        StatusActiveRadio.IsChecked = false;
                        StatusActiveRadio.IsEnabled = false;
                        StatusDisabledRadio.IsChecked = false;
                        StatusDisabledRadio.IsEnabled = false;
                        break;
                }
            }

            progressBar.UpdateBarAndText(85, "Filling in output box...");
            outputField.Text = "Found device. ID: " + fullDeviceInfo.DeviceId + ".";
            if (!String.IsNullOrEmpty(fullDeviceInfo.SerialNumber)) outputField.Text += "\nSerial Number: " + fullDeviceInfo.SerialNumber;
            if (!String.IsNullOrEmpty(fullDeviceInfo.LastSync)) outputField.Text += "\nLast Sync: " + fullDeviceInfo.LastSync;

            progressBar.UpdateBarAndText(100, "Done!");
            IsLoading = false;
            progressBar.Close();
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
        private async void getInfoButton_Click(object sender, RoutedEventArgs e)
        {
            IsLoading = true;
            if (Globals.DeviceIdExists() == false)
            {
                outputField.Text = "No device ID currently in memory. Press " + submitDeviceId.Content + " then try again.";
                return;
            }
            else if (Globals.DeviceId == "csv")
            {
                GetInput.ShowInfoDialog("Not Supported", "This action is not supported with a CSV", "We don't support using CSVs for info because they put out unreadable output.");
                return;
            }
            string deviceId = Globals.DeviceId;
            //if (deviceId == null || deviceId.Length < 1) ;
            outputField.Text = await Task.Run(() => GAM.RunGAMFormatted("info cros " + Globals.DeviceId + " allfields"));
            IsLoading = false;
        }


        private void copyToClipboard_Click(object sender, RoutedEventArgs e)
        {
            if (outputField.Text.Length < 1)
            {
                outputField.Text = "Nothing was here to copy.";
            }
            Clipboard.SetText(outputField.Text, TextDataFormat.UnicodeText);
            outputField.Text += "\n\nCopied to clipboard.";
        }

        private void copyId_Click(object sender, RoutedEventArgs e)
        {
            if (Globals.DeviceId != null && Globals.DeviceId != "csv")
            {
                Clipboard.SetText(Globals.DeviceId, TextDataFormat.UnicodeText);
                outputField.Text += "\n\nCopied to clipboard.";
            }
            else if (Globals.DeviceId == "csv")
            {
                GetInput.ShowInfoDialog("Not Supported", "This action is not supported from a CSV", "We don't support copying all IDs from a CSV at this time.");
                return;
            }
            else
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
            }
            else
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
            LocationField.IsEnabled = value;
            AssetIdField.IsEnabled = value;
            UserField.IsEnabled = value;
            NoteField.IsEnabled = value;
            OrganizationalUnitField.IsEnabled = value;
            OrganizationalUnitBrowseButton.IsEnabled = value;
            CopyDeviceIdButton.IsEnabled = value;
            ApplyChangesButton.IsEnabled = value;
            RevertChangesButton.IsEnabled = value;
            GetInfoButton.IsEnabled = value;
        }

        private void OrganizationalUnitBrowseButton_Click(object sender, RoutedEventArgs e)
        {
            OrganizationalUnitField.Text = OrgUnit.GetOrgUnitFromSelector();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void RevertChangesButton_Click(object sender, RoutedEventArgs e)
        {
            // check to see which fields have been changed (ones that aren't the global or "<no value present>", should make this an independent function)
            if (LocationField.Text != Globals.Location) LocationField.Text = Globals.Location;
            if (AssetIdField.Text != Globals.AssetId) AssetIdField.Text = Globals.AssetId;
            if (UserField.Text != Globals.User && UserField.Text.Length > 0) UserField.Text = Globals.User;
            if (NoteField.Text != Globals.Note) NoteField.Text = Globals.Note;
            switch(Globals.Status)
            {
                case "ACTIVE":
                    StatusDisabledRadio.IsChecked = false;
                    StatusDeprovisionedRadio.IsChecked = false;
                    StatusActiveRadio.IsChecked = true;
                    break;
                case "DISABLED":
                    StatusDisabledRadio.IsChecked = true;
                    StatusDeprovisionedRadio.IsChecked = false;
                    StatusActiveRadio.IsChecked = false;
                    break;
                default:
                    break; // nothing, because if it's deprovisioned they can't check anything
            }
            if (OrganizationalUnitField.Text != Globals.OrgUnitPath && Globals.OrgUnitPath.Length > 0) OrganizationalUnitField.Text = Globals.OrgUnitPath;
        }

        private async void ApplyChangesButton_Click(object sender, RoutedEventArgs e)
        {
            IsLoading = true;
            // check to see which fields have been changed (ones that aren't the global or "<no value present>", should make this an independent function)
            ProgressBarDialog progressBar = GetInput.ShowProgressBarDialog("Updating Device", 50, "Updating device info...");
            progressBar.UpdateBarAndText(50, "Updating device info...");
            string gamCommand = "update cros " + Globals.DeviceId + " ";
            string outputText = "";

            if (LocationField.Text != Globals.Location)
            {
                if (LocationField.Text.Length < 1 && String.IsNullOrEmpty(Globals.Location))
                {
                    switch (GetInput.GetYesOrNo("Empty Location?", "Clear the field?", "Click yes if you want to empty the Location field. Click no to cancel."))
                    {
                        case "yes":
                            break;
                        case "no":
                            outputField.Text = "Cancelling because you didn't want to empty a field.";
                            progressBar.Close();
                            return;
                        default:
                            progressBar.Close();
                            return;
                    }
                }
                gamCommand += "location \"" + LocationField.Text + "\" ";
                Globals.Location = LocationField.Text;
                outputText += "Location, ";
            }
            if (AssetIdField.Text != Globals.AssetId)
            {
                if (AssetIdField.Text.Length < 1 && String.IsNullOrEmpty(Globals.AssetId)) {
                    switch(GetInput.GetYesOrNo("Empty Asset ID?", "Clear the field?", "Click yes if you want to empty the Asset ID field. Click no to cancel.")) {
                        case "yes":
                            break;
                        case "no":
                            outputField.Text = "Cancelling because you didn't want to empty a field.";
                            progressBar.Close();
                            return;
                        default:
                            progressBar.Close();
                            return;
                    }
                }
                gamCommand += "asset_id \"" + AssetIdField.Text + "\" ";
                Globals.AssetId = LocationField.Text;
                outputText += "Asset ID, ";

            }
            if (UserField.Text != Globals.User)
            {
                if (UserField.Text.Length < 1 && String.IsNullOrEmpty(Globals.User))
                {
                    switch (GetInput.GetYesOrNo("Empty User?", "Clear the field?", "Click yes if you want to empty the User field. Click no to cancel."))
                    {
                        case "yes":
                            break;
                        case "no":
                            outputField.Text = "Cancelling because you didn't want to empty a field.";
                            progressBar.Close();
                            return;
                        default:
                            progressBar.Close();
                            return;
                    }
                }
                gamCommand += "user \"" + UserField.Text + "\" ";
                Globals.User = UserField.Text;
                outputText += "User, ";
            }
            if (NoteField.Text != Globals.Note)
            {
                if (NoteField.Text.Length < 1 && String.IsNullOrEmpty(Globals.Note))
                {
                    switch (GetInput.GetYesOrNo("Empty Note?", "Clear the field?", "Click yes if you want to empty the Note field. Click no to cancel."))
                    {
                        case "yes":
                            break;
                        case "no":
                            outputField.Text = "Cancelling because you didn't want to empty a field.";
                            progressBar.Close();
                            return;
                        default:
                            progressBar.Close();
                            return;
                    }
                }
                gamCommand += "notes \"" + NoteField.Text.Replace("\"", "\\\"") + "\" "; // will output a note like this: "He told me \"Hello!\" yesterday."
                Globals.Note = NoteField.Text;
                outputText += "Note, ";
            }
            if (StatusDisabledRadio.IsChecked == true && Globals.Status != "DISABLED")
            {
                progressBar.UpdateBarAndText(55, "Disabling...");
                await Task.Run(() => GAM.RunGAM("update cros " + Globals.DeviceId + " action disable"));
                Globals.Status = "DISABLED";
                outputText += "Status, ";
            }
            if (StatusActiveRadio.IsChecked == true && Globals.Status != "ACTIVE")
            {
                progressBar.UpdateBarAndText(55, "Enabling...");
                await Task.Run(() => GAM.RunGAM("update cros " + Globals.DeviceId + " action reenable"));
                Globals.Status = "ACTIVE";
                outputText += "Status, ";
            }
            if (StatusDeprovisionedRadio.IsChecked == true && Globals.Status != "DEPROVISIONED")
            {
                //deprovision_same_model_replace|deprovision_different_model_replace|deprovision_retiring_device [acknowledge_device_touch_requirement]
                int depReason = GetInput.GetDeprovisionReason();
                switch (depReason)
                {
                    case 1:
                        progressBar.UpdateBarAndText(55, "Deprovisioning...");
                        await Task.Run(() => GAM.RunGAM("update cros " + Globals.DeviceId + " action deprovision_same_model_replace acknowledge_device_touch_requirement"));
                        break;
                    case 2:
                        progressBar.UpdateBarAndText(55, "Deprovisioning...");
                        await Task.Run(() => GAM.RunGAM("update cros " + Globals.DeviceId + " action deprovision_different_model_replace acknowledge_device_touch_requirement"));
                        break; // different
                    case 3: // retire
                        progressBar.UpdateBarAndText(55, "Deprovisioning...");
                        await Task.Run(() => GAM.RunGAM("update cros " + Globals.DeviceId + " action deprovision_retiring_device acknowledge_device_touch_requirement"));
                        break;
                    default:
                        outputField.Text = "No deprovision reason was selected so that choice was not saved.\n";
                        break; // do nothing: they selected nothing.
                }
                if (depReason != 0)
                {
                    Globals.Status = "DEPROVISIONED";
                    outputText += "Status, ";
                    StatusActiveRadio.IsEnabled = false;
                    StatusDeprovisionedRadio.IsEnabled = false;
                    StatusDisabledRadio.IsEnabled = false;
                }
            }
            if (OrganizationalUnitField.Text != Globals.OrgUnitPath && Globals.OrgUnitPath.Length > 0)
            {
                gamCommand += "ou " + OrganizationalUnitField.Text + " ";
                Globals.OrgUnitPath = OrganizationalUnitField.Text;
                outputText += "Orgizational Unit ";
            }

            if (OrganizationalUnitField.Text.Length < 1 || (OrganizationalUnitField.Text.Length == 1 && OrganizationalUnitField.Text[0] != '/'))
            {
                GetInput.ShowInfoDialog("No empty org unit path.", "Your org unit path can't be blank.", "You can't have a blank org unit path, silly!");
                return;
            }

            progressBar.UpdateBarAndText(75, "Updating info...");
            if (gamCommand != "update cros " + Globals.DeviceId + " ") // if something was changed
            {
                string gamOutput = await Task.Run(() => GAM.RunGAMFormatted(gamCommand));
                Console.WriteLine(gamOutput);
            }
            progressBar.UpdateBarAndText(99, "Finishing up...");
            IsLoading = false;
            if (outputText.Length > 1) outputField.Text = outputText += "was updated.";
            progressBar.Close();
            // build a GAM command from that information
            // run it
            // update globals
            // show success in output field
        }

        private bool _isLoading;
        public bool IsLoading
        {
            get
            {
                return _isLoading;
            }

            set
            {
                ToggleMainWindowButtons(!value);
                if (value == true)
                {
                    outputField.Text = "Loading...";
                } else
                {
                }
                _isLoading = value;
                return;
            }
        }

        public void ClearAndFocusInputBar()
        {
            deviceInputField.Focus();
            deviceInputField.Text = "";
        }

    }
}
