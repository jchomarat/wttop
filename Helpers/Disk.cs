using System;
using System.Collections.Generic;
using System.Linq;

namespace wttop.Helpers {

    public class DiskDetails
    {
        public string Name {get; set; }

        public int BytesRead {get; set; }
       
        public int BytesWrite {get; set; }
    }

    public class Disk
    {
        List<DiskDetails> disks = new List<DiskDetails>();

        public List<DiskDetails> Disks
        {
            get
            {
                return disks;
            }
            set 
            {
                disks = value;
            }
        }
        
        public int TotalBytesWritten
        {
            get 
            {
                return Disks.Sum(d => d.BytesWrite);
            }
        }
       
        public int TotalBytesRead
        {
            get 
            {
                return Disks.Sum(d => d.BytesRead);
            }
        }
    }
}