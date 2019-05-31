using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Windows;
using System.Windows.Media;
using WpfTextBox = System.Windows.Controls.TextBox;

namespace ChromebookGUI
{
    /// <summary>
    /// A class for holding information about an Organizational Unit, specifically OrgUnitPath, OrgUnitName and OrgUnitDescription.
    /// </summary>
    public class OrgUnit
    {
        public string OrgUnitPath { get; set; }
        public string OrgUnitName { get; set; }
        public string OrgUnitDescription { get; set; }

        /// <summary>
        /// DEPRECATED. Please use AwaitableGetOrgUnitFromSelector, then HandleAwaitableOrgUnitFromSelector.
        /// Easily get an org path. Handles all of the logic of opening the data selector window and finding which is the path.
        /// </summary>
        /// <returns>A string containing the full path to the organizational unit.</returns>
        public static string GetOrgUnitFromSelector()
        {
            List<List<string>> fixedOrgs = GAM.RunGAMCommasFixed("print orgs allfields");

            List<OrgUnit> orgUnits = new List<OrgUnit>();
            foreach (List<string> org in fixedOrgs)
            {
                if (org[0] == "orgUnitPath") continue;

                orgUnits.Add(new OrgUnit()
                {
                    OrgUnitPath = !String.IsNullOrEmpty(org[0]) ? org[0] : null,
                    OrgUnitName = !String.IsNullOrEmpty(org[2]) ? (org[2].StartsWith("id:") ? "(no description provided)" : org[2]) : null,
                    OrgUnitDescription = !String.IsNullOrEmpty(org[3]) ? (org[3].StartsWith("id:") ? "(no description provided)" : org[3]) : null
                });
            }
            if (orgUnits.Count < 2)
            {
                return "There was an error getting your org units. You don't seem to have any.";
            }
            List<string> orgSelection = GetInput.GetDataGridSelection("Pick an org!", "Click on an row to select it, or paste the full path here and press submit...", "Organizational Unit Selector", orgUnits);
            string orgPath = null;
            foreach (string item in orgSelection)
            {
                if (item.Contains("/")) orgPath = item;
            }
            if (orgPath == null | orgSelection.Contains("Click on an row to select it, or paste the full path here and press submit..."))
            {
                return "Either you didn't enter anything or there was an error. Nothing has been changed.";
            }

            return orgPath;
        }

        /// <summary>
        /// An awaitable version of OrgUnit.GetOrgUnitFromSelector.
        /// Make sure to handle output with OrgUnit.HandleAwaitableGetOrgUnitFromSelector.
        /// </summary>
        /// <returns></returns>
        public static List<OrgUnit> AwaitableGetOrgUnitFromSelector()
        {
            List<List<string>> fixedOrgs = GAM.RunGAMCommasFixed("print orgs allfields");

            List<OrgUnit> orgUnits = new List<OrgUnit>();
            foreach (List<string> org in fixedOrgs)
            {
                if (org[0] == "orgUnitPath") continue;

                orgUnits.Add(new OrgUnit()
                {
                    OrgUnitPath = !String.IsNullOrEmpty(org[0]) ? org[0] : null,
                    OrgUnitName = !String.IsNullOrEmpty(org[2]) ? (org[2].StartsWith("id:") ? "(no description provided)" : org[2]) : null,
                    OrgUnitDescription = !String.IsNullOrEmpty(org[3]) ? (org[3].StartsWith("id:") ? "(no description provided)" : org[3]) : null
                });
            }
            if (orgUnits.Count < 2)
            {
                return new List<OrgUnit>();
            } else
            {
                return orgUnits;
            }
        }

        /// <summary>
        /// Handles OrgUnit.AwaitableGetOrgUnitFromSelector.
        /// Returns an empty string if the user didn't pick anything.
        /// </summary>
        /// <param name="orgUnits">Data returned from OrgUnit.GetOrgUnitFromSelector</param>
        /// <returns></returns>
        public static string HandleAwaitableGetOrgUnitFromSelector(List<OrgUnit> orgUnits)
        {
            List<string> orgSelection = GetInput.GetDataGridSelection("Pick an org!", "Click on an row to select it, or paste the full path here and press submit...", "Organizational Unit Selector", orgUnits);
            string orgPath = null;
            foreach (string item in orgSelection)
            {
                if (item.Contains("/"))
                {
                    orgPath = item;
                    break;
                }
            }
            if (orgPath == null || orgSelection.Contains("Click on an row to select it, or paste the full path here and press submit..."))
            {
                return "";
            }

            return orgPath;
        }
    }

