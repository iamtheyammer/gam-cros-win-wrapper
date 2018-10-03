using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;

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
    }
}
