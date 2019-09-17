using System;
using System.Collections.Generic;
using System.Linq;

namespace wttop.Core {

    public class InterfaceDetails
    {
        public string Name {get; set; }

        public long BytesReceived {get; set; }
       
        public long BytesSent {get; set; }
    }

    public class Network
    {
        List<InterfaceDetails> interfaces = new List<InterfaceDetails>();

        public List<InterfaceDetails> Interfaces
        {
            get
            {
                return interfaces;
            }
            set 
            {
                interfaces = value;
            }
        }
        
        public long TotalBytesReceived
        {
            get 
            {
                return interfaces.Sum(inter => inter.BytesReceived);
            }
        }
       
        public long TotalBytesSent
        {
            get 
            {
                return interfaces.Sum(inter => inter.BytesSent);
            }
        }
    }
}