using System;
using System.Collections.Generic;
using System.Linq;

namespace wttop.Helpers {

    public class InterfaceDetails
    {
        public string Name {get; set; }

        public int BytesReceived {get; set; }
       
        public int BytesSent {get; set; }
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
        
        public int TotalBytesReceived
        {
            get 
            {
                return interfaces.Sum(inter => inter.BytesReceived);
            }
        }
       
        public int TotalBytesSent
        {
            get 
            {
                return interfaces.Sum(inter => inter.BytesSent);
            }
        }
    }
}