using System;
using System.Collections.Generic;
using System.Linq;

namespace wttop.Helpers {

    public class DiskDetails
    {
        public string Name {get; set; }

        public long BytesRead {get; set; }
       
        public long BytesWrite {get; set; }
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
        
        public long TotalBytesWritten
        {
            get 
            {
                return Disks.Sum(d => d.BytesWrite);
            }
        }
       
        public long TotalBytesRead
        {
            get 
            {
                return Disks.Sum(d => d.BytesRead);
            }
        }
    }
}