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
        public bool Error { get; set; }
        public string ErrorText { get; set; }
    }

    public class Button
    {
        public bool IsEnabled { get; set; }
        public string Text { get; set; }
    }
}
