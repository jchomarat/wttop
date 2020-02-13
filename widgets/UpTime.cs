using System;
using System.Threading.Tasks;
using Terminal.Gui;
using Mono.Terminal;

namespace wttop.Widgets {

    /// <summary>
    /// Widget that will display the text on top of the app
    /// </summary>
    public class Uptime : WidgetFrameless
    { 
        Label textLabel;

        string headerText = "Uptime: ";
        
        string contentTemplate = "{0}";

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
            var headerLabel = new Label(headerText)
            {
                X = 4,
                Y = 1
            };
            headerLabel.TextColor = Terminal.Gui.Attribute.Make(settings.LabelHeaderColor, settings.MainBackgroundColor);
            Add(headerLabel);

            textLabel = new Label(string.Empty)
            {
                X = Pos.Right(headerLabel),
                Y = 1
            };
           
            Add(textLabel);
        }

        protected override async Task Update(MainLoop MainLoop)
        {
            var upt = await systemInfo.GetSystemUpTime();
            textLabel.Text = string.Format(contentTemplate, upt.UpTimeForHuman);
        }
    }
}