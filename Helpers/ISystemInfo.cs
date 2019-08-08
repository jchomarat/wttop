using System;
using System.Collections.Generic;

namespace wttop.Helpers {
    
    public interface ISystemInfo {
        int GetCPUsCount();

        IEnumerable<Cpu> GetCPUsUsage();
    }
}