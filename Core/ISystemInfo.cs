using System.Collections.Generic;

namespace wttop.Core {
    
    /// <summary>
    /// Interface to implement in order to get system information.
    /// Only Windows is supported so far.
    /// If you want to implement it for Osx & Linux, we just need to implement this interface
    /// </summary>
    public interface ISystemInfo
    {
        OSInfo GetOSInfo();

        int GetCPUsCount();

        IEnumerable<Cpu> GetCPUsUsage();

        Memory GetMemoryUsage();

        Network GetNetworkStatistics();

        Process GetProcessActivity();

        Disk GetDiskActivity();

        Uptime GetSystemUpTime();

        SystemTime GetSystemDateTime();
    }
}