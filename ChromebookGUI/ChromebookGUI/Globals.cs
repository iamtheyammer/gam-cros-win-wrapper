using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChromebookGUI
{   
    /// <summary>
    /// Allows us to hold global information in memory, specifically the DeviceId, Note, Status, User, AssetId, Location and SerialNumber.
    /// Contains methods for clearing, checking if a DeviceId is present, and importing from a BasicDeviceInfo object.
    /// </summary>
    public static class Globals
    {

        public static string DeviceId { get; set; }
        public static string Note { get; set; }
        public static string Status { get; set; }
        public static string User { get; set; }
        public static string AssetId { get; set; }
        public static string Location { get; set; }
        public static string SerialNumber { get; set; }


        /// <summary>
        /// Returns a boolean indicating whether the DeviceId global is null or has a value.
        /// </summary>
        /// <returns></returns>
        public static bool DeviceIdExists()
        {
            if(DeviceId == null)
            {
                return false;
            } else
            {
                return true;
            }
        }

        /// <summary>
        /// Sets all globals to null.
        /// </summary>
        public static void ClearGlobals()
        {
            DeviceId = null;
            Note = null;
            Status = null;
            User = null;
            AssetId = null;
            Location = null;
            SerialNumber = null;
        }
        
        /// <summary>
        /// Sets globals from a BasicDeviceInfo object. Only sets the DeviceId, Note, Status and Serial Number. Only the DeviceId is required.
        /// </summary>
        /// <param name="info"></param>
        public static void SetGlobalsFromBasicDeviceInfo(BasicDeviceInfo info)
        {
            DeviceId = info.DeviceId;
            Note = (!String.IsNullOrEmpty(info.Notes)) ? info.Notes : null;
            Status = (!String.IsNullOrEmpty(info.Status)) ? info.Status : null;
            SerialNumber = (!String.IsNullOrEmpty(info.SerialNumber)) ? info.SerialNumber : null;
        }
    }
}
