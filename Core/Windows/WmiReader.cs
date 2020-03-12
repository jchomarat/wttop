using System.Collections.Generic;
using System.Management;
using System.Threading.Tasks;

namespace wttop.Core {

    /// <summary>
    /// Wrapper for all WMI calls with async pattern
    /// </summary>
    public class WmiReader
    {
        /// <summary>
        /// Execute a scalar query and get back the management base objects
        /// </summary>
        /// <param name="QueryString">The query to execute</param>
        /// <returns>Management base object</returns>
        public Task<ManagementBaseObject> ExecuteScalar(string QueryString)
        {
            var tsc = new TaskCompletionSource<ManagementBaseObject>();
            using (var searcher = new ManagementObjectSearcher(QueryString))
            {
                ManagementOperationObserver results = new ManagementOperationObserver();

                results.ObjectReady += (s, e) => {
                    tsc.SetResult(e.NewObject);
                };

                searcher.Get(results);

                return tsc.Task;
            }
        }

        /// <summary>
        /// Execute a query and get back a list of management base objects
        /// </summary>
        /// <param name="QueryString">The querry to execute</param>
        /// <returns>Management base object</returns>
        public Task<List<ManagementBaseObject>> Execute(string QueryString)
        {
            var tsc = new TaskCompletionSource<List<ManagementBaseObject>>();
            var items = new List<ManagementBaseObject>();

            using (var searcher = new ManagementObjectSearcher(QueryString))
            {
                ManagementOperationObserver results = new ManagementOperationObserver();

                results.ObjectReady += (s, e) => {
                    items.Add(e.NewObject);
                };

                results.Completed += (s, e) => {
                    tsc.SetResult(items);
                };

                searcher.Get(results);

                return tsc.Task;
            }
        }
    }
}