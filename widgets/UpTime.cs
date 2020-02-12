using System;
using Microsoft.Extensions.DependencyInjection;
using Terminal.Gui;
using Mono.Terminal;
using wttop.Core;

namespace wttop.Widgets {

    /// <summary>
    /// Widget that will display the text on top of the app
    /// </summary>
    public class Uptime : WidgetFrameless
    { 
        Label textLabel;
        
        ISystemInfo systemInfo;

        Settings settings;

        string textTemplate = "Uptime: {0}";

        protected override int RefreshTimeSeconds
        {
            get
            {
                return 60;
            }
        }

        public Uptime(IServiceProvider serviceProvider) : base()
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
            var upt = systemInfo.GetSystemUpTime();
            textLabel.Text = string.Format(textTemplate, upt.UpTimeForHuman);
        }
    }
}