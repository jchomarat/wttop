using System;
using Microsoft.Extensions.DependencyInjection;
using Terminal.Gui;
using Mono.Terminal;
using wttop.Core;

namespace wttop.Widgets {
    
    // Widght that will display the text on top of the app
    public class InfoText : WidgetFrameless
    { 
        Label textLabel;
        
        ISystemInfo systemInfo;

        string textTemplate = "{0} on {1} (version {2}) / up-time: {3}";

        public InfoText(IServiceProvider serviceProvider) : base()
        {
            systemInfo = serviceProvider.GetService<ISystemInfo>();
            DrawWidget();
        }

        void DrawWidget()
        {
            textLabel = new Label(string.Empty)
            {
                X = 4,
                Y = 1
            };
           
            Add(textLabel);
        }
        public override bool Update(MainLoop MainLoop)
        {
            var osInfo = systemInfo.GetOSInfo();
            textLabel.Text = string.Format(textTemplate, osInfo.MachineName, osInfo.OSName, osInfo.Version, osInfo.UpTimeForHuman);
            return true;
        }
    }
}