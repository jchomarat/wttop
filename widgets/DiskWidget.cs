using System;
using System.Linq;
using System.Threading.Tasks;
using Terminal.Gui;
using Mono.Terminal;
using wttop.Widgets.Common;

namespace wttop.Widgets {
 
    /// <summary>
    /// Widget that will display the disk graph
    /// </summary>
    public class DiskWidget : WidgetFrame
    { 
        int cycle = 0;

        Label write;
        
        Label read;

        long valueWrite = 0;
        
        long valueRead = 0;

        View disksView;

        Label[] disksName = null;

        Bar[] disksUsage = null;

        public DiskWidget(IServiceProvider serviceProvider): base(serviceProvider) {}

        protected override void DrawWidget()
        {
            this.Title = settings.DiskWidgetTitle;
            
            // Disk activity
            Label titleWrite = new Label("Write (kB/sec): ")
            {
                X = 1,
                Y = 1
            };

            Add(titleWrite);

            write = new Label(string.Empty)
            {
                X = Pos.Right(titleWrite),
                Y = 1
            };
            
            write.TextColor = Terminal.Gui.Attribute.Make(settings.DiskWidgetWriteTextColor, settings.MainBackgroundColor);

            Add(write);            
            
            Label titleRead = new Label("Read (kB/sec) : ")
            {
                X = 1,
                Y = Pos.Bottom(titleWrite)
            };

            Add(titleRead);

            read = new Label(string.Empty)
            {
                X = Pos.Right(write),
                Y = Pos.Bottom(write)
            };

            read.TextColor = Terminal.Gui.Attribute.Make(settings.DiskWidgetReadTextColor, settings.MainBackgroundColor);
           
            Add(read);

            // Disk usage
            var diskCount = systemInfo.DiskCount;
            disksView = new View()
            {
                X = 1,
                Y = 4,
                Width = Dim.Fill(),
                Height= Dim.Fill()
            };

            Add(disksView);

            disksName = new Label[diskCount];
            disksUsage = new Bar[diskCount];
            int offset = 1;
            
            for (var i = 0; i < diskCount; i++)
            {    
                disksUsage[i] = 
                    new Bar(settings.DiskWidgetUsageBarColor, settings.MainBackgroundColor){
                        X = 1,
                        Y = offset,
                        Width = Dim.Percent(40),
                        Height= Dim.Sized(1)
                    };

                disksName[i] = new Label(string.Empty) {
                        X = Pos.Right(disksUsage[i]),
                        Y = offset
                    };

                offset++;
            }

            disksView.Add(disksName);
            disksView.Add(disksUsage);
        }
        
        protected override async Task Update(MainLoop MainLoop)
        {
            var disks = await systemInfo.GetDiskActivity();
            if (valueWrite == 0)
            {
                // First round, don't do anything, we need a first value for calculation
                valueWrite = disks.TotalBytesWritten;
                valueRead = disks.TotalBytesRead;
            }
            else
            {
                var i = ((disks.TotalBytesWritten - valueWrite)/100);
                write.Text = $"{i}              ";
                valueWrite = disks.TotalBytesWritten;

                var j = ((disks.TotalBytesRead - valueRead)/100);
                read.Text = $"{j}               ";
                valueRead = disks.TotalBytesRead;
            }

            // Disks usage refresh does not neet to be refreshed at the same pace, use cycle
            if (cycle == 0 || cycle == 59)
            {
                // First iteration, or one minute
                var storageInfo = await systemInfo.GetDiskStorageInfo();
                for (var i = 0; i < disksUsage.Length; i++)
                {
                    disksName[i].Text = string.Format("used on '{0} ({1})'", storageInfo.ElementAt(i).VolumeCaption, storageInfo.ElementAt(i).VolumeName);
                    disksUsage[i].Update(MainLoop, storageInfo.ElementAt(i).PercentageUsed);
                };

                if (cycle == 59)
                    cycle = 0;
                else
                    cycle ++;
            }
        }
    }
}