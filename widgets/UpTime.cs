using System;
using System.Threading.Tasks;
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

        string textTemplate = "Uptime: {0}";

        protected override int RefreshTimeSeconds
        {
            get
            {
                return 60;
            }
        }

        public Uptime(IServiceProvider serviceProvider) : base(serviceProvider) { }

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
            var upt = await systemInfo.GetSystemUpTime();
            textLabel.Text = string.Format(textTemplate, upt.UpTimeForHuman);
        }
    }
}