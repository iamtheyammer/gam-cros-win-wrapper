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
            //deviceInputField.Text = "Hello, WPF!";
            /*List<string> gam = GAM.RunGAM("print users");
            string result = null;
            if (gam == null)
            {
                outputField.Text = "No output from GAM.";
                return;
            }
            
            foreach (string line in gam)
            {
                result += line + "\n";
            }
            outputField.Text = result;*/
            if (deviceInputField.Text.Length < 1 || deviceInputField.Text == "Output will appear here.")
            {
                deviceInputField.Text = "You must enter something into the field at the top.";
                return;
            }

            outputField.Text = GAM.GetDeviceId(deviceInputField.Text);
        }

        private void outputField_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        public class GAM
        {
            public static List<string> RunGAM(String gamCommand)
            {
                // this function lets us run gam, which is pretty important

                var proc = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "gam.exe",
                        Arguments = gamCommand, // this must be passed in without the "gam"
                        UseShellExecute = false,
                        RedirectStandardOutput = true, // let us read the output here
                        CreateNoWindow = true // don't open a cmd window
                    }
                };

                List<string> output = new List<string>();
                proc.Start();

                while (!proc.StandardOutput.EndOfStream) // while it's still running
                {
                    output.Add(proc.StandardOutput.ReadLine()); // append the line to output

                }
                return output;
            }

            public static String GetDeviceId(String input)
            {
                if (input.Length == 36) // this is already a device ID
                {
                    return input;
                }
                else if (input.Contains("@"))
                {
                    // deal with emails later.
                }
                else
                {
                    // must be a serial number. 
                    List<string> response = RunGAM("print cros query \"id:" + input + "\"");
                    if (response.Count < 2) return "No results found for that entry (" + input + ".)"; // this count thing does NOT start at zero. ugh!
                    return response[1];
                }
                return "hi";
            }
        }

    }

}
