using System.Collections.Generic;
using System.Linq;

namespace wttop.Core
{
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
             return wmi.GetCPUsUsage()
                .Select(tuple => new wttop.Core.Cpu()
                {
                    Name = tuple.Item1,
                    PercentageUsage = int.Parse(tuple.Item2)
                });
        }

        public Memory GetMemoryUsage()
        {
            var memoryInfo = wmi.GetMemoryUsageKb();

            return new Memory()
            {
                TotalKb = memoryInfo.Item1,
                AvailableKb = memoryInfo.Item2
            };
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
    }
}