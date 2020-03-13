using System.Collections.Generic;
using System.Linq;

namespace wttop.Core
{
    /// <summary>
    /// Disk details
    /// </summary>
    public class Disk
    {
        /// <summary>
        /// A list of disk
        /// </summary>
        public List<DiskDetails> Disks { get; set; } = new List<DiskDetails>();

        /// <summary>
        /// Total number of bytes written
        /// </summary>
        public long TotalBytesWritten => Disks.Sum(d => d.BytesWriten);

        /// <summary>
        /// Total number of bytes read
        /// </summary>
        public long TotalBytesRead => Disks.Sum(d => d.BytesRead);
    }
}