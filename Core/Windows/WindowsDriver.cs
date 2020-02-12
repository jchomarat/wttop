using System;
using System.Collections.Generic;
using System.Linq;

namespace wttop.Core
{
    /// <summary>
    /// Windows driver for wttop.
    /// This is a wrapper of WMI calls. Why WMi, well because in dotnetcore, we cannot get all this system values.
    /// WMI is probably the oldest way to do it, but a straigthforward one.
    /// </summary>
    public class WindowsDriver : ISystemInfo
    {
        WMIWrapper wmi;

        public WindowsDriver()
        {
            wmi = new WMIWrapper();
        }

        public OSInfo GetOSInfo()
        {
            return wmi.GetOperatingSystemInformation();
        }

        public int GetCPUsCount()
        {
             return wmi.GetNumberOfLogicalProcessors();
        }

        //TODO Change tuple to actual object
        public IEnumerable<Cpu> GetCPUsUsage()
        {
            return wmi.GetCPUsUsage();
        }

        public Memory GetMemoryUsage()
        {
            return wmi.GetMemoryUsageKb();
        }

        public Network GetNetworkStatistics()
        {
            var network = new Network();
            network.Interfaces = wmi.GetNetworkInterfacesDetails().ToList();
            return network;
        }

        public Process GetProcessActivity()
        {
            var process = new Process();
            process.Processes = wmi.GetProcessesActivity().ToList();
            return process;
        }

        public Disk GetDiskActivity()
        {
            var disk = new Disk();
            disk.Disks = wmi.GetAllDisksActivity().ToList();
            return disk;
        }

        public Uptime GetSystemUpTime()
        {
            return wmi.GetSystemUpTime();
        }

        public SystemTime GetSystemDateTime()
        {
            return new SystemTime(DateTime.Now);
        }
    }
}