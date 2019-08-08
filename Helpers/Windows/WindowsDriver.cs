using System;
using System.Collections.Generic;
using System.Linq;

namespace wttop.Helpers {

    public class WindowsDriver : ISystemInfo {

        public int GetCPUsCount() {
            return 8;
        }

        public IEnumerable<Cpu> GetCPUsUsage() {
             WMIWrapper wmi = new WMIWrapper();

             return wmi.GetCPUsUsage()
                .Select(tuple => new wttop.Helpers.Cpu(){
                    Name = tuple.Item1,
                    PercentageUsage = int.Parse(tuple.Item2)
                });

            // return new List<Cpu>() {
            //     new Cpu() {Name = "0", PercentageUsage = r.Next(1, 100)},
            //     new Cpu() {Name = "1", PercentageUsage = r.Next(1, 100)},
            //     new Cpu() {Name = "2", PercentageUsage = r.Next(1, 100)},
            //     new Cpu() {Name = "3", PercentageUsage = r.Next(1, 100)},
            //     new Cpu() {Name = "4", PercentageUsage = r.Next(1, 100)},
            //     new Cpu() {Name = "5", PercentageUsage = r.Next(1, 100)},
            //     new Cpu() {Name = "6", PercentageUsage = r.Next(1, 100)},
            //     new Cpu() {Name = "7", PercentageUsage = r.Next(1, 100)},
            // };
        }
    }
}