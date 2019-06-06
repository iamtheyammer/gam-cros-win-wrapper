using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace ChromebookGUI.Main_Window_Views
{
    /// <summary>
    /// Interaction logic for WelcomePage.xaml
    /// </summary>
    public partial class WelcomePage : Page
    {
        public WelcomePage()
        {
            InitializeComponent();
        }

        private void ViewPrivacyPolicyButton_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("https://github.com/iamtheyammer/gam-cros-win-wrapper/tree/master/PrivacyPolicy.md");
        }

        private void NoEnhancedTelemetryButton_Click(object sender, RoutedEventArgs e)
        {
            Preferences.AllowEnhancedTelemetry = false;
            Preferences.NeedsTelemetryConsent = false;
            // go to the text box view
            NavigationService?.Navigate(new TextBoxView());
        }

        private void AllowEnhancedTelemetry_Click(object sender, RoutedEventArgs e)
        {
            Preferences.AllowEnhancedTelemetry = true;
            Preferences.NeedsTelemetryConsent = false;
            // go to the text box view
            NavigationService?.Navigate(new TextBoxView());
        }

        private async void IsGAMSetupCorrectlyButton_Click(object sender, MouseEventArgs e)
        {
            string currentText = (sender as TextBlock).Text;
            IsGAMSetupCorrectlyButton.Text = "Checking...";
            string email = "";
            try
            {
                email = await Task.Run(GAM.GetUserEmail);
            }
            catch
            {
                // ignored
            }

            bool isSetupCorrectly = email.Length > 1 && email.Contains('@');
            Console.WriteLine("GAM is " + (isSetupCorrectly ? "" : "not ") + "set up correctly. Email: " + email);
            if (!isSetupCorrectly)
            {
                bool extraButtonClicked = GetInput.ShowInfoDialog(
                        "GAM Setup",
                        "There's something wrong.",
                        "GAM isn't working correctly. " +
                              "Please check if it's installed and that it's in your path. " +
                              "Click the button at the bottom left for instructions to add GAM to your path.",
                        new Button
                        {
                            IsEnabled = true,
                            Text = "Add GAM to the path"
                        }
                    );
                if (extraButtonClicked)
                    Process.Start(
                        "https://github.com/iamtheyammer/gam-cros-win-wrapper/blob/master/docs/instructions/AddingGAMToYourPath.md");
            }
            IsGAMSetupCorrectlyButton.Text = (isSetupCorrectly ? "GAM is working. (click here to test again)" : "GAM isn't working. (click to try again)");
        }

        private void EnableDarkMode_Checked(object sender, RoutedEventArgs e)
        {
            Console.WriteLine((sender as CheckBox).IsChecked);
        }
    }
}
