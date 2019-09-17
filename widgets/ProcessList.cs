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

        ProcessListDataSourceBuilder dataSource;
        
        ISystemInfo systemInfo;

        public ProcessList(string text, IServiceProvider serviceProvider) : base(text)
        {
            systemInfo = serviceProvider.GetService<ISystemInfo>();
            DrawWidget();
        }

        void DrawWidget()
        {
            dataSource = new ProcessListDataSourceBuilder();
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
                "ThreadCount", 
                "HandleCount", 
                "Priority"
            };
        }

        public Terminal.Gui.Attribute HeaderStyle
        {
            get
            {
                return Terminal.Gui.Attribute.Make(Color.White, Color.DarkGray);
            }
        }

        public decimal[] GetColumnsWidth()
        {
            return  new decimal[] {
                0.25m, 
                0.15m, 
                0.15m, 
                0.15m, 
                0.15m,
                0.15m
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
                p.ThreadCount.ToString(),
                p.HandleCount.ToString(),
                p.PriorityBase.ToString()
            };
        }

        public string GetFooter()
        {
            return $"{TotalProcessesCount} running processes";
        }

        public Terminal.Gui.Attribute FooterStyle
        {
            get
            {
                return Terminal.Gui.Attribute.Make(Color.White, Color.Black);
            }
        }
    }
}