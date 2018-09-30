using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public bool Error { get; set; }
        public string ErrorText { get; set; }
    }
}
