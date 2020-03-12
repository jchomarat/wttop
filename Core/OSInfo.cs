using System;
using System.Management;

namespace wttop.Core {

    /// <summary>
    /// The operating system information
    /// </summary>
    public class OSInfo
    {
        /// <summary>
        /// Get or set the machine name
        /// </summary>
        public string MachineName {get; set; }

        /// <summary>
        /// Get or set the operating system name
        /// </summary>
        public string OSName {get; set; }

        /// <summary>
        /// Get or set the version
        /// </summary>
        public string Version {get; set; }
    }
}