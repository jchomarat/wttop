using System;
using Microsoft.Extensions.DependencyInjection;
using Terminal.Gui;
using wttop.Widgets.Common;
using Mono.Terminal;
using wttop.Helpers;

namespace wttop.Widgets {
    
    public class DiskGraph : Widget
    { 
        Label write;
        
        Label read;
        
        ISystemInfo systemInfo;

        long valueWrite = 0;
        
        long valueRead = 0;

        public DiskGraph(string text, IServiceProvider serviceProvider) : base(text)
        {
            systemInfo = serviceProvider.GetService<ISystemInfo>();
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
            
            write.TextColor = Terminal.Gui.Attribute.Make(Color.Red, Color.Black);

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

            read.TextColor = Terminal.Gui.Attribute.Make(Color.Green, Color.Black);
           
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