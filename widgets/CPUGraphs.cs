using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Terminal.Gui;
using wttop.Widgets.Common;
using Mono.Terminal;
using wttop;
using wttop.Core;

namespace wttop.Widgets {
    
    // Widght that will display the CPU usage graph
    public class CPUGraphs : Widget
    {    
        Label[] cpus;
        
        Bar[] bars;
        
        ISystemInfo systemInfo;

        Settings settings;

        public CPUGraphs(IServiceProvider serviceProvider)
        {
            systemInfo = serviceProvider.GetService<ISystemInfo>();
            settings = serviceProvider.GetService<Settings>();

            this.Title = settings.CPUWidgetTitle;
            DrawWidget();
        }

        void DrawWidget()
        {
            var maxCpusCount = systemInfo.GetCPUsCount();
            
            cpus = new Label[maxCpusCount];
            bars = new Bar[maxCpusCount];
            var offsetY = 1;
            for (var i = 0; i < maxCpusCount; i++)
            {    
                cpus[i] = new Label($"cpu-{i}: ") {
                        X = 1,
                        Y = offsetY
                    };

                bars[i] = 
                    new Bar(settings.CPUBarColor, settings.MainBackgroundColor){
                        X = Pos.Right(cpus[i]),
                        Y = offsetY,
                        Width = Dim.Percent(75),
                        Height= Dim.Sized(1)
                    };

                offsetY += 2;
            }
        
            Add(cpus);
            Add(bars);
        }

        public override bool Update(MainLoop MainLoop)
        {
            var cpusUsage = systemInfo.GetCPUsUsage();
            for (var i = 0; i < bars.Length; i++)
            {
                bars[i].Update(MainLoop, cpusUsage.ElementAt(i).PercentageUsage);
            };
            return true;
        }
    }
}