using System;
using Microsoft.Extensions.DependencyInjection;
using Terminal.Gui;
using Mono.Terminal;
using wttop.Core;

namespace wttop.Widgets {

    /// <summary>
    /// Widget that will display the text on top of the app
    /// </summary>
    public class InfoText : WidgetFrameless
    { 
        Label textLabel;
        
        ISystemInfo systemInfo;

        Settings settings;

        string textTemplate = "{0} on {1} (version {2}) / up-time: {3}";

        protected override int RefreshTimeSeconds
        {
            get
            {
                return 60;
            }
        }

        public InfoText(IServiceProvider serviceProvider) : base()
        {
            systemInfo = serviceProvider.GetService<ISystemInfo>();
            settings = serviceProvider.GetService<Settings>();
            
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
        protected override void Update(MainLoop MainLoop)
        {
            var osInfo = systemInfo.GetOSInfo();
            textLabel.Text = string.Format(textTemplate, osInfo.MachineName, osInfo.OSName, osInfo.Version, osInfo.UpTimeForHuman);
        }
    }
}