namespace wttop.Core
{
    /// <summary>
    /// Disk details
    /// </summary>
    public class DiskDetails
    {
        /// <summary>
        /// Get or set the disk name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Get or set the bytes read
        /// </summary>
        public long BytesRead { get; set; }

        /// <summary>
        /// Get or set the bytes written
        /// </summary>
        public long BytesWriten { get; set; }
    }
}