    /// <summary>
    /// A class for holding information (specifically DeviceId, LastSync, SerialNumber, Status and Notes, plus Error and ErrorText) about a Chrome device.
    /// </summary>
    public class BasicDeviceInfo
    {
        public string DeviceId { get; set; }
        public string LastSync { get; set; }
        public string SerialNumber { get; set; }
        public string Status { get; set; }
        public string Notes { get; set; }
        public string AssetId { get; set; }
        public string Location { get; set; }
        public string User { get; set; }
        public string OrgUnitPath { get; set; }
        public bool Error { get; set; }
        public string ErrorText { get; set; }

        /// <summary>
        /// Converts a BasicDeviceInfo object to a dictionary with the same key names. 
        /// </summary>
        /// <param name="input">The BasicDeviceInfo object you'd like back as a dictionary.</param>
        /// <returns>
        /// A Dictionary(string, string) with the same data
        /// </returns>
        public static Dictionary<string, string> ToDictionary(BasicDeviceInfo input)
        {
            Dictionary<string, string> output = new Dictionary<string, string>();

            if (!String.IsNullOrEmpty(input.LastSync)) output.Add("LastSync", input.LastSync);
            if (!String.IsNullOrEmpty(input.SerialNumber)) output.Add("SerialNumber", input.SerialNumber);
            if (!String.IsNullOrEmpty(input.Status)) output.Add("Status", input.Status);
            if (!String.IsNullOrEmpty(input.Notes)) output.Add("Notes", input.Notes);
            if (!String.IsNullOrEmpty(input.AssetId)) output.Add("AssetId", input.AssetId);
            if (!String.IsNullOrEmpty(input.Location)) output.Add("Location", input.Location);
            if (!String.IsNullOrEmpty(input.User)) output.Add("User", input.User);
            if (!String.IsNullOrEmpty(input.OrgUnitPath)) output.Add("OrgUnitPath", input.OrgUnitPath);
            if (!String.IsNullOrEmpty(input.Error ? "true" : "false")) output.Add("Error", input.Error ? "true" : "false");
            if (!String.IsNullOrEmpty(input.ErrorText)) output.Add("ErrorText", input.ErrorText);

            return output;
        }

        /// <summary>
        /// Opposite of BasicDeviceInfo.ToDictionary.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static BasicDeviceInfo DictionaryToBasicDeviceInfo(Dictionary<string, string> input)
        {
            BasicDeviceInfo output = new BasicDeviceInfo
            {
                LastSync = !String.IsNullOrEmpty(input["LastSync"]) ? input["LastSync"] : null,
                SerialNumber = !String.IsNullOrEmpty(input["SerialNumber"]) ? input["SerialNumber"] : null,
                Status = !String.IsNullOrEmpty(input["Status"]) ? input["Status"] : null,
                Notes = !String.IsNullOrEmpty(input["Notes"]) ? input["Notes"] : null,
                AssetId = !String.IsNullOrEmpty(input["AssetId"]) ? input["AssetId"] : null,
                Location = !String.IsNullOrEmpty(input["Location"]) ? input["Location"] : null,
                User = !String.IsNullOrEmpty(input["User"]) ? input["User"] : null,
                OrgUnitPath = !String.IsNullOrEmpty(input["OrgUnitPath"]) ? input["OrgUnitPath"] : null,
                Error = !String.IsNullOrEmpty(input["Error"]) ? (input["Error"] == "true" ? true : false) : false,
                ErrorText = !String.IsNullOrEmpty(input["ErrorText"]) ? input["ErrorText"] : null
            };

            return output;
        }

        public static BasicDeviceInfo HandleGetDeviceId(List<BasicDeviceInfo> possibleDevices)
        {
            if (possibleDevices.Count == 1)
            {
                return possibleDevices[0];
            }
            return GetInput.GetDeviceSelection("Which device would you like to select?", "Click on a row or enter a Device ID or Serial Number.", "Device Selector", possibleDevices);
        }


    }

    public class Button
    {
        public bool IsEnabled { get; set; }
        public string Text { get; set; }
    }
}
