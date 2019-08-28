using System;
using Microsoft.Extensions.DependencyInjection;
using Terminal.Gui;
using wttop.Widgets.Common;
using Mono.Terminal;
using wttop.Helpers;

namespace wttop.Widgets {
    
    public class InfoText : WidgetFrameless
    { 
        Label textLabel;
        
        ISystemInfo systemInfo;

        string textTemplate = "{0} on {1} / up-time: {2}";

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
            textLabel.Text = string.Format(textTemplate, osInfo.MachineName, osInfo.OSName, osInfo.UpTimeForHuman);
            return true;
        }
    }
}