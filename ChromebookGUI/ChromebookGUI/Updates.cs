using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.Diagnostics;

namespace ChromebookGUI
{
    class Updates
    {
        public static string IsNewestVersion()
        {
            Console.WriteLine("Starting newest version check...");
            HttpClient http = Globals.HttpClientObject;
            Task<string> newestVersionsString = http.GetStringAsync("https://iamtheyammer.github.io/gam-cros-win-wrapper/releases.json");
            Dictionary<string, string> newestVersions = null;
            try
            {
                newestVersionsString.Wait();
                newestVersions = JsonConvert.DeserializeObject<Dictionary<string, string>>(newestVersionsString.Result);
            } catch (Exception e)
            {
                GetInput.ShowInfoDialog("Internet Issue", "No internet connection", "Could not connect to the internet to check for updates.\nPlease make sure that github.io is accessible on your network.\n\nThe full URL this software is trying to access is:\nhttps://iamtheyammer.github.io/gam-cros-win-wrapper/releases.json");
                return "error";
            }
            if(newestVersions == null)
            {
                GetInput.ShowInfoDialog("Error", "error checking for updates", "error!");
            }
            string currentVersion = Software.Version;
            string newestVersion = newestVersions[Software.Type];
            if (newestVersion == currentVersion)
            {
                return "true";
            } else
            {
                return "false";
            }
        }

        /// <summary>
        /// Returns "true" if an update is available, "false" if one is not and "error" if there was an error.
        /// String because I need more options. If a new update is available or there was an error it already opens a dialog!
        /// </summary>
        /// <returns></returns>
        public static string CheckForUpdates()
        {
            string isNewestVersion = Updates.IsNewestVersion();
            if (isNewestVersion == "false")
            {
                bool extraButtonPressed = GetInput.ShowInfoDialog("Update Available", "An update for this app is currently available.", "You are running " + Software.Type + " " + Software.Version + ".\n" + ((Software.Type != "alpha") ? "Update at https://github.com/iamtheyammer/gam-cros-win-wrapper/releases/latest" : "Update at https://github.com/iamtheyammer/gam-cros-win-wrapper/"), new Button { IsEnabled = true, Text = "Go to GitHub Repo" });
                if(extraButtonPressed)
                {
                    Process.Start("https://github.com/iamtheyammer/gam-cros-win-wrapper");
                }
                return "true";
            }
            else if (isNewestVersion == "error")
            {
                return "error";
                // nothing
            }
            else
            {
                return "false";
                // nothing
            }
        }
    }
}
