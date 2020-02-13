using System;

namespace wttop.Core {

    public class Storage {

        public string VolumeCaption {get; set; }

        public string VolumeName {get; set; }

        public long TotalKb {get; set; }
       
        public long AvailableKb {get; set; }

        public long UsedKb {
            get {
                return TotalKb - AvailableKb;
            }
        }

        public decimal AvailableGB {
            get {
                return Math.Round((decimal)AvailableKb/(1024*1024), 1); 
            }
        }

        public decimal UsedGB {
            get {
                return Math.Round((decimal)UsedKb/(1024*1024), 1); 
            }
        }

        public decimal TotalGB {
            get {
                return Math.Round((decimal)TotalKb/(1024*1024), 1); 
            }
        }

        public int PercentageUsed {
            get {
                return (int)Math.Floor((decimal)(100*UsedKb)/TotalKb);
            }
        }
    }
}