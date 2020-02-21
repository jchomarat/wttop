using System;
using System.Text;
using System.Runtime.InteropServices;

namespace wttop.Core
{
    internal class PsApi
    {
        [DllImport("Psapi.dll", SetLastError=true)]
        internal static extern bool EnumProcesses(
                [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.U4)] [In][Out] UInt32[] processIds,
                UInt32 arraySizeBytes,
                [MarshalAs(UnmanagedType.U4)] out UInt32 bytesCopied
        );

        [DllImport("Psapi.dll", SetLastError=true)]
        internal static extern uint GetModuleBaseName(IntPtr hProcess, IntPtr hModule, StringBuilder lpBaseName, int nSize);

        [DllImport("psapi.dll", CallingConvention=CallingConvention.StdCall, SetLastError = true)]
        internal static extern int EnumProcessModules(IntPtr hProcess, [Out] IntPtr lphModule, uint cb, out uint lpcbNeeded);
    }
}