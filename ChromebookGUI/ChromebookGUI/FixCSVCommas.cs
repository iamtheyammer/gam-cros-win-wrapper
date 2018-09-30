using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// This class fixes any issues with CSVs you might have when getting info from GAM.
/// It will "fix" data that comes out in CSV from GAM-- by giving you a string[] result from it, where each IEnumerable
/// from the string will contain a comma.
/// </summary>
namespace ChromebookGUI
{
    class FixCSVCommas
    {
        public static List<List<string>> FixCommas(List<string> input)
        {
            // cycle through each match, and if it contains a comma, join it with the next.
            // this is my way of fixing commas inside of descriptions or paths.
            List<List<string>> output = new List<List<string>>();
            foreach(string inputLine in input)
            {
                List<string> removeCommas = inputLine.Split(',').ToList();
                for (int i = 0; i < removeCommas.Count; i++)
                {
                    string line = removeCommas[i];
                    if (!line.Contains("\"")) continue; // if it does not contain a quote, continue
                    removeCommas[i] = removeCommas[i].Replace("\"", String.Empty);
                    removeCommas[i + 1] = removeCommas[i + 1].Replace("\"", String.Empty);
                    removeCommas[i] = removeCommas[i] + "," + removeCommas[i + 1];
                    removeCommas.RemoveAt(i + 1);
                }
                output.Add(removeCommas);
            }
            return output;
        }
    }
}
