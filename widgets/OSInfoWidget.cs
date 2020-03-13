using System;
using System.Threading.Tasks;
using Terminal.Gui;
using Mono.Terminal;

namespace wttop.Widgets
{

    /// <summary>
    /// Widget that will display the text on top of the app
    /// </summary>
    public class OSInfoWidget : WidgetFrameless
    {
        private string headerText = "Operating system: ";
        private string contentTemplate = "{0} on {1} (version {2})";

        private Label _textLabel;

        protected override int RefreshTimeSeconds => 60;

        /// <summary>
        /// Main constructor to build the information widget
        /// </summary>
        /// <param name="serviceProvider">A service provider</param>
        public OSInfoWidget(IServiceProvider serviceProvider) : base(serviceProvider) { }

        protected override void DrawWidget()
        {
            var headerLabel = new Label(headerText)
            {
                X = 4,
                Y = 1
            };
            headerLabel.TextColor = Terminal.Gui.Attribute.Make(settings.LabelWidgetHeaderColor, settings.MainBackgroundColor);
            Add(headerLabel);

            _textLabel = new Label(string.Empty)
            {
                X = Pos.Right(headerLabel),
                Y = 1
            };

            Add(_textLabel);
        }

        protected override async Task Update(MainLoop MainLoop)
        {
            var osInfo = await systemInfo.GetOSInfo();
            _textLabel.Text = string.Format(contentTemplate, osInfo.MachineName, osInfo.OSName, osInfo.Version);
        }
    }
}