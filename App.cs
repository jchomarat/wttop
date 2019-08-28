using System;
using Terminal.Gui;
using Microsoft.Extensions.DependencyInjection;
using wttop.Widgets;
using wttop.Widgets.Common;
using wttop.Helpers;

namespace wttop
{
    class App
    {
        static void Main(string[] args)
        {
            var serviceProvider = new ServiceCollection()
                .AddSingleton<ISystemInfo, WindowsDriver>()
                .BuildServiceProvider();

            Application.Init();
            var top = Application.Top;

            // Main color schema
            var mainColorScheme = new ColorScheme();
            var mainColorAttributes = Terminal.Gui.Attribute.Make(Color.White, Color.Black);
            mainColorScheme.Focus = mainColorAttributes;
            mainColorScheme.HotFocus = mainColorAttributes;
            mainColorScheme.HotNormal = mainColorAttributes;
            mainColorScheme.Normal = mainColorAttributes;

            // Creates the top-level window to show
            var win = new Window("wttop")
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

            var viewTopLeft = new CPUGraphs("CPU", serviceProvider)
            {
                X = 0,
                Y = Pos.Bottom(osInfo),
                Width = Dim.Percent(50),
                Height= Dim.Sized(20)
            };
            
            win.Add(viewTopLeft);

            var viewTopRight = new View()
            {
                X = Pos.Right(viewTopLeft),
                Y = Pos.Bottom(osInfo),
                Width = Dim.Fill(),
                Height= Dim.Sized(20)
            };

            win.Add(viewTopRight);

            var memoryGraph = new MemoryGraph("Memory", serviceProvider) 
            {
                X = 0,
                Y = 0,
                Width = Dim.Fill(),
                Height= Dim.Percent(33)
            };

            var networkGraph = new NetworkGraph("Network activity", serviceProvider)
            {
                X = 0,
                Y = Pos.Bottom(memoryGraph),
                Width = Dim.Fill(),
                Height= Dim.Fill()
            };

            viewTopRight.Add(memoryGraph); 
            viewTopRight.Add(networkGraph); 

            var token = Application.MainLoop.AddTimeout(TimeSpan.FromSeconds(1), (MainLoop) => {
                // List all component to refresh
                osInfo.Update(MainLoop);
                viewTopLeft.Update(MainLoop);
                memoryGraph.Update(MainLoop);
                networkGraph.Update(MainLoop);
                return true;
            });

            Application.Run();
        }
    }
}
