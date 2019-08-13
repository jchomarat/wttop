using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;

namespace wttop.Helpers {

    public class WMIWrapper {
        
        ManagementObjectCollection runQuery(string queryString)
        {
            var query = new System.Management.ObjectQuery(queryString);
            var searcher = new ManagementObjectSearcher(query);
            return searcher.Get();
        }

        public IEnumerable<Tuple<string, string>> GetCPUsUsage()
        {
            var queryString = "SELECT name, PercentProcessorTime FROM Win32_PerfFormattedData_PerfOS_Processor where NOT name = '_Total'";
            var results = runQuery(queryString);

            return results.Cast<ManagementObject>()
                .Select(mo => new Tuple<string, string>(mo["name"].ToString(), mo["PercentProcessorTime"].ToString()));
        }

        public int GetNumberOfLogicalProcessors()
        {
            var queryString = "SELECT NumberOfLogicalProcessors FROM Win32_Processor";
            var results = runQuery(queryString);

            return Convert.ToInt32(results.Cast<ManagementObject>().FirstOrDefault()["NumberOfLogicalProcessors"]);
        }

        public Tuple<int, int> GetMemoryUsageKb()
        {
            var queryString = "SELECT FreePhysicalMemory, TotalVisibleMemorySize FROM Win32_OperatingSystem";
            var results = runQuery(queryString);

            var available = Convert.ToInt32(results.Cast<ManagementObject>().FirstOrDefault()["FreePhysicalMemory"]);
            var total = Convert.ToInt32(results.Cast<ManagementObject>().FirstOrDefault()["TotalVisibleMemorySize"]);
            
            return new Tuple<int, int>(total, available);
        }
    }
}