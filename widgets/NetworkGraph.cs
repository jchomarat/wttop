using System;
using Microsoft.Extensions.DependencyInjection;
using Terminal.Gui;
using wttop.Widgets.Common;
using Mono.Terminal;
using wttop.Helpers;

namespace wttop.Widgets {
    
    public class NetworkGraph : Widget
    { 
        Label upl;
        Label dl;
        
        ISystemInfo systemInfo;

        int valueUpl = 0;
        
        int valueDl = 0;

        public NetworkGraph(string text, IServiceProvider serviceProvider) : base(text)
        {
            systemInfo = serviceProvider.GetService<ISystemInfo>();
            DrawWidget();
        }

        void DrawWidget()
        {
            Label titleDl = new Label("Download (B/sec): ")
            {
                X = 1,
                Y = 2
            };

            Add(titleDl);

            dl = new Label(string.Empty)
            {
                X = Pos.Right(titleDl),
                Y = 2
            };
           
            Add(dl);

            Label titleUpl = new Label("Upload (B/sec): ")
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
                upl.Text = (network.TotalBytesSent - valueUpl).ToString();
                valueUpl = network.TotalBytesSent;

                dl.Text = (network.TotalBytesReceived - valueDl).ToString();
                valueDl = network.TotalBytesReceived;
            }
            return true;
        }
    }
}