using System;
using System.Management;

namespace wttop.Core {

    public class Uptime
    {
        public TimeSpan UpTime {get; set; }

        public string UpTimeForHuman
        {
            get
            {
                return $"{UpTime.Days}d {UpTime.Hours}h {UpTime.Minutes}m";
            }
        }

        public static TimeSpan ParseUpTime(string input)
        {
            DateTime upTime = ManagementDateTimeConverter.ToDateTime(input);
            return (DateTime.Now - upTime);
        }
    }
}