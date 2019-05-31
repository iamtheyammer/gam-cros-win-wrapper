using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChromebookGUI.Classes
{
    class Debug
    {
        public static bool IsDebugMode()
        {
#if DEBUG
            return true;
#else
            return false;
#endif
        }
    }
}
