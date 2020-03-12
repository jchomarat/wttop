using System;
using System.Linq;
using System.Threading.Tasks;
using Terminal.Gui;
using wttop.Widgets.Common;
using Mono.Terminal;

namespace wttop.Widgets
{

    /// <summary>
    /// Widget that will display the CPU usage graph
    /// </summary>
    public class CpuRamWidget : WidgetFrame
    {
        private int _cycle = 0;
        private Bar _cpuBar;
        private Bar _ramBar;
        private Label _ramDetails;
        private Bar _swapBar;
        private Label _swapDetails;

        /// <summary>
        /// Main constructor for the RAM widget
        /// </summary>
        /// <param name="serviceProvider">A service provider</param>
        public CpuRamWidget(IServiceProvider serviceProvider) : base(serviceProvider) { }

        protected override void DrawWidget()
        {
            Title = settings.CpuRamWidgetTitle;

            // CPU part
            Label cpuTitle = new Label("CPU : ")
            {
                X = 1,
                Y = 1
            };

            Add(cpuTitle);

            _cpuBar = new Bar(settings.CpuRamWidgetBarColor, settings.MainBackgroundColor)
            {
                X = Pos.Right(cpuTitle),
                Y = 1,
                Width = Dim.Percent(75),
                Height = Dim.Sized(1)
            };

            Add(_cpuBar);

            // Memory part
            Label ramTitle = new Label("RAM : ")
            {
                X = 1,
                Y = 3
            };

            Add(ramTitle);

            _ramBar = new Bar(settings.CpuRamWidgetRamBarColor, settings.MainBackgroundColor)
            {
                X = Pos.Right(ramTitle),
                Y = 3,
                Width = Dim.Percent(30),
                Height = Dim.Sized(1)
            };

            Add(_ramBar);

            _ramDetails = new Label(string.Empty)
            {
                X = Pos.Right(_ramBar),
                Y = 3
            };

            Add(_ramDetails);

            Label swapTitle = new Label("Swap: ")
            {
                X = 1,
                Y = 4
            };

            Add(swapTitle);

            _swapBar = new Bar(settings.CpuRamWidgetSwapBarColor, settings.MainBackgroundColor)
            {
                X = Pos.Right(swapTitle),
                Y = 4,
                Width = Dim.Percent(30),
                Height = Dim.Sized(1)
            };

            Add(_swapBar);

            _swapDetails = new Label(string.Empty)
            {
                X = Pos.Right(_swapBar),
                Y = 4
            };

            Add(_swapDetails);
        }

        protected override async Task Update(MainLoop MainLoop)
        {
            var cpusUsage = await systemInfo.GetTotalCpuUsage();
            _cpuBar.Update(MainLoop, cpusUsage.PercentageUsage);

            // Memory refresh does not neet to be refreshed at the same pace, use cycle
            if (_cycle == 0 || _cycle == 59)
            {
                // First iteration, or one minute
                var memoryUsage = await systemInfo.GetMemoryUsage();
                _ramBar.Update(MainLoop, memoryUsage.PhysicalPercentageUsed);
                _ramDetails.Text = $"({memoryUsage.PhysicalAvailableGb} GB available)";

                _swapBar.Update(MainLoop, memoryUsage.SwapPercentageUsed);
                _swapDetails.Text = $"({memoryUsage.SwapAvailableGb} GB available)";

                _cycle = _cycle == 59 ? 0 : _cycle++;
            }
        }
    }
}