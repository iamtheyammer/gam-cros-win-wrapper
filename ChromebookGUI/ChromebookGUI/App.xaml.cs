using ChromebookGUI.Classes;
using Sentry;
using System;
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
                if (Debug.IsDebugMode()) o.Debug = true; 
                o.BeforeSend = ((arg) => { Console.WriteLine("Sending..."); return arg; });
            });
            SentrySdk.ConfigureScope((scope) =>
            {
                scope.SetTag("software-type", Software.Type);
            });

            try
            {
                Preferences.Init();
                Task.Run(AutoComplete.Init);

                MainWindow window = new MainWindow();
                window.Show();

                if (Preferences.PromptWhenUpdatesAreAvailable)
                {
                    Updates.CheckForUpdates();
                }

            }
            catch (Exception err)
            {
                if (Debug.IsDebugMode())
                {
                    throw;
                }
                Debug.CaptureException(err);

            }


        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            Preferences.Save();
            AutoComplete.Save();
        }

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            if (Debug.IsDebugMode())
            {
                e.Handled = false;
                throw (e.Exception);
            }
            Debug.CaptureException(e.Exception);
        }
    }
}
