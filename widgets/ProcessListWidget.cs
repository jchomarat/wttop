using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using Terminal.Gui;
using Mono.Terminal;
using wttop.Core;
using wttop.Widgets.Common;

namespace wttop.Widgets {

    public enum ProcessListOrder
    {
        CPU, MEMORY
    }

    /// <summary>
    /// Widget to list running processes.
    /// This class also uses Grid, a generic component that draws a grid.
    /// </summary>
    public class ProcessListWidget : WidgetFrame
    { 
        Grid grid;

        ProcessListDataSourceBuilder dataSource;

        ProcessListOrder orderBy = ProcessListOrder.CPU;

        protected override int RefreshTimeSeconds
        {
            get
            {
                return 3;
            }
        }

        public void SetOrderBy(ProcessListOrder Order)
        {
            orderBy = Order;
        }

        public ProcessListWidget(IServiceProvider serviceProvider) : base(serviceProvider) {}

        protected override void DrawWidget()
        {
            this.Title = settings.ProcessesListWidgetTitle;            

            dataSource = new ProcessListDataSourceBuilder();
            dataSource.HeaderStyle = Terminal.Gui.Attribute.Make(settings.ProcessesListWidgetHeaderTextColor, settings.ProcessesListWidgetHeaderBackgroundColor);
            dataSource.FooterStyle = Terminal.Gui.Attribute.Make(settings.ProcessesListWidgetFooterTextColor, settings.ProcessesListWidgetFooterBackgroundColor);
            grid = new Grid(dataSource)
            {
                X = 0,
                Y = 0,
                Width = Dim.Fill(),
                Height = Dim.Fill() 
            };

            Add(grid);
        }

        protected override async Task Update(MainLoop MainLoop)
        {
            var processList = await systemInfo.GetProcessActivity();
            dataSource.TotalProcessesCount = processList.Processes.Count();
            
            switch(orderBy)
            {
                case ProcessListOrder.CPU:
                    grid.Update(MainLoop, processList.GetTop15CPU.ToList());
                    break;

                case ProcessListOrder.MEMORY:
                    grid.Update(MainLoop, processList.GetTop15Memory.ToList());
                    break;
            }            
        }
    }

    /// <summary>
    /// In order to use the Grid widget, we need a GridDataSourceBuilder to get the source, the refresh and the style.
    /// </summary>
    public class ProcessListDataSourceBuilder: IGridDataSourceBuilder
    {
        IList dataSource;

        public IList DataSource 
        {
            get {return dataSource;}
            set {dataSource = value;}
        }

        public int TotalProcessesCount
        {
            get;
            set;
        }

        public string[] GetHeader()
        {
            return new string[] {
                "Name", 
                "ID", 
                "CPU%", 
                "Memory(MB)",
                "ThreadCount",  
                "Owner"
            };
        }

        public Terminal.Gui.Attribute HeaderStyle
        {
            get;
            set;
        }

        public decimal[] GetColumnsWidth()
        {
            return  new decimal[] {
                0.25m, 
                0.15m, 
                0.15m, 
                0.20m, 
                0.15m,
                0.10m
            };
        }

        public string[] GetRowData(int index)
        {
            if (dataSource == null)
                throw new ArgumentException("You need to set the IList data source");

            ProcessInfo p = dataSource[index] as ProcessInfo;
            
            return new string[] {
                p.Name,
                p.IDProcess.ToString(),
                p.PercentProcessorTime.ToString(),
                p.MemoryUsageMb.ToString(),
                p.ThreadCount.ToString(),
                p.Owner
            };
        }

        public string GetFooter()
        {
            return $"{TotalProcessesCount} running processes";
        }

        public Terminal.Gui.Attribute FooterStyle
        {
            get;
            set;
        }
    }
}