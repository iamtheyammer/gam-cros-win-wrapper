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
                output.Add(proc.StandardOutput.ReadLine()); // append the line to output
                //Console.WriteLine("Standard output:");
                //Console.WriteLine(proc.StandardOutput.ReadLine());
                Console.WriteLine("Error output:");
                Console.WriteLine(proc.StandardError.ReadLine());

            }
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

        public static String GetDeviceId(String input)
        {
            if (input.Length == 36) // this is already a device ID
            {
                return input;
            }
            else if (input.Contains("@"))
            {
                // deal with emails later.
            }
            else
            {
                // must be a serial number. 
                List<string> response = RunGAM("print cros query \"id:" + input + "\"");
                if (response.Count < 2) return "No results found for that entry (" + input + ".)"; // this count thing does NOT start at zero. ugh!
                return response[1];
            }
            return "hi";
        }
    }
}
