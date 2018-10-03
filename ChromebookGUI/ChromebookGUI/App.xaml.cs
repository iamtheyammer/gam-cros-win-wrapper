using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ChromebookGUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            MainWindow window = new MainWindow();
            window.Show();

            string isNewestVersion = Updates.IsNewestVersion();
            if (isNewestVersion == "false")
            {
                GetInput.ShowInfoDialog("Update Available", "An update for this app is currently available.", "You are running " + Software.Type + " " + Software.Version + ".\n" + ((Software.Type != "alpha") ? "Update at https://github.com/iamtheyammer/gam-cros-win-wrapper/releases/latest" : "Update at https://github.com/iamtheyammer/gam-cros-win-wrapper/"));
                Console.WriteLine("Shown dialog.");
                
            } else if (isNewestVersion == "error")
            {
                // nothing
            } else
            {
                // nothing
            }
        }
    }
}
