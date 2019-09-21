using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChromebookGUI
{
    class Software
    {
        /// <summary>
        /// This is the current version of the software.
        /// By default, users of a certain Software.Type will only get updates for the Software.Type they are currently using.
        /// As an example users of "beta" will get alerted when a new "beta" release is available.
        /// Release: ex. 1.0
        /// Beta: ex. 0.1
        /// Alpha: ex. 0.1.2
        /// </summary>
        public static string Version => "1.1.4";

        /// <summary>
        /// This is the software type. It can be one of { "release", "beta", "alpha" }.
        /// Release: ex. 1.0
        /// Beta: ex. 0.1
        /// Alpha: ex. 0.1.2
        /// </summary>
        public static string Type => "beta";
    }
}
