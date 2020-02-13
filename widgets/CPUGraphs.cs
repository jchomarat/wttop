using System;
using System.Linq;
using System.Threading.Tasks;
using Terminal.Gui;
using wttop.Widgets.Common;
using Mono.Terminal;

namespace wttop.Widgets {

    /// <summary>
    /// Widget that will display the CPU usage graph
    /// </summary>
    public class CPUGraphs : WidgetFrame
    {    
        Label[] cpus = null;
        
        Bar[] bars = null;

        public CPUGraphs(IServiceProvider serviceProvider) : base(serviceProvider) { }

        protected override void DrawWidget()
        {
            this.Title = settings.CPUWidgetTitle;
            
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

                offsetY += 1;
            }

            Add(cpus);
            Add(bars);
        }

        protected override async Task Update(MainLoop MainLoop)
        {                
            var cpusUsage = await systemInfo.GetCPUsUsage();
            for (var i = 0; i < bars.Length; i++)
            {
                bars[i].Update(MainLoop, cpusUsage.ElementAt(i).PercentageUsage);
            };
        }
    }
}