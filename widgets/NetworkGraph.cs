using System;
using Microsoft.Extensions.DependencyInjection;
using Terminal.Gui;
using Mono.Terminal;
using wttop.Core;

namespace wttop.Widgets {
    
    // Widght that will display the network graph
    public class NetworkGraph : Widget
    { 
        Label upl;
        
        Label dl;
        
        ISystemInfo systemInfo;

        long valueUpl = 0;
        
        long valueDl = 0;

        public Color DownloadTextColor { get; set; } = Color.Green;

        public Color UploadTextColor { get; set; } = Color.Red;

        public NetworkGraph(string text, IServiceProvider serviceProvider) : base(text)
        {
            systemInfo = serviceProvider.GetService<ISystemInfo>();
        }

        public override void Init()
        {
            Label titleDl = new Label("Download (kB/sec): ")
            {
                X = 1,
                Y = 1
            };

            Add(titleDl);

            dl = new Label(string.Empty)
            {
                X = Pos.Right(titleDl),
                Y = 1
            };

            dl.TextColor = Terminal.Gui.Attribute.Make(DownloadTextColor, Settings.MainBackgroundColor);
           
            Add(dl);

            Label titleUpl = new Label("Upload (kB/sec): ")
            {
                X = 1,
                Y = Pos.Bottom(titleDl)
            };

            Add(titleUpl);

            upl = new Label(string.Empty)
            {
                X = Pos.Right(dl),
                Y = Pos.Bottom(dl)
            };

            upl.TextColor = Terminal.Gui.Attribute.Make(UploadTextColor, Settings.MainBackgroundColor);

            Add(upl);

            
        }
        public override bool Update(MainLoop MainLoop)
        {
            var network = systemInfo.GetNetworkStatistics();
            if (valueDl == 0)
            {
                // First round, don't do anything
                valueDl = network.TotalBytesReceived;
                valueUpl = network.TotalBytesSent;
            }
            else
            {
                var i = ((network.TotalBytesSent - valueUpl)/100);
                upl.Text = $"{i}             ";
                valueUpl = network.TotalBytesSent;

                var j = ((network.TotalBytesReceived - valueDl)/100);
                dl.Text = $"{j}              ";
                valueDl = network.TotalBytesReceived;
            }
            return true;
        }
    }
}