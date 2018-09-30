using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public static String RunGAMFormatted(String gamCommand)
        {
            string result = null;
            foreach(string line in RunGAM(gamCommand))
            {
                result += "\n" + line;
            }
            return result;
        }

        public static List<List<string>> RunGAMCommasFixed(String gamCommand)
        {
            return FixCSVCommas.FixCommas(GAM.RunGAM(gamCommand));
        }

        public static String GetDeviceId(String input)
        {
            if (input.Length == 36) // this is already a device ID
            {
                return input;
            }
            else if (input.Contains("@"))
            {
                string user = input.Split('@')[0];
                if (String.IsNullOrEmpty(user))
                {
                    return "There was an error processing your email entry: I couldn't find the email.";
                }
                // results:
                // deviceId,lastSync,notes,serialNumber,status
                List<string> gamResults = RunGAM("print cros query \"user:" + user + "\" fields deviceId,lastSync,notes,serialNumber,status");
                List<List<string>> deviceInfos = FixCSVCommas.FixCommas(gamResults);
                if (deviceInfos[0][0] == "deviceId") deviceInfos.RemoveAt(0);
                if(deviceInfos.Count == 1) if (deviceInfos[0].Count > 1)
                    {
                        return deviceInfos[0][0];
                    }
                List<BasicDeviceInfo> infoObjects = new List<BasicDeviceInfo>();
                foreach(List<string> line in deviceInfos)
                {
                    infoObjects.Add(new BasicDeviceInfo()
                    {
                        DeviceId = line[0],
                        LastSync = line[1],
                        SerialNumber = line[3],
                        Status = line[4],
                        Notes = line[2]
                    });
                }
                List<string> selection = GetInput.GetDataGridSelection("Which device would you like to select?", "Click on a row or enter a Device ID or Serial Number.", infoObjects);
                string deviceId = null;
                foreach (string item in selection)
                {
                    if (item.Length == 36) deviceId = item;
                    break;
                }
                if(deviceId == null)
                {
                    return "There was an error. You didn't pick anything.";
                } else
                {
                    return deviceId;
                }
            }
            else
            {
                // must be a serial number. 
                List<string> response = RunGAM("print cros query \"id:" + input + "\"");
                if (response.Count < 2) return "No results found for that entry (" + input + ")."; // this count thing does NOT start at zero. ugh!
                return response[1];
            }
            return "hi";
        }
    }
}
