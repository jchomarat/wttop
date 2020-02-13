using Terminal.Gui;

namespace wttop
{
    public class Settings
    {
        # region Main app

        // Main app title
        public string MainAppTitle {get; set; } = "wttop";

         // Main text color throughout the entire app.
        public Color MainForegroundColor {get; set; } = Color.White;

        // Main background color throughtout the enture app.
        public Color MainBackgroundColor {get; set; } = Color.Black;

        # endregion

        # region Labels Widget

        public Color LabelWidgetHeaderColor {get; set; } = Color.Blue;

        #endregion
        
        # region CpuRam Widget

        // CPU widget title
        public string CpuRamWidgetTitle {get; set; }= "CPU & Memory";

        // Color of the CPU graph bar
        public Color CpuRamWidgetBarColor {get; set; } = Color.Green;

        // Color of the memory graph bar
        public Color CpuRamWidgetRamBarColor {get; set; } = Color.Brown;

        // Color of the swap memory graph bar
        public Color CpuRamWidgetSwapBarColor {get; set; } = Color.Red;

        # endregion

        # region Network widget

        // Network widget title
        public string NetworkWidgetTitle {get; set; } = "Network";

        // Color of the network download value text
        public Color NetworkWidgetDownloadTextColor {get; set;} = Color.Green;

        // Color of the network upload value text
        public Color NetworkWidgetUploadTextColor {get; set; } = Color.Brown;

        # endregion        

        # region Disk widget

        // Disk widget title
        public string DiskWidgetTitle {get; set; } = "Disk";

        // Color of the disk read value text
        public Color DiskWidgetReadTextColor {get; set; } = Color.Green;

        // Color of the disk write value text
        public Color DiskWidgetWriteTextColor {get; set; } = Color.Brown;

        // Color of the disk space usage bar
        public Color DiskWidgetUsageBarColor {get; set; } = Color.Green;

        # endregion

        # region Processes list widget

        // Processes list widget title (it is only the top 20)
        public string ProcessesListWidgetTitle {get; set; } = "Processes list (top 15)";

        // Color of the Processes list header text
        public Color ProcessesListWidgetHeaderTextColor {get; set; } = Color.White;

        // Color of the processes list header background
        public Color ProcessesListWidgetHeaderBackgroundColor {get; set; } = Color.DarkGray;

        // Color of the Processes list footer text
        public Color ProcessesListWidgetFooterTextColor {get; set; } = Color.Brown;

        // Color of the processes list footer background
        public Color ProcessesListWidgetFooterBackgroundColor {get; set; } = Color.Black;

        # endregion   
    }
}