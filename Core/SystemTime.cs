using System;
using System.Management;

namespace wttop.Core {

    public class SystemTime
    {
        DateTime now = DateTime.MinValue;

        public SystemTime(DateTime Now)
        {
            this.now = Now;
        }

        public override string ToString()
        {
            return now.ToString("dddd, dd MMMM yyyy HH:mm:ss");
        }
    }
}