using System;
using System.Collections;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Terminal.Gui;
using Mono.Terminal;
using wttop.Helpers;
using wttop.Widgets.Common;
using NStack;

namespace wttop.Widgets {
    
    public class ProcessList : Widget
    { 
        Grid grid;
        
        ISystemInfo systemInfo;

        public ProcessList(string text, IServiceProvider serviceProvider) : base(text)
        {
            systemInfo = serviceProvider.GetService<ISystemInfo>();
            DrawWidget();
        }

        void DrawWidget()
        {
            
            string[] headers = new string[] {"Name", "IDProcess", "PercentProcessorTime", "ThreadCount", "HandleCount", "PriorityBase"};
            decimal[] colWidth = new decimal[] {0.25m, 0.15m, 0.15m, 0.15m, 0.15m, 0.15m};
            grid = new Grid(headers, colWidth)
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
            var dataSource = new ProcessListDataSource(processList.GetTop10.ToList());
            
            grid.Update(MainLoop, dataSource);
            return true;
        }
    }

    public class ProcessListDataSource: GridDataSource
    {
        IList dataSource;

        public IList DataSource 
        {
            get {return dataSource;}
            set {dataSource = value;}
        }

        public ProcessListDataSource(IList src)
        {
            dataSource = src;
        }

        public string[] GetRowFromObject(int index)
        {
            ProcessInfo p = dataSource[index] as ProcessInfo;
            
            return new string[] {
                p.Name,
                p.IDProcess.ToString(),
                p.PercentProcessorTime.ToString(),
                p.ThreadCount.ToString(),
                p.HandleCount.ToString(),
                p.PriorityBase.ToString()
            };
        }
    }
}