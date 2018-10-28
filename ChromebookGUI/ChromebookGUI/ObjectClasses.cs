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
        public bool Error { get; set; }
        public string ErrorText { get; set; }
    }

    public class Button
    {
        public bool IsEnabled { get; set; }
        public string Text { get; set; }
    }
}
