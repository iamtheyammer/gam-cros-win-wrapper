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

        private void SubmitDeviceId_Click(object sender, RoutedEventArgs e)
        {
            outputField.Text = "Loading...";
            if (deviceInputField.Text.Length < 1 || deviceInputField.Text == "Output will appear here.")
            {
                deviceInputField.Text = "You must enter something into the field at the top.";
                return;
            }

            outputField.Text = GAM.GetDeviceId(deviceInputField.Text);
            string deviceId = GAM.GetDeviceId(deviceInputField.Text);
            Globals.DeviceId = deviceId;
            outputField.Text = "Found device. ID: " + deviceId + ".";
            //deviceInputField.Text = deviceId;
        }

        private void outputField_TextChanged(object sender, TextChangedEventArgs e)
        {

        }


        private void getInfoButton_Click(object sender, RoutedEventArgs e)
        {
            if(Globals.DeviceIdExists() == false)
            {
                outputField.Text = "No device ID currently in memory. Press " + submitDeviceId.Content + " then try again.";
                return;
            }
            string deviceId = Globals.DeviceId;
            //if (deviceId == null || deviceId.Length < 1) ;
            outputField.Text = GAM.RunGAMFormatted("info cros " + Globals.DeviceId + " allfields");
        }

        private void setLocationButton_Click(object sender, RoutedEventArgs e)
        {
            if (Globals.DeviceIdExists() == false)
            {
                outputField.Text = "No device ID currently in memory. Press " + submitDeviceId.Content + " then try again.";
                return;
            }
            string newLocation = GetInput.getInput("What would you like to set the location to?", "Enter a new location...");
            if(newLocation == null)
            {
                outputField.Text = "You didn't enter anything, silly goose!";
                return;
            }
            string gamResult = GAM.RunGAMFormatted("update cros " + Globals.DeviceId + " location " + newLocation);
            outputField.Text = gamResult + "\nAs long as you don't see an error, this query completed successfully.";
        }

        private void setAssetIdButton_Click(object sender, RoutedEventArgs e)
        {
            if (Globals.DeviceIdExists() == false)
            {
                outputField.Text = "No device ID currently in memory. Press " + submitDeviceId.Content + " then try again.";
                return;
            }
            string newAssetId = GetInput.getInput("What would you like to set the asset ID to?", "Enter a new asset ID...");
            if(newAssetId == null)
            {
                outputField.Text = "You didn't enter anything, silly goose!";
                return;
            }
            string gamResult = GAM.RunGAMFormatted("update cros " + Globals.DeviceId + " assetid " + newAssetId);
            outputField.Text = gamResult + "\nAs long as you don't see an error, this query completed successfully.";
        }

        private void setUserButton_Click(object sender, RoutedEventArgs e)
        {
            if (Globals.DeviceIdExists() == false)
            {
                outputField.Text = "No device ID currently in memory. Press " + submitDeviceId.Content + " then try again.";
                return;
            }
            string newUser = GetInput.getInput("What would you like to set the user to?", "Enter a new user...");
            if (newUser == null)
            {
                outputField.Text = "You didn't enter anything, silly goose!";
                return;
            }
            string gamResult = GAM.RunGAMFormatted("update cros " + Globals.DeviceId + " user " + newUser);
            outputField.Text = gamResult + "\nAs long as you don't see an error, this query completed successfully.";
        }

        private void disableButton_Click(object sender, RoutedEventArgs e)
        {
            if (Globals.DeviceIdExists() == false)
            {
                outputField.Text = "No device ID currently in memory. Press " + submitDeviceId.Content + " then try again.";
                return;
            }
            string gamResult = GAM.RunGAMFormatted("update cros " + Globals.DeviceId + " disable");
            outputField.Text = gamResult + "\nAs long as you don't see an error, this query completed successfully.";
        }

        private void enableButton_Click(object sender, RoutedEventArgs e)
        {
            if (Globals.DeviceIdExists() == false)
            {
                outputField.Text = "No device ID currently in memory. Press " + submitDeviceId.Content + " then try again.";
                return;
            }
            string gamResult = GAM.RunGAMFormatted("update cros " + Globals.DeviceId + " enable");
            outputField.Text = gamResult += "\nAs long as you don't see an error, this query completed successfully.";
        }

        private void changeOuButton_Click(object sender, RoutedEventArgs e)
        {
            // we'll use the radio selector here, not doing now
        }

        private void deprovisionButton_Click(object sender, RoutedEventArgs e)
        {
            int depChoice = GetInput.GetDeprovisionReason();
            string depAction = null;
            switch (depChoice) {
                case 0:
                    outputField.Text = "Either you cancelled or selected nothing.";
                    break;
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
            string gamOutput = GAM.RunGAMFormatted("update cros " + Globals.DeviceId + " action " + depAction + " acknowledge_device_touch_requirement");
            outputField.Text = gamOutput += "\nAs long as you don't see an error, this query completed successfully.";
        }

        private void noteButton_Click(object sender, RoutedEventArgs e)
        {
            if (Globals.DeviceIdExists() == false)
            {
                outputField.Text = "No device ID currently in memory. Press " + submitDeviceId.Content + " then try again.";
            }
            List<string> gamResult = GAM.RunGAM("info cros " + Globals.DeviceId + " fields notes");
            string note = null;
            if(gamResult.Count < 2)
            {
                note = "No note found.";
            } else
            {
                note = gamResult[1].Substring(8);
            }
            Console.WriteLine("GAMRESULT:");
            Console.WriteLine(gamResult);
            string newNote = GetInput.getInput("Edit/modify note:", note);

            if (newNote == null)
            {
                outputField.Text = "You didn't change the note so I'm leaving it as it is.";
                return;
            }
            string finalGamResult = GAM.RunGAMFormatted("update cros " + Globals.DeviceId + " notes \"" + newNote + "\"");
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
            if(Globals.DeviceId != null)
            {
                Clipboard.SetText(Globals.DeviceId, TextDataFormat.UnicodeText);
                outputField.Text += "\n\nCopied to clipboard.";
            }
            else
            {
                outputField.Text += "\n\nNo device ID currently in memory.";
            }
        }

        private void deviceInputField_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void deviceInputField_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }

        private void selectAllTextInBox(object sender, object e)
        {
            deviceInputField.SelectAll();
        }

    }

}
