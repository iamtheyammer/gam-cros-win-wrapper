﻿using System;
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
            window.ToggleMainWindowButtons(false);
            window.Show();

            Preferences.Init();

            if(Preferences.PromptWhenUpdatesAreAvailable == true)
            {
                Updates.CheckForUpdates();
            }
            
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            Preferences.Save();
        }
    }
}
