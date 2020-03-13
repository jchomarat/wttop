using System;
using System.Collections;
using wttop.Core;
using wttop.Widgets.Common;

namespace wttop.Widgets
{
    /// <summary>
    /// In order to use the Grid widget, we need a GridDataSourceBuilder to get the source, the refresh and the style.
    /// </summary>
    public class ProcessListDataSourceBuilder: IGridDataSourceBuilder
    {
        /// <summary>
        /// Data source
        /// </summary>
        public IList DataSource { get; set; }

        /// <summary>
        /// Total process count
        /// </summary>
        public int TotalProcessesCount { get; set; }

        /// <summary>
        /// Get the process header
        /// </summary>
        /// <returns>Header string array</returns>
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

        /// <summary>
        /// Get or set the header style
        /// </summary>
        public Terminal.Gui.Attribute HeaderStyle { get; set; }

        /// <summary>
        /// Get the column width
        /// </summary>
        /// <returns>The column width array</returns>
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

        /// <summary>
        /// Get the row data
        /// </summary>
        /// <param name="index">the index of the row data</param>
        /// <returns>The details elements</returns>
        public string[] GetRowData(int index)
        {
            if (DataSource == null)
                throw new ArgumentException("You need to set the IList data source");

            ProcessInfo p = DataSource[index] as ProcessInfo;
            
            return new string[] {
                p.Name,
                p.IdProcess.ToString(),
                p.PercentProcessorTime.ToString(),
                p.MemoryUsageMb.ToString(),
                p.ThreadCount.ToString(),
                p.Owner
            };
        }

        /// <summary>
        /// Get the footer as a string, in this case the total num of processes running
        /// </summary>
        /// <returns>The footer as a string</returns>
        public string GetFooter()
        {
            return $"{TotalProcessesCount} running processes";
        }

        /// <summary>
        /// Get or set the foter style
        /// </summary>
        public Terminal.Gui.Attribute FooterStyle { get; set; }
    }
}