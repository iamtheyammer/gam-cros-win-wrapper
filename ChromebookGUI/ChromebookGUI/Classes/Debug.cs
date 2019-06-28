using Sentry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sentry.Protocol;

namespace ChromebookGUI.Classes
{
    class Debug
    {
        /// <summary>
        /// Gets whether default enhanced telemetry data is in scope.
        /// </summary>
        public static bool IsDefaultEnhancedTelemetryInScope { get; set; }

        public static bool IsDebugMode()
        {
#if DEBUG
            return true;
#else
            return false;
#endif
        }

        /// <summary>
        /// Captures an exception. If default ET is not in the scope,
        /// it'll add it, provided AllowET is true.
        /// </summary>
        /// <param name="e"></param>
        public static void CaptureException(Exception e)
        {
            if(IsDefaultEnhancedTelemetryInScope == false &&
                Preferences.AllowEnhancedTelemetry == true)
            {
                AddDefaultEnhancedTelemetryToScope();
            }
            SentrySdk.ConfigureScope((scope) =>
            {
                scope.SetExtra("globals", Globals.ToJsonString());
            });
            SentrySdk.CaptureException(e);
        }

        /// <summary>
        /// Takes in a Dictionary of data to add to the exception.
        /// Only adds it if AllowEnhancedTelemetry is true.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="extras"></param>
        public static void CaptureRichException(Exception e, Dictionary<string, object> extras)
        {
            SentrySdk.WithScope((scope) =>
            {
                if(Preferences.AllowEnhancedTelemetry == true)
                {
                    scope.SetExtras(extras);
                    scope.SetExtra("globals", Globals.ToJsonString());
                    scope.User = new User
                    {
                        Email = Globals.GetAndSaveUserEmail()
                    };
                }
                SentrySdk.CaptureException(e);
            });
        }

        /// <summary>
        /// Adds the default scope before sending an error to Sentry.
        /// (globals and user email)
        /// </summary>
        public static async void AddDefaultEnhancedTelemetryToScope()
        {
            if (IsDefaultEnhancedTelemetryInScope) return;
            await SentrySdk.ConfigureScopeAsync(async (scope) =>
            {
                scope.SetTag("softwareType", Software.Type);
                scope.User = new Sentry.Protocol.User
                {
                    Email = await Task.Run(Globals.GetAndSaveUserEmail)
                };

            });
            Console.WriteLine("Enhanced telemetry has been added to the scope.");
            IsDefaultEnhancedTelemetryInScope = true;
        }

        public static void ShowErrorMessage()
        {
            bool extraButtonClicked = GetInput.ShowInfoDialog(
                    "Error",
                    "An error has occured",
                    "An error has occured in ChromebookGUI. " +
                    (Preferences.AllowEnhancedTelemetry == true ?
                    "Thanks to your enhanced telemetry setting, data that " +
                    "should be enough to fix this has been sent. " :
                    "No enhanced telemetry data has been sent per your preference. ") +
                    "\n\nFeel free to try what you were doing again. If it continues crashing," +
                    " please open a GitHub issue.",
                    new Button
                    {
                        IsEnabled = true,
                        Text = "Open an issue..."
                    }
                );
            if(extraButtonClicked)
            {
                OpenReportIssueOrCrash();
            }
        }

        public static void OpenReportIssueOrCrash()
        {
            System.Diagnostics.Process.Start("https://github.com/iamtheyammer/gam-cros-win-wrapper/issues/new" +
                          "?assignees=iamtheyammer&labels=bug&template=issue-crash-report.md" +
                          "&title=%5BI%2FC%5D%20(title%20goes%20here)%20%7C%20" + DateTimeOffset.UtcNow.ToUnixTimeSeconds());
        }
    }
}
