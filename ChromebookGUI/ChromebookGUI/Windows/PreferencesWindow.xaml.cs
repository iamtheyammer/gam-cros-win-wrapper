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
using System.Windows.Shapes;

namespace ChromebookGUI
{
    /// <summary>
    /// Interaction logic for Preferences.xaml
    /// </summary>
    public partial class PreferencesWindow : Window
    {
        public PreferencesWindow()
        {
            InitializeComponent();
        }

        private void SaveAndCloseButton_Click(object sender, RoutedEventArgs e)
        {
            // more settings go here

            Preferences.SerialNumberAssetIdPriority = (SearchForAssetIdsBeforeSerialNumbersCheckBox.IsChecked != false);
            Preferences.ShowWarningWhenImportingFromCSVFile = (ShowWarningWhenImportingFromCSVFile.IsChecked != false);
            Preferences.PromptWhenUpdatesAreAvailable = (PromptWhenUpdatesAreAvailableCheckBox.IsChecked != false);
            Preferences.UseTextBoxLayoutInsteadOfButtonLayout = (UseTextBoxLayoutInsteadOfButtonLayoutCheckBox.IsChecked != false);
            Preferences.UiMode = UseDarkModeCheckbox.IsChecked != null ? "dark" : "light";
            Preferences.Save();
            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
