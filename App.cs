using System;
using Terminal.Gui;
using Microsoft.Extensions.DependencyInjection;
using wttop.Widgets;
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

            // Creates the top-level window to show
            var win = new Window("wttop")
            {
                X = 0,
                Y = 0,

                Width = Dim.Fill(),
                Height = Dim.Fill()
            };
            top.Add (win);

            var cpuGraphs = new CPUGraphs("CPU", serviceProvider)
            {
                X = 0,
                Y = 0,
                Width = Dim.Percent(50),
                Height= Dim.Percent(40)
            };
            
            win.Add(cpuGraphs);

            var memoryGraph = new MemoryGraph("Memory", serviceProvider) 
            {
                X = Pos.Right(cpuGraphs),
                Y = 0,
                Width = Dim.Fill(),
                Height= Dim.Percent(40)
            };

            win.Add(memoryGraph); 

            var token = Application.MainLoop.AddTimeout(TimeSpan.FromSeconds(1), (MainLoop) => {
                // List all component to refresh
                cpuGraphs.Update(MainLoop);
                memoryGraph.Update(MainLoop);
                return true;
            });

            Application.Run();
        }
    }
}
