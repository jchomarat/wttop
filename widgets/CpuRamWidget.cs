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
        
        Bar cpuBar;

        Bar ramBar;

        Label ramDetails;

        Bar swapBar;

        Label swapDetails;

        public CpuRamWidget(IServiceProvider serviceProvider) : base(serviceProvider) { }

        protected override void DrawWidget()
        {
            this.Title = settings.CpuRamWidgetTitle;
            
            // CPU part
            Label cpuTitle = new Label("CPU : ")
            {
                X = 1,
                Y = 1
            };

            Add(cpuTitle);
            
            cpuBar = new Bar(settings.CpuRamWidgetBarColor, settings.MainBackgroundColor)
            {
                X = Pos.Right(cpuTitle),
                Y = 1,
                Width = Dim.Percent(75),
                Height= Dim.Sized(1)
            };

            Add(cpuBar);

            // Memory part
            Label ramTitle = new Label("RAM : ")
            {
                X = 1,
                Y = 3
            };

            Add(ramTitle);
            
            ramBar = new Bar(settings.CpuRamWidgetRamBarColor, settings.MainBackgroundColor)
            {
                X = Pos.Right(ramTitle),
                Y = 3,
                Width = Dim.Percent(30),
                Height= Dim.Sized(1)
            };

            Add(ramBar);

            ramDetails = new Label(string.Empty)
            {
                X = Pos.Right(ramBar),
                Y = 3
            };
            
            Add(ramDetails);

            Label swapTitle = new Label("Swap: ")
            {
                X = 1,
                Y = 4
            };

            Add(swapTitle);
            
            swapBar = new Bar(settings.CpuRamWidgetSwapBarColor, settings.MainBackgroundColor)
            {
                X = Pos.Right(swapTitle),
                Y = 4,
                Width = Dim.Percent(30),
                Height= Dim.Sized(1)
            };

            Add(swapBar);

            swapDetails = new Label(string.Empty)
            {
                X = Pos.Right(swapBar),
                Y = 4
            };
            
            Add(swapDetails);
        }

        protected override async Task Update(MainLoop MainLoop)
        {             
            var cpusUsage = await systemInfo.GetTotalCpuUsage();
            cpuBar.Update(MainLoop, cpusUsage.PercentageUsage);

            // Memory refresh does not neet to be refreshed at the same pace, use cycle
            if (cycle == 0 || cycle == 59)
            {
                // First iteration, or one minute
                var memoryUsage = await systemInfo.GetMemoryUsage();
                ramBar.Update(MainLoop, memoryUsage.PhysicalPercentageUsed);
                ramDetails.Text = $"({memoryUsage.PhysicalAvailableGb} GB available)";

                swapBar.Update(MainLoop, memoryUsage.SwapPercentageUsed);
                swapDetails.Text = $"({memoryUsage.SwapAvailableGb} GB available)";

                if (cycle == 59)
                    cycle = 0;
                else
                    cycle ++;
            }
        }
    }
}