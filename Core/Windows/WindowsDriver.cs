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
        CpuUtilizationReader cpuUtilizationReader;

        ProcessesReader processReader;

        int cpuCount = 0;

        int diskCount = 0;

        public int CpuCount
        {
            get
            {
                return cpuCount;
            }
        }

        public int DiskCount
        {
            get
            {
                return diskCount;
            }
        }

        public WindowsDriver()
        {
            cpuCount = Environment.ProcessorCount;
            diskCount = DriveInfo.GetDrives().Length;

            cpuUtilizationReader = new CpuUtilizationReader();
            // Do an initial reading
            int c = cpuUtilizationReader.CurrentUtilization;

            processReader = new ProcessesReader();
        }

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

        public Task<Cpu> GetTotalCpuUsage()
        {
            var tsc = new TaskCompletionSource<Cpu>();
            var cpu = new Cpu()
            {
                Name = "Total",
                PercentageUsage = cpuUtilizationReader.CurrentUtilization
            };

            tsc.SetResult(cpu);
            return tsc.Task;

        }

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
                memory.PhysicalTotalGb = Convert.ToInt32(memoryStatusEx.ullTotalPhys/1024/1024/1024);
                memory.PhysicalAvailableGb = Convert.ToInt32(memoryStatusEx.ullAvailPhys/1024/1024/1024);

                memory.SwapAvailableGb = Convert.ToInt32(memoryStatusEx.ullAvailVirtual/1024/1024/1024);
                memory.SwapTotalGb = Convert.ToInt32(memoryStatusEx.ullTotalVirtual/1024/1024/1024);
                memory.SwapPercentageUsed = (int)Math.Round((decimal)((memory.SwapTotalGb - memory.SwapAvailableGb)*100)/memory.SwapTotalGb, 0);
            }

            tsc.SetResult(memory);
            return tsc.Task;
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

        public Task<Process> GetProcessActivity()
        {
            // var queryString = "SELECT Name, PercentProcessorTime, IDProcess, ThreadCount, HandleCOunt, PriorityBase, WorkingSetPrivate FROM Win32_PerfFormattedData_PerfProc_Process WHERE IDProcess > 0";
            // var wmiReader = new WmiReader();
            // var results = await wmiReader.Execute(queryString);
            
            var tsc = new TaskCompletionSource<Process>();
            var pws = processReader.Refresh();

            var p = new Process()
            {
                Processes = pws.Select(pw => new ProcessInfo(){
                                Name = pw.Name,
                                PercentProcessorTime = pw.CpuUsage,
                                IDProcess = pw.Pid,
                                Owner = pw.UserName,
                                ThreadCount = pw.ThreadCount,
                                MemoryUsageMb = pw.MemoryUsageMB
                            })
                            .ToList()
            };

            tsc.SetResult(p);
            return tsc.Task;
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