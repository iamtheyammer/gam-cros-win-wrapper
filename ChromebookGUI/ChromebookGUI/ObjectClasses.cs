using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChromebookGUI
{
    public class OrgUnit
    {
        public string OrgUnitPath { get; set; }
        public string OrgUnitName { get; set; }
        public string OrgUnitDescription { get; set; }
    }

    public class BasicDeviceInfo
    {
        public string DeviceId { get; set; }
        public string LastSync { get; set; }
        public string SerialNumber { get; set; }
        public string Status { get; set; }
        public string Notes { get; set; }
    }
}
