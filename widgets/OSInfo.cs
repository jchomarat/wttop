using System;
using System.Threading.Tasks;
using Terminal.Gui;
using Mono.Terminal;

namespace wttop.Widgets {

    /// <summary>
    /// Widget that will display the text on top of the app
    /// </summary>
    public class OSInfo : WidgetFrameless
    { 
        Label textLabel;

        string textTemplate = "Operating system: {0} on {1} (version {2})";

        protected override int RefreshTimeSeconds
        {
            get
            {
                return 60;
            }
        }

        public OSInfo(IServiceProvider serviceProvider) : base(serviceProvider) {}

        protected override void DrawWidget()
        {
            textLabel = new Label(string.Empty)
            {
                X = 4,
                Y = 1
            };
           
            Add(textLabel);
        }
        
        protected override async Task Update(MainLoop MainLoop)
        {
            var osInfo = await systemInfo.GetOSInfo();
            textLabel.Text = string.Format(textTemplate, osInfo.MachineName, osInfo.OSName, osInfo.Version);
        }
    }
}