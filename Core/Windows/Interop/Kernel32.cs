using System;
using System.Text;
using System.Runtime.InteropServices;

namespace wttop.Core
{
    internal class Kernel32
    {
        [StructLayout(LayoutKind.Sequential)]
        internal struct SYSTEM_INFO
        {
            internal ushort wProcessorArchitecture;
            internal ushort wReserved;
            internal uint dwPageSize;
            internal IntPtr lpMinimumApplicationAddress;
            internal IntPtr lpMaximumApplicationAddress;
            internal IntPtr dwActiveProcessorMask;
            internal uint dwNumberOfProcessors;
            internal uint dwProcessorType;
            internal uint dwAllocationGranularity;
            internal ushort wProcessorLevel;
            internal ushort wProcessorRevision;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        internal class MEMORYSTATUSEX
        {
            internal uint dwLength;
            internal uint dwMemoryLoad;
            internal ulong ullTotalPhys;
            internal ulong ullAvailPhys;
            internal ulong ullTotalPageFile;
            internal ulong ullAvailPageFile;
            internal ulong ullTotalVirtual;
            internal ulong ullAvailVirtual;
            internal ulong ullAvailExtendedVirtual;

            public MEMORYSTATUSEX()
            {
                this.dwLength = (uint)Marshal.SizeOf(typeof(Kernel32.MEMORYSTATUSEX));
            }
        }
        
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetSystemTimes(out ulong idleTime, out ulong kernelTime, out ulong userTime);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        internal static extern void GetSystemInfo(out SYSTEM_INFO si);

        [DllImport("kernel32.dll")]
        internal static extern int GetProcessAffinityMask(IntPtr handle, out IntPtr processAffinityMask, out IntPtr systemAffinityMask);

        [DllImport("kernel32.dll", SetLastError=true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetPhysicallyInstalledSystemMemory(out ulong MemoryInKilobytes);

        [return: MarshalAs( UnmanagedType.Bool )]
        [DllImport( "kernel32.dll", CharSet = CharSet.Auto, EntryPoint = "GlobalMemoryStatusEx", SetLastError = true )]
        internal static extern bool GlobalMemoryStatusEx( [In, Out] MEMORYSTATUSEX lpBuffer);

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern IntPtr OpenProcess(int processAccess, bool bInheritHandle, int processId);

        [DllImport("kernel32.dll", SetLastError=true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool CloseHandle(IntPtr hObject);

        [DllImport("kernel32.dll")]
        internal static extern bool QueryFullProcessImageName(IntPtr hprocess, int dwFlags, StringBuilder lpExeName, out int size);
    }
}