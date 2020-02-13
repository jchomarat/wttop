using System;
using System.Threading.Tasks;
using Terminal.Gui;
using Mono.Terminal;

namespace wttop.Widgets {
 
    /// <summary>
    /// Widget that will display the disk graph
    /// </summary>
    public class DiskWidget : WidgetFrame
    { 
        Label write;
        
        Label read;

        long valueWrite = 0;
        
        long valueRead = 0;

        public DiskWidget(IServiceProvider serviceProvider): base(serviceProvider) {}

        protected override void DrawWidget()
        {
            this.Title = settings.DiskWidgetTitle;
            
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
        }
        
        protected override async Task Update(MainLoop MainLoop)
        {
            var disks = await systemInfo.GetDiskActivity();
            if (valueWrite == 0)
            {
                // First round, don't do anything
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
        }
    }
}