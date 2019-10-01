using System;
using System.Configuration;
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
            if (args.Length == 0)
            {
                // No args, launch the app
                StartApp();
            }
            else
            {
                HandleCommandArgs(args);
            }
        }

        static void HandleCommandArgs(string[] args)
        {
            if (args[0] == "-h" || args[0] == "--help")
            {
                ShowHelp();
            }
            else
            {
                Console.WriteLine("Invalid command.");
                ShowHelp();
            }
        }

        static void ShowHelp()
        {
            Console.WriteLine("");
            Console.WriteLine(" WTTOP: the new Windows Terminal system monitor");
            Console.WriteLine("   Author:  Julien Chomarat (https://github.com/jchomarat)");
            Console.WriteLine("   Version: 1.0");
            Console.WriteLine("   Licence: MIT");
            Console.WriteLine("");
            Console.WriteLine(" Usage: wttop [options]");
            Console.WriteLine("");
            Console.WriteLine(" Options:");
            Console.WriteLine("    -h, --help    Show this help");
            Console.WriteLine("");
        }

        static void StartApp()
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

            // Add the settings configuration
            serviceCollection.AddSingleton<Settings>();
            // Build the provider
            var serviceProvider = serviceCollection.BuildServiceProvider();

            // Get the settings for what can be done here
            var settings = new Settings();

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
            mainColorScheme.SetColorsForAllStates(settings.MainForegroundColor, settings.MainBackgroundColor);

            // Creates the top-level window to show
            var win = new Window(settings.MainAppTitle)
            {
                X = 0,
                Y = 0,

                Width = Dim.Fill(),
                Height = Dim.Fill()
            };
            win.ColorScheme = mainColorScheme;
            top.Add(win);

            var osInfo = new InfoText(serviceProvider)
            {
                X = 0,
                Y = 0,
                Width = Dim.Fill(),
                Height = Dim.Sized(3)
            };

            win.Add(osInfo);

            var cpuGraph = new CPUGraphs(serviceProvider)
            {
                X = 0,
                Y = Pos.Bottom(osInfo),
                Width = Dim.Percent(50),
                Height= Dim.Sized(18)
            };
            
            win.Add(cpuGraph);

            var viewTopRight = new View()
            {
                X = Pos.Right(cpuGraph),
                Y = Pos.Bottom(osInfo),
                Width = Dim.Fill(),
                Height= Dim.Sized(18)
            };

            win.Add(viewTopRight);

            var memoryGraph = new MemoryGraph(serviceProvider) 
            {
                X = 0,
                Y = 0,
                Width = Dim.Fill(),
                Height= Dim.Sized(6)
            };

            var networkGraph = new NetworkGraph(serviceProvider)
            {
                X = 0,
                Y = Pos.Bottom(memoryGraph),
                Width = Dim.Fill(),
                Height= Dim.Sized(6)
            };

            var diskGraph = new DiskGraph(serviceProvider)
            {
                X = 0,
                Y = Pos.Bottom(networkGraph),
                Width = Dim.Fill(),
                Height= Dim.Sized(6)
            };

            viewTopRight.Add(memoryGraph); 
            viewTopRight.Add(networkGraph);
            viewTopRight.Add(diskGraph);

            var processList = new ProcessList(serviceProvider)
            {
                X = 0,
                Y = Pos.Bottom(cpuGraph),
                Width = Dim.Fill(),
                Height= Dim.Fill()
            };

            win.Add(processList);

            // Refresh section. Every second, update on all listed widget will be called
            //TODO: allow different rate per widget
            int tick = 0;
            var token = Application.MainLoop.AddTimeout(TimeSpan.FromSeconds(1), (MainLoop) => {
                // List all component to refresh
                osInfo.RefreshIfNeeded(MainLoop, tick);
                cpuGraph.RefreshIfNeeded(MainLoop, tick);
                memoryGraph.RefreshIfNeeded(MainLoop, tick);
                networkGraph.RefreshIfNeeded(MainLoop, tick);
                diskGraph.RefreshIfNeeded(MainLoop, tick);
                processList.RefreshIfNeeded(MainLoop, tick);
                tick ++;
                // Every hour put it back to 0
                if (tick > 360) tick = 1;
                
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

        static bool Quit ()
        {
            var n = MessageBox.Query (50, 7, "Quit Demo", "Are you sure you want to quit this demo?", "Yes", "No");
            return n == 0;
        }
    }
}
