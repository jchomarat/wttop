using System;
using System.Collections.Generic;
using System.Linq;

namespace wttop.Helpers {

    public class WindowsDriver : ISystemInfo {

        public int GetCPUsCount() {
             WMIWrapper wmi = new WMIWrapper();
             return wmi.GetNumberOfLogicalProcessors();
        }

        public IEnumerable<Cpu> GetCPUsUsage() {
             WMIWrapper wmi = new WMIWrapper();

             return wmi.GetCPUsUsage()
                .Select(tuple => new wttop.Helpers.Cpu(){
                    Name = tuple.Item1,
                    PercentageUsage = int.Parse(tuple.Item2)
                });
        }

        public Memory GetMemoryUsage() {
            WMIWrapper wmi = new WMIWrapper();
            var memoryInfo = wmi.GetMemoryUsageKb();

            return new Memory()
            {
                TotalKb = memoryInfo.Item1,
                AvailableKb = memoryInfo.Item2
            };
        }
    }
}