﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using CsvHelper;
using CsvHelper.Configuration.Attributes;

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

            // this function has no try catch because everything using it should
            // have its own
        }

        public static StreamReader RunGAMStream(string gamCommand)
        {
            // this function lets us run gam, which is pretty important

            using (Process gam = new Process())
            {
                gam.StartInfo.FileName = "gam.exe";
                gam.StartInfo.Arguments = gamCommand;
                gam.StartInfo.UseShellExecute = false;
                gam.StartInfo.RedirectStandardOutput = true;
                gam.StartInfo.CreateNoWindow = true;

                gam.Start();

                return gam.StandardOutput;
            }
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
        /// Checks if a string is a device ID by seeing if it matches a regex.
        /// https://www.regexpal.com/?fam=109206
        /// </summary>
        /// <param name="input">The string you wish to check.</param>
        /// <returns></returns>
        public static bool IsDeviceId(string input)
        {
            MatchCollection matches = Regex.Matches(input, "[a-z0-9]{8}-([a-z0-9]{4}-){3}[a-z0-9]{11,13}");
            if(matches.Count == 1 && matches[0].Length == input.Length)
            {
                return true;
            } else
            {
                return false;
            }
        }

        /// <summary>
        /// Gets a Device ID, from a variety of sources (email, serial number, asset id, email). 
        /// Almost all of the time, you'll want to handle output from this with BasicDeviceInfo.HandleGetDeviceId.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static List<BasicDeviceInfo> GetDeviceId(string input)
        {

            if (IsDeviceId(input)) // this is already a device ID
            {
                return new List<BasicDeviceInfo>
                {
                    new BasicDeviceInfo()
                    {
                        DeviceId = input
                    }
                };
            }
            else if (input.Contains("@"))
            {
                string user = input.Split('@')[0];
                if (String.IsNullOrEmpty(user))
                {
                    return new List<BasicDeviceInfo>
                    {
                        new BasicDeviceInfo()
                        {
                            ErrorText = "There was an error processing your email entry: I couldn't find the email.",
                            Error = true
                        } 
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
                    List<BasicDeviceInfo> firstTry = GetDeviceByGAMQuery("asset_id:" + input);
                    if (!firstTry[0].Error) return firstTry;
                    return GetDeviceByGAMQuery("id:"+ input);
                }
                else
                {
                    List<BasicDeviceInfo> firstTry = GetDeviceByGAMQuery("id:" + input);
                    if (!firstTry[0].Error) return firstTry;
                    return GetDeviceByGAMQuery("asset_id:" + input);
                }
            }
        }

        /// <summary>
        /// Takes in a GAM query, and returns a single device. 
        /// </summary>
        /// <param name="query">The gam query to search for: like "user:myUser"</param>
        /// <returns></returns>
        private static List<BasicDeviceInfo> GetDeviceByGAMQuery(string query)
        {
            StreamReader gamResults = RunGAMStream("print cros query \"" + query +
                                                   "\" fields deviceId,lastSync,serialNumber,status,user,location,assetId,notes,orgUnitPath");
            using (var csv = new CsvReader(gamResults))
            {
                csv.Configuration.HeaderValidated = null;
                csv.Configuration.MissingFieldFound = null;
                var records = csv.GetRecords<GAMCsvRecord>().ToList();
                if (records.Count < 1)
                {
                    return new List<BasicDeviceInfo>
                    {
                        new BasicDeviceInfo
                        {
                            Error = true,
                            ErrorText = "No results for that entry."
                        }
                    };
                }
                List<BasicDeviceInfo> output = new List<BasicDeviceInfo>();
                foreach (GAMCsvRecord record in records)
                {
                    output.Add(record.ToBasicDeviceInfo());
                }

                return output;
            }
        }
       

        public static BasicDeviceInfo GetAllDeviceInfo(string deviceId)
        {
            List<string> gamResult = RunGAM("info cros " + deviceId +
                                            " fields deviceId,lastSync,serialNumber,status,notes,assetId,location,user,orgUnitPath");
            BasicDeviceInfo output = new BasicDeviceInfo
            {
                DeviceId = deviceId
            };
            Dictionary<string, string> dict = new Dictionary<string, string>();
            for (int i = 1; i < gamResult.Count; i++) //starting i at 1 to ignore the first line of GAM output
            {
                gamResult[i] =
                    gamResult[i].TrimStart(new char[] {' '}); // remove white space from the start of the string
                int splitPoint = gamResult[i].IndexOf(": ");
                dict.Add(gamResult[i].Substring(0, splitPoint),
                    gamResult[i].Substring(splitPoint + 2, (gamResult[i].Length - splitPoint) - 2));
            }

            // now, I need to put a switch in a for loop to put this data into a basicdeviceinfo object, but should test it first..
            foreach (KeyValuePair<string, string> kvp in dict)
            {
                switch (kvp.Key)
                {
                    case "orgUnitPath":
                        output.OrgUnitPath = kvp.Value;
                        break;
                    case "annotatedAssetId":
                        output.AssetId = kvp.Value;
                        break;
                    case "annotatedLocation":
                        output.Location = kvp.Value;
                        break;
                    case "annotatedUser":
                        output.User = kvp.Value;
                        break;
                    case "lastSync":
                        output.LastSync = kvp.Value;
                        break;
                    case "notes":
                        output.Notes = kvp.Value;
                        break;
                    case "serialNumber":
                        output.SerialNumber = kvp.Value;
                        break;
                    case "status":
                        output.Status = kvp.Value;
                        break;
                    default:
                        Console.WriteLine(kvp.Key + "," + kvp.Value);
                        break;
                }
            }

            return output;
        }

        public static string GetUserEmail()
        {
            List<string> userInfo = RunGAM("info user");
            string emailLine = null;
            foreach (string line in userInfo)
            {
                if (line.StartsWith("User: "))
                {
                    emailLine = line;
                    break;
                }
            }

            if (string.IsNullOrEmpty(emailLine)) return "Error getting email.";
            return emailLine.Split(' ')[1];

        }
    }

    class GAMCsvRecord
    {
        [Name("deviceId")]
        public string DeviceId { get; set; }

        [Name("serialNumber")]
        public string SerialNumber { get; set; }

        [Name("status")]
        public string Status { get; set; }

        [Name("lastSync")]
        public string LastSync { get; set; }

        [Name("annotatedUser")]
        public string User { get; set; }

        [Name("annotatedLocation")]
        public string Location { get; set; }

        [Name("annotatedAssetId")]
        public string AssetId { get; set; }

        [Name("notes")]
        public string Notes { get; set; }

        [Name("orgUnitPath")]
        public string OrgUnitPath { get; set; }

        public BasicDeviceInfo ToBasicDeviceInfo()
        {
            return new BasicDeviceInfo
            {
                DeviceId = !String.IsNullOrEmpty(DeviceId) ? DeviceId : null,
                LastSync = !String.IsNullOrEmpty(LastSync) ? LastSync : null,
                SerialNumber = !String.IsNullOrEmpty(SerialNumber) ? SerialNumber : null,
                Status = !String.IsNullOrEmpty(Status) ? Status : null,
                Notes = !String.IsNullOrEmpty(Notes) ? Notes : null,
                AssetId = !String.IsNullOrEmpty(AssetId) ? AssetId : null,
                Location = !String.IsNullOrEmpty(Location) ? Location : null,
                User = !String.IsNullOrEmpty(User) ? User : null,
                OrgUnitPath = !String.IsNullOrEmpty(OrgUnitPath) ? OrgUnitPath : null,
                Error = false
            };
        }

    }
}
