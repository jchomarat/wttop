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
            // foreach (var mo in results)
            // {
            //     mo.
            //     Console.WriteLine($"Proc: {mo["name"]}, usage: {mo["PercentProcessorTime"]}");
            // }
        }
    }
}