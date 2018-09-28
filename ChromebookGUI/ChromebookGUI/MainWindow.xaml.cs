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
            string deviceId = Globals.DeviceId;
            //if (deviceId == null || deviceId.Length < 1) ;
            InputWindow getInput = new InputWindow();
            getInput.instructionTextBlock.Text
            outputField.Text = GAM.RunGAMFormatted("info cros " + Globals.DeviceId + " allfields");
        }

        private void setLocationButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }

}
