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
    public class CpuRamWidget : WidgetFrame
    {    
        int cycle = 0;

        Label[] cpus = null;
        
        Bar[] bars = null;

        Bar ramBar;

        Label ramDetails;

        Bar swapBar;

        Label swapDetails;

        public CpuRamWidget(IServiceProvider serviceProvider) : base(serviceProvider) { }

        protected override void DrawWidget()
        {
            this.Title = settings.CpuRamWidgetTitle;
            
            // CPU part
            var maxCpusCount = systemInfo.CpuCount;     
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
                    new Bar(settings.CpuRamWidgetBarColor, settings.MainBackgroundColor){
                        X = Pos.Right(cpus[i]),
                        Y = offsetY,
                        Width = Dim.Percent(75),
                        Height= Dim.Sized(1)
                    };

                offsetY += 1;
            }

            Add(cpus);
            Add(bars);

            // Memory part
            offsetY += 2;

            Label ramTitle = new Label("RAM : ")
            {
                X = 1,
                Y = offsetY
            };

            Add(ramTitle);
            
            ramBar = new Bar(settings.CpuRamWidgetRamBarColor, settings.MainBackgroundColor)
            {
                X = Pos.Right(ramTitle),
                Y = offsetY,
                Width = Dim.Percent(30),
                Height= Dim.Sized(1)
            };

            Add(ramBar);

            ramDetails = new Label(string.Empty)
            {
                X = Pos.Right(ramBar),
                Y = offsetY
            };
            
            Add(ramDetails);

            offsetY += 1;

            Label swapTitle = new Label("Swap: ")
            {
                X = 1,
                Y = offsetY
            };

            Add(swapTitle);
            
            swapBar = new Bar(settings.CpuRamWidgetSwapBarColor, settings.MainBackgroundColor)
            {
                X = Pos.Right(swapTitle),
                Y = offsetY,
                Width = Dim.Percent(30),
                Height= Dim.Sized(1)
            };

            Add(swapBar);

            swapDetails = new Label(string.Empty)
            {
                X = Pos.Right(swapBar),
                Y = offsetY
            };
            
            Add(swapDetails);
        }

        protected override async Task Update(MainLoop MainLoop)
        {             
            var cpusUsage = await systemInfo.GetCPUsUsage();
            for (var i = 0; i < bars.Length; i++)
            {
                bars[i].Update(MainLoop, cpusUsage.ElementAt(i).PercentageUsage);
            };

            // Memory refresh does not neet to be refreshed at the same pace, use cycle
            if (cycle == 0 || cycle == 59)
            {
                // First iteration, or one minute
                var memoryUsage = await systemInfo.GetMemoryUsage();
                ramBar.Update(MainLoop, memoryUsage.PercentageUsed);
                ramDetails.Text = $"({memoryUsage.AvailableGB} GB available)";

                swapBar.Update(MainLoop, memoryUsage.PercentageSwapAvailable);
                swapDetails.Text = $"({memoryUsage.AvailableSwapGB} GB available)";

                if (cycle == 59)
                    cycle = 0;
                else
                    cycle ++;
            }
        }
    }
}