using System;

namespace wttop.Core
{

    /// <summary>
    /// The storage class
    /// </summary>
    public class Storage
    {
        /// <summary>
        /// Get or set the volume caption
        /// </summary>
        public string VolumeCaption { get; set; }

        /// <summary>
        /// Get or set the volume name
        /// </summary>
        public string VolumeName { get; set; }

        /// <summary>
        /// Get or set the total size in kilo bytes
        /// </summary>
        public long TotalKb { get; set; }

        /// <summary>
        /// Get or set the available size in kilo bytes
        /// </summary>
        public long AvailableKb { get; set; }

        /// <summary>
        /// Get the used size in kilo bytes
        /// </summary>
        public long UsedKb => TotalKb - AvailableKb;

        /// <summary>
        /// Get the available size in giga bytes
        /// </summary>
        public decimal AvailableGB => Math.Round((decimal)AvailableKb / (1024 * 1024), 1);

        /// <summary>
        /// Get the used size in giga bytes
        /// </summary>
        public decimal UsedGB => Math.Round((decimal)UsedKb / (1024 * 1024), 1);

        /// <summary>
        /// Get the total size in giga bytes
        /// </summary>
        public decimal TotalGB => Math.Round((decimal)TotalKb / (1024 * 1024), 1);

        /// <summary>
        /// Get the percentage size used
        /// </summary>
        public int PercentageUsed => (int)Math.Floor((decimal)(100 * UsedKb) / TotalKb);
    }
}