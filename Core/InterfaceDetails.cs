namespace wttop.Core
{
    /// <summary>
    /// Interface details
    /// </summary>
    public class InterfaceDetails
    {
        /// <summary>
        /// Get or set the interface name
        /// </summary>
        public string Name {get; set; }

        /// <summary>
        /// Get or set the bytes received
        /// </summary>
        public long BytesReceived {get; set; }
       
        /// <summary>
        /// Get or set the bytes sent
        /// </summary>
        public long BytesSent {get; set; }
    }
}