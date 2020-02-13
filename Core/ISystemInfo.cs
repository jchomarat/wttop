using System.Collections.Generic;
using System.Threading.Tasks;

namespace wttop.Core {
    
    /// <summary>
    /// Interface to implement in order to get system information.
    /// Only Windows is supported so far.
    /// If you want to implement it for Osx & Linux, we just need to implement this interface
    /// </summary>
    public interface ISystemInfo
    {
        // During init of the UI, we need to know how many CPUs and disks we have, so these properties
        // must be instantiate on the driver instantiation
        int CpuCount { get; }

        int DiskCount{ get; }

        Task<OSInfo> GetOSInfo();

        Task<IEnumerable<Cpu>> GetCPUsUsage();

        Task<Memory> GetMemoryUsage();

        Task<Network> GetNetworkStatistics();

        Task<Process> GetProcessActivity();

        Task<Disk> GetDiskActivity();

        Task<IEnumerable<Storage>> GetDiskStorageInfo();

        Task<Uptime> GetSystemUpTime();

        Task<SystemTime> GetSystemDateTime();

        Task<Cpu> GetTotalCpuUsage();
    }
}