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
        
        # region CPU Widget

        // CPU widget title
        public string CPUWidgetTitle {get; set; }= "CPU";

        // Color of the CPU graph bar
        public Color CPUBarColor {get; set; } = Color.Green;

        # endregion

        # region Memory widget

        // Memory widget title
        public string MemoryWidgetTitle {get; set; } = "Memory";

        // Color of the memory graph bar
        public Color MemoryBarColor {get; set; } = Color.Brown;

        # endregion

        # region Network widget

        // Network widget title
        public string NetworkWidgetTitle {get; set; } = "Network activity";

        // Color of the network download value text
        public Color NetworkDownloadTextColor {get; set;} = Color.Green;

        // Color of the network upload value text
        public Color NetworkUploadTextColor {get; set; } = Color.Brown;

        # endregion        

        # region Disk widget

        // Disk widget title
        public string DiskWidgetTitle {get; set; } = "Disk activity";

        // Color of the disk read value text
        public Color DiskReadTextColor {get; set; } = Color.Green;

        // Color of the disk write value text
        public Color DiskWriteTextColor {get; set; } = Color.Brown;

        # endregion

        # region Processes list widget

        // Processes list widget title (it is only the top 20)
        public string ProcessesListWidgetTitle {get; set; } = "Processes list (top 15)";

        // Color of the Processes list header text
        public Color ProcessesListHeaderTextColor {get; set; } = Color.White;

        // Color of the processes list header background
        public Color ProcessesListHeaderBackgroundColor {get; set; } = Color.DarkGray;

        // Color of the Processes list footer text
        public Color ProcessesListFooterTextColor {get; set; } = Color.Brown;

        // Color of the processes list footer background
        public Color ProcessesListFooterBackgroundColor {get; set; } = Color.Black;

        # endregion   
    }
}