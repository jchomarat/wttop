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
        Task<OSInfo> GetOSInfo();

        Task<int> GetCPUsCount();

        Task<IEnumerable<Cpu>> GetCPUsUsage();

        Task<Memory> GetMemoryUsage();

        Task<Network> GetNetworkStatistics();

        Task<Process> GetProcessActivity();

        Task<Disk> GetDiskActivity();

        Task<Uptime> GetSystemUpTime();

        Task<SystemTime> GetSystemDateTime();
    }
}