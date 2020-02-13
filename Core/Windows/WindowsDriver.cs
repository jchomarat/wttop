using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Threading.Tasks;

namespace wttop.Core
{
    /// <summary>
    /// Windows driver for wttop.
    /// This is a wrapper of WMI calls. Why WMi, well because in dotnetcore, we cannot get all this system values.
    /// WMI is probably the oldest way to do it, but a straigthforward one.
    /// </summary>
    public class WindowsDriver : ISystemInfo
    {
        public async Task<OSInfo> GetOSInfo()
        {
            var queryString = "SELECT caption, version, CSName FROM Win32_OperatingSystem";
            var wmiReader = new WmiReader();
            var results = await wmiReader.ExecuteScalar(queryString);

            return new OSInfo()
            {
                MachineName = results["CSName"].ToString(),
                OSName = results["caption"].ToString(),
                Version = results["version"].ToString()
            };
        }

        public int GetCPUsCount()
        {
            return Environment.ProcessorCount;
        }

        public async Task<IEnumerable<Cpu>> GetCPUsUsage()
        {
            var queryString = "SELECT name, PercentProcessorTime FROM Win32_PerfFormattedData_PerfOS_Processor WHERE NOT name = '_Total'";
            var wmiReader = new WmiReader();
            var results = await wmiReader.Execute(queryString);

            return results.Select(mo => new wttop.Core.Cpu()
                {
                    Name = mo["name"].ToString(),
                    PercentageUsage = int.Parse(mo["PercentProcessorTime"].ToString())
                });
        }

        public async Task<Cpu> GetTotalCpuUsage()
        {
            var queryString = "SELECT PercentProcessorTime FROM Win32_PerfFormattedData_PerfOS_Processor WHERE name = '_Total'";
            var wmiReader = new WmiReader();
            var results = await wmiReader.ExecuteScalar(queryString);

            return new Cpu()
            {
                Name = "Total",
                PercentageUsage = int.Parse(results["PercentProcessorTime"].ToString())
            };
        }

        public async Task<Memory> GetMemoryUsage()
        {
            var queryString = "SELECT FreePhysicalMemory, TotalVisibleMemorySize FROM Win32_OperatingSystem";
            var wmiReader = new WmiReader();
            var results = await wmiReader.ExecuteScalar(queryString);

            return new Memory()
            {
                AvailableKb = Convert.ToInt32(results["FreePhysicalMemory"]),
                TotalKb = Convert.ToInt32(results["TotalVisibleMemorySize"])
            };
        }

        public async Task<Network> GetNetworkStatistics()
        {
            var queryString = "SELECT BytesReceivedPersec, BytesSentPersec, Name FROM Win32_PerfRawData_Tcpip_NetworkInterface";
            var wmiReader = new WmiReader();
            var results = await wmiReader.Execute(queryString);

            return new Network()
            {
                Interfaces = results.Select(mo => new InterfaceDetails(){
                                Name = mo["Name"].ToString(),
                                BytesReceived = Convert.ToInt64(mo["BytesReceivedPersec"]),
                                BytesSent = Convert.ToInt64(mo["BytesSentPersec"])
                            })
                            .ToList()
            };
        }

        public async Task<Process> GetProcessActivity()
        {
            var queryString = "SELECT Name, PercentProcessorTime, IDProcess, ThreadCount, HandleCOunt, PriorityBase, WorkingSetPrivate FROM Win32_PerfFormattedData_PerfProc_Process WHERE IDProcess > 0";
            var wmiReader = new WmiReader();
            var results = await wmiReader.Execute(queryString);

            return new Process()
            {
                Processes = results.Select(mo => new ProcessInfo(){
                                Name = mo["Name"].ToString(),
                                PercentProcessorTime = Convert.ToInt32(mo["PercentProcessorTime"]),
                                IDProcess = Convert.ToInt32(mo["IDProcess"]),
                                ThreadCount = Convert.ToInt32(mo["ThreadCount"]),
                                HandleCount = Convert.ToInt32(mo["HandleCOunt"]),
                                PriorityBase = Convert.ToInt32(mo["PriorityBase"]),
                                MemoryUsageB = Convert.ToInt64(mo["WorkingSetPrivate"])
                            })
                            .ToList()
            };
        }

        public async Task<Disk> GetDiskActivity()
        {
            var queryString = "SELECT DiskReadBytesPersec, DiskWriteBytesPersec, Name FROM Win32_PerfRawData_PerfDisk_PhysicalDisk WHERE NOT name = '_Total'";
            var wmiReader = new WmiReader();
            var results = await wmiReader.Execute(queryString);

            return new Disk()
            {
                Disks = results.Select(mo => new DiskDetails(){
                            Name = mo["Name"].ToString(),
                            BytesRead = Convert.ToInt64(mo["DiskReadBytesPersec"]),
                            BytesWrite = Convert.ToInt64(mo["DiskWriteBytesPersec"])
                        })
                        .ToList()
            };
        }

        public async Task<Uptime> GetSystemUpTime()
        {
            var queryString = "SELECT LastBootUpTime FROM Win32_OperatingSystem";
            var wmiReader = new WmiReader();
            var results = await wmiReader.ExecuteScalar(queryString);

            return new Uptime()
            {
                UpTime = Uptime.ParseUpTime(results["LastBootUpTime"].ToString())
            };
        }

        public Task<SystemTime> GetSystemDateTime()
        {
            var tsc = new TaskCompletionSource<SystemTime>();
            tsc.SetResult(new SystemTime(DateTime.Now));
            return tsc.Task;
        }
    }
}