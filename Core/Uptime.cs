using System;
using System.Management;

namespace wttop.Core
{

    /// <summary>
    /// Uptime class
    /// </summary>
    public class Uptime
    {
        /// <summary>
        /// Get or set the uptime
        /// </summary>
        public TimeSpan UpTime { get; set; }

        /// <summary>
        /// Get a string from uptime
        /// </summary>
        public string UpTimeForHuman => $"{UpTime.Days}d {UpTime.Hours}h {UpTime.Minutes}m";

        /// <summary>
        /// Parse the uptime string
        /// </summary>
        /// <param name="input">A string representing the uptime</param>
        /// <returns>A timestamp with the uptime</returns>
        public static TimeSpan ParseUpTime(string input)
        {
            DateTime upTime = ManagementDateTimeConverter.ToDateTime(input);
            return (DateTime.Now - upTime);
        }
    }
}