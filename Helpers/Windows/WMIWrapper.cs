using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;

namespace wttop.Helpers {

    public class WMIWrapper
    {  
        ManagementObjectCollection runQuery(string queryString)
        {
            var query = new System.Management.ObjectQuery(queryString);
            var searcher = new ManagementObjectSearcher(query);
            return searcher.Get();
        }

        public OSInfo GetOperatingSystemInformation()
        {
            var queryString = "SELECT caption, CSName, LastBootUpTime FROM Win32_OperatingSystem";
            var results = runQuery(queryString);

            var item = results.Cast<ManagementObject>().FirstOrDefault();
            return new OSInfo()
            {
                MachineName = item["CSName"].ToString(),
                OSName = item["caption"].ToString(),
                UpTime = OSInfo.ParseUpTime(item["LastBootUpTime"].ToString())
            };
        }

        public IEnumerable<Tuple<string, string>> GetCPUsUsage()
        {
            var queryString = "SELECT name, PercentProcessorTime FROM Win32_PerfFormattedData_PerfOS_Processor WHERE NOT name = '_Total'";
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

        public IEnumerable<InterfaceDetails> GetNetworkInterfacesDetails()
        {
            var queryString = "SELECT BytesReceivedPersec, BytesSentPersec, Name FROM Win32_PerfRawData_Tcpip_NetworkInterface";
            var results = runQuery(queryString);

            return results.Cast<ManagementObject>()
                .Select(mo => new InterfaceDetails(){
                    Name = mo["Name"].ToString(),
                    BytesReceived = Convert.ToInt64(mo["BytesReceivedPersec"]),
                    BytesSent = Convert.ToInt64(mo["BytesSentPersec"])
                });
        }

        public IEnumerable<ProcessInfo> GetProcessesActivity()
        {
            var queryString = "SELECT Name, PercentProcessorTime, IDProcess, ThreadCount, HandleCOunt, PriorityBase FROM Win32_PerfFormattedData_PerfProc_Process WHERE IDProcess > 0";
            var results = runQuery(queryString);

            return results.Cast<ManagementObject>()
                .Select(mo => new ProcessInfo(){
                    Name = mo["Name"].ToString(),
                    PercentProcessorTime = Convert.ToInt32(mo["PercentProcessorTime"]),
                    IDProcess = Convert.ToInt32(mo["IDProcess"]),
                    ThreadCount = Convert.ToInt32(mo["ThreadCount"]),
                    HandleCount = Convert.ToInt32(mo["HandleCOunt"]),
                    PriorityBase = Convert.ToInt32(mo["PriorityBase"])
                });
        }

        public IEnumerable<DiskDetails> GetAllDisksActivity()
        {
            var queryString = "SELECT DiskReadBytesPersec, DiskWriteBytesPersec, Name FROM Win32_PerfRawData_PerfDisk_PhysicalDisk WHERE NOT name = '_Total'";
            var results = runQuery(queryString);

            return results.Cast<ManagementObject>()
                .Select(mo => new DiskDetails(){
                    Name = mo["Name"].ToString(),
                    BytesRead = Convert.ToInt64(mo["DiskReadBytesPersec"]),
                    BytesWrite = Convert.ToInt64(mo["DiskWriteBytesPersec"])
                });
        }
    }
}