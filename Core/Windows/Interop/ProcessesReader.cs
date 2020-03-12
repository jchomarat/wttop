using System;
using System.Collections.Generic;
using SD = System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;

namespace wttop.Core
{    
    /// <summary>
    /// Process reader class
    /// </summary>
    internal class ProcessesReader
    {
        List<ProcessWrapper> processWrapper;

        public ProcessWrapper[] ProcessWrapper
        {
            get { return processWrapper.ToArray(); }
        }

        public ProcessesReader()
        {
            FillProcessList();
        }

        void FillProcessList()
        {
            SD.Process[] processlist = SD.Process.GetProcesses();

            // First call
            processWrapper = processlist.Select(p =>
            {
                return new ProcessWrapper(p);
            }).ToList();
        }

        public ProcessWrapper[] Refresh()
        {
            SD.Process[] processlist = SD.Process.GetProcesses();

            // Remove dead processes
            var tmpList = processWrapper.Where(pd => processlist.Any(p => p.Id == pd.Pid)).ToList();

            // Add new ones
            var newProcess = processlist
                                .Where(p => !processWrapper.Any(pd => pd.Pid == p.Id))
                                .Select(p => { return new ProcessWrapper(p); }).ToList();

            tmpList.AddRange(newProcess);

            processWrapper = tmpList;

            // Refresh metric
            processWrapper.ToList().ForEach(p => p.Refresh());

            return processWrapper.ToArray();
        }
    }
}