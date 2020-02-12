using System;
using System.Threading.Tasks;
using Terminal.Gui;
using Mono.Terminal;

namespace wttop.Widgets {

    /// <summary>
    /// Widget that will display the text on top of the app
    /// </summary>
    public class SystemTime : WidgetFrameless
    { 
        Label textLabel;

        string textTemplate = "Time: {0}";

        protected override int RefreshTimeSeconds
        {
            get
            {
                return 1;
            }
        }

        public SystemTime(IServiceProvider serviceProvider) : base(serviceProvider) {}

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
            var time = await systemInfo.GetSystemDateTime();
            textLabel.Text = string.Format(textTemplate, time.ToString());
        }
    }
}