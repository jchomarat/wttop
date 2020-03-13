using Terminal.Gui;

namespace wttop
{
    /// <summary>
    /// The settings class
    /// </summary>
    public class Settings
    {
        # region Main app

        /// <summary>Main app title</summary>
        public string MainAppTitle { get; set; } = "wttop";

        /// <summary>Main text color throughout the entire app</summary>        
        public Color MainForegroundColor { get; set; } = Color.White;

        /// <summary>Main background color throughtout the enture app</summary>
        public Color MainBackgroundColor { get; set; } = Color.Black;

        # endregion

        # region Labels Widget

        /// <summary>Label widget header color</summary>
        public Color LabelWidgetHeaderColor { get; set; } = Color.Blue;

        #endregion

        #region CpuRam Widget

        /// <summary>CPU widget title</summary>
        public string CpuRamWidgetTitle { get; set; } = "CPU & Memory";

        /// <summary>Color of the CPU graph bar</summary>
        public Color CpuRamWidgetBarColor { get; set; } = Color.Green;

        /// <summary>Color of the memory graph bar</summary>
        public Color CpuRamWidgetRamBarColor { get; set; } = Color.Brown;

        /// <summary>Color of the swap memory graph bar</summary>
        public Color CpuRamWidgetSwapBarColor { get; set; } = Color.Red;

        # endregion

        # region Network widget

        /// <summary>Network widget title</summary>
        public string NetworkWidgetTitle { get; set; } = "Network";

        /// <summary>Color of the network download value text</summary>
        public Color NetworkWidgetDownloadTextColor { get; set; } = Color.Green;

        /// <summary>Color of the network upload value text</summary>
        public Color NetworkWidgetUploadTextColor { get; set; } = Color.Brown;

        # endregion        

        # region Disk widget

        /// <summary>Disk widget title</summary>
        public string DiskWidgetTitle { get; set; } = "Disk";

        /// <summary>Color of the disk read value text</summary>
        public Color DiskWidgetReadTextColor { get; set; } = Color.Green;

        /// <summary>Color of the disk write value text</summary>
        public Color DiskWidgetWriteTextColor { get; set; } = Color.Brown;

        /// <summary>Color of the disk space usage bar</summary>
        public Color DiskWidgetUsageBarColor { get; set; } = Color.Magenta;

        # endregion

        # region Processes list widget

        /// <summary>Processes list widget title (it is only the top 20)</summary>
        public string ProcessesListWidgetTitle { get; set; } = "Processes list (top 15)";

        /// <summary>Color of the Processes list header text</summary>
        public Color ProcessesListWidgetHeaderTextColor { get; set; } = Color.White;

        /// <summary>Color of the processes list header background</summary>
        public Color ProcessesListWidgetHeaderBackgroundColor { get; set; } = Color.DarkGray;

        /// <summary>Color of the Processes list footer text</summary>
        public Color ProcessesListWidgetFooterTextColor { get; set; } = Color.Brown;

        /// <summary>Color of the processes list footer background</summary>
        public Color ProcessesListWidgetFooterBackgroundColor { get; set; } = Color.Black;

        # endregion   
    }
}