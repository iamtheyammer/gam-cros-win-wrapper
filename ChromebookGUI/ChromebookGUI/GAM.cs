using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ChromebookGUI
{
    public class GAM
    {
        public static List<string> RunGAM(String gamCommand)
        {
            // this function lets us run gam, which is pretty important

            var proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "gam.exe",
                    Arguments = gamCommand, // this must be passed in without the "gam"
                    UseShellExecute = false,
                    RedirectStandardOutput = true, // let us read the output here
                    RedirectStandardError = true,
                    CreateNoWindow = true // don't open a cmd window
                }
            };

            List<string> output = new List<string>();
            proc.Start();

            while (!proc.StandardOutput.EndOfStream) // while it's still running
            {
                //Console.WriteLine("recieved output from GAM...");
                output.Add(proc.StandardOutput.ReadLine()); // append the line to output
                //Console.WriteLine("Standard output:");
                //Console.WriteLine(proc.StandardOutput.ReadLine());
                //Console.WriteLine("Error output:");
                //Console.WriteLine(proc.StandardError.ReadLine());

            }
            proc.Close();
            return output;
        }

        /// <summary>
        /// Runs GAM, but returns a newline character seperated string, ready for display.
        /// </summary>
        /// <param name="gamCommand"></param>
        /// <returns>A newline character seperated string, ready for display.</returns>
        public static String RunGAMFormatted(String gamCommand)
        {
            string result = null;
            foreach(string line in RunGAM(gamCommand))
            {
                result += "\n" + line;
            }
            return result;
        }

        /// <summary>
        /// Get the GAM command for the CSV. Turns a normal GAM command into one that pulls from a CSV. Can be fed into GAM.RunGAM or GAM.RunGAMFormatted.
        /// </summary>
        /// <param name="csvLocation">The absolute path to your CSV file. Get it with GetInput.GetFileSelection("csv").</param>
        /// <param name="gamCommandBeforeDeviceId">The GAM command you would run before the device id parameter.</param>
        /// <param name="gamCommandAfterDeviceId">The rest of the GAM command, after the device id.</param>
        /// <returns>The GAM command that can be fed into GAM.RunGAM or GAM.RunGAMFormatted.</returns>
        public static string GetGAMCSVCommand(String csvLocation, String gamCommandBeforeDeviceId, String gamCommandAfterDeviceId)
        {
            return "csv " + csvLocation + " " + gamCommandBeforeDeviceId + " ~deviceId " + gamCommandAfterDeviceId;
        }

        /// <summary>
        /// Runs GAM and FixCSVCommas.FixCommas
        /// </summary>
        /// <param name="gamCommand"></param>
        /// <returns></returns>
        public static List<List<string>> RunGAMCommasFixed(String gamCommand)
        {
            return FixCSVCommas.FixCommas(GAM.RunGAM(gamCommand));
        }
       

        /// <summary>
        /// Checks if a string is a device ID by seeing if it has 4 dashes in it.
        /// </summary>
        /// <param name="input">The string you wish to check.</param>
        /// <returns></returns>
        public static bool IsDeviceId(string input)
        {
            if(Regex.Matches(input, "-").Count == 4)
            {
                return true;
            } else
            {
                return false;
            }
        }

        /// <summary>
        /// Gets a Device ID, from a variety of sources (email, serial number, asset id, email)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static BasicDeviceInfo GetDeviceId(String input)
        {
            if (IsDeviceId(input)) // this is already a device ID
            {
                return new BasicDeviceInfo
                {
                    DeviceId = input
                };
            }
            else if (input.Contains("@"))
            {
                string user = input.Split('@')[0];
                if (String.IsNullOrEmpty(user))
                {
                    return new BasicDeviceInfo
                    {
                        ErrorText = "There was an error processing your email entry: I couldn't find the email.",
                        Error = true
                    };
                }
                return GetDeviceByGAMQuery("user:" + user);
            } else if (input.Contains(":")) {
                return GetDeviceByGAMQuery(input);
            } else

            {
                // must be a serial number/asset id. 
                
                // obey the preference here
                if (Preferences.SerialNumberAssetIdPriority)
                {
                    BasicDeviceInfo firstTry = GetDeviceByGAMQuery("asset_id:" + input);
                    if (!firstTry.Error) return firstTry;
                    return GetDeviceByGAMQuery("id:"+ input);
                }
                else
                {
                    BasicDeviceInfo firstTry = GetDeviceByGAMQuery("id:" + input);
                    if (!firstTry.Error) return firstTry;
                    return GetDeviceByGAMQuery("asset_id:" + input);
                }
            }
        }

        /// <summary>
        /// Takes in a GAM query, and returns a single device. 
        /// </summary>
        /// <param name="query">The gam query to search for: like "user:myUser"</param>
        /// <returns></returns>
        public static BasicDeviceInfo GetDeviceByGAMQuery(string query)
        {
            List<string> gamResults = RunGAM("print cros query \"" + query + "\" fields deviceId,lastSync,serialNumber,status,user");
            Dictionary<string, int> fieldOrder = new Dictionary<string, int>();
            if (gamResults[0].StartsWith("deviceId"))
            {
                string[] gamFieldOrder = gamResults[0].Split(',');
                for (int i = 0; i < gamResults[0].Split(',').Length; i++)
                {
                    fieldOrder.Add(gamFieldOrder[i], i);
                }
                gamResults.RemoveAt(0);
            }
            if (gamResults.Count == 0) return new BasicDeviceInfo()
            {
                Error = true,
                ErrorText = "Invalid query string or no results for that query string.",
            };
            List<List<string>> deviceInfos = FixCSVCommas.FixCommas(gamResults);
            if (deviceInfos.Count == 1) if (deviceInfos[0].Count > 1) // using 2 if statements here because I can't check for [0] if it doesn't exist
                {
                    return new BasicDeviceInfo()
                    {
                        DeviceId = deviceInfos[0][fieldOrder["deviceId"]],
                        LastSync = deviceInfos[0][fieldOrder["lastSync"]],
                        SerialNumber = deviceInfos[0][fieldOrder["serialNumber"]],
                        Status = deviceInfos[0][fieldOrder["status"]],
                        User = deviceInfos[0][fieldOrder["annotatedUser"]],
                        Error = false
                    };
                }
            // there is more than one result
            Console.WriteLine(gamResults[0]);
            List<BasicDeviceInfo> infoObjects = new List<BasicDeviceInfo>();
            for (int i = 0; i < deviceInfos.Count; i++)
            {

                infoObjects.Add(new BasicDeviceInfo()
                {
                    DeviceId = deviceInfos[i][fieldOrder["deviceId"]],
                    LastSync = deviceInfos[i][fieldOrder["lastSync"]],
                    SerialNumber = deviceInfos[i][fieldOrder["serialNumber"]],
                    Status = deviceInfos[i][fieldOrder["status"]],
                    User = deviceInfos[i][fieldOrder["annotatedUser"]],
                    Error = false
                });
            }
            List<string> selection = GetInput.GetDataGridSelection("Which device would you like to select?", "Click on a row or enter a Device ID or Serial Number.", "Device Selector", infoObjects);
            string deviceId = null;
            foreach (string item in selection)
            {
                if (IsDeviceId(item)) deviceId = item;
                break;
            }
            if (deviceId == null)
            {
                return new BasicDeviceInfo
                {
                    ErrorText = "There was an error. You didn't pick anything.",
                    Error = true
                };
            }
            else
            {
                for (int i = 1; i < deviceInfos.Count; i++)
                {
                    if (deviceId == deviceInfos[i][0])
                    {
                        return new BasicDeviceInfo
                        {
                            DeviceId = deviceInfos[i][fieldOrder["deviceId"]],
                            LastSync = deviceInfos[i][fieldOrder["lastSync"]],
                            SerialNumber = deviceInfos[i][fieldOrder["serialNumber"]],
                            Status = deviceInfos[i][fieldOrder["status"]],
                            User = deviceInfos[i][fieldOrder["annotatedUser"]],
                            Error = false
                        };
                    }
                }
                // we have not found a match for the device id
                return new BasicDeviceInfo
                {
                    DeviceId = deviceId,
                    Error = true,
                    ErrorText = "No devices found for that query. (404)"
                };
            }
        }
    }
}
