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
        
        Bar2[] bars;
        
        ISystemInfo systemInfo;

        public Color BarColor { get; set; } = Color.Green;

        public CPUGraphs(string text, IServiceProvider serviceProvider) : base(text)
        {
            systemInfo = serviceProvider.GetService<ISystemInfo>();
        }

        public override void Init()
        {
            var maxCpusCount = systemInfo.GetCPUsCount();
            
            cpus = new Label[maxCpusCount];
            bars = new Bar2[maxCpusCount];
            var offsetY = 1;
            for (var i = 0; i < maxCpusCount; i++)
            {    
                cpus[i] = new Label($"cpu-{i}: ") {
                        X = 1,
                        Y = offsetY
                    };

                bars[i] = 
                    new Bar2(BarColor, Settings.MainBackgroundColor){
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