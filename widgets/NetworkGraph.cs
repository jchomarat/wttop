using System;
using System.Threading.Tasks;
using Terminal.Gui;
using Mono.Terminal;

namespace wttop.Widgets {

    /// <summary>
    /// Widget that will display the network graph
    /// </summary>
    public class NetworkGraph : WidgetFrame
    { 
        Label upl;
        
        Label dl;

        long valueUpl = 0;
        
        long valueDl = 0;

        public NetworkGraph(IServiceProvider serviceProvider) : base(serviceProvider, false) {}

        protected override void DrawWidget()
        {
            this.Title = settings.NetworkWidgetTitle;

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

            dl.TextColor = Terminal.Gui.Attribute.Make(settings.NetworkDownloadTextColor, settings.MainBackgroundColor);
           
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

            upl.TextColor = Terminal.Gui.Attribute.Make(settings.NetworkUploadTextColor, settings.MainBackgroundColor);

            Add(upl);

            
        }
        protected override async Task Update(MainLoop MainLoop)
        {
            var network = await systemInfo.GetNetworkStatistics();
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
        }
    }
}