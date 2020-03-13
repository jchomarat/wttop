using System;
using System.Linq;
using System.Threading.Tasks;
using Terminal.Gui;
using Mono.Terminal;
using wttop.Widgets.Common;

namespace wttop.Widgets
{
    /// <summary>
    /// Widget to list running processes.
    /// This class also uses Grid, a generic component that draws a grid.
    /// </summary>
    public class ProcessListWidget : WidgetFrame
    { 
        private Grid _grid;
        private ProcessListDataSourceBuilder _dataSource;
        
        /// <summary>
        /// Get or set the Order
        /// </summary>
        public ProcessListOrder Order { get; set; } = ProcessListOrder.Cpu;

        protected override int RefreshTimeSeconds => 3;

        /// <summary>
        /// Process the list of widgets
        /// </summary>
        /// <param name="serviceProvider"></param>
        public ProcessListWidget(IServiceProvider serviceProvider) : base(serviceProvider) { }

        protected override void DrawWidget()
        {
            Title = settings.ProcessesListWidgetTitle;            

            _dataSource = new ProcessListDataSourceBuilder();
            _dataSource.HeaderStyle = Terminal.Gui.Attribute.Make(settings.ProcessesListWidgetHeaderTextColor, settings.ProcessesListWidgetHeaderBackgroundColor);
            _dataSource.FooterStyle = Terminal.Gui.Attribute.Make(settings.ProcessesListWidgetFooterTextColor, settings.ProcessesListWidgetFooterBackgroundColor);
            _grid = new Grid(_dataSource)
            {
                X = 0,
                Y = 0,
                Width = Dim.Fill(),
                Height = Dim.Fill() 
            };

            Add(_grid);
        }

        protected override async Task Update(MainLoop MainLoop)
        {
            var processList = await systemInfo.GetProcessActivity();
            _dataSource.TotalProcessesCount = processList.Processes.Count();
            
            switch(Order)
            {
                case ProcessListOrder.Cpu:
                    _grid.Update(MainLoop, processList.GetTop15CPU.ToList());
                    break;

                case ProcessListOrder.Memory:
                    _grid.Update(MainLoop, processList.GetTop15Memory.ToList());
                    break;
            }            
        }
    }
}