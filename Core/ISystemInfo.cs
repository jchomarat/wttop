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
        /// <summary>
        /// During init of the UI, we need to know how many CPUs and disks we have, so these properties
        /// must be instantiate on the driver instantiation
        /// </summary>
        int CpuCount { get; }

        /// <summary>
        /// Get the disk count size
        /// </summary>
        int DiskCount{ get; }

        /// <summary>
        /// Provide the operating system information
        /// </summary>
        /// <returns>Operating system information </returns>
        Task<OSInfo> GetOSInfo();

        /// <summary>
        /// Provide the list of CPU
        /// </summary>
        /// <returns>A list of CPU</returns>
        Task<IEnumerable<Cpu>> GetCPUsUsage();

        /// <summary>
        /// Get the memory usage
        /// </summary>
        /// <returns>Memory information</returns>
        Task<Memory> GetMemoryUsage();

        /// <summary>
        /// Get the network statistics
        /// </summary>
        /// <returns>Network information</returns>
        Task<Network> GetNetworkStatistics();

        /// <summary>
        /// Get the process activity
        /// </summary>
        /// <returns>Process information</returns>
        Task<Process> GetProcessActivity();

        /// <summary>
        /// Get the disk activity
        /// </summary>
        /// <returns>Disk information</returns>
        Task<Disk> GetDiskActivity();

        /// <summary>
        /// Get the disk storage information
        /// </summary>
        /// <returns>A list of storage information</returns>
        Task<IEnumerable<Storage>> GetDiskStorageInfo();

        /// <summary>
        /// Get the system uptime
        /// </summary>
        /// <returns>System uptime</returns>
        Task<Uptime> GetSystemUpTime();

        /// <summary>
        /// Get the system date time
        /// </summary>
        /// <returns>System time</returns>
        Task<SystemTime> GetSystemDateTime();

        /// <summary>
        /// Get the total CPU usage
        /// </summary>
        /// <returns>Cpu information</returns>
        Task<Cpu> GetTotalCpuUsage();
    }
}