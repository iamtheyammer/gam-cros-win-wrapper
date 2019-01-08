using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Diagnostics;
using System.IO;

namespace ChromebookGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        dynamic currentView = null; 
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (Preferences.UseTextBoxLayoutInsteadOfButtonLayout == true) RenderTextBoxView(); else RenderDefaultView();
        }

        private void RenderTextBoxView()
        {
            TextBoxView textBoxView = new TextBoxView();
            string currentOmnibarText = "";
            if (!string.IsNullOrEmpty(Globals.DeviceId)) currentOmnibarText = currentView.deviceInputField.Text;
            currentView = textBoxView;
            if (!string.IsNullOrEmpty(Globals.DeviceId))
            {
                currentView.deviceInputField.Text = currentOmnibarText;
                currentView.SubmitDeviceId_Click(new object(), new RoutedEventArgs());
            }
            ViewFrame.Navigate(currentView);
        }

        private void RenderDefaultView()
        {
            DefaultView defaultView = new DefaultView();
            string currentOmnibarText = "";
            if (!string.IsNullOrEmpty(Globals.DeviceId)) currentOmnibarText = currentView.deviceInputField.Text;
            currentView = defaultView;
            if (!string.IsNullOrEmpty(Globals.DeviceId))
            {
                currentView.deviceInputField.Text = currentOmnibarText;
                currentView.ToggleMainWindowButtons(true);
            }
            ViewFrame.Navigate(defaultView);
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
                GetInput.ShowInfoDialog("ChromebookGUI: No file selected", "No File Selected", "You didn't select a file or pressed cancel. Either way no changes have been made.");
                return;
            }
            Globals.ClearGlobals();
            Globals.CsvLocation = filePath;
            Globals.DeviceId = "csv";
            currentView.ToggleMainWindowButtons(true);

        }

        private void ImportFromGoogleAdminQueryStringBulk_Click(object sender, RoutedEventArgs e)
        {
            string inputBoxPrefill = "example: user:jsmith\nThat imports all devices with the user:jsmith.\nClick the link on the bottom left for more information.";
            string queryString = GetInput.getInput(
                "Enter an Admin Console query string.",
                inputBoxPrefill,
                "Import from Google Admin query string",
                new Button()
                {
                    IsEnabled = true,
                    Text = "Open Help Page"
                }
            );
            if(queryString == "ExtraButtonClicked")
            {
                Process.Start("https://support.google.com/chrome/a/answer/1698333#search");
                ImportFromGoogleAdminQueryStringBulk_Click(sender, e);
                return;
            } else if (queryString == inputBoxPrefill || queryString == null)
            {
                GetInput.ShowInfoDialog("ChromebookGUI: No input", "No Input", "No query string entered, silly goose!");
                return;
            }

            List<string> gamResult = GAM.RunGAM("print cros query \"" + queryString + "\"");
            if(gamResult.Count == 0)
            {
                GetInput.ShowInfoDialog("ChromebookGUI: Invalid Query String", "Invalid Query String", "That's an invalid query string. If you don't think it is, try running this in cmd:\n\ngam print cros query \"" + queryString + "\"");
                return;
            } else if (gamResult.Count < 2)
            {
                GetInput.ShowInfoDialog("ChromebookGUI: No Results", "No Results from Query String", "No results from that query (" + queryString + ")");
                return;
            }
            File.WriteAllLines(System.IO.Path.GetTempPath() + "ChromebookGUI.csv", gamResult.ToArray());
            Globals.ClearGlobals();
            Globals.CsvLocation = System.IO.Path.GetTempPath() + "ChromebookGUI.csv";
            Globals.DeviceId = "csv";
            currentView.ToggleMainWindowButtons(true);
            GetInput.ShowInfoDialog("ChromebookGUI: Success", "Successful Query", "Found " + (gamResult.Count - 1) + " devices using query " + queryString + "."); // subtract 1 because the first is "deviceId"
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

        private void HelpGuidesSearchingTheOmnibar_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("https://github.com/iamtheyammer/gam-cros-win-wrapper/blob/master/docs/instructions/Omnibar.md");
        }

        private void UseDefaultButtonLayout_Click(object sender, RoutedEventArgs e)
        {
            RenderDefaultView();
        }

        private void UseTextBoxLayout_Click(object sender, RoutedEventArgs e)
        {
            RenderTextBoxView();
        }
    }

}
