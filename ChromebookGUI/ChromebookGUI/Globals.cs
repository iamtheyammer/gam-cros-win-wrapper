using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChromebookGUI
{
    public static class Globals
    {

        public static string DeviceId { get; set; }

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
    }
}
