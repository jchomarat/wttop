using System.Collections.Generic;
using System.Linq;

namespace wttop.Helpers
{
    public class WindowsDriver : ISystemInfo
    {
        WMIWrapper wmi;

        public WindowsDriver()
        {
            wmi = new WMIWrapper();
        }

        public int GetCPUsCount()
        {
             return wmi.GetNumberOfLogicalProcessors();
        }

        public IEnumerable<Cpu> GetCPUsUsage()
        {
             return wmi.GetCPUsUsage()
                .Select(tuple => new wttop.Helpers.Cpu()
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
    }
}