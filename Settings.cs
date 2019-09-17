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

        public static Color CPUBarColor = Color.Green;

        public static Color MemoryBarColor = Color.Red;

        public static Color NetworkDownloadTextColor = Color.Green;

        public static Color NetworkUploadTextColor = Color.Red;

        public static Color DiskReadTextColor = Color.Green;

        public static Color DiskWriteTextColor = Color.Red;

        public static Color ProcessesListHeaderTextColor = Color.White;

        public static Color ProcessesListHeaderBackgroundColor = Color.DarkGray;

        public static Color ProcessesListFooterTextColor = Color.Brown;

        public static Color ProcessesListFooterBackgroundColor = Color.Brown;
    }
}