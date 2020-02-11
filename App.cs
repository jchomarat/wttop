﻿using System;
using System.Configuration;
using System.Runtime.InteropServices;
using Terminal.Gui;
using Microsoft.Extensions.DependencyInjection;
using wttop.Widgets;
using wttop.Core;
using wttop.Core.ext;

namespace wttop
{
    /// <summary>
    /// Main class to start the application
    /// </summary>
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

        /// <summary>
        /// Parse command line arguments
        /// </summary>
        /// <param name="args">Argumenrs array passed to the exe.</param>
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

        /// <summary>
        /// Display the help and the about
        /// </summary>
        static void ShowHelp()
        {
            Console.WriteLine("");
            Console.WriteLine(" WTTOP: the new Windows Terminal system monitor");
            Console.WriteLine("   Author:  Julien Chomarat (https://github.com/jchomarat)");
            Console.WriteLine("   Version: 1.1");
            Console.WriteLine("   Licence: MIT");
            Console.WriteLine("");
            Console.WriteLine(" Usage: wttop [options]");
            Console.WriteLine("");
            Console.WriteLine(" Options:");
            Console.WriteLine("    -h, --help    Show this help");
            Console.WriteLine("");
        }

        /// <summary>
        /// Start the application
        /// </summary>
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
            catch (Exception ex)
            {
                Console.WriteLine($"Error - could not initialize the application. The inner exception is {ex.Message}");
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

            var osInfo = new wttop.Widgets.OSInfo(serviceProvider)
            {
                X = 0,
                Y = 0,
                Width = Dim.Fill(),
                Height = Dim.Sized(3)
            };

            win.Add(osInfo);

            var upTime = new wttop.Widgets.Uptime(serviceProvider)
            {
                X = 0,
                Y = 1,
                Width = Dim.Fill(),
                Height = Dim.Sized(3)
            };

            win.Add(upTime);

            var systemTime = new wttop.Widgets.SystemTime(serviceProvider)
            {
                X = 0,
                Y = 2,
                Width = Dim.Fill(),
                Height = Dim.Sized(3)
            };

            win.Add(systemTime);

            var cpuGraph = new CPUGraphs(serviceProvider)
            {
                X = 0,
                Y = Pos.Bottom(systemTime),
                Width = Dim.Percent(50),
                Height= Dim.Sized(18)
            };
            
            win.Add(cpuGraph);

            var viewTopRight = new View()
            {
                X = Pos.Right(cpuGraph),
                Y = Pos.Bottom(systemTime),
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
            // Each seconds the UI refreshs, but a frequency can be set by overridind the property RefreshTimeSeconds for each widdget
            int tick = 0;
            var token = Application.MainLoop.AddTimeout(TimeSpan.FromSeconds(1), (MainLoop) => {
                // List all component to refresh
                osInfo.RefreshIfNeeded(MainLoop, tick);
                upTime.RefreshIfNeeded(MainLoop, tick);
                systemTime.RefreshIfNeeded(MainLoop, tick);
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
            catch(Exception ex)
            {
                Console.WriteLine($"Error - could not launch the application. The inner exception is {ex.Message}");
            }
        }
    }
}
