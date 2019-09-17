using System;
using System.Runtime.InteropServices;
using Terminal.Gui;
using Microsoft.Extensions.DependencyInjection;
using wttop.Widgets;
using wttop.Core;
using wttop.Core.ext;

namespace wttop
{
    /// Main class, called to start the application
    class App
    {
        static void Main(string[] args)
        {
            // Use injection to send the driver implementation to the core
            ServiceCollection serviceCollection = new ServiceCollection();
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                serviceCollection.AddSingleton<ISystemInfo, WindowsDriver>();
            }
            else
            {
                // Other drivers not implemented yet
                throw new NotImplementedException("This wttop version only supports Windows. Linux & OSX will come.");
            }
            var serviceProvider = serviceCollection.BuildServiceProvider();

            try
            {
                Application.Init();
            }
            catch
            {
                throw new ApplicationException("Could not initialize the application.");
            }

            // Build the application UI with widgets
            var top = Application.Top;

            // Main color schema
            var mainColorScheme = new ColorScheme();
            mainColorScheme.SetColorsForAllStates(Settings.MainForegroundColor, Settings.MainBackgroundColor);

            // Creates the top-level window to show
            var win = new Window(Settings.MainAppTitle)
            {
                X = 0,
                Y = 0,

                Width = Dim.Fill(),
                Height = Dim.Fill()
            };
            win.ColorScheme = mainColorScheme;
            top.Add (win);

            var osInfo = new InfoText(serviceProvider)
            {
                X = 0,
                Y = 0,
                Width = Dim.Fill(),
                Height = Dim.Sized(3)
            };

            win.Add(osInfo);

            var cpuGraph = new CPUGraphs(Settings.CPUWidgetTitle, serviceProvider)
            {
                X = 0,
                Y = Pos.Bottom(osInfo),
                Width = Dim.Percent(50),
                Height= Dim.Sized(20),
                BarColor = Settings.CPUBarColor
            };
            cpuGraph.Init();
            
            win.Add(cpuGraph);

            var viewTopRight = new View()
            {
                X = Pos.Right(cpuGraph),
                Y = Pos.Bottom(osInfo),
                Width = Dim.Fill(),
                Height= Dim.Sized(20)
            };

            win.Add(viewTopRight);

            var memoryGraph = new MemoryGraph(Settings.MemoryWidgetTitle, serviceProvider) 
            {
                X = 0,
                Y = 0,
                Width = Dim.Fill(),
                Height= Dim.Sized(6),
                BarColor = Settings.MemoryBarColor
            };
            memoryGraph.Init();

            var networkGraph = new NetworkGraph(Settings.NetworkWidgetTitle, serviceProvider)
            {
                X = 0,
                Y = Pos.Bottom(memoryGraph),
                Width = Dim.Fill(),
                Height= Dim.Sized(7),
                DownloadTextColor = Settings.NetworkDownloadTextColor,
                UploadTextColor = Settings.NetworkUploadTextColor
            };
            networkGraph.Init();

            var diskGraph = new DiskGraph(Settings.DiskWidgetTitle, serviceProvider)
            {
                X = 0,
                Y = Pos.Bottom(networkGraph),
                Width = Dim.Fill(),
                Height= Dim.Sized(7),
                ReadTextColor = Settings.DiskReadTextColor,
                WriteTextColor = Settings.DiskWriteTextColor
            };
            diskGraph.Init();

            viewTopRight.Add(memoryGraph); 
            viewTopRight.Add(networkGraph);
            viewTopRight.Add(diskGraph);

            var processList = new ProcessList(Settings.ProcessesListWidgetTitle, serviceProvider)
            {
                X = 0,
                Y = Pos.Bottom(cpuGraph),
                Width = Dim.Fill(),
                Height= Dim.Fill(),
                HeaderTextColor = Settings.ProcessesListHeaderTextColor,
                HeaderBackgroundColor = Settings.ProcessesListHeaderBackgroundColor,
                FooterTextColor = Settings.ProcessesListFooterTextColor,
                FooterBackgroundColor = Settings.ProcessesListFooterBackgroundColor
            };
            processList.Init();

            win.Add(processList);

            // Refresh section. Every second, update on all listed widget will be called
            //TODO: allow different rate per widget
            var token = Application.MainLoop.AddTimeout(TimeSpan.FromSeconds(1), (MainLoop) => {
                // List all component to refresh
                osInfo.Update(MainLoop);
                cpuGraph.Update(MainLoop);
                memoryGraph.Update(MainLoop);
                networkGraph.Update(MainLoop);
                diskGraph.Update(MainLoop);
                processList.Update(MainLoop);
                return true;
            });

            try
            {
                Application.Run();
            }
            catch
            {
                throw new ApplicationException("Could not launch the application.");
            }
        }
    }
}
