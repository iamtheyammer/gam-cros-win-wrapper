using ChromebookGUI.Classes;
using ChromebookGUI.Windows;
using Sentry;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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

        private void DeviceInputField_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Escape) return;
                AutoComplete.OnKeyUp(sender, e, deviceInputFieldStack, deviceInputField);
            } catch (Exception err)
            {
                Debug.CaptureRichException(err, new Dictionary<string, object>
                {
                    ["pressed-key"] = e.Key.ToString(),
                    ["omnibar-value"] = deviceInputField.Text
                    
                });
                Debug.ShowErrorMessage();
            }
            
        }

        private void DeviceInputField_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            switch(e.Key)
            {
                case Key.Down:
                    AutoComplete.FocusNextCompletion(deviceInputFieldStack, deviceInputField);
                    return;
                case Key.Escape:
                    AutoComplete.Close(deviceInputFieldStack);
                    return;
                //case Key.Up:
                //    AutoComplete.FocusPreviousCompletion(deviceInputFieldStack);
                //    return;
                default:
                    return;
            }
        }

        /// <summary>
        /// Essentially, run GAM.GetDeviceId() on whatever is entered into the text field.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void SubmitDeviceId_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                IsLoading = true;
                AutoComplete.Close(deviceInputFieldStack);
                if (deviceInputField.Text.Length < 1 || deviceInputField.Text.ToLower() == "enter a device id, asset id, serial number, query string or email...")
                {
                    outputField.Text = "You must enter something into the field at the top.";
                    return;
                }

                string input = deviceInputField.Text;
                List<BasicDeviceInfo> possibleDevices = await Task.Run(() => GAM.GetDeviceId(input));
                BasicDeviceInfo deviceInfo = BasicDeviceInfo.HandleGetDeviceId(possibleDevices);
                Globals.ClearGlobals(); // clear the globals before adding new ones
                Globals.SetGlobalsFromBasicDeviceInfo(deviceInfo);
                if (deviceInfo.Error)
                {
                    outputField.Text = deviceInfo.ErrorText;
                    return;
                }

                Globals.SetGlobalsFromBasicDeviceInfo(deviceInfo);
                NoteField.Text = !string.IsNullOrEmpty(deviceInfo.Notes) ? deviceInfo.Notes : "";
                AssetIdField.Text = !string.IsNullOrEmpty(deviceInfo.AssetId) ? deviceInfo.AssetId : "";
                LocationField.Text = !string.IsNullOrEmpty(deviceInfo.Location) ? deviceInfo.Location : "";
                UserField.Text = !string.IsNullOrEmpty(deviceInfo.User) ? deviceInfo.User : "";
                OrganizationalUnitField.Text = !string.IsNullOrEmpty(deviceInfo.OrgUnitPath) ? deviceInfo.OrgUnitPath : "";

                if (!String.IsNullOrEmpty(deviceInfo.Status))
                {
                    switch(deviceInfo.Status)
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

                outputField.Text = "Found device. ID: " + deviceInfo.DeviceId + ".";
                if (!String.IsNullOrEmpty(deviceInfo.SerialNumber)) outputField.Text += "\nSerial Number: " + deviceInfo.SerialNumber;
                if (!String.IsNullOrEmpty(deviceInfo.LastSync)) outputField.Text += "\nLast Sync: " + deviceInfo.LastSync;

                AutoComplete.AddItemToList(input);
                IsLoading = false;
                //deviceInputField.Text = deviceId;
            } catch (Exception err)
            {
                Debug.CaptureRichException(err, CollectFieldsForRichException());
                Debug.ShowErrorMessage();
            }
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

            if (Globals.DeviceId == "csv")
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
            if (deviceInputField.Text == "Enter a Device ID, Asset ID, Query String, Serial Number or Email...")
            {
                deviceInputField.Text = String.Empty;
            }
        }

        private void FontSizeUpButton_Click(object sender, RoutedEventArgs e)
        {
            outputField.FontSize += 2;
        }

        private void FontSizeDownButton_Click(object sender, RoutedEventArgs e)
        {
            if ((outputField.FontSize - 2) > 0)
            {
                outputField.FontSize -= 2;
            }
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

        private async void OrganizationalUnitBrowseButton_Click(object sender, RoutedEventArgs e)
        {
            string currentOutput = outputField.Text;
            IsLoading = true;
            string orgUnit = OrgUnit.HandleAwaitableGetOrgUnitFromSelector(await Task.Run(OrgUnit.AwaitableGetOrgUnitFromSelector));
            IsLoading = false;
            if(string.IsNullOrEmpty(orgUnit))
            {
                GetInput.ShowInfoDialog("No org unit selected",
                    "No org unit selected",
                    "For best results, please select an org unit.");
                outputField.Text = currentOutput;
                return;
            } else
            {
                OrganizationalUnitField.Text = orgUnit;
            }
            outputField.Text = currentOutput;
        }
        
        private void RevertChangesButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // check to see which fields have been changed (ones that aren't the global or "<no value present>", should make this an independent function)
                if (LocationField.Text != Globals.Location) LocationField.Text = Globals.Location;
                if (AssetIdField.Text != Globals.AssetId) AssetIdField.Text = Globals.AssetId;
                if (UserField.Text != Globals.User && UserField.Text.Length > 0) UserField.Text = Globals.User;
                if (NoteField.Text != Globals.Note) NoteField.Text = Globals.Note;
                switch (Globals.Status)
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
            catch (Exception err)
            {
                Debug.CaptureRichException(err, new Dictionary<string, object>
                {

                });

            }
        }

        private async void ApplyChangesButton_Click(object sender, RoutedEventArgs e)
        {
            
            try
            {
                IsLoading = true;
                // check to see which fields have been changed (ones that aren't the global or "<no value present>", should make this an independent function)
                ProgressBarDialog progressBar = GetInput.ShowProgressBarDialog(title: "Updating Device", initialProgressBarValue: 50, initialProgressText: "Updating device info...");
                progressBar.UpdateBarAndText(value: 50, text: "Updating device info...");
                string gamCommand = "update cros " + Globals.DeviceId + " ";
                string outputText = "";

                if (LocationField.Text != Globals.Location)
                {
                    if (ShouldEmptyField(modifiedText: LocationField.Text, originalText: Globals.Location))
                    {
                        switch (GetInput.GetYesOrNo(title: "Empty Location?", headline: "Clear the field?", instructionText: "Click yes if you want to empty the Location field. Click no to cancel."))
                        {
                            case "yes":
                                break;
                            case "no":
                                outputField.Text = "Cancelling because you didn't want to empty a field.";
                                progressBar.Close();
                                IsLoading = false;
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
                    if (ShouldEmptyField(modifiedText: AssetIdField.Text, originalText: Globals.AssetId))
                    {
                        switch (GetInput.GetYesOrNo(title: "Empty Asset ID?", headline: "Clear the field?", instructionText: "Click yes if you want to empty the Asset ID field. Click no to cancel."))
                        {
                            case "yes":
                                break;
                            case "no":
                                outputField.Text = "Cancelling because you didn't want to empty a field.";
                                progressBar.Close();
                                IsLoading = false;
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
                    if (ShouldEmptyField(modifiedText: UserField.Text, originalText: Globals.User))
                    {
                        switch (GetInput.GetYesOrNo(title: "Empty User?", headline: "Clear the field?", instructionText: "Click yes if you want to empty the User field. Click no to cancel."))
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
                    if (ShouldEmptyField(modifiedText: NoteField.Text, originalText: Globals.Note))
                    {
                        switch (GetInput.GetYesOrNo(title: "Empty Note?", headline: "Clear the field?", instructionText: "Click yes if you want to empty the Note field. Click no to cancel."))
                        {
                            case "yes":
                                break;
                            case "no":
                                outputField.Text = "Cancelling because you didn't want to empty a field.";
                                progressBar.Close();
                                IsLoading = false;
                                return;
                            default:
                                progressBar.Close();
                                return;
                        }
                    }
                    gamCommand += "notes \"" + NoteField.Text.Replace(oldValue: "\"", newValue: "\\\"") + "\" "; // will output a note like this: "He told me \"Hello!\" yesterday."
                    Globals.Note = NoteField.Text;
                    outputText += "Note, ";
                }
                if (StatusDisabledRadio.IsChecked == true && Globals.Status != "DISABLED")
                {
                    progressBar.UpdateBarAndText(value: 55, text: "Disabling...");
                    await Task.Run(function: () => GAM.RunGAM(gamCommand: "update cros " + Globals.DeviceId + " action disable"));
                    Globals.Status = "DISABLED";
                    outputText += "Status, ";
                }
                if (StatusActiveRadio.IsChecked == true && Globals.Status != "ACTIVE")
                {
                    progressBar.UpdateBarAndText(value: 55, text: "Enabling...");
                    await Task.Run(function: () => GAM.RunGAM(gamCommand: "update cros " + Globals.DeviceId + " action reenable"));
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
                            progressBar.UpdateBarAndText(value: 55, text: "Deprovisioning...");
                            await Task.Run(function: () => GAM.RunGAM(gamCommand: "update cros " + Globals.DeviceId + " action deprovision_same_model_replace acknowledge_device_touch_requirement"));
                            break;
                        case 2:
                            progressBar.UpdateBarAndText(value: 55, text: "Deprovisioning...");
                            await Task.Run(function: () => GAM.RunGAM(gamCommand: "update cros " + Globals.DeviceId + " action deprovision_different_model_replace acknowledge_device_touch_requirement"));
                            break; // different
                        case 3: // retire
                            progressBar.UpdateBarAndText(value: 55, text: "Deprovisioning...");
                            await Task.Run(function: () => GAM.RunGAM(gamCommand: "update cros " + Globals.DeviceId + " action deprovision_retiring_device acknowledge_device_touch_requirement"));
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

                if (OrganizationalUnitField.Text.Length < 1 || (OrganizationalUnitField.Text.Length == 1 && OrganizationalUnitField.Text[index: 0] != '/'))
                {
                    GetInput.ShowInfoDialog(title: "No empty org unit path.", subject: "Your org unit path can't be blank.", fullText: "You can't have a blank org unit path, silly!");
                    return;
                }

                progressBar.UpdateBarAndText(value: 75, text: "Updating info...");
                if (gamCommand != "update cros " + Globals.DeviceId + " ") // if something was changed
                {
                    string gamOutput = await Task.Run(function: () => GAM.RunGAMFormatted(gamCommand: gamCommand));
                    Console.WriteLine(value: gamOutput);
                }
                progressBar.UpdateBarAndText(value: 99, text: "Finishing up...");
                IsLoading = false;
                if (outputText.Length > 1) outputField.Text = outputText += "was updated.";
                progressBar.Close();
                // build a GAM command from that information
                // run it
                // update globals
                // show success in output field
            }
            catch (Exception err)
            {
                Debug.CaptureRichException(err, CollectFieldsForRichException());
                Debug.ShowErrorMessage();
            }
        }

        private bool ShouldEmptyField(string modifiedText, string originalText)
        {
            return (
                (modifiedText != originalText) &&
                (string.IsNullOrEmpty(modifiedText) && !string.IsNullOrEmpty(originalText))
                );
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

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            AutoComplete.Close(deviceInputFieldStack);
        }

        private Dictionary<string, object> CollectFieldsForRichException()
        {
            return new Dictionary<string, object>
            {
                [key: "omnibar-text"] = deviceInputField.Text,
                [key: "note-field-text"] = NoteField.Text,
                [key: "assetid-field-text"] = AssetIdField.Text,
                [key: "location-field-text"] = LocationField.Text,
                [key: "user-field-text"] = UserField.Text,
                [key: "organizational-unit-field-text"] = OrganizationalUnitField.Text
            };
        }
    }
}
