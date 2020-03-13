using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
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
        private CpuUtilizationReader _cpuUtilizationReader;
        private ProcessesReader _processReader;

        /// <summary>
        /// Get the cpu count
        /// </summary>
        public int CpuCount { get; } = 0;

        /// <summary>
        /// Get the disk count
        /// </summary>
        public int DiskCount { get; } = 0;

        /// <summary>
        /// Constructor for the Windows driver
        /// </summary>
        public WindowsDriver()
        {
            CpuCount = Environment.ProcessorCount;
            DiskCount = DriveInfo.GetDrives().Length;

            _cpuUtilizationReader = new CpuUtilizationReader();
            // Do an initial reading
            int c = _cpuUtilizationReader.CurrentUtilization;

            _processReader = new ProcessesReader();
        }

        /// <summary>
        /// Provide the operating system information
        /// </summary>
        /// <returns>Operating system information </returns>
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

        /// <summary>
        /// Provide the list of CPU
        /// </summary>
        /// <returns>A list of CPU</returns>
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

        /// <summary>
        /// Get the total CPU usage
        /// </summary>
        /// <returns>Cpu information</returns>
        public Task<Cpu> GetTotalCpuUsage()
        {
            var tsc = new TaskCompletionSource<Cpu>();
            var cpu = new Cpu()
            {
                Name = "Total",
                PercentageUsage = _cpuUtilizationReader.CurrentUtilization
            };

            tsc.SetResult(cpu);
            return tsc.Task;

        }

        /// <summary>
        /// Get the memory usage
        /// </summary>
        /// <returns>Memory information</returns>
        public Task<Memory> GetMemoryUsage()
        {
            var tsc = new TaskCompletionSource<Memory>();
            var memory = new Memory();

            Kernel32.MEMORYSTATUSEX memoryStatusEx = new Kernel32.MEMORYSTATUSEX();
            if (!Kernel32.GlobalMemoryStatusEx(memoryStatusEx))
            {
                int error = Marshal.GetLastWin32Error();
                var exception = new Win32Exception("Could not retrieve the global memory status");
                throw exception;
            }
            else
            {
                memory.PhysicalPercentageUsed = (int)memoryStatusEx.dwMemoryLoad;
                memory.PhysicalTotalGb = Convert.ToInt32(memoryStatusEx.ullTotalPhys / 1024 / 1024 / 1024);
                memory.PhysicalAvailableGb = Convert.ToInt32(memoryStatusEx.ullAvailPhys / 1024 / 1024 / 1024);

                memory.SwapAvailableGb = Convert.ToInt32(memoryStatusEx.ullAvailVirtual / 1024 / 1024 / 1024);
                memory.SwapTotalGb = Convert.ToInt32(memoryStatusEx.ullTotalVirtual / 1024 / 1024 / 1024);
                memory.SwapPercentageUsed = (int)Math.Round((decimal)((memory.SwapTotalGb - memory.SwapAvailableGb) * 100) / memory.SwapTotalGb, 0);
            }

            tsc.SetResult(memory);
            return tsc.Task;
        }

        ///// <summary>
        /// Get the network statistics
        /// </summary>
        /// <returns>Network information</returns>
        public async Task<Network> GetNetworkStatistics()
        {
            var queryString = "SELECT BytesReceivedPersec, BytesSentPersec, Name FROM Win32_PerfRawData_Tcpip_NetworkInterface";
            var wmiReader = new WmiReader();
            var results = await wmiReader.Execute(queryString);

            return new Network()
            {
                Interfaces = results.Select(mo => new InterfaceDetails()
                {
                    Name = mo["Name"].ToString(),
                    BytesReceived = Convert.ToInt64(mo["BytesReceivedPersec"]),
                    BytesSent = Convert.ToInt64(mo["BytesSentPersec"])
                })
                            .ToList()
            };
        }

        /// <summary>
        /// Get the process activity
        /// </summary>
        /// <returns>Process information</returns>
        public Task<Process> GetProcessActivity()
        {
            var tsc = new TaskCompletionSource<Process>();
            var pws = _processReader.Refresh();

            var p = new Process()
            {
                Processes = pws.Select(pw => new ProcessInfo()
                {
                    Name = pw.Name,
                    PercentProcessorTime = pw.CpuUsage,
                    IdProcess = pw.Pid,
                    Owner = pw.UserName,
                    ThreadCount = pw.ThreadCount,
                    MemoryUsageMb = pw.MemoryUsageMB
                })
                            .ToList()
            };

            tsc.SetResult(p);
            return tsc.Task;
        }

        /// <summary>
        /// Get the disk activity
        /// </summary>
        /// <returns>Disk information</returns>
        public async Task<Disk> GetDiskActivity()
        {
            var queryString = "SELECT DiskReadBytesPersec, DiskWriteBytesPersec, Name FROM Win32_PerfRawData_PerfDisk_PhysicalDisk WHERE NOT name = '_Total'";
            var wmiReader = new WmiReader();
            var results = await wmiReader.Execute(queryString);

            return new Disk()
            {
                Disks = results.Select(mo => new DiskDetails()
                {
                    Name = mo["Name"].ToString(),
                    BytesRead = Convert.ToInt64(mo["DiskReadBytesPersec"]),
                    BytesWriten = Convert.ToInt64(mo["DiskWriteBytesPersec"])
                })
                        .ToList()
            };
        }

        /// <summary>
        /// Get the disk storage information
        /// </summary>
        /// <returns>A list of storage information</returns>
        public async Task<IEnumerable<Storage>> GetDiskStorageInfo()
        {
            var queryString = "SELECT VolumeName, Caption, FreeSpace, Size FROM Win32_logicaldisk";
            var wmiReader = new WmiReader();
            var results = await wmiReader.Execute(queryString);

            return results.Select(mo => new Storage()
            {
                VolumeCaption = mo["Caption"].ToString(),
                VolumeName = mo["VolumeName"].ToString(),
                AvailableKb = Convert.ToInt64(mo["FreeSpace"]),
                TotalKb = Convert.ToInt64(mo["Size"])
            });
        }

        /// <summary>
        /// Get the system uptime
        /// </summary>
        /// <returns>System uptime</returns>
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

        /// <summary>
        /// Get the system date time
        /// </summary>
        /// <returns>System time</returns>
        public Task<SystemTime> GetSystemDateTime()
        {
            var tsc = new TaskCompletionSource<SystemTime>();
            tsc.SetResult(new SystemTime(DateTime.Now));
            return tsc.Task;
        }
    }
}