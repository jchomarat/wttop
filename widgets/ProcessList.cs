using System;
using System.Collections;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Terminal.Gui;
using Mono.Terminal;
using wttop.Core;
using wttop.Widgets.Common;

namespace wttop.Widgets {
    
    // Widget to list processes.
    // This class also uses Grid, a generic component that draws a grid.
    public class ProcessList : Widget
    { 
        Grid grid;

        ProcessListDataSourceBuilder dataSource;
        
        ISystemInfo systemInfo;

        public Color HeaderTextColor { get; set; } = Color.White;

        public Color HeaderBackgroundColor { get; set; } = Color.DarkGray;

        public Color FooterTextColor { get; set; } = Color.White;

        public Color FooterBackgroundColor { get; set; } = Color.Black;

        public ProcessList(string text, IServiceProvider serviceProvider) : base(text)
        {
            systemInfo = serviceProvider.GetService<ISystemInfo>();
        }

        public override void Init()
        {
            dataSource = new ProcessListDataSourceBuilder();
            dataSource.HeaderStyle = Terminal.Gui.Attribute.Make(HeaderTextColor, HeaderBackgroundColor);
            dataSource.FooterStyle = Terminal.Gui.Attribute.Make(FooterTextColor, FooterBackgroundColor);
            grid = new Grid(dataSource)
            {
                X = 0,
                Y = 0,
                Width = Dim.Fill(),
                Height = Dim.Fill()
            };

            Add(grid);
        }

        public override bool Update(MainLoop MainLoop)
        {
            var processList = systemInfo.GetProcessActivity();
            dataSource.TotalProcessesCount = processList.Processes.Count();

            grid.Update(MainLoop, processList.GetTop20.ToList());
            return true;
        }
    }

    // In order to use the Grid widget, we need a GridDataSourceBuilder to get the source, the refresh and the style.
    public class ProcessListDataSourceBuilder: GridDataSourceBuilder
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
                "Priority"
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
                p.MemoryUsageMB.ToString(),
                p.ThreadCount.ToString(),
                p.PriorityBase.ToString()
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