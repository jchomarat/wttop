using System;
using System.Text;
using System.Runtime.InteropServices;

namespace wttop.Core
{
    internal class WtsApi32
    {
        [Flags]
        internal enum WtsInfoClass
        {
            WTSUserName = 5, 
            WTSDomainName = 7,
        }

        [DllImport("Wtsapi32.dll")]
        internal static extern bool WTSQuerySessionInformation(IntPtr hServer, int sessionId, WtsInfoClass InfoClass, out IntPtr ppBuffer, out int pBytesReturned);

        [DllImport("Wtsapi32.dll")]
        internal static extern void WTSFreeMemory(IntPtr pointer);
    }
}