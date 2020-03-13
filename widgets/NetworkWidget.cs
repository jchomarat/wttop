using System;
using System.Threading.Tasks;
using Terminal.Gui;
using Mono.Terminal;

namespace wttop.Widgets
{

    /// <summary>
    /// Widget that will display the network graph
    /// </summary>
    public class NetworkWidget : WidgetFrame
    {
        private Label _upl;
        private Label _dl;
        private long _valueUpl = 0;
        private long _valueDl = 0;

        /// <summary>
        /// Main constructor for the network widget
        /// </summary>
        /// <param name="serviceProvider">A service provider</param>
        public NetworkWidget(IServiceProvider serviceProvider) : base(serviceProvider) { }

        protected override void DrawWidget()
        {
            this.Title = settings.NetworkWidgetTitle;

            Label titleDl = new Label("Download (kB/sec): ")
            {
                X = 1,
                Y = 1
            };

            Add(titleDl);

            _dl = new Label(string.Empty)
            {
                X = Pos.Right(titleDl),
                Y = 1
            };

            _dl.TextColor = Terminal.Gui.Attribute.Make(settings.NetworkWidgetDownloadTextColor, settings.MainBackgroundColor);

            Add(_dl);

            Label titleUpl = new Label("Upload (kB/sec)  : ")
            {
                X = 1,
                Y = Pos.Bottom(titleDl)
            };

            Add(titleUpl);

            _upl = new Label(string.Empty)
            {
                X = Pos.Right(_dl),
                Y = Pos.Bottom(_dl)
            };

            _upl.TextColor = Terminal.Gui.Attribute.Make(settings.NetworkWidgetUploadTextColor, settings.MainBackgroundColor);

            Add(_upl);


        }
        protected override async Task Update(MainLoop MainLoop)
        {
            var network = await systemInfo.GetNetworkStatistics();
            if (_valueDl == 0)
            {
                // First round, don't do anything
                _valueDl = network.TotalBytesReceived;
                _valueUpl = network.TotalBytesSent;
            }
            else
            {
                var i = ((network.TotalBytesSent - _valueUpl) / 100);
                _upl.Text = $"{i}             ";
                _valueUpl = network.TotalBytesSent;

                var j = ((network.TotalBytesReceived - _valueDl) / 100);
                _dl.Text = $"{j}              ";
                _valueDl = network.TotalBytesReceived;
            }
        }
    }
}