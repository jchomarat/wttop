using System;
using System.Threading.Tasks;
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
    public class App
    {
        public static void Main(string[] args)
        {
            StartApp();
        }

        /// <summary>
        /// Display the help and the about
        /// </summary>
        static Dialog AboutDialog()
        {
            var about = new Dialog("WTTOP: The New Windows Terminal System Monitor", 60, 10, null);
            about.Add(
                new Label("Version: 1.1")
                {
                    X = 2,
                    Y = 1
                },
                new Label("Author: Julien Chomarat (https://github.com/jchomarat)")
                {
                    X = 2,
                    Y = 2
                },
                new Label("Licence: MIT")
                {
                    X = 2,
                    Y = 3
                }
            );
            return about;
        }

        /// <summary>
        /// Start the application
        /// </summary>
        public static void StartApp()
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
                Console.WriteLine($"Error - could not initialize the application. The inner exception is {ex}");
            }

            // Build the application UI with widgets
            var top = Application.Top;

            // Main color schema
            var mainColorScheme = new ColorScheme();
            mainColorScheme.SetColorsForAllStates(settings.MainForegroundColor, settings.MainBackgroundColor);

            // Creates the top-level window to show
            var win = new WttopWindow(settings.MainAppTitle)
            {
                X = 0,
                Y = 0,

                Width = Dim.Fill(),
                Height = Dim.Fill()
            };
            win.ColorScheme = mainColorScheme;
            top.Add(win);

            var osInfoWidget = new OSInfoWidget(serviceProvider)
            {
                X = 0,
                Y = 0,
                Width = Dim.Fill(),
                Height = Dim.Sized(3),
                CanFocus = false
            };

            win.Add(osInfoWidget);

            var upTimeWidget = new UptimeWidget(serviceProvider)
            {
                X = 0,
                Y = 1,
                Width = Dim.Fill(),
                Height = Dim.Sized(3),
                CanFocus = false
            };

            win.Add(upTimeWidget);

            var systemTimeWidget = new SystemTimeWidget(serviceProvider)
            {
                X = 0,
                Y = 2,
                Width = Dim.Fill(),
                Height = Dim.Sized(3),
                CanFocus = false
            };

            win.Add(systemTimeWidget);

            var cpuRamWidget = new CpuRamWidget(serviceProvider)
            {
                X = 0,
                Y = Pos.Bottom(systemTimeWidget),
                Width = Dim.Percent(50),
                Height = Dim.Sized(18),
                CanFocus = false
            };

            win.Add(cpuRamWidget);

            var viewTopRight = new View()
            {
                X = Pos.Right(cpuRamWidget),
                Y = Pos.Bottom(systemTimeWidget),
                Width = Dim.Fill(),
                Height = Dim.Sized(18),
                CanFocus = false
            };

            win.Add(viewTopRight);

            var networkWidget = new NetworkWidget(serviceProvider)
            {
                X = 0,
                Y = 0,
                Width = Dim.Fill(),
                Height = Dim.Sized(6),
                CanFocus = false
            };

            var diskWidget = new DiskWidget(serviceProvider)
            {
                X = 0,
                Y = Pos.Bottom(networkWidget),
                Width = Dim.Fill(),
                Height = Dim.Fill(),
                CanFocus = false
            };

            viewTopRight.Add(networkWidget);
            viewTopRight.Add(diskWidget);

            var processListWidget = new ProcessListWidget(serviceProvider)
            {
                X = 0,
                Y = Pos.Bottom(cpuRamWidget),
                Width = Dim.Fill(),
                Height = Dim.Fill() - 1,
                CanFocus = false
            };

            win.Add(processListWidget);

            var toolbarView = new View()
            {
                X = 0,
                Y = Pos.Bottom(processListWidget),
                CanFocus = true
            };

            win.Add(toolbarView);

            toolbarView.Add(
                new Button("Cpu")
                {
                    X = 2,
                    Y = 0,
                    Clicked = () => { processListWidget.Order = ProcessListOrder.Cpu; }
                },
                new Button("Memory")
                {
                    X = 9,
                    Y = 0,
                    Clicked = () => { processListWidget.Order = ProcessListOrder.Memory; }
                },
                new Button("Quit")
                {
                    X = 24,
                    Y = 0,
                    Clicked = () => { top.Running = false; }
                },
                new Button("About")
                {
                    X = 33,
                    Y = 0,
                    Clicked = () => { Application.Run(AboutDialog()); }
                }
            );

            // Refresh section. Every second, update on all listed widget will be called
            // Each seconds the UI refreshs, but a frequency can be set by overridind the property RefreshTimeSeconds for each widdget
            int tick = 0;
            var token = Application.MainLoop.AddTimeout(TimeSpan.FromSeconds(1), (MainLoop) =>
            {
                // List all component to refresh
                Task.Run(async () =>
                {
                    await osInfoWidget.RefreshIfNeeded(MainLoop, tick);
                    await upTimeWidget.RefreshIfNeeded(MainLoop, tick);
                    await systemTimeWidget.RefreshIfNeeded(MainLoop, tick);
                    await cpuRamWidget.RefreshIfNeeded(MainLoop, tick);
                    await networkWidget.RefreshIfNeeded(MainLoop, tick);
                    await diskWidget.RefreshIfNeeded(MainLoop, tick);
                    await processListWidget.RefreshIfNeeded(MainLoop, tick);
                });
                tick++;
                // Every hour put it back to 0
                if (tick > 360) tick = 1;

                return true;
            });

            try
            {
                Application.Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error - could not launch the application. The inner exception is {ex}");
            }
        }
    }
}
