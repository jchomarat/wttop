using System;
using Microsoft.Extensions.DependencyInjection;
using Terminal.Gui;
using Mono.Terminal;
using wttop.Core;

namespace wttop.Widgets {
    
    // Widght that will display the disk graph
    public class DiskGraph : Widget
    { 
        Label write;
        
        Label read;
        
        ISystemInfo systemInfo;

        Settings settings;

        long valueWrite = 0;
        
        long valueRead = 0;

        public DiskGraph(string text, IServiceProvider serviceProvider) : base(text)
        {
            systemInfo = serviceProvider.GetService<ISystemInfo>();
            settings = serviceProvider.GetService<Settings>();

            DrawWidget();
        }

        void DrawWidget()
        {
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
            
            write.TextColor = Terminal.Gui.Attribute.Make(settings.DiskWriteTextColor, settings.MainBackgroundColor);

            Add(write);            
            
            Label titleRead = new Label("Read (kB/sec): ")
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

            read.TextColor = Terminal.Gui.Attribute.Make(settings.DiskReadTextColor, settings.MainBackgroundColor);
           
            Add(read);
        }
        
        public override bool Update(MainLoop MainLoop)
        {
            var disks = systemInfo.GetDiskActivity();
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
            return true;
        }
    }
}