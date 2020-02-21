using System;
using System.Runtime.InteropServices;

namespace wttop.Core
{
    internal class CpuUtilizationReader
    {
        private struct ProcessCpuInformation
        {
            public long idleTime;
            public long kernelTime;
            public long userTime;
        }

        private ProcessCpuInformation _processCpuInfo = new ProcessCpuInformation();

        public int CurrentUtilization
        {
            get
            {
                if (!Kernel32.GetSystemTimes(out var idleTime, out var kernelTime, out var userTime))
                {
                    int error = Marshal.GetLastWin32Error();
                    var exception = new OutOfMemoryException();
                    throw exception;
                }

                long cpuTotalTime = ((long)userTime - _processCpuInfo.userTime) + ((long)kernelTime - _processCpuInfo.kernelTime);
                long cpuBusyTime = cpuTotalTime - ((long)idleTime - _processCpuInfo.idleTime);

                _processCpuInfo.kernelTime = (long)kernelTime;
                _processCpuInfo.userTime = (long)userTime;
                _processCpuInfo.idleTime = (long)idleTime;

                if (cpuTotalTime > 0 && cpuBusyTime > 0)
                {
                    long reading = cpuBusyTime * 100 / cpuTotalTime;
                    reading = Math.Min(reading, 100);
                    return (int)reading;
                }
                return 0;
            }
        }
    }
}