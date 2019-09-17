using Terminal.Gui;

namespace wttop
{
    public class Settings
    {
        // Main app title
        public static string MainAppTitle = "wttop";

        // CPU widget title
        public static string CPUWidgetTitle = "CPU";

        // Memory widget title
        public static string MemoryWidgetTitle = "Memory";

        // Network widget title
        public static string NetworkWidgetTitle = "Network activity";

        // Disk widget title
        public static string DiskWidgetTitle = "Disk activity";

        // Processes list widget title (it is only the top 20)
        public static string ProcessesListWidgetTitle = "Processes list (top 20)";
       
        // Main text color throughout the entire app.
        public static Color MainForegroundColor = Color.White;

        // Main background color throughtout the enture app.
        public static Color MainBackgroundColor = Color.Black;

        // Color of the CPU graph bar
        public static Color CPUBarColor = Color.Green;

        // Color of the memory graph bar
        public static Color MemoryBarColor = Color.Brown;

        // Color of the network download value text
        public static Color NetworkDownloadTextColor = Color.Green;

        // Color of the network upload value text
        public static Color NetworkUploadTextColor = Color.Brown;

        // Color of the disk read value text
        public static Color DiskReadTextColor = Color.Green;

        // Color of the disk write value text
        public static Color DiskWriteTextColor = Color.Brown;

        // Color of the Processes list header text
        public static Color ProcessesListHeaderTextColor = Color.White;

        // Color of the processes list header background
        public static Color ProcessesListHeaderBackgroundColor = Color.DarkGray;

        // Color of the Processes list footer text
        public static Color ProcessesListFooterTextColor = Color.Brown;

        // Color of the processes list footer background
        public static Color ProcessesListFooterBackgroundColor = Color.Black;
    }
}