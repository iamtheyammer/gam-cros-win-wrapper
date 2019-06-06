using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;

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
        public static string OrgUnitPath { get; set; }
        /// <summary>
        /// If using a CSV, the location of such CSV.
        /// </summary>
        public static string CsvLocation { get; set; }

        /// <summary>
        /// The email for the current GAM user.
        /// Get with Globals.GetAndSaveUserEmail();
        /// </summary>
        private static string AdminEmail { get; set; }

        public static readonly HttpClient HttpClientObject = new HttpClient();


        /// <summary>
        /// Returns a boolean indicating whether the DeviceId global is null or has a value.
        /// </summary>
        /// <returns></returns>
        public static bool DeviceIdExists()
        {
            return !string.IsNullOrEmpty(DeviceId);
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
            AssetId = null;
        }
        
        /// <summary>
        /// Sets globals from a BasicDeviceInfo object. Only sets the DeviceId, Note, Status, Serial Number, Asset ID and Location. Only the DeviceId is required.
        /// </summary>
        /// <param name="info"></param>
        public static void SetGlobalsFromBasicDeviceInfo(BasicDeviceInfo info)
        {
            DeviceId = (!String.IsNullOrEmpty(info.DeviceId)) ? info.DeviceId : null;
            Note = (!String.IsNullOrEmpty(info.Notes)) ? info.Notes : null;
            Status = (!String.IsNullOrEmpty(info.Status)) ? info.Status : null;
            SerialNumber = (!String.IsNullOrEmpty(info.SerialNumber)) ? info.SerialNumber : null;
            AssetId = (!String.IsNullOrEmpty(info.AssetId)) ? info.AssetId : null;
            User = (!String.IsNullOrEmpty(info.User)) ? info.User : null;
            OrgUnitPath = (!String.IsNullOrEmpty(info.OrgUnitPath)) ? info.OrgUnitPath : null;
            Location = (!String.IsNullOrEmpty(info.Location)) ? info.Location : null;
        }

        /// <summary>
        /// Returns a string of JSON.stringify(Globals), essentially.
        /// If a value is null, the key will still exist.
        /// </summary>
        /// <returns></returns>
        public static string ToJsonString()
        {
            Dictionary<string, string> toSerialize = new Dictionary<string, string>()
            {
                ["deviceId"] = DeviceId,
                ["note"] = Note,
                ["status"] = Status,
                ["assetId"] = AssetId,
                ["serialNumber"] = SerialNumber,
                ["orgUnitPath"] = OrgUnitPath,
                ["user"] = User,
                ["location"] = Location,
            };
            return JsonConvert.SerializeObject(toSerialize);
        }

        /// <summary>
        /// Gets the user's email if we don't have it already.
        /// </summary>
        /// <returns></returns>
        public static string GetAndSaveUserEmail()
        {
            if(String.IsNullOrEmpty(AdminEmail))
            {
                AdminEmail = GAM.GetUserEmail();
            }
            return AdminEmail;
        }
    }
}
