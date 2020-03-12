using System;
using System.Collections.Generic;
using System.Linq;

namespace wttop.Core
{
    /// <summary>
    /// The network class
    /// </summary>
    public class Network
    {
        /// <summary>
        /// List of network interfaces
        /// </summary>
        public List<InterfaceDetails> Interfaces { get; set; } = new List<InterfaceDetails>();

        /// <summary>
        /// Total number of bytes received by the network adapter
        /// </summary>
        public long TotalBytesReceived => Interfaces.Sum(inter => inter.BytesReceived);

        /// <summary>
        /// Total number of bytes sent by the network adapter
        /// </summary>
        public long TotalBytesSent => Interfaces.Sum(inter => inter.BytesSent);
    }
}