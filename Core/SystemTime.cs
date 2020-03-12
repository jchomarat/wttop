using System;
using System.Management;

namespace wttop.Core {

    /// <summary>
    /// The system time
    /// </summary>
    public class SystemTime
    {
        private DateTime _now = DateTime.MinValue;

        /// <summary>
        /// Constructor for the system time class
        /// </summary>
        /// <param name="Now">A datetime to create the system time</param>
        public SystemTime(DateTime Now)
        {
            _now = Now;
        }

        /// <summary>
        /// Provide the time as a string in the needed format
        /// </summary>
        /// <returns>The system time as "dddd, dd MMMM yyyy HH:mm:ss"</returns>
        public override string ToString()
        {
            return _now.ToString("dddd, dd MMMM yyyy HH:mm:ss");
        }
    }
}