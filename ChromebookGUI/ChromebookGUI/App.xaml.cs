using ChromebookGUI.Classes;
using Sentry;
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
            IDisposable sentry = SentrySdk.Init(o =>
            {
                o.Dsn = new Dsn("https://dd9a6666551c4d91b31209f52d7bd7ba@sentry.io/1457501");
                o.Release = Software.Version;
                o.Debug = true;
                o.BeforeSend = ((arg) => { Console.WriteLine("Sending..."); return arg; });
            });

            try
            {
                Preferences.Init();
                Task.Run(() => AutoComplete.Init());

                MainWindow window = new MainWindow();
                window.Show();

                if (Preferences.PromptWhenUpdatesAreAvailable == true)
                {
                    Updates.CheckForUpdates();
                }

            }
            catch (Exception err)
            {
                if (ChromebookGUI.Classes.Debug.IsDebugMode() == true) return;
                string decision = GetInput.GetYesOrNo("An error occured", "An error has occured.",
                    "Would it be OK with you if we sent details about your current device and your email to the developer? " +
                    "It really helps fix bugs, and we will never share the info.");
                if (decision == "yes")
                {
                    SentrySdk.ConfigureScope(scope =>
                    {
                        scope.SetExtra("globals", Globals.ToJsonString());
                        scope.User = new Sentry.Protocol.User
                        {
                            Email = GAM.GetUserEmail()
                        };
                    });
                }
                SentrySdk.CaptureException(err);
            }

            
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            Preferences.Save();
            AutoComplete.Save();
        }
         
        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            if (Debug.IsDebugMode() == true) return;
            string decision = GetInput.GetYesOrNo("An error occured", "An error has occured.",
                "Would it be OK with you if we sent details about your current device and your email to the developer? " +
                "It really helps fix bugs, and we will never share the info.");
            if (decision == "yes")
            {
                SentrySdk.ConfigureScope(scope =>
                {
                    scope.SetExtra("globals", Globals.ToJsonString());
                    scope.User = new Sentry.Protocol.User
                    {
                        Email = GAM.GetUserEmail()
                    };
                });
            }
            SentrySdk.CaptureException(e.Exception);
        }
    }
}